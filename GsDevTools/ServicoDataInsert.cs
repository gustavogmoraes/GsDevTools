using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace GSDevTools
{
    public static class ServicoDataInsert
    {
        public static void Insert(frmDataInsertHelper dataHelperReference)
        {
            const string server = "localhost";
            const string databaseName = "CLI_LINX_Original";
            const string userName = "sa";
            const string password = "Fpwfpwfpw@";
            const string tableName = "CONT_ARQUIVOCONTABIL";

            const string filePath = @"C:\Users\gustavo.moraes\Desktop\LINX_NOVA_2\LINX.xlsx";

            using (var gsbd = new GSBancoDeDados())
            {
                gsbd.DefinaStringDeConexao(server, databaseName, userName, password);

                var columnsWithDataTypes = GetColumnsFromDatabaseTable(tableName);

                var dataTable = GetDataTableFromExcel(filePath, tableName, columnsWithDataTypes, dataHelperReference);

                dataHelperReference.Invoke((MethodInvoker)delegate { dataHelperReference.txtStatus.Text = "Inserting"; });

                gsbd.BulkCopy(dataTable);

                dataHelperReference.Invoke((MethodInvoker)delegate { dataHelperReference.txtStatus.Text = "Done"; });
            }
        }

        private static List<string> GetInsertsByValueList(List<Value> valueList, string tableName,
            List<ColumnWithDataType> columns, char fieldTerminator, char rowTerminator,
            frmDataInsertHelper helperReference)
        {
            helperReference.Invoke((MethodInvoker)delegate { helperReference.txtStatus.Text = "Mounting inserts"; });

            var grouping = valueList.GroupBy(x => x.RowNum).ToList();

            var inserts = new string[grouping.Count];

            Parallel.ForEach(grouping, row =>
            {
                var line = new StringBuilder();

                foreach (var column in columns)
                {
                    var value = row.FirstOrDefault(x => x.ColumnName == column.ColumnName);
                    if(value == null) throw new Exception("Error");

                    line.Append(GetValueToInsert(value.Val, column.DataType) + $"{fieldTerminator} ");
                }

                inserts[row.Key] = $"INSERT INTO {tableName} VALUES(" + line.Remove(line.Length - 2, 2) + ")" + rowTerminator;
            });

            var multipleLists = inserts.ToList().SplitList().ToList();

            return multipleLists.Select(x => string.Join(string.Empty, x)).ToList();
        }

        private static List<string> GetInsertsUsingSchema(string[,] rows, string tableName, List<ColumnWithDataType> columns, char fieldTerminator, char rowTerminator, frmDataInsertHelper helperReference)
        {
            var inserts = new List<string>();
            helperReference.Invoke((MethodInvoker) delegate { helperReference.txtStatus.Text = "Mounting inserts"; });

            Parallel.For(0, rows.GetUpperBound(0), a =>
            {
                var line = new StringBuilder();

                for (int j = 0; j < columns.Count; j++)
                {
                    line.Append(GetValueToInsert(rows[a, j], columns[j].DataType) + $"{fieldTerminator} ");
                }

                inserts.Add($"INSERT INTO {tableName} VALUES(" + line.Remove(line.Length - 2, 2) + ")" + rowTerminator);
            });

            var multipleLists = inserts.SplitList().ToList();

            return multipleLists.Select(x => string.Join(string.Empty, x)).ToList();
        }

        private static string GetValueToInsert(string value, Type type)
        {
            if (value == "NULL") return value;

            return type.IsNumericType() ? value : $"'{value}'";
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize = 1000)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        // Bulk insert for file
        //private static string[] GetInsertsUsingSchema(string[,] rows, List<ColumnWithDataType> columns, char fieldTerminator, char rowTerminator)
        //{
        //    var inserts = new List<string>();
        //    for (int a = 0; a < rows.GetUpperBound(0); a++)
        //    {
        //        var line = new StringBuilder();
        //        for (int j = 0; j < columns.Count; j++)
        //        {
        //            line.Append(rows[a, j] + $"{fieldTerminator} ");
        //        }

        //        inserts.Add(line.Remove(line.Length - 2, 2).ToString() + rowTerminator);
        //    }

        //    return inserts.ToArray();
        //}

        private static DataTable GetDataTableFromExcel(string path, string tableName, List<ColumnWithDataType> columnWithDataTypes, frmDataInsertHelper helperReference)
        {
            using (var pck = new ExcelPackage())
            {
                helperReference.Invoke((MethodInvoker)delegate { helperReference.txtStatus.Text = "Opening Excel file"; });
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets.First();

                helperReference.Invoke((MethodInvoker)delegate { helperReference.txtStatus.Text = "Getting data from excel"; });
                int height = ws.Dimension.Rows;
                int width = ws.Dimension.Columns;

                var columns = new List<string>();
                for (int i = 1; i < width + 1; i++)
                {
                    var val = ws.Cells[1, i].Value.ToString().Trim();
                    columns.Add(val);
                }

                var valList = new dynamic[(height - 1) * width];
                Parallel.For(1, height, i =>
                {
                    for (int j = 0; j < width; j++)
                    {
                        lock (ws)
                            valList[(i - 1) * width + j] = new Value
                            {
                                RowNum = i - 1,
                                ColumnName = columns[j],
                                Val = (ws.Cells[i + 1, j + 1].Value ?? string.Empty).ToString()
                            };
                    }
                });

                var grouping = valList.GroupBy(x => x.RowNum).ToList();

                var dataTable = new DataTable(tableName);
                columnWithDataTypes.ForEach(x => dataTable.Columns.Add(x.ColumnName, x.DataType));
                grouping.ForEach(row =>
                {
                    var valueToInsert = row.Select(column =>
                    {
                        var columInDb = columnWithDataTypes.FirstOrDefault(x => x.ColumnName == column.ColumnName);
                        return new
                        {
                            column.Val,
                            columInDb?.DataType,
                            ColumOrder = columInDb?.ColumnOrderOnDB
                        };
                    });

                    var itemArray = valueToInsert.OrderBy(x => x.ColumOrder).Select(x => ConvertValueAcordingToDataType(x.Val, x.DataType)).ToArray();
                    dataTable.Rows.Add(itemArray);
                });

                return dataTable;
            }
        }

        public class Value
        {
            public int RowNum { get; set; }
            public string ColumnName { get; set; }
            public  string Val { get; set; }
        }

        public static DataTable GetDataTableFromExcel(string path, List<ColumnWithDataType> columnsWithDataTypes, frmDataInsertHelper dataHelperReference)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets.First();

                var tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    var header = firstRowCell.Text.Trim();
                    var columnWithDataType = columnsWithDataTypes.Find(x => x.ColumnName.Trim() == header);
                    if (columnWithDataType == null) throw new Exception("Column not found on column with data type list");

                    tbl.Columns.Add(header, columnWithDataType.DataType);
                }

                ReadIntoTableWithParallel(tbl, ws, dataHelperReference);

                return tbl;
            }
        }

        private static List<ColumnWithDataType> GetColumnsFromDatabaseTable(string tableName)
        {
            using (var gsbd = new GSBancoDeDados())
            {
                var cols = gsbd.ExecuteConsulta(
                        $"SELECT COLUMN_NAME, DATA_TYPE " +
                        $"FROM INFORMATION_SCHEMA.COLUMNS " +
                        $"WHERE TABLE_NAME = '{tableName}' ")
                    .Rows.OfType<DataRow>().Select(row => new ColumnWithDataType
                    {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        DataType = ConvertaTipoDadosAplicacaoBanco(row["DATA_TYPE"].ToString().ToUpperInvariant())
                    }).ToList();

                for (int i = 0; i < cols.Count; i++)
                {
                    cols[i].ColumnOrderOnDB = i;
                }

                return cols;
            }
        }

        private static void ReadIntoTableWithParallel(DataTable dataTable, ExcelWorksheet sheet, frmDataInsertHelper dataHelperReference)
        {
            int height = sheet.Dimension.Rows;
            int width = sheet.Dimension.Columns;

            string[,] rows = new string[height, width];
            Parallel.For(1, height, (i) =>
            {
                for (int j = 0; j < width; j++)
                {
                    rows[i - 1, j] = (sheet.Cells[i + 1, j + 1].Value ?? "").ToString();
                }
            });

            for (int a = 0; a < height; a++)
            {
                dataHelperReference.Invoke((MethodInvoker) delegate
                {
                    dataHelperReference.txtStatus.Text = $"Importing row {a}";
                });
                
                var row = dataTable.Rows.Add();
                for (int j = 0; j < width; j++)
                {
                    row[j] = ConvertValueAcordingToDataType(rows[a, j], dataTable.Columns[j].DataType);
                }
            }
        }

        public static object ConvertValueAcordingToDataType(string text, Type datatype)
        {
            if (text == "NULL")
            {
                text = null;
            }

            if (datatype == typeof(string))
            {
                return string.IsNullOrEmpty(text) ? null : text;
            }

            if (datatype.IsNumericType())
            {
                return string.IsNullOrEmpty(text) ? 0 : Convert.ChangeType(text, datatype);
            }

            return null;
        }

        public static Dictionary<Type, string> DicionarioTipoDadosParaBancoDeDados = new Dictionary<Type, string>
        {
            {typeof(string), "VARCHAR"},
            {typeof(int), "NUMERIC"},
            {typeof(DateTime), "DATETIME2"},
            {typeof(decimal), "DECIMAL"},
            {typeof(Guid), "VARCHAR" },
            {typeof(float), "FLOAT" },
        };

        public static bool IsNumericType(this Type o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string ConvertaTipoDadosAplicacaoBanco(Type tipoDadoAplicacao)
        {
            return DicionarioTipoDadosParaBancoDeDados[tipoDadoAplicacao];
        }

        public static Type ConvertaTipoDadosAplicacaoBanco(string tipoDadoBanco)
        {
            if (tipoDadoBanco == "CHAR")
            {
                return typeof(string);
            }

            if (tipoDadoBanco == "INTEGER")
            {
                return typeof(int);
            }

            return DicionarioTipoDadosParaBancoDeDados.FirstOrDefault(x => x.Value == tipoDadoBanco).Key;
        }

        public class ColumnWithDataType
        {
            public string ColumnName { get; set; }

            public Type DataType { get; set; }

            public int ColumnOrderOnDB { get; set; }
        }

        public static void KeepTimeRunning(Stopwatch stopwatch, frmDataInsertHelper frmDataInsertHelper)
        {
            while (stopwatch.IsRunning)
            {
                frmDataInsertHelper.Invoke((MethodInvoker) delegate
                {
                    frmDataInsertHelper.txtStopwatch.Text = stopwatch.Elapsed.ToString("g").Substring(2, 5);
                });

                Thread.Sleep(250);
            }
        }
    }
}

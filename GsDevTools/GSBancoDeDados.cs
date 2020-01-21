using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSDevTools
{
    public class GSBancoDeDados : IDisposable
    {
        #region Propriedades

        private SqlConnection _conexao;

        private static string _stringDeConexao;

        #endregion


        #region Métodos

        /// <summary>
        /// Define a string de conexão com o banco de dados.
        /// </summary>
        /// <param name="urlDoServidor">A url do servidor, para local utilize '.', lembre-se de passar a string com o '@' antes para ignorar as '\'.</param>
        /// <param name="nomeDoBanco">O nome do banco.</param>
        /// <param name="login">O usuário do banco.</param>
        /// <param name="senha">A senha do banco.</param>
        public void DefinaStringDeConexao(string urlDoServidor, string nomeDoBanco, string login, string senha)
        {
            _stringDeConexao = String.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                                             urlDoServidor, nomeDoBanco, login, senha);
        }

        /// <summary>
        /// Define a string de conexão com o banco de dados para o computador do desenvolvedor
        /// </summary>
        public void DefinaStringDeConexao()
        {
            _stringDeConexao = @"Server = RAGNOS\SQLEXPRESS; Database = CopiaProducao; Trusted_Connection = Yes";
        }


        #region Métodos de query

        /// <summary>
        /// Executa diretamente a query SQL enviada.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        public void ExecuteComando(string comandoSQL)
        {
            try
            {
                _conexao = new SqlConnection(_stringDeConexao);
                _conexao.Open();

                //Cria e define a query
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = comandoSQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _conexao;

                while (_conexao.State != ConnectionState.Open)
                {
                    //Executa a query
                    cmd.ExecuteNonQuery();
                    _conexao.Close();
                }
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void MultipleInserts(List<string> inserts)
        {
            try
            {
                _conexao = new SqlConnection(_stringDeConexao);
                _conexao.Open();

                inserts.ForEach(x =>
                {
                    var command = new SqlCommand(x, _conexao)
                    {
                        CommandType = CommandType.Text
                    };

                    var rows = command.ExecuteNonQuery();
                });

                _conexao.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void BulkInsert(string filePath, char fieldTerminator, char rowTerminator, string tableName)
        {
            ExecuteComando($"BULK INSERT dbo.{tableName} " +
                           $"FROM '{filePath}' " +
                           $"WITH " +
                           $"( " +
                           $"     FIELDTERMINATOR = '{fieldTerminator}', " +
                           $"     ROWTERMINATOR = '{rowTerminator}' " +
                           $")");
        }

        public void BulkCopy(DataTable dataTable)
        {
            try
            {
                _conexao = new SqlConnection(_stringDeConexao);
                var bulkCopy = new SqlBulkCopy(_conexao, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null)
                {
                    DestinationTableName = dataTable.TableName
                };

                dataTable.Columns.Cast<DataColumn>().ToList().ForEach(x => bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(x.ColumnName, x.ColumnName)));

                _conexao.Open();

                bulkCopy.WriteToServer(dataTable.CreateDataReader());
                _conexao.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado retorno tupla única.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public dynamic ExecuteConsultaRetornoUnico(string comandoSQL, Type tipoRetorno)
        {
            try
            {
                string resultadoConsulta = string.Empty;

                _conexao = new SqlConnection(_stringDeConexao);
                _conexao.Open();

                //Cria e define a query
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = comandoSQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _conexao;

                //Executa a query
                resultadoConsulta = cmd.ExecuteScalar().ToString();

                if (String.IsNullOrEmpty(resultadoConsulta))
                    return null;

                _conexao.Close();

                if (tipoRetorno == typeof(string))
                {
                    return resultadoConsulta;
                }

                return tipoRetorno.GetMethod("Parse", new Type[] { typeof(string) })
                                  .Invoke(null, new object[] { resultadoConsulta });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado.
        /// </summary>
        /// <param name="comandoSql">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public DataTable ExecuteConsulta(string comandoSql)
        {
            try
            {
                _conexao = new SqlConnection(_stringDeConexao);
                _conexao.Open();

                //Cria e define a query
                var cmd = new SqlCommand
                {
                    CommandText = comandoSql,
                    CommandType = CommandType.Text,
                    Connection = _conexao
                };

                //Executa a query
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dt.Columns.Add(reader.GetName(i));
                }

                while (reader.Read())
                {
                    var values = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader[i]);
                    }

                    dt.Rows.Add(values.ToArray());
                }

                _conexao.Close();

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        //private void ConvertDataReaderToTableManually()
        //{
        //    SqlConnection conn = null;
        //    try
        //    {
        //        string connString = ConfigurationManager.ConnectionStrings["NorthwindConn"].ConnectionString;
        //        conn = new SqlConnection(connString);
        //        string query = "SELECT * FROM Customers";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //        DataTable dtSchema = dr.GetSchemaTable();
        //        DataTable dt = new DataTable();
        //        // You can also use an ArrayList instead of List<>
        //        List<DataColumn> listCols = new List<DataColumn>();

        //        if (dtSchema != null)
        //        {
        //            foreach (DataRow drow in dtSchema.Rows)
        //            {
        //                string columnName = System.Convert.ToString(drow["ColumnName"]);
        //                DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
        //                column.Unique = (bool)drow["IsUnique"];
        //                column.AllowDBNull = (bool)drow["AllowDBNull"];
        //                column.AutoIncrement = (bool)drow["IsAutoIncrement"];
        //                listCols.Add(column);
        //                dt.Columns.Add(column);
        //            }
        //        }

        //        // Read rows from DataReader and populate the DataTable
        //        while (dr.Read())
        //        {
        //            DataRow dataRow = dt.NewRow();
        //            for (int i = 0; i < listCols.Count; i++)
        //            {
        //                dataRow[((DataColumn)listCols[i])] = dr[i];
        //            }
        //            dt.Rows.Add(dataRow);
        //        }
        //        GridView2.DataSource = dt;
        //        GridView2.DataBind();
        //    }
        //    catch (SqlException ex)
        //    {
        //        // handle error
        //    }
        //    catch (Exception ex)
        //    {
        //        // handle error
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //}

        #endregion


        #region Métodos auxiliares

        public bool VerifiqueStatusDaConexao()
        {
            try
            {
                //Cria e define a query
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT 1";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = _conexao;

                //Executa a query
                var retorno = cmd.ExecuteScalar();

                if ((int)retorno == 1)
                    return true;
                else
                    return false;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Obtém o próximo código inteiro que estiver disponível na tabela desejada.
        /// </summary>
        /// <param name="tabela">A tabela desejada</param>
        /// <param name="colunaChave">A coluna chave dessa tabela</param>
        /// <returns>
        /// Inteiro com o próximo código disponível
        /// </returns>
        public int ObtenhaProximoCodigoDisponivel(string tabela, string colunaChave)
        {
            string comandoSQL = String.Format("SELECT TOP 1 {0} + 1 FROM {1} MO WHERE NOT EXISTS" +
                                              "(SELECT NULL FROM {1} MI WHERE MI.{0} = MO.{0} + 1) ORDER BY {0}",
                                              colunaChave,
                                              tabela);

            var retorno = ExecuteConsultaRetornoUnico(comandoSQL, typeof(int));

            if (retorno == null)
                return 1;

            return retorno;
        }

        public void Dispose()
        {

        }

        #endregion

        #endregion

        #region Dispose 
        #endregion

    }
}

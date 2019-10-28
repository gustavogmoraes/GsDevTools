using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteDB;

namespace GSDevTools
{
    public static class Persistencia
    {

        public const string NomeCollectionItensClipboard = "ItensClipboard";
        public const string NomeCollectionConfiguracao = "Configuracao";

        public static LiteDatabase AbraConexao()
        {
            var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dados");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return new LiteDatabase(Path.Combine(directoryPath, "@Dados.db"));
        }

        public static LiteCollection<ClipboardItem> ObtenhaCollectionClipboardItem(this LiteDatabase dbConnection)
        {
            return dbConnection.GetCollection<ClipboardItem>(NomeCollectionItensClipboard);
        }

        public static Configuracao ObtenhaConfiguracao()
        {
            using (var pers = AbraConexao())
            {
                return pers.GetCollection<Configuracao>(NomeCollectionConfiguracao).FindAll().FirstOrDefault() ?? new Configuracao();
            }
        }

        public static void AltereConfiguracao(Configuracao config)
        {
            using (var pers = AbraConexao())
            {
                pers.GetCollection<Configuracao>(NomeCollectionConfiguracao).Upsert(config);
            }
        }


        public static frmClipboardTool FormClipboardTool { get; set; }

        public static void ChangedClipboardCollection(ClipboardItem item = null, bool remove = false)
        {
            if (item != null)
            {
                FormClipboardTool?.RefreshSingleItem(item, remove);
                return;
            }

            FormClipboardTool?.RefreshList();
        }

        public static string SaveImageAndGetPath(Guid itemId, Image image)
        {
            var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dados", "Images");
            var newPath = Path.Combine(directoryPath, itemId + ".jpeg");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            image.Save(newPath, ImageFormat.Jpeg);

            return newPath;
        }
    }
}

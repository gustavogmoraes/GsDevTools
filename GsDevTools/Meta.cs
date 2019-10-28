using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSDevTools
{
    public static class Meta
    {
        #region Gerar direct links OneDrive

        // Como gerar os direct links de download
        // Se tentar só compartilhar e "acessar"/baixar o item, acaba pegando o código fonte do DriveWebView
        // https://solvemethod.com/onedrive-direct-link/

        #endregion

        public static string Versao => "1.9";
        public static string ConexaoUrl => @"https://1drv.ms/t/s!AgTuMixP3feMrmQKIzFvHubSiYuE";

        public static int ConexaoTimeout => 5000;
        public static int ConexaoRetries => 3;

        public static string VersionamentoUrl => @"https://onedrive.live.com/download?cid=8CF7DD4F2C32EE04&resid=8CF7DD4F2C32EE04%216013&authkey=AOCNgO0zTlnJtU8";
        public static int VersionamentoTimeout => 8000;

        public static int AtualizacaoTimeout => 15000;
        public static int AtualizacaoRetries => 3;

        public static string AppUpdaterDiretorio => "AppUpdater";
        public static string AppUpdaterExecutavel => "AppUpdater.exe";

        public static string GSDevToolsExecutavel => "GSDevTools.exe";
    }
}

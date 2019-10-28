using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.SharpClipboardNS;

namespace GSDevTools
{
    public static class ServicoClipboard
    {
        public static bool Habilitado
        {
            get => Persistencia.ObtenhaConfiguracao().ClipboardHabilitada;
            set
            {
                var cfg = Persistencia.ObtenhaConfiguracao();
                cfg.ClipboardHabilitada = value;

                Persistencia.AltereConfiguracao(cfg);

                if (cfg.ClipboardHabilitada)
                {
                    Ligue();
                    //ClipBoard.StartMonitoring();
                }
                else
                {
                    Desligue();
                }
            }
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        private const int WM_DRAWCLIPBOARD = 0x0308; // WM_DRAWCLIPBOARD message
        private static IntPtr _clipboardViewerNext;  // Our variable that will hold the value to identify the next window in the clipboard viewer chain

        public static SharpClipboard ClipBoard = new SharpClipboard();

        private static void Ligue()
        {
            // Attach your code to the ClipboardChanged event to listen to cuts/copies.
            ClipBoard.ClipboardChanged += ClipboardChanged;
            FirstCall = true;
        }

        private static void Desligue()
        {
            ClipBoard.ClipboardChanged -= ClipboardChanged;
        }

        private static bool FirstCall { get; set; }

        private static object PreviousContent { get; set; }

        private static SharpClipboard.ContentTypes PreviousContentType { get; set; }

        private static void ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            if (e.ContentType == SharpClipboard.ContentTypes.Text &&
                Persistencia.ObtenhaConfiguracao().GlobalizacaoHabilitada)
            {
                ServicoGlobalizacao.TryExposeGlobalization(ClipBoard.ClipboardText);
            }

            if (!Habilitado)
            {
                return;
            }

            if (FirstCall)
            {
                FirstCall = false;
                return;
            }

            if (CheckIfContentIsTheSame(e.ContentType)) return;

            using (var persistencia = Persistencia.AbraConexao())
            {
                var item = new ClipboardItem(e.ContentType, ClipBoard);
                persistencia.ObtenhaCollectionClipboardItem().Insert(item);

                Persistencia.ChangedClipboardCollection(item);
            }

            PreviousContent = ExtractContent(e.ContentType);
            PreviousContentType = e.ContentType;
        }

        private static object ExtractContent(SharpClipboard.ContentTypes contentType)
        {
            switch (contentType)
            {
                case SharpClipboard.ContentTypes.Text:
                    return ClipBoard.ClipboardText;
                case SharpClipboard.ContentTypes.Image:
                    return ClipBoard.ClipboardImage;
                case SharpClipboard.ContentTypes.Files:
                    return ClipBoard.ClipboardFiles;
                case SharpClipboard.ContentTypes.Other:
                    return ClipBoard.ClipboardObject;
            }

            return null;
        }

        private static bool CheckIfContentIsTheSame(SharpClipboard.ContentTypes contentType)
        {
            if (PreviousContent == null || PreviousContentType != contentType)
            {
                return false;
            }

            switch (contentType)
            {
                case SharpClipboard.ContentTypes.Text:
                    return ((string) PreviousContent).Equals(ClipBoard.ClipboardText);
                case SharpClipboard.ContentTypes.Image:
                    return ((Image) PreviousContent).Equals(ClipBoard.ClipboardImage);
                case SharpClipboard.ContentTypes.Files:
                    return ((List<string>) PreviousContent).SequenceEqual(ClipBoard.ClipboardFiles);
                case SharpClipboard.ContentTypes.Other:
                    return PreviousContent.Equals(ClipBoard.ClipboardObject);
            }

            return false;
        }
    }
}

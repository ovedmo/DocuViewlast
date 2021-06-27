using GdPicture14.WEB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocuViewareBlazor.Server.EventDispatcher
{
    public static class DocuViewareEventDispatcher
    {
        public static void PageTransferReady(object sender, PageTransferReadyEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        public static void NewDocumentLoaded(object sender, NewDocumentLoadedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        public static void LoadDocumentError(object sender, LoadDocumentErrorEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        public static void CustomAction(object sender, CustomActionEventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}

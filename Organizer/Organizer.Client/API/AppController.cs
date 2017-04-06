using CefSharp;
using CefSharp.WinForms;
using Model.DataProviders;
using Newtonsoft.Json;
using Organizer.Client.API;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using Organizer.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Client
{
    public class AppController
    {
        private static ChromiumWebBrowser _instanceBrowser = null;
        private static MainWindow _instanceMainForm = null;

        public AppController(ChromiumWebBrowser originalBrowser, MainWindow mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }

        public void showDevTools()
        {
            _instanceBrowser.ShowDevTools();
        }

        public void opencmd()
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", "/c pause");
            Process.Start(start);
        }
    }
}

﻿using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Organizer.Client.API;
using Model.Setup;
using Organizer.Model;

namespace Organizer.Client
{
    public partial class MainWindow : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public MainWindow()
        {
            var dbContext = new DataContext();
            InitializeComponent();
            InitializeChromium();
            chromeBrowser.RegisterJsObject("appCtrl", new AppController(chromeBrowser, this));
            chromeBrowser.RegisterJsObject("goalsCtrl", new GoalsController(chromeBrowser, this, dbContext));
            chromeBrowser.RegisterJsObject("todosCtrl", new TodosController(chromeBrowser, this, dbContext));
            chromeBrowser.RegisterJsObject("reportsCtrl", new ReportsController(chromeBrowser, this, dbContext));
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Dock = DockStyle.Fill;

            int h = Screen.PrimaryScreen.WorkingArea.Height;
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            this.ClientSize = new Size(w, h);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        public void InitializeChromium(bool index = true)
        {
            CefSettings settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            // Note that if you get an error or a white screen, you may be doing something wrong !
            // Try to load a local file that you're sure that exists and give the complete path instead to test
            // for example, replace page with a direct path instead :
            // String page = @"C:\Users\SDkCarlos\Desktop\afolder\index.html";

            string page = string.Format(@"{0}\views\index.html", Application.StartupPath);
            if (!File.Exists(page))
            {
                MessageBox.Show("Error The html file doesn't exists : " + page);
            }

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(page);

            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            chromeBrowser.BrowserSettings = browserSettings;
        }
    }
}

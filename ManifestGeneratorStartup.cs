﻿using MobileDeliveryLogger;
using System;
using System.Configuration;
using System.Windows.Forms;
using MobileDeliveryGeneral.Settings;
using MobileDeliverySettings.Settings;

namespace ManifestGenerator
{
    static class ManifestGenerator
    {
        /// <summary>
        /// Winform app to generate Manifests.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var config = WinformReadSettings.GetSettings(typeof(ManifestGenerator));

            Logger logger = new Logger(config.AppName, config.LogPath, config.LogLevel);
            Logger.Level = config.LogLevel;

            Logger.Info($"Starting {config.AppName} {config.Version} {DateTime.Now}");
            Logger.Info($"Logfile path: {config.LogPath} ");

            Application.Run(new frmManifestGenerator(config, logger)); 
        }
    }
}

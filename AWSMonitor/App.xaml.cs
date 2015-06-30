using System;
using System.Configuration;
using System.Windows;
using AwsGateway.Settings;

namespace AWSMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AwsKeyProviders.Key = ConfigurationManager.AppSettings.Get("Key");
            AwsKeyProviders.Secret = ConfigurationManager.AppSettings.Get("Secret");

            if (string.IsNullOrWhiteSpace(AwsKeyProviders.Key) || string.IsNullOrWhiteSpace(AwsKeyProviders.Key))
            {
                MessageBox.Show(
                    "Please set both: AWS Key and Secret in AWSMonitor.exe.config\n" +
                    "Application won't work without those settings.\n" + 
                    "Exiting application now.",
                    "Configuration error", MessageBoxButton.OK, MessageBoxImage.Error);

                Shutdown(1);
                return;
            }
            else
            {
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
        }
    }
}

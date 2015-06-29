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
            AwsKeyProviders.Key = ConfigurationManager.AppSettings.Get("Key");
            AwsKeyProviders.Secret = ConfigurationManager.AppSettings.Get("Secret");
        }
    }
}

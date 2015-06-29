using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AwsGateway.Data;
using AwsGateway.DataProviders;

namespace AWSMonitor
{
    /// <summary>
    /// Interaction logic for Ec2Details.xaml
    /// </summary>
    public partial class Ec2Details : Window
    {
        public Ec2Record CurrentEc2Record { get; private set; }

        public Ec2Details()
        {
            InitializeComponent();
        }

        public void ShowEc2Records(Ec2Record ec2Record)
        {
            this.Show();

            this.CurrentEc2Record = ec2Record;
            this.DataContext = ec2Record;
        }

        private void groupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listSender = (ListBox) sender;

            if ((listSender == null) || (listSender.SelectedItems.Count <= 0)) return;

            var selectedItem = (string) listSender.SelectedItems[0];
            incommingListView.ItemsSource = SecurityGroupsProvider.GetSecurityGroupByName(selectedItem).IncommingAgregated;
            outgoingListView.ItemsSource = SecurityGroupsProvider.GetSecurityGroupByName(selectedItem).OutgoingAgregated;
        }
    }
}

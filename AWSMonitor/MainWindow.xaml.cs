using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AwsGateway.AwsTools;
using AwsGateway.Data;
using AwsGateway.DataProviders;

namespace AWSMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Ec2Record> m_FullList = new List<Ec2Record>();

        private ObservableCollection<Ec2Record> Ec2Data
        {
            get 
            { 
                var result = new ObservableCollection<Ec2Record>();

                m_FullList.ForEach(
                    ec2record =>
                        {
                            if (string.IsNullOrWhiteSpace(filterText.Text) || ec2record.Name.ToUpperInvariant().Contains(filterText.Text.ToUpperInvariant()))
                            {
                                result.Add(ec2record);
                            }
                        });

                return result;
            }
        }

        private List<VolumeRecord> m_FullListOfVolumes = new List<VolumeRecord>();

        private ObservableCollection<VolumeRecord> VolumeData
        {
            get
            {
                return new ObservableCollection<VolumeRecord>(m_FullListOfVolumes);
            }
        }
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // load information about EC2 instances
            m_FullList = Ec2InstanceProvider.GetAllEc2Instances(force: true) as List<Ec2Record>;

            ec2InstanceList.ItemsSource = Ec2Data;
            ec2Tab.Header = string.Format("EC2 instances ({0})", Ec2Data.Count);

            m_FullListOfVolumes = VolumeProvider.GetAllVolumes(force: true) as List<VolumeRecord>;

            volumesList.ItemsSource = VolumeData;
            volumesTab.Header = string.Format("Volumes ({0})", VolumeData.Count);

            var securityGroups = SecurityGroupsProvider.GetAllSecurityGroups(force: true);

            secrityGroupList.ItemsSource = securityGroups;
            securityGroupsTab.Header = string.Format("Security groups ({0})", securityGroups.Count);

            var rdsDatabases = RdsInstanceProvider.GetAllRdsInstances(force: true);

            rdsList.ItemsSource = rdsDatabases;
            rdsDatabasesTab.Header = string.Format("RDS DBs ({0})", rdsDatabases.Count);

            var elasticBeanstalkApplications = ElasticBeanStalkInstanceProvider.GetAllElasticBeanStalkInstances(force: true);

            elasticBeanstalkList.ItemsSource = elasticBeanstalkApplications;
            elasticBeanstalkTab.Header = String.Format("Elastic Beanstalk Apps ({0})", elasticBeanstalkApplications.Count);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction, (ListView) e.Source);

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction, ListView sourceListView)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(sourceListView.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void ec2InstanceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sourceEc2Record = ((ListView)e.Source).SelectedItem;

            if (sourceEc2Record != null)
            {
                var ec2DetailsWnd = new Ec2Details();
                ec2DetailsWnd.ShowEc2Records((Ec2Record)sourceEc2Record);
            }
        }

        private void MenuItem_RDP_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem =  (Ec2Record) ec2InstanceList.SelectedItem;

            if (selectedItem != null)
            {
                Process.Start("mstsc.exe", string.Format("/v:{0}", selectedItem.PrivateIp));
            }
        }

        private void filterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ec2InstanceList.ItemsSource = Ec2Data;
        }

        private void MenuItem_CopyHostName_Click(object sender, RoutedEventArgs e)
        {
            if (ec2InstanceList.SelectedItem != null)
            {
                Clipboard.SetData(DataFormats.Text, ((Ec2Record)ec2InstanceList.SelectedItem).Name);
            }
        }

        private void MenuItem_CopyIp_Click(object sender, RoutedEventArgs e)
        {
            if (ec2InstanceList.SelectedItem != null)
            {
                Clipboard.SetData(DataFormats.Text, ((Ec2Record)ec2InstanceList.SelectedItem).PrivateIp);
            }
        }

        private void MenuItem_CopyAllHostNames_Click(object sender, RoutedEventArgs e)
        {
            var actualData = Ec2Data;

            if (actualData.Any())
            {
                Clipboard.SetData(DataFormats.Text, actualData.Select(ec2record => ec2record.Name).Aggregate((data, newEntry) => data + ", " + newEntry));
            }
        }

        private void MenuItem_CopyAllIp_Click(object sender, RoutedEventArgs e)
        {
            var actualData = Ec2Data;

            if (actualData.Any())
            {
                Clipboard.SetData(DataFormats.Text, actualData.Select(ec2record => ec2record.PrivateIp).Aggregate((data, newEntry) => data + ", " + newEntry));
            }
        }

        private void MenuItem_CopyVolumeId_Click(object sender, RoutedEventArgs e)
        {
            if (volumesList.SelectedItem != null)
            {
                Clipboard.SetData(DataFormats.Text, ((VolumeRecord)volumesList.SelectedItem).Id);
            }
        }


        private void MenuItem_CopyAllVolumeIds_Click(object sender, RoutedEventArgs e)
        {
            var actualData = VolumeData;

            if (actualData.Any())
            {
                Clipboard.SetData(DataFormats.Text, actualData.Select(volumeRecord => volumeRecord.Id).Aggregate((data, newEntry) => data + ", " + newEntry));
            }
        }

        private void MenuItem_StopInstance(object sender, RoutedEventArgs e)
        {
            var actualData = Ec2Data;

            if (actualData.Any())
            {
                var listOfInstances = new List<string>() { ((Ec2Record)ec2InstanceList.SelectedItem).InstanceId };

                Ec2InstanceManager.StopInstance(listOfInstances);
            }
        }

        private void MenuItem_StartInstance(object sender, RoutedEventArgs e)
        {
            var actualData = Ec2Data;

            if (actualData.Any())
            {
                var listOfInstances = new List<string>() { ((Ec2Record)ec2InstanceList.SelectedItem).InstanceId };

                Ec2InstanceManager.StartInstance(listOfInstances);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            string buttonTag = (senderButton.Tag ?? string.Empty).ToString();

            if (!string.IsNullOrWhiteSpace(buttonTag))
            {
                var listOfInstances = new List<string>() { buttonTag };

                if (senderButton.Content.ToString() == "Start")
                {
                    Ec2InstanceManager.StartInstance(listOfInstances);
                }

                if (senderButton.Content.ToString() == "Stop")
                {
                    Ec2InstanceManager.StopInstance(listOfInstances);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                RefreshButton_Click(sender, null);
            }
        }

        private void Button_RDP_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            string privateIp = (senderButton.Tag ?? string.Empty).ToString();

            if (!string.IsNullOrWhiteSpace(privateIp))
            {
                Process.Start("mstsc.exe", string.Format("/v:{0}", privateIp));
            }
        }

        private void Button_URL_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            string urlToHit = (senderButton.Tag ?? string.Empty).ToString();

            if (!string.IsNullOrWhiteSpace(urlToHit))
            {
                Process.Start("http://" + urlToHit);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.ElasticBeanstalk;
using Amazon.ElasticBeanstalk.Model;
using AwsGateway.Data;

namespace AwsGateway.DataProviders
{
    public class ElasticBeanStalkInstanceProvider
    {
        private static IList<ElasticBeanStalkRecord> m_ElastickBeanstalkRecords;

        public static IList<ElasticBeanStalkRecord> GetAllElasticBeanStalkInstances(bool force = false)
        {
            if (m_ElastickBeanstalkRecords == null || force)
            {
                var elasticBeanstalk = ElasticBeanStalkConnectionProvider.GetElasticBeanstalkConnection();

                DescribeApplicationsRequest request = new DescribeApplicationsRequest();
                DescribeApplicationsResponse res = elasticBeanstalk.DescribeApplications();

                m_ElastickBeanstalkRecords = new List<ElasticBeanStalkRecord>();

                res.DescribeApplicationsResult.Applications.ForEach(
                        application => 
                            {
                                var elasticBeanStalkRecord = new ElasticBeanStalkRecord()
                                {
                                    ApplicationName = application.ApplicationName,
                                    DateCreated = application.DateCreated,
                                    Description = application.Description,
                                    DateUpdated = application.DateUpdated
                                };

                                elasticBeanStalkRecord.ConfigurationTemplates = new List<string>(application.ConfigurationTemplates);
                                elasticBeanStalkRecord.Versions = new List<string>(application.Versions);

                                m_ElastickBeanstalkRecords.Add(elasticBeanStalkRecord);
                            }
                    );
            }

            return m_ElastickBeanstalkRecords;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.ElasticBeanstalk;
using AwsGateway.Settings;

namespace AwsGateway.DataProviders
{
    public class ElasticBeanStalkConnectionProvider
    {
        private static AmazonElasticBeanstalkClient elasticBeanstalk;

        public static AmazonElasticBeanstalkClient GetElasticBeanstalkConnection()
        {
            if (elasticBeanstalk == null)
            {
                elasticBeanstalk = new AmazonElasticBeanstalkClient(AwsKeyProviders.Key, AwsKeyProviders.Secret, RegionEndpoint.EUWest1);
            }

            return elasticBeanstalk;
        }

    }
}

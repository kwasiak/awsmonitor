using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.RDS;
using AwsGateway.Settings;

namespace AwsGateway.DataProviders
{
    public static class RdsConnectionProvider
    {
        private static AmazonRDSClient rds;

        public static AmazonRDSClient GetRdsConnection()
        {
            if (rds == null)
            {
                rds = new AmazonRDSClient(AwsKeyProviders.Key, AwsKeyProviders.Secret, RegionEndpoint.EUWest1);
            }

            return rds;
        }
    }
}

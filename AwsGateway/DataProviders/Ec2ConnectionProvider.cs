using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.EC2;
using AwsGateway.Settings;

namespace AwsGateway.DataProviders
{
    public static class Ec2ConnectionProvider
    {
        private static AmazonEC2Client ec2;

        public static AmazonEC2Client GetEc2Connection()
        {
            if (ec2 == null)
            {
                ec2 = new AmazonEC2Client(AwsKeyProviders.Key, AwsKeyProviders.Secret, RegionEndpoint.EUWest1);
            }

            return ec2;
        }
    }
}

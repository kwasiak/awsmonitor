using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.CloudWatch;
using AwsGateway.Settings;

namespace AwsGateway.DataProviders
{
    public class CloudWatchConnectionProvider
    {
        private static IAmazonCloudWatch cloudWatch;

        public static IAmazonCloudWatch GetCloudWatchConnection()
        {
            if (cloudWatch == null)
            {
                cloudWatch = AWSClientFactory.CreateAmazonCloudWatchClient(AwsKeyProviders.Key, AwsKeyProviders.Secret, RegionEndpoint.EUWest1);
            }

            return cloudWatch;
;
        }

    }
}

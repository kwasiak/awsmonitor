using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.EC2.Model;

namespace AwsGateway.Tools
{
    public static class AwsTags
    {
        public static string GetValueFromEc2Tag(ICollection<Tag> ec2Tags, string key)
        {
            var result = "-";

            if (ec2Tags != null)
            {
                foreach (var ec2Tag in ec2Tags)
                {
                    if (ec2Tag.Key.ToUpperInvariant() == key.ToUpperInvariant())
                    {
                        result = ec2Tag.Value;
                        break;
                    }
                }
            }

            return result;
        }
    }
}

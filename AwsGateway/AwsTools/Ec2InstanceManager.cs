using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AwsGateway.DataProviders;

using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.OpsWorks.Model;

namespace AwsGateway.AwsTools
{
    public static class Ec2InstanceManager
    {
        public static void StartInstance(List<string> instancesId)
        {
            var ec2 = Ec2ConnectionProvider.GetEc2Connection();

            StartInstancesRequest startInstanceRequest = new StartInstancesRequest()
                {
                    InstanceIds = instancesId
                };

            StartInstancesResponse startResult = ec2.StartInstances(startInstanceRequest);
        }

        public static void StopInstance(List<string> instancesId)
        {
            var ec2 = Ec2ConnectionProvider.GetEc2Connection();

            StopInstancesRequest stopInstanceRequest = new StopInstancesRequest()
            {
                InstanceIds = instancesId
            };

            StopInstancesResponse stopResult = ec2.StopInstances(stopInstanceRequest);
        }

    }
}

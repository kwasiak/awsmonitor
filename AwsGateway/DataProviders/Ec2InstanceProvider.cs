using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using AwsGateway.Data;
using AwsGateway.Tools;

namespace AwsGateway.DataProviders
{
    public static class Ec2InstanceProvider
    {
        private static IList<Ec2Record> m_Ec2Records;

        public static IList<Ec2Record> GetAllEc2Instances(bool force = false)
        {
            if (m_Ec2Records == null || force)
            {
                var ec2 = Ec2ConnectionProvider.GetEc2Connection();

                DescribeInstancesRequest request = new DescribeInstancesRequest();
                DescribeInstancesResponse res = ec2.DescribeInstances(request);

                m_Ec2Records = new List<Ec2Record>();

                res.DescribeInstancesResult.Reservations.ForEach(
                    reservation => reservation.Instances.ForEach(
                        runningInstance => m_Ec2Records.Add(
                            new Ec2Record()
                                {
                                    InstanceId = runningInstance.InstanceId,
                                    Status = runningInstance.State.Name,
                                    Type = runningInstance.InstanceType,
                                    Platform = string.IsNullOrWhiteSpace(runningInstance.Platform) ? "Linux" : "Windows",
                                    Ip = string.IsNullOrEmpty(runningInstance.PublicIpAddress) ? "-" : runningInstance.PublicIpAddress,
                                    PrivateIp = runningInstance.PrivateIpAddress,
                                    PublicDns = runningInstance.PublicDnsName,
                                    PrivateDns = string.IsNullOrEmpty(runningInstance.PrivateDnsName) ? "-" : runningInstance.PrivateDnsName,
                                    Name = AwsTags.GetValueFromEc2Tag(runningInstance.Tags, "Name"),
                                    AttachedVolumes = VolumeProvider.GetVolumesByEc2Id(runningInstance.InstanceId),
                                    LaunchTime = runningInstance.LaunchTime,
                                    SecurityGroups = runningInstance
                                                        .SecurityGroups
                                                        .Select(group => group.GroupName)
                                                        .ToList()
                                }
                                               )
                                       )
                    );
            }

            return m_Ec2Records;
        }

        public static Ec2Record GetEc2RecordById(string recordId)
        {
            return m_Ec2Records.FirstOrDefault(record => record.InstanceId == recordId);
        }
    }
}

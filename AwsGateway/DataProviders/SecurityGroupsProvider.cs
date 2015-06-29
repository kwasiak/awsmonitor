using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using AwsGateway.Data;

using Amazon.S3;
using Amazon.S3.Model;

namespace AwsGateway.DataProviders
{
    public static class SecurityGroupsProvider
    {
        private static IList<SecurityGroupRecord> m_SecurityGroups;

        public static IList<SecurityGroupRecord> GetAllSecurityGroups(bool force = false)
        {
            if (m_SecurityGroups == null || force)
            {
                var ec2 = Ec2ConnectionProvider.GetEc2Connection();

                DescribeSecurityGroupsRequest req = new DescribeSecurityGroupsRequest();
                DescribeSecurityGroupsResponse res = ec2.DescribeSecurityGroups(req);

                m_SecurityGroups = new List<SecurityGroupRecord>();

                res.DescribeSecurityGroupsResult.SecurityGroups.ForEach(
                        securityGroup => m_SecurityGroups.Add(
                            new SecurityGroupRecord()
                                {
                                    Description = securityGroup.Description,
                                    Id = securityGroup.GroupId,
                                    Name = securityGroup.GroupName,
                                    Vpc = securityGroup.VpcId,
                                    Incomming = IpPermissionsToSecurityGroup(securityGroup.IpPermissions),
                                    Outgoing = IpPermissionsToSecurityGroup(securityGroup.IpPermissionsEgress)
                                }
                            )
                    );
            }

            return m_SecurityGroups;
        }

        public static IList<SecurityGroupEntryRecord> IpPermissionsToSecurityGroup(IList<IpPermission> permissions)
        {
            List<SecurityGroupEntryRecord> result = new List<SecurityGroupEntryRecord>();

            if (permissions != null && permissions.Any())
            {
                foreach (var ipPermission in permissions)
                {
                    result.Add(
                        new SecurityGroupEntryRecord()
                            {
                                IpRange = new List<string>(ipPermission.IpRanges),
                                Protocol = ipPermission.IpProtocol,
                                FromPort = ipPermission.FromPort.ToString(),
                                ToPort = ipPermission.ToPort.ToString()
                            }
                        );
                }
            }

            return result;
        }

        public static SecurityGroupRecord GetSecurityGroupByName(String securityGroupName)
        {
            return m_SecurityGroups.FirstOrDefault(group => group.Name == securityGroupName);
        }
    }
}

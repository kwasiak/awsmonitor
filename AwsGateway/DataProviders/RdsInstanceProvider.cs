using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.RDS;
using Amazon.RDS.Model;
using AwsGateway.Data;

namespace AwsGateway.DataProviders
{
    public class RdsInstanceProvider
    {
        private static IList<RdsRecord> m_RdsRecords;

        public static IList<RdsRecord> GetAllRdsInstances(bool force = false)
        {
            if (m_RdsRecords == null || force)
            {
                var rdsConnection = RdsConnectionProvider.GetRdsConnection();

                DescribeDBInstancesRequest request = new DescribeDBInstancesRequest();
                DescribeDBInstancesResponse response = rdsConnection.DescribeDBInstances(request);

                m_RdsRecords = new List<RdsRecord>();

                response.DescribeDBInstancesResult.DBInstances.ForEach(
                        instance => m_RdsRecords.Add(new RdsRecord()
                        {
                            AllocatedStorage = instance.AllocatedStorage,
                            DBInstanceClass = instance.DBInstanceClass,
                            DBInstanceIdentifier = instance.DBInstanceIdentifier,
                            DBInstanceStatus = instance.DBInstanceStatus,
                            DBName = instance.DBName,
                            Engine = instance.Engine,
                            EngineVersion = instance.EngineVersion,
                            MasterUsername = instance.MasterUsername,
                            InstanceCreateTime = instance.InstanceCreateTime
                        })
                    );
            }

            return m_RdsRecords;
        }
    }
}

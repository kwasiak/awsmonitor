using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.CloudWatch.Model;
using Amazon.Util;
using System.Globalization;

namespace AwsGateway.DataProviders
{
    public static class CloudWatchProvider
    {
        public static List<CloudWatchResultRecord> GetCpuUtilization(string instanceId, int minutesBack)
        {
            var cloudWatchClient = CloudWatchConnectionProvider.GetCloudWatchConnection();

            var dimension = new Dimension
            {
                Name = "InstanceId",
                Value = instanceId,
            };

            var request = new GetMetricStatisticsRequest();
            request.Dimensions.Add(dimension);

            var currentTime = DateTime.UtcNow;
            var startTime = currentTime.AddDays(-1);
            request.StartTime = startTime;
            request.EndTime = currentTime;

            request.Namespace = "AWS/EC2";
            request.Statistics.Add("Maximum");
            request.Statistics.Add("Average");
            request.MetricName = "CPUUtilization";
            request.Period = 60;

            var response = cloudWatchClient.GetMetricStatistics(request);

            if (response.GetMetricStatisticsResult.Datapoints.Count > 0)
            {
                var result = new List<CloudWatchResultRecord>();
                int counter = 0;

                foreach(var dataPoint in response.GetMetricStatisticsResult.Datapoints)
                {
                    result.Add(new CloudWatchResultRecord()
                    {
                        RecordId = counter,
                        CpuAverage = dataPoint.Average
                    });
                }

                return result;
            }
            else
            {
                return null;
            }
        }

    }
}

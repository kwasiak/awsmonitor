using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsGateway.DataProviders
{
    public class CloudWatchResultRecord
    {
        public int RecordId { get; set; }
        public double CpuAverage { get; set; }
    }
}

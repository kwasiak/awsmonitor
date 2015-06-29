using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsGateway.Data
{
    public class RdsRecord
    {
        public int AllocatedStorage { get; set; }
        public string DBInstanceClass { get; set; }
        public string DBInstanceIdentifier { get; set; }
        public string DBInstanceStatus { get; set; }
        public string DBName { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }
        public DateTime InstanceCreateTime { get; set; }
        public string MasterUsername { get; set; }
    }
}

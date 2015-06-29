using System.Collections.Generic;

namespace AwsGateway.Data
{
    public class SecurityGroupEntryRecord
    {
        public string Protocol { get; set; }
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string UserGroup { get; set; }
        public string SourceCidr { get; set; }
        public IList<string> IpRange { get; set; }
    }
}

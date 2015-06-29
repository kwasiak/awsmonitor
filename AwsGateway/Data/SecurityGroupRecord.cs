using System;
using System.Collections.Generic;
using System.Linq;

namespace AwsGateway.Data
{
    public class SecurityGroupRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vpc { get; set; }
        public string Description { get; set; }
        public IList<SecurityGroupEntryRecord> Incomming { get; set; }
        public IList<Tuple<string, string, string, string>> IncommingAgregated
        {
            get { return AgregateSecurityData(Incomming); }
        }


        public IList<SecurityGroupEntryRecord> Outgoing { get; set; }
        public IList<Tuple<string, string, string, string>> OutgoingAgregated
        {
            get { return AgregateSecurityData(Outgoing); }
        }


        public IList<Tuple<string, string, string, string>> AgregateSecurityData(IList<SecurityGroupEntryRecord> securityData)
        {
            var result = new List<Tuple<string, string, string, string>>();

            foreach (var securityGroupEntryRecord in securityData)
            {
                foreach (var ip in securityGroupEntryRecord.IpRange)
                {
                    result.Add(new Tuple<string, string, string, string>(securityGroupEntryRecord.Protocol, securityGroupEntryRecord.FromPort, securityGroupEntryRecord.ToPort, ip));
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
namespace AwsGateway.Data
{
    public class VolumeRecord
    {
        public SnapshotRecord Snapshot { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public int Capacity { get; set; }
        public DateTime Created { get; set; }
        public string Zone { get; set; }
        public string Status { get; set; }
        public string AttachmentInfo { get; set; }
        public IList<Ec2Record> AttachedEc2 { get; set; }
        public string AttachedMachines 
        { 
            get
            {
                if (AttachedEc2 == null || !AttachedEc2.Any())
                {
                    return "-";
                }
                else
                {
                    return AttachedEc2.Select(attachment => attachment.Name).Aggregate((s, s1) => s + ',' + s1);
                }
            }
        }
    }
}

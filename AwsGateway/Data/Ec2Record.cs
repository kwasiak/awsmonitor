using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.EC2.Model;
using AwsGateway.DataProviders;

namespace AwsGateway.Data
{
    public class Ec2Record
    {
        public AmiRecord AmiRecord { get; set; }
        public string InstanceId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Platform { get; set; }
        public string Ip { get; set; }
        public string PrivateIp { get; set; }
        public string PublicDns { get; set; }
        public string PrivateDns { get; set; }
        public string Name { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTime LaunchTime { get; set; }

        public IList<VolumeRecord> AttachedVolumes { get; set; }
        public IList<string> SecurityGroups { get; set; }

        public string AttachedVolumesString
        {
            get
            {
                if (AttachedVolumes == null || !AttachedVolumes.Any())
                {
                    AttachedVolumes = VolumeProvider.GetVolumesByEc2Id(InstanceId);
                }

                return AttachedVolumes.Any()
                           ? AttachedVolumes
                                 .Select(volume => string.Format("{0} ({1} GB)", volume.Id, volume.Capacity))
                                 .Aggregate((agregate, str) => agregate + ",\n" + str)
                           : "-";
            }
        }
    }
}

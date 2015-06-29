using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.AWSSupport.Model;
using Amazon.EC2;
using Amazon.EC2.Model;
using AwsGateway.Data;
using AwsGateway.Tools;

namespace AwsGateway.DataProviders
{
    public static class VolumeProvider
    {
        private static IList<VolumeRecord> m_Volumes;

        public static IList<VolumeRecord> GetAllVolumes(bool force = false)
        {
            if (m_Volumes == null || force)
            {
                var ec2 = Ec2ConnectionProvider.GetEc2Connection();

                DescribeVolumesRequest request = new DescribeVolumesRequest();
                DescribeVolumesResponse res = ec2.DescribeVolumes(request);

                m_Volumes = new List<VolumeRecord>();

                res.DescribeVolumesResult.Volumes.ForEach(
                        volume => m_Volumes.Add(new VolumeRecord()
                        {
                            Capacity = volume.Size,
                            Created = volume.CreateTime,
                            Id = volume.VolumeId,
                            Name = AwsTags.GetValueFromEc2Tag(volume.Tags, "Name"),
                            Status = volume.State,
                            AttachedEc2 = AttachmentToMachineNames(volume.Attachments)
                        }
                                    
                            ));

            }

            return m_Volumes;
        }

        public static VolumeRecord GetVolumeById(string volumeId)
        {
            if (m_Volumes == null)
            {
                GetAllVolumes();
            }

            return m_Volumes.FirstOrDefault(volume => volume.Id == volumeId);
        }

        /// <summary>
        /// Get all volumes attached to the given EC2 instance
        /// </summary>
        /// <param name="ec2Id">ID of the EC2 instance</param>
        /// <returns>List of attached volumes</returns>
        public static IList<VolumeRecord> GetVolumesByEc2Id(string ec2Id)
        {
            IList<VolumeRecord> result = new List<VolumeRecord>();

            if (m_Volumes == null)
            {
                GetAllVolumes();
            }

            foreach (var volumeRecord in m_Volumes)
            {
                foreach (var ec2Record in volumeRecord.AttachedEc2)
                {
                    if (ec2Record.InstanceId == ec2Id)
                    {
                        result.Add(volumeRecord);
                    }
                }
            }

            return result;
        }

        private static IList<Ec2Record> AttachmentToMachineNames(List<VolumeAttachment> attachments)
        {
            var result = new List<Ec2Record>();

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var foundEc2 = Ec2InstanceProvider.GetEc2RecordById(attachment.InstanceId);
                    if (foundEc2 != null)
                    {
                        result.Add(foundEc2);
                    }
                }
            }

            return result;
        }

    }
}

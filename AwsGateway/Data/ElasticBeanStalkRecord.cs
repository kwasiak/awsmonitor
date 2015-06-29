using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsGateway.Data
{
    public class ElasticBeanStalkRecord
    {
        public string ApplicationName { get; set; }
        public List<string> ConfigurationTemplates { get; set; }
        public int ConfigurationTemplatesCount { get { return ConfigurationTemplates.Count; } }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Description { get; set; }
        public List<string> Versions { get; set; }
        public int VersionsCount { get { return Versions.Count; } }

        public ElasticBeanStalkRecord()
        {
            ConfigurationTemplates = new List<string>();
            Versions = new List<string>();
        }
    }
}

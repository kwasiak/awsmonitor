using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AwsGateway.Data
{
    /// <summary>
    /// Single record (i.e.: line) from the AWS financial record CSV
    /// </summary>
    public class AwsFinancialReportRecord : Dictionary<string,string>
    {
        public string GetRecordOrNull(string key)
        {
            if (!this.Keys.Contains(key))
            {
                return null;
            }

            return this[key];
        }

        public string GetRecordOrEmptyChar(string key, char emptyChar)
        {
            return this.GetRecordOrNull(key) ?? emptyChar.ToString(CultureInfo.InvariantCulture);
        }

        public string GetRecordOrEmptyChar(string key)
        {
            return this.GetRecordOrEmptyChar(key, '-');
        }
    }
}

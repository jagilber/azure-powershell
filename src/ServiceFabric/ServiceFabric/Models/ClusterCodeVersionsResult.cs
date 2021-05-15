// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.ServiceFabric.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    [JsonObject(IsReference = false)]
    public class ClusterCodeVersionsResult
    {
        public string id
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public Properties properties { get; set; }

        public string type
        {
            get;
            set;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.CurrentCulture, "Version: {0}; ", this.properties.codeVersion.ToString() ?? string.Empty);
            sb.AppendFormat(CultureInfo.CurrentCulture, "SupportExpiryDate: {0}; ", properties.supportExpiryUtc?.ToString("O"));
            sb.AppendFormat(CultureInfo.CurrentCulture, "Environment: {0}; ", this.properties.environment ?? string.Empty);

            return sb.ToString();
        }

        public class Properties
        {
            [DefaultValue("0.0.0.0")]
            public Version codeVersion { get; set; }
            public string environment { get; set; }
            public DateTime? supportExpiryUtc { get; set; }
        }
    }
}
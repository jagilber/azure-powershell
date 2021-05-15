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
    using System.Management.Automation;
    using System.Text;
    using Newtonsoft.Json;
    using Microsoft.Azure.Management.ServiceFabric.Models;

    [JsonObject(IsReference = false)]
    public class RuntimePackageDetails
    {
        public string Environment
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool IsGoalPackage
        {
            get;
            set;
        }

        public DateTime? SupportExpiryDate
        {
            get;
            set;
        }

        public Version Version
        {
            get;
            set;
        }

        public RuntimePackageDetails(ClusterCodeVersionsResult packageDetails)
        {
            Version = packageDetails.properties.codeVersion;
            SupportExpiryDate = packageDetails.properties.supportExpiryUtc;
            Environment = packageDetails.properties.environment;
            IsGoalPackage = false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.CurrentCulture, "Version: {0}; ", this.Version.ToString() ?? string.Empty);
            sb.AppendFormat(CultureInfo.CurrentCulture, "SupportExpiryDate: {0}; ", this.SupportExpiryDate);
            sb.AppendFormat(CultureInfo.CurrentCulture, "Environment: {0}; ", this.Environment ?? string.Empty);
            return sb.ToString();
        }
    }
}
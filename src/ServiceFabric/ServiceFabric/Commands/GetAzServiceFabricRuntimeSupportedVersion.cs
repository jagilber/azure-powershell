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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.Azure.Commands.ServiceFabric.Common;
using System.Net;
using Microsoft.Azure.Management.Internal.Resources;
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.Azure.Commands.ServiceFabric.Models;
using Microsoft.Azure.Management.Internal.Resources.Models;
using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.Common;

namespace Microsoft.Azure.Commands.ServiceFabric.Commands
{
    [Cmdlet(VerbsCommon.Get, ResourceManager.Common.AzureRMConstants.AzurePrefix + "ServiceFabricRuntimeSupportedVersion", DefaultParameterSetName = "ByRegion"), OutputType(typeof(List<RuntimePackageDetails>))]
    public partial class GetAzServiceFabricRuntimeSupportedVersion : ServiceFabricCmdletBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "ByRegion", Position = 0)]
        [Parameter(ParameterSetName = "ByLatest")]
        [Parameter(ParameterSetName = "ByVersion")]
        public string Region
        {
            get;
            set;
        }

        [Parameter(Mandatory = false, Position = 1)]
        [ValidateSet("Windows", "Linux")]
        [PSDefaultValue(Help = "Windows", Value = "Windows")]
        public string Environment
        {
            get;
            set;
        } = "Windows";

        [Parameter(Mandatory = false, ParameterSetName = "ByVersion", Position = 2)]
        public string Version
        {
            get;
            set;
        }

        [Parameter(Mandatory = false, ParameterSetName = "ByLatest", Position = 3)]
        public SwitchParameter Latest
        {
            get;
            set;
        }

        public override string ResourceGroupName
        {
            get => base.ResourceGroupName;
            set => base.ResourceGroupName = value;
        }

        protected List<RuntimePackageDetails> FormatOutput(List<ClusterCodeVersionsResult> items)
        {
            List<RuntimePackageDetails> runtimePackageDetails = new List<RuntimePackageDetails>();

            if (items == null)
            {
                return runtimePackageDetails;
            }

            if (!string.IsNullOrEmpty(Environment))
            {
                if (Environment.ToLower().Equals("windows"))
                {
                    items = items.Where(x => x.properties.environment.ToLower().Equals("windows")).ToList();
                }
                else
                {
                    items = items.Where(x => !x.properties.environment.ToLower().Equals("windows")).ToList();
                }
            }

            Version maxVersion = items.Max(x => x.properties.codeVersion);

            if (Latest)
            {
                items = items.Where(x => x.properties.codeVersion == maxVersion).ToList();
            }
            else if (!string.IsNullOrEmpty(Version))
            {
                Version version = new Version(Version);
                items = items.Where(x => x.properties.codeVersion.CompareTo(version) == 0).ToList();
            }

            foreach (ClusterCodeVersionsResult package in items)
            {
                RuntimePackageDetails packageDetails = new RuntimePackageDetails(package);
                packageDetails.IsGoalPackage = packageDetails.Version == maxVersion.ToString();
                runtimePackageDetails.Add(packageDetails);
            }

            return runtimePackageDetails;
        }

        protected override void ProcessRecord()
        {
            try
            {
                if (!IsRegionValid())
                {
                    this.WriteWarning($"{Region} is not a valid region");
                    return;
                }

                List<ClusterCodeVersionsResult> runtimeVersions = this.GetRuntimeSupportedVersion();
                this.WriteObject(this.FormatOutput(runtimeVersions));
            }
            catch (AggregateException aggregateException)
            {
                PrintSdkExceptionDetail(aggregateException);
                throw;
            }
        }

        private List<ClusterCodeVersionsResult> GetRuntimeSupportedVersion()
        {
            string resourceUri = $"https://management.azure.com/subscriptions/{SFRPClient.SubscriptionId}/providers/"
                + $"{Constants.ServiceFabricResourceProvider}/locations/{Region}/clusterVersions?api-version={Constants.ServiceFabricResourceProviderApiVersion}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, resourceUri);
            CancellationToken cancellationToken = new CancellationToken();
            SFRPClient.Credentials.ProcessHttpRequestAsync(httpRequestMessage, cancellationToken).Wait();

            HttpResponseMessage httpResult = SFRPClient.HttpClient.SendAsync(httpRequestMessage).Result;
            if (httpResult.StatusCode != HttpStatusCode.OK)
            {
                this.WriteError(
                    new ErrorRecord(
                        new HttpRequestException(JsonConvert.SerializeObject(httpResult, Formatting.Indented)),
                        httpResult.ReasonPhrase,
                        ErrorCategory.InvalidArgument,
                        httpResult
                    )
                );
            }

            string result = httpResult.Content.ReadAsStringAsync().Result;
            ClusterCodeVersionsListResult packageJson = JsonConvert.DeserializeObject<ClusterCodeVersionsListResult>(result);

            this.WriteVerbose(JsonConvert.SerializeObject(packageJson.value, Formatting.Indented));
            return packageJson.value.ToList();
        }

        private bool IsRegionValid()
        {
            Lazy<IResourceManagementClient> resourceManagerClient = new Lazy<IResourceManagementClient>(() =>
                AzureSession.Instance.ClientFactory.CreateArmClient<ResourceManagementClient>(DefaultContext, AzureEnvironment.Endpoint.ResourceManager)
            );

            Provider providers = resourceManagerClient.Value.Providers.Get(Constants.ServiceFabricResourceProvider);
            IEnumerable<string> locations = providers.ResourceTypes
                .FirstOrDefault(x => x.ResourceType.Equals(Constants.clusterProvider)).Locations.Select(x => x.ToLower().Replace(" ", ""));
            string providedRegion = Region.ToLower().Replace(" ", "");

            if (!locations.Contains(providedRegion))
            {
                this.WriteWarning(JsonConvert.SerializeObject(locations, Formatting.Indented));
                this.WriteError(
                    new ErrorRecord(
                        new ArgumentException("invalid region provided. select from list above.", "Region"),
                        "invalid region",
                        ErrorCategory.InvalidArgument,
                        Region
                    )
                );

                return false;
            }

            this.WriteVerbose(JsonConvert.SerializeObject(locations, Formatting.Indented));
            return true;
        }
    }
}
﻿// ----------------------------------------------------------------------------------
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

namespace Microsoft.Azure.Commands.ResourceManager.Cmdlets.Implementation
{
    using System.Collections;
    using System.Linq;
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Components;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Entities.Resources;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Extensions;
    using Microsoft.WindowsAzure.Commands.Common;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A cmdlet that creates a new azure resource.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureRmResource", SupportsShouldProcess = true, DefaultParameterSetName = ResourceManipulationCmdletBase.ResourceIdParameterSet), OutputType(typeof(PSObject))]
    public sealed class SetAzureResourceCmdlet : ResourceManipulationCmdletBase
    {
        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The resource kind.")]
        [ValidateNotNullOrEmpty]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets the property object.
        /// </summary>
        [Alias("PropertyObject")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A hash table which represents resource properties.")]
        [ValidateNotNullOrEmpty]
        public PSObject Properties { get; set; }

        /// <summary>
        /// Gets or sets the plan object.
        /// </summary>
        [Alias("PlanObject")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A hash table which represents resource plan properties.")]
        [ValidateNotNullOrEmpty]
        public Hashtable Plan { get; set; }

        /// <summary>  
        /// Gets or sets the plan object.  
        /// </summary>  
        [Alias("SkuObject")]  
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A hash table which represents sku properties.")]  
        [ValidateNotNullOrEmpty]  
        public Hashtable Sku { get; set; }  


        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [Alias("Tags")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A hash table which represents resource tags.")]
        public Hashtable[] Tag { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if an HTTP PATCH request needs to be made instead of PUT.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "When set indicates if an HTTP PATCH should be used to update the object instead of PUT.")]
        public SwitchParameter UsePatchSemantics { get; set; }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void OnProcessRecord()
        {
            base.OnProcessRecord();

            if(!string.IsNullOrEmpty(this.ODataQuery))
            {
                this.WriteWarning("The ODataQuery parameter is being deprecated in Set-AzureRmResource cmdlet and will be removed in a future release. Also, the usability of Tag parameter in this cmdlet will be modified in a future release. This will impact creating, updating and appending tags for Azure resources. For more details about the change, please visit https://github.com/Azure/azure-powershell/issues/726#issuecomment-213545494");
            }

            var resourceId = this.GetResourceId();
            this.ConfirmAction(
                this.Force,
                "Are you sure you want to update the following resource: " + resourceId,
                "Updating the resource...",
                resourceId,
                () =>
                {
                    var apiVersion = this.DetermineApiVersion(resourceId: resourceId).Result;
                    var resourceBody = this.GetResourceBody();

                    var operationResult = this.ShouldUsePatchSemantics()
                        ? this.GetResourcesClient()
                            .PatchResource(
                                resourceId: resourceId,
                                apiVersion: apiVersion,
                                resource: resourceBody,
                                cancellationToken: this.CancellationToken.Value,
                                odataQuery: this.ODataQuery)
                            .Result
                        : this.GetResourcesClient()
                            .PutResource(
                                resourceId: resourceId,
                                apiVersion: apiVersion,
                                resource: resourceBody,
                                cancellationToken: this.CancellationToken.Value,
                                odataQuery: this.ODataQuery)
                            .Result;

                    var managementUri = this.GetResourcesClient()
                        .GetResourceManagementRequestUri(
                            resourceId: resourceId,
                            apiVersion: apiVersion,
                            odataQuery: this.ODataQuery);

                    var activity = string.Format("{0} {1}", this.ShouldUsePatchSemantics() ? "PATCH" : "PUT", managementUri.PathAndQuery);
                    var result = this.GetLongRunningOperationTracker(activityName: activity, isResourceCreateOrUpdate: true)
                        .WaitOnOperation(operationResult: operationResult);

                    this.TryConvertToResourceAndWriteObject(result);
                });
        }

        /// <summary>
        /// Gets the resource body.
        /// </summary>
        private JToken GetResourceBody()
        {
            if(this.ShouldUsePatchSemantics())
            {
                var resourceBody = this.GetPatchResourceBody();

                return resourceBody == null ? null : resourceBody.ToJToken();
            }
            else
            {
                var getResult = this.GetResource().Result;

                if (getResult.CanConvertTo<Resource<JToken>>())
                {
                    var resource = getResult.ToResource();
                    return new Resource<JToken>()
                    {
                        Kind = this.Kind ?? resource.Kind,
                        Plan = this.Plan.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourcePlan>() ?? resource.Plan,
                        Sku = this.Sku.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourceSku>() ?? resource.Sku,
                        Tags = TagsHelper.GetTagsDictionary(this.Tag) ?? resource.Tags,
                        Location = resource.Location,
                        Properties = this.Properties == null ? resource.Properties : this.Properties.ToResourcePropertiesBody(),
                    }.ToJToken();
                }
                else
                {
                    return this.Properties.ToJToken();
                }
            }

        }

        /// <summary>
        /// Gets the resource body for PATCH calls
        /// </summary>
        private Resource<JToken> GetPatchResourceBody()
        {
            Resource<JToken> resourceBody = null;

            if (this.Properties != null)
            {
                resourceBody = new Resource<JToken>()
                {
                    Properties = this.Properties.ToResourcePropertiesBody()
                };
            }

            if (this.Plan != null)
            {
                if(resourceBody != null)
                {
                    resourceBody.Plan = this.Plan.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourcePlan>();
                }
                else
                {
                    resourceBody = new Resource<JToken>()
                    {
                        Plan = this.Plan.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourcePlan>()
                    };
                }
            }

            if (this.Kind != null)
            {
                if(resourceBody != null)
                {
                    resourceBody.Kind = this.Kind;
                }
                else
                {
                    resourceBody = new Resource<JToken>()
                    {
                        Kind = this.Kind
                    };
                }
            }

            if (this.Sku != null)
            {
                if(resourceBody != null)
                {
                    resourceBody.Sku = this.Sku.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourceSku>();
                }
                else
                {
                    resourceBody = new Resource<JToken>()
                    {
                        Sku = this.Sku.ToDictionary(addValueLayer: false).ToJson().FromJson<ResourceSku>()
                    };
                }
            }

            if (this.Tag != null)
            {
                if(resourceBody != null)
                {
                    resourceBody.Tags = TagsHelper.GetTagsDictionary(this.Tag);
                }
                else
                {
                    resourceBody = new Resource<JToken>()
                    {
                        Tags = TagsHelper.GetTagsDictionary(this.Tag)
                    };
                }
            }

            return resourceBody;
        }

        /// <summary>
        /// Determines if the cmdlet should use <c>PATCH</c> semantics.
        /// </summary>
        private bool ShouldUsePatchSemantics()
        {
            return this.UsePatchSemantics || ((this.Tag != null || this.Sku != null) && this.Plan == null && this.Properties == null && this.Kind == null);
        }

        /// <summary>
        /// Gets a resource.
        /// </summary>
        private async Task<JObject> GetResource()
        {
            var resourceId = this.GetResourceId();
            var apiVersion = await this
                .DetermineApiVersion(resourceId: resourceId)
                .ConfigureAwait(continueOnCapturedContext: false);

            return await this
                .GetResourcesClient()
                .GetResource<JObject>(
                    resourceId: resourceId,
                    apiVersion: apiVersion,
                    cancellationToken: this.CancellationToken.Value)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
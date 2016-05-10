// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using Microsoft.Azure.Management.Internal.Resources.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Management.Internal.Resources
{
    /// <summary>
    /// Operations for managing providers.
    /// </summary>
    public partial interface IProviderOperations
    {
        /// <summary>
        /// Gets a resource provider.
        /// </summary>
        /// <param name='resourceProviderNamespace'>
        /// Namespace of the resource provider.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Resource provider information.
        /// </returns>
        Task<ProviderGetResult> GetAsync(string resourceProviderNamespace, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of resource providers.
        /// </summary>
        /// <param name='parameters'>
        /// Query parameters. If null is passed returns all deployments.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// List of resource providers.
        /// </returns>
        Task<ProviderListResult> ListAsync(ProviderListParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Get a list of deployments.
        /// </summary>
        /// <param name='nextLink'>
        /// NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// List of resource providers.
        /// </returns>
        Task<ProviderListResult> ListNextAsync(string nextLink, CancellationToken cancellationToken);

        /// <summary>
        /// Registers provider to be used with a subscription.
        /// </summary>
        /// <param name='resourceProviderNamespace'>
        /// Namespace of the resource provider.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Resource provider registration information.
        /// </returns>
        Task<ProviderRegistionResult> RegisterAsync(string resourceProviderNamespace, CancellationToken cancellationToken);

        /// <summary>
        /// Unregisters provider from a subscription.
        /// </summary>
        /// <param name='resourceProviderNamespace'>
        /// Namespace of the resource provider.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Resource provider registration information.
        /// </returns>
        Task<ProviderUnregistionResult> UnregisterAsync(string resourceProviderNamespace, CancellationToken cancellationToken);
    }
}

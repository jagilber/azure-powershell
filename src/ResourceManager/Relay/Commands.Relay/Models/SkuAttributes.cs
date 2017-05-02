// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Commands.Relay.Models
{
    using Azure;
    using Management;
    using Relay;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Sku of the Namespace.
    /// </summary>
    public partial class SkuAttributes
    {
        /// <summary>
        /// Initializes a new instance of the Sku class.
        /// </summary>
        public SkuAttributes() { }

        /// <summary>
        /// Static constructor for Sku class.
        /// </summary>
        static SkuAttributes()
        {
            Name = "Standard";
            Tier = "Standard";
        }

        /// <summary>
        /// Name of this Sku
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public static string Name { get; private set; }

        /// <summary>
        /// The tier of this particular SKU
        /// </summary>
        [JsonProperty(PropertyName = "tier")]
        public static string Tier { get; private set; }

    }
}


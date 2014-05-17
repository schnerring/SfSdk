// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="">
//   Copyright (c) 2014 ebeeb
// </copyright>
// <summary>
//   Implements the functionality of creating a new ICountry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SfSdk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using SfSdk.Contracts;

    /// <summary>
    ///     Implements the functionality of creating a new <see cref="ICountry"/>.
    /// </summary>
    [Serializable]
    internal class Country : ICountry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class. 
        ///     This constructor is required for the JSON deserializer to be able
        ///     to identify concrete classes to use when deserializing the interface properties.
        /// </summary>
        /// <param name="servers">
        /// The servers.
        /// </param>
        [JsonConstructor]
        internal Country(IEnumerable<Server> servers)
        {
            this.Servers = servers.ToList<IServer>();
        }

        /// <summary>
        ///     Gets the country's name.
        /// </summary>
        [JsonProperty]
        public string CountryName { get; private set; }

        /// <summary>
        ///     Gets the servers.
        /// </summary>
        public IList<IServer> Servers { get; private set; }
    }
}
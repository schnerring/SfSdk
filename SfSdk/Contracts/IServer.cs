// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServer.cs" company="">
//   Copyright (c) 2014 ebeeb
// </copyright>
// <summary>
//   Contains information about a sub-server of a country where S&amp;F is available.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SfSdk.Contracts
{
    using System;

    /// <summary>
    ///     Contains information about a sub-server of a country where S&amp;F is available.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        ///     Gets the server's name.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        ///     Gets game server's <see cref="Uri"/>.
        /// </summary>
        Uri ServerUri { get; }
    }
}
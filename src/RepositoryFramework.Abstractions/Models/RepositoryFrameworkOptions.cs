﻿namespace RepositoryFramework
{
    /// <summary>
    /// Options for repository framework.
    /// </summary>
    public sealed class RepositoryFrameworkOptions<T, TKey> : IRepositoryFrameworkOptions
        where TKey : notnull
    {
        /// <summary>
        /// Your repository force translation during execution
        /// </summary>
        public bool HasToTranslate { get; set; } = true;
        /// <summary>
        /// It's a parameter used by framework to understand the level of privacy,
        /// used for instance in library Api.Server to avoid auto creation of an api with this repository implementation.
        /// </summary>
        public bool IsNotExposableAsApi { get; set; }
    }
}
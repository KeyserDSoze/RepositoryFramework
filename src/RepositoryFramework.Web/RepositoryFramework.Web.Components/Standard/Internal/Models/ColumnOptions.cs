﻿namespace RepositoryFramework.Web.Components.Standard
{
    internal sealed class ColumnOptions
    {
        public bool IsActive { get; set; } = true;
        public Type Type { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
    }
}

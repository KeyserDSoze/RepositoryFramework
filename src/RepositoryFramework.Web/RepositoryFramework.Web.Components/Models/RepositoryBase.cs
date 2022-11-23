﻿using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components
{
    public abstract class RepositoryBase : ComponentBase
    {
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? Key { get; set; }
        [Inject]
        public IServiceProvider ServiceProvider { get; set; } = null!;
        [Inject]
        public AppMenu AppMenu { get; set; } = null!;
        private Type? _keyType;
        private Type? _modelType;
        private protected abstract Type StandardType { get; }
        private protected abstract bool HasKeyInParameters { get; }
        private protected bool IsLoadable => _keyType != null && _modelType != null;
        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.ToLower();
                _keyType = AppMenu.Models[name].KeyType;
                _modelType = AppMenu.Models[name].ModelType;
            }

            return base.OnInitializedAsync();
        }
        private protected RenderFragment LoadStandard()
        {
            var genericType = StandardType.MakeGenericType(new[] { _modelType!, _keyType! });
            var frag = new RenderFragment(b =>
            {
                b.OpenComponent(1, genericType);
                if (HasKeyInParameters)
                    b.AddAttribute(2, nameof(Key), Key);
                b.CloseComponent();
            });
            return frag;
        }
    }
}
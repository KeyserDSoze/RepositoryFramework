﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

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
        private protected abstract Action<RenderTreeBuilder>? RenderTreeBuilderConfigurator { get; }
        private protected bool IsLoadable => _keyType != null && _modelType != null;
        protected override Task OnParametersSetAsync()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.ToLower();
                _modelType = AppMenu.Navigations[name].ModelType;
                _keyType = AppMenu.Navigations[name].KeyType;
            }

            return base.OnParametersSetAsync();
        }
        private protected RenderFragment LoadStandard()
        {
            var genericType = StandardType.MakeGenericType(new[] { _modelType!, _keyType! });
            var frag = new RenderFragment(b =>
            {
                b.OpenComponent(1, genericType);
                RenderTreeBuilderConfigurator?.Invoke(b);
                b.CloseComponent();
            });
            return frag;
        }
    }
}

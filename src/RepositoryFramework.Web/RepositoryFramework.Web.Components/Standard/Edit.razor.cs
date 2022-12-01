﻿using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Edit<T, TKey>
        where TKey : notnull
    {
        private static readonly Func<string, TKey> s_keyParser = IKey.Parser<TKey>();
        [Parameter]
        public string Key { get; set; } = null!;
        [Parameter]
        public bool EditableKey { get; set; } = true;
        [Parameter]
        public bool DisableEdit { get; set; }
        [Parameter]
        public bool AllowDelete { get; set; }
        [Inject]
        public AppSettings AppSettings { get; set; }

        private T? _entity;
        private bool _isNew;
        private bool _isRequestedToCreateNew;
        private TKey _key = default!;
        private RepositoryFeedback? _feedback;
        private Dictionary<string, RepositoryUiPropertyValueRetrieved> _propertiesRetrieved = new();
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            foreach (var retrieve in RepositoryUiPropertyValueRetriever<T, TKey>.Instance.Retrieves)
            {
                if (!_propertiesRetrieved.ContainsKey(retrieve.Key))
                    _propertiesRetrieved.Add(retrieve.Key, new RepositoryUiPropertyValueRetrieved
                    {
                        Default = retrieve.Value.Default,
                        IsMultiple = retrieve.Value.IsMultiple,
                        Values = retrieve.Value.Retriever != null ? await retrieve.Value.Retriever.Invoke(ServiceProvider).NoContext() : default,
                        LabelComparer = retrieve.Value.LabelComparer,
                    });
            }

            if (Query != null)
            {
                if (!string.IsNullOrWhiteSpace(Key))
                {
                    _key = s_keyParser(Key);
                    _entity = await Query.GetAsync(_key).NoContext();
                }
                else
                {
                    _key = default;
                    _entity = default;
                }
                if (_entity == null)
                {
                    _entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
                    _isNew = true;
                    _isRequestedToCreateNew = true;
                }
            }
        }
        private async Task SaveAsync(bool withRedirect)
        {
            if (Command != null)
            {
                var result = _isNew ?
                    await Command.InsertAsync(_key, _entity).NoContext() :
                    await Command.UpdateAsync(_key, _entity).NoContext();
                if (result.IsOk && withRedirect)
                    NavigationManager.NavigateTo($"../../../../Repository/{typeof(T).Name}/Query");
                if (!result.IsOk)
                {
                    _feedback = new RepositoryFeedback
                    {
                        IsOk = false,
                        Message = result.Message,
                        Title = "Saving error",
                        IsVisible = true,
                    };
                }
            }
            else
            {
                _feedback = new RepositoryFeedback
                {
                    IsOk = false,
                    Message = "Command pattern or repository pattern not installed to perform the task. It's not possible to save the current item.",
                    Title = "Saving error",
                    IsVisible = true,
                };
            }
        }
        private void CheckIfYouWantToDelete()
        {
            _feedback = new RepositoryFeedback
            {
                IsOk = true,
                Message = $"Are you sure to delete the current item with key {Key}",
                Title = "Delete",
                IsVisible = true,
                Ok = () => DeleteAsync(),
                HasCancelButton = true
            };
        }
        private async ValueTask DeleteAsync()
        {
            if (Command != null)
            {
                var result = await Command.DeleteAsync(_key).NoContext();
                if (result.IsOk)
                    NavigationManager.NavigateTo($"../../../../Repository/{typeof(T).Name}/Query");
                else
                    _feedback = new RepositoryFeedback
                    {
                        IsOk = false,
                        Message = result.Message,
                        Title = "Delete error",
                        IsVisible = true,
                    };
            }
            else
            {
                _feedback = new RepositoryFeedback
                {
                    IsOk = false,
                    Message = "Command pattern or repository pattern not installed to perform the task. It's not possible to delete the current item.",
                    Title = "Delete error",
                    IsVisible = true,
                };
            }
        }
    }
}

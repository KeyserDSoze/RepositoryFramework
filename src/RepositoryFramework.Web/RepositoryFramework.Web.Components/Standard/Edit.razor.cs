using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Edit<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public string Key { get; set; } = null!;
        [Parameter]
        public bool EditableKey { get; set; } = true;
        [Parameter]
        public bool DisableEdit { get; set; }
        [Parameter]
        public bool AllowDelete { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }

        private T? _entity;
        private bool _isNew;
        private bool _isRequestedToCreateNew;
        private TKey _key = default!;
        private Dictionary<string, PropertyUiSettings> _propertiesRetrieved;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().NoContext();
            _propertiesRetrieved =
                ServiceProvider.GetService<IRepositoryPropertyUiMapper<T, TKey>>() is IRepositoryPropertyUiMapper<T, TKey> uiMapper ?
                await uiMapper.ValuesAsync(ServiceProvider!).NoContext()
                : new();

            if (Query != null)
            {
                if (!string.IsNullOrWhiteSpace(Key))
                {
                    _key = Key.FromBase64<TKey>();
                    _entity = await Query.GetAsync(_key).NoContext();
                }
                else
                {
                    _key = default!;
                    _entity = default;
                }
                if (_entity == null)
                {
                    _entity = typeof(T).CreateWithDefaultConstructorPropertiesAndField<T>();
                    _isNew = true;
                    _isRequestedToCreateNew = true;
                }
            }
            LoadService.Hide();
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
                    NotificationService.Notify(new Radzen.NotificationMessage
                    {
                        Duration = 4_000,
                        CloseOnClick = true,
                        Severity = Radzen.NotificationSeverity.Error,
                        Summary = "Saving error",
                        Detail = result.Message
                    });
                }
            }
            else
            {
                NotificationService.Notify(new Radzen.NotificationMessage
                {
                    Duration = 4_000,
                    CloseOnClick = true,
                    Severity = Radzen.NotificationSeverity.Error,
                    Summary = "Saving error",
                    Detail = "Command pattern or repository pattern not installed to perform the task. It's not possible to save the current item."
                });
            }
        }
        private async Task CheckIfYouWantToDeleteAsync()
        {
            _ = await DialogService.OpenAsync<Popup>("Delete confirmation",
                new Dictionary<string, object>
                {
                    { "Ok", () => DeleteAsync() },
                    { "Message", $"Are you sure to delete the current item with key {Key.FromBase64<TKey>()}" },
                });
        }
        private async ValueTask DeleteAsync()
        {
            if (Command != null)
            {
                var result = await Command.DeleteAsync(_key).NoContext();
                if (result.IsOk)
                    NavigationManager.NavigateTo($"../../../../Repository/{typeof(T).Name}/Query");
                else
                    NotificationService.Notify(new Radzen.NotificationMessage
                    {
                        Duration = 4_000,
                        CloseOnClick = true,
                        Severity = Radzen.NotificationSeverity.Error,
                        Summary = "Deleting error",
                        Detail = "Command pattern or repository pattern not installed to perform the task. It's not possible to save the current item."
                    });
            }
            else
            {
                NotificationService.Notify(new Radzen.NotificationMessage
                {
                    Duration = 4_000,
                    CloseOnClick = true,
                    Severity = Radzen.NotificationSeverity.Error,
                    Summary = "Deleting error",
                    Detail = "Command pattern or repository pattern not installed to perform the task. It's not possible to delete the current item."
                });
            }
        }
        private TKey? _keyBeforeEdit;
        private void ChangeKeyEditingStatus(bool x)
        {
            _isNew = x;
            if (_isNew)
                _keyBeforeEdit = _key;
            else
                _key = _keyBeforeEdit!;
        }
    }
}

using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using RepositoryFramework.Web.Components.Services;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class Query<T, TKey>
        where TKey : notnull
    {
        [Parameter]
        public int PageSize { get; set; } = 10;
        [Inject]
        public ICopyService Copy { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        private bool _isLoading = true;
        private List<Entity<T, TKey>>? _entities;
        private int _totalItems;
        private static readonly string? s_editUri = $"Repository/{typeof(T).Name}/Edit/{{0}}";

        private string GetEditUri(TKey key)
            => s_editUri != null ? string.Format(s_editUri, key.ToBase64()) : string.Empty;
        private async Task OnReadData(LoadDataArgs args)
        {
            _isLoading = true;
            if (Query != null)
            {
                if (args.Filters.Any())
                {
                    StringBuilder whereBuilder = new();
                    var properties = typeof(T).FetchProperties();
                    foreach (var filter in args.Filters)
                    {
                        var property = properties.First(x => x.Name == filter.Property);
                        foreach (var value in Values())
                        {
                            whereBuilder.Append(whereBuilder.Length <= 0 ? "x => " : " AndAlso ");
                            whereBuilder.Append(GetValidOperation(property.Name, value.Operator, value.Value, property.PropertyType.IsNumeric()));
                        }
                        IEnumerable<(object Value, FilterOperator Operator)> Values()
                        {
                            yield return (filter.FilterValue, filter.FilterOperator);
                            if (filter.SecondFilterValue != null || filter.FilterOperator == FilterOperator.IsNull)
                                yield return (filter.SecondFilterValue, filter.SecondFilterOperator);
                        }
                        string GetValidOperation(string name, FilterOperator op, object? value, bool isNumeric)
                        {
                            var stringedValue = value?.ToString();
                            if (!isNumeric)
                                stringedValue = $"\"{stringedValue}\"";
                            return op switch
                            {
                                FilterOperator.GreaterThan => $"x.{name} > {stringedValue}",
                                FilterOperator.LessThan => $"x.{name} < {stringedValue}",
                                FilterOperator.GreaterThanOrEquals => $"x.{name} >= {stringedValue}",
                                FilterOperator.LessThanOrEquals => $"x.{name} <= {stringedValue}",
                                FilterOperator.NotEquals => $"x.{name} != {stringedValue}",
                                FilterOperator.Contains => $"x.{name}.Contains({stringedValue})",
                                FilterOperator.DoesNotContain => $"!x.{name}.Contains({stringedValue})",
                                FilterOperator.EndsWith => $"!x.{name}.EndsWith({stringedValue})",
                                FilterOperator.StartsWith => $"!x.{name}.StartsWith({stringedValue})",
                                FilterOperator.IsNull => $"!x.{name} == null",
                                FilterOperator.IsNotNull => $"!x.{name} != null",
                                _ => $"x.{name} == {stringedValue}",
                            };
                        }
                    }
                }
                int actualPage = (args.Top.Value + args.Skip.Value) / PageSize;
                var page = await Query.PageAsync(actualPage, PageSize).NoContext();
                _totalItems = (int)page.TotalCount;
                _entities = page.Items.ToList();
            }
            _isLoading = false;
        }
        private string GetKey(Entity<T, TKey> entity)
            => entity!.Key!.GetType().IsPrimitive() ? entity.Key.ToString() : entity.Key.ToJson();
        private protected RenderFragment OpenEnumerableVisualizer(T? entity, BaseProperty property)
        {
            if (entity != null)
            {
                var value = property.Value(entity);
                if (value != null)
                {
                    var genericType = typeof(Visualizer<>).MakeGenericType(new[] { property.Self.PropertyType });
                    var frag = new RenderFragment(b =>
                    {
                        b.OpenComponent(1, genericType);
                        b.AddAttribute(2, nameof(Entity), value);
                        b.CloseComponent();
                    });
                    return frag;
                }
            }
            return null;
        }
        private void ShowTooltip(ElementReference elementReference, string value)
        {
            TooltipService.Open(elementReference, value);
        }
    }
}

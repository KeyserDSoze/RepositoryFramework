using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace RepositoryFramework.Web.Components.Standard
{
    public partial class QueryFilter
    {
        [Parameter]
        public PropertyUiSettings? PropertyUiSettings { get; set; }
        [Parameter]
        public required SearchValue SearchValue { get; set; }
        [Parameter]
        public required Action Search { get; set; }
        private bool? _booleanValue;
        private string? _stringValue { get; set; }
        private ValueBearer<DateTime>? _dateTime { get; set; }
        private ValueBearer<DateOnly>? _date { get; set; }
        private ValueBearer<decimal>? _number { get; set; }
        private IEnumerable<string>? _optionKeys { get; set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (!PropertyUiSettings.HasValues())
            {
                if (SearchValue.BaseProperty.AssemblyType == typeof(bool) || SearchValue.BaseProperty.AssemblyType == typeof(bool?))
                {
                    _booleanValue = SearchValue.Value.Cast<bool?>();
                }
                else if (SearchValue.BaseProperty.AssemblyType == typeof(DateTime) || SearchValue.BaseProperty.AssemblyType == typeof(DateTime?))
                {
                    _dateTime = SearchValue.Value != null ? SearchValue.Value.Cast<ValueBearer<DateTime>>() : new ValueBearer<DateTime>();
                }
                else if (SearchValue.BaseProperty.AssemblyType == typeof(DateOnly) || SearchValue.BaseProperty.AssemblyType == typeof(DateOnly?))
                {
                    _date = SearchValue.Value != null ? SearchValue.Value.Cast<ValueBearer<DateOnly>>() : new ValueBearer<DateOnly>();
                }
                else if (SearchValue.BaseProperty.AssemblyType.IsNumeric())
                {
                    _number = SearchValue.Value != null ? SearchValue.Value.Cast<ValueBearer<decimal>>() : new ValueBearer<decimal>();
                }
                else
                {
                    _stringValue = SearchValue.Value?.ToString();
                }
            }
            else
            {
                if (SearchValue.Value != null)
                    _optionKeys = SearchValue.Value as IEnumerable<string>;
            }
        }

        public void Contains(string? value)
        {
            _stringValue = value;
            if (_stringValue == null)
                SearchValue.Expression = null;
            else
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath}.Contains(\"{_stringValue}\")";
            SearchValue.Value = _stringValue;
            Search();
        }

        public void DateTimeSearch()
        {
            if (_dateTime?.Start != null && _dateTime?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_dateTime.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_dateTime.End}";
            else if (_dateTime?.Start != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_dateTime.Start}";
            else if (_dateTime?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_dateTime.End}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = _dateTime;
            Search();
        }

        public void DateSearch()
        {
            if (_date?.Start != null && _date?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}";
            else if (_date?.Start != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start}";
            else if (_date?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = _date;
            Search();
        }

        public void NumberSearch(decimal value, bool atStart)
        {
            if (atStart)
                _number.Start = value;
            else
                _number.End = value;
            if (_number?.Start != null && _number?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_number.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_number.End}";
            else if (_number?.Start != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_number.Start}";
            else if (_number?.End != null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_number.End}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = _number;
            Search();
        }

        public void BoolSearch()
        {
            if (_booleanValue == null)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} == null";
            else
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} == {_booleanValue}";
            SearchValue.Value = _booleanValue;
            Search();
        }

        public void MultipleChoices(object x)
        {
            StringBuilder builder = new();
            if (x is IEnumerable<string> ids)
            {
                foreach (var id in ids)
                {
                    if (builder.Length == 0)
                        builder.Append("x => ");
                    else
                        builder.Append(" OrElse ");
                    var value = PropertyUiSettings!.Values!.FirstOrDefault(x => x.Id == id)?.Value;
                    if (value.GetType().IsNumeric())
                        builder.Append($"x.{SearchValue.BaseProperty.NavigationPath} == {value}");
                    else
                        builder.Append($"x.{SearchValue.BaseProperty.NavigationPath} == \"{value}'\"");
                }
            }
            SearchValue.Expression = builder.ToString();
            SearchValue.Value = x;
            Search();
        }
    }
}

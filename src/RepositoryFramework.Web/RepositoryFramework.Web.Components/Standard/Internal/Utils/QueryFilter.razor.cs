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
        private const string FromLabel = "From";
        private const string ToLabel = "To";
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

        public void DateTimeSearch(object? value, bool atStart)
        {
            if (value is DateTime date)
            {
                if (atStart)
                    _dateTime.Start = date;
                else
                    _dateTime.End = date;
            }
            else
            {
                if (atStart)
                    _dateTime.Start = default;
                else
                    _dateTime.End = default;
            }
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

        public void DateSearch(object? value, bool atStart)
        {
            if (value is DateOnly date)
            {
                if (atStart)
                    _date.Start = date;
                else
                    _date.End = date;
            }
            else
            {
                if (atStart)
                    _date.Start = default;
                else
                    _date.End = default;
            }
            if (_date?.Start != default && _date?.End != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}";
            else if (_date?.Start != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start}";
            else if (_date?.End != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = _date;
            Search();
        }

        public void NumberSearch(object? value, bool atStart)
        {
            if (value is decimal number)
            {
                if (atStart)
                    _number.Start = number;
                else
                    _number.End = number;
            }
            else
            {
                if (atStart)
                    _number.Start = default;
                else
                    _number.End = default;
            }
            if (_number?.Start != default && _number?.End != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_number.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_number.End}";
            else if (_number?.Start != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_number.Start}";
            else if (_number?.End != default)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_number.End}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = _number;
            Search();
        }
        public void BoolSearch(object? value, bool emptyIsValid)
        {
            if (value == null && emptyIsValid)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} == null";
            else if (value is bool booleanValue)
                SearchValue.Expression = $"x => x.{SearchValue.BaseProperty.NavigationPath} == {booleanValue}";
            else
                SearchValue.Expression = null;
            SearchValue.Value = value;
            Search();
        }

        public void MultipleChoices(object? x)
        {
            if (x is ChangeEventArgs eventArgs)
            {
                StringBuilder builder = new();
                if (eventArgs.Value is IEnumerable<string> ids)
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
                            builder.Append($"x.{SearchValue.BaseProperty.NavigationPath} == \"{value}\"");
                    }
                }
                SearchValue.Expression = builder.ToString();
            }
            else
                SearchValue.Expression = null;
            SearchValue.Value = x;
            Search();
        }
    }
}

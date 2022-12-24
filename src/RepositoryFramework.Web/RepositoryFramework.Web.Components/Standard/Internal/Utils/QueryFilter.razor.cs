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
        public required ISearchValue SearchValue { get; set; }
        [Parameter]
        public required Action Search { get; set; }
        private bool? _booleanValue;
        private string? _stringValue { get; set; }
        private ValueBearer<DateTime>? _dateTime { get; set; }
        private ValueBearer<DateOnly>? _date { get; set; }
        private ValueBearer<decimal?>? _number { get; set; }
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
                    _number = SearchValue.Value != null ? SearchValue.Value.Cast<ValueBearer<decimal?>>() : new ValueBearer<decimal?>();
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
                SearchValue.UpdateLambda(null);
            else
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath}.Contains(\"{_stringValue}\")");
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
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_dateTime.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_dateTime.End}");
            else if (_dateTime?.Start != null)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_dateTime.Start}");
            else if (_dateTime?.End != null)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_dateTime.End}");
            else
                SearchValue.UpdateLambda(null);
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
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}");
            else if (_date?.Start != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} >= {_date.Start}");
            else if (_date?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} <= {_date.End}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = _date;
            Search();
        }

        public void NumberSearch(ChangeEventArgs args, bool atStart)
        {
            if (args.Value == null)
            {
                if (atStart)
                    _number.Start = default;
                else
                    _number.End = default;
            }
            else
            {
                var number = decimal.Parse(args.Value.ToString());
                if (atStart)
                    _number.Start = number;
                else
                    _number.End = number;
            }
            if (_number?.Start != default && _number?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {_number.Start} AndAlso x.{SearchValue.BaseProperty.Title} <= {_number.End}");
            else if (_number?.Start != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {_number.Start}");
            else if (_number?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} <= {_number.End}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = _number;
            Search();
        }
        public void BoolSearch(object? value, bool emptyIsValid)
        {
            if (value == null && emptyIsValid)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} == null");
            else if (value is bool booleanValue)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.NavigationPath} == {booleanValue}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = value;
            Search();
        }

        public void MultipleChoices(IEnumerable<LabelValueDropdownItem> items)
        {
            if (items.Any())
            {
                StringBuilder builder = new();
                foreach (var id in items.Select(x => x.Value))
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
                SearchValue.UpdateLambda(builder.ToString());
            }
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = items;
            Search();
        }
    }
}

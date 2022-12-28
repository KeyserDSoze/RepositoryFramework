using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private IEnumerable<string>? _optionKeys { get; set; }
        private string _booleanSelectedKey = "None";
        private static readonly IEnumerable<LabelValueDropdownItem> BooleanState = new List<LabelValueDropdownItem>()
        {
            new LabelValueDropdownItem
            {
                Id = "None",
                Label= "None",
                Value = null
            },
            new LabelValueDropdownItem
            {
                Id = "True",
                Label= "True",
                Value = true
            },
            new LabelValueDropdownItem
            {
                Id = "False",
                Label= "False",
                Value = false
            },
        };
        private static readonly IEnumerable<LabelValueDropdownItem> BooleanTriState = new List<LabelValueDropdownItem>()
        {
            new LabelValueDropdownItem
            {
                Id = "None",
                Label= "None",
                Value = null
            },
           new LabelValueDropdownItem
            {
                Id = "True",
                Label= "True",
                Value = true
            },
            new LabelValueDropdownItem
            {
                Id = "False",
                Label= "False",
                Value = false
            },
            new LabelValueDropdownItem
            {
                Id = "Null",
                Label= "Null",
                Value = null!
            },
        };
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (PropertyUiSettings.HasValues() && SearchValue.Value != null)
            {
                _optionKeys = SearchValue.Value as IEnumerable<string>;
            }
        }

        public void Contains(ChangeEventArgs args)
        {
            var value = args?.Value?.ToString();
            if (value == null || value == string.Empty)
                SearchValue.UpdateLambda(null);
            else
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title}.Contains(\"{value}\")");
            Search();
        }

        public void DateTimeSearch(ChangeEventArgs? args, bool atStart)
        {
            var dateTime = new ValueBearer<DateTime>();
            var value = args?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (atStart)
                    dateTime.Start = default;
                else
                    dateTime.End = default;
            }
            else
            {
                var date = DateTime.Parse(value);
                if (atStart)
                    dateTime.Start = date;
                else
                    dateTime.End = date;
            }
            if (dateTime?.Start != null && dateTime?.End != null)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {dateTime.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {dateTime.End}");
            else if (dateTime?.Start != null)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {dateTime.Start}");
            else if (dateTime?.End != null)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} <= {dateTime.End}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = dateTime;
            Search();
        }

        public void DateSearch(ChangeEventArgs? args, bool atStart)
        {
            var dateonly = new ValueBearer<DateOnly>();
            var value = args?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (atStart)
                    dateonly.Start = default;
                else
                    dateonly.End = default;
            }
            else
            {
                var date = DateOnly.Parse(value);
                if (atStart)
                    dateonly.Start = date;
                else
                    dateonly.End = date;
            }
            if (dateonly?.Start != default && dateonly?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {dateonly.Start} AndAlso x.{SearchValue.BaseProperty.NavigationPath} <= {dateonly.End}");
            else if (dateonly?.Start != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {dateonly.Start}");
            else if (dateonly?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} <= {dateonly.End}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = dateonly;
            Search();
        }

        public void NumberSearch(ChangeEventArgs args, bool atStart)
        {
            var number = new ValueBearer<decimal?>();
            var value = args?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (atStart)
                    number.Start = default;
                else
                    number.End = default;
            }
            else
            {
                var parsedNumber = decimal.Parse(args.Value.ToString()!);
                if (atStart)
                    number.Start = parsedNumber;
                else
                    number.End = parsedNumber;
            }
            if (number?.Start != default && number?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {number.Start} AndAlso x.{SearchValue.BaseProperty.Title} <= {number.End}");
            else if (number?.Start != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} >= {number.Start}");
            else if (number?.End != default)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} <= {number.End}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = number;
            Search();
        }
        public void BoolSearch(LabelValueDropdownItem item, bool emptyIsValid)
        {
            var value = item.Value;
            if (value == null && emptyIsValid)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} == null");
            else if (value is bool booleanValue)
                SearchValue.UpdateLambda($"x => x.{SearchValue.BaseProperty.Title} == {booleanValue}");
            else
                SearchValue.UpdateLambda(null);
            SearchValue.Value = value;
            _booleanSelectedKey = item.Id;
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
                        builder.Append($"x.{SearchValue.BaseProperty.Title} == {value}");
                    else
                        builder.Append($"x.{SearchValue.BaseProperty.Title} == \"{value}\"");
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

using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace RepositoryFramework.TagHelpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent ShowEntities<T, TKey>(this IHtmlHelper html,
            IStringLocalizer? localizer = null,
            Expression<Func<T, bool>>? prefiltering = null)
            where TKey : notnull
        {
            return null;
        }
    }
    [HtmlTargetElement("RepositoryQuery", Attributes = "", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RepositoryQueryHelper : TagHelper
    {


        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("culture")]
        public CultureInfo CultureInfo { get; set; }
        [HtmlAttributeName("canModify")]
        public bool CanModify { get; set; }
        [HtmlAttributeName("canDelete")]
        public bool CanDelete { get; set; }
        private string Id { get; } = $"rystem-repository-query-{Guid.NewGuid():N}";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ProcessAsync(context, output).ToResult();
        }
        private const string TableScript = "<script>$(document).ready(function() {{new TableRystem('{0}', '{1}').show();}});</script>";
        private const string Header = "<thead><tr>{0}</tr></thead>";
        private const string HeaderWithDelete = "<thead><tr>{0}<td></td></tr></thead>";
        private const string BodyElement = "<td>{0}</td>";
        private const string Trash = "<td style='cursor:pointer;' onclick=\"document.location = '{0}';\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></td>";
        private const string ValueElement = "{0}";
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            output.Attributes.Add("id", this.Id);
            //output.Content.AppendHtml(string.Format(this.CanDelete ? HeaderWithDelete : Header, string.Join("", Headers.Select(x => $"<td>{x}</td>"))));
            output.Content.AppendHtml("<tbody>");
            //foreach (NavigationValue navigationValue in Values)
            //{
            //    output.Content.AppendHtml($"<tr>");
            //    string valueBase = ValueElement;
            //    if (this.CanModify)
            //    {
            //        this.ModifyRequest.FurtherPath = navigationValue.Key;
            //        valueBase = $"<a style='cursor:pointer;' href='{this.ModifyRequest.GetUrl(ViewContext)}'>{{0}}</a>";
            //    }
            //    foreach (var element in navigationValue.Elements)
            //    {
            //        output.Content.AppendHtml(string.Format(BodyElement, string.Format(valueBase, element.Value)));
            //    }
            //    if (this.CanModify)
            //        output.Content.AppendHtml("</a>");
            //    if (this.CanDelete)
            //    {
            //        this.DeleteRequest.FurtherPath = navigationValue.Key;
            //        output.Content.AppendHtml(string.Format(Trash, this.DeleteRequest.GetUrl(ViewContext)));
            //    }
            //    output.Content.AppendHtml("</tr>");
            //}
            output.Content.AppendHtml("</tbody>");
            output.PostContent.AppendHtml(string.Format(TableScript, this.Id, GetLanguage()));
            return Task.CompletedTask;
        }
        private const string EmptyFunction = "undefined";

        private string GetLanguage()
        {
            switch (this.CultureInfo?.Name.ToLower().Split('-').First())
            {
                default:
                    return EmptyFunction;
                case "it":
                    return "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Italian.json";
                case "fr":
                    return "https://cdn.datatables.net/plug-ins/1.10.21/i18n/French.json";
                case "de":
                    return "https://cdn.datatables.net/plug-ins/1.10.21/i18n/German.json";
                case "es":
                    return "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json";
                case "pt":
                    return "https://cdn.datatables.net/plug-ins/1.10.21/i18n/Portuguese.json";
            }
        }
    }
}

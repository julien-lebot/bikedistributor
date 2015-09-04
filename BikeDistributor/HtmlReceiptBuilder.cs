using System.Collections.Generic;
using System.Linq;
using System.Text;
using BikeDistributor.ViewModels;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace BikeDistributor
{
    public class HtmlReceiptBuilder
    {
        private readonly string _key;
        private readonly IRazorEngineService _templateService;

        public static HtmlReceiptBuilder TestBuilder()
        {
            return new HtmlReceiptBuilder(
@"<html><body><h1>Order Receipt for @Model.Company</h1>
@if (Model.Lines.Any()) {
<ul>
@foreach(var line in Model.Lines)
{
<li>@line.Quantity x @line.Brand @line.Model = @line.Amount.FormatCurrency(@Model.Currency)</li>
}
</ul>
}
<h3>Sub-Total: @Model.SubTotal.FormatCurrency(@Model.Currency)</h3>
<h3>Tax: @Model.Tax.FormatCurrency(@Model.Currency)</h3>
<h2>Total: @Model.Total.FormatCurrency(@Model.Currency)</h2>
</body></html>",
                    "Demo"
            );
        }

        public HtmlReceiptBuilder(string template, string key)
        {
            // Removes all new lines, line feeds, tabs
            template = new string[]
            {
                "\n", "\r", "\t"
            }.Aggregate(template, (current, c) => current.Replace(c, string.Empty));

            _key = key;
            _templateService = RazorEngineService.Create(
                new TemplateServiceConfiguration() { Namespaces = new HashSet<string> { "BikeDistributor.Helpers", "System.Linq" } });
            _templateService.Compile(template, key, typeof(OrderViewModel));
        }

        public string GenerateReceipt(OrderViewModel order)
        {
            return _templateService.RunCompile(_key, typeof(OrderViewModel), order);          
        }
    }
}
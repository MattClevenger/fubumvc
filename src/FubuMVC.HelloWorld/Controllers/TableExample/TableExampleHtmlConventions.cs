using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.HelloWorld.Controllers.Products;
using FubuMVC.UI;
using HtmlTags;

namespace FubuMVC.HelloWorld.Controllers.TableExample
{
    public class TableExampleHtmlConventions : HtmlConventionRegistry
    {
        public TableExampleHtmlConventions()
        {
            Displays.IfPropertyIs<Product[]>().BuildBy(
                r => r.Get<ProductsTableGenerator>().GenerateTable(r.Value<Product[]>()));

            Displays.IfPropertyIs<ProductPart[]>().BuildBy(r =>
            {
                var partNums = r.Value<ProductPart[]>()
                               .Select(pp => pp.PartNum)
                               .ToArray();
                var partNumsString = string.Join(", ", partNums);
                return new HtmlTag("span").Text(partNumsString);
            });

        }

    }
}
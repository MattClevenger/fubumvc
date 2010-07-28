using System;
using System.Linq.Expressions;
using FubuCore;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using FubuMVC.HelloWorld.Controllers.Products;
using FubuMVC.UI.Tables;
using FubuMVC.UI.Tags;
using HtmlTags;

namespace FubuMVC.HelloWorld.Controllers.TableExample
{

    public static class TableExampleExtensions
    {
        public static HtmlTag ProductsTableFor<TModel>(this IFubuPage<TModel> page, Expression<Func<TModel, Product[]>> productsExpression)
            where TModel : class
        {
            var model = page.Model;
            var products = productsExpression.Compile().Invoke(model);
            var generator = new ProductsTableGenerator(page.Get<ITagGenerator<Product>>(), page.Get<IUrlRegistry>());
            var table = generator.GenerateTable(products);
            return table;
        }
    }

    public class ProductsTableGenerator : TableGenerator<Product>
    {
        public ProductsTableGenerator(ITagGenerator<Product> tags, IUrlRegistry urls)
            : base(tags)
        {
            AddColumnUsingTransformation(p => {
                        var ptir = new ProductsTableItemRequest();
                        var controllerUrl = urls.UrlFor(ptir);
                        controllerUrl = controllerUrl.AddQueryString(p, prod => prod.Code);
                        var link = new LinkTag(p.Code, controllerUrl);
                        return link;
                }, "Code");
            AddColumnWithDisplayFor(p => p.Name, "This is the name");
            AddColumnWithDisplayFor(p => p.Parts);
        }
    }
}
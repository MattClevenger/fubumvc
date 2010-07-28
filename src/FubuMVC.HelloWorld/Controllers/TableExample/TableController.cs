using System.Linq;
using FubuMVC.Core.View;
using FubuMVC.HelloWorld.Controllers.Products;

namespace FubuMVC.HelloWorld.Controllers.TableExample
{
    public class TableController
    {
        private Product[] _products = new[]
                               {
                                   new Product{Code="1111", Name="TV Remote Control", Parts = GetParts("1111")},
                                   new Product{Code="2222", Name="DVR Unit", Parts = GetParts("2222")},
                                   new Product{Code="3333", Name="HD Antenna", Parts = GetParts("3333")}
                               };


        public ProductsTableViewModel Query(ProductsTableRequest query)
        {
            return new ProductsTableViewModel { Products = _products };
        }

        public Product View(ProductsTableItemRequest query)
        {
            var product = _products.Where(p => p.Code == query.Code).FirstOrDefault();
            return product;
        }

        private static ProductPart[] GetParts(string numberRoot)
        {
            return new[]
                       {
                           new ProductPart(numberRoot + "- 1"),
                           new ProductPart(numberRoot + "- 2"),
                           new ProductPart(numberRoot + "- 3")
                       };
        }

    }

    public class ProductsTableRequest
    {
    }

    public class ProductsTableItemRequest
    {
        public string Code { get; set; }
    }

    public class ProductsTableViewModel
    {
        public Product[] Products { get; set; }
    }

    public class TableView : FubuPage<ProductsTableViewModel>
    {
    }

    public class TableItemView : FubuPage<Product>
    {
    }


}
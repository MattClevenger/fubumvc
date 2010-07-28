<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/Site.Master" Inherits="FubuMVC.HelloWorld.Controllers.TableExample.TableView" %>
<%@ Import Namespace="FubuMVC.HelloWorld.Controllers.Home" %>
<%@ Import Namespace="FubuMVC.UI.Tables" %>
<%@ Import Namespace="FubuMVC.HelloWorld.Controllers.TableExample" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%= this.LinkTo(new HomeInputModel()).Text("Home") %>
   
    <h1>This page illustrates usages of the TableGenerator class and its extension methods</h1>
        
    <h2>Using DisplayFor convention, which uses the ProductsTableGenerator class (see TableExampleHtmlConventions.cs).
        Also note that the ProductPart[] array was processed using its DisplayFor convention.</h2>
    <%= this.DisplayFor(m => m.Products) %>

    <h2>Using a custom extension method, that uses a class that extends TableGenerator, customized for Products</h2>
    <%= this.ProductsTableFor(m => m.Products) %>
    
    <h2>Generated manually using an extension method for easy to handle cases</h2>
    <%= this.TableFor(m => m.Products, p => p.Code, p => p.Name, p => p.Parts) %>

    <h2>Tweeked generation with a transformation on header row to add a class, then generated manually</h2>
    <%= this.TableGeneratorFor(m => m.Products, p => p.Code, p => p.Name, p => p.Parts)
            .AddHeaderRowTransformer(r => r.AddClass("header"))
            .GenerateTable(Model.Products) %>
</asp:Content>
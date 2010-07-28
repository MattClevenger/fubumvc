<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/Site.Master" Inherits="FubuMVC.HelloWorld.Controllers.TableExample.TableItemView" %>
<%@ Import Namespace="FubuMVC.HelloWorld.Controllers.Products"%>
<%@ Import Namespace="FubuMVC.HelloWorld.Controllers.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= this.LinkTo(new HomeInputModel()).Text("Home") %>
    <h2>Product</h2>
    <%= this.RenderPartial().Using(typeof(ProductPartial))%>
</asp:Content> 
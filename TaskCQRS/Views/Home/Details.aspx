<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<TaskCQRS.Domain.TaskItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<h2>Details:</h2>
Id: <%:Model.Id%><br />
Name: <%:Model.Name%><br />

<%: Html.ActionLink("Rename","Rename", new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Remove","Remove",new{Id=Model.Id, Version=Model.Version}) %><br />

</asp:Content>

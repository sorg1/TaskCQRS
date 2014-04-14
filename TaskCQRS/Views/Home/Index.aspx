<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TaskCQRS.Domain.TaskItemListDto>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All items:</h2>
    <ul><% foreach (var taskItemListDto in Model)
        {%><li>
            <%: Html.ActionLink("Name: " + taskItemListDto.Name,"Details",new{Id=taskItemListDto.Id}) %>
        </li>
    <%} %></ul>
    <%: Html.ActionLink("Add","Add") %>
</asp:Content>
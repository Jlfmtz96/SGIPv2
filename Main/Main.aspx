<%@ Page Title="Bienvenido" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="SGIPv2.Main.Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<style>

</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="container-fluid">
    <div class="relative" style="text-align: -webkit-center; background-color: rgb(241 245 249);">
        <img class="img-fluid" style="width: 50%;" src="https://wp.uaslp.mx/noticias/wp-content/uploads/sites/5/2023/03/281805687_417506837045017_2663146561332425394_n-1320x879.jpg" />
        <asp:Label ID="lblBienvenida" runat="server" Text="" CssClass="h1 absolute text-8xl text-white top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2" style="font-weight: 600;"></asp:Label>
        <h2 class="absolute text-3xl text-white bottom-4 left-1/2 -translate-x-1/2"  style="font-weight: 600;">Sistema de Indicadores de Posgrado</h2>
    </div>
</div>
</asp:Content>

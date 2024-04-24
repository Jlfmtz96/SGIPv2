<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SGIPv2.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="container-fluid">
      <div class="bg-gray-100 flex justify-center items-center h-3/4"> <!-- Adjust the height as needed -->
        <!-- Left: Image -->
         <div class="w-1/2  h-3/4 lg:h-full hidden lg:block">
            <img src="https://wp.uaslp.mx/noticias/wp-content/uploads/sites/5/2023/03/281805687_417506837045017_2663146561332425394_n-1320x879.jpg" alt="Placeholder Image" class="object-cover w-full h-full">
        </div>
        <!-- Right: Login Form -->
        <div class="lg:p-36 md:p-52 sm:20 p-8 w-full lg:w-1/2">
            <h1 class="text-2xl font-semibold mb-4">Iniciar sesión</h1>

            <form id="loginForm" runat="server">
                <!-- Username Input -->
                <div class="mb-4">
                    <label for="username" class="w-full border border-gray-300 rounded-md py-2 px-3 focus:outline-none focus:border-blue-500" >Usuario</label>
                    <asp:TextBox ID="tbUsuario" CssClass="form-control w-full border border-gray-300 rounded-md py-2 px-3 focus:outline-none focus:border-blue-500" runat="server" placeholder="Nombre de Usuario"></asp:TextBox>
                </div>
                <!-- Password Input -->
                <div class="mb-4">
                    <label for="password" class="w-full border border-gray-300 rounded-md py-2 px-3 focus:outline-none focus:border-blue-500" >Contraseña</label>
 <asp:TextBox ID="tbPassword" CssClass="form-control w-full border border-gray-300 rounded-md py-2 px-3 focus:outline-none focus:border-blue-500"  TextMode="Password" runat="server" placeholder="Contraseña"></asp:TextBox>                </div>
               
                 <div class="row">
                        <asp:Label runat="server" style="color: red;" ID="lblError"></asp:Label>
                 </div>
                <br />
                <!-- Login Button -->
 <asp:Button ID="BtnIngresar" CssClass="bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded-md py-2 px-4 w-full" runat="server" Text="Ingresar" OnClick="BtnIngresar_Click" />            </form>

        </div>
    </div>
</div>

</asp:Content>

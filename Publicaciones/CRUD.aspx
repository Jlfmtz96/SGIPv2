<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="SGIPv2.Publicaciones.CRUD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Publicaciones
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
        <script>
    function showErrorDiv() {
        $('#errorDiv').removeClass('hidden');
    }

    function showSuccessDiv() {
        $('#successDiv').removeClass('hidden');
    }
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
        <br />
    <div class="mx-auto" style="width:250px">
        <asp:Label runat="server" CssClass="h2" ID="lbltitulo"></asp:Label>
    </div>
    <br />
    <!-- Div de error -->
    <div id="errorDiv" class="hidden mb-2">
        <div class="flex items-center justify-center w-full">
            <div class="bg-red-500 py-4 px-8 rounded shadow-lg text-white text-sm">
                <asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
            </div>
        </div>
    </div>

    <!-- Div de éxito -->
    <div id="successDiv" class="hidden mb-2">
        <div class="flex items-center justify-center">
            <div class="bg-green-500 py-4 px-8 rounded shadow-lg text-white text-sm">
                <asp:Label runat="server" ID="lblSuccessMessage"></asp:Label>
            </div>
        </div>
    </div>
    <form runat="server" class="h-100 flex items-center justify-center" style="width: 100%;">
        <asp:ScriptManager runat="server" />
        <div class="space-y-12">
            <div class="grid grid-cols-1 gap-x-6 gap-y-8">
                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Id del Producto</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" Enabled="false" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbclave" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Titulo</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbtitulo" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de publicación</label>
                    <div class="mt-2 relative">
                        <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbfpub" style="width: 350px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="calExtPub" runat="server" TargetControlID="tbfpub" PopupButtonID="btnCalPub" />
                        <button id="btnCalPub" class="absolute right-0 top-0 bottom-0 h-full w-8 px-1 flex items-center justify-center" style="border: none; background: none;" runat="server">
                            <i class="fas fa-calendar"></i>
                        </button>
                    </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Tipo</label>
                  <div class="mt-2">
                    <asp:DropDownList runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="ddlTipo" style="width: 350px;">
                        <asp:ListItem Text="Artículo" Value="Artículo" />
                        <asp:ListItem Text="Memoria" Value="Memoria" />
                    </asp:DropDownList>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Lugar</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tblugar" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>
            </div>   
        
            <asp:Button runat="server" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer" ID="BtnCreate" Text="Agregar" Visible="false" OnClick="BtnCreate_Click" />
            <asp:Button runat="server" CssClass="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 hover:cursor-pointer" ID="BtnUpdate" Text="Editar" Visible="false" OnClick="BtnUpdate_Click" />
            <asp:Button runat="server" CssClass="btn btn-primary" ID="BtnDelete" Text="Eliminar" Visible="false" OnClick="BtnDelete_Click"/>
            <asp:Button runat="server" CssClass="hover:cursor-pointer text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700 " ID="BtnVolver" Text="Volver" Visible="true" OnClick="BtnVolver_Click" />
        </div>

    </form>
</asp:Content>

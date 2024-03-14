<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGIPv2.Pages.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Alumnos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <form runat="server" class="container mx-auto">
        <br />
        <div class="container mx-auto">
            <div class="flex justify-center">
                <h2 class="font-bold">Listado de registros</h2>
            </div>
            
        </div>
        <br />
        <div class="container mx-auto">
            <div class="flex justify-end">
                <div class="col-auto">
                    <asp:LinkButton ID="btnGenerarPDF" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-pdf' />" OnClick ="btnGenerarPDF_Click" />
                    <asp:LinkButton ID="btnGenerarExcel" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-excel' />" OnClick="btnGenerarExcel_Click"  />

                    <asp:Button runat="server" ID="BtnCreate" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer form-control" Text="Agregar" OnClick="BtnCreate_Click"/>
                </div>
            </div>
        </div>
        <br />
        <div class="container mx-auto">
            <div class="overflow-x-auto">
                <asp:GridView runat="server" ID="gvalumnos" class="table-auto w-full whitespace-no-wrap bg-white border border-gray-200 mx-auto text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="Opciones del administrador">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Ver" CssClass="py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 hover:cursor-pointer" ID="BtnRead" OnClick="BtnRead_Click"/>
                                <asp:Button runat="server" Text="Editar" CssClass="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 hover:cursor-pointer" ID="BtnUpdate" OnClick="BtnUpdate_Click"/>
                                <asp:Button runat="server" Text="Eliminar" CssClass="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900 hover:cursor-pointer" ID="BtnDelete" OnClick="BtnDelete_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</asp:Content>

﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGIPv2.Pages.Index" %>
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
                                    <div class="dropdown" style="position: relative; margin-right: auto; background-color:dimgray; border-radius:20px; padding:10px; color:white">
                                        <button class="btn dropdown-toggle" type="button" id="btnDropdown" aria-haspopup="true" aria-expanded="false">
                                            Seleccionar Columnas
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="btnDropdown" style="position: absolute; top: 100%; left: 0; background-color: dimgray; border-radius:20px;">
                                            <div class="dropdown-item" style=" padding: 8px;">
                                                <asp:CheckBox ID="chkColumn1" runat="server" Text="Clave UASLP" Checked="true" />
                                            </div>
                                            <div class="dropdown-item" style=" padding: 8px;">
                                                <asp:CheckBox ID="chkColumn2" runat="server" Text="Nombre" Checked="true" />
                                            </div>
                                            <div class="dropdown-item" style=" padding: 8px;">
                                                <asp:CheckBox ID="chkColumn3" runat="server" Text="Licenciatura" Checked="true" />
                                            </div>
                                        </div>
                                    </div>


                <asp:Label ID="lblResultsCount" runat="server" Text="" CssClass="mr-3" style="align-self: center;"></asp:Label>

                 <div>
                     <asp:TextBox ID="txtBusqueda" runat="server" style="border: 1px solid #CCCCCC; border-radius: 4px; padding: 6px 10px;"></asp:TextBox>
                     <asp:Button ID="btnBuscar" CssClass="focus:outline-none text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer form-control" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                 </div>
                <div class="col-auto">
                    <asp:LinkButton ID="btnGenerarPDF" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-pdf' />" OnClick ="btnGenerarPDF_Click" />
                    <asp:LinkButton ID="btnGenerarExcel" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-excel' />" OnClick="btnGenerarExcel_Click"  />

                    <asp:Button runat="server" ID="BtnCreate" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer form-control" Text="Agregar" OnClick="BtnCreate_Click"/>
                </div>
                <div>
                        
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

        <script>
            $(document).ready(function () {
                // Initially hide the dropdown items
                $(".dropdown-item").hide();

                // Toggle dropdown items on button click
                $("#btnDropdown").click(function () {
                    $(".dropdown-item").toggle();
                });

                // Function to hide/show columns based on checkbox state
                $("input[type='checkbox']").change(function () {
                    // Get the checkbox that triggered the change event
                    var chkBox = $(this);

                    // Get the column number
                    var columnIndex = chkBox.parent().index() +2; // Index of the parent div + 1

                    // Hide or show the entire column based on the checkbox state
                    if (chkBox.prop("checked")) {
                        $('table tr th:nth-child(' + columnIndex + ')').show();
                        $('table tr td:nth-child(' + columnIndex + ')').show();
                    } else {
                        $('table tr th:nth-child(' + columnIndex + ')').hide();
                        $('table tr td:nth-child(' + columnIndex + ')').hide();
                    }
                });


            });
        </script>

    </form>
</asp:Content>

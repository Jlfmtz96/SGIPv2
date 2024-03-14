<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGIPv2.Investigadores.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
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
    <div class="flex justify-between items-center">
         <div class="col-auto mr-4">

                
                    <label for="default-search" class="mb-2 text-sm font-medium text-gray-900 sr-only dark:text-white">Search</label>
                    <div class="relative">


                        <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                            <svg class="w-4 h-4 text-gray-500" aria-hidden="true" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                            </svg>
                        </div>
                        <input type="search" id="default-search" class="block w-72 p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Buscar"  />                        
                        
                   
                    </div>
          <span id="result-count"></span>

        </div>
        <div class="col-auto">
            <asp:LinkButton ID="btnGenerarPDF" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-pdf' />" OnClick="btnGenerarPDF_Click"/>
            <asp:LinkButton ID="btnGenerarExcel" runat="server" CssClass="bg-red text-xl mr-2" Text="<span class='far fa-file-excel' />" OnClick="btnGenerarExcel_Click" />
            <asp:Button runat="server" ID="BtnCreate" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer form-control" Text="Agregar" OnClick="BtnCreate_Click"/>
        </div>
    </div>
</div>

        <br />
        <div class="container mx-auto">
            <div class="overflow-x-auto">
                <asp:GridView runat="server" ID="gvinv" class="table-auto w-full whitespace-no-wrap bg-white border border-gray-200 mx-auto text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="Opciones del administrador">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Ver" CssClass="py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 hover:cursor-pointer" ID="BtnRead" OnClick="BtnRead_Click"/>
                                <asp:Button runat="server" Text="Editar" CssClass="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 hover:cursor-pointer" ID="BtnUpdate" OnClick="BtnUpdate_Click"/>
                                <asp:Button runat="server" Text="Eliminar" CssClass="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900 hover:cursor-pointer" ID="BtnDelete" OnClick="BtnDelete_Click"/>
                                <asp:CheckBox runat="server" ID="CheckBoxSelect" Text="Seleccionar" CssClass="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-900 hover:cursor-pointer" ClientIDMode="Static" onclick="toggleCheckboxAppearance(this)" />
                                </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

            <script type="text/javascript">
                function toggleCheckboxAppearance(checkbox) {
                    if (checkbox.checked) {
                        checkbox.parentElement.classList.remove("bg-red-700", "hover:bg-red-800", "focus:ring-red-300", "dark:bg-green-600", "dark:hover:bg-green-700", "dark:focus:ring-green-900");
                        checkbox.parentElement.classList.add("bg-green-600", "hover:bg-green-700", "focus:ring-green-900", "dark:bg-red-700", "dark:hover:bg-red-800", "dark:focus:ring-red-300");
                        checkbox.nextSibling.textContent = "Seleccionado";
                    } else {
                        checkbox.parentElement.classList.remove("bg-green-600", "hover:bg-green-700", "focus:ring-green-900", "dark:bg-red-700", "dark:hover:bg-red-800", "dark:focus:ring-red-300");
                        checkbox.parentElement.classList.add("bg-red-700", "hover:bg-red-800", "focus:ring-red-300", "dark:bg-green-600", "dark:hover:bg-green-700", "dark:focus:ring-green-900");
                        checkbox.nextSibling.textContent = "Seleccionar";
                    }
                }

                document.getElementById("default-search").addEventListener("input", function () {
                    var searchTerm = this.value.toLowerCase();
                    var rows = document.querySelectorAll("#<%= gvinv.ClientID %> tr");
                   var resultCount = 0;

                   rows.forEach(function (row, index) {
                       if (index === 0) { // Handle header row separately
                           row.style.display = ""; // Always display header row
                       } else {
                           var cells = row.querySelectorAll("td");
                           var matchFound = false;

                           cells.forEach(function (cell) {
                               if (cell.textContent.toLowerCase().includes(searchTerm)) {
                                   matchFound = true;
                               }
                           });

                           if (matchFound || searchTerm === '') { // Display row if match found or if search term is empty
                               row.style.display = "";
                               resultCount++;
                           } else {
                               row.style.display = "none";
                           }
                       }
                   });


                   document.getElementById("result-count").textContent = "Resultados mostrados: " + resultCount;
               });


            </script>

    </form>
</asp:Content>

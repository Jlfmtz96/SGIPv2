<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="SGIPv2.Pages.CRUD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    CRUD
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <br />
    <div class="mx-auto" style="width:250px">
        <asp:Label runat="server" CssClass="h2" ID="lbltitulo"></asp:Label>
    </div>
    <form runat="server" class="h-100 flex items-center justify-center" style="width: 100%;">
        <div class="space-y-12">
            <div class="grid grid-cols-1 gap-x-6 gap-y-8">
                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Clave del Alumno</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" Enabled="false" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbclave" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Nombre</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbnombre" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Apellido Paterno</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbappat" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Apellido Materno</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbapmat" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>

                <div class="col-span-full">
                  <label class="block text-sm font-medium leading-6 text-gray-900">Licenciatura</label>
                  <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tblicenciatura" style="width: 350px;"></asp:TextBox>
                  </div>
                </div>


            </div>   
            
            <asp:Button runat="server" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer" ID="BtnCreate" Text="Agregar" Visible="false" OnClick="BtnCreate_Click" OnClientClick="return validarFormulario();" />
            <asp:Button runat="server" CssClass="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 hover:cursor-pointer" ID="BtnUpdate" Text="Editar" Visible="false" OnClick="BtnUpdate_Click" OnClientClick="return validarFormulario();" />
            <asp:Button runat="server" CssClass="btn btn-primary" ID="BtnDelete" Text="Eliminar" Visible="false" OnClick="BtnDelete_Click"/>
            <asp:Button runat="server" CssClass="hover:cursor-pointer text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700 " ID="BtnVolver" Text="Volver" Visible="true" OnClick="BtnVolver_Click" />
        </div>

    </form>

        <!-- Modal -->
    <div id="myModal" class="hidden fixed inset-0 z-10 flex items-center justify-center overflow-x-hidden overflow-y-auto outline-none focus:outline-none">
      <div class="relative w-auto max-w-md mx-auto my-6">
        <!-- Contenido del modal -->
        <div class="relative flex flex-col bg-white border-2 border-gray-300 shadow-xl rounded-lg outline-none focus:outline-none">
          <!-- Cabecera del modal -->
          <div class="flex items-start justify-between p-5 border-b border-solid border-gray-300 rounded-t">
            <h3 class="text-3xl font-semibold">Mensaje</h3>
            <button id="modalCloseBtn" class="p-1 ml-auto bg-transparent border-0 text-black float-right text-3xl leading-none font-semibold outline-none focus:outline-none">×</button>
          </div>
          <!-- Contenido del modal -->
          <div class="relative p-6 flex-auto">
            <p class="my-4 text-gray-600 text-lg leading-relaxed">Por favor, complete todos los campos.</p>
            <ul id="missingFieldsList" class="list-disc pl-8"></ul>
          </div>
        </div>
      </div>
    </div>

    <script>
        // Función para mostrar el modal
        function showModal() {
            var modal = document.getElementById('myModal');
            modal.classList.remove('hidden');
            modal.classList.add('block');
        }

        // Función para ocultar el modal
        function hideModal() {
            var modal = document.getElementById('myModal');
            modal.classList.remove('block');
            modal.classList.add('hidden');
        }

        // Función de validación del formulario
        function validarFormulario() {
            var inputs = document.querySelectorAll('input[type="text"]');
            for (var i = 0; i < inputs.length; i++) {
                if (!inputs[i].value.trim()) {
                    showModal(); // Mostrar el modal si algún campo está vacío
                    return false; // Devolver false para evitar que se envíe el formulario
                }
            }
            return true; // Si todos los campos están completos, permitir que se envíe el formulario
        }

        // Configuración del botón de cerrar el modal
        var modalCloseBtn = document.getElementById('modalCloseBtn');
        if (modalCloseBtn) {
            modalCloseBtn.onclick = function () {
                hideModal(); // Ocultar el modal al hacer clic en el botón de cerrar
                return false; // Devolver false para evitar que se envíe el formulario
            };
        }
    </script>
</asp:Content>

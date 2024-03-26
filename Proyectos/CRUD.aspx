<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="SGIPv2.Proyectos.CRUD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
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
              <label class="block text-sm font-medium leading-6 text-gray-900">Clave</label>
              <div class="mt-2">
                <asp:TextBox runat="server" Enabled="false" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbclave" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Título</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbtitulo" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

             <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Protocolo</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbprotocolo" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Alcance</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbalcance" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Área</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbarea" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

            <div class="col-span-full">
                <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de inicio</label>
                <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbfinicio" style="width: 350px;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calExtInicio" runat="server" TargetControlID="tbfinicio" PopupButtonID="btnCalInicio" />
                    <asp:ImageButton runat="server" ID="btnCalInicio" CssClass="fa fa-calendar" />
                </div>
            </div>

            <div class="col-span-full">
                <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de fin</label>
                <div class="mt-2">
                    <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbffin" style="width: 350px;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calExtFin" runat="server" TargetControlID="tbffin" PopupButtonID="btnCalFin" />
                    <asp:ImageButton runat="server" ID="btnCalFin" CssClass="fa fa-calendar" />
                </div>
            </div>



               <div class="col-span-full">
                 <label class="block text-sm font-medium leading-6 text-gray-900">Registro de ética</label>
                 <div class="mt-2">
                   <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbregetica" style="width: 350px;"></asp:TextBox>
                 </div>
               </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Lugar de registro</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tblugar" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Cuerpo académico</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbca" style="width: 350px;"></asp:TextBox>
              </div>
            </div>
             
            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Financiamiento</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbfin" style="width: 350px;"></asp:TextBox>
              </div>
            </div>
            
            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Grado de postgrado</label>
              <div class="mt-2">
                <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbgradpost" style="width: 350px;"></asp:TextBox>
              </div>
            </div>

             <div class="col-span-full">
               <label class="block text-sm font-medium leading-6 text-gray-900">Comentarios</label>
               <div class="mt-2">
                 <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbcomentarios" style="width: 350px;"></asp:TextBox>
               </div>
             </div>

             <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Profesor responsable</label>
              <div class="mt-2">
                <asp:DropDownList runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbprofesores" style="width: 350px;">
                </asp:DropDownList>
              </div>
            </div>

            <div class="col-span-full">
              <label class="block text-sm font-medium leading-6 text-gray-900">Alumno(s) participantes</label>
              <div class="mt-2">
                <asp:DropDownList runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbalumnos" style="width: 350px;">
                </asp:DropDownList>
              </div>
            </div>


        </div>   
        
        <asp:Button runat="server" CssClass="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 hover:cursor-pointer" ID="BtnCreate" Text="Agregar" Visible="false" OnClick="BtnCreate_Click"/>
        <asp:Button runat="server" CssClass="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 hover:cursor-pointer" ID="BtnUpdate" Text="Editar" Visible="false"  OnClick="BtnUpdate_Click"/>
        <asp:Button runat="server" CssClass="btn btn-primary" ID="BtnDelete" Text="Eliminar" Visible="false" OnClick="BtnDelete_Click"/>
        <asp:Button runat="server" CssClass="hover:cursor-pointer text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700 " ID="BtnVolver" Text="Volver" Visible="true" OnClick="BtnVolver_Click"/>
    </div>

</form>
</asp:Content>

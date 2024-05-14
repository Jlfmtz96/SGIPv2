<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="SGIPv2.Proyectos.CRUD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function showErrorDiv() {
            $('#errorDiv').removeClass('hidden');
        }

        function showSuccessDiv() {
            $('#successDiv').removeClass('hidden');
        }
    </script>

    <script>
        function habilitarOtroInput() {
            var otroInput = document.getElementById('<%= otroInput.ClientID %>');
        var otroRadio = document.getElementById('<%= otro.ClientID %>');
            otroInput.disabled = !otroRadio.checked;

            if (otroRadio.checked) {
                otroInput.focus(); // Enfoca el elemento otroInput
            }
        }
    </script>

    <script>
        function habilitarPosgradoInput() {
            var especialidadInput = document.getElementById('<%= especialidadInput.ClientID %>');
        var masterInput = document.getElementById('<%= masterInput.ClientID %>');
        var doctorInput = document.getElementById('<%= doctorInput.ClientID %>');
        
        var especialidadRadio = document.getElementById('<%= especialidad.ClientID %>');
        var masterRadio = document.getElementById('<%= master.ClientID %>');
        var doctorRadio = document.getElementById('<%= doctor.ClientID %>');

            especialidadInput.disabled = !especialidadRadio.checked;
            masterInput.disabled = !masterRadio.checked;
            doctorInput.disabled = !doctorRadio.checked;

            if (especialidadRadio.checked) {
                especialidadInput.focus();
            } else if (masterRadio.checked) {
                masterInput.focus();
            } else if (doctorRadio.checked) {
                doctorInput.focus();
            }
        }
    </script>

    <script>
        function habilitarRedInput() {
            var redInput = document.getElementById('<%= redInput.ClientID %>');
        var nombreRed = document.getElementById('<%= nombreRed.ClientID %>');

            redInput.disabled = !nombreRed.checked;

            if (nombreRed.checked) {
                redInput.focus();
            }
        }
    </script>

    <script>
        function habilitarCamposCE() {
            var numeroInput = document.getElementById('<%= numeroInput.ClientID %>');
        var instInput = document.getElementById('<%= instInput.ClientID %>');
        var instOInput = document.getElementById('<%= instOInput.ClientID %>');
        var siCE = document.getElementById('<%= siCE.ClientID %>');

            numeroInput.disabled = !siCE.checked;
            instInput.disabled = !siCE.checked;
            instOInput.disabled = !siCE.checked;

            if (siCE.checked) {
                numeroInput.focus(); // Enfoca el elemento numeroInput
            }
        }
    </script>

    <script>
        function habilitarNombreInvCA() {
            var invCAInput = document.getElementById('<%= invCAInput.ClientID %>');
            var siInv = document.getElementById('<%= siInv.ClientID %>');

            invCAInput.disabled = !siInv.checked;

            if (siInv.checked) {
                invCAInput.focus(); // Enfoca el elemento invCAInput
            }
        }
    </script>

    <script>
        function habilitarNombreCA() {
            var caInput = document.getElementById('<%= caInput.ClientID %>');
        var caSi = document.getElementById('<%= caSi.ClientID %>');

            caInput.disabled = !caSi.checked;

            if (caSi.checked) {
                caInput.focus(); // Enfoca el elemento caInput
            }
        }
    </script>

    <script>
        function habilitarMedioInput() {
            var medioInput = document.getElementById('<%= medioInput.ClientID %>');
            var mediosSi = document.getElementById('<%= mediosSi.ClientID %>');
            medioInput.disabled = !mediosSi.checked;

            if (mediosSi.checked) {
                medioInput.focus(); // Enfoca el elemento medioInput
            }
        }
    </script>

    <script>
        function buscar() {
            var input, filter, listBox, option, i, txtValue;
            input = document.getElementById("searchInput");
            filter = input.value.toUpperCase();
            listBox = document.getElementById("<%= searchResults.ClientID %>");
            option = listBox.getElementsByTagName("option");
            var noResultsMessage = document.getElementById('noResultMessage');

            if (filter === "") {
                listBox.style.display = "none";
                noResultsMessage.style.display = "none";
                return;
            }

            var elementsFound = false;

            listBox.style.display = "block";
            for (i = 0; i < option.length; i++) {
                txtValue = option[i].text;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    option[i].style.display = "";
                    elementsFound = true;
                } else { 
                    option[i].style.display = "none";
                }
            }

            // Mostrar mensaje si no se encontraron elementos
            if (!elementsFound) {
                noResultsMessage.style.display = 'block';
            } else {
                noResultsMessage.style.display = 'none';
            }
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
<form runat="server" class="h-100" style="width: 100%;">
        <asp:ScriptManager runat="server" />
    <div class="space-y-12 container mx-auto">
        <div class="border-b border-gray-900/10 pb-12">
            <div class="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
                <div class:"sm:col-span-4">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Clave</label>
                    <div class="mt-2">
                        <div class="flex rounded-md shadow-sm ring-1 ring-inset ring-gray-300 focus-whitin:ring-2 focus:whitin:ring-indigo-600 sm:max-w-md">
                            <asp:TextBox runat="server" Enabled="false" CssClass="block flex-1 border-0 bg-transparent py-1.5 pl-1 text-gray-900 placeholder:text-gray-400 sm:text-sm sm:leading-6" ID="tbclave" style="width: 350px;"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="col-span-full">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Nombre del Proyecto</label>
                    <div class="mt-2">
                        <asp:TextBox runat="server" CssClass="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbtitulo" style="width: 350px;"></asp:TextBox>
                    </div>
                </div>

                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">Nivel del Proyecto</legend>
                    <div class="mt-6 flex gap-x-8">
                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="enfermeria" runat="server" name="nivel" value="enfermeria" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarOtroInput()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="enfermeria" class="font-medium text-gray-900">Enfermería</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="nutricion" runat="server" name="nivel" value="nutricion" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarOtroInput()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="nutricion" class="font-medium text-gray-900">Nutrición</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="otro" name="nivel" runat="server" value="otro" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarOtroInput()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="otro" class="font-medium text-gray-900">Otro:</label>
                                <input id="otroInput" runat="server" name="otroInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>
                    </div>
                </fieldset>

                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">Nivel de proyecto de Posgrado:</legend>
                    <div class="mt-6 flex gap-x-8">
                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="especialidad" runat="server" name="posgrado" value="especialidad" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarPosgradoInput()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="especialidad" class="font-medium text-gray-900">Especialidad:</label>
                                <input id="especialidadInput" runat="server" name="especialidadInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="master" runat="server" name="posgrado" value="master" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarPosgradoInput()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="master" class="font-medium text-gray-900">Maestría:</label>
                                <input id="masterInput" runat="server" name="masterInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="doctor" runat="server" name="posgrado" value="doctor" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarPosgradoInput()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="doctor" class="font-medium text-gray-900">Doctorado:</label>
                                <input id="doctorInput" runat="server" name="doctorInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="no" runat="server" name="posgrado" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarPosgradoInput()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="no" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                    </div>
                </fieldset>

                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">Proyecto de red de trabajo</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="redNo" runat="server" name="red" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarRedInput()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="redNo" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="nombreRed" runat="server" name="red" value="nombreRed" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarRedInput()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="nombreRed" class="font-medium text-gray-900">Nombre de la red:</label>
                                <input id="redInput" runat="server" name="redInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>
                    </div>
                </fieldset>

                <div class="sm:col-span-2 sm:col-start-1">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de inicio</label>
                    <div class="mt-2">
                        <asp:TextBox runat="server" TextMode="Date" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbfinicio" style="width: 100%;"></asp:TextBox>
                    </div>
                </div>

                <div class="sm:col-span-2">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de fin</label>
                    <div class="mt-2">
                        <asp:TextBox runat="server" TextMode="Date" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbffin" style="width: 100%;"></asp:TextBox>
                    </div>
                </div>

                <div class="col-span-full">
                    <label class="flex items-center text-sm font-medium leading-6 text-gray-900">
                        Docente responsable
                        <button type="button" class="ml-2 p-1 text-indigo-600 hover:text-indigo-800 focus:outline-none focus:text-indigo-800" id="btnSearch">
                            +
                        </button>
                    </label>
                    <div class="mt-2 hidden">
                        <asp:ListBox runat="server" ID="ListBox1" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" SelectionMode="Multiple" style="width: 350px;"></asp:ListBox>
                        <button type="button" id="removeButton" class="mt-2 bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded">
                            Quitar Seleccionados
                        </button>
                    </div>

                    <!-- Modal -->
                    <div id="myModal" class="modal hidden fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                        <div class="modal-dialog w-3/4 md:w-1/2 lg:w-2/3 xl:w-1/2 bg-white rounded-lg shadow-lg overflow-hidden">
                            <div class="modal-content p-4">
                                <div class="modal-header flex justify-between items-center border-b pb-2 mb-4 gap-x-6">
                                    <input type="text" id="searchInput" class="w-full px-4 py-2 border rounded-md" placeholder="Buscar..." oninput="buscar()">
                                    <button type="button" class="close text-gray-400 hover:text-gray-600 focus:outline-none focus:text-gray-600">
                                        <span>&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div id="searchResults" class="mt-2">
                                        <asp:ListBox ID="searchResults" runat="server" Rows="10" Width="100%" style="display: none"></asp:ListBox>
                                        <div id="noResultMessage" style="display:none;">
                                            <p>No se econtaron resultados</p>
                                            <a href="#">¿Deseas añadirlo?</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer flex justify-end">
                                    <button type="button" id="agregarButton" class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                                        Agregar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <script>
                        document.addEventListener("DOMContentLoaded", function () {
                            document.getElementById('btnSearch').addEventListener('click', function () {
                                var modal = document.getElementById('myModal');
                                modal.classList.remove('hidden');
                                document.body.classList.add('overflow-hidden'); // Agregar clase para bloquear el scroll

                                // Mostrar el ListBox
                                <%= searchResults.ClientID %>
                                var listBox = document.getElementById('<%= ListBox1.ClientID %>');
                                listBox.parentElement.classList.remove('hidden');
                            });

                            // Manejar el cierre del modal al hacer clic en el botón de cierre
                            document.querySelector('.modal .close').addEventListener('click', function () {
                                var modal = document.getElementById('myModal');
                                modal.classList.add('hidden');
                                document.body.classList.remove('overflow-hidden'); // Quitar clase para desbloquear el scroll
                            });

                            // Agregar funcionalidad para agregar elementos al ListBox
                            document.getElementById('agregarButton').addEventListener('click', function () {
                                var listBox = document.getElementById('<%= ListBox1.ClientID %>');
                                var searchResults = document.getElementById('<%= searchResults.ClientID %>');

                                // Obtener los elementos ya existentes en el ListBox
                                var existingElements = Array.from(listBox.options).map(function (option) {
                                    return option.value;
                                });

                                // Agregar elementos seleccionados al ListBox
                                for (var i = 0; i < searchResults.options.length; i++) {
                                    if (searchResults.options[i].selected) {
                                        var value = searchResults.options[i].value;
                                        if (!existingElements.includes(value)) {
                                            var option = document.createElement('option');
                                            option.text = searchResults.options[i].text;
                                            option.value = searchResults.options[i].value;
                                            listBox.add(option);
                                        }
                                    }
                                }

                                // Ocultar modal
                                var modal = document.getElementById('myModal');
                                modal.classList.add('hidden');
                                document.body.classList.remove('overflow-hidden'); // Quitar clase para desbloquear el scroll
                            });

                            // Agregar funcionalidad para quitar elementos del ListBox
                            document.getElementById('removeButton').addEventListener('click', function () {
                                var listBox = document.getElementById('<%= ListBox1.ClientID %>');
                                var selectedOptions = listBox.selectedOptions;

                                // Quitar elementos seleccionados del ListBox
                                for (var i = 0; i < selectedOptions.length; i++) {
                                    listBox.removeChild(selectedOptions[i]);
                                }
                            });
                        });
                    </script>
                </div>

                <div class="col-span-full">
                    <label class="flex items-center text-sm font-medium leading-6 text-gray-900">
                        Estudiantes participantes
                        <button type="button" class="ml-2 p-1 text-indigo-600 hover:text-indigo-800 focus:outline-none focus:text-indigo-800" onclick="showSearch()">
                            +
                        </button>
                    </label>
                    <div class="mt-2">
                        <asp:DropDownList runat="server" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="ddAlumnos" style="width: 350px;"></asp:DropDownList>
                    </div>
                </div>

                <!-- Registro del Comité de Ética -->
                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">Registro del Comité de Ética e Investigación de la Facultad/ otro Comité:</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="siCE" runat="server" name="ce" value="si" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarCamposCE()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="siCE" class="font-medium text-gray-900">Si</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="noCE" runat="server" name="ce" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarCamposCE()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="noCE" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3 ceInput">
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="numeroCE" class="font-medium text-gray-900">Número:</label>
                                <input id="numeroInput" runat="server" name="numeroInput" type="text" class="block w-full rounded-md border-0 pl-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" disabled/>
                            </div>
                        </div>

                    </div>

                    <div class="mt-2 ceInput">
                        <div class="text-sm leading-6 flex gap-x-2">
                            <label for="ce" class="font-medium text-gray-900">Nombre de Institución que otorga:</label>
                            <input id="instInput" runat="server" name="instInput" type="text" class="block rounded-md border-0 pl-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" disabled />
                        </div>
                    </div>

                    <div class="mt-2 ceInput">
                        <div class="text-sm leading-6 flex gap-x-2">
                            <label for="ce" class="font-medium text-gray-900">Otro:</label>
                            <input id="instOInput" runat="server" name="instOInput" type="text" class="block rounded-md border-0 pl-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" disabled />
                        </div>
                    </div>
                </fieldset>

                <!-- Fin Registro del Comité de Ética -->


                <!-- Lugar y fecha de aplicación -->
                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">Lugar de aplicación:</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="interno" runat="server" name="aplicacion" value="interno" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600"/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="interno" class="font-medium text-gray-900">Interno</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="externo" runat="server" name="aplicacion" value="externo" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600"/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="externo" class="font-medium text-gray-900">Externo</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="lugar" class="font-medium text-gray-900">Lugar:</label>
                                <input id="lugarInput" runat="server" name="lugarInput" type="text" class="block w-full rounded-md border-0 pl-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" />
                            </div>
                        </div>
                    </div>
                </fieldset>

                 <div class="col-span-2">
                    <label class="block text-sm font-medium leading-6 text-gray-900">Fecha de aplicación</label>
                    <div class="mt-2">
                        <asp:TextBox runat="server" TextMode="Date" CssClass="block w-full rounded-md border-0 px-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" ID="tbfechaAp" style="width: 100%;"></asp:TextBox>
                    </div>
                </div>

                <!-- Fin Lugar y fecha de aplicación -->



                <!-- Docente pertenece a CA -->
                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">El docente responsable pertenece a algún Cuerpo Académico o Grupo de Investigación:</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="noInv" runat="server" name="invCA" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarNombreInvCA()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="noInv" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="siInv" runat="server" name="invCA" value="si" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarNombreInvCA()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="siInv" class="font-medium text-gray-900">Si</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="nombreinvCA" class="font-medium text-gray-900">Nombre:</label>
                                <input id="invCAInput" runat="server" name="numeroInput" type="text" class="block w-full rounded-md border-0 pl-1.5 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" disabled/>
                            </div>
                        </div>

                    </div>
                </fieldset>
                <!-- Fin Docente pertenece a CA -->

                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">El proyecto pertenece a alguna Línea Generación de Conocimientos de algún CA:</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="caNo" runat="server" name="ca" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarNombreCA()/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="caNo" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="caSi" runat="server" name="ca" value="si" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick=habilitarNombreCA()/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="nombreCA" class="font-medium text-gray-900">Nombre:</label>
                                <input id="caInput" runat="server" name="caInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>
                    </div>
                </fieldset>

                <!-- Medios de difusión -->
                <fieldset class="col-span-full">
                    <legend class="text-sm font-semibold leading-6 text-gray-900">El proyecto se difundirá fuera de la facultad:</legend>
                    <div class="mt-6 flex gap-x-8">

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="mediosNo" runat="server" name="medios" value="no" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick="habilitarMedioInput()"/>
                            </div>
                            <div class="text-sm leading-6">
                                <label for="mediosNo" class="font-medium text-gray-900">No</label>
                            </div>
                        </div>

                        <div class="relative flex gap-x-3">
                            <div class="flex h-6 items-center">
                                <input id="mediosSi" runat="server" name="medios" value="si" type="radio" class="h-4 w-4 rounded border-b-gray-300 text-indigo-600 focus:ring-indigo-600" onclick="habilitarMedioInput()"/>
                            </div>
                            <div class="text-sm leading-6 flex gap-x-2">
                                <label for="nombreMedio" class="font-medium text-gray-900">Medio:</label>
                                <input id="medioInput" runat="server" name="medioInput" type="text" class="h-8 rounded pl-1.5 focus:ring-indigo-600" disabled/>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <!--Fin Medios de difusión -->

                <div class="col-span-full">
                   <label class="block text-sm font-medium leading-6 text-gray-900">Comentarios</label>
                   <div class="mt-2">
                       <textarea id="comments" runat="server" name="comments" rows="3" class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"></textarea>
                   </div>
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

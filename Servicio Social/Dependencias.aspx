<%@ Page Title="" Language="C#" MasterPageFile="~/Dependencias.Master" AutoEventWireup="true" CodeBehind="Dependencias.aspx.cs" Inherits="Servicio_Social.Dependencias1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
 <div class="container">
                   <div style="text-align: center">
                       <div class="form-group">
                             <h2 class="text-gray-900 mb-4" style="color:#2e5790">Menú principal de la Dependencia</h2>
                       </div>
                       <asp:Panel runat="server" ID="pnlBuscar" Visible="true" >
                            <div class="text-center">
                                <div  text-align: center"> Seleccione una opción:</div>
                                <asp:RadioButtonList ID="rbtnBuscar" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbtnBuscar_SelectedIndexChanged" CssClass="opcion-radio" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="A">Iniciar Sesión</asp:ListItem>
                                    <asp:ListItem Value="B">Registrar Dependencia</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="rbtnBuscar"
                                    Display="None" ErrorMessage="* Seleccione la forma en que desea buscar la Dependencia."
                                    SetFocusOnError="True" ValidationGroup="Buscar"></asp:RequiredFieldValidator>
                           </div>
                        </asp:Panel> 
                        <asp:Panel runat="server" ID="pnlIngreso" Visible="false" >
                          <div class="text-center">
                              <div  text-align: center"> Seleccione una opción:</div>
                              <asp:RadioButtonList ID="rbtnIngreso" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbtnIngreso_SelectedIndexChanged" CssClass="opcion-radio" RepeatDirection="Horizontal">
                                  <asp:ListItem Value="A">Perfil de la Dependencia</asp:ListItem>
                                  <asp:ListItem Value="B">Estudiantes Registrados</asp:ListItem>
                              </asp:RadioButtonList>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rbtnBuscar"
                                  Display="None" ErrorMessage="* Seleccione la forma en que desea buscar la Dependencia."
                                  SetFocusOnError="True" ValidationGroup="Buscar"></asp:RequiredFieldValidator>
                         </div>
                      </asp:Panel>      
                   </div>
                <br />
                 <asp:Panel runat="server" ID="Panel1" Visible="true" >
                     <div class="form-group inline-group">
                          <div class="input-group mb-3">
                              <input runat="server" type="text" class="tbEstilo" id="usuario" required="required" placeholder="Usuario"/>
                           </div> 
                         <br />
                         <div class="input-group mb-3">
                             <input runat="server" type="password" class="tbEstilo" id="inPassword" placeholder="Contraseña" required="required" />
                        </div>
                            <%-- <asp:Button runat="server" ID="btnMostrar" Text="Mostrar"  /--%>
                       </div>
                     <div class="containerbtnIniciar">
                    <div>
                        <asp:Button runat="server" ID="btnIniciar" CssClass="miBoton" onclick="btnIngresar_Click" Text="Ingresar" />
                    </div>
                    </div>
                    <div style="text-align: center">
                     <div  style="color: blue;"  > ¿Olvidó su contraseña? </div>
                   </div>
                         
                </asp:Panel>
                <asp:Panel ID="pnlRegistrarDependencia" runat="server" Visible="false">
                        <!-- Campos para Registrar Dependencia -->
                           <%--<div class="row">
                            <div class="col-sm-6">
                                <asp:Label ID="Label15" runat="server" AssociatedControlID="TextBox11" CssClass="label-derecha"> Nombre de usuario:</asp:Label>
                                <asp:TextBox ID="TextBox11" runat="server" ></asp:TextBox>
                            </div>
                            <div class="col-sm-6 style="margin-left: 15px;"">
                                <asp:Label ID="Label16" runat="server" AssociatedControlID="TextBox12" CssClass="label-derecha">Contraseña:</asp:Label>
                                <asp:TextBox ID="TextBox12"  runat="server" TextMode="Password" ></asp:TextBox>
                            </div>
                        </div>--%>
                                <table class="tabla-formulario">
                                <tr>
                                    <td>
                                            <asp:Label ID="lblNombreUsuario" runat="server" AssociatedControlID="txtNombreUsuario" CssClass="label-derecha"> Nombre de usuario:</asp:Label>
                                            <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                    </td>
                                    <td>
                                            <asp:Label ID="lblContrasena" runat="server" AssociatedControlID="txtContrasena" CssClass="label-derecha">Contraseña:</asp:Label>
                                                <asp:TextBox ID="txtContrasena" CssClass="textbox-estilo" runat="server" TextMode="Password" ></asp:TextBox>
                                                <div class="input-group-append">
                                                    <%--<button class="btn btn-outline-secondary" type="button" onclick="btnMostrar_Click">Mostrar</button>--%>
                                                </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="label-derecha">E-mail:</asp:Label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblConfirmarContraseña" runat="server" AssociatedControlID="txtConfirmarContraseña" CssClass="label-derecha">Confirmar contraseña:</asp:Label>
                                            <asp:TextBox ID="txtConfirmarContraseña" runat="server" TextMode="Password" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblUnidad" runat="server" AssociatedControlID="ddlUnidad" CssClass="label-derecha">Unidad:</asp:Label>
                                            <asp:DropDownList ID="ddlUnidad2" runat="server" CssClass="DropDownList-estilo">
                                            </asp:DropDownList>
                                            
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblOrganismo" runat="server" AssociatedControlID="ddlOrganismo" CssClass="label-derecha">Organismo:</asp:Label>
                                            <asp:DropDownList ID="ddlOrganismo" runat="server" CssClass="DropDownList-estilo">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblDependencia" runat="server" AssociatedControlID="txtDependencia" CssClass="label-derecha">Dependencia:</asp:Label>
                                            <asp:TextBox ID="txtDependencia" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblResponsable" runat="server" AssociatedControlID="txtResponsable" CssClass="label-derecha">Responsable:</asp:Label>
                                            <asp:TextBox ID="txtResponsable" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblAreaResponsable" runat="server" AssociatedControlID="txtAreaResponsable" CssClass="label-derecha">Área responsable:</asp:Label>
                                            <asp:TextBox ID="txtAreaResponsable" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono" CssClass="label-derecha">Teléfono:</asp:Label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblDomicilio" runat="server" AssociatedControlID="txtDomicilio" CssClass="label-derecha">Domicilio:</asp:Label>
                                            <asp:TextBox ID="txtDomicilio" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <asp:Label ID="lblCiudad" runat="server" AssociatedControlID="txtCiudad" CssClass="label-derecha">Ciudad:</asp:Label>
                                            <asp:TextBox ID="txtCiudad" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                 <div class="containerbtnRegistrar">
                    <asp:Button runat="server" ID="Button2" CssClass=" miBoton" Text="Registrar" />
                 </div>
                </asp:Panel>
        <!-- ESTE ES UN EJEMPLO A BORRAR LUEGO -->
         <asp:Panel ID="PnlEjemploModPerfil" runat="server" Visible="false">
         <!-- Campos para Registrar Dependencia -->
         <div class="row">
             <table class="tabla-formulario">
                 <tr>
                     <td>
                             <asp:Label ID="Label1" runat="server" AssociatedControlID="txtNombreUsuario" CssClass="label-derecha" > Nombre de usuario:</asp:Label>
                             <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox-estilo" value="Cecilia Santoyo Rodríguez"></asp:TextBox>
                     </td>
                     <td>
                             <asp:Label ID="Label2" runat="server" AssociatedControlID="txtContrasena" CssClass="label-derecha">Contraseña:</asp:Label>
                                 <asp:TextBox ID="TextBox2" CssClass="textbox-estilo" runat="server" TextMode="Password" value="11130020" ></asp:TextBox>
                                 <div class="input-group-append">
                                     <%--<button class="btn btn-outline-secondary" type="button" onclick="btnMostrar_Click">Mostrar</button>--%>
                                 </div>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label5" runat="server" AssociatedControlID="txtEmail" CssClass="label-derecha">E-mail:</asp:Label>
                             <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox-estilo" value="cecysantoyo000@gmail.com"></asp:TextBox>
                         </div>
                     </td>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label6" runat="server" AssociatedControlID="txtConfirmarContraseña" CssClass="label-derecha">Confirmar contraseña:</asp:Label>
                             <asp:TextBox ID="TextBox4" runat="server" TextMode="Password" CssClass="textbox-estilo" value="11130020"></asp:TextBox>
                         </div>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label7" runat="server" AssociatedControlID="ddlUnidad" CssClass="label-derecha">Unidad:</asp:Label>
                             <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="DropDownList-estilo">
                             </asp:DropDownList>
                             
                         </div>
                     </td>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label8" runat="server" AssociatedControlID="ddlOrganismo" CssClass="label-derecha">Organismo:</asp:Label>
                             <asp:DropDownList ID="DropDownList2" runat="server" CssClass="DropDownList-estilo">
                             </asp:DropDownList>
                         </div>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label9" runat="server" AssociatedControlID="txtDependencia" CssClass="label-derecha">Dependencia:</asp:Label>
                             <asp:TextBox ID="TextBox5" runat="server" CssClass="textbox-estilo" value="SERC-DISTRITO"></asp:TextBox>
                         </div>
                     </td>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label10" runat="server" AssociatedControlID="txtResponsable" CssClass="label-derecha">Responsable:</asp:Label>
                             <asp:TextBox ID="TextBox6" runat="server" CssClass="textbox-estilo" value="JESUS MANUEL SILVEYRA"></asp:TextBox>
                         </div>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label11" runat="server" AssociatedControlID="txtAreaResponsable" CssClass="label-derecha">Área responsable:</asp:Label>
                             <asp:TextBox ID="TextBox7" runat="server" CssClass="textbox-estilo" value="SERVICIOS"></asp:TextBox>
                         </div>
                     </td>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label12" runat="server" AssociatedControlID="txtTelefono" CssClass="label-derecha">Teléfono:</asp:Label>
                             <asp:TextBox ID="TextBox8" runat="server" CssClass="textbox-estilo" value="8712744155" ></asp:TextBox>
                         </div>
                     </td>
                 </tr>
                 <tr>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label13" runat="server" AssociatedControlID="txtDomicilio" CssClass="label-derecha">Domicilio:</asp:Label>
                             <asp:TextBox ID="TextBox9" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3" ></asp:TextBox>
                         </div>
                     </td>
                     <td>
                         <div class="form-group">
                             <asp:Label ID="Label14" runat="server" AssociatedControlID="txtCiudad" CssClass="label-derecha">Ciudad:</asp:Label>
                             <asp:TextBox ID="TextBox10" runat="server" CssClass="textbox-estilo" value="SALTILLO"></asp:TextBox>
                         </div>
                     </td>
                 </tr>
             </table>
         </div>
  <div class="containerbtnRegistrar">
     <asp:Button runat="server" ID="Button1" CssClass=" miBoton" Text="Guardar cambios" />
  </div>
 </asp:Panel>
 <!-- FIN DE ESTE ES UN EJEMPLO A BORRAR LUEGO -->
 <asp:Panel ID="pnlEstudiantesRegistrados" runat="server" Visible="false">
               <div class="form-group">
                 <div text-align: center"> A partir del estatus  "En espera", el Prestador de servicio social tiene 5 días para entrevistarse con el(la) responsable del programa. Una vez esta entrevista, puede asignarlo o eliminarlo del programa. </div>
                   <table class="tabla-formulario">
                    <tr>
                        <td>
                                <asp:Label ID="Label3" runat="server" AssociatedControlID="ddlPeriodo">Periodo:</asp:Label>
                                 <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="DropDownList-estilo">
                                 </asp:DropDownList>
                        </td>
                        <td>
                                <asp:Label ID="Label4" runat="server" AssociatedControlID="ddlEstatus">Estatus:</asp:Label>
                                <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="DropDownList-estilo">
                                </asp:DropDownList>
                        </td>
                    </tr> 
                   </table>
                 </div>
                <div class="gridview-container">
                    <asp:GridView ID="GridViewEstudiantes" runat="server" AutoGenerateColumns="true" OnRowDataBound="GridViewEstudiantes_RowDataBound">
                   <Columns>
                       <asp:TemplateField HeaderText="Evaluar Estudiante">
                        <ItemTemplate>
                            <div style="text-align: center;"> <!-- Centra el contenido de la celda -->
                                <asp:Button ID="btnOperacion" runat="server" Text="Evaluar" CssClass=" miBoton2" CommandName="Evaluar" CommandArgument='<%# Container.DataItemIndex %>' OnClick="btnEvaluar_Click"/>
                                <asp:Button ID="Button3" runat="server" Text="Liberar Prestador" CssClass=" miBoton" CommandName="Evaluar" CommandArgument='<%# Container.DataItemIndex %>' OnClick="btnLiberar_Click"/>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="column-padding" />
                    </asp:TemplateField>
                   </Columns>
                        
                        <%-- <Columns>
                           <asp:TemplateField HeaderText="Programa">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrograma" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Matrícula">
                                 <ItemTemplate>
                                     <asp:Label ID="lblMatricula" runat="server"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prestador">
                                 <ItemTemplate>
                                     <asp:Label ID="lblPrestador" runat="server"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estuatus">
                                 <ItemTemplate>
                                     <asp:Label ID="lblEstatus" runat="server"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                          <asp:TemplateField HeaderText="Fecha Estatus">
                             <ItemTemplate>
                                 <asp:Label ID="lblFechaEstatus" runat="server"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operaciones">
                            <ItemTemplate>
                                <asp:Button ID="btnOperacion" runat="server" Text="Operación" CommandName="Operacion" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>--%>
                    </asp:GridView>
                </div>
</asp:Panel>
<asp:Panel ID="pnlEvaluarEstudiante" runat="server" Visible="false">
     <div style="text-align: center">
         <div style="text-align: center"> INSTRUMENTO DE EVALUACIÓN DE LA DEPENDENCIA AL PRESTADOR </div>
         <div style="text-align: left; font-weight:bold;" > Prestador: ROCÍO MEDINA HURTADO </div>
         <br />
         <div style="text-align: center"> Conteste las siguientes preguntas según el valor que usted considere de acuerdo a la escala siguiente: </div>
         <div style="text-align: center"> 1.Pésimo       2.Deficiente     3.Suficiente    4.Adecuado    5.Excelente</div>
         <br />
     </div>
         <div class="row">
            <div class="col-sm-8" style="background-color:lavender;">
             <asp:Label ID="Label19" runat="server" AssociatedControlID="" style="font-weight:bold;">1.	¿En que medida el alumno está realizando las actividades para dar cumplimiento con el objeto del programa?:</asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:DropDownList ID="DropDownList3" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>  
            <div class="col-sm-8" style="background-color:lavender;">
              <asp:Label ID="Label15" runat="server" AssociatedControlID="" style="font-weight:bold;">2. ¿En que medida considera que el alumno cuenta con las habilidades necesarias para realizar las actividades comprendidas en el programa?</asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:DropDownList ID="DropDownList4" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
            <div class="col-sm-8" style="background-color:lavender;">
           <asp:Label ID="Label16" runat="server" AssociatedControlID="" style="font-weight:bold;">3. ¿En que medida considera que el alumno cuenta con los conocimientos necesarios para realizar las actividades comprendidas en el programa?</asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:DropDownList ID="DropDownList5" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
              <div class="col-sm-8" style="background-color:lavender;">
                <asp:Label ID="Label17" runat="server" AssociatedControlID="" style="font-weight:bold;">4. ¿En que medida considera que el alumno cuenta con la actitud necesaria para realizar las actividades comprendidas en el programa?</asp:Label>
              </div>
              <div class="col-sm-4">
                 <asp:DropDownList ID="DropDownList6" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
              </div>
             <div class="col-sm-8" style="background-color:lavender;">
              <asp:Label ID="Label18" runat="server" AssociatedControlID="" style="font-weight:bold;">5. ¿En que medida el alumno es puntual con el horario establecido?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList7" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
              <div class="col-sm-8" style="background-color:lavender;">
              <asp:Label ID="Label20" runat="server" AssociatedControlID="" style="font-weight:bold;">6. ¿En que medida el alumno cumple sus actividades en los tiempos establecidos?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList8" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
              <div class="col-sm-8" style="background-color:lavender;">
              <asp:Label ID="Label21" runat="server" AssociatedControlID="" style="font-weight:bold;">7. ¿En que medida le proporciona al alumno espacios físicos adecuados para desempeñas sus actividades asignadas al servicio social?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList9" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
               <div class="col-sm-8" style="background-color:lavender;">
              <asp:Label ID="Label22" runat="server" AssociatedControlID="" style="font-weight:bold;">8. ¿En que medida le brinda mobiliario y herramientas necesarias para desempeñas sus actividades?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList10" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
               <div class="col-sm-8" style="background-color:lavender;">
             <asp:Label ID="Label23" runat="server" AssociatedControlID="" style="font-weight:bold;">9. ¿En que medida el alumno hace uso adecuado del espacio y herramientas que se le proporcionan para realizar sus actividades?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList11" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
               <div class="col-sm-8" style="background-color:lavender;">
             <asp:Label ID="Label24" runat="server" AssociatedControlID="" style="font-weight:bold;">10. ¿En que medida atiende al alumno cuando requiere de alguna asesoría?</asp:Label>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList12" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
               <div class="col-sm-8" style="background-color:lavender;">
                 <div class="form-group">
                    <asp:Label ID="Label25" runat="server" AssociatedControlID="" style="font-weight:bold;">11. ¿Cómo es la relación de trabajo con el prestador de servicio social que se le asignó?</asp:Label>
                </div>
            </div>
            <div class="col-sm-4">
               <asp:DropDownList ID="DropDownList13" runat="server" CssClass="DropDownList-estilo"></asp:DropDownList>
            </div>
        <div class="col-sm-8" style="background-color:lavender;">
            <div class="form-group">
                <asp:Label ID="Label26" runat="server" AssociatedControlID="" style="font-weight:bold;">12. ¿Cuáles son las necesidades que usted como responsable del programa de servicio social detecta que hacen falta Enel perfil del alumno asignado a su dependencia?</asp:Label>
             </div>
         </div>
         <div class="col-sm-4">
               <asp:TextBox ID="TextBox15" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
         </div>
        <div class="col-sm-8" style="background-color:lavender;">
            <div class="form-group">
                <asp:Label ID="Label32" runat="server" AssociatedControlID="" style="font-weight:bold;">13. ¿Qué características requiere el prestador de servicio para el desempeño de sus actividades?</asp:Label>
             </div>
        </div>
 <div class="col-sm-4">
       <asp:TextBox ID="TextBox16" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
 </div>
       
 </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlPDF" Visible="false">
<div style="text-align: center">
<table class="tabla-formulario">
    <tr>
        <td>
                <asp:Label ID="Label27" runat="server" AssociatedControlID="" CssClass="label-derecha"> Prestador:</asp:Label>
                <asp:TextBox ID="TextBox11" runat="server" CssClass="textbox-estilo" value ="JENNIFER MORALES GARCÍA"></asp:TextBox>
        </td>
        <td>
                <asp:Label ID="Label28" runat="server" AssociatedControlID="" CssClass="label-derecha"> Matrícula:</asp:Label>
                <asp:TextBox ID="TextBox12" runat="server" CssClass="textbox-estilo" value ="21343236"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label29" runat="server" AssociatedControlID="" CssClass="label-derecha"> Escuela/Facultad:</asp:Label>
            <asp:TextBox ID="TextBox13" runat="server" CssClass="textbox-estilo" value ="FACULTAD DE CIENCIAS DE LA ADMINISTRACIÓN"></asp:TextBox>
        </td>
        <td>
            <div class="form-group">
                <asp:Label ID="Label30" runat="server" AssociatedControlID="" CssClass="label-derecha"> Carrera:</asp:Label>
                <asp:TextBox ID="TextBox14" runat="server" CssClass="textbox-estilo" value ="LICENCIADO DE ADMINISTRACION DE EMPRESAS"></asp:TextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="form-group">
                <asp:Label ID="Label31" runat="server" AssociatedControlID="" CssClass="label-derecha"> Programa:</asp:Label>
                <asp:TextBox ID="TextBox21" runat="server" CssClass="textbox-estilo" value ="ADMINISTRACIÓN DE INVENTARIOS CICLICOS"></asp:TextBox>
            </div>
        </td>
        <td>
            <div class="form-group">
               <%--<<asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
               <asp:Label ID="lblSelectedDate" runat="server" Text=""></asp:Label>--%>
        
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="form-group">
               <%--<asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
            <asp:Label ID="Label32" runat="server" Text=""></asp:Label>--%>
            </div>
        </td>
    </tr>
</table>
    <div class="form-group">
        <iframe src="SERVICIOSOCIAL.pdf" style="width: 80%; height: 600px; border: none;"></iframe>
    </div>
</div>
</asp:Panel>
 </div>        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

</asp:Content>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Web.UI.WebControls.WebParts;


namespace Servicio_Social
{

    public class GlobalConstants
    {
        public static string SQL { get { return ConfigurationManager.ConnectionStrings["SQL"].ConnectionString; } }
        public static string ORA { get { return ConfigurationManager.ConnectionStrings["ORA"].ConnectionString; } }
    }


    public class MensajesCorreo
    {
        public string tipoTexto(string tipo)
        {
            string _body = "";
            switch (tipo)
            {
                case "1":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Registro Exitoso</h2>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Nos complace informarle que ha sido registrado exitosamente en nuestro sistema.</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para completar el proceso de autorización, le solicitamos que valide los datos proporcionados dentro de las próximas 48 horas. Por favor, póngase en contacto con nuestro equipo de validación llamando a los siguientes números telefónicos:</p>" +
                        "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Una vez que se haya completado el proceso de validación, recibirá un correo electrónico de confirmación.</p>" +
                        "<footer style=\"text-align: center; color: #999999; font-style: italic; font-size: 14px;\">¡Gracias por registrarse!</footer>" +
                        "<div style=\"text-align: center; margin-top: 20px;\">" +
                    "</body></html>";
                    break;

                case "2":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Dependencia no autorizada</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Al parecer hay una discrepancia con sus datos registrados</p>" +
                         "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Por favor contacte al departamento de Servicio Social para su más información:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "11":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Dependencia autorizada</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Su información ha sido verificada y autorizada</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Puede continuar con el registro de Programas utilizando su correo electrónico y contraseña ingresada en su registro.</p>" +
                         "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para cualquier aclaración comunicarse al teléfono correspondiente:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "3":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Programa autorizado</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Su información ha sido verificada y autorizada.</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para cualquier aclaración comunicarse al teléfono correspondiente:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "4":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Programa no autorizado</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Al parecer hay una discrepancia con sus datos registrados.</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para cualquier aclaración comunicarse al teléfono correspondiente:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "5":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Alumno autorizado - Servicio Social</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Su usuario ha sido autorizado exitósamente, ya puede ingresar a la página para hacer su selección de Programa para realizar el servicio social.</p>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">http://www.serviciosocial.uadec.mx/Home.aspx</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para cualquier aclaración comunicarse al teléfono correspondiente:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "6":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Alumno no autorizado - Servicio Social</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Al parecer hay una discrepancia con sus datos registrados.</p>" +
                        "<p style=\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Para cualquier aclaración comunicarse al teléfono correspondiente:</p>" +
                         "<ul style=\"list-style-type: none; padding-left: 0; margin-bottom: 20px;\">" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Saltillo:</span> <a href=\"tel:8444124477\" style=\"color: #007bff; text-decoration: none;\">844 412 44 77</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Norte:</span> <a href=\"tel:8666496057\" style=\"color: #007bff; text-decoration: none;\">866 649 60 26</a></li>" +
                        "<li style=\"margin-bottom: 10px;\"><span style=\"color: #007bff; font-weight: bold;\">Unidad Torreón:</span> <a href=\"tel:8717293208\" style=\"color: #007bff; text-decoration: none;\">871 729 32 08</a></li>" +
                        "</ul>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                case "23":
                    _body = "<html>" +
                        "<body>" +
                        "<h2 style=\"color: #333333; font-size: 24px; margin-bottom: 20px;\">Alumno ha seleccionado un programa - Servicio Social</h2>" +
                        "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">Un alumno se ha registrado a tu programa. Verifica en tu perfil de Dependencias, en el apartado de Alumnos registrados para su autorización.</p>" +
                       "<p style =\"color: #666666; font-size: 16px; margin-bottom: 20px;\">(Cuenta con 5 días hábiles para revisarlo).</p>" +
                        "<br/><br/><br/><em>No es necesario responder este correo electrónico.</em>" +
                        "</body></html>";
                    break;
                default:
                    _body = "";
                    break;

            }
            return _body;
        }
    }



    public class SeguridadUtils
    {
        private static readonly byte[] clave = Encoding.UTF8.GetBytes("S3rv1c1050c14lCg"); // Clave secreta para encriptación

        public static string Encriptar(string textoPlano)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = clave;
                aes.GenerateIV(); // Generar un vector de inicialización único

                ICryptoTransform encriptador = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                    {
                        byte[] textoBytes = Encoding.UTF8.GetBytes(textoPlano);
                        cs.Write(textoBytes, 0, textoBytes.Length);
                    }
                    byte[] iv = aes.IV;
                    byte[] textoCifrado = ms.ToArray();
                    byte[] textoCifradoIV = new byte[iv.Length + textoCifrado.Length];
                    Buffer.BlockCopy(iv, 0, textoCifradoIV, 0, iv.Length);
                    Buffer.BlockCopy(textoCifrado, 0, textoCifradoIV, iv.Length, textoCifrado.Length);
                    return Convert.ToBase64String(textoCifradoIV);
                }
            }
        }

        public static string Desencriptar(string textoEncriptado)
        {
            byte[] textoCifradoIV = Convert.FromBase64String(textoEncriptado);

            using (Aes aes = Aes.Create())
            {
                aes.Key = clave;
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] textoCifrado = new byte[textoCifradoIV.Length - iv.Length];
                Buffer.BlockCopy(textoCifradoIV, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(textoCifradoIV, iv.Length, textoCifrado, 0, textoCifrado.Length);
                aes.IV = iv;

                ICryptoTransform desencriptador = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(textoCifrado))
                {
                    using (CryptoStream cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public bool verificar(string usuario, string contraseñaIngresada)
        {
            // Recuperar la contraseña almacenada en la base de datos para el usuario especificado
            string contraseñaAlmacenada = ObtenerContraseñaDesdeBaseDeDatos(usuario);

            // Verificar si la contraseña ingresada coincide con la almacenada
            string contraseñaEncriptada = SeguridadUtils.Encriptar(contraseñaIngresada);
            return contraseñaAlmacenada == contraseñaEncriptada;
        }

        private string ObtenerContraseñaDesdeBaseDeDatos(string usuario)
        {
            // Implementa la lógica para recuperar la contraseña desde tu base de datos
            // Este método es ficticio y debe ser reemplazado por tu propia lógica de acceso a datos
            string contraseñaAlmacenada = "hashDeLaContraseña"; // Obtén la contraseña desde la base de datos
            return contraseñaAlmacenada;
        }
    }



}
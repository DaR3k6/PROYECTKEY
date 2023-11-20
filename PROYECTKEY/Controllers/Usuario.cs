using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROYECTKEY.Modelo;
using System;
using System.Net;
using System.Text;
namespace PROYECTKEY.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios y vendedores.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class Usuario : Controller
    { /// <summary>
      /// Método para registrar un nuevo usuario.
      /// </summary>
      /// <param name="nombreUsuario">Nombre del usuario.</param>
      /// <param name="apellido">Apellido del usuario.</param>
      /// <param name="email">Correo electrónico del usuario.</param>
      /// <param name="password">Contraseña del usuario.</param>
      /// <param name="idRol">Identificador del rol del usuario.</param>
      /// <returns>Respuesta HTTP indicando el éxito o fracaso del registro.</returns>
      
        [HttpPost]
        [Route("usuario/registrarUsuario")]
        public ContentResult RegistrarUsuario(string nombreUsuario, string apellido, string email, string password, int idRol)
        {
            string respuesta = ClaseUsuario.RegistrarUsuario(nombreUsuario, apellido, email, password, idRol);

            dynamic jsonResponse = JsonConvert.DeserializeObject(respuesta);

            if (jsonResponse != null && jsonResponse.status == true)
            {
                // Registro exitoso
                var json = JsonConvert.SerializeObject(jsonResponse);
                return Content(json, "application/json");
            }
            else
            {
                // Error en el registro
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var json = JsonConvert.SerializeObject(jsonResponse);
                return Content(json, "application/json");
            }
        }

        /// <summary>
        /// Registra un nuevo vendedor.
        /// </summary>
        /// <param name="documento">Número de documento del vendedor.</param>
        /// <param name="nombre">Nombre del vendedor.</param>
        /// <param name="fechaNacimiento">Fecha de nacimiento del vendedor.</param>
        /// <param name="usuarioId">ID del usuario asociado al vendedor.</param>
        /// <returns>Respuesta de registro.</returns>
        [HttpPost]
        [Route("vendedor/registrarVendedor")]
        public ContentResult registrarVendedor(string documento, string nombre, DateTime fechaNacimiento, int usuarioId)
        {
            string respuesta = ClaseUsuario.RegistrarVendedor(documento, nombre, fechaNacimiento, usuarioId);


            dynamic jsonResponse = JsonConvert.DeserializeObject(respuesta);


            if (jsonResponse != null && jsonResponse.status == true)
            {
                // Registro exitoso
                var json = JsonConvert.SerializeObject(jsonResponse);
                return Content(json, "application/json");
            }
            else
            {
                // Error en el registro
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var json = JsonConvert.SerializeObject(jsonResponse);
                return Content(json, "application/json");
            }

        }

        /// <summary>
        /// Realiza el inicio de sesión de un usuario.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Respuesta de inicio de sesión.</returns>
        [HttpPost]
        [Route("usuario/login")]
        public ContentResult usuarioLogin( string email, string password)
        {
            dynamic respuesta = ClaseUsuario.LoginUsuario(email, password);
                
            if (respuesta != null && respuesta.status == true)
            {
                // Código 200 si el inicio de sesión es exitoso
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json, "application/json");
            }
            else
            {
                // Código 400 si hay un error en el inicio de sesión
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json, "application/json");
            }
        }

        /// <summary>
        /// Obtiene información detallada de un usuario.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Información detallada del usuario.</returns>
        [HttpGet]
        [Route("informacion/usuario/{id}")]
        public ContentResult InformacionUsuario(int id)
        {
            dynamic respuesta = ClaseUsuario.InformacionUsuario(id);
 
            if (respuesta != null && respuesta.status == true)
            {
                // Código 200 si el inicio de sesión es exitoso
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json, "application/json");
            }
            else
            {
                // Código 400 si hay un error en el inicio de sesión
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var json = JsonConvert.SerializeObject(respuesta);
                return Content(json, "application/json");
            }
        }
       

    }
}

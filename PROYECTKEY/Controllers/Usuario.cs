using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROYECTKEY.Modelo;

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
        public HttpResponseMessage registrarUsuario(string nombreUsuario, string apellido, string email, string password, int idRol)
        {
            string respuesta = ClaseUsuario.RegistrarUsuario(nombreUsuario, apellido, email, password, idRol);

            // Crea una respuesta HTTP
            var response = new HttpResponseMessage();

            if (respuesta.Contains("éxito"))
            {
                // Código 200 si el registro es exitoso
                response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                // Código 400 si hay un error en el registro
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }

            // Agrega el contenido JSON a la respuesta
            response.Content = new StringContent(respuesta, System.Text.Encoding.UTF8, "application/json");

            return response;
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
        public HttpResponseMessage registrarVendedor(string documento, string nombre, DateTime fechaNacimiento, int usuarioId)
        {
            string respuesta = ClaseUsuario.RegistrarVendedor(documento, nombre, fechaNacimiento, usuarioId);

            // Crea una respuesta HTTP
            var response = new HttpResponseMessage();

            if (respuesta.Contains("éxito"))
            {
                // Código 200 si el registro es exitoso
                response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                // Código 400 si hay un error en el registro
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }

            // Agrega el contenido JSON a la respuesta
            response.Content = new StringContent(respuesta, System.Text.Encoding.UTF8, "application/json");

            return response;
        }

        /// <summary>
        /// Realiza el inicio de sesión de un usuario.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Respuesta de inicio de sesión.</returns>
        [HttpPost]
        [Route("usuario/login")]
        public HttpResponseMessage usuarioLogin(string email, string password)
        {
            dynamic respuesta = ClaseUsuario.LoginUsuario(email, password);

            // Crea una respuesta HTTP
            var response = new HttpResponseMessage();

            if (respuesta != null && respuesta.status == true)
            {
                // Código 200 si el inicio de sesión es exitoso
                response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                // Código 400 si hay un error en el inicio de sesión
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }

            // Agrega el contenido JSON a la respuesta
            response.Content = new StringContent(JsonConvert.SerializeObject(respuesta), System.Text.Encoding.UTF8, "application/json");

            return response;
        }

        /// <summary>
        /// Obtiene información detallada de un usuario.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Información detallada del usuario.</returns>
        [HttpGet]
        [Route("informacion/usuario/{id}")]
        public HttpResponseMessage InformacionUsuario(int id)
        {
            dynamic respuesta = ClaseUsuario.InformacionUsuario(id);

            // Crea una respuesta HTTP
            var response = new HttpResponseMessage();
            
            if (respuesta != null)
            {
                // Código 200 si se obtiene la información correctamente
                response.StatusCode = System.Net.HttpStatusCode.OK; 
            }
            else
            {
                // Código 400 si hay un error en la obtención de información
                response.StatusCode = System.Net.HttpStatusCode.BadRequest; 
            }

            // Agrega el contenido JSON a la respuesta
            response.Content = new StringContent(JsonConvert.SerializeObject(respuesta), System.Text.Encoding.UTF8, "application/json");

            return response;
        }
       

    }
}

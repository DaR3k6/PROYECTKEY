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
    /// Controlador para la gestión de usuarios
    /// </summary>
    [ApiController]
    [Route("api")]
    public class Usuario : ControllerBase
    {
        /// <summary>
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
            try
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
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al registrar el usuario: {e.Message}" }),
                    ContentType = "application/json"
                };
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
        public ContentResult usuarioLogin(string email, string password)
        {
            try

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
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al logarse {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }


        /// <summary>
        /// Actualizar el usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombreUsuario"></param>
        /// <param name="apellido"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("usuario/actualizarUsuario/{id}")]
        public ContentResult ActualizarUsuario(int id, string nombreUsuario, string apellido, string email, string password, int idRol)
        {
            try
            {
                // Llama al modelo para agregar o actualizar el producto
                dynamic respuesta = ClaseUsuario.ActualizarUsuario(id, nombreUsuario, apellido, email, password, idRol);

                // Crea una respuesta HTTP
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Código 200 si el registro es exitoso
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Código 400 si hay un error en el registro
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    response.ContentType = "application/json";
                }

                return response;
            }
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al actualizar el producto: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }


        /// <summary>
        /// Obtiene información detallada de un usuario.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Información detallada del usuario.</returns>
        [HttpGet]
        [Route("usuario/informacion/{id}")]
        public ContentResult InformacionUsuario(int id)
        {
            try
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
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al traer la informacion del usuario: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Obtiene información detallada de todos los usuario.
        /// </summary>
        /// <returns>Información detallada del usuario.</returns>
        [HttpGet]
        [Route("usuario/ListarTodosUsuarios")]
        public ContentResult ListarTodosUsuarios()
        {
            try
            {
                dynamic respuesta = ClaseUsuario.ListarTodosUsuarios();

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
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al traer los usuarios: {e.Message}" }),
                    ContentType = "application/json"
                };
            }


        }


        /// <summary>
        /// Obtiene información detallada de todos los roles.
        /// </summary>
        /// <returns>Información detallada del usuario.</returns>
        [HttpGet]
        [Route("usuario/ListarTodosRoles")]
        public ContentResult TraerRoles()
        {
            try
            {
                dynamic respuesta = ClaseUsuario.TraerRoles();

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
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al traer los roles: {e.Message}" }),
                    ContentType = "application/json"
                };
            }

        }

        /// <summary>
        /// Elimina todo el Usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("usuario/eliminar/{idUsuario}")]
        public ContentResult EliminarUsuario(int idUsuario)
        {
            try
            {
                dynamic respuesta = ClaseUsuario.EliminarUsuarioId(idUsuario);

                // Crea una respuesta HTTP
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Código 200 si el registro es exitoso
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {

                    // Código 400 si hay un error en el registro
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Error: no sé puedo eliminar el producto" });
                    response.ContentType = "application/json";
                }

                return response;

            }
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al eliminar el usuario: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }


    }
}

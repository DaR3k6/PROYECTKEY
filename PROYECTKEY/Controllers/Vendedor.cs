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
    public class Vendedor : ControllerBase
    {
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
            string respuesta = ClaseVendedor.RegistrarVendedor(documento, nombre, fechaNacimiento, usuarioId);

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
        [HttpPut]
        [Route("vendedor/actualizarVendedor/{idVendedor}")]
        public ContentResult ActualizarVendedor(int idVendedor, string documento, string nombre, DateTime fechaNacimiento, int usuarioId)
        {
            try
            {
                // Llama al modelo para agregar o actualizar el vendedor
                dynamic respuesta = ClaseVendedor.ActualizarVendedor(idVendedor, documento, nombre, fechaNacimiento, usuarioId);

                // Crea una respuesta HTTP
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Código 200 si la actualización es exitosa
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Código 400 si hay un error en la actualización
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al actualizar el vendedor: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Elimina todo el Usuario
        /// </summary>
        /// <param name="idVendedor"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("vendedor/eliminarVendedor/{idVendedor}")]
        public ContentResult EliminarVendedor(int idVendedor)
        {
            try
            {
                // Llama al modelo para eliminar el vendedor
                dynamic respuesta = ClaseVendedor.EliminarVendedor(idVendedor);

                // Crea una respuesta HTTP
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Código 200 si la eliminación es exitosa
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Código 400 si hay un error en la eliminación
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al eliminar el vendedor: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Trae todos los vendedor o clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("cliente/listarCliente")]
        public ContentResult ListarCliente()
        {
            try
            {
                // Llama al modelo para obtener la lista de clientes
                dynamic respuesta = ClaseVendedor.ListarCliente();

                // Crea una respuesta HTTP
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Código 200 si la operación es exitosa
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Código 400 si hay un error en la operación
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al obtener los clientes: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

    }
}

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



    }
}

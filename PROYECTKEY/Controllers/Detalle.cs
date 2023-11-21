using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROYECTKEY.Modelo;
using System.Globalization;
using System.Net;

namespace PROYECTKEY.Controllers
{
    [ApiController]
    [Route("api")]
    /// <summary>
    /// Controlador para gestionar detalles de factura.
    /// </summary>
    public class Detalle : ControllerBase
    {
        /// <summary>
        /// Agrega una factura y su detalle a la base de datos.
        /// </summary>
        /// <param name="fechaFactura"></param>
        /// <param name="total"></param>
        /// <param name="metodoPagoNombre"></param>
        /// <param name="metodoPagoDescripcion"></param>
        /// <param name="habilitado"></param>
        /// <param name="producto_idProducto"></param>
        /// <param name="cantidad"></param>
        /// <param name="precioUnitario"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("facturaDetalle/agregarFacturaYDetalle")]
        public ContentResult AgregarFacturaYDetalle(DateTime fechaFactura, decimal total, string metodoPagoNombre, string metodoPagoDescripcion, bool habilitado, int producto_idProducto, int cantidad, decimal precioUnitario)
        {
            try
            {
                // Llamar a la función para insertar la factura y el detalle
                dynamic respuesta = ClaseDetalle.InsertarFacturaYDetalle(fechaFactura, total, metodoPagoNombre, metodoPagoDescripcion, habilitado, producto_idProducto, cantidad, precioUnitario);


                // Crear una respuesta HTTP
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al insertar factura y detalle: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Elimina una sola factura
        /// </summary>
        /// <param name="facturaID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("facturaDetalle/eliminarFacturaYDetalle/{facturaID}")]
        public ContentResult EliminarFacturaYDetalle(int facturaID)
        {
            try
            {
                // Llamar a la función para eliminar factura y el detalle
                dynamic respuesta = ClaseDetalle.EliminarFacturaDetalles(facturaID);


                // Create an HTTP response
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Code 200 if the deletion is successful
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Code 400 if there is an error in the deletion
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al eliminar factura y detalle: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Actualizar la factura
        /// </summary>
        /// <param name="facturaID">ID de la factura a actualizar.</param>
        /// <param name="nuevaFechaFactura"></param>
        /// <param name="nuevoTotal"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="habilitado"></param>
        /// <param name="cantidad"></param>
        /// <param name="precioUnitario"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("facturaDetalle/actualizarDetalleFacturaMetodos/{facturaID}")]
        public ContentResult ActualizarDetalleFacturaMetodos(int facturaID, DateTime nuevaFechaFactura, decimal nuevoTotal, string nombre, string descripcion, bool habilitado, int cantidad, decimal precioUnitario)
        {
            try
            {
                dynamic respuesta = ClaseDetalle.ActualizarFacturaDetalleMetodoPago(facturaID, nuevaFechaFactura, nuevoTotal, nombre, descripcion, habilitado, cantidad, precioUnitario);

                // Create an HTTP response
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Code 200 if the update is successful
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Code 400 if there is an error in the update
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al actualizar factura, detalle y método de pago: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }
        /// <summary>
        /// Listar toda una factura
        /// </summary>
        /// <param name="facturaID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listarUnDetalleFacturaMetodos/{facturaID}")]
        public ContentResult ListarUnDetalleFacturaMetodos(int facturaID)
        {
            try
            {
                // Call the function to list factura details and payment methods
                dynamic respuesta = ClaseDetalle.ListarFacturasDetallesMetodosPago(facturaID);

                // Create an HTTP response
                var response = new ContentResult();

                if (respuesta.status == true)
                {
                    // Code 200 if the operation is successful
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Content = JsonConvert.SerializeObject(respuesta);
                    response.ContentType = "application/json";
                }
                else
                {
                    // Code 400 if there is an error in the operation
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al listar factura, detalle y método de pago: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PROYECTKEY.Modelo;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace PROYECTKEY.Controllers
{
    [ApiController]
    [Route("api")]

    /// <summary>
    /// Controlador para la gestión de productos.
    /// </summary>
    public class ProductoController : Controller
    {
        /// <summary>
        /// Agrega o actualiza un producto con la información proporcionada.
        /// </summary>
        /// <param name="nombre">Nombre del producto.</param>
        /// <param name="descripcion">Descripción detallada del producto.</param>
        /// <param name="precio">Precio del producto.</param>
        /// <param name="stock">Cantidad en stock del producto.</param>
        /// <param name="vendedor_idVendedor">ID del vendedor asociado.</param>
        /// <param name="categoria_idCategoria">ID de la categoría del producto.</param>
        /// <param name="imagen">Imagen del producto como bytes.</param>
        /// <returns>Respuesta HTTP con el resultado.</returns>
        [HttpPost]
        [Route("producto/agregarProducto")]
        public HttpResponseMessage AgregarProducto(string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, IFormFile imagen)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(nombre) || precio <= 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(
                            "Datos de producto no válidos",
                            Encoding.UTF8,
                            "application/json")
                    };
                }

                // Verificar si se proporciona una imagen
                byte[] imagenBytes = null;
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imagen.CopyTo(memoryStream);
                        imagenBytes = memoryStream.ToArray();
                    }
                }

                // Llama al modelo para agregar o actualizar el producto
                dynamic respuesta = ClaseProducto.AgregarProducto(nombre, descripcion, precio, stock, vendedor_idVendedor, categoria_idCategoria, imagenBytes);

                // Crea una respuesta HTTP
                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    // Código 200 si el registro es exitoso
                    response.StatusCode = HttpStatusCode.OK;
                    var jsonResponse = JsonConvert.SerializeObject(respuesta);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }
                else
                {
                    // Código 400 si hay un error en el registro
                    response.StatusCode = HttpStatusCode.BadRequest;
                    var jsonResponse = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }

                return response;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error en la ruta para agregar producto: " + e.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        $"Error al agregar producto: {e.Message}",
                        Encoding.UTF8,
                        "application/json")
                };
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="nombre">Nuevo nombre del producto.</param>
        /// <param name="descripcion">Nueva descripción del producto.</param>
        /// <param name="precio">Nuevo precio del producto.</param>
        /// <param name="stock">Nueva cantidad en stock del producto.</param>
        /// <param name="vendedor_idVendedor">Nuevo ID del vendedor asociado.</param>
        /// <param name="categoria_idCategoria">Nuevo ID de la categoría del producto.</param>
        /// <param name="imagen">Nueva imagen del producto como bytes.</param>
        /// <returns>Respuesta HTTP con el resultado.</returns>  
        [HttpPut]
        [Route("producto/actualizar/{id}")]
        public HttpResponseMessage ActualizarProducto(int id, string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, IFormFile imagen)
        {
            try
            {
                // Validaciones
                if (id <= 0 || string.IsNullOrEmpty(nombre) || precio <= 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(
                            "Datos de producto no válidos",
                            Encoding.UTF8,
                            "application/json")
                    };
                }

                // Verificar si se proporciona una imagen
                byte[] imagenBytes = null;
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imagen.CopyTo(memoryStream);
                        imagenBytes = memoryStream.ToArray();
                    }
                }

                // Llama al modelo para agregar o actualizar el producto
                dynamic respuesta = ClaseProducto.ActualizarProducto(id, nombre, descripcion, precio, stock, vendedor_idVendedor, categoria_idCategoria, imagenBytes);

                // Crea una respuesta HTTP
                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    // Código 200 si la actualización es exitosa
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    // Código 400 si hay un error en la actualización
                    response.StatusCode = HttpStatusCode.BadRequest;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error en la ruta para actualizar producto: " + e.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        $"Error al actualizar producto: {e.Message}",
                        Encoding.UTF8,
                        "application/json")
                };
            }
        }

        /// <summary>
        /// Busca productos por nombre.
        /// </summary>
        /// <param name="nombre">Nombre del producto a buscar.</param>
        /// <returns>Respuesta HTTP con el resultado.</returns>
        [HttpGet]
        [Route("producto/buscarProducto/{nombre}")]
        public HttpResponseMessage TraerProductosPorNombre(string nombre)
        {
            try
            {
                dynamic respuesta = ClaseProducto.TraerProductosPorNombre(nombre);

                // Crea una respuesta HTTP
                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    var jsonResponse = JsonConvert.SerializeObject(respuesta);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    var jsonResponse = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        $"Error al actualizar producto: {e.Message}",
                        Encoding.UTF8,
                        "application/json")
                };
            }

        }
        /// <summary>
        /// Trae todos los datos del producto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("producto/todosProductos")]

        public HttpResponseMessage TodosProductos()
        {
            try
            {
                dynamic respuesta = ClaseProducto.TodosProductos();

                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    // Código 200 si se obtiene la información correctamente
                    response.StatusCode = HttpStatusCode.OK;
                    var jsonResponse = JsonConvert.SerializeObject(respuesta);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }
                else
                {
                    // Código 400 si hay un error en la obtención de información
                    response.StatusCode = HttpStatusCode.BadRequest;
                    var jsonResponse = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }

                return response;
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                         $"Error al obtener todos los productos: {e.Message}",
                         Encoding.UTF8,
                         "application/json")
                };
            }
        }
        /// <summary>
        /// Trae el nombre de la categoria 
        /// </summary>
        /// <param name="nombreCategoria"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("producto/filtrarPorCategoria/{nombreCategoria}")]

        public HttpResponseMessage FiltrarPorCategoria(string nombreCategoria)
        {
            try
            {
                dynamic respuesta = ClaseProducto.FiltrarPorCategoria(nombreCategoria);

                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    // Código 200 si se obtiene la información correctamente
                    response.StatusCode = HttpStatusCode.OK;
                    var jsonResponse = JsonConvert.SerializeObject(respuesta);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }
                else
                {
                    // Código 400 si hay un error en la obtención de información
                    response.StatusCode = HttpStatusCode.BadRequest;
                    var jsonResponse = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }

                return response;
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                      $"Error al filtrar productos por categoría: {e.Message}",
                      Encoding.UTF8,
                      "application/json")
                };

            }
        }
        /// <summary>
        /// Elimina todo el producto
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("producto/eliminar/{idProducto}")]

        public HttpResponseMessage EliminarProducto(int idProducto)
        {
            try
            {
                dynamic respuesta = ClaseProducto.EliminarProductoId(idProducto);

                // Crea una respuesta HTTP
                var response = new HttpResponseMessage();

                if (respuesta.status == true)
                {
                    // Código 200 si se elimina el producto correctamente
                    response.StatusCode = HttpStatusCode.OK;
                    var jsonResponse = JsonConvert.SerializeObject(respuesta);
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }
                else
                {
                    // Código 400 si hay un error en la eliminación
                    response.StatusCode = HttpStatusCode.BadRequest;
                    var jsonResponse = JsonConvert.SerializeObject(new { status = false, mensaje = "Error en la operación" });
                    response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
                }

                return response;

            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                      $"Error al eliminar producto: {e.Message}",
                      Encoding.UTF8,
                      "application/json")
                };
            }
        }

    }
}

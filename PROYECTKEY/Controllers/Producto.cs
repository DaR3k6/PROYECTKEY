using Microsoft.AspNetCore.Mvc;
using PROYECTKEY.Modelo;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PROYECTKEY.Controllers
{
    [ApiController]
    [Route("api")]

    /// <summary>
    /// Controlador para la gestión de productos.
    /// </summary>
    public class Producto : ControllerBase
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
        public ContentResult AgregarProducto(string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, IFormFile imagen)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(nombre) || precio <= 0)
                {
                    return new ContentResult
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Datos de producto no válidos" }),
                        ContentType = "application/json"
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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al agregar producto: {e.Message}" }),
                    ContentType = "application/json"
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
        public ContentResult ActualizarProducto(int id, string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, IFormFile imagen)
        {
            try
            {
                
                // Validaciones
                if (id <= 0 || string.IsNullOrEmpty(nombre) || precio <= 0)
                {
                    return new ContentResult
                    {

                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Datos de producto no válidos" }),
                        ContentType = "application/json"
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
        /// Busca productos por nombre.
        /// </summary>
        /// <param name="nombre">Nombre del producto a buscar.</param>
        /// <returns>Respuesta HTTP con el resultado.</returns>
        [HttpGet]
        [Route("producto/buscarProducto/{nombre}")]
        public ContentResult TraerProductosPorNombre(string nombre)
        {
            try
            {
                dynamic respuesta = ClaseProducto.TraerProductosPorNombre(nombre);

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
                    response.Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Error: no se encontró el nombre" });
                    response.ContentType = "application/json";
                }

                return response; 
            }
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al buscar el producto por nombre: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// Trae todos los datos del producto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("producto/todosProductos")]

        public ContentResult TodosProductos()
        {
            try
            {
                dynamic respuesta = ClaseProducto.TodosProductos();

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
                    response.Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Error: no se encontró todos los productos" });
                    response.ContentType = "application/json";
                }

                return response;
            }
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al obtener los productos: {e.Message}" }),
                    ContentType = "application/json"
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

        public ContentResult FiltrarPorCategoria(string nombreCategoria)
        {
            try
            {
                dynamic respuesta = ClaseProducto.FiltrarPorCategoria(nombreCategoria);

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
                    response.Content = JsonConvert.SerializeObject(new { status = false, mensaje = "Error: no se encontró la categoria del producto" });
                    response.ContentType = "application/json";
                }

                return response;
            }
            catch (Exception e)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al obtener filtrar por categoria: {e.Message}" }),
                    ContentType = "application/json"
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

        public ContentResult EliminarProducto(int idProducto)
        {
            try
            {
                dynamic respuesta = ClaseProducto.EliminarProductoId(idProducto);

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
                    Content = JsonConvert.SerializeObject(new { status = false, mensaje = $"Error al eliminar el producto: {e.Message}" }),
                    ContentType = "application/json"
                };
            }
        }

    }
}

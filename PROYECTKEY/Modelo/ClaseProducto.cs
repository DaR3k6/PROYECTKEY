using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace PROYECTKEY.Modelo
{
    public static class ClaseProducto
    {
        //CADENA DE CONEXION 
        public static string cadena = "Server=(local)\\BDSQL;Database=PROYECT_KEY;User Id=kevin;Password=12345;";

        public static string expecion = "";


        //FUNCION AGREGAR PRODUCTO
        public static dynamic AgregarProducto(string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, byte[] imagen)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("AgregarProducto", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@descripcion", descripcion);
                        adapter.SelectCommand.Parameters.AddWithValue("@precio", precio);
                        adapter.SelectCommand.Parameters.AddWithValue("@stock", stock);
                        adapter.SelectCommand.Parameters.AddWithValue("@vendedor_idVendedor", vendedor_idVendedor);
                        adapter.SelectCommand.Parameters.AddWithValue("@categoria_idCategoria", categoria_idCategoria);
                        adapter.SelectCommand.Parameters.AddWithValue("@imagen", imagen);
                        adapter.SelectCommand.Parameters.Add("@idProducto", SqlDbType.Int).Direction = ParameterDirection.Output;
                        adapter.SelectCommand.Parameters.Add("@mensaje", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        int idProducto = Convert.ToInt32(adapter.SelectCommand.Parameters["@idProducto"].Value);
                        bool mensaje = Convert.ToBoolean(adapter.SelectCommand.Parameters["@mensaje"].Value);

                        if (mensaje)
                        {
                            Console.WriteLine("El producto ya existe en la base de datos");
                            return new { status = false, mensaje = "El producto ya existe en la base de datos" };
                        }
                        else
                        {

                            return new
                            {
                                mensaje = "Producto agregado con éxito",
                                status = true,
                                producto = new
                                {
                                    idProducto,
                                    nombre,
                                    descripcion,
                                    precio,
                                    stock,
                                    vendedor_idVendedor,
                                    categoria_idCategoria,
                                    imagen = Convert.ToBase64String(imagen)
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al agregar el producto: " + error.Message);
                return new { mensaje = "Error al agregar el producto", status = false, error = error.Message };
            }
        }

        // FUNCION DE ACTUALIZAR PRODUCTO
        public static dynamic ActualizarProducto(int idProducto, string nombre, string descripcion, decimal precio, int stock, int vendedor_idVendedor, int categoria_idCategoria, byte[] imagen)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("ActualizarProducto", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        adapter.SelectCommand.Parameters.AddWithValue("@idProducto", idProducto);
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@descripcion", descripcion);
                        adapter.SelectCommand.Parameters.AddWithValue("@precio", precio);
                        adapter.SelectCommand.Parameters.AddWithValue("@stock", stock);
                        adapter.SelectCommand.Parameters.AddWithValue("@vendedor_idVendedor", vendedor_idVendedor);
                        adapter.SelectCommand.Parameters.AddWithValue("@categoria_idCategoria", categoria_idCategoria);
                        adapter.SelectCommand.Parameters.AddWithValue("@imagen", imagen);
                        adapter.SelectCommand.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        int resultado = Convert.ToInt32(adapter.SelectCommand.Parameters["@resultado"].Value);

                        if (resultado == 0)
                        {
                            Console.WriteLine("Producto actualizado con éxito");
                            return new
                            {
                                status = true,
                                mensaje = "Producto actualizado con éxito",
                                producto = new
                                {
                                    idProducto,
                                    nombre,
                                    descripcion,
                                    precio,
                                    stock,
                                    vendedor_idVendedor,
                                    categoria_idCategoria,
                                    imagen = Convert.ToBase64String(imagen)
                                }
                            };
                        }
                        else if (resultado == 1)
                        {
                            return new { status = false, mensaje = "Error: Producto no encontrado" };
                        }
                        else
                        {
                            return new { status = false, mensaje = "Error en la operación" };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al actualizar el producto: " + error.Message);
                return new { mensaje = "Error al actualizar el producto", status = false, error = error.Message };
            }
        }

        // FUNCION DE TRAER PRODUCTOS POR NOMBRE
        public static dynamic TraerProductosPorNombre(string nombreProducto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.ObtenerProductoPorNombre", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombreProducto);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            Console.WriteLine("Productos encontrados con éxito");

                            List<object> productos = new List<object>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                byte[] imagenBytes = (byte[])row["Imagen"];
                                string imagenBase64 = Convert.ToBase64String(imagenBytes);

                                productos.Add(new
                                {
                                    idProducto = row["idProducto"],
                                    nombre = row["Nombre"],
                                    descripcion = row["Descripcion"],
                                    precio = row["Precio"],
                                    stock = row["Stock"],
                                    vendedor_idVendedor = row["Vendedor_idVendedor"],
                                    categoria_idCategoria = row["Categoria_idCategoria"],
                                    imagen = imagenBase64
                                });
                            }

                            return new { status = true, mensaje = "Productos encontrados con éxito", producto = new { productos } };
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron productos con ese nombre");
                            return new { status = false, mensaje = "No se encontraron productos con ese nombre", productos = new List<object>() };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al buscar el producto: " + error.Message);
                return new { status = false, mensaje = "Error al buscar el producto", error = error.Message };
            }
        }

        // FUNCION DE ELIMINAR PRODUCTO
        public static dynamic EliminarProductoId(int idProducto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.EliminarProductoPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idProducto", idProducto);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Producto eliminado");
                            return new { mensaje = "Producto eliminado con éxito", status = true };
                        }
                        else
                        {
                            Console.WriteLine("Producto no existe");
                            return new { mensaje = "El producto no existe", status = false };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al eliminar el producto: " + error.Message);
                return new { mensaje = "Error al eliminar el producto", status = false, error = error.Message };
            }
        }
        // FUNCION DE TRAER TODOS LOS PRODUCTOS
        public static dynamic TodosProductos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.TodosProductos", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            Console.WriteLine("Traen todos los productos");

                            List<object> productos = new List<object>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                byte[] imagenBytes = (byte[])row["Imagen"];
                                string imagenBase64 = Convert.ToBase64String(imagenBytes);
                                productos.Add(new
                                {
                                    idProducto = row["idProducto"],
                                    nombre = row["Nombre"],
                                    descripcion = row["Descripcion"],
                                    precio = row["Precio"],
                                    stock = row["Stock"],
                                    vendedor_idVendedor = row["Vendedor_idVendedor"],
                                    categoria_idCategoria = row["Categoria_idCategoria"],
                                    imagen = imagenBase64
                                });
                            }

                            return new { mensaje = "Productos obtenidos con éxito", status = true, productos = new { productos } };
                        }
                        else
                        {
                            Console.WriteLine("No encontraron todos los productos");
                            return new { mensaje = "No se encontraron productos", status = false, productos = new List<object>() };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error no trae todos los productos: " + error.Message);
                return new { mensaje = $"Error no trae todos los productos: {error.Message}", status = false, error = error.Message };
            }
        }

        // FUNCION DE TRAER TODA LA CATEGORIA DEL PRODUCTO
        public static dynamic FiltrarPorCategoria(string nombreCategoria)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.FiltrarCategoriasPorNombre", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@nombreCategoria", nombreCategoria);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            Console.WriteLine("Categoría encontrada");

                            List<object> productos = new List<object>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                productos.Add(new
                                {
                                    id = row["idCategoria"],
                                    nombre = row["Nombre"]
                                });
                            }

                            return new { mensaje = "Categoría obtenida con éxito", status = true, categroia = new { productos } };
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron categorías");
                            return new { mensaje = "Categoría no encontrada", status = false, productos = new List<object>() };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al traer la categoría del producto: " + error.Message);
                return new { mensaje = $"Error al traer la categoría del producto: {error.Message}", status = false, error = error.Message };
            }
        }

    }
}

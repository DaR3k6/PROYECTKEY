using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;


namespace PROYECTKEY.Modelo
{
    public class ClaseDetalle
    {
        //CADENA DE CONEXION 
        public static string cadena = "Server=PPROYECT_KEY.mssql.somee.com\r\n;Database=PROYECT_KEY\r\n;User Id=Derek_SQLLogin_1;Password=w3smbf9an6;";
        public static string expecion = "";

        //FUNCION PARA AGREGAR UN DETALLE, FACTURA Y EL METODO PAGO 
        public static dynamic InsertarFacturaYDetalle(DateTime fechaFactura, decimal total, string metodoPagoNombre, string metodoPagoDescripcion, bool habilitado, int producto_idProducto, int cantidad, decimal precioUnitario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.InsertarFacturaYDetalle", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@fechaFactura", fechaFactura);
                        adapter.SelectCommand.Parameters.AddWithValue("@total", total);
                        adapter.SelectCommand.Parameters.AddWithValue("@metodoPagoNombre", metodoPagoNombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@metodoPagoDescripcion", metodoPagoDescripcion);
                        adapter.SelectCommand.Parameters.AddWithValue("@habilitado", habilitado);
                        adapter.SelectCommand.Parameters.AddWithValue("@producto_idProducto", producto_idProducto);
                        adapter.SelectCommand.Parameters.AddWithValue("@cantidad", cantidad);
                        adapter.SelectCommand.Parameters.AddWithValue("@precioUnitario", precioUnitario);

                        // Parámetros de salida
                        SqlParameter resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        SqlParameter nuevoIDParam = new SqlParameter("@nuevoID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        adapter.SelectCommand.Parameters.Add(resultadoParam);
                        adapter.SelectCommand.Parameters.Add(nuevoIDParam);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        int resultado = Convert.ToInt32(resultadoParam.Value);
                        int nuevoID = Convert.ToInt32(nuevoIDParam.Value);

                        if (resultado == 1)
                        {
                            Console.WriteLine("Inserción exitosa");
                            return new
                            {
                                mensaje = "Inserción exitosa",
                                status = true,
                                detalle = new
                                {
                                    resultado = nuevoID,
                                    fechaFactura,
                                    total,
                                    metodoPagoNombre,
                                    metodoPagoDescripcion,
                                    producto_idProducto,
                                    cantidad,
                                    precioUnitario
                                }
                            };
                        }
                        else
                        {
                            Console.WriteLine("Error en la inserción");
                            return new
                            {
                                mensaje = "Error en la inserción",
                                status = false,
                            };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al agregar el detalle: " + error.Message);
                return new
                {
                    mensaje = "Error en la inserción",
                    status = false,
                    error = error.Message,
                    nuevoID = (int?)null
                };
            }
        }

        //FUNCION DE ELIMAR LA FACTURA, DETALLE Y EL METODO DE PAGO
        public static dynamic EliminarFacturaDetalles(int facturaID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.EliminarFacturaDetallesYMetodoPago", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@facturaID", facturaID);
                        adapter.SelectCommand.Parameters.Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        bool resultado = Convert.ToBoolean(adapter.SelectCommand.Parameters["@resultado"].Value);

                        if (resultado == true)
                        {
                            return new
                            {
                                status = true,
                                mensaje = "Eliminación exitosa",
                                producto = new
                                {
                                    resultado,
                                }
                            };
                        }
                        else
                        {
                            return new
                            {
                                status = false,
                                mensaje = "Error en la eliminación",

                            };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Error al eliminar la factura y sus detalles: " + error.Message);
                return new
                {
                    status = false,
                    mensaje = "Error en la eliminación",
                    error = error.Message
                };
            }
        }

        public static dynamic ActualizarFacturaDetalleMetodoPago(int idFactura, DateTime nuevaFechaFactura, decimal nuevoTotal, string nombre, string descripcion, bool habilitado, int cantidad, decimal precioUnitario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.ActualizarFacturaDetallesYMetodoPago", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@idFactura", idFactura);
                        adapter.SelectCommand.Parameters.AddWithValue("@nuevaFechaFactura", nuevaFechaFactura);
                        adapter.SelectCommand.Parameters.AddWithValue("@nuevoTotal", nuevoTotal);
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@descripcion", descripcion);
                        adapter.SelectCommand.Parameters.AddWithValue("@habilitado", habilitado);
                        adapter.SelectCommand.Parameters.AddWithValue("@cantidad", cantidad);
                        adapter.SelectCommand.Parameters.AddWithValue("@precioUnitario", precioUnitario);
                        adapter.SelectCommand.Parameters.Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        bool resultado = Convert.ToBoolean(adapter.SelectCommand.Parameters["@resultado"].Value);

                        if (resultado == true)
                        {
                            return new
                            {
                                status = true,
                                mensaje = "Actualización exitosa",
                                producto = new
                                {
                                    idFactura,
                                    nuevaFechaFactura,
                                    nuevoTotal,
                                    nombre,
                                    descripcion,
                                    habilitado,
                                    cantidad,
                                    precioUnitario
                                }
                            };
                        }
                        else
                        {
                            return new
                            {
                                status = false,
                                mensaje = "Error en la actualización",

                            };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Error al actualizar la factura, detalles y método de pago: " + error.Message);
                return new
                {
                    status = false,
                    mensaje = "Error en la actualización",
                    error = error.Message
                };
            }
        }

        //FUNCION DE LISTRAR LA FACTURA, DETALLE Y LOS METODO DE PAGO
        public static dynamic ListarFacturasDetallesMetodosPago(int facturaID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.ListarFacturasDetallesMetodosPago", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@facturaID", facturaID);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            List<object> detalles = new List<object>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                detalles.Add(new
                                {
                                    idFactura = row["idFactura"],
                                    fechaFactura = row["FechaFactura"],
                                    total = row["Total"],
                                    cantidad = row["Cantidad"],
                                    precioUnitario = row["PrecioUnitario"],
                                    nombre = row["Nombre"],
                                    descripcion = row["Descripcion"],
                                    habilitado = row["Habilitado"]
                                });
                            }

                            return new
                            {
                                status = true,
                                mensaje = "Lista de facturas, detalles y métodos de pago",
                                detalle = new { detalles }
                            };
                        }
                        else
                        {
                            return new
                            {
                                status = false,
                                mensaje = "No se encontraron datos para la factura especificada",
                                productos = new List<object>()
                            };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                return new
                {
                    status = false,
                    mensaje = "Error al buscar la factura",
                    error = error.Message
                };
            }
        }

    }
}

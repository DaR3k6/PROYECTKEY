using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using PROYECTKEY.Controllers;

namespace PROYECTKEY.Modelo
{
    public class ClaseVendedor
    {
        //CADENA DE CONEXION 
        //public static string cadena = "Server=(local)\\BDSQL;Database=PROYECT_KEY;User Id=kevin;Password=12345;";

        public static string cadena = "Server=PPROYECT_KEY.mssql.somee.com\r\n;Database=PROYECT_KEY\r\n;User Id=Derek_SQLLogin_1;Password=w3smbf9an6;";

        public static string expecion = "";

        //FUNCION DEL REGISTRAR VENDEDOR LLENA UN FORMULARIO
        public static string RegistrarVendedor(string documento, string nombre, DateTime fechaNacimiento, int usuarioId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("RegistrarVendedor", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@documento", documento);
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        adapter.SelectCommand.Parameters.AddWithValue("@usuarioId", usuarioId);

                        adapter.SelectCommand.Parameters.Add("@idGenerado", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        adapter.SelectCommand.Parameters.Add("@registroExitoso", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        adapter.SelectCommand.ExecuteNonQuery();

                        long idGenerado = Convert.ToInt64(adapter.SelectCommand.Parameters["@idGenerado"].Value);
                        bool registroExitoso = Convert.ToBoolean(adapter.SelectCommand.Parameters["@registroExitoso"].Value);

                        if (registroExitoso == true)
                        {
                            Console.WriteLine("El proceso de registro fue exitoso.");
                            return JsonConvert.SerializeObject(new
                            {
                                mensaje = "éxito",
                                status = true,
                                vendedor = new
                                {
                                    id = idGenerado,
                                    documento,
                                    nombre,
                                    fechaNacimiento,
                                    usuarioId
                                }
                            });
                        }
                        else
                        {
                            Console.WriteLine("El proceso de registro falló.");
                            return JsonConvert.SerializeObject(new
                            {
                                status = false,
                                mensaje = "Error al registrar el vendedor."
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error al registrar el vendedor: " + e.Message);
                return JsonConvert.SerializeObject(new
                {
                    status = false,
                    mensaje = "Error al registrar el vendedor",
                    error = e.Message
                });
            }
        }

        //FUNCION DE ACTUALIZAR EL VENDEDOR
        public static dynamic ActualizarVendedor(int idVendedor, string documento, string nombre, DateTime fechaNacimiento, int usuarioId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("ActualizarVendedor", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@idVendedor", idVendedor);
                        adapter.SelectCommand.Parameters.AddWithValue("@documento", documento);
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                        adapter.SelectCommand.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        adapter.SelectCommand.Parameters.AddWithValue("@usuarioId", usuarioId);
                        adapter.SelectCommand.Parameters.Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        bool resultado = Convert.ToBoolean(adapter.SelectCommand.Parameters["@resultado"].Value);


                        if (resultado == true)
                        {
                            return new
                            {
                                mensaje = "Vendedor actualizado con éxito.",
                                status = true,
                                vendedor = new
                                {
                                    id = idVendedor,
                                    documento,
                                    nombre,
                                    fechaNacimiento,
                                    usuarioId
                                }
                            };

                        }
                        else
                        {
                            return new
                            {
                                mensaje = "Error al actualizar el vendedor o el vendedor no existe",
                                status = false,

                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dynamic errorResponse = new System.Dynamic.ExpandoObject();
                errorResponse.status = false;
                errorResponse.mensaje = $"Error al actualizar el vendedor: {e.Message}";
                errorResponse.error = e.Message;

                return errorResponse;
            }
        }
        //FUNCION DE ELIMINAR VENDEDOR
        public static dynamic EliminarVendedor(int idVendedor)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("EliminarVendedor", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@idVendedor", idVendedor);

                        adapter.SelectCommand.ExecuteNonQuery();
                    }
                }

                return new
                {
                    mensaje = $"Vendedor eliminado con éxito. ID de vendedor: {idVendedor}",
                    status = true
                };
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return new
                {
                    mensaje = $"Error al eliminar el vendedor: {e.Message}",
                    status = false,
                    error = e.Message
                };
            }
        }

        //FUNCION DE TRAER TODOS LOS CLIENTES
        public static dynamic ListarCliente()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("ListarCliente", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            List<object> clientes = new List<object>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                clientes.Add(new
                                {
                                    idVendedor = row["idVendedor"],
                                    documento = row["Documento"],
                                    nombre = row["Nombre"],
                                    fechaNacimiento = row["FechaNacimiento"],
                                    idUsuario = row["Usuario_idUsuario"]
                                });
                            }

                            return new { mensaje = "Clientes obtenidos con éxito", status = true, clientes = new { clientes } };
                        }
                        else
                        {
                            return new { mensaje = "No se encontraron clientes", status = false, clientes = new List<object>() };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                return new { mensaje = $"Error al obtener los clientes: {error.Message}", status = false, error = error.Message };
            }
        }

    }
}

using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace PROYECTKEY.Modelo
{
    public class ClaseVendedor
    {
        //CADENA DE CONEXION 
        public static string cadena = "Server=(local)\\BDSQL;Database=PROYECT_KEY;User Id=kevin;Password=12345;";

        //public static string cadena = "Server=APIKEYLOOKEY.mssql.somee.com\r\n;Database=APIKEYLOOKEY;User Id=Derek_SQLLogin_1;Password=w3smbf9an6;";

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


    }
}

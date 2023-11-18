using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;


namespace PROYECTKEY.Modelo
{
    public class ClaseUsuario
    {
        //CADENA DE CONEXION 
        public static string cadena = "Server=(local)\\BDSQL;Database=PROYECT_KEY;User Id=kevin;Password=12345;";

        public static string expecion = "";

        //FUNCION PARA ENCRYPTAR LA CONTRASEÑA
        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // FUNCIÓN PARA REGISTRAR EL USUARIO
        public static string RegistrarUsuario(string nombreUsuario, string apellido, string email, string password, int idRol)
        {
            int idUsuario = 0;
            bool correoExiste = false;

            try
            {
                string hashedPassword = HashPassword(password);

                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("UsuarioRegistrado", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombreUsuario);
                        adapter.SelectCommand.Parameters.AddWithValue("@apellido", apellido);
                        adapter.SelectCommand.Parameters.AddWithValue("@email", email);
                        adapter.SelectCommand.Parameters.AddWithValue("@password", hashedPassword);
                        adapter.SelectCommand.Parameters.AddWithValue("@idRol", idRol);
                        adapter.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        adapter.SelectCommand.Parameters.Add("@correoExiste", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        // Recupera los parámetros de salida
                        idUsuario = Convert.ToInt32(adapter.SelectCommand.Parameters["@idUsuario"].Value);
                        correoExiste = Convert.ToBoolean(adapter.SelectCommand.Parameters["@correoExiste"].Value);
                    }
                }

                if (correoExiste)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        mensaje = "El correo ya existe",
                        status = false
                    });
                }

                // Valida y devuelve el resultado
                if (idUsuario != -1)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        mensaje = $"Usuario registrado con éxito. ID de usuario: {idUsuario}",
                        status = true
                    });
                }
                else if (idUsuario == -1)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        mensaje = "El usuario ya existe.",
                        status = false,
                        error = "Usuario duplicado"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        mensaje = "El procedimiento almacenado devolvió un código de error.",
                        status = false,
                        error = "Error en el procedimiento almacenado"
                    });
                }
            }
            catch (Exception e)
            {
                expecion = e.Message;
                return JsonConvert.SerializeObject(new
                {
                    mensaje = $"Error al registrar el usuario: {e.Message}",
                    status = false,
                    error = e.Message
                });
            }
        }

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

        //FUNCION DE LOGIN PARA INICAR QUE TIPO DE ROL ES 'VENDEDOR O CLIENTE'
        public static dynamic LoginUsuario(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.IniciarSesion", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@email", email);

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            DataRow row = table.Rows[0];
                            string hashedPasswordFromDB = row["Contraseña"].ToString();                           // Verifica la contraseña
                            string hashedPasswordInput = HashPassword(password);

                            // Cambia la comparación para usar Equals
                            if (hashedPasswordFromDB.Equals(hashedPasswordInput))
                            {
                                return new
                                {
                                    mensaje = "Inicio de sesión exitoso",
                                    status = true,
                                    usuario = new
                                    {
                                        id = row["idUsuario"],
                                    }
                                };
                            }
                            else
                            {
                                return new
                                {
                                    mensaje = "Contraseña incorrecta",
                                    status = false
                                };
                            }
                        }
                        else
                        {
                            return new
                            {
                                mensaje = "Usuario no encontrado",
                                status = false
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error al logear el usuario: " + e.Message);
                return new
                {
                    mensaje = "Error al logearse",
                    status = false
                };
            }
        }

        //FUNCION TRAER LA INFORMACION DEL USUARIO
        public static dynamic InformacionUsuario(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.TraerInformacioUsuario", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@id", id);

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            DataRow row = table.Rows[0];
                            dynamic result = row[0];
                            Console.WriteLine(JsonConvert.SerializeObject(result));
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error al obtener la información del usuario: " + e.Message);
                return new
                {
                    mensaje = "Error al obtener la información del usuario",
                    status = false
                };
            }
        }

    }
}

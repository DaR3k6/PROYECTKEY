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
        public static string cadena = "Server=PPROYECT_KEY.mssql.somee.com\r\n;Database=PROYECT_KEY\r\n;User Id=Derek_SQLLogin_1;Password=w3smbf9an6;";
        
        //public static string cadena = "Server=(local)\\BDSQL;Database=PROYECT_KEY;User Id=kevin;Password=12345;";


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
                        status = true,
                        usuario = new
                        {
                            id = idUsuario,
                            nombre = nombreUsuario,
                            apellido = apellido,
                            email = email,
                            password = hashedPassword,
                            idRol = idRol
                        }
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

        // FUNCIÓN PARA ACTUALIZAR EL USUARIO
       public static dynamic ActualizarUsuario(int idUsuario, string nombreUsuario, string apellido, string email, string password, int idRol)
        {
            int resultado = 0;

            try
            {
                string hashedPassword = HashPassword(password);

                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("EditarUsuario", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                        adapter.SelectCommand.Parameters.AddWithValue("@nombre", nombreUsuario);
                        adapter.SelectCommand.Parameters.AddWithValue("@apellido", apellido);
                        adapter.SelectCommand.Parameters.AddWithValue("@email", email);
                        adapter.SelectCommand.Parameters.AddWithValue("@password", hashedPassword);
                        adapter.SelectCommand.Parameters.AddWithValue("@idRol", idRol);
                        adapter.SelectCommand.Parameters.Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        // Recupera el resultado de la ejecución del procedimiento almacenado
                        resultado = Convert.ToInt32(adapter.SelectCommand.Parameters["@resultado"].Value);
                    }
                }

                if (resultado == 1)
                {
                    return new
                    {
                        mensaje = $"Usuario actualizado con éxito. ID de usuario: {idUsuario}",
                        status = true,
                        usuario = new
                        {
                            id = idUsuario,
                            nombre = nombreUsuario,
                            apellido = apellido,
                            email = email,
                            password = hashedPassword,
                            idRol = idRol
                        }
                    };
                }
                else
                {
                    return new
                    {
                        mensaje = "El usuario no pudo ser actualizado.",
                        status = false,
                        error = "Error en la actualización"
                    };
                }
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return new
                {
                    mensaje = $"Error al actualizar el usuario: {e.Message}",
                    status = false,
                    error = e.Message
                };
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

                            string hashedPasswordFromDB = row["Contraseña"].ToString();
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
                                        email,
                                        nombre = row["Nombre"],
                                        password = hashedPasswordInput,

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

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.[ListarTodosUsuarios]", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@id", id);

                        DataTable table = new DataTable();
                        adapter.Fill(table);


                        if (table.Rows.Count > 0)
                        {
                            DataRow row = table.Rows[0];

                            return new
                            {
                                mensaje = "Información todos los usuarios obtenidos",
                                status = true,
                                usuario = new
                                {
                                    id = row["idUsuario"],
                                    nombre = row["Nombre"],
                                    apellido = row["Apellido"],
                                    email = row["Email"],
                                    password = row["Contraseña"],
                                    rol = row["Rol_idRol"]

                                }
                            };
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
                Console.Error.WriteLine("Error al obtener la información del usuario: " + e.Message);
                return new
                {
                    mensaje = "Error al obtener la información del usuario",
                    status = false
                };
            }
        }

        //FUNCION TRAER TODOS LOS USUARIOS
        public static dynamic ListarTodosUsuarios()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.ListarTodosUsuarios", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        adapter.Fill(table);


                        if (table.Rows.Count > 0)
                        {
                            List<dynamic> usuarios = new List<dynamic>();

                            foreach (DataRow row in table.Rows)
                            {
                                usuarios.Add(new
                                {
                                    id = row["idUsuario"],
                                    nombre = row["Nombre"],
                                    apellido = row["Apellido"],
                                    email = row["Email"],
                                    password = row["Contraseña"],
                                    rol = row["Rol_idRol"]
                                });
                            }

                            return new
                            {
                                mensaje = "Información de usuarios obtenida con éxito",
                                status = true,
                                usuarios = usuarios
                            };

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
                Console.Error.WriteLine("Error al obtener la información del usuario: " + e.Message);
                return new
                {
                    mensaje = "Error al obtener la información del usuario",
                    status = false
                };
            }
        }

        //FUNCION DE TAER LOS ROLES
        public static dynamic TraerRoles()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.TraerRoles", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        adapter.Fill(table);


                        if (table.Rows.Count > 0)
                        {
                            List<dynamic> roles = new List<dynamic>();

                            foreach (DataRow row in table.Rows)
                            {
                                roles.Add(new
                                {
                                    id = row["idRol"],
                                    nombre = row["Nombre"],
                                });
                            }

                            return new
                            {
                                mensaje = "Información de roles obtenida con éxito",
                                status = true,
                                roles = roles
                            };

                        }
                        else
                        {
                            return new
                            {
                                mensaje = "Rol no encontrado",
                                status = false
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error al obtener la información del usuario: " + e.Message);
                return new
                {
                    mensaje = "Error al obtener la información del rol",
                    status = false
                };
            }
        }

        //FUNCION DE ELIMINAR UN USUARIO
        public static dynamic EliminarUsuarioId(int idUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("dbo.EliminarUsuario", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@idUsuario", idUsuario);

                        // Agrega el parámetro de salida para capturar el resultado del procedimiento almacenado
                        SqlParameter resultadoParam = new SqlParameter("@resultado", SqlDbType.Int);
                        resultadoParam.Direction = ParameterDirection.Output;
                        adapter.SelectCommand.Parameters.Add(resultadoParam);

                        // Crea un DataTable para almacenar los resultados del procedimiento almacenado
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        // Recupera el valor del parámetro de salida
                        int resultado = Convert.ToInt32(resultadoParam.Value);

                        // Verifica el resultado
                        if (resultado == 0)
                        {
                            return new { mensaje = "Usuario eliminado con éxito", status = true };
                        }
                        else
                        {
                            return new { mensaje = "El usuario no existe o no fue eliminado", status = false };
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error al eliminar el usuario: " + error.Message);
                return new { mensaje = "Error al eliminar el usuario", status = false, error = error.Message };
            }
        }



    }
}

using System;
using System.Data.SqlClient;
using System.Data;
namespace CrtProduccion.entidades
{
    class dmLibroDirecciones
    {
        #region Atributos

        public int fld_oldiLD = 0;
        public int fld_idLD { get; set; }
        public int? fld_idCargo { get;set; }
        public int? fld_idDpto { get; set; }
        public string fld_Ced_Rnc { get; set; }
        public string fld_Nombres { get; set; }
        public string fld_Apellidos { get; set; }
        public bool fld_escliente { get; set; }
        public bool fld_esEmpleado { get; set; }
        public bool fld_esProovedor { get; set; }
        public double fld_sueldo { get; set; }
        public string fld_estado { get; set; }
        
        public string errormsg = "";

        #endregion



        #region Constructores


        public dmLibroDirecciones()
        {
            limpiar();
        }
        public dmLibroDirecciones(
            int pidLD,
            int? Pfld_idCargo,
            int?Pfld_idDpto,
            String pCed_RNC,
            String Pfld_Nombre,
            String Pfld_Apellido,
            String Pfld_estado,
            bool Pfld_escliente,
            bool pfld_esEmpleado,
            bool pfld_esProveedor,
            Double Pfld_sueldo)
        {
            fld_idLD = pidLD;
            fld_idCargo = Pfld_idCargo;
            fld_idCargo = Pfld_idCargo;
            fld_Ced_Rnc = pCed_RNC;
            fld_Nombres = Pfld_Nombre;
            fld_Apellidos = Pfld_Apellido;
            fld_idCargo = Pfld_idCargo;
            fld_escliente = Pfld_escliente;
            fld_esEmpleado = pfld_esEmpleado;
            fld_esProovedor = pfld_esProveedor;
            fld_sueldo = Pfld_sueldo;
            fld_estado = Pfld_estado;       
        }
        #endregion



        #region Métodos y funciones
       
        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idLD = 0;
            fld_idCargo = 0;
            fld_idCargo = 0;
            fld_Ced_Rnc = "";
            fld_Nombres = "";
            fld_Apellidos = "";
            fld_estado = "";
            fld_escliente = true;
            fld_esEmpleado = true;
            fld_esProovedor = true;
            fld_sueldo = 0; 
        }
        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_Ced_Rnc.Equals(""))
            {
                errormsg = "CedulaRNC no puede estar vacío.";
                lret = false;
            }
            if (lret && fld_Nombres.Equals(""))
            {
                errormsg = "Nombre  no puede estar vacío.";
                lret = false;
            }
            return lret;
        }
        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla LD</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idLD = 0;

            if (datamanager.ConexionAbrir())
            {
                // Preparamos consulta pra la actualización
              SqlCommand cmd = new SqlCommand(" Insert into LibroDirecciones(cedulaRNC, Nombres,"+
                                              " Apellidos,esCliente,esEmpleado,esProveedor,"+ 
                                              " idCargo,idDpto,sueldo)" +
                                              " output INSERTED.idLD" +
                                              " Values(@cedulaRNC,@Nombres,@Apellidos,"+
                                              " @esCliente,@esEmpleado,@esProveedor,"+
                                              " @idCargo,@idDpto,@sueldo)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@cedulaRNC", fld_Ced_Rnc);
                cmd.Parameters.AddWithValue("@Nombres",fld_Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", fld_Apellidos);
                cmd.Parameters.AddWithValue("@esCliente", SqlDbType.Bit).Value = false;
                cmd.Parameters.AddWithValue("@esEmpleado", SqlDbType.Bit).Value = false;
                cmd.Parameters.AddWithValue("@esProveedor", SqlDbType.Bit).Value =false;
                cmd.Parameters.AddWithValue("@idCargo", Convert.ToInt32(fld_idCargo));
                cmd.Parameters.AddWithValue("@idDpto", Convert.ToInt32(fld_idDpto));
                cmd.Parameters.AddWithValue("@sueldo", Convert.ToDouble(fld_sueldo));
                //cmd.Parameters.AddWithValue("@estado", fld_estado);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idCargo Insertado.
                fld_idLD = (int)cmd.ExecuteScalar();


                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idLd Retornado es Cero
            return fld_idLD;
        }
        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla LD.
        /// </summary>
        /// <param name="dr">Objeto SqlDataReader que contiene los datos extraido de la tabla.</param>
        /// <param name="asignar">true para asignar los campos del registro leido a las propiedades.</param>
        /// <returns>True : cuando el dato existe, false cuando no existe.</returns>
        public bool leerDatos(SqlDataReader dr, bool asignar)
        {
                bool encontrado = false;
                if (dr.Read())
                {
                    encontrado = true;
                    if (asignar)
                    {
                        fld_idLD = (int)dr["idLD"];
                        fld_Ced_Rnc = dr["cedulaRNC"].ToString();
                        fld_Nombres = dr["Nombres"].ToString();
                        fld_Apellidos = dr["Apellidos"].ToString();
                        fld_estado = dr["estado"].ToString();
                        fld_escliente = Convert.ToBoolean(dr["esCliente"].ToString());
                        fld_escliente = Convert.ToBoolean(dr["esEmpleado"].ToString());
                        fld_escliente = Convert.ToBoolean(dr["esProveedor"].ToString());
                        fld_sueldo = Convert.ToDouble(dr["sueldo"].ToString());

                    try
                    {
                        fld_idCargo = (int?)dr["idCargo"];
                        fld_idDpto = (int?)dr["idDpto"];
                    }
                    catch (Exception)
                    {
                        fld_idCargo = null;
                        fld_idDpto = null;
                    }
                }
                }
                else
                {
                    if (asignar) limpiar();
                }

                return encontrado;      
            }
        /// <summary>
        ///  Buscar en la tabla de LD por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pcedulaRNC, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" select idLD, cedulaRNC,Nombres,Apellidos,"+
                                              " esCliente,esEmpleado,esProveedor,idCargo,idDpto,sueldo,estado,photo" +
                                              " from LibroDirecciones" +
                                              " where cedulaRNC = '" + pcedulaRNC + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de LD por el idCargo
        /// </summary>
        /// <param name="idCargo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idLD, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" select idLD, cedulaRNC,Nombres,Apellidos,"+
                                              " esCliente,esEmpleado,esProveedor,idCargo,"+
                                              " idDpto,sueldo,estado,photo" +
                                              " from LibroDirecciones" +
                                              " where idLD = " + idLD.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla LD.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla LD</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer(" Select top 1 idLD, cedulaRNC,Nombres,"+
                                              " Apellidos,esCliente,"+
                                              " esEmpleado,EsProveedor,idCargo,idDpto,"+
                                              " sueldo,estado" +
                                              " from LibroDirecciones" +
                                              " order by idLD desc ");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla LD</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand(" update LibroDirecciones" +
                                                " Set cedulaRNC = @cedulaRNC," +
                                                " Nombres = @Nombres,Apellidos=@Apellidos,"+
                                                " esCliente=@esCliente,esEmpleado=@esEmpleado,"+
                                                " esProveedor=@esProveedor,idCargo=@idCargo,"+
                                                " idDpto=@idDpto,sueldo=@sueldo,estado=@estado" +
                                                " Where idLD = @idLD ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idLD", fld_idLD);
                cmd.Parameters.AddWithValue("@cedulaRNC", fld_Ced_Rnc);
                cmd.Parameters.AddWithValue("@Nombres", fld_Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", fld_Apellidos);
                cmd.Parameters.AddWithValue("@esCliente", SqlDbType.Bit).Value = false;
                cmd.Parameters.AddWithValue("@esEmpleado", SqlDbType.Bit).Value =false;
                cmd.Parameters.AddWithValue("@esProveedor", SqlDbType.Bit).Value =false;
                cmd.Parameters.AddWithValue("@sueldo", fld_sueldo);
                cmd.Parameters.AddWithValue("@estado", fld_estado);

                if (fld_idCargo != null)
                    cmd.Parameters.AddWithValue("@idCargo", fld_idCargo);
                else
                    cmd.Parameters.AddWithValue("@idCargo", DBNull.Value);

                if (fld_idDpto != null)
                    cmd.Parameters.AddWithValue("@idDpto", fld_idDpto);
                else
                    cmd.Parameters.AddWithValue("@idDpto", DBNull.Value);


                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();
            }
            return lRet > 0;
        }
        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla LibroDirecciones</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidLD)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata(" delete from libroDirecciones" +
                                                   " where idLD = " + pidLD.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }
    }
}
#endregion





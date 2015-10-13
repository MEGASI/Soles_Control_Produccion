using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmVehiculo_tipo
    {
        #region Atributos

        public int fld_oldidTipoV = 0;
        public int fld_idTipoV { get; set; }
        public string fld_Descripcion { get; set; }
        public string errormsg = "";

        #endregion

        #region Constructores
        public dmVehiculo_tipo()
        {
            limpiar();
        }

        public dmVehiculo_tipo(int pidTipoV, String pDescripcion)

        {
            fld_idTipoV = pidTipoV;
            fld_Descripcion = pDescripcion;
        }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idTipoV = 0;
            fld_Descripcion = "";

        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_Descripcion.Equals(""))
            {
                errormsg = "Descripcion no puede estar vacío.";
                lret = false;
            }
            return lret;
        }

        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla Vehiculo_Tipo</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idTipoV = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into Vehiculo_Tipo(Descripcion)" +
                                                " output INSERTED.idTipoVehiculo" +
                                                " Values(@Descripcion)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idTipoVehiculo Insertado.
                fld_idTipoV = Convert.ToByte(cmd.ExecuteScalar());

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idTipoVehiculo Retornado es Cero
            return fld_idTipoV;
        }


        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla Vehiculo_tipo.
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
                    fld_idTipoV =Convert.ToByte (dr["idTipoVehiculo"]);
                    fld_Descripcion = dr["Descripcion"].ToString();
                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }

        /// <summary>
        ///  Buscar en la tabla de Vehiculo_Tipo por el Descripcion del Vehiculo.
        /// </summary>
        /// <param name="pNombre"> Descripcion único que identifica el Vehiculo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pVehiculo, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idTipoVehiculo, Descripcion" +
                                               " from Vehiculo_Tipo" +
                                               " where Descripcion = '" + pVehiculo + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de Vehiculo_Tipo por el idTipoVehiculo
        /// </summary>
        /// <param name="idTipoVehiculo"> código único que identifica el Vehiculo_Tipo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idTipoVehiculo, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idTipoVehiculo, Descripcion" +
                                               " from Vehiculo_Tipo" +
                                               " where idTipoVehiculo = " + idTipoVehiculo.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla Vehiculo_Tipo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla Vehiculo_Tipo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idTipoVehiculo, Descripcion" +
                                               " from Vehiculo_Tipo" +
                                               " order by idTipoVehiculo desc ");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla Vehiculo_Tipo</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("update Vehiculo_Tipo" +
                                                " Set Descripcion = @Descripcion" +
                                                " Where idTipoVehiculo = @idTipoVehiculo ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idTipoVehiculo", fld_idTipoV);
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);


                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla Vehiculo_Tipo</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidTipoV)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from Vehiculo_Tipo" +
                                               " where idTipoVehiculo = " + pidTipoV.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}






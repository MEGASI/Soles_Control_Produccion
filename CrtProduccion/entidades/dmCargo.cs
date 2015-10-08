using System;
using System.Data.SqlClient;


namespace CrtProduccion.entidades
{
    class dmCargo
    {

        #region Atributos

        public int fld_oldidCargo = 0;
        public int fld_idCargo { get; set; }
        public string fld_NombreCargo { get; set; }
        public string errormsg = "";

        #endregion

        #region Constructores
        public dmCargo()
        {
            limpiar();
        }

        public dmCargo(int pidCargo, String pNombreCargo)

        {
            fld_idCargo = pidCargo;
            fld_NombreCargo = pNombreCargo;
        }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idCargo = 0;
            fld_NombreCargo = "";

        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_NombreCargo.Equals(""))
            {
                errormsg = "Nombre de cargo no puede estar vacío.";
                lret = false;
            }
            return lret;
        }

        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla segGrupo</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idCargo = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into cargo(Descripcion)" +
                                                " output INSERTED.idCargo" +
                                                " Values(@Descripcion)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Descripcion", fld_NombreCargo);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idCargo Insertado.
                fld_idCargo = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idCargo Retornado es Cero
            return fld_idCargo;
        }


        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla segGrupo.
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
                    fld_idCargo = (int)dr["idCargo"];
                    fld_NombreCargo = dr["Descripcion"].ToString();
                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }

        /// <summary>
        ///  Buscar en la tabla de segGrupo por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idCargo, Descripcion" +
                                               " from cargo" +
                                               " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idCargo
        /// </summary>
        /// <param name="idCargo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idCargo, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idCargo, Descripcion" +
                                               " from cargo" +
                                               " where idCargo = " + idCargo.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idCargo, Descripcion" +
                                               " from cargo" +
                                               " order by idCargo desc ");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla segGrupo</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("update cargo" +
                                                " Set Descripcion = @Descripcion" +
                                                " Where idCargo = @idCargo ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idCargo", fld_idCargo);
                cmd.Parameters.AddWithValue("@Descripcion", fld_NombreCargo);


                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla segGrupo</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidCargo)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from cargo" +
                                               " where idCargo = " + pidCargo.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}





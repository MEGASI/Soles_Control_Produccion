using System;
using System.Data.SqlClient;


namespace CrtProduccion.entidades
{
    class dmproyecto
    {

        #region Atributos

        public int fld_oldidProyecto = 0;
        public int fld_oldidProyectoCrtl = 0;
        public int fld_idProyecto { get; set; }
        public int fld_idProyectoCRTL { get; set; }
        public string fld_Descripcion { get; set; }
        public string errormsg = "";
       

        #endregion

        #region Constructores
        public dmproyecto()
        {
            limpiar();
        }

        public dmproyecto(int Pfld_idProyecto, String Pfld_Descripcion, int pfld_idProyectoCRTL)

        {
            fld_idProyecto = Pfld_idProyecto;
            fld_Descripcion = Pfld_Descripcion;
            fld_idProyectoCRTL = pfld_idProyectoCRTL;
        }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idProyecto = 0;
            fld_Descripcion = "";
            fld_idProyectoCRTL = 0;

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
                errormsg = "Nombre de Proyecto no puede estar vacío.";
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
            fld_idProyecto = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into Proyecto(Descripcion)" +
                                                " output INSERTED.idProyecto" +
                                                " Values(@Descripcion)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idProyecto Insertado.
                fld_idProyecto = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idProyecto Retornado es Cero
        return fld_idProyecto;
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
                    fld_idProyecto = (int)dr["idProyecto"];
                    fld_Descripcion = dr["Descripcion"].ToString();
                   fld_idProyectoCRTL = (int)dr["idProyectoCRTL"];
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
            var dr = datamanager.ConsultaLeer("select idProyecto, Descripcion" +
                                               " from Proyecto" +
                                               " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idProyecto
        /// </summary>
        /// <param name="idProyecto"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idProyecto, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idProyecto, Descripcion,idProdectoCTRL" +
                                               " from Proyecto" +
                                               " where idProyecto = " + idProyecto.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idProyecto, Descripcion" +
                                               " from Proyecto" +
                                               " order by idProyecto desc ");
            return leerDatos(dr, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public bool BuscarCRTL()
        {
            var dr = datamanager.ConsultaLeer("select Descripcion, idProdectoCTRL from proyecto union " +
                                                                "all select 'N/A' as descripcion, " +
                                                                "null as idProyecto order by descripcion");
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
                SqlCommand cmd = new SqlCommand("update Proyecto" +
                                                " Set Descripcion = @Descripcion" +
                                                " Where idProyecto = @idProyecto ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idProyecto", fld_idProyecto);
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
        /// <para>Método que elimina un registro de la tabla segGrupo</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidProyecto)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from Proyecto" +
                                               " where idProyecto = " + pidProyecto.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}





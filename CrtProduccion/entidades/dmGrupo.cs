using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmGrupo
    {
        #region Atributos

        public int fld_oldIdGrupo = 0;
        public int fld_idGrupo { get; set; }
        public string fld_NombreGrupo { get; set; }
        public string errormsg = "";

        #endregion

        #region Constructores
        public dmGrupo()
        {
            limpiar();
        }

        public dmGrupo(int pidGrupo, String pNombreGrupo)

        {
            fld_idGrupo = pidGrupo;
            fld_NombreGrupo = pNombreGrupo;
        }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idGrupo = 0;
            fld_NombreGrupo = "";
            
        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_NombreGrupo.Equals(""))
            {
                errormsg = "Nombre de Grupo no puede estar vacío.";
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
            fld_idGrupo = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into segGrupo(Nombre)" +
                                                " output INSERTED.idGrupo" +
                                                " Values(@Nombre)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Nombre", fld_NombreGrupo);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idGrupo Insertado.
                fld_idGrupo = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idGrupo Retornado es Cero
            return fld_idGrupo;
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
                    fld_idGrupo = (int)dr["idGrupo"];
                    fld_NombreGrupo = dr["Nombre"].ToString();
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
            var dr = datamanager.ConsultaLeer("select idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " where Nombre = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idGrupo
        /// </summary>
        /// <param name="idGrupo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idGrupo, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " where idGrupo = " + idGrupo.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " order by idGrupo desc ");
            return leerDatos(dr,true);
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
                SqlCommand cmd = new SqlCommand("update segGrupo" +
                                                " Set Nombre = @Nombre" +
                                                " Where idGrupo = @idGrupo ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idGrupo", fld_idGrupo);
                cmd.Parameters.AddWithValue("@Nombre", fld_NombreGrupo);
                

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
        public bool borrarDatos(int pidGrupo)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from segGrupo" +
                                               " where idGrupo = " + pidGrupo.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}

    



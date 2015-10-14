using System;
using System.Data.SqlClient;
namespace CrtProduccion.entidades
{
    class dmVehiculoMarca
    {

        
        #region Atributos

        public int fld_oldidMarcaV = 0;
        public int fld_idMacarV { get; set; }
        public string fld_Descripcion { get; set; }
        public string errormsg = "";

        #endregion

        #region Constructores
        public dmVehiculoMarca()
        {
            limpiar();
        }

        public dmVehiculoMarca(int pidMarcaV, String pDescripcion)

        {
            fld_idMacarV = pidMarcaV;
            fld_Descripcion = pDescripcion;
        }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idMacarV = 0;
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
                errormsg = "Marca del Vehiculo no puede estar vacío.";
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
            fld_idMacarV = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into Vehiculo_Marca(Descripcion)" +
                                                " output INSERTED.idMarca" +
                                                " Values(@Descripcion)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idMarca Insertado.
                fld_idMacarV = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idMarca Retornado es Cero
            return fld_idMacarV;
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
                    fld_idMacarV = (int)dr["idMarca"];
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
        ///  Buscar en la tabla de segGrupo por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idMarca, Descripcion" +
                                               " from Vehiculo_Marca" +
                                               " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idMarca
        /// </summary>
        /// <param name="idMarca"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idMarca, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idMarca, Descripcion" +
                                               " from Vehiculo_Marca" +
                                               " where idMarca = " + idMarca.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idMarca, Descripcion" +
                                               " from Vehiculo_Marca" +
                                               " order by idMarca desc ");
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
                SqlCommand cmd = new SqlCommand("update Vehiculo_Marca" +
                                                " Set Descripcion = @Descripcion" +
                                                " Where idMarca = @idMarca ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idMarca", fld_idMacarV);
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
        public bool borrarDatos(int pidMarcaV)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from Vehiculo_Marca" +
                                               " where idMarca = " + pidMarcaV.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}



    





using System;
using System.Data.SqlClient;
namespace CrtProduccion.entidades
{
    class dmActividad
    {
        #region Atributos
        
        public int fld_idActividad= 0;
        public int fld_oldidActividad = 0;
        public string fld_codigo { get; set; }
        public string fld_Descripcion { get; set; }
        public string fld_idMedida { get; set; }
        public double fld_Precio { get; set; }
        public string errormsg = "";

        #endregion



        #region Constructores
        public dmActividad()
        {
            limpiar();
        }

        public dmActividad(
            int pidActividad,
            String pfld_codigo,
            String pfld_Descripcion,
            String pfld_idMedida,
            Double pfld_Precio)

        {
            fld_idActividad = pidActividad;
            fld_codigo = pfld_codigo;
            fld_Descripcion = pfld_Descripcion;
            fld_idMedida = pfld_idMedida;
            fld_Precio = pfld_Precio;
        }

        #endregion


           #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idActividad = 0;
            fld_codigo = "";
            fld_idMedida = "";
            fld_Precio = 0;
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

            if (lret && fld_codigo.Equals(""))
            {
                errormsg = "Codigo  no puede estar vacío.";
                lret = false;

            }


          
            return lret;
        }

        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla actividades</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idActividad = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into actividades(Codigo,Descripcion,idMedida,Precio)" +
                                                " output INSERTED.idActividad" +
                                                " Values(@Codigo,@Descripcion,@idMedida,@Precio)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización

                cmd.Parameters.AddWithValue("@Codigo", fld_codigo);
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@idMedida", fld_idMedida);
                cmd.Parameters.AddWithValue("@Precio", fld_Precio);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idGrupo Insertado.
                fld_idActividad = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idGrupo Retornado es Cero
            return fld_idActividad;
        }


        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla actividades.
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
                    fld_idActividad = (int)dr["idActividad"];
                    fld_codigo = dr["codigo"].ToString();
                    fld_Descripcion = dr["descripcion"].ToString();
                    fld_idMedida = dr["idMedida"].ToString();
                    fld_Precio = Convert.ToDouble(dr["Precio"]);
                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }

        /// <summary>
        ///  Buscar en la tabla de actividades por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" Select idActividad,Codigo,"+
                                              " Descripcion,idMedida,Precio" +
                                              " from actividades" +
                                              " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de actividades por el idGrupo
        /// </summary>
        /// <param name="idGrupo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idActividad, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" Select idActividad,Codigo,"+
                                              " Descripcion,idMedida,Precio" +
                                              " from actividades" +
                                              " where idActividad = " + idActividad.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla actividades.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla actividades</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer(" Select top 1 idActividad,"+
                                              " Codigo,Descripcion,idMedida,"+
                                              " Precio from actividades" +
                                              " order by idActividad desc ");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla actividades</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand(" Update actividades" +
                                                " Set Codigo = @Codigo, "+
                                                " Descripcion = @descripcion," +
                                                " idMedida = @idMedida,"+
                                                " Precio = @Precio"+
                                                " Where idActividad = @idActividad ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idActividad", fld_idActividad);
                cmd.Parameters.AddWithValue("@codigo",fld_codigo);
                cmd.Parameters.AddWithValue("@descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@idMedida", fld_idMedida);
                cmd.Parameters.AddWithValue("@Precio", fld_Precio);


                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla actividades</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidActividad)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from actividades" +
                                               " where idActividad = " + pidActividad.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}






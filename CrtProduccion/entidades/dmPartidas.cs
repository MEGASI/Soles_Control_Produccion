using System;
using System.Data.SqlClient;


namespace CrtProduccion.entidades
{
    class dmPartidas
    {
        
        
            #region Atributos

            public int fld_oldidPartida = 0;
            public int fld_idpartida { get; set; }
            public string fld_codigo { get; set; }
            public int fld_idPartidaTipo { get; set; }
            public string fld_descripcion { get; set; }
            public string fld_medida { get; set; }
            public Double fld_Precio { get; set; }
            public string errormsg = "";

            #endregion


            #region Constructores

            public dmPartidas()
            {
                limpiar();
            }

            public dmPartidas(
                  int pidPartida,
                  String pfld_Codigo,
                  int pidPartidaTipo,
                  String pDescripcion,
                  String pidmedida,
                  Double pPrecio)

            {
                fld_idpartida = pidPartida;
                fld_codigo = pfld_Codigo;
                fld_idPartidaTipo = pidPartidaTipo;
                fld_descripcion = pDescripcion;
                fld_medida = pidmedida;
                fld_Precio = pPrecio;

            }

        #endregion

        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idpartida = 0;
            fld_codigo = "";

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
                errormsg = "Nombre de Codigo no puede estar vacío.";
                lret = false;
            }
            return lret;
        }

        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla Partida</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idpartida = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand(" Insert into Partida(Codigo,idPartidaTipo,"+
                                                " Descripcion,idMedida,precio)" +
                                                " output INSERTED.idPartida" +
                                                " Values(@Codigo,@Descripcion,"+
                                                " @idPartidaTipo,@Descripcion,"+
                                                " @idMedida,@precio)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Codigo", fld_codigo);
                cmd.Parameters.AddWithValue("@idPartidaTipo", fld_idPartidaTipo);
                cmd.Parameters.AddWithValue("@Descripcion", fld_descripcion);
                cmd.Parameters.AddWithValue("@idMedida", fld_medida);
                cmd.Parameters.AddWithValue("@precio", fld_Precio);

                // Ejecutamos consulta de Actualización
                // y Retornamos el idGrupo Insertado.
                fld_idpartida = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idGrupo Retornado es Cero
            return fld_idpartida;
        }


        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla Partida.
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
                    //precio
                    fld_idpartida = (int)dr["idPartida"];
                    fld_codigo = dr["Codigo"].ToString();
                    fld_idPartidaTipo = (int)dr["idPartidaTipo"];
                    fld_descripcion = dr["descripcion"].ToString();

                    fld_medida = dr["idMedida"].ToString();

                    fld_Precio = Convert.ToDouble(dr["precio"].ToString());

                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }

        /// <summary>
        ///  Buscar en la tabla de Partida por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" Select idPartida,Codigo,"+
                                              " idPartidaTipo,Descripcion,"+
                                              " idMedida from Partida,Precio"+
                                              " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de Partida por el idGrupo
        /// </summary>
        /// <param name="idGrupo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idPartida, bool asignar)
        {
            var dr = datamanager.ConsultaLeer(" Select idPartida,"+
                                              " Codigo,idPartidaTipo,"+
                                              " Descripcion,idMedida" +
                                              " from Partida,Precio" +
                                              " where idPartida = " + idPartida.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla Partida.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla Partida</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer(" Select top 1 idPartida,"+
                                              " Codigo,idPartidaTipo,"+
                                              " Descripcion,idMedida"+ 
                                              " from Partida,Precio"+
                                              " order by idPartida desc");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla Partida</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand(" update Partida" +
                                                " Set Codigo=@Codigo,"+
                                                " idPartidaTipo=@idPartidaTipo,"+
                                                " Descripcion=@Descripcion"+
                                                " idMedida=@idMedida" +
                                                " Precio=@Precio"+
                                                " Where idPartida = @idPartida ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idPartida", fld_idpartida);
                cmd.Parameters.AddWithValue("@codigo", fld_codigo);
                cmd.Parameters.AddWithValue("@idPartidaTipo", fld_idPartidaTipo);
                cmd.Parameters.AddWithValue("@Descripcion", fld_descripcion);
                cmd.Parameters.AddWithValue("@idMedida", fld_medida);
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
        /// <para>Método que elimina un registro de la tabla Partida</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidPartida)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata(" Delete " +
                                                   " from Partida" +
                                                   " where idPartida = " + pidPartida.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}







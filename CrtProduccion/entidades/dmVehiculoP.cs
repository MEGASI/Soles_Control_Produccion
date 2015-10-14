using System;
using System.Data.SqlClient;


namespace CrtProduccion.entidades
{
    class dmVehiculoP
    {
        #region Atributos

        public int fld_oldidPart = 0;
        public int fld_oldidSuplido = 0;
        public int fld_idParte { get; set; }
        public int fld_idSuplidor { get; set; }
        public string fld_Referencia { get; set; }
        public string fld_Descripcion { get; set; }
        public double fld_Precio { get; set; }
        public double fld_Existencia { get; set; }
        public string errormsg = "";

        #endregion


        #region Constructores
        public dmVehiculoP()
        {
            limpiar();
        }

        public dmVehiculoP(
            int pidParte,
            int pidSuplidor,
            String PRefencia,
            String pReferencia,
            String pDescripcion,
            Double Pprecio,
            Double PExistencia)
        {
            fld_idParte = pidParte;
            fld_idSuplidor = pidSuplidor;
            fld_Referencia = PRefencia;
            fld_Descripcion = pDescripcion;
            fld_Precio = Pprecio;
            fld_Existencia = PExistencia;
        }

        #endregion
        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idParte = 0;
            fld_idSuplidor = 0;
            fld_Referencia = "";
            fld_Descripcion = "";
            fld_Precio = 0;
            fld_Existencia = 0;

        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_Referencia.Equals(""))
            {
                errormsg = "Descripcion de Vehiculo_Partes no puede estar vacío.";
                lret = false;
            }
            if (lret && fld_Referencia.Equals(""))
            {
                errormsg = "Descripcion de Vehiculo_Partes no puede estar vacío.";
                lret = false;
            }
           
                return lret;
        }
        public int crearDatos()
        {
            fld_idParte = 0;

            if (datamanager.ConexionAbrir())
            {
                // Preparamos consulta para la actualización
                SqlCommand cmd = new SqlCommand("Insert into Vehiculo_Partes(Referencia,descripcion,idSuplidor,precio,existencia)" +
                                                " output INSERTED.idParte" +
                                                " Values(@Referencia,@descripcion,@idSuplidor,@precio,@existencia)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Referencia", fld_Referencia);
                cmd.Parameters.AddWithValue("@descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@idSuplidor", fld_idSuplidor);
                cmd.Parameters.AddWithValue("@precio", Convert.ToDouble(fld_Precio));
                cmd.Parameters.AddWithValue("@existencia",Convert.ToDouble( fld_Existencia));
                
                // Ejecutamos consulta de Actualización
                // y Retornamos el idParte Insertado.
                fld_idParte = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idParte Retornado es Cero
            return fld_idParte ;
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

                    fld_idParte = (int)dr["idParte"];
                    fld_idSuplidor = (int)dr["idSuplidor"];
                    fld_Referencia = dr["Referencia"].ToString();
                    fld_Descripcion = dr["Descripcion"].ToString();
                    fld_Precio = Convert.ToDouble(dr["precio"]);
                    fld_Existencia = Convert.ToDouble(dr["existencia"]);

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
        public bool buscar(String PNParte, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idParte, Referencia,descripcion,idSuplidor,precio,existencia" +
                                               " from Vehiculo_Partes" +
                                               " where Referencia = '" + PNParte + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idParte
        /// </summary>
        /// <param name="idParte"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idParte, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idParte, Referencia,descripcion,idSuplidor,precio,existencia" +
                                               " from Vehiculo_Partes" +
                                               " where idParte = " + idParte.ToString());
            return leerDatos(dr, asignar);
        }
        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer(" select  idParte, Referencia,descripcion,idSuplidor,precio,existencia from Vehiculo_Partes order by idParte desc");
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
                SqlCommand cmd = new SqlCommand("update Vehiculo_Partes" +
                                                " Set Referencia = @Referencia,descripcion=@descripcion,"+
                                                "idSuplidor=@idSuplidor,precio=@Precio,existencia=@existencia" +
                                                " Where idParte = @idParte ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idParte", fld_idParte );
                cmd.Parameters.AddWithValue("@Referencia", fld_Referencia);
                cmd.Parameters.AddWithValue("@descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@idSuplidor", Convert.ToInt32(fld_idSuplidor));
                cmd.Parameters.AddWithValue("@precio", Convert.ToDouble( fld_Precio));
                cmd.Parameters.AddWithValue("@Existencia",Convert.ToDouble( fld_Existencia));

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
        public bool borrarDatos(int pidParte)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from Vehiculo_Partes" +
                                               " where idParte = " + pidParte.ToString());
            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}





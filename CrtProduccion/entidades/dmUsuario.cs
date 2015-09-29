using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmUsuario
    {
        #region Atributos

        public int fld_oldIDUsuario = 0;
        public bool fld_cambiopsw = false;
        public string errormsg = "";

        private int IDUsuario;

        /// <summary>
        ///  ID o Código único que identifica el usuario
        /// </summary>
        public int fld_idusuario
        {
            get { return IDUsuario; }
            set { IDUsuario = value; }
        }


        private string Nombre;
        /// <summary>
        /// Nombre del Usuario
        /// </summary>
        public string fld_nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        private string Clave;
        /// <summary>
        /// Clave encriptada del usuario.
        /// </summary>
        public string fld_clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        #endregion

        #region Constructores

        public dmUsuario()
        {
            limpiar();
        }

        public dmUsuario(int PID_Usuario, String PNombre, String PClave)
        {
            fld_idusuario = PID_Usuario;
            fld_nombre = PNombre;
            fld_clave = PClave;
            fld_cambiopsw = false;
        }
        #endregion

        #region Métodos y Funciones

        public override string ToString()
        {
            return String.Format("{0} - {1}", IDUsuario, Nombre);
        }


        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idusuario = 0;
            fld_nombre = "";
            fld_clave = "";
            fld_cambiopsw = false;
        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;
          
            if (lret && fld_nombre.Equals(""))
            {
                errormsg = "Nombre de usuario no puede estar vacío.";
                lret = false;
            }

            if (lret && fld_clave.Equals(""))
            {
                errormsg = "Clave de usuario no puede estar vacía.";
                lret = false;
            }
            return lret;
        }


        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla segUsuario</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idusuario = 0;

            if (datamanager.ConexionAbrir())
            {

                if (fld_cambiopsw)
                {
                    string encriptClave = datamanager.md5(fld_nombre.Trim() + fld_clave.Trim());
                    fld_clave = encriptClave;
                    fld_cambiopsw = false;
                }

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into segUsuario(NOMBRE, clave)" +
                                                 " output INSERTED.idUsuario" +
                                                 " Values(@NOMBRE, @clave)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Nombre", fld_nombre);
                cmd.Parameters.AddWithValue("@Clave", fld_clave);


                // Ejecutamos consulta de Actualización
                // y Retornamos el idUsuario Insertado.
                fld_idusuario = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idUsuiario Retornado es Cero
            return fld_idusuario;
        }
        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla segUsuario.
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
                    fld_idusuario = (int)dr["IDUsuario"];
                    fld_nombre = dr["Nombre"].ToString();
                    fld_clave = dr["Clave"].ToString();
                    fld_cambiopsw = false;
                }
            }
            else {
                if (asignar) limpiar();
            }

            return encontrado;
        }

        /// <summary>
        ///  Buscar en la tabla de segUsuario por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el usuario.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idUsuario, Nombre, clave" +
                                               " from segUsuario" +
                                               " where Nombre = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segUsuario por el idUsuario
        /// </summary>
        /// <param name="pidUsuario"> código único que identifica el usuario.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int pidUsuario, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idUsuario, Nombre, clave" +
                                               " from segUsuario" +
                                               " where idUsuario = " + pidUsuario.ToString());
            return leerDatos(dr, asignar);
        }

        
        /// <summary>
        /// Lee el último registro insertado en la tabla segUsuario.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segUsuario</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idUsuario, Nombre, clave" +
                                               " from segUsuario" +
                                               " order by idUsuario desc ");
            return leerDatos(dr, true);
        }

      
        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla segUsuario</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {
                if (fld_cambiopsw)
                {
                    string encriptClave = datamanager.md5(fld_nombre.Trim() + fld_clave.Trim());
                    fld_clave = encriptClave;
                    fld_cambiopsw = false;
                }

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("update segUsuario" +
                                                " Set Nombre=@Nombre, Clave = @Clave" +
                                                " Where idUsuario = @idUsuario ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idUsuario", fld_idusuario);
                cmd.Parameters.AddWithValue("@Nombre", fld_nombre);
                cmd.Parameters.AddWithValue("@Clave", fld_clave);

                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }




        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla segUsuario</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidUsuario)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from segUsuario" +
                                               " where idUsuario = " + pidUsuario.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();

            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion

    }

}

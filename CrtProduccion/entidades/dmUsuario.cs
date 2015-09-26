using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmUsuario
    {
        public int fld_oldIDUsuario = 0;
        public bool fld_cambiopsw = false;

        private int IDUsuario;
        public int fld_idusuario
        {
            get { return IDUsuario; }
            set {
                fld_oldIDUsuario = IDUsuario;
                IDUsuario = value;
            }
        }

        private string Nombre;
        public string fld_nombre
        { get { return Nombre; }
          set { Nombre = value; }
        }

        private string Clave;
        public string fld_clave
        { get { return Clave; }
            set { Clave = value; }
        }

        public dmUsuario() {
            limpiar();
        }

        public dmUsuario(int PID_Usuario, String PNombre, String PClave)
        {
            fld_idusuario = PID_Usuario;
            fld_nombre = PNombre;
            fld_clave = PClave;
            fld_cambiopsw = false;
        }


        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar() {
            fld_idusuario  = 0;
            fld_nombre = "";
            fld_clave = "";
            fld_cambiopsw = false;
        }

        // CRUD  -- C = Create
        public int crearDatos() {
            fld_idusuario = 0;

            if (datamanager.ConexionAbrir())
            {

                if (fld_cambiopsw) {
                   string encriptClave = datamanager.md5(fld_nombre.Trim() + fld_clave.Trim());
                    fld_clave = encriptClave;
                    fld_cambiopsw = false;
                }

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand( "Insert into segUsuario(NOMBRE, clave)"+
                                                 " output INSERTED.idUsuario"+
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


        // CRUD  -- R = Read
        public bool leerDatos(SqlDataReader dr)
        {

            bool encontrado = false;
            if (dr.Read())
            {
                fld_idusuario = (int)dr["IDUsuario"];
                fld_nombre = dr["Nombre"].ToString();
                fld_clave = dr["Clave"].ToString();
                encontrado = true;
                fld_cambiopsw = false;
            }
            else limpiar();

            return encontrado;
        }

        public bool buscar(String pNombre) {
            var dr = datamanager.ConsultaLeer("select idUsuario, Nombre, clave"+
                                               " from segUsuario"+
                                               " where Nombre = '" + pNombre + "'");
            return leerDatos(dr);
        }

        public bool buscar(int pidUsuario)
        {
            var dr = datamanager.ConsultaLeer("select idUsuario, Nombre, clave" +
                                               " from segUsuario" +
                                               " where idUsuario = " + pidUsuario.ToString() );
            return leerDatos(dr);
        }


        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idUsuario, Nombre, clave" +
                                               " from segUsuario" +
                                               " order by idUsuario desc " );
            return leerDatos(dr);
        }

        // CRUD  -- U = Update
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



        // CRUD -- D = Delete
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

    }

}

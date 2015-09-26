using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmGrupo
    {
        public int Fld_idGrupo { get; set; }
        public string Fld_NombreGrupo { get; set; }

        public dmGrupo() { }

        public dmGrupo(int Pfld_idGrupo, String PFld_NombreGrupo)

        {
            Fld_idGrupo = Pfld_idGrupo;
            Fld_NombreGrupo = PFld_NombreGrupo;
        }

        public void limpiar()
        {
            Fld_idGrupo = 0;
            Fld_NombreGrupo = "";
            
        }
        public int crearDatos()
        {
            Fld_idGrupo = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into segGrupo(idGrupo, Nombre)" +
                                                 " output INSERTED.idGrupo" +
                                                 " Values(@idGrupo, @Nombre)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idGrupo", Fld_idGrupo);
                cmd.Parameters.AddWithValue("@Nombre", Fld_NombreGrupo);


                // Ejecutamos consulta de Actualización
                // y Retornamos el idGrupo Insertado.
                Fld_idGrupo = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idGrupo Retornado es Cero
            return Fld_idGrupo;
        }


        // CRUD  -- R = Read
        public bool leerDatos(SqlDataReader dr)
        {

            bool encontrado = false;
            if (dr.Read())
            {
                Fld_idGrupo = (int)dr["idGrupo"];
                Fld_NombreGrupo = dr["Nombre"].ToString();
                
                encontrado = true;
            }
            else limpiar();

            return encontrado;
        }

        public bool buscar(String pNombre)
        {
            var dr = datamanager.ConsultaLeer("select idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " where Nombre = '" + pNombre + "'");
            return leerDatos(dr);
        }

        public bool buscar(int idGrupo)
        {
            var dr = datamanager.ConsultaLeer("select idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " where idGrupo = " + idGrupo.ToString());
            return leerDatos(dr);
        }

        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idGrupo, Nombre" +
                                               " from segGrupo" +
                                               " order by idGrupo desc ");
            return leerDatos(dr);
        }

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
                cmd.Parameters.AddWithValue("@idGrupo", Fld_idGrupo);
                cmd.Parameters.AddWithValue("@Nombre", Fld_NombreGrupo);
                ;

                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

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

    
    }
}

    



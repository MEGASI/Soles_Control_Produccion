using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmBrigada
    {
        #region Atributos

        public int fld_olidBrigada = 0;
        public int fld_idBrigada { get; set; }
        public int fld_idVehiculo { get; set; }
        public DateTime fld_Fecha { get; set; }
        public int fld_idChofer { get; set; }
        public int fld_idSupervisor { get; set; }
        public Boolean fld_activa { get; set; }
        public string fld_Chofer { get; set; }
        public string fld_Supervisor { get; set; }
        public string fld_Brigadista { get; set; }
        public int fld_idBrigadista { get; set; }
        public int fld_idturno { get; set; }
        public string errormsg = "";
     

        #endregion


        #region Constructores

        public dmBrigada()
        {
            int pfld_idBrigada = fld_idBrigada;
            int pfld_idVehiculo = fld_idVehiculo;
            DateTime pfld_fecha = fld_Fecha;
            int pfld_idchofer = fld_idChofer;
            int pdfld_idsupervisor = fld_idSupervisor;
            Boolean pfld_activa = fld_activa;
            string pfld_Chofer = fld_Chofer;
            string pfld_Supervisor = fld_Supervisor;
            int pfld_turno = fld_idturno;

        }




        #endregion

        #region Metodos y Funciones


        public void limpiar()
        {
            fld_idBrigada = 0;
            fld_idVehiculo = 0;
            fld_idSupervisor = 0;
            fld_idChofer = 0;
            fld_activa = false;
            fld_Chofer = "";
            fld_Supervisor = "";
            fld_idturno = 0;
            

        }
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_idVehiculo.Equals(""))
            {
                errormsg = "Descripcion de Vehiculo no puede estar vacío.";
                lret = false;
            }


            if (lret && fld_Chofer.Equals(""))
            {
                errormsg = "Chofer no puede estar vacío.";
                lret = false;
            }

            if (lret && fld_Supervisor.Equals(""))
            {
                errormsg = "Supervisor  no puede estar vacío.";
                lret = false;
            }

            return lret;
        }
        public int crearDatos()
        {
            fld_idBrigada = 0;

            if (datamanager.ConexionAbrir())
            {
                // Preparamos consulta para la actualización
                SqlCommand cmd = new SqlCommand("Insert into BrigadaH(idVehiculo,Fecha,idChofer,idSupervisor,Activa)" +
                                                " output INSERTED.idBrigada" +
                                                " Values(@idVehiculo,@Fecha,@idChofer,@idSupervisor,@activa)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
//                cmd.Parameters.AddWithValue("@idVehiculo", fld_idVehiculo);
                cmd.Parameters.AddWithValue("@Fecha", fld_Fecha);
                cmd.Parameters.AddWithValue("@idChofer", Convert.ToInt32(fld_idChofer));
                cmd.Parameters.AddWithValue("@idSupervisor", Convert.ToDouble(fld_idSupervisor));
                cmd.Parameters.AddWithValue("@Activa",fld_activa);



                if (fld_idVehiculo != 0)
                    cmd.Parameters.AddWithValue("@idVehiculo", fld_idVehiculo);
                else
                    cmd.Parameters.AddWithValue("@idVehiculo", DBNull.Value);




                // Ejecutamos consulta de Actualización
                // y Retornamos el idBrigada Insertado.



                fld_idBrigada = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idBrigada Retornado es Cero
            return fld_idBrigada;
        }
        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla BrigadaH.
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
                    fld_idBrigada = (int)dr["idBrigada"];
                   // fld_idVehiculo = (int)dr["idVehiculo"];
                    fld_Fecha =Convert.ToDateTime( dr["Fecha"].ToString());
                   // fld_idturno = (int)dr["idturno"];
                    fld_Chofer = dr["Chofer"].ToString();
                    fld_Supervisor = dr["Supervisor"].ToString();
                    fld_idChofer = (int)dr["idChofer"];
                    fld_idSupervisor = (int)dr["idSupervisor"];
                    fld_activa = Convert.ToBoolean((dr["activa"]));


                    try
                    {
                        fld_idVehiculo = (int)dr["idVehiculo"];

                    }
                    catch (Exception)

                    {
                        fld_idVehiculo = 0;
                    }

                }
            }
            else
            {
                if (asignar) limpiar();
            }
            return encontrado;

        }
        /// <summary>
        ///  Buscar en la tabla de BrigadaH por el Nombre del parte.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica la parte.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String PBrigada, bool asignar)
        {


            var dr = datamanager.ConsultaLeer(" Select B.idBrigada,B.idVehiculo, V.Modelo as Vehiculo," +
                                              " B.Fecha, B.idChofer, L.Nombres as Chofer, B.idSupervisor, LL.Nombres as Supervisor," +
                                              " B.Activa from brigadaH B left outer join Vehiculo  V on B.idVehiculo = V.idVehiculo" +
                                              " left outer join  LibroDirecciones L on L.idLD = B.idChofer" +
                                              " left outer join  LibroDirecciones LL on LL.idLD = B.idSupervisor" +
                                              " Where B.idBrigada = '" + PBrigada + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idBrigada
        /// </summary>
        /// <param name="idBrigada"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idBrigada, bool asignar)
        {


            var dr = datamanager.ConsultaLeer(" Select B.idBrigada,B.idVehiculo, V.Modelo as Vehiculo," +
                                              " B.Fecha, B.idChofer, L.Nombres as Chofer, B.idSupervisor, LL.Nombres as Supervisor," +
                                              " B.Activa from brigadaH B left outer join Vehiculo  V on B.idVehiculo = V.idVehiculo" +
                                              " left outer join  LibroDirecciones L on L.idLD = B.idChofer" +
                                              " left outer join  LibroDirecciones LL on LL.idLD = B.idSupervisor" +
                                              " Where B.idBrigada =" + idBrigada.ToString());

            return leerDatos(dr, asignar);
        }
        /// <summary>
        /// Lee el último registro insertado en la tabla BrigadaH.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla BrigadaH</returns>
        public bool buscarUltimo()
        {

            // var dr = datamanager.ConsultaLeer(" select idBrigada ,Referencia from BrigadaH ");
            var dr = datamanager.ConsultaLeer(" Select top 1 B.idBrigada,B.idVehiculo, V.Modelo as Vehiculo,"+
                                              " B.Fecha, B.idChofer, L.Nombres as Chofer, B.idSupervisor, LL.Nombres as Supervisor,"+
                                              " B.Activa from brigadaH B left outer join Vehiculo  V on B.idVehiculo = V.idVehiculo"+
                                              " left outer join  LibroDirecciones L on L.idLD = B.idChofer"+
                                              " left outer join  LibroDirecciones LL on LL.idLD = B.idSupervisor"+
                                              " order by B.idBrigada desc");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla BrigadaH</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                //SqlCommand cmd = new SqlCommand("update BrigadaH Set Referencia =@Referencia where idBrigada = @idBrigadas");
                SqlCommand cmd = new SqlCommand(" update BrigadaH" +
                                                " Set idVehiculo = @idVehiculo,"+
                                                " Fecha=@Fecha," +
                                                " idChofer=@idChofer,"+
                                                " idSupervisor=@idSupervisor,"+
                                                " Activa=@Activa" +
                                                " Where idBrigada = @idBrigada ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idBrigada", fld_idBrigada);
                cmd.Parameters.AddWithValue("@Fecha", fld_Fecha);
                cmd.Parameters.AddWithValue("@idChofer",Convert.ToInt32(fld_idChofer));
                cmd.Parameters.AddWithValue("@idSupervisor", Convert.ToInt32(fld_idSupervisor));
                cmd.Parameters.AddWithValue("@Activa",fld_activa);



                if (fld_idVehiculo != 0)
                    cmd.Parameters.AddWithValue("@idVehiculo", fld_idVehiculo);
                else
                    cmd.Parameters.AddWithValue("@idVehiculo", DBNull.Value);

                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();
            }
            return lRet > 0;
        }
        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla BrigadaH</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidBrigada)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata(" delete " +
                                                   " from BrigadaH" +
                                                   " where idBrigada = " + pidBrigada.ToString());
            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }


    }
}

#endregion





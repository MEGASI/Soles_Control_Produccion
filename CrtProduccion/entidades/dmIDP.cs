using System;
using System.Data.SqlClient;

namespace CrtProduccion.entidades
{
    class dmIDP
    {

        #region Atributos

        public int fld_olid = 0;
        public int fld_oldBrigada = 0;
        public int fld_id { get; set; }
        public int fld_idp;
        public DateTime fld_Fecha { get; set; }
        public int fld_idProyecto{ get; set; }
        public int fld_idSupervisorLocal { get; set; }
        public int fld_idSupervisorEdeste { get; set; }
        public string fld_supervisorLocal { get; set; }
        public string fld_supervisorEdeste { get; set; }
        public string fld_circuito { get; set; }
        public Boolean fld_diaferiado { get; set; }
        public string fld_Observacion { get; set; }
        public string fld_codigo { get; set; }
        public string fld_cantidad { get; set; }
        public string fld_descripcion { get; set; }
        public string fld_direccion { get; set; }
        public string fld_Nposte { get; set; }
        public int fld_idCodigo { get; set; }
        public string fld_idCodigoac { get; set; }
        public string  fld_precio { get; set; }
        public int fld_idBrigada { get; set; }
        public string fld_idPartida { get; set; }
        public int fld_secuencia { get; set; }




        public string fld_estado { get; set; }
        

        public string errormsg = "";


        #endregion



        #region Constructores

        public dmIDP()
        {
            int pfld_id = fld_id;
            int pfld_idp = fld_idp;
            int pfld_idProyecto = fld_idProyecto;
            DateTime pfld_fecha = fld_Fecha;
            int pfld_idSupervisorLocal = fld_idSupervisorLocal;
            int pfld_idSupervisorEdeste = fld_idSupervisorEdeste;
            Boolean pfld_diaferiado = fld_diaferiado;
            string pfld_Observacion = fld_Observacion;
            string pfld_supervisorLocal = fld_supervisorLocal;
            string pfld_supervisorEdeste = fld_supervisorEdeste;
            string pfld_estado = fld_estado;
            string pfld_circuito = fld_circuito;
            int pfld_idBrigada = fld_idBrigada;
            string pfld_idPartida = fld_idPartida;


        }




        #endregion

        #region Metodos y Funciones


        public void limpiar()
        {
            fld_id= 0;
            fld_idp = 0;
            fld_idProyecto = 0;
            fld_idSupervisorLocal = 0;
            fld_idSupervisorEdeste = 0;
            fld_diaferiado = false;
            fld_Observacion = "";
            fld_supervisorLocal = "";
            fld_supervisorEdeste = "";
            fld_circuito = "";
            fld_descripcion = "";
            fld_direccion = "";
            fld_codigo = "";
            fld_Nposte = "";
            fld_cantidad = "";
            fld_precio = "";
            fld_idBrigada = 0;
            fld_idPartida = "";
            
           


        }

        public bool validar()
        {
            bool lret = true;

            if (lret && fld_idp.Equals(""))
            {
                errormsg = "IDP no puede estar vacio";
                lret = false;
            }
          
            return lret;
        }

        public int crearDatos()
        {
            fld_id = 0;

            if (datamanager.ConexionAbrir())
            {
                // Preparamos consulta para la actualización
                SqlCommand cmd = new SqlCommand(" Insert into IDP_H(idp,fecha,feriado,"+
                                                " idProyecto,circuito,idSuperLocal,"+
                                                " idSuperEde,observacion)" +
                                                " output INSERTED.id" +
                                                " Values(@idp,@Fecha,@feriado,"+
                                                " @idProyecto,@circuito,@idSuperLocal,"+
                                                " @idSuperEde,@observacion)", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@id", fld_id);
                cmd.Parameters.AddWithValue("@idp", fld_idp);
                cmd.Parameters.AddWithValue("@Fecha", fld_Fecha);
                cmd.Parameters.AddWithValue("@feriado", fld_diaferiado);
                cmd.Parameters.AddWithValue("@idProyecto", fld_idProyecto);
                cmd.Parameters.AddWithValue("@circuito", fld_circuito);
                cmd.Parameters.AddWithValue("@idSuperLocal", fld_idSupervisorLocal);
                cmd.Parameters.AddWithValue("@idSuperEde", fld_idSupervisorEdeste);
                cmd.Parameters.AddWithValue("@Observacion",fld_Observacion);
               // cmd.Parameters.AddWithValue("@estado", fld_estado);


                // Ejecutamos consulta de Actualización
                // y Retornamos el idBrigada Insertado.
                fld_id = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idBrigada Retornado es Cero
            return fld_id;
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
                    fld_id = (int)dr["id"];
                    fld_idp = (int)dr["idp"];
                    fld_Fecha = Convert.ToDateTime(dr["Fecha"].ToString());
                    fld_circuito = dr["circuito"].ToString();
                    fld_idProyecto = (int)dr["idProyecto"];
                    fld_diaferiado = Convert.ToBoolean((dr["feriado"]));
                    fld_idSupervisorLocal = (int)dr["idSuperLocal"];
                    fld_idSupervisorEdeste = (int)dr["idSuperEde"];
                    fld_Observacion = dr["Observacion"].ToString();
                    

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
        public bool buscar(String Pid, bool asignar)
        {


            var dr = datamanager.ConsultaLeer(" Select Top 1 H.id, H.idp, H.fecha, H.feriado, H.idProyecto, P.Descripcion as Proyecto, H.circuito," +
                                              " H.idSuperLocal, LD.nombres as SupervisorLocal,H.idSuperEde, LDD.nombres as SupervisorEdeeste," +
                                              " H.observacion, H.estado  from IDP_H H  left outer join" +
                                              " proyecto P on P.idProyecto = P.idProyecto" +
                                              " left outer join vld LD  On LD.idLD = H.idSuperLocal" +
                                              " left outer join  vld LDD on LDD.idLD = H.idSuperEde" +
                                              " Where H.id = '" + Pid + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idBrigada
        /// </summary>
        /// <param name="idBrigada"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idID, bool asignar)
        {


            var dr = datamanager.ConsultaLeer(" Select Top 1 H.id, H.idp, H.fecha, H.feriado, H.idProyecto, P.Descripcion as Proyecto, H.circuito," +
                                              " H.idSuperLocal, LD.nombres as SupervisorLocal,H.idSuperEde, LDD.nombres as SupervisorEdeeste," +
                                              " H.observacion, H.estado  from IDP_H H  left outer join" +
                                              " proyecto P on P.idProyecto = P.idProyecto" +
                                              " left outer join vld LD  On LD.idLD = H.idSuperLocal" +
                                              " left outer join  vld LDD on LDD.idLD = H.idSuperEde" +
                                              " Where H.id =" + idID.ToString());

            return leerDatos(dr, asignar);
        }
        /// <summary>
        /// Lee el último registro insertado en la tabla BrigadaH.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla BrigadaH</returns>
        public bool buscarUltimo()
        {

            
            var dr = datamanager.ConsultaLeer(" Select Top 1 H.id, H.idp, H.fecha, H.feriado, H.idProyecto, P.Descripcion as Proyecto, H.circuito," +
                                              " H.idSuperLocal, LD.nombres as SupervisorLocal,H.idSuperEde, LDD.nombres as SupervisorEdeeste," +
                                              " H.observacion, H.estado  from IDP_H H  left outer join" +
                                              " proyecto P on P.idProyecto = P.idProyecto" +
                                              " left outer join vld LD  On LD.idLD = H.idSuperLocal" +
                                              " left outer join  vld LDD on LDD.idLD = H.idSuperEde"+
                                              " order by H.id desc");
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

                SqlCommand cmd = new SqlCommand(" update IDP_H" +
                                                " Set idp = @idp," +
                                                " fecha=@fecha," +
                                                " feriado=@feriado," +
                                                " idProyecto=@idProyecto," +
                                                " circuito=@circuito," +
                                                " idSuperLocal=@idSuperLocal," +
                                                " idSuperEde=@idSuperEde," +
                                                " observacion=@observacion" +
                                              
                                                " Where id = @id ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización

                cmd.Parameters.AddWithValue("@id", fld_id);
                cmd.Parameters.AddWithValue("@idp", fld_idp);
                cmd.Parameters.AddWithValue("@Fecha", fld_Fecha);
                cmd.Parameters.AddWithValue("@feriado", fld_diaferiado);
                cmd.Parameters.AddWithValue("@idProyecto", fld_idProyecto);
                cmd.Parameters.AddWithValue("@circuito", fld_circuito);
                cmd.Parameters.AddWithValue("@idSuperLocal", fld_idSupervisorLocal);
                cmd.Parameters.AddWithValue("@idSuperEde", fld_idSupervisorEdeste);
                cmd.Parameters.AddWithValue("@Observacion", fld_Observacion);
                //cmd.Parameters.AddWithValue("@estado", fld_estado);

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
        public bool borrarDatos(int pid)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata(" delete " +
                                                   " from IDP_H" +
                                                   " where id = " + pid.ToString());
            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }


        public int crearDatos1()
        {
            fld_idBrigada = 0;

            if (datamanager.ConexionAbrir())
            {
                // Preparamos consulta para la actualización
                SqlCommand cmd = new SqlCommand(" Insert into IDP_Brigada(id,secuencia)" +
                                                " output INSERTED.idBrigada" +
                                                " Values(@id,@secuencia)", datamanager.ConexionSQL);




                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@id", fld_id);
                cmd.Parameters.AddWithValue("@secuencia", fld_secuencia);
           


                // Ejecutamos consulta de Actualización
                // y Retornamos el idBrigada Insertado.
                fld_idBrigada = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idBrigada Retornado es Cero
            return fld_id;
        }
        public bool actualizarDatos1()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización

                SqlCommand cmd = new SqlCommand(" update IDP_Brigada" +
                                                " set id=@id," +
                                                " secuencia=@secuencia" +
                                                " Where idBrigada = @idBrigada ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización

                cmd.Parameters.AddWithValue("@id", fld_id);
                cmd.Parameters.AddWithValue("@secuencia", fld_secuencia);
               

                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();
            }
            return lRet > 0;
        }
    }
}

#endregion







using System;
using System.Data.SqlClient;


namespace CrtProduccion.entidades
{
    class dmVehiculo
    {
        #region Atributos 

        public int fld_idVehiculo = 0;
        public int fld_oldidVehiculo = 0;
        public string fld_Ficha { get; set; }
        public int fld_idMarca { get; set; }
        public string fld_modelo { get; set; }
        public int fld_idTipoVehiculo { get; set; }
        public string fld_placa { get; set; }
        public string fld_Descripcion { get; set; }
        public int fld_ano { get; set; }
        public string fld_chasis { get; set; }
        public int fld_idColor { get; set; }
        public int fld_idLllantas { get; set; }
        public int fld_idfilAceite { get; set; }
        public string fld_idEstado { get; set; }
        public DateTime fld_SegVence { get; set; }
        public DateTime fld_ultMant { get; set; }
        public Double fld_Kilometraje { get; set; }
        public byte fld_Photo { get; set; }
        public int fld_idParte { get; set; }
        public string fld_Llantas { get; set; }
        public string fld_FiltAceite { get; set; }
        public string errormsg = "";


        #endregion


        #region Constructores


        public dmVehiculo()
        {
            limpiar();
        }

        public dmVehiculo(
            int pfld_idVehiculo,
            int pfld_idMarca,
            int pfld_ano,
            int pfld_idllantas,
            int pfld_idaceite,
            int pfld_idColor,
            int pfld_idTipoVehiculo,
            string pfld_Ficha,
            string pfld_Descripcion,
            string pfld_Modelo,
            string pfld_Placa,
            string pfld_chasis,
            string Pfld_idEstado,
            byte Pfld_Photo,
            DateTime pfld_SegVence,
            DateTime pfld_ulMant,
            Double pfld_kilometraje
            )
        

        {
            fld_idVehiculo = pfld_idVehiculo;
            fld_idMarca = pfld_idMarca;
            fld_ano = pfld_ano;
            fld_idLllantas = pfld_idllantas;
            fld_idfilAceite = pfld_idaceite;
            fld_Ficha = pfld_Ficha;
            fld_Descripcion = pfld_Descripcion;
            fld_modelo = pfld_Modelo;
            fld_placa = pfld_Placa;
            fld_chasis = pfld_chasis;
            fld_idEstado = Pfld_idEstado;
            fld_idTipoVehiculo = pfld_idTipoVehiculo;
            fld_idColor = pfld_idColor;
            fld_SegVence = pfld_SegVence;
            fld_ultMant = pfld_ulMant;
            fld_Kilometraje = pfld_kilometraje;
            fld_Photo = Pfld_Photo;



        }



#endregion



        #region Métodos y funciones

        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idVehiculo = 0;
            fld_idMarca = 0;
            fld_ano = 0;
            fld_idLllantas = 0;
            fld_idfilAceite = 0;
            fld_Ficha = "";
            fld_Descripcion = "";
            fld_modelo = "";
            fld_placa = "";
            fld_chasis = "";
            fld_idEstado = "";
            fld_idTipoVehiculo = 0;
            fld_idColor = 0;
            fld_Kilometraje = 0;
            fld_Llantas = "";
            fld_FiltAceite = "";

        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_Ficha.Equals(""))
            {
                errormsg = "Ficha no  puede estar vacío.";
                lret = false;
            }

            if (lret &&fld_Descripcion.Equals(""))
            {
                errormsg = "Descripcion no puede estar vacío.";
                lret = false;
            }


            return lret;
        }

        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla departamento</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
           fld_idVehiculo = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                    SqlCommand cmd = new SqlCommand("Insert into Vehiculo(Ficha,Descripcion,idMarca,Modelo,"+
                    " idTipoVehiculo,placa,ano,chasis,"+
                    " idColor,idllantas,idFiltAceite,idEstado," +
                    " SeguroVence,ultMantenim, " +
                    " Kilometraje) output INSERTED.idVehiculo" +
                    " Values(@Ficha,@Descripcion,@idMarca,@Modelo,"+
                    " @idTipoVehiculo,@Placa,@ano,@chasis,@idColor,"+
                    " @idLlantas,@idFiltAceite,@idEstado,@SeguroVence," +
                    " @ultMantenim,@kilometraje)", datamanager.ConexionSQL);




                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Ficha", fld_Ficha);
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@idMarca", fld_idMarca);
                cmd.Parameters.AddWithValue("@Modelo", fld_modelo);
                cmd.Parameters.AddWithValue("@idTipoVehiculo",fld_idTipoVehiculo);
                cmd.Parameters.AddWithValue("@placa", fld_placa);
                cmd.Parameters.AddWithValue("@ano", fld_ano);
                cmd.Parameters.AddWithValue("@chasis", fld_chasis);
                cmd.Parameters.AddWithValue("@idColor", fld_idColor);
                cmd.Parameters.AddWithValue("@idllantas", fld_idLllantas);
                cmd.Parameters.AddWithValue("@idFiltAceite", fld_idfilAceite);
                cmd.Parameters.AddWithValue("@idEstado", fld_idEstado);
                cmd.Parameters.AddWithValue("@seguroVence", fld_SegVence);
                cmd.Parameters.AddWithValue("@ultMantenim", fld_ultMant);
                cmd.Parameters.AddWithValue("@kilometraje", fld_Kilometraje);
                //cmd.Parameters.AddWithValue("@photo", fld_Photo);



           

                // Ejecutamos consulta de Actualización
                // y Retornamos el idGrupo Insertado.
                fld_idVehiculo = (int)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idVehiculo Retornado es Cero
            return fld_idVehiculo;
        }
        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla departamento.
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
                    fld_idVehiculo = (int)dr["idVehiculo"];
                    fld_Ficha = dr["Ficha"].ToString();
                    fld_Descripcion = dr["Descripcion"].ToString();
                    fld_modelo = dr["Modelo"].ToString();
                    fld_placa = dr["placa"].ToString();
                    fld_ano = (int)dr["ano"];
                    fld_chasis = dr["chasis"].ToString();
                    fld_idEstado = dr["idestado"].ToString();
                    fld_SegVence=Convert.ToDateTime(dr["seguroVence"].ToString());
                    fld_ultMant = Convert.ToDateTime(dr["ultMantenim"].ToString());
                    fld_Kilometraje = Convert.ToDouble(dr["kilometraje"].ToString());
                    fld_idLllantas = (int)dr["idLlantas"];
                    fld_Llantas = dr["Llanta"].ToString();
                    fld_FiltAceite = dr["filtroAceite"].ToString();
                    fld_idfilAceite = (int)dr["idFiltAceite"];

                    //  Lo Capturamos Con un try  para poder leer
                    try { fld_idColor = Convert.ToSByte((int)dr["idColor"]); }
                    catch (Exception) { fld_idColor = 0; }
                    try { fld_idTipoVehiculo = (int)dr["idTipoVehiculo"]; }
                    catch (Exception) { fld_idTipoVehiculo = 0; }
                    try{ fld_idMarca = (int)dr["idMarca"];}
                    catch (Exception){fld_idMarca = 0;}
                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }
        /// <summary>
        ///  Buscar en la tabla de Vehiculo Descripcion.
        /// </summary>
        /// <param name="pNombre"> Descripcion único que identifica el Vehiculo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {

            var dr = datamanager.ConsultaLeer(" SELECT  v.idVehiculo, v.Ficha, v.descripcion,v.idMarca,vm.Descripcion as Marca,"+
                                              " v.modelo, v.idTipoVehiculo,vt.descripcion as TipoVehiculo," +
                                              " v.placa, v.ano, v.chasis, v.idColor,vc.Descripcion as Color," +
                                              " v.idllantas, ll.descripcion AS llanta," +
                                              " v.idFiltAceite, fa.descripcion AS filtroAceite,v.idEstado," +
                                              " v.seguroVence, v.ultMantenim, v.kilometraje, v.photo" +
                                              " FROM  Vehiculo AS v" +
                                              " LEFT OUTER JOIN Vehiculo_Partes AS fa ON v.idFiltAceite = fa.idParte" +
                                              " LEFT OUTER JOIN Vehiculo_Partes AS ll ON v.idllantas = ll.idParte " +
                                              " LEFT OUTER JOIN Vehiculo_Tipo AS vt ON v.idTipoVehiculo =vt.idTipoVehiculo" +
                                              " LEFT OUTER JOIN Vehiculo_Marca AS vm ON v.idMarca =vm.idMarca" +
                                              " LEFT OUTER JOIN color AS vc ON v.idColor =vc.idColor" +
                                              " Where v.idVehiculo =" + pNombre.ToString());
            
         
            
            
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de Vehiculo por el idGrupo
        /// </summary>
        /// <param name="idGrupo"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idVehiculo, bool asignar)
        {
          var dr = datamanager.ConsultaLeer(" SELECT  v.idVehiculo, v.Ficha, v.descripcion, v.idMarca,vm.descripcion as Marca,"+
                                            " v.modelo, v.idTipoVehiculo,vt.descripcion as TipoVehiculo," +
                                            " v.placa, v.ano, v.chasis, v.idColor,vc.Descripcion as Color," +
                                            " v.idllantas, ll.descripcion AS llanta," +
                                            " v.idFiltAceite, fa.descripcion AS filtroAceite,v.idEstado," +
                                            " v.seguroVence, v.ultMantenim, v.kilometraje, v.photo" +
                                            " FROM  Vehiculo AS v" +
                                            " LEFT OUTER JOIN Vehiculo_Partes AS fa ON v.idFiltAceite = fa.idParte" +
                                            " LEFT OUTER JOIN Vehiculo_Partes AS ll ON v.idllantas = ll.idParte " +
                                            " LEFT OUTER JOIN Vehiculo_Tipo AS vt ON v.idTipoVehiculo = vt.idTipoVehiculo" +
                                            " LEFT OUTER JOIN Vehiculo_Marca AS vm ON v.idMarca = vm.idMarca" +
                                            " LEFT OUTER JOIN color AS vc ON v.idColor =vc.idColor" +
                                            " Where v.idVehiculo =" + idVehiculo.ToString());

            
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla Vehiculo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla Vehiculo</returns>
        public bool buscarUltimo()
        {


          var dr = datamanager.ConsultaLeer(" SELECT  v.idVehiculo, v.Ficha, v.descripcion, v.idMarca,vm.Descripcion as Marca,"+
                                            " v.modelo, v.idTipoVehiculo,vt.descripcion as TipoVehiculo," +
                                            " v.placa, v.ano, v.chasis, v.idColor,vc.Descripcion as Color," +
                                            " v.idllantas, ll.descripcion AS llanta," +
                                            " v.idFiltAceite, fa.descripcion AS filtroAceite,v.idEstado," +
                                            " v.seguroVence, v.ultMantenim, v.kilometraje, v.photo" +
                                            " FROM  Vehiculo AS v" +
                                            " LEFT OUTER JOIN Vehiculo_Partes AS fa ON v.idFiltAceite = fa.idParte" +
                                            " LEFT OUTER JOIN Vehiculo_Partes AS ll ON v.idllantas = ll.idParte " +
                                            " LEFT OUTER JOIN Vehiculo_Tipo AS vt ON v.idTipoVehiculo =vt.idTipoVehiculo" +
                                            " LEFT OUTER JOIN Vehiculo_Marca AS vm ON v.idMarca = vm.idMarca" +
                                            " LEFT OUTER JOIN color AS vc ON v.idColor =vc.idColor" +
                                            " Order by v.idVehiculo desc "); 

            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla Vehiculo</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand(" update Vehiculo" +
                                                " Set Ficha=@Ficha, Descripcion = @Descripcion," +
                                                " idMarca=@idMarca,Modelo=@Modelo,idTipoVehiculo=@idTipoVehiculo," +
                                                " Placa=@Placa,ano=@ano,chasis=@chasis,idColor=@idColor,idllantas=@idllantas,"+
                                                " idFiltAceite=@idFiltAceite,idEstado=@idEstado,seguroVence=@seguroVence,"+
                                                " ultMantenim=@ultMantenim,kilometraje=@kilometraje" +
                                                " Where idVehiculo = @idVehiculo", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idVehiculo", fld_idVehiculo);
                cmd.Parameters.AddWithValue("@Ficha", fld_Ficha);
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);                
                cmd.Parameters.AddWithValue("@idMarca", fld_idMarca);
                cmd.Parameters.AddWithValue("@Modelo", fld_modelo);
                cmd.Parameters.AddWithValue("@idTipoVehiculo",fld_idTipoVehiculo);
                cmd.Parameters.AddWithValue("@placa", fld_placa);
                cmd.Parameters.AddWithValue("@ano", fld_ano);
                cmd.Parameters.AddWithValue("@chasis", fld_chasis);
                cmd.Parameters.AddWithValue("@idColor", fld_idColor);
                cmd.Parameters.AddWithValue("@idllantas", fld_idLllantas);
                cmd.Parameters.AddWithValue("@idFiltAceite", fld_idfilAceite);
                cmd.Parameters.AddWithValue("@idEstado", fld_idEstado);
                cmd.Parameters.AddWithValue("@seguroVence", fld_SegVence);
                cmd.Parameters.AddWithValue("@ultMantenim", fld_ultMant);
                cmd.Parameters.AddWithValue("@kilometraje", fld_Kilometraje);
                // cmd.Parameters.AddWithValue("@photo", Convert.ToByte(fld_Photo));

                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla departamento</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidVehiculo)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from Vehiculo" +
                                               " where idVehiculo = " + pidVehiculo.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        
    }
}
#endregion


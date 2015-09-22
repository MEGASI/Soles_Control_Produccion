using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System.Security.Cryptography;


namespace CrtProduccion
{
    public static class datamanager
    {
        
         public static string cadenadeconexion
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["dbcnn"].ConnectionString;
            }
            private set { }
        }

        public static SqlConnection ConexionSQL;

        public static Dictionary<string, tpermiso> permisos = new Dictionary<string, tpermiso>();

        public static String loginName { get; private set; }
        public static int IdUsuario { get; private set; }

        public static Boolean ConexionAbrir()
        {
            Boolean ret = true;
            try
            {
                ConexionSQL = new SqlConnection(cadenadeconexion);
                ConexionSQL.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ret = false;
            }
            return ret;
        }
        // Fin ConexionCerrar

        public static Boolean ConexionCerrar()
        {
            Boolean ret = true;
            try
            {
                ConexionSQL = new SqlConnection(cadenadeconexion);
                ConexionSQL.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ret = false;
            }
            return ret;
        }
        // Fin ConexionCerra

        public static SqlDataReader ConsultaLeer(string cmdSQL)
        {
            SqlDataReader reader = null;
            if (ConexionAbrir())
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    cmd.CommandText = cmdSQL;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = ConexionSQL;
                    reader = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    reader = null;
                }
                ConexionCerrar();
            }
            return reader;
        }
        // Fin ConsultaLeer

        public static DataSet ConsultaDatos(string cmdSQL)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            if (ConexionAbrir())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    cmd.Connection = ConexionSQL;
                    cmd.CommandText = cmdSQL;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    ds = null;
                }
                ConexionCerrar();
            }
            return ds;
        }
        // Fin ConsultaDatos

        public static Boolean ValidarUsuario(String pnombre, String pclave)
        {
            Boolean lRet = false;
            String lpassword = "";
            int lidUsuario = 0;

            // debe encriptar la clave para eso  concatenar pnombre y pclave

            if (ConexionAbrir())
            {
                



                var dr = ConsultaLeer("Select idUsuario, Clave  from segUsuario where  Nombre='" + pnombre + "'");
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        lidUsuario = dr.GetInt32(0);
                        lpassword = dr.GetString(1);

                        // Comparar  con la clave encriptada
                        if (pclave.Equals(lpassword))
                        {
                            lRet = true;
                            // Asigno valor a propiedades de la clase.
                            loginName = pnombre;
                            IdUsuario = lidUsuario;
                            cargaPermisos(IdUsuario);
                        }
                    }
                }
            }
            return lRet;
        }
        // Fin validarUsuario 


        /// <summary>
        /// <para>Carga los permisos a una coleccion de datos para tenerlo disponible de forma local en memoria.</para>
        /// <para>Es aconsejable ejecutar este método luego del login del usuario, de manera que se carguen todos los permisos.</para>
        /// <para>para poder hacer uso del método datamanager.probarPermiso(), tanto en las opciones del menu, como en los</para>
        /// <para>formularios durante el funcionamiento de la aplicación.</para>
        /// </summary>
        /// <param name="idUsuario">Es el id del usuario que extraemos de la tabla seguridadItem en el login.</param>
        /// <returns>retorna un true si se pudieron cargar los permisos y falso en caso contrario.</returns>
        public static Boolean cargaPermisos(int idUsuario)
        {
            Boolean lret = false;
            // Asegurarno que hay conexion a SQL y que se puede abrir
            if (ConexionAbrir())
            {
                // Llenar dataReader con los permisos
                var dr = ConsultaLeer("exec dbo.segPermiso " + idUsuario.ToString() + ",  null");

                // Si el datareader se creo
                if (dr != null)
                {
                    lret = dr.HasRows;

                    // Si el dataReader tiene un registro
                    while (dr.Read())
                    {
                        // Asigno los valores que trae la consulta a cada uno de los elementos del Diccionario
                        permisos.Add(dr.GetString(4).Trim(),
                                new tpermiso(dr.GetString(4).Trim(),
                                             (Boolean)dr.GetSqlBoolean(0),
                                             (Boolean)dr.GetSqlBoolean(1),
                                             (Boolean)dr.GetSqlBoolean(2),
                                             (Boolean)dr.GetSqlBoolean(3)));

                    } //  if (dr.Read())

                }  // if (dr != null)

            } //  (datamanager.ConexionAbrir())


            return lret;
        }
        // Fin cargaPermisos

        /// <summary>
        /// <para>Obtiene el estado del permiso para un deteterminado item de seguridad y un tipo de permiso determinado.</para>
        /// </summary>
        /// <param name="idItem">Es un string que identifica un elemento de seguridad que puede ser un formulario o una opcion.
        /// estos estan definidos en la base de datos en la tabla seguridadItem</param>
        /// <param name="tipoPermiso"> puede ser "acceso", "crear","modificar","borrar". </param>         
        /// <returns>retorna un true si es permitido o false cuando es denegado el permiso.</returns>
        public static Boolean probarPermiso(String idItem, String tipoPermiso)
        {
            Boolean lret = false;
            tpermiso lpermiso = new tpermiso(idItem, false, false, false, false);

            if (permisos.ContainsKey(idItem))
            {
                lpermiso = permisos[idItem];

                switch (tipoPermiso)
                {
                    case "acceso":
                        lret = lpermiso.acceso;
                        break;
                    case "crear":
                        lret = lpermiso.crear;
                        break;
                    case "modificar":
                        lret = lpermiso.modificar;
                        break;
                    case "borrar":
                        lret = lpermiso.borrar;
                        break;
                    default:
                        lret = false;
                        break;
                }
            }

            return lret;
        }

    }
    // Fin  datamanager


    // clase con la estrcutura de los permisos.
    public class tpermiso
    {
        public string idItem;
        public readonly Boolean acceso;
        public readonly Boolean crear;
        public readonly Boolean modificar;
        public readonly Boolean borrar;

        public tpermiso(string idItem, Boolean acceso, Boolean crear, Boolean modificar, Boolean borrar)
        {
            this.idItem = idItem;
            this.acceso = acceso;
            this.crear = crear;
            this.modificar = modificar;
            this.borrar = borrar;
        }
    }
    // Fin Seguridad


}

        
        
        
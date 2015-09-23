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
        public static SqlConnection ConexionSQL;
        public static Dictionary<string, tpermiso> permisos = new Dictionary<string, tpermiso>();

        /// <summary>
        /// <para>Cadena de conexión a la base de datos extraida del archivo App.config.</para>
        /// </summary>
        public static string cadenadeconexion
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["dbcnn"].ConnectionString;
            }
            private set { }
        }

        public static string loginName { get; private set; }
        public static int idUsuario { get; private set; }


        public static bool ConexionAbrir()
        {
            bool ret = true;
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
        // Fin ConexionAbrir

        public static bool ConexionCerrar()
        {
            bool ret = true;
            try
            {
                if (ConexionSQL.State == System.Data.ConnectionState.Open)
                ConexionSQL.Close();
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
                //ConexionCerrar();
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


        public static bool ConsultaNodata(string cmdSQL)
        {
            int lret = 0;

            if (ConexionAbrir())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    cmd.Connection = ConexionSQL;
                    cmd.CommandText = cmdSQL;

                    lret = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
              
                }
                ConexionCerrar();
            }
            return (lret>0);
        }
        // Fin ConsultaDatos



        /// <summary>
        /// <para>Valida el nombre de usuario y la clave, método usado durante el login.</para>
        /// </summary>
        /// <param name="pnombre">Es el nombre del usuario utilizado para iniciar seccion en el sistema.</param>
        /// <param name="pclave">Es la clave del usuario sin encriptar, utilizada para iniciar seccion.</param>
        /// <returns>retorna un true si el nombre y la clave son correctos.</returns>
        public static bool ValidarUsuario(string pnombre, string pclave)
        {
            bool lRet = false;
            string lpassword = "";
            int lidUsuario = 0;
            string lEncriptPsw = md5(pnombre.Trim() + pclave.Trim());
            //System.Windows.Clipboard.SetText(lEncriptPsw);

            if (ConexionAbrir())
            {
                var dr = ConsultaLeer("Select idUsuario, Clave  from segUsuario where  Nombre='" + pnombre + "'");
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        lidUsuario = dr.GetInt32(0);
                        lpassword = dr.GetString(1);


                        if (lEncriptPsw.Equals(lpassword))
                        {
                            lRet = true;
                            // Asigno valor a propiedades de la clase.
                            loginName = pnombre;
                            idUsuario = lidUsuario;

                            // Cargo los permisos
                            cargaPermisos(idUsuario);
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
        public static bool cargaPermisos(int idUsuario)
        {
            bool lret = false;
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
                                             (bool)dr.GetSqlBoolean(0),
                                             (bool)dr.GetSqlBoolean(1),
                                             (bool)dr.GetSqlBoolean(2),
                                             (bool)dr.GetSqlBoolean(3)));

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
        public static bool probarPermiso(string idItem, string tipoPermiso)
        {
            bool lret = false;
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



        /// <summary>
        /// <para>Encripta una cadena de caracteres al formato MD5.</para>
        /// </summary>
        /// <param name="Value">Cadena de cualquier longitud que se quiera encriptar.</param>         
        /// <returns>Cadena de caracteres encriptada en formato MD5</returns>
        public static string md5(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();

            // Invertir el md5
            String ret2 = "";
            for (int i = ret.Length - 1; i >= 0; i--)
                ret2 = ret2 + ret.Substring(i, 1);

            return ret2;
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

        
        
        
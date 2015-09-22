using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;



namespace CrtProduccion
{


    public class Conexion
    {

        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlConnection Conn = new SqlConnection();
        public static SqlConnection ObtenerCOnexion()
        {
            {

                SqlConnection Conn = new SqlConnection("Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015");
                return Conn;
            }
        }
   }
}
   
    




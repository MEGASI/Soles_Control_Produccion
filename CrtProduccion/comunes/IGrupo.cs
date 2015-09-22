using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CrtProduccion
{
 public class IGrupo
    {
        public static int Agregar(Group PAgregarG)
        {
            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {
                Cnn.Open();
                SqlCommand Cmd = new SqlCommand("insert into segGrupo (NOMBRE ) values (@NOMBRE) ", Cnn);
  
                Cmd.Parameters.AddWithValue("@NOMBRE", PAgregarG.NombreG);

                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();
            }
            return retorno;

        }

        public static int Modificar(Group PModificar)
        {

            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {

                Cnn.Open();

                SqlCommand Cmd = new SqlCommand("update segGrupo set Nombre=@Nombre where idGrupo= @idGrupo ", Cnn);

                Cmd.Parameters.AddWithValue("@idGrupo", PModificar.ID_Group);
                Cmd.Parameters.AddWithValue("@NOMBRE", PModificar.NombreG);
                
                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();

            }
            return retorno;

        }


        public static int Eliminar(Group PEliminar)
        {

            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {

                Cnn.Open();

                SqlCommand Cmd = new SqlCommand("update segGrupo set Nombre=@Nombre where idGrupo= @idGrupo ", Cnn);

                Cmd.Parameters.AddWithValue("@idGrupo", PEliminar.ID_Group);
                Cmd.Parameters.AddWithValue("@NOMBRE", PEliminar.NombreG);

                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();

            }
            return retorno;

        }




















    }
}

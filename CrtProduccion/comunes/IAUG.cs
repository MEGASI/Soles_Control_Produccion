using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion
{
     public class IAUG
    {

        public static int AgregarID(AUG PAgregarID)
        {

            int retorno = 0;

            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {
                Cnn.Open();

                SqlCommand cmd = new SqlCommand("insert into segGrupoUsuario(idGrupo, idUsuario) Values (@idGrupo,@idUsuario)", Cnn);

                cmd.Parameters.AddWithValue("idGrupo",Convert.ToInt16( PAgregarID.idGrupo));
                cmd.Parameters.AddWithValue("idUsuario", Convert.ToInt16(PAgregarID.idUsuario));
                



                retorno = cmd.ExecuteNonQuery();
                Cnn.Close();

            }
                return retorno;
        }


        public static int Actualizar(AUG PActualizar)
        {

            int retorno = 0;

            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {
                Cnn.Open();



                SqlCommand cmd = new SqlCommand ("update  segGrupoUsuario set  idGrupo = @idGrupo , idUsuario = @idUsuario  where  idGrupo =@idGrupo and idUsuario = @idUsuario", Cnn);
                cmd.Parameters.AddWithValue("idGrupo", Convert.ToInt16(PActualizar.idGrupo));
                cmd.Parameters.AddWithValue("idUsuario", Convert.ToInt16(PActualizar.idUsuario));




                retorno = cmd.ExecuteNonQuery();
                Cnn.Close();

            }
            return retorno;
        }











    }
}

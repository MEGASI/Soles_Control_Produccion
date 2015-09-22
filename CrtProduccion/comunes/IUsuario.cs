using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CrtProduccion
{
    public class IUsuario
    {

        public static int Agregar(Usuario PAgregar)
        {

            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {

                Cnn.Open();


                SqlCommand Cmd = new SqlCommand("insert into segUsuario (NOMBRE, clave) values (@NOMBRE,@clave) ", Cnn);

                Cmd.Parameters.AddWithValue("@Nombre", PAgregar.Nombre);
                Cmd.Parameters.AddWithValue("@Clave", PAgregar.Clave);



                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();



            }


            return retorno;


        }

        public static int Modificar(Usuario PModificar)
        {

            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {

                Cnn.Open();


                SqlCommand Cmd = new SqlCommand("update segUsuario set Nombre=@Nombre ,Clave=@Clave where idUsuario= @idUsuario ", Cnn);

                Cmd.Parameters.AddWithValue("@idUsuario", PModificar.ID_Usuario);
                Cmd.Parameters.AddWithValue("@Nombre", PModificar.Nombre);
                Cmd.Parameters.AddWithValue("@Clave", PModificar.Clave);



                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();



            }


            return retorno;


        }


        public static int Eliminar(Usuario PEliminar)
        {

            int retorno = 0;
            using (SqlConnection Cnn = Conexion.ObtenerCOnexion())
            {

                Cnn.Open();


                SqlCommand Cmd = new SqlCommand("delete  from  segUsuario  where idUsuario= @idUsuario ", Cnn);

                Cmd.Parameters.AddWithValue("@idUsuario", PEliminar.ID_Usuario);
                Cmd.Parameters.AddWithValue("@Nombre", PEliminar.Nombre);
                Cmd.Parameters.AddWithValue("@Clave", PEliminar.Clave);



                retorno = Cmd.ExecuteNonQuery();
                Cnn.Close();



            }


            return retorno;


        }


    }
}

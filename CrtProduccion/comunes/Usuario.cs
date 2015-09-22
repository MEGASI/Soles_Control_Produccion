using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CrtProduccion
{
     public class Usuario
    {
         public int ID_Usuario { get; set; }
        public string Nombre { get; set; }
         public string Clave { get; set; }


        public Usuario() { }



        public Usuario(int PID_Usuario,String PNombre,String PClave  )

        {

            this.ID_Usuario = PID_Usuario;
            this.Nombre = PNombre;
            this.Clave = PClave;


        }




    }
}

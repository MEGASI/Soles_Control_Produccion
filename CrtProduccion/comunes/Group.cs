using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrtProduccion
{
     public class Group
    {
         public int ID_Group { get; set; }
        public string NombreG { get; set; }



        public Group() { }


        public Group( int PID_Group, String PNombreG)

        {
            this.ID_Group = PID_Group;
            this.NombreG = PNombreG;


        }
    }
}

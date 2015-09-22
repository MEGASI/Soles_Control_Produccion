using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrtProduccion
{
    public class AUG
    {
        public int idGrupo { get; set; }
        public int idUsuario { get; set; }
        public int Miembro { get; set; }
        public AUG() { }

        public AUG( int PidGrupo, int PidUsuario, int PMiembro)

        {
            this.idGrupo = PidGrupo;
            this.idUsuario = PidUsuario;
            this.Miembro = PMiembro;


        }

    }
}

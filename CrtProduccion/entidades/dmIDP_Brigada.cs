using System;
using System.Data.SqlClient;



namespace CrtProduccion.entidades
{
    class dmIDP_Brigada
    {


        #region Atributos

        public int fld_oldidBrigada = 0;
        public int fld_idBrigada = 0;
        public int fld_id { get; set; }
        public int fld_secuencia { get; set; }
        public string errormsg = "";

        #endregion

        public dmIDP_Brigada()
        {
            int pfld_oldidBrigada = fld_idBrigada;
            int pfld_id = fld_id;
            int pfld_secuencia = fld_secuencia;


        }





    }

}


   




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrtProduccion
{
    public class ComboBoxItem
    {
        public string Name;
        public object Value;

        public int intValue { get; set; }

        public ComboBoxItem(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public ComboBoxItem(string Name, int intValue)
        {
            this.Name = Name;
            this.intValue = intValue;
        }

        // override ToString() function
        public override string ToString()
        {
            return this.Name;
        }

    }
    // fin ComboBoxItem

    /// <summary>
    /// Clase CBox donde el valor entero puede ser nulo.
    /// </summary>
    public class CBoxNullItem
    {
        public string Name;
        public object Value;

        //public int? intValue { get; set; }

        public CBoxNullItem(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public CBoxNullItem(string Name, int? intValue)
        {
            this.Name = Name;
           // this.intValue = intValue;
            this.Value = intValue; 
        }

        // override ToString() function
        public override string ToString()
        {
            return this.Name;
        }

    }
    // fin CBoxNullItem

    public class Permiso
    {
        // Definir atributos de la clase permiso.
        public Boolean acceso { get; private set; }
        public Boolean crear { get; private set; }
        public Boolean modificar { get; private set; }
        public Boolean borrar { get; private set; }

        // Constructor con el parametro idUSuario  e idSegItem
        public Permiso(int idUsuario, String idSegItem)
        {
            cargar(idUsuario, idSegItem);
        }
        // Carga los permisos correspondiente para un usuario y un item de seguridad
        public void cargar(int idUsuario, String idSegItem)
        {
            this.acceso = false;
            this.crear = false;
            this.modificar = false;
            this.borrar = false;
            // Asegurarno que hay conexion a SQL y que se puede abrir
            if (datamanager.ConexionAbrir())
            {
                // Llenar dataReader con los permisos
                var dr = datamanager.ConsultaLeer("exec dbo.segPermiso " + idUsuario.ToString() + ", '" + idSegItem + "'");

                // Si el datareader se creo
                if (dr != null)
                {
                    // Si el dataReader tiene un registro
                    if (dr.Read())
                    {   // Asigno los valores que trae la consulta a las propiedades de esta clase.
                        this.acceso = (Boolean)dr.GetSqlBoolean(0);
                        this.crear = (Boolean)dr.GetSqlBoolean(1);
                        this.modificar = (Boolean)dr.GetSqlBoolean(2);
                        this.borrar = (Boolean)dr.GetSqlBoolean(3);
                    } //  if (dr.Read())
                }  // if (dr != null


            } //  (datamanager.ConexionAbrir())
        }
    }
    // Fin Permiso


}

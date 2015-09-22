using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CrtProduccion
{
     public class Permiso25
    {
        class ComboBoxItem
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
    }
}


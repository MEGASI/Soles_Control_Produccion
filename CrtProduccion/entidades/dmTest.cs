using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrtProduccion.entidades
{
    class dmTest : DependencyObject
    {


        public DateTime FechaIngeso
        {
            get { return (DateTime)GetValue(FechaIngesoProperty); }
            set { SetValue(FechaIngesoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FechaIngeso.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FechaIngesoProperty =
            DependencyProperty.Register("FechaIngeso", typeof(DateTime), typeof(dmTest));


        public string Apellidos
        {
            get { return GetValue(ApellidosProperty) as string; }
            set { SetValue(ApellidosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Apellidos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApellidosProperty =
            DependencyProperty.Register("Apellidos", typeof(string), typeof(dmTest) );




        public string Nombres
        {
            get { return (string)GetValue(NombresProperty); }
            set { SetValue(NombresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Nombres.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NombresProperty =
            DependencyProperty.Register("Nombres", typeof(string), typeof(dmTest) );



        public int IDEmpleado
        {
            get { return (int)GetValue(IDEmpleadoProperty); }
            set { SetValue(IDEmpleadoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IDEmpleado.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDEmpleadoProperty =
            DependencyProperty.Register("IDEmpleado", typeof(int), typeof(dmTest) );




        public long? DNI
        {
            get { return (long?)GetValue(DNIProperty); }
            set { SetValue(DNIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DNI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DNIProperty =
            DependencyProperty.Register("DNI", typeof(long?), typeof(dmTest));






       public override string ToString()
        {
            return "ID: " + IDEmpleado.ToString() + " " + Apellidos + " " +Nombres;
        }

    }

}

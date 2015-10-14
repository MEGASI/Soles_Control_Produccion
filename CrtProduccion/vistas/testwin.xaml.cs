using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for testwin.xaml
    /// </summary>
    public partial class testwin : Window
    {

        entidades.dmTest objE = new entidades.dmTest()
        {
            Apellidos = "Perez",
            DNI = 29111222,
            FechaIngeso = new DateTime(2000, 11, 19),
            IDEmpleado = 123,
            Nombres = "Juan"
        };
        
        //= new entidades.dmTest();
       
       

        public testwin()
        {
           
            InitializeComponent();
            stackPanelEmpl.DataContext = objE;
        
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
          
            //txtNombres.Focus();
            //txtNombres.Text = "NombreModificado";
            objE.Apellidos = "ApellidoModificado";
            objE.Nombres = "NombreModificado.";
            //txtDNI.Focus();
            MessageBox.Show(objE.ToString());
        }
    }
}

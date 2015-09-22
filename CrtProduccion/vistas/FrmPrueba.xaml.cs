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
using System.Data;
using System.Data.SqlClient;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for FrmPrueba.xaml
    /// </summary>
    public partial class FrmPrueba : Window
    {
        public FrmPrueba()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Permiso opermiso = new Permiso(datamanager.idUsario, '0');
            // if (opermiso.acceso) { // Puede entrar al sistema }
        }
    }
}

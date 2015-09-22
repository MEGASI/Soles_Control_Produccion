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
    /// Interaction logic for segLogin.xaml
    /// </summary>
    public partial class segLogin : Window
    {
        int intentos = 0;

        public segLogin()
        {
            InitializeComponent();
        }


        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            intentos += 1;

            if (datamanager.ValidarUsuario(UsuarioT.Text, passwordT.Password))
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Usuario O Contraseña no son validas \n"+
                                "Intento "+ intentos.ToString().Trim()+"/3",
                "Error al inicio de Sesion", MessageBoxButton.OK, MessageBoxImage.Error);
                if (intentos==3) DialogResult = false;

            }
        }

        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Seguro que Desea Salir ?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
             {
                DialogResult = false;
                //Application.Current.Shutdown();
            }
        }
    }
}

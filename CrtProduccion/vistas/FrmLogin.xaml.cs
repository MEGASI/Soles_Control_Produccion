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
    /// Interaction logic for FrmLogin.xaml
    /// </summary>
    public partial class FrmLogin : Window
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {




            if (datamanager.ValidarUsuario(UsuarioT.Text, passwordT.Password))
            {
                MessageBox.Show("Iniciando Sesion", "Bienvenido", MessageBoxButton.OK, MessageBoxImage.Information);
                FrmLogin login = new FrmLogin();
                login.Close();
                WelcomeForm Principal = new WelcomeForm();
                
                Principal.Show();
              
            }
            else
            {
                MessageBox.Show("Usuario O Contraseña no son validas ", "Error al inicio de Sesion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Seguro que Desea Salir ?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
  }

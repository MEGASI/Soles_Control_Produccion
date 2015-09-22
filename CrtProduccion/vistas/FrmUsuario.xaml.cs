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

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for FrmUsuario.xaml
    /// </summary>
    public partial class FrmUsuario : Window
    {
        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Usuario Us = new Usuario();
                Us.Nombre = Nombre.Text;
                Us.Clave = Clave.Password;

                int resultado = IUsuario.Agregar(Us);


                if (resultado > 0)
                {
                    MessageBox.Show("Datos Guardados Correctamente", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                {

                    MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                }


            }

            catch
            {
                MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            FrmEditUsuarios FR = new FrmEditUsuarios();
            FR.Show();
            this.Hide();
        }

        private void guardarBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Nombre.Text == "" || Clave.Password == "")
                {
                    MessageBox.Show("Campos Nombre y/o Clave estan Vacios");


                }
                else
                {
                    Usuario Us = new Usuario();
                    Us.Nombre = Nombre.Text;
                    Us.Clave = Clave.Password;

                    int resultado = IUsuario.Agregar(Us);
                    if (resultado > 0)
                    {
                        MessageBox.Show("Datos Guardados Correctamente", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {

                        MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Clave_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Nombre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

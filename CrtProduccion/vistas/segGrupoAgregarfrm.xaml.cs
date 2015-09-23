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
    /// Interaction logic for segGrupoAgregarfrm.xaml
    /// </summary>
    public partial class segGrupoAgregarfrm : Window
    {
        public Permiso sgPermiso;

        public segGrupoAgregarfrm()
        {
            InitializeComponent();


        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!datamanager.probarPermiso("P01", "modificar")) {
                MessageBox.Show("No tiene permiso para modificar", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            Group Go = new Group();
            Go.NombreG = NombreG.Text;



            int resultado = IGrupo.Agregar(Go);
            if (resultado > 0)
            {
                MessageBox.Show("Datos Guardados Correctamente", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            else
            {

                MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void button_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            segGrupofrm FG = new segGrupofrm();
            FG.Show();
            this.Close();
        }

        private void NombreT_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }
    }
}

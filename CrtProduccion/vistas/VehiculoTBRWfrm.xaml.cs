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
    /// Interaction logic for VehiculoTBRWfrm.xaml
    /// </summary>
    public partial class VehiculoTBRWfrm : Window
    {
        public Byte idVehiculo = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();
        public VehiculoTBRWfrm()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.idVehiculo = 0;
            this.DialogResult = false;
        }

        public void llenaGrid()
        {

            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos("Select descripcion, idTipoVehiculo from Vehiculo_Tipo  order by Descripcion");

            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 175;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Descripcion";
            DataG.Columns[0].CanUserResize = false;

            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 58;
            DataG.Columns[1].Header = "Número";
            DataG.Columns[1].CanUserResize = false;

            datamanager.ConexionCerrar();
        }
        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = DataG.SelectedItem;
            object item1 = DataG.SelectedItem;

            string sidVehiculo = (DataG.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;


            if (!Byte.TryParse(sidVehiculo, out idVehiculo))
            {
                idVehiculo = 0;
            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
        }
        private void DataG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                this.DialogResult = true;
            }
        }

        private void DataG_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }
    }
}

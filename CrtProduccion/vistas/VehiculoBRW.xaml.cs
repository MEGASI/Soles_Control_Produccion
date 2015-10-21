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
    /// Interaction logic for VehiculoBRW.xaml
    /// </summary>
    public partial class VehiculoBRW : Window
    {
        public int idvehiculos = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public VehiculoBRW()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.idvehiculos = 0;
            this.DialogResult = false;
        }

        public void llenaGrid()
        {
            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos("select * from Vehiculo order by Descripcion");

            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 175;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Codigo";
            DataG.Columns[0].CanUserResize = false;

            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 58;
            DataG.Columns[1].Header = "Ficha";
            DataG.Columns[1].CanUserResize = false;


            DataG.Columns[2].IsReadOnly = true;
            DataG.Columns[2].Width = 100;
            DataG.Columns[2].Header = "Descripcion";
            DataG.Columns[2].CanUserResize = false;

           
            DataG.Columns[3].IsReadOnly = true;
            DataG.Columns[3].Width = 58;
            DataG.Columns[3].Header = "idMarca";
            DataG.Columns[3].CanUserResize = false;

            DataG.Columns[4].IsReadOnly = true;
            DataG.Columns[4].Width = 100;
            DataG.Columns[4].Header = "Modelo";
            DataG.Columns[4].CanUserResize = false;
            
            DataG.Columns[5].IsReadOnly = true;
            DataG.Columns[5].Width = 58;
            DataG.Columns[5].Header = "idTipoVehiculo";
            DataG.Columns[5].CanUserResize = false;

            DataG.Columns[6].IsReadOnly = true;
            DataG.Columns[6].Width = 58;
            DataG.Columns[6].Header = "Placa";
            DataG.Columns[6].CanUserResize = false;

            DataG.Columns[7].IsReadOnly = true;
            DataG.Columns[7].Width = 58;
            DataG.Columns[7].Header = "Año";
            DataG.Columns[7].CanUserResize = false;


            DataG.Columns[8].IsReadOnly = true;
            DataG.Columns[8].Width = 70;
            DataG.Columns[8].Header = "Chasis";
            DataG.Columns[8].CanUserResize = false;

            DataG.Columns[9].IsReadOnly = true;
            DataG.Columns[9].Width = 58;
            DataG.Columns[9].Header = "idColor";
            DataG.Columns[9].CanUserResize = false;

            DataG.Columns[10].IsReadOnly = true;
            DataG.Columns[10].Width = 58;
            DataG.Columns[10].Header = "idLlantas";
            DataG.Columns[10].CanUserResize = false;

            DataG.Columns[11].IsReadOnly = true;
            DataG.Columns[11].Width = 58;
            DataG.Columns[11].Header = "idFiltAceite";
            DataG.Columns[11].CanUserResize = false;

            DataG.Columns[12].IsReadOnly = true;
            DataG.Columns[12].Width = 58;
            DataG.Columns[12].Header = "Estado";
            DataG.Columns[12].CanUserResize = false;

            DataG.Columns[13].IsReadOnly = true;
            DataG.Columns[13].Width = 75;
            DataG.Columns[13].Header = "SeguroVenc";
            DataG.Columns[13].CanUserResize = false;

            DataG.Columns[14].IsReadOnly = true;
            DataG.Columns[14].Width = 75;
            DataG.Columns[14].Header = "UltimoMant";
            DataG.Columns[14].CanUserResize = false;

            DataG.Columns[15].IsReadOnly = true;
            DataG.Columns[15].Width = 100;
            DataG.Columns[15].Header = "kilometraje";
            DataG.Columns[15].CanUserResize = false;

            //DataG.Columns[16].IsReadOnly = true;
            //DataG.Columns[16].Width = 58;
            //DataG.Columns[16].Header = "kilometraje";
            //DataG.Columns[16].CanUserResize = false;


           

            datamanager.ConexionCerrar();
        }

        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = DataG.SelectedItem;
            object item1 = DataG.SelectedItem;

            string sidVehiculo = (DataG.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;


            if (!Int32.TryParse(sidVehiculo, out idvehiculos))
            {
                idvehiculos = 0;
            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
        }
    }
}
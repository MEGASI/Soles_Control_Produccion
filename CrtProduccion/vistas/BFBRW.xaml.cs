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

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for BFBRW.xaml
    /// </summary>
    public partial class BFBRW : Window
    {

        public int idBrigada = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public BFBRW()
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
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                this.DialogResult = true;
            }
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dataGrid.SelectedItem;
            object item1 = dataGrid.SelectedItem;

            string sidVehiculo = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            if (!Int32.TryParse(sidVehiculo, out idBrigada))
            {
                idBrigada = 0;
            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
        }



        public void llenaGrid()
        {
            dsGrid.Clear();
            dsGrid = datamanager.ConsultaDatos(" Select idBrigada,nombres from vbrigadaD order by idBrigada desc");

            dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;

            
            


           

          
          


            datamanager.ConexionCerrar();
        }
    }
}
   

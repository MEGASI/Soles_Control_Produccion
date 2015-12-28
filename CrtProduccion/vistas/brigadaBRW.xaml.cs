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
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for brigadaBRW.xaml
    /// </summary>
    public partial class brigadaBRW : Window
    {

        public int idBrigada = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();
        public brigadaBRW()
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
            this.idBrigada = 0;
            this.DialogResult = false;
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

            string sidBrigada = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            if (!Int32.TryParse(sidBrigada, out idBrigada))
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
            dsGrid = datamanager.ConsultaDatos("  Select H.idBrigada,V.Descripcion as Vehiculo,"+
                                               "  H.fecha, LD.nombres as Chofer,"+
                                               "  LDD.nombres as Supervisor,"+
                                               "  H.activa from brigadaH H "+
                                               "  left outer join   vLD LD  on   H.idChofer = LD.idLD"+
                                               "  left outer join vLD LDD on H.idSupervisor = LDD.idLD"+
                                               "  left outer join  Vehiculo V on h.idVehiculo = V.idVehiculo "+
                                               "  where activa = 1 "+
                                               "  order by H.idBrigada desc");

            dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;

            dataGrid.CanUserAddRows = false;
            dataGrid.Columns[0].Width = 75;
            dataGrid.Columns[0].IsReadOnly = true;
            dataGrid.Columns[0].Header = "idBrigada";
            dataGrid.Columns[0].CanUserResize = false;

            dataGrid.Columns[1].IsReadOnly = true;
            dataGrid.Columns[1].Width = 100;
            dataGrid.Columns[1].Header = "Descripción";
            dataGrid.Columns[1].CanUserResize = false;


            dataGrid.Columns[2].IsReadOnly = true;
            dataGrid.Columns[2].Width = 72;
            dataGrid.Columns[2].Header = "Fecha";
            dataGrid.Columns[2].CanUserResize = false;

            dataGrid.Columns[3].IsReadOnly = true;
            dataGrid.Columns[3].Width = 175;
            dataGrid.Columns[3].Header = "Chofer";
            dataGrid.Columns[3].CanUserResize = false;



            dataGrid.Columns[4].IsReadOnly = true;
            dataGrid.Columns[4].Width = 175;
            dataGrid.Columns[4].Header = "Supervisor";
            dataGrid.Columns[4].CanUserResize = false;


            dataGrid.Columns[5].IsReadOnly = true;
            dataGrid.Columns[5].Width = 50;
            dataGrid.Columns[5].Header = "Activa";
            dataGrid.Columns[5].CanUserResize = false;



            ////dataGrid.Columns[6].IsReadOnly = true;
            ////dataGrid.Columns[6].Width = 75;
            ////dataGrid.Columns[6].Header = "idSupervisor";
            ////dataGrid.Columns[6].CanUserResize = false;




            ////dataGrid.Columns[7].IsReadOnly = true;
            ////dataGrid.Columns[7].Width = 175;
            ////dataGrid.Columns[7].Header = "Supervisor";
            ////dataGrid.Columns[7].CanUserResize = false;

            ////dataGrid.Columns[8].IsReadOnly = true;
            ////dataGrid.Columns[8].Width = 75;
            ////dataGrid.Columns[8].Header = "activa";
            ////dataGrid.Columns[8].CanUserResize = false;


            datamanager.ConexionCerrar();
        }


    }



}
   
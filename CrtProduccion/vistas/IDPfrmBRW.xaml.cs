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
    /// Interaction logic for IDPfrmBRW.xaml
    /// </summary>
    /// 

    #region Funciones y Metodos

    public partial class IDPfrmBRW : Window
    {
        public int idIDP = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();


        public IDPfrmBRW()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            
            this.idIDP = 0;
            this.DialogResult = false;
        }

        #endregion


        #region Llenando Grid

        public void llenaGrid()
        {

            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos(" Select H.id,H.idp,H.fecha,H.feriado, H.circuito,"+
                                               " H.idSuperlocal, LD.nombres as SuperVisorLocal, "+
                                               " H.idSuperEde As SuperVisorEdeeste, LDD.nombres, H.Observacion,"+
                                               " H.estado from IDP_H H"+
                                               " left outer join vLD LD on H.idSuperLocal = LD.idLD"+
                                               " left outer join vLD LDD on H.idSuperEde = LDD.idLD "+
                                               " order by H.id desc");



            dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;

            dataGrid.CanUserAddRows = false;
            dataGrid.Columns[0].Width = 45;
            dataGrid.Columns[0].IsReadOnly = true;
            dataGrid.Columns[0].Header = "id";
            dataGrid.Columns[0].CanUserResize = false;
            dataGrid.Columns[0].Visibility = Visibility.Hidden;


            dataGrid.Columns[1].IsReadOnly = true;
            dataGrid.Columns[1].Width = 75;
            dataGrid.Columns[1].Header = "IDP";
            dataGrid.Columns[1].CanUserResize = false;



            dataGrid.Columns[2].IsReadOnly = true;
            dataGrid.Columns[2].Width = 150;
            dataGrid.Columns[2].Header = "Fecha";
            dataGrid.Columns[2].CanUserResize = false;


            dataGrid.Columns[3].IsReadOnly = true;
            dataGrid.Columns[3].Width = 75;
            dataGrid.Columns[3].Header = "Feriado";
            dataGrid.Columns[3].CanUserResize = false;


            dataGrid.Columns[4].IsReadOnly = true;
            dataGrid.Columns[4].Width = 75;
            dataGrid.Columns[4].Header = "Circuito";
            dataGrid.Columns[4].CanUserResize = false;


            dataGrid.Columns[5].IsReadOnly = true;
            dataGrid.Columns[5].Width = 200;
            dataGrid.Columns[5].Header = "SupervisorLocal";
            dataGrid.Columns[5].CanUserResize = false;
            dataGrid.Columns[5].Visibility = Visibility.Hidden;



            dataGrid.Columns[6].IsReadOnly = true;
            dataGrid.Columns[6].Width = 200;
            dataGrid.Columns[6].Header = "SupervisorLocal";
            dataGrid.Columns[6].CanUserResize = false;



            dataGrid.Columns[7].IsReadOnly = true;
            dataGrid.Columns[7].Width = 200;
            dataGrid.Columns[7].Header = "Supervisor Edeeste";
            dataGrid.Columns[7].CanUserResize = false;
            dataGrid.Columns[7].Visibility = Visibility.Hidden;


            dataGrid.Columns[8].IsReadOnly = true;
            dataGrid.Columns[8].Width = 200;
            dataGrid.Columns[8].Header = "Supervisor Edeeste";
            dataGrid.Columns[8].CanUserResize = false;
        

            dataGrid.Columns[9].IsReadOnly = true;
            dataGrid.Columns[9].Width = 150;
            dataGrid.Columns[9].Header = "Observacion";
            dataGrid.Columns[9].CanUserResize = false;


            dataGrid.Columns[10].IsReadOnly = true;
            dataGrid.Columns[10].Width = 75;
            dataGrid.Columns[10].Header = "estado";
            dataGrid.Columns[10].CanUserResize = false;

            datamanager.ConexionCerrar();
        }

        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dataGrid.SelectedItem;
 
            string sidIDP = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            if (!Int32.TryParse(sidIDP, out idIDP))
            {
                idIDP = 0;
               

            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                this.DialogResult = true;
            }


        }
        #endregion
    }
}

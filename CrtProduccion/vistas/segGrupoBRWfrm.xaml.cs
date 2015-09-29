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
using System.Windows.Forms;
namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupoBRWfrm.xaml
    /// </summary>
    public partial class segGrupoBRWfrm : Window
    {

        public int idGrupo = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();


        public segGrupoBRWfrm()
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
            this.idGrupo = 0;
            this.DialogResult = false;
        }

        public void llenaGrid()
        {

            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos("select Nombre, idGrupo from segGrupo");

            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 152;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Grupo";
            DataG.Columns[0].CanUserResize = false;

            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 48;
            DataG.Columns[1].Header = "Número";
            DataG.Columns[1].CanUserResize = false;

            datamanager.ConexionCerrar();

        }

        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = DataG.SelectedItem;
            object item1 = DataG.SelectedItem;

            string sidUGrupo= (DataG.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;


            if (!Int32.TryParse(sidUGrupo, out idGrupo))
            {
                idGrupo = 0;
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

    }
}

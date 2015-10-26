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
    /// Interaction logic for proyectoBRWfrm.xaml
    /// </summary>
    public partial class proyectoBRWfrm : Window
    {

        #region Metodos



        public int idProyecto = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();
        public proyectoBRWfrm()
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
            this.idProyecto = 0;
            this.DialogResult = false;
        }
        #endregion


        #region LlenandoGrid


        public void llenaGrid()
        {
            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos("select Descripcion, idProyecto from proyecto order by Descripcion");

            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 175;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Proyecto";
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

            string sidUProyecto = (DataG.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;


            if (!Int32.TryParse(sidUProyecto, out idProyecto))
            {
                idProyecto = 0;
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
        #endregion


        #region Busqueda Incrementada


        private void txtCampo_TextChanged(object sender, TextChangedEventArgs e)
        {
            dsGrid.Clear();
            if (cbFiltro.Text == "Codigo")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" select * from Proyecto  where idProyecto Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }

            else if (cbFiltro.Text == "Proyecto")
            {

                dsGrid.Clear();
                SqlDataAdapter adapter = new SqlDataAdapter(" select * from Proyecto  where Descripcion Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }
        }

    }
}
#endregion
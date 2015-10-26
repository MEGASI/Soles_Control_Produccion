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
    /// Interaction logic for LlantasBRW.xaml
    /// </summary>
    public partial class LlantasBRW : Window
    {
        #region Metodos
        public int idLlantas = 0;
        public string NombreL = "";
        public int idfiltro = 0;
        public string FiltroN = "";
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public LlantasBRW()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.seleccion();
        }

        private void seleccion()
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
          

        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.idLlantas = 0;
            this.idfiltro = 0;
            this.DialogResult = false;
        }
        #endregion

        #region LlenandoGrid


        public void llenaGrid()
        {
            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos(" SELECT  v.idllantas, ll.descripcion AS llanta,"+
                                               " v.idFiltAceite, fa.descripcion AS filtroAceite"+
                                               " FROM  Vehiculo AS v  LEFT OUTER JOIN Vehiculo_Partes AS"+
                                               " fa ON v.idFiltAceite = fa.idParte LEFT OUTER JOIN Vehiculo_Partes AS"+ 
                                               " ll ON v.idllantas = ll.idParte ");


            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;


            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 100;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "idLlanta";
            DataG.Columns[0].CanUserResize = false;


            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 125;
            DataG.Columns[1].Header = "Llanta";
            DataG.Columns[1].CanUserResize = false;



            DataG.CanUserAddRows = false;
            DataG.Columns[2].Width = 100;
            DataG.Columns[2].IsReadOnly = true;
            DataG.Columns[2].Header = "idFil";
            DataG.Columns[2].CanUserResize = false;


            DataG.Columns[3].IsReadOnly = true;
            DataG.Columns[3].Width = 125;
            DataG.Columns[3].Header = "Filtro";
            DataG.Columns[3].CanUserResize = false;


            datamanager.ConexionCerrar();
        }

        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item1 = DataG.SelectedItem;

            string sidLlantas = (DataG.SelectedCells[0].Column.GetCellContent(item1) as TextBlock).Text;
            string sidAceite = (DataG.SelectedCells[2].Column.GetCellContent(item1) as TextBlock).Text;
            NombreL = (DataG.SelectedCells[1].Column.GetCellContent(item1) as TextBlock).Text;
           
            FiltroN = (DataG.SelectedCells[3].Column.GetCellContent(item1) as TextBlock).Text;

            if (!Int32.TryParse(sidLlantas, out idLlantas )|| ( !Int32.TryParse(sidAceite, out idfiltro)))
            {
                idLlantas = 0;
                idfiltro = 0;
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
                this.seleccion();
            }
        }

        #endregion


        #region  Busqueda Incrementada

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {     
            dsGrid.Clear();
            if (cbFiltro.Text == "Llanta")
            {   
                 SqlDataAdapter adapter = new SqlDataAdapter (" SELECT  v.idllantas, ll.descripcion AS llanta," +
                                               " v.idFiltAceite, fa.descripcion AS filtroAceite" +
                                               " FROM  Vehiculo AS v  LEFT OUTER JOIN Vehiculo_Partes AS" +
                                               " fa ON v.idFiltAceite = fa.idParte LEFT OUTER JOIN Vehiculo_Partes AS" +
                                               " ll ON v.idllantas = ll.idParte  where ll.descripcion Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }
            else if (cbFiltro.Text == "FiltroAceite")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" SELECT  v.idllantas, ll.descripcion AS llanta," +
                                                " v.idFiltAceite, fa.descripcion AS filtroAceite" +
                                                " FROM  Vehiculo AS v  LEFT OUTER JOIN Vehiculo_Partes AS" +
                                                " fa ON v.idFiltAceite = fa.idParte LEFT OUTER JOIN Vehiculo_Partes AS" +
                                                " ll ON v.idllantas = ll.idParte  where ll.descripcion Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }    
        }
    }
}
#endregion
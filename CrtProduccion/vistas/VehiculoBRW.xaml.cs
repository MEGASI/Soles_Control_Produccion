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
    /// Interaction logic for VehiculoBRW.xaml
    /// </summary>
    public partial class VehiculoBRW : Window
    {
        #region  Metodos


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

        #endregion


        #region LlenarGrid




        public void llenaGrid()
        {
            dsGrid.Clear();
            dsGrid = datamanager.ConsultaDatos("  SELECT  v.idVehiculo, v.Ficha, v.descripcion, v.idMarca,vm.Descripcion as Marca," +
                                               "  v.modelo, v.idTipoVehiculo,vt.descripcion as TipoVehiculo," +
                                               "  v.placa, v.ano, v.chasis, v.idColor,vc.Descripcion as Color," +
                                               "  v.idllantas, ll.descripcion AS llanta," +
                                               "  v.idFiltAceite, fa.descripcion AS filtroAceite,v.idEstado," +
                                               "  v.seguroVence, v.ultMantenim, v.kilometraje, v.photo" +
                                               "  FROM  Vehiculo AS v" +
                                               "  LEFT OUTER JOIN Vehiculo_Partes AS fa ON v.idFiltAceite = fa.idParte" +
                                               "  LEFT OUTER JOIN Vehiculo_Partes AS ll ON v.idllantas = ll.idParte " +
                                               "  LEFT OUTER JOIN Vehiculo_Tipo AS vt ON v.idTipoVehiculo =vt.idTipoVehiculo" +
                                               "  LEFT OUTER JOIN Vehiculo_Marca AS vm ON v.idMarca = vm.idMarca" +
                                               "  LEFT OUTER JOIN color AS vc ON v.idColor =vc.idColor" +
                                               "  Order by v.idVehiculo  ");

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
            DataG.Columns[4].Header = "Marca";
            DataG.Columns[4].CanUserResize = false;

            DataG.Columns[5].IsReadOnly = true;
            DataG.Columns[5].Width = 100;
            DataG.Columns[5].Header = "Modelo";
            DataG.Columns[5].CanUserResize = false;

            DataG.Columns[6].IsReadOnly = true;
            DataG.Columns[6].Width = 75;
            DataG.Columns[6].Header = "idTipoVehiculo";
            DataG.Columns[6].CanUserResize = false;

            DataG.Columns[7].IsReadOnly = true;
            DataG.Columns[7].Width = 58;
            DataG.Columns[7].Header = "TipoVehiculo";
            DataG.Columns[7].CanUserResize = false;


            DataG.Columns[8].IsReadOnly = true;
            DataG.Columns[8].Width = 100;
            DataG.Columns[8].Header = "Placa";
            DataG.Columns[8].CanUserResize = false;

            DataG.Columns[9].IsReadOnly = true;
            DataG.Columns[9].Width = 58;
            DataG.Columns[9].Header = "Año";
            DataG.Columns[9].CanUserResize = false;

            DataG.Columns[10].IsReadOnly = true;
            DataG.Columns[10].Width = 100;
            DataG.Columns[10].Header = "Chasis";
            DataG.Columns[10].CanUserResize = false;

            DataG.Columns[11].IsReadOnly = true;
            DataG.Columns[11].Width = 58;
            DataG.Columns[11].Header = "idColor";
            DataG.Columns[11].CanUserResize = false;

            DataG.Columns[12].IsReadOnly = true;
            DataG.Columns[12].Width = 80;
            DataG.Columns[12].Header = "Color";
            DataG.Columns[12].CanUserResize = false;

            DataG.Columns[13].IsReadOnly = true;
            DataG.Columns[13].Width = 75;
            DataG.Columns[13].Header = "idLlantas";
            DataG.Columns[13].CanUserResize = false;

            DataG.Columns[14].IsReadOnly = true;
            DataG.Columns[14].Width = 100;
            DataG.Columns[14].Header = "Llantas";
            DataG.Columns[14].CanUserResize = false;

            DataG.Columns[15].IsReadOnly = true;
            DataG.Columns[15].Width = 75;
            DataG.Columns[15].Header = "idFiltaceite";
            DataG.Columns[15].CanUserResize = false;

            DataG.Columns[16].IsReadOnly = true;
            DataG.Columns[16].Width = 100;
            DataG.Columns[16].Header = "Filtro";
            DataG.Columns[16].CanUserResize = false;

            DataG.Columns[17].IsReadOnly = true;
            DataG.Columns[17].Width = 100;
            DataG.Columns[17].Header = "IdEstado";
            DataG.Columns[17].CanUserResize = false;

            DataG.Columns[18].IsReadOnly = true;
            DataG.Columns[18].Width = 100;
            DataG.Columns[18].Header = "SegVence";
            DataG.Columns[18].CanUserResize = false;

            DataG.Columns[19].IsReadOnly = true;
            DataG.Columns[19].Width = 100;
            DataG.Columns[19].Header = "Ultmant";
            DataG.Columns[19].CanUserResize = false;

            DataG.Columns[20].IsReadOnly = true;
            DataG.Columns[20].Width = 100;
            DataG.Columns[20].Header = "kilometraje";
            DataG.Columns[20].CanUserResize = false;

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


        #endregion




        #region  Busqueda Incrementada
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
           dsGrid.Clear();
            if (cbFiltro.Text == "Ficha")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Vehiculo where Ficha  Like '" + textBox.Text + "%'", datamanager.cadenadeconexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }
            else if (cbFiltro.Text == "Descripcion")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Vehiculo where Descripcion  Like '" + textBox.Text + "%'", datamanager.cadenadeconexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }
            else if (cbFiltro.Text == "Modelo")

            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Vehiculo where Modelo  Like '" + textBox.Text + "%'", datamanager.cadenadeconexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }        
        }
    }
}
#endregion
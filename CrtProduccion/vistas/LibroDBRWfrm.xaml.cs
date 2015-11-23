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
    /// Interaction logic for LibroDBRWfrm.xaml
    /// </summary>
    public partial class LibroDBRWfrm : Window
    {
        #region Metodos
        public int idLD = 0;

        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public LibroDBRWfrm()
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
            this.idLD = 0;
            this.DialogResult = false;
        }

        #endregion


        #region LlenandoGrid

        public void llenaGrid()
        {

            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                               " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                               " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                               " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                               " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                               " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto order by ld.idLD");


            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 50;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Codigo";
            DataG.Columns[0].CanUserResize = false;

            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 75;
            DataG.Columns[1].Header = "CedulaRNC";
            DataG.Columns[1].CanUserResize = false;

            DataG.Columns[2].IsReadOnly = true;
            DataG.Columns[2].Width = 125;
            DataG.Columns[2].Header = "Nombres";
            DataG.Columns[2].CanUserResize = false;

            DataG.Columns[3].IsReadOnly = true;
            DataG.Columns[3].Width = 125;
            DataG.Columns[3].Header = "Apellidos";
            DataG.Columns[3].CanUserResize = false;

            DataG.Columns[4].IsReadOnly = true;
            DataG.Columns[4].Width = 90;
            DataG.Columns[4].Header = "esCliente";
            DataG.Columns[4].CanUserResize = false;

            DataG.Columns[5].IsReadOnly = true;
            DataG.Columns[5].Width = 90;
            DataG.Columns[5].Header = "esEmpleado";
            DataG.Columns[5].CanUserResize = false;

            DataG.Columns[6].IsReadOnly = true;
            DataG.Columns[6].Width = 90;
            DataG.Columns[6].Header = "esProveedor";
            DataG.Columns[6].CanUserResize = false;

            DataG.Columns[7].IsReadOnly = true;
            DataG.Columns[7].Width = 55;
            DataG.Columns[7].Header = "idCargo";
            DataG.Columns[7].CanUserResize = false;

            DataG.Columns[8].IsReadOnly = true;
            DataG.Columns[8].Width = 100;
            DataG.Columns[8].Header = "Cargo";
            DataG.Columns[8].CanUserResize = false;

            DataG.Columns[9].IsReadOnly = true;
            DataG.Columns[9].Width = 50;
            DataG.Columns[9].Header = "idDpto";
            DataG.Columns[9].CanUserResize = false;

            DataG.Columns[10].IsReadOnly = true;
            DataG.Columns[10].Width = 100;
            DataG.Columns[10].Header = "Departamento";
            DataG.Columns[10].CanUserResize = false;


            DataG.Columns[11].IsReadOnly = true;
            DataG.Columns[11].Width = 60;
            DataG.Columns[11].Header = "Sueldo";
            DataG.Columns[11].CanUserResize = false;


            DataG.Columns[12].IsReadOnly = true;
            DataG.Columns[12].Width = 90;
            DataG.Columns[12].Header = "Estado";
            DataG.Columns[12].CanUserResize = false;



         


            datamanager.ConexionCerrar();

        }

        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = DataG.SelectedItem;
            object item1 = DataG.SelectedItem;

            string siupLB = (DataG.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;



            if (!Int32.TryParse(siupLB, out idLD))
            {
                idLD = 0;

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
            if (cbFiltro.Text == "CedulaRNC")
            {

                SqlDataAdapter adapter = new SqlDataAdapter(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                                            " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                                            " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                                            " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                                            " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                                            " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto " +
                                                            " where CedulaRNC Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                //  SqlDataAdapter adapter = new SqlDataAdapter(" select * from cargo  where idCargo Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }
            else if (cbFiltro.Text == "Nombres")
            {


                SqlDataAdapter adapter = new SqlDataAdapter(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                                            " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                                            " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                                            " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                                            " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                                            " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto " +
                                                            " where Nombres Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;


            }
            else if (cbFiltro.Text == "Apellidos")
            {


                SqlDataAdapter adapter = new SqlDataAdapter(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                                            " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                                            " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                                            " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                                            " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                                            " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto " +
                                                            " where Apellidos Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }

            else if (cbFiltro.Text == "Departamento")
            {


                SqlDataAdapter adapter = new SqlDataAdapter(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                                            " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                                            " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                                            " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                                            " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                                            " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto " +
                                                            " where D.Descripcion Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }


            else if (cbFiltro.Text == "Cargo")
            {


                SqlDataAdapter adapter = new SqlDataAdapter(" select ld.idLD, ld.cedulaRNC, ld.Nombres, ld.Apellidos, ld.esCliente," +
                                                            " ld.esEmpleado, ld.esProveedor, ld.idCargo," +
                                                            " C.descripcion as Cargo, ld.idDpto, D.Descripcion as Departamento, " +
                                                            " ld.sueldo,ld.estado, ld.photo from LibroDirecciones ld" +
                                                            " LEFT OUTER JOIN cargo as C on ld.idCargo = C.idCargo" +
                                                            " LEFT OUTER JOIN departamento as D on ld.idDpto = D.idDpto " +
                                                            " where C.descripcion  Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }
        }

       
    }
}
#endregion
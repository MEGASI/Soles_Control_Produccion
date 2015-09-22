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

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for FrmEditUsuarios.xaml
    /// </summary>
    public partial class FrmEditUsuarios : Window
    {
        SqlCommand Cmd = new SqlCommand();
        SqlConnection Cnn = new SqlConnection();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        public FrmEditUsuarios()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Cmd.CommandText = "select * from segUsuario";

            Cnn.ConnectionString = "Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
            Cmd.Connection = Cnn;
            adapter.SelectCommand = Cmd;
            adapter.Fill(ds);
            dataGrid.ItemsSource = ds.Tables[0].DefaultView;
            Cnn.Close();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FrmEditUsuarios1_Closed(object sender, EventArgs e)
        {

        }

        private void BuscarBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string Nombre, Clave;

                String sql = @"select  Nombre, clave from segUsuario where  idUsuario = " + ID_NameT.Text;
                SqlConnection conexion = Conexion.ObtenerCOnexion();
                conexion.Open();
                SqlCommand command = new SqlCommand(sql, conexion);
                SqlDataReader reader;

                reader = command.ExecuteReader();
                while (reader.Read())
                {


                    Nombre = reader.GetString(0);
                    Clave = reader.GetString(1);



                    NameT.Text = Nombre;
                    ClaveT.Password = Clave;



                }
                reader.Close();
                conexion.Close();


            }
            catch { MessageBox.Show("ID_Usuario Incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }

        }

        private void ActualizarBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Usuario G = new Usuario();

                G.ID_Usuario = Convert.ToInt32((ID_NameT.Text.Trim()));
                G.Nombre = NameT.Text.Trim();
                G.Clave = ClaveT.Password.Trim();



                int resultado = IUsuario.Modificar(G);

                if (resultado > 0)


                {
                    MessageBox.Show("Datos Actualizados Correctamente", "Actualizando", MessageBoxButton.OK, MessageBoxImage.Information);
                    ID_NameT.Text = "";
                    NameT.Text = "";
                    ClaveT.Password = "";
                    dataGrid.UpdateDefaultStyle();
                }
                else
                {

                    MessageBox.Show("Error al Actualizar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                }


            }
            catch { MessageBox.Show("Campo Vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        private void EliminarBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Usuario US = new Usuario();

                US.ID_Usuario = Convert.ToInt16(ID_NameT.Text);
                US.Nombre = NameT.Text.Trim();
                US.Clave = ClaveT.Password.Trim();



                int Resultado = IUsuario.Eliminar(US);

                if (Resultado > 0)

                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);

                    ID_NameT.Text = "";
                    NameT.Text = "";
                    ClaveT.Password = "";
                }
                else
                {
                    MessageBox.Show("Error al Eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                }

            }
            catch { MessageBox.Show("Error al Eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

            //checkBox.IsChecked = true;
            //if (checkBox.IsEnabled == true)
            //{

            //    Cmd.CommandText = "select * from segUsuario";

            //    Cnn.ConnectionString = "Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
            //    Cmd.Connection = Cnn;
            //    adapter.SelectCommand = Cmd;
            //    adapter.Fill(ds);
            //    dataGrid.ItemsSource = ds.Tables[0].DefaultView;

            //    Cnn.Close();


            //}
        }

        private void ClaveT_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FrmEditUsuarios1_Loaded(object sender, RoutedEventArgs e)
        {
            Cmd.CommandText = "select * from segUsuario";

            Cnn.ConnectionString = "Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
            Cmd.Connection = Cnn;
            adapter.SelectCommand = Cmd;
            adapter.Fill(ds);
            dataGrid.ItemsSource = ds.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
            dataGrid.Columns[0].Width = 50;
            dataGrid.Columns[0].IsReadOnly = true;
            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Nombre";
            dataGrid.Columns[2].Header = "Clave";

            Cnn.Close();

        }

        private void ID_NameT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                try
                {
                    string Nombre, Clave;

                    String sql = @"select  Nombre, clave from segUsuario where  idUsuario = " + ID_NameT.Text;
                    SqlConnection conexion = Conexion.ObtenerCOnexion();
                    conexion.Open();
                    SqlCommand command = new SqlCommand(sql, conexion);
                    SqlDataReader reader;

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        Nombre = reader.GetString(0);
                        Clave = reader.GetString(1);



                        NameT.Text = Nombre;
                        ClaveT.Password = Clave;



                    }
                    reader.Close();
                    conexion.Close();


                }
                catch { MessageBox.Show("ID_Usuario Incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }

            }
        }
    } }

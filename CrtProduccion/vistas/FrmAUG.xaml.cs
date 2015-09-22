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
    /// Interaction logic for FrmAUG.xaml
    /// </summary>
    public partial class FrmAUG : Window
    {
        SqlCommand Cmd = new SqlCommand();
        SqlConnection Cnn = new SqlConnection();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataSet dsGrid = new DataSet();

        public FrmAUG()
        {
            InitializeComponent();

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            this.llenaGrid();

        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            this.guardar();     
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }


        private void idUT_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void Miembro_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void cbGrupo_Loaded(object sender, RoutedEventArgs e)
        {
            // Llenando el ComboBox de los Grupos de Usuarios
            SqlDataReader reader =
            datamanager.ConsultaLeer("select nombre, idGrupo from segGrupo");

            while (reader != null && reader.Read())
            {
                cbGrupo.Items.Add(new ComboBoxItem(reader.GetString(0), reader.GetInt32(1)));
            }

            cbGrupo.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }

        public void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.llenaGrid();
        }

        public void llenaGrid()
        {




            dsGrid.Clear();

            if (cbGrupo.SelectedValue != null)
            {

                int selectedValue = ((ComboBoxItem)cbGrupo.SelectedItem).intValue;

                dsGrid = datamanager.ConsultaDatos("exec dbo.[segGrupoMiembro] " + selectedValue.ToString());

                dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;
                dataGrid.Columns[2].Visibility = System.Windows.Visibility.Hidden;
                dataGrid.Columns[3].Visibility = System.Windows.Visibility.Hidden;
                dataGrid.Columns[4].Visibility = System.Windows.Visibility.Hidden;
                dataGrid.CanUserAddRows = false;
                dataGrid.Columns[0].Width = 150;
                dataGrid.Columns[0].IsReadOnly = true;
                dataGrid.Columns[0].Header = "Usuario";
                dataGrid.Columns[1].Header = "Miembro";

                datamanager.ConexionCerrar();
            }

        }

        private void guardar()
        {

            try
            {
                // Recorriendo el DgGrid y  Guardando
                if (datamanager.ConexionAbrir())

                {
                

                    SqlCommand Cmd1 = new SqlCommand();

                    Cmd1.Connection = datamanager.ConexionSQL;
                    Cmd1.CommandText = "dbo.segMiembroRUD";
                    Cmd1.CommandType = CommandType.StoredProcedure;

                    Cmd1.Parameters.Add("@idUsuario",SqlDbType.Int).Value = 0;
                    Cmd1.Parameters.Add("@idGrupo",SqlDbType.Int).Value = 0;
                    Cmd1.Parameters.Add("@miembro",SqlDbType.Bit).Value = false;

                    for (int i = 0; i < dataGrid.Items.Count ; i++)
                    {
                        var miembro = (dataGrid.Items[i] as System.Data.DataRowView).Row.ItemArray[1];
                        var oldmiembro = (dataGrid.Items[i] as System.Data.DataRowView).Row.ItemArray[4];

                        if (miembro != oldmiembro)
                            
                        {

                            var idGrupo = (dataGrid.Items[i] as System.Data.DataRowView).Row.ItemArray[2];
                            var idUsuario = (dataGrid.Items[i] as System.Data.DataRowView).Row.ItemArray[3];

                            Cmd1.Parameters["@idUsuario"].Value = idUsuario;
                            Cmd1.Parameters["@idGrupo"].Value = idGrupo;
                            Cmd1.Parameters["@miembro"].Value = miembro;

                            Cmd1.ExecuteNonQuery();
                        }                        
                    }

                    datamanager.ConexionCerrar();
                    MessageBox.Show(" Guardado", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.llenaGrid();
                }
            }
            catch  (Exception ex)

            {
                ex.Message.ToString();
                MessageBox.Show("Error Al Guargar", "Error");
            }

            
        }

        private void idGT_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Salirbtn_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

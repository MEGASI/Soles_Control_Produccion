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


namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for FrmPU.xaml
    /// </summary>
    public partial class FrmPU : Window
    {
        public FrmPU()

       
        {
            InitializeComponent();
            SqlCommand Cmd = new SqlCommand();
            SqlConnection Cnn = new SqlConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataSet dsGrid1 = new DataSet();
        }
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private void CbUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.llenaGridPU();
        }
        private void CbUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataReader reader =
             datamanager.ConsultaLeer("select nombre, idUsuario from segUsuario");


            while (reader != null && reader.Read())
            {

                CbUsuario.Items.Add(new ComboBoxItem(reader.GetString(0), reader.GetInt32(1)));
            }

            CbUsuario.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }

        public void llenaGridPU()
        {
            DataSet dsGrid = new DataSet();
            dsGrid.Clear();

            if (CbUsuario.SelectedValue != null)
            {
                int selectedValue = ((ComboBoxItem)CbUsuario.SelectedItem).intValue;

                dsGrid = datamanager.ConsultaDatos(" exec segPerfilUsuario " + selectedValue.ToString());

                DgGrupo.ItemsSource = dsGrid.Tables[0].DefaultView;
                DgGrupo.ItemsSource = dsGrid.Tables[0].DefaultView;
                DgGrupo.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                DgGrupo.Columns[6].Visibility = System.Windows.Visibility.Hidden;
                DgGrupo.Columns[7].Visibility = System.Windows.Visibility.Hidden;
                DgGrupo.Columns[8].Visibility = System.Windows.Visibility.Hidden;
                DgGrupo.Columns[9].Visibility = System.Windows.Visibility.Hidden;
                DgGrupo.CanUserAddRows = false;
                DgGrupo.Columns[0].Width = 150;
                DgGrupo.Columns[0].IsReadOnly = true;
                DgGrupo.Columns[2].Header = "Acceso";
                DgGrupo.Columns[3].Header = "Crear";
                DgGrupo.Columns[4].Header = "Modificar";
                DgGrupo.Columns[5].Header = "Eliminar";

                datamanager.ConexionCerrar();
            }
        }
        private void guardarPU()
        {

            try
            {
                // Recorriendo el DgGrid y  Guardando
                if (datamanager.ConexionAbrir())

                {

                    SqlCommand Cmd1 = new SqlCommand();

                    // Conectar a SQL y preparar la ejecuci'on del procedimiento SQL
                    Cmd1.Connection = datamanager.ConexionSQL;
                    Cmd1.CommandText = "dbo.segPerfilUsuarioRUD";
                    Cmd1.CommandType = CommandType.StoredProcedure;

                    // Creando los parametros en el mismo orden que estan en el procedure
                    Cmd1.Parameters.Add("@idUsuario", SqlDbType.Int).Value = 0;
                    Cmd1.Parameters.Add("@idSegItem", SqlDbType.VarChar).Value = "";
                    Cmd1.Parameters.Add("@acceso", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@crear", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@modificar", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@borrar", SqlDbType.Bit).Value = false;

                    //El Grupo Seleccionado en el ComboBox
                    int IdUsuario = ((ComboBoxItem)CbUsuario.SelectedItem).intValue;

                    // Recorrer el DataGrid por completo
                    for (int i = 0; i < DgGrupo.Items.Count; i++)
                    {

                        // Extrarer valor de cada fila (celda) del datagrid y ponerlo en una variable
                        var idSegitem = (DgGrupo.Items[i] as System.Data.DataRowView).Row.ItemArray[0];
                        var acceso = (DgGrupo.Items[i] as System.Data.DataRowView).Row.ItemArray[2];
                        var crear = (DgGrupo.Items[i] as System.Data.DataRowView).Row.ItemArray[3];
                        var modificar = (DgGrupo.Items[i] as System.Data.DataRowView).Row.ItemArray[4];
                        var borrar = (DgGrupo.Items[i] as System.Data.DataRowView).Row.ItemArray[5];

                        // Asignar los valores de las variables a los parametros del procedure SQL
                        Cmd1.Parameters["@idUsuario"].Value = IdUsuario;
                        Cmd1.Parameters["@idSegitem"].Value = idSegitem;
                        Cmd1.Parameters["@acceso"].Value = acceso;
                        Cmd1.Parameters["@crear"].Value = crear;
                        Cmd1.Parameters["@modificar"].Value = modificar;
                        Cmd1.Parameters["@borrar"].Value = borrar;

                        // Ejecutar el procedure SQL
                        Cmd1.ExecuteNonQuery();
                    }

                    datamanager.ConexionCerrar();
                    MessageBox.Show(" Guardado", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.llenaGridPU();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.guardarPU();
        }
    }
}


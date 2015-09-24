using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupoAsignafrm.xaml
    /// </summary>
    public partial class segGrupoAsignafrm : Window
    {

        string idSegItem = "HS0103";

        bool permiteModificar = false;
        bool permiteCrear = false;
        bool permiteBorrar = false;

        public segGrupoAsignafrm()
        {

            // Cargar los permisos del usuario para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();

        }
 
       private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            this.guardar();     
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

            DataSet dsGrid = new DataSet();
            dsGrid.Clear();
       
            if (cbGrupo.SelectedValue != null)
            {

                int selectedValue = ((ComboBoxItem)cbGrupo.SelectedItem).intValue;

                dsGrid = datamanager.ConsultaDatos("exec dbo.[segGrupoMiembro] " + selectedValue.ToString());

                dataGrid.IsReadOnly = !permiteModificar;

                dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;
                dataGrid.Columns[2].Visibility = System.Windows.Visibility.Hidden;
                dataGrid.Columns[3].Visibility = System.Windows.Visibility.Hidden;
                dataGrid.Columns[4].Visibility = System.Windows.Visibility.Hidden;
             
                dataGrid.Columns[0].Width = 250;
                dataGrid.Columns[0].IsReadOnly = true;
                dataGrid.Columns[0].Header = "Usuario";
                dataGrid.Columns[1].Header = "Miembro";

                datamanager.ConexionCerrar();
            }

            btnAceptar.IsEnabled = false;
            btnAceptar_png.IsEnabled = false;
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


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            btnAceptar.IsEnabled = permiteModificar;
            btnAceptar_png.IsEnabled = btnAceptar.IsEnabled;
        }
    }
}

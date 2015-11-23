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
    /// Interaction logic for segUsuarioPerfilfrm.xaml
    /// </summary>
    public partial class segUsuarioPerfilfrm : Window
    {

        #region  Cargango ItemsSeg


        string idSegItem = "HS0105";

        bool permiteModificar = false;
        bool permiteCrear = false;
        bool permiteBorrar = false;

        public segUsuarioPerfilfrm()
        {
            // Cargar los permisos del usuario para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();


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

        #endregion



        #region GuardarGrid


        public void llenaGridPU()
        {
            DataSet dsGrid = new DataSet();
            dsGrid.Clear();

            if (CbUsuario.SelectedValue != null)
            {
                
                int selectedValue = ((ComboBoxItem)CbUsuario.SelectedItem).intValue;
                dsGrid = datamanager.ConsultaDatos(" exec segPerfilUsuario " + selectedValue.ToString());

                DgPermisos.IsReadOnly = !permiteModificar;

                DgPermisos.ItemsSource = dsGrid.Tables[0].DefaultView;
                DgPermisos.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                DgPermisos.Columns[6].Visibility = System.Windows.Visibility.Hidden;
                DgPermisos.Columns[7].Visibility = System.Windows.Visibility.Hidden;
                DgPermisos.Columns[8].Visibility = System.Windows.Visibility.Hidden;
                DgPermisos.Columns[9].Visibility = System.Windows.Visibility.Hidden;
                DgPermisos.Columns[1].Width = 193;
                DgPermisos.Columns[1].IsReadOnly = true;
                DgPermisos.Columns[2].Header = "Acceso";
                DgPermisos.Columns[2].Width = 60;
                DgPermisos.Columns[3].Header = "Crear";
                DgPermisos.Columns[3].Width = 60;
                DgPermisos.Columns[4].Header = "Modificar";
                DgPermisos.Columns[4].Width = 60;
                DgPermisos.Columns[5].Header = "Eliminar";
                DgPermisos.Columns[5].Width = 60;

                datamanager.ConexionCerrar();

                btnAceptar.IsEnabled = false;
                btnAceptar_png.IsEnabled = false;
            }
        }
        #endregion


        #region  RecorriendoGrid


        private void guardarPU()
        {

            try
            {
                // Recorriendo el DgGrid y  Guardando
                if (datamanager.ConexionAbrir())

                {

                    SqlCommand Cmd1 = new SqlCommand();

                    // Conectar a SQL y preparar la ejecucion del procedimiento SQL
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
                    for (int i = 0; i < DgPermisos.Items.Count; i++)
                    {

                        // Extrarer valor de cada fila (celda) del datagrid y ponerlo en una variable
                        var idSegitem = (DgPermisos.Items[i] as System.Data.DataRowView).Row.ItemArray[0];
                        var acceso = (DgPermisos.Items[i] as System.Data.DataRowView).Row.ItemArray[2];
                        var crear = (DgPermisos.Items[i] as System.Data.DataRowView).Row.ItemArray[3];
                        var modificar = (DgPermisos.Items[i] as System.Data.DataRowView).Row.ItemArray[4];
                        var borrar = (DgPermisos.Items[i] as System.Data.DataRowView).Row.ItemArray[5];

                        // Asignar los valores de las variables a los parametros del procedure SQL
                        Cmd1.Parameters["@idUsuario"].Value = IdUsuario;
                        Cmd1.Parameters["@idSegitem"].Value = idSegitem;
                        Cmd1.Parameters["@acceso"].Value = acceso;
                        Cmd1.Parameters["@crear"].Value = crear;
                        Cmd1.Parameters["@modificar"].Value = modificar;
                        Cmd1.Parameters["@borrar"].Value = borrar;

                        // Ejecutar el procedure SQL
                        Cmd1.ExecuteNonQuery();

                        // Actualizar los permisos actuales del usuario
                        if (IdUsuario == datamanager.idUsuario) {
                            datamanager.cargaPermisos(IdUsuario);
                        }
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


        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.guardarPU();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DgPermisos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            btnAceptar.IsEnabled = permiteModificar;
            btnAceptar_png.IsEnabled = btnAceptar.IsEnabled;
        }

        
    }
 }

#endregion 
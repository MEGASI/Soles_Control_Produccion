using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupoPerfilfrm.xaml
    /// </summary>
    public partial class segGrupoPerfilfrm : Window
    {
        #region Cargando ItemSeg 
        string idSegItem = "HS0104";

        bool permiteModificar = false;
        bool permiteCrear = false;
        bool permiteBorrar = false;

        public segGrupoPerfilfrm()
        {

            // Cargar los permisos del usuario para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

 
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.llenaGridPG();
        }


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.guardarPG();
        }

        #endregion


        #region  Select CB
        private void CbGrupo_Loaded(object sender, RoutedEventArgs e)
        {
           SqlDataReader reader =
           datamanager.ConsultaLeer("select nombre, idGrupo from segGrupo");

            while (reader != null && reader.Read())
            {
                CbGrupo.Items.Add(new ComboBoxItem(reader.GetString(0), reader.GetInt32(1)));
            }

            CbGrupo.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        #endregion

        #region LlenandoGrid

        public void llenaGridPG()
        {

            DataSet dsGrid = new DataSet();
            dsGrid.Clear();

            if (CbGrupo.SelectedValue != null)
            {

                int selectedValue = ((ComboBoxItem)CbGrupo.SelectedItem).intValue;
                dsGrid = datamanager.ConsultaDatos("exec[segPerfilGrupo] " + selectedValue.ToString());

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

        #region Metodo Guardar

        private void guardarPG()
        {
            try
            {
                // Recorriendo el DgGrid y  Guardando
                if (datamanager.ConexionAbrir())
                {
                    SqlCommand Cmd1 = new SqlCommand();

                    // Conectar a SQL y preparar la ejecuci'on del procedimiento SQL
                    Cmd1.Connection = datamanager.ConexionSQL;
                    Cmd1.CommandText = "dbo.segPerfilGrupoRUD";
                    Cmd1.CommandType = CommandType.StoredProcedure;

                    // Creando los parametros en el mismo orden que estan en el procedure
                    Cmd1.Parameters.Add("@idGrupo", SqlDbType.Int).Value = 0;
                    Cmd1.Parameters.Add("@idSegItem", SqlDbType.VarChar).Value = "";
                    Cmd1.Parameters.Add("@acceso", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@crear", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@modificar", SqlDbType.Bit).Value = false;
                    Cmd1.Parameters.Add("@borrar", SqlDbType.Bit).Value = false;

                    //El Grupo Seleccionado en el ComboBox
                    int idGrupo = ((ComboBoxItem)CbGrupo.SelectedItem).intValue;

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
                        Cmd1.Parameters["@idGrupo"].Value = idGrupo;
                        Cmd1.Parameters["@idSegitem"].Value = idSegitem;
                        Cmd1.Parameters["@acceso"].Value = acceso;
                        Cmd1.Parameters["@crear"].Value = crear;
                        Cmd1.Parameters["@modificar"].Value = modificar;
                        Cmd1.Parameters["@borrar"].Value = borrar;

                        // Ejecutar el procedure SQL
                        Cmd1.ExecuteNonQuery();

                        // Cagar los permisos del usuario actual
                        // Actualizar los permisos actuales del usuario
                    }
                    datamanager.ConexionCerrar();
                    MessageBox.Show(" Guardado", "Guardando", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.llenaGridPG();
                    datamanager.cargaPermisos(datamanager.idUsuario);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

       
        private void DgPermisos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            btnAceptar.IsEnabled = permiteModificar;
            btnAceptar_png.IsEnabled = btnAceptar.IsEnabled;
        }

       
    }
}
#endregion
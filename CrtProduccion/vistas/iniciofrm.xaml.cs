using System.Windows;


namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for iniciofrm.xaml
    /// </summary>
    public partial class iniciofrm : Window
    {
        #region Acceso

        public iniciofrm()
        {
            // Controla el acceso al sistema.
            segLoginfrm dlg = new segLoginfrm();
            dlg.ShowDialog();
            if (dlg.DialogResult.HasValue == false)
                Application.Current.Shutdown();
            else
            {

                // Validar que el usuario logueado tiene acceso al sistema.
                if (!datamanager.probarPermiso("0", "acceso") || dlg.DialogResult.Value == false)
                {
                    if (dlg.DialogResult.Value != false)
                        MessageBox.Show("No tiene acceso al sistema.", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }

                // Iniciar todos los componentes del menu principal
                InitializeComponent();
                opcionesEstado();
            }

        }
        #endregion

        #region Opciones  de Estado
        private void opcionesEstado()
        {

            mainArchivo.IsEnabled = datamanager.probarPermiso("A", "acceso");
            mainHerramientas.IsEnabled = datamanager.probarPermiso("H", "acceso");
            herCambioClave.IsEnabled = datamanager.probarPermiso("H00", "acceso");

            #endregion


       #region Heramientas de Seguridad
            // Opcion de Herramienta - Seguridad
            herSeguridad.IsEnabled = datamanager.probarPermiso("HS01", "acceso");
                

            // Opciones de Seguridad
            segUsuario.IsEnabled = datamanager.probarPermiso("HS0101", "acceso");
            segGrupo.IsEnabled = datamanager.probarPermiso("HS0102", "acceso");
            segAsignaGrupo.IsEnabled = datamanager.probarPermiso("HS0103", "acceso");
            segPerfilGrupo.IsEnabled = datamanager.probarPermiso("HS0104", "acceso");
            segPerfilUsuario.IsEnabled = datamanager.probarPermiso("HS0105", "acceso");
            SegLibrodirecciones.IsEnabled = datamanager.probarPermiso("AD0101", "acceso");
            SegDepartemento.IsEnabled = datamanager.probarPermiso("AD0102", "acceso");
            SegCargo.IsEnabled = datamanager.probarPermiso ("AD0103", "acceso");
            herCambioClave.IsEnabled = datamanager.probarPermiso("Ch", "acceso");           
            

            
        }
        #endregion
      
        
        #region Opciones de Seguridad
        private void segUsuario_Click(object sender, RoutedEventArgs e)
        {
            segUsuariofrm dlg = new segUsuariofrm();
            dlg.ShowDialog();
        }


        private void segGrupo_Click(object sender, RoutedEventArgs e)
        {
            segGrupofrm dlg = new segGrupofrm();
            dlg.ShowDialog();
        }

        private void segAsignaGrupo_Click(object sender, RoutedEventArgs e)
        {
            segGrupoAsignafrm dlg = new segGrupoAsignafrm();
            dlg.ShowDialog();
        }

        private void segPerfilGrupo_Click(object sender, RoutedEventArgs e)
        {
            segGrupoPerfilfrm dlg = new segGrupoPerfilfrm();
            dlg.ShowDialog();

            // Actualiza los estados de las opciones por si cambian los permisos.
            opcionesEstado();
        }

        private void segPerfilUsuario_Click(object sender, RoutedEventArgs e)
        {
            segUsuarioPerfilfrm dlg = new segUsuarioPerfilfrm();
            dlg.ShowDialog();

            // Actualiza los estados de las opciones por si cambian los permisos.
            opcionesEstado();
        }

        private void mainSalir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Seguro que Desea Salir ?", "Salir",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                Application.Current.Shutdown();
        }
        #endregion


        #region FormulariosDiag

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            vistas.dptofrm dlg = new vistas.dptofrm();

            dlg.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            vistas.Cargofrm dlg = new vistas.Cargofrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            vistas.Proyectofrm dlg = new vistas.Proyectofrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            vistas.LibroDfrm dlg = new vistas.LibroDfrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            vistas.Vehiculo_Tipofrm dlg = new vistas.Vehiculo_Tipofrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            vistas.Vehiculo_Marcafrm dlg = new vistas.Vehiculo_Marcafrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            vistas.Vehiculo_Partesfrm dlg = new vistas.Vehiculo_Partesfrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            vistas.Colorfrm dlg = new vistas.Colorfrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            vistas.Vehiculofrm dlg = new vistas.Vehiculofrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            vistas.Actividafrm dlg = new vistas.Actividafrm();
            dlg.ShowDialog();
        }

        private void herCambioClave_Click(object sender, RoutedEventArgs e)
        {
            vistas.Pswfrm dlg = new vistas.Pswfrm();
            //segUsuariofrm dlg = new segUsuariofrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            vistas.Partidafrm dlg = new vistas.Partidafrm();
            dlg.ShowDialog();
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
          
                vistas.Brigadafrm dlg = new vistas.Brigadafrm();
                dlg.ShowDialog();
        
                  
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            vistas.IDPfrm dlg = new vistas.IDPfrm();
            dlg.ShowDialog();
        }
    }
}
#endregion


using System.Windows;



namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for WelcomeForm.xaml
    /// </summary>
    public partial class WelcomeForm : Window
    {

        Permiso sgPermiso;
        public WelcomeForm()
        {
            InitializeComponent();


           

            if (!datamanager.probarPermiso("0", "acceso"))
            {
                // No tiene acceso al sistema, close
                this.Close();
            }

            //sgPermiso = new Permiso(datamanager.IdUsuario, "0");

            //  if (!sgPermiso.acceso) {
            // No tiene acceso al sistema, close
            //     this.Close();
            // }



        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {   
          }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
          }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //sgPermiso.cargar(datamanager.IdUsuario, "P01");

            if (datamanager.probarPermiso("P01","acceso"))
            {

                FrmUsuario U = new FrmUsuario();
                U.ShowDialog();
            }
            else
            {
                // No tiene acceso
                MessageBox.Show("No tiene permiso", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            //    WindowInteropHelper parentHelper = new WindowInteropHelper(this);

            //    WindowInteropHelper childHelper = new WindowInteropHelper(U);
            //    Win32Native.SetParent(childHelper.Handle, parentHelper.Handle);
            //}

            //     public class Win32Native
            //{

            //    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetParent")]

            //    public extern static IntPtr SetParent(IntPtr childPtr, IntPtr parentPtr);

        }
        protected void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Seguro que Desea Salir ?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
            }
            else
            {
                Application.Current.Shutdown();
            }
           }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //sgPermiso.cargar(datamanager.IdUsuario, "P01");

            if (datamanager.probarPermiso("P01","acceso"))
            {
                FrmEditGroup FG = new FrmEditGroup();
                // G.sgPermiso = sgPermiso;  //Creo que puedo hacer eso para pasarlo al formulario
                FG.ShowDialog();
            }
            else {
                // No tiene acceso
                MessageBox.Show("No tiene permiso", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            
        }
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
        }
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            // Tienes que buscar el itemId de este
            if (datamanager.probarPermiso("P0???", "acceso"))
            {
                FrmPG PG = new FrmPG();
                PG.ShowDialog();
            }
        }
        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            FrmAUG FG = new FrmAUG();
                FG.ShowDialog();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {        
        }
        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
        }
        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            FrmPU PU = new FrmPU();
            PU.ShowDialog();
        }
        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {

        }
    }
    }
 


using System.Windows;
using System.Windows.Input;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segUsuarioEditarfrm.xaml
    /// </summary>
    public partial class segUsuariofrm : Window
    {

        entidades.dmUsuario regUsuario = new entidades.dmUsuario();

        private string _modalidad = "";

        public string modalidad
        {
            get { return _modalidad; }
            set {

                if (_modalidad != value) {
                    if (value == "CREAR" || value == "MODIFICAR") {
                        btnBorrar.IsEnabled = false;
                        btnSalir.IsEnabled = false;
                        btnBuscar.IsEnabled = false;

                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;
                        
                        btnCancelar.Visibility = Visibility.Visible;
                        btnGuardar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, true);
                        if (value == "MODIFICAR") {
                            txtNombre.IsEnabled = false;
                        }

                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true;
                        btnSalir.IsEnabled = true;
                        btnBuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        btnCancelar.Visibility = Visibility.Hidden;
                        btnGuardar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, false);
                        txtNombre.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        public segUsuariofrm()
        {
            InitializeComponent();


            regUsuario.buscarUltimo();
            mostrar();


            if (regUsuario.fld_idusuario == 0)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }


        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            regUsuario.limpiar();
            mostrar();
            modalidad = "CREAR";
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            regUsuario.buscar(regUsuario.fld_oldIDUsuario);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }

        
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            regUsuario.fld_oldIDUsuario = regUsuario.fld_idusuario;
            modalidad = "MODIFICAR";
            txtClave.Focus();
        }


        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;

            if (this.modalidad == "CREAR") {
                lret = regUsuario.crearDatos()>0;
            }

            if (this.modalidad == "MODIFICAR")
            {
                lret = regUsuario.actualizarDatos();
            }

            if (lret)
            {
                modalidad = "CONSULTAR";
               MessageBox.Show("Información del usuario fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {

            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este usuario?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                        
                if (this.modalidad == "CONSULTAR" && regUsuario.fld_idusuario!=0)
                {
                    lret = regUsuario.borrarDatos(regUsuario.fld_idusuario);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }

            }
        }



        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void mostrar() {
            txtNombre.Text = regUsuario.fld_nombre;
            txtClave.Password = regUsuario.fld_clave;
        }


        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter )
            {
                txtNombre_LostFocus(sender, e);
            }

        }

        private void txtClave_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool fromtxt = ((System.Windows.Controls.PasswordBox)sender).IsFocused;

            
            if (fromtxt==true)
            {
                regUsuario.fld_cambiopsw = true;
            }
        }

   

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtNombre.Text.Equals(regUsuario.fld_nombre)) {
                if (!regUsuario.buscar(txtNombre.Text))
                    MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);

                mostrar();
                txtNombre.Focus();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.segUsuarioBRWfrm dlgfrm = new vistas.segUsuarioBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value) {
                // Si el suario presiona Aceptar
                if (!regUsuario.buscar(dlgfrm.idUsuario)){
                    MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    mostrar();
                    txtNombre.Focus();
                }
            }

            
        }
    }
    
}

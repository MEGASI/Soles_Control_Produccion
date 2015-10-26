using System.Windows;
using System.Windows.Input;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segUsuarioEditarfrm.xaml
    /// </summary>
    public partial class segUsuariofrm : Window
    {

        #region Declaración de Variables y Propiedades

        private entidades.dmUsuario registro { get; set; }
              
        string idSegItem = "HS0101";

        bool permiteModificar = false;
        bool permiteCrear = false;
        bool permiteBorrar = false;

        private string _modalidad = "";
        public string modalidad
        {
            get { return _modalidad; }
            set
            {

                if (_modalidad != value)
                {
                    if (value == "CREAR" || value == "MODIFICAR")
                    {
                        btnBorrar.IsEnabled = false;
                        btnSalir.IsEnabled = false;
                        btnBuscar.IsEnabled = false;

                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, true);
                        if (value == "MODIFICAR")
                        {
                            txtNombre.IsEnabled = false;
                        }
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnBuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtNombre.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }
        #endregion


        #region Cargando Permisos
        // Constructor del Formulario
        public segUsuariofrm()
        {

            // Cargar los permisos del usuario para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
           
        }

        private void segUsuariofrm_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmUsuario();
            registro.buscarUltimo();

            //DataContext = registro;
            mostrar();

            // Operaciones permitidas en este formulario.
            // Implementación de la seguridad del formulario.
            // Crear
            btnNuevo.IsEnabled = permiteCrear;
            // Modificar
            btnModificar.IsEnabled = permiteModificar;
            // Borrar
            btnBorrar.IsEnabled = permiteBorrar;

            if (registro.fld_idusuario == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
#endregion


        #region Funcionalidades de los botones

        // Click del Boton Nuevo
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldIDUsuario = registro.fld_idusuario;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtNombre.Focus();
        }

        // Click del Boton Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_oldIDUsuario,true);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }

        // Click del Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldIDUsuario = registro.fld_idusuario;
            modalidad = "MODIFICAR";
            txtClave.Focus();
        }

        // Click Boton Guardar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            registro.fld_clave = txtClave.Password;

            // Validamos que los datos estan correcto, conforme a las reglas establecidas.
            bool lret = registro.validar();

            if (lret && this.modalidad == "CREAR")
               lret = registro.crearDatos() > 0;
            
            if (lret && this.modalidad == "MODIFICAR")
               lret = registro.actualizarDatos();
            
            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del usuario fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            } else     
               MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Click Boton Borrar
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {

            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este usuario?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idusuario != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idusuario);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtNombre.Focus();
        }

        // Click Boton Salir
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Click Boton Buscar
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.segUsuarioBRWfrm dlgfrm = new vistas.segUsuarioBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idUsuario,true))
                {
                    MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtNombre.Focus();
                }
            }
        }
        #endregion


        #region Validaciones 

        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtNombre_LostFocus(sender, e);
            }
        }

        private void txtClave_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool fromtxt = ((System.Windows.Controls.PasswordBox)sender).IsFocused;
            if (fromtxt == true)
            {
                registro.fld_cambiopsw = true;
            }
        }

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {

           if (!txtNombre.Text.Equals(registro.fld_nombre))
            {
                registro.fld_nombre = txtNombre.Text;

                bool found = registro.buscar(txtNombre.Text,false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtNombre.Focus();
                }
                else {
                    if (found)
                    {
                        MessageBox.Show("Usuario ya existe.", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtNombre.Focus();
                    }
                }

           }
        }

        #endregion

        #region Valores Extraidos de La BD

        /// <summary>
        /// Muestra los valores que se traen desde la base de datos
        /// Asignando el campo equivalente de cada control en el formulario.
        /// </summary>
        /// 


        private void mostrar()
        {

            //DataContext = null;
            //DataContext = registro;

            txtNombre.Text = registro.fld_nombre;
            txtClave.Password = registro.fld_clave;
            

        }


    }
}

#endregion
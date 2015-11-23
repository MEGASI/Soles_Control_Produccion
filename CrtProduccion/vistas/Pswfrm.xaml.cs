using System.Windows;
using System.Windows.Input;

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Pswfrm.xaml
    /// </summary>
    public partial class Pswfrm : Window
    {
        #region Declaración de Variables y Propiedades

        private entidades.dmpssw registro { get; set; }

        string idSegItem = "Ch";

        bool permiteModificar = false;
       
        

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
                        
                        btnSalir.IsEnabled = false;
                        btnbuscar.IsEnabled = false;


                        btnModificar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, true);
                        if (value == "MODIFICAR")
                        {
                            txtNombre.IsEnabled = false;
                        }
                    }

                    if (value == "CONSULTAR")
                    {
                     
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;


                        btnModificar.Visibility = Visibility.Visible;
                        


                        comunes.libreria.estadoControles(this, false);
                        txtNombre.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }
        #endregion


        #region Cargar Permisosgando 
        // Constructor del Formulario

        public Pswfrm()
        {
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            
            


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmpssw();
            registro.buscarUltimo();

            //DataContext = registro;
            mostrar();

            // Operaciones permitidas en este formulario.
            // Implementación de la seguridad del formulario.
            

            // Modificar
            btnModificar.IsEnabled = permiteModificar;
            
           

            if (registro.fld_idusuario == 0 && permiteModificar)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion



        #region Funcionalidades de los botones

        // Click del Boton Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

            registro.buscar(registro.fld_oldIDUsuario, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldIDUsuario = registro.fld_idusuario;
            modalidad = "MODIFICAR";
            TxtpswNueClave.Focus();

        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            //ValidaCion de Claves Iguales
            if (comunes.libreria.ValidarPsw(TxtpswNueClave.Password, TxtpswConfiClave.Password))
            {
                MessageBox.Show("Las Claves no Coinciden", "Error en la Validacion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                registro.fld_clave = TxtpswConfiClave.Password;

                bool lret = registro.validar();

                if (lret && this.modalidad == "CREAR")
                    lret = registro.crearDatos() > 0;

                if (lret && this.modalidad == "MODIFICAR")
                    lret = registro.actualizarDatos();

                if (lret)
                {

                    modalidad = "CONSULTAR";
                    MessageBox.Show("Información del usuario fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                    MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.segUsuarioBRWfrm dlgfrm = new vistas.segUsuarioBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idUsuario, true))
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

        private void txtNombre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtNombre.Text.Equals(registro.fld_nombre))
            {
                registro.fld_nombre = txtNombre.Text;

                bool found = registro.buscar(txtNombre.Text, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtNombre.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Usuario ya existe.", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtNombre.Focus();
                    }
                }

            }
        }

        private void TxtpswConfiClave_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool fromtxt = ((System.Windows.Controls.PasswordBox)sender).IsFocused;
            if (fromtxt == true)
            {
                registro.fld_cambiopsw = true;
            }

        }

        private void mostrar()
        {
            txtNombre.Text = registro.fld_nombre;
            TxtpswClave.Password = registro.fld_clave;
        }

        

    }
}
#endregion
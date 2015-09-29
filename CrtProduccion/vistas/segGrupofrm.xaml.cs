using System;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupofrm.xaml
    /// </summary>
    public partial class segGrupofrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmGrupo registro { get; set; }

        string idSegItem = "HS0102";

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
                        btnbuscar.IsEnabled = false;


                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, true);
                        txtIdGrupo.IsEnabled = false;
                       

                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtIdGrupo.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion


        #region Constructor y Loader
        //   Constructor del Fromulario
        public segGrupofrm()
        {
            // Cargar los permisos del grupo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
       }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmGrupo();
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

            if (registro.Fld_idGrupo == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion


        #region Funcionalidades de los Botones

        // Click del boton Nuevo
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldIdGrupo = registro.Fld_idGrupo;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtNombre.Focus();
        }

        // Click del Boton Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.Fld_idGrupo, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }

        // Click Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldIdGrupo = registro.Fld_idGrupo;
            modalidad = "MODIFICAR";
            txtIdGrupo.Focus();
        }

        // Click Boton Gurdar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            // Asignar los valores de los conroles del formulario a los campos.
            registro.Fld_NombreGrupo = txtNombre.Text;

            // Validar los valores asignados.
            bool lret = registro.validar();

            
            if (lret && this.modalidad == "CREAR")
                lret = registro.crearDatos() > 0;

            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del Grupo fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Click Boton Borrar
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Grupo de Usuario?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.Fld_idGrupo != 0)
                {
                    lret = registro.borrarDatos(registro.Fld_idGrupo);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtNombre.Focus();
        }


        // Click boton Salir
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        // Click Boton Buscar
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            segGrupoBRWfrm dlgfrm = new segGrupoBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idGrupo,true))
                {
                    MessageBox.Show("Nombre de Grupo no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
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

      
        private void txtIdGrupo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtIdGrupo_LostFocus(sender, e);
            }
        }

        private void txtIdGrupo_LostFocus(object sender, RoutedEventArgs e)
        {
            int idGrupo = 0;

            if (!Int32.TryParse(txtIdGrupo.Text, out idGrupo))
            {
                idGrupo = 0;
            }

            if (idGrupo!=registro.Fld_idGrupo)
            {
                registro.Fld_idGrupo = idGrupo;
                bool found = registro.buscar(idGrupo, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Grupo no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idGrupo, true);

                    mostrar();
                    txtIdGrupo.Focus();
                }
                
            }


        }

        /// <summary>
        /// Muestra los Grupos Existentes y no permite agregarlos si ya existen 
        /// Parte de la validacion.
        /// </summary>

        private void NameGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtNombre.Text.Equals(registro.Fld_NombreGrupo))
            {
                registro.Fld_NombreGrupo = txtNombre.Text;

                bool found = registro.buscar(txtNombre.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Grupo no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtNombre.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Grupo ya existe.", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtNombre.Focus();
                    }
                }
            }

        }
        /// <summary>
        ///  Consiste en Recuperar el Foco , ya sea que se pierda por accion del usuario.
        /// Parte de la validacion.
        /// </summary>

        private void NameGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                NameGroup_LostFocus(sender, e);
            }
        }


        #endregion

        #region Valores extraidos de la BD
        /// <summary>
        /// Muestra los valores que se traen desde la base de datos.
        /// Asignando el campo equivalente de cada control en el formulario.
        /// </summary>
        private void mostrar()
        {
            txtNombre.Text = registro.Fld_NombreGrupo;
            txtIdGrupo.Text = Convert.ToInt16(registro.Fld_idGrupo).ToString();
        }
        #endregion
    }
}





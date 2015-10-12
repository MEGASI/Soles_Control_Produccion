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

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Cargofrm.xaml
    /// </summary>
    public partial class Cargofrm : Window
    {
        #region Declaraciones de variables y Propiedades

        private entidades.dmCargo registro { get; set; }

        string idSegItem = "AD0103";

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
                        txtidCargo.IsEnabled = false;
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtidCargo.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }

        }
        #endregion
        #region Constructor y Loader
        //   Constructor del Fromulario
        public Cargofrm()
        {
            // Cargar los permisos del Cargo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmCargo();
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

            if (registro.fld_idCargo == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion
        #region Funcionalidades de los Botones

        // Click del boton Nuevo
        private void btnNuevo_Click_1(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidCargo = registro.fld_idCargo;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtNombre.Focus();
        }

        // Click del Boton Cancelar
        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idCargo, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }
        // Click Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidCargo = registro.fld_idCargo;
            modalidad = "MODIFICAR";
            txtidCargo.Focus();
        }
        // Click Boton Gurdar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_NombreCargo = txtNombre.Text;

            // Validar los valores asignados.
            bool lret = registro.validar();
            if (lret && this.modalidad == "CREAR")
                lret = registro.crearDatos() > 0;
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del Cargo fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Click Boton Borrar
        private void btnBorrar_Click_(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Cargo de Usuario?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idCargo != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idCargo);
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
        private void btnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        ///  la funcion que realiza es buscar de la tabla cargo el nombre 
        /// </summary>
        // Click Boton Buscar
        private void btnbuscar_Click_1(object sender, RoutedEventArgs e)
        {
            vistas.CargoBRWf dlgfrm = new vistas.CargoBRWf();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idCargo, true))
                {
                    MessageBox.Show("Nombre de Cargo no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void txtidCargo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtidCargo_LostFocus(sender, e);
            }
        }
        private void txtidCargo_LostFocus(object sender, RoutedEventArgs e)
        {
            int idCargo = 0;
            if (!Int32.TryParse(txtidCargo.Text, out idCargo))
            {
                idCargo = 0;
            }
            if (idCargo != registro.fld_idCargo)
            {
                registro.fld_idCargo = idCargo;
                bool found = registro.buscar(idCargo, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Cargo no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idCargo, true);

                    mostrar();
                    txtidCargo.Focus();
                }
            }
        }
        private void NameGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtNombre.Text.Equals(registro.fld_NombreCargo))
            {
                registro.fld_NombreCargo = txtNombre.Text;

                bool found = registro.buscar(txtNombre.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Cargo no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtNombre.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Cargo ya existe.", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtNombre.Focus();
                    }
                }
            }
        }
        #endregion

        #region Valores Extraidos de la BD

        /// <summary>
        /// Muestra los valores que se traen desde la base de datos
        /// Asignando el campo equivalente de cada control en el formulario.
        /// </summary>
        private void mostrar()
        {
            txtNombre.Text = registro.fld_NombreCargo;
            txtidCargo.Text = Convert.ToInt16(registro.fld_idCargo).ToString();
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtidCargo_LostFocus(sender, e);
            }
        }
        #endregion    
    }
}

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
    /// Interaction logic for Vehiculo_Marcafrm.xaml
    /// </summary>
    public partial class Vehiculo_Marcafrm : Window
    {
        #region Declaraciones de variables y Propiedades

        private entidades.dmVehiculoMarca registro { get; set; }

        string idSegItem = "AT0104";

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
                        TxtidVehiculo_M.IsEnabled = false;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        TxtidVehiculo_M.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion

        #region Constructor y Loader
        //   Constructor del Fromulario
        public Vehiculo_Marcafrm()
        {
            // Cargar los permisos del grupo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmVehiculoMarca();
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

            if (registro.fld_idMacarV == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion

        #region Funcionalidades de los Botones
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidMarcaV = registro.fld_idMacarV;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtDescripcion.Focus();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idMacarV, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtDescripcion.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidMarcaV = registro.fld_idMacarV;
            modalidad = "MODIFICAR";
            TxtidVehiculo_M.Focus();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Descripcion = txtDescripcion.Text;

            // Validar los valores asignados.
            bool lret = registro.validar();
            if (lret && this.modalidad == "CREAR")
                lret = registro.crearDatos() > 0;
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del Vehiculo fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Vehiculo de Registro?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idMacarV != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idMacarV);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtDescripcion.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.Vehiculo_MarcaBRWfrm dlgfrm = new vistas.Vehiculo_MarcaBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idMarca, true))
                {
                    MessageBox.Show("Descripcion de Vehiculo no existe", "Vehiculo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtDescripcion.Focus();
                }
            }
        }
        #endregion
        #region Validaciones
        private void TxtidVehiculo_M_LostFocus(object sender, RoutedEventArgs e)
        {
            int idMarcaV = 0;
            if (!Int32.TryParse(TxtidVehiculo_M.Text, out idMarcaV))
            {
                idMarcaV = 0;
            }
            if (idMarcaV != registro.fld_idMacarV)
            {
                registro.fld_idMacarV = idMarcaV;
                bool found = registro.buscar(idMarcaV, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Cargo no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idMarcaV, true);

                    mostrar();
                    TxtidVehiculo_M.Focus();
                }
            }
        }

        private void TxtidVehiculo_M_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtidVehiculo_M_LostFocus(sender, e);
            }
        }
        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtDescripcion.Text.Equals(registro.fld_Descripcion))
            {
                registro.fld_Descripcion = txtDescripcion.Text;

                bool found = registro.buscar(txtDescripcion.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Descripcion del  la Marca  no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtDescripcion.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Marca ya existe.", "Marca", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtDescripcion.Text = "";
                        txtDescripcion.Focus();
                    }
                }
            }

            #endregion

        #region Valores Extraidos de la BD

        }
        private void mostrar()
        {
            txtDescripcion.Text = registro.fld_Descripcion;
            TxtidVehiculo_M.Text = Convert.ToInt16(registro.fld_idMacarV).ToString();
        }

       
    }
}

#endregion



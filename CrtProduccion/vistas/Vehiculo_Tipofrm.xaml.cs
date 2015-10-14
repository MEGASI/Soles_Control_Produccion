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
    /// Interaction logic for Vehiculo_Tipofrm.xaml
    /// </summary>
    public partial class Vehiculo_Tipofrm : Window
    {
        #region Declaraciones de variables y Propiedades

        private entidades.dmVehiculo_tipo registro { get; set; }

        string idSegItem = "AT0103";

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
                        TxtidVehiculo_T.IsEnabled = false;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        TxtidVehiculo_T.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion

        #region Constructor y Loader
        //   Constructor del Fromulario
        public Vehiculo_Tipofrm()
        {
            // Cargar los permisos del grupo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmVehiculo_tipo();
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

            if (registro.fld_idTipoV == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }


        #endregion

        #region Funcionalidades de los Botones

        // Click del boton Nuevo

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidTipoV = registro.fld_idTipoV;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtDescripcion.Focus();
        }
        //Btn Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idTipoV, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtDescripcion.Focus();
        }
        // Click Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidTipoV = registro.fld_idTipoV;
            modalidad = "MODIFICAR";
            TxtidVehiculo_T.Focus();
        }
        // btn Guardar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Descripcion = txtDescripcion.Text;

            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret)
                {
                    TxtidVehiculo_T.Text = registro.fld_idTipoV.ToString();
                }

            }

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
        // Click Boton Borrar
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Vehiculo del Registro ?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idTipoV != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idTipoV);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtDescripcion.Focus();
        }
        // Click boton Salir
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.VehiculoTBRWfrm dlgfrm = new vistas.VehiculoTBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idVehiculo, true))
                {
                    MessageBox.Show("Descripcion del Vehiculo  no existe", "Vehiculo", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void TxtidVehiculo_T_LostFocus(object sender, RoutedEventArgs e)
        {
            int idGrupo = 0;

            if (!Int32.TryParse(TxtidVehiculo_T.Text, out idGrupo))
            {
                idGrupo = 0;
            }

            if (idGrupo != registro.fld_idTipoV)
            {
                registro.fld_idTipoV = idGrupo;
                bool found = registro.buscar(idGrupo, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Grupo no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idGrupo, true);

                    mostrar();
                    TxtidVehiculo_T.Focus();
                }
            }

        }
        private void TxtidVehiculo_T_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtidVehiculo_T_LostFocus(sender, e);
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
                        MessageBox.Show("Nombre de Grupo no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtDescripcion.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Grupo ya existe.", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtDescripcion.Text = "";
                        txtDescripcion.Focus();
                    }
                }
            }

        }

        private void txtDescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtDescripcion_LostFocus(sender, e);
            }
        }
        #endregion


        #region Valores extraidos de la BD
        private void mostrar()
        {
            txtDescripcion.Text = registro.fld_Descripcion;
            TxtidVehiculo_T.Text = Convert.ToInt16(registro.fld_idTipoV).ToString();
        }

        
    }
}
#endregion
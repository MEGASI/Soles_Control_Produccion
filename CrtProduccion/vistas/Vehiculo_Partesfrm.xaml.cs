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
    /// Interaction logic for Vehiculo_Partesfrm.xaml
    /// </summary>
    public partial class Vehiculo_Partesfrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmVehiculoP registro { get; set; }

        string idSegItem = "AT0102";

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
        public Vehiculo_Partesfrm()
        {
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmVehiculoP();
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

            if (registro.fld_idParte == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion
        
        
        
        
                
        #region Funcionalidades de los Botones

        // Click del boton Nuevo
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidPart = registro.fld_idParte;
            
            
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            TxtidVehiculo_M.Focus();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idParte, true);
            mostrar();
            modalidad = "CONSULTAR";
            TxtidVehiculo_M.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidPart = registro.fld_idParte;
            modalidad = "MODIFICAR";
            TxtidVehiculo_M.Focus();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Referencia = txtReferencia.Text;
            registro.fld_Descripcion = txtDescripcion.Text;
            registro.fld_suplidor = txtidSuplidor.Text;
            registro.fld_Precio = Convert.ToDouble(txtPrecio.Text);
            registro.fld_Existencia = Convert.ToDouble(txtexistencia.Text);

            // Validar los valores asignados.
            bool lret = registro.validar();
            if (lret && this.modalidad == "CREAR")
                lret = registro.crearDatos() > 0;
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del la  Parte fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar esta Parte de Resgistro?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idParte != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idParte);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtReferencia.Focus();
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.VehiculoPartesBRWfrm dlgfrm = new vistas.VehiculoPartesBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idParte, true))
                {
                    MessageBox.Show("Descripcion de Parte no existe", "Partes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtReferencia.Focus();
                }
            }
        }
        #endregion
       
        
        #region Validaciones
        private void TxtidVehiculo_M_LostFocus(object sender, RoutedEventArgs e)
        {
            int idParteV = 0;
            if (!Int32.TryParse(TxtidVehiculo_M.Text, out idParteV))
            {
                idParteV = 0;
            }
            if (idParteV != registro.fld_idParte)
            {
                registro.fld_idParte = idParteV;
                bool found = registro.buscar(idParteV, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id de la Parte  no existe", "Partes", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idParteV, true);

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
        private void txtReferencia_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtReferencia.Text.Equals(registro.fld_Referencia))
            {
                registro.fld_Referencia = txtReferencia.Text;

                bool found = registro.buscar(txtReferencia.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de   la Parte  no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
        txtReferencia.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Parte ya existe.", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtReferencia.Text = "";
                        txtReferencia.Focus();
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
            TxtidVehiculo_M.Text = Convert.ToInt32(registro.fld_idParte).ToString();
            txtReferencia.Text = registro.fld_Referencia;
            txtDescripcion.Text = registro.fld_Descripcion;
            txtidSuplidor.Text = registro.fld_suplidor;
            txtPrecio.Text = Convert.ToString(registro.fld_Precio).ToString();
            txtexistencia.Text = Convert.ToString(registro.fld_Existencia).ToString();
            
        }
        private void txtPrecio_KeyUp(object sender, KeyEventArgs e)
        {

            // Metodo para Validar Solo Numero
            comunes.libreria.soloNumero(txtPrecio.Text, e);
        }
        private void btnSuplidor_Click(object sender, RoutedEventArgs e)
        {
            vistas.SuplidorfrmBRW dlgfrm = new vistas.SuplidorfrmBRW();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {

                registro.fld_idSuplidor = dlgfrm.idSuplidor;
                registro.fld_suplidor = dlgfrm.nombre;
                   mostrar();   
            }
        }

       
    }
    }
#endregion
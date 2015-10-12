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
    /// Interaction logic for dptofrm.xaml
    /// </summary>
    public partial class dptofrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmDpto registro { get; set; }

        string idSegItem = "AD0102";

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
                        TxtIdDpto.IsEnabled = false;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        TxtIdDpto.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion


        #region Constructor y Loader
        //   Constructor del Fromulario
        public dptofrm()
        {
            // Cargar los permisos del departamento para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmDpto();
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

            if (registro.fld_idDpto == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion


        #region Funcionalidades de los Botones

        // Click del boton Nuevo
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidDpto = registro.fld_idDpto;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            TxtNombre.Focus();
        }

        // Click del Boton Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idDpto, true);
            mostrar();
            modalidad = "CONSULTAR";
            TxtNombre.Focus();
        }

        // Click Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidDpto = registro.fld_idDpto;
            modalidad = "MODIFICAR";
            TxtIdDpto.Focus();
        }

        // Click Boton Gurdar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_NombreDpto = TxtNombre.Text;

            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret) {
                    TxtIdDpto.Text = registro.fld_idDpto.ToString(); 
                }
               
            }

            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del Departamento fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Click Boton Borrar
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Departamento?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idDpto != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idDpto);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            TxtNombre.Focus();
        }


        // Click boton Salir
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        // Click Boton Buscar
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.DptoBRWfrm dlgfrm = new vistas.DptoBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idDpto, true))
                {
                    MessageBox.Show("Nombre de departamento no existe", "departamento", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    TxtNombre.Focus();
                }
            }
        }
        #endregion


        #region Validaciones


        private void TxtIdDpto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtIdDpto_LostFocus(sender, e);
            }
        }
        private void TxtIdDpto_LostFocus(object sender, RoutedEventArgs e)
        {
            int idDpto = 0;

            if (!Int32.TryParse(TxtIdDpto.Text, out idDpto))
            {
                idDpto = 0;
            }

            if (idDpto != registro.fld_idDpto)
            {
                registro.fld_idDpto = idDpto;
                bool found = registro.buscar(idDpto, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Departamento no existe", "departamento", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idDpto, true);

                    mostrar();
                    TxtIdDpto.Focus();
                }

            }


        }

        /// <summary>
        /// Muestra los departamentos Existentes y no permite agregarlos si ya existen. 
        /// Parte de la validacion.
        /// </summary>

        private void NameGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtNombre.Text.Equals(registro.fld_NombreDpto))
            {
                registro.fld_NombreDpto = TxtNombre.Text;

                bool found = registro.buscar(TxtNombre.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de departamento no existe", "departamento", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    TxtNombre.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("departamento ya existe.", "departamento", MessageBoxButton.OK, MessageBoxImage.Information);
                        TxtNombre.Text = "";
                        TxtNombre.Focus();
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
            TxtNombre.Text = registro.fld_NombreDpto;
            TxtIdDpto.Text = Convert.ToInt16(registro.fld_idDpto).ToString();
        }


        #endregion

       
    }
}





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
using System.Data.SqlClient;

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Actividafrm.xaml
    /// </summary>
    public partial class Actividafrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmActividad registro { get; set; }

        string idSegItem = "PA0107";

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
                        TxtidActividad.IsEnabled = false;
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        TxtidActividad.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }

        }
        #endregion



        #region Constructor y Loader
        //   Constructor del formulario 

        public Actividafrm()
        {
            // Cargar los permisos del Cargo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmActividad();
            registro.buscarUltimo();
            llenarcbEstado();

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

            if (registro.fld_idActividad == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion

        #region Funcionalidades de los Botones
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidActividad = registro.fld_idActividad;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            Txtcodigo.Focus();


        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idActividad, true);
            mostrar();
            modalidad = "CONSULTAR";
            Txtcodigo.Focus();


        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidActividad = registro.fld_idActividad;
            modalidad = "MODIFICAR";
            Txtcodigo.Focus();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_codigo = Txtcodigo.Text;
            registro.fld_Descripcion = txtDescripcion.Text;
            registro.fld_Precio = Convert.ToDouble(txtPrecio.Text);

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

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este esta Actividad ?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idActividad != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idActividad);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            Txtcodigo.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion



        #region Validaciones

        private void TxtidActividad_LostFocus(object sender, RoutedEventArgs e)
        {
            int idActi = 0;
            if (!Int32.TryParse(TxtidActividad.Text, out idActi))
            {
                idActi = 0;
            }
            if (idActi != registro.fld_idActividad)
            {
                registro.fld_idActividad = idActi;
                bool found = registro.buscar(idActi, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Actividad no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idActi, true);

                    mostrar();
                    TxtidActividad.Focus();
                }
            }

        }
        private void TxtidActividad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtidActividad_LostFocus(sender, e);
            }
        }
        private void Txtcodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Txtcodigo.Text.Equals(registro.fld_codigo))
            {
                registro.fld_codigo = Txtcodigo.Text;

                bool found = registro.buscar(Txtcodigo.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Cargo no existe", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    Txtcodigo.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Cargo ya existe.", "Cargo", MessageBoxButton.OK, MessageBoxImage.Information);
                        Txtcodigo.Text = "";
                        Txtcodigo.Focus();
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
            TxtidActividad.Text = Convert.ToInt32(registro.fld_idActividad).ToString();
            Txtcodigo.Text = registro.fld_codigo;
            txtDescripcion.Text = registro.fld_Descripcion;

            txtPrecio.Text = Convert.ToDouble(registro.fld_Precio).ToString();


            foreach (CBoxStrItem lobj in cbidMedida.Items)
                if ((string)lobj.Value == registro.fld_idMedida)
                {
                    cbidMedida.SelectedValue = lobj;
                    break;
                }
        }


        #endregion

        #region Llenando ComboB


        public void llenarcbEstado()
        {
            cbidMedida.Items.Clear();
            SqlDataReader dr =
            datamanager.ConsultaLeer("select  idMedida , Descripcion from Medidas order by Descripcion");
            string col1 = "";
            string col2 = "";
            while (dr.Read())
            {
                /*Esta es la descripción que mostrará el combobox.
                col2 es entero y es lo que se almacenará
                Estamos usando valores nulos, por eso en la 
                conversión a entero lanza una excepcion 
               la aprovechamos para asignar el valor nulo.*/
                col1 = dr["Descripcion"].ToString();
                col2 = dr["idMedida"].ToString();
                cbidMedida.Items.Add(new CBoxStrItem(col1, col2));

            }

            // Cuando se carga del load esta linea genera un error.
            // cbidCargo.SelectedIndex = 0;
            datamanager.ConexionCerrar();


        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbidMedida.SelectedValue != null)
            {
                string selectedValue = Convert.ToString(((CBoxStrItem)cbidMedida.SelectedItem).Value);
                registro.fld_idMedida = selectedValue;
            }
        }

    }
}


#endregion

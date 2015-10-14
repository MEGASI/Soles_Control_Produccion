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
    /// Interaction logic for LibroDfrm.xaml
    /// </summary>
    public partial class LibroDfrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmLibroDirecciones registro { get; set; }

        string idSegItem = "AD0101";

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
                        btnBorrar.IsEnabled = true;
                        btnSalir.IsEnabled = false;
                        btnbuscar.IsEnabled = false;

                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, true);
                        txtidLD.IsEnabled = false;
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtidLD.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion

        #region Constructor y Loader
        //   Constructor del Formulario
        public LibroDfrm()
        {
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmLibroDirecciones();
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
        //Boton Nuevo
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldiLD = registro.fld_idLD;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtCedRNC.Focus();

        }
        //Boton Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idLD, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtCedRNC.Focus();
        }
        // Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldiLD = registro.fld_idLD;
            modalidad = "MODIFICAR";
            txtidLD.Focus();
        }
        //Boton Guardar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Ced_Rnc = txtCedRNC.Text.Trim();
            registro.fld_Nombres = txtNombre.Text.Trim();
            registro.fld_Apellidos = txtApellido.Text.Trim();
            registro.fld_sueldo = Convert.ToDouble(txtsueldo.Text.Trim());
            registro.fld_escliente = checkCliente.IsEnabled;
            registro.fld_esEmpleado = checkEmpleado.IsEnabled;
            registro.fld_esProovedor = checkProveedor.IsEnabled;
            registro.fld_idCargo = Convert.ToInt32(cbidCargo.Text);
            registro.fld_idDpto = Convert.ToInt32(cbidDpto.Text);
            registro.fld_estado = cbestado.Text;



            // Validar los valores asignados.
            bool lret = registro.validar();
            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret)
                {
                    txtidLD.Text = registro.fld_idLD.ToString();
                }
            }
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del  fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Click Boton Borrar


        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Registro de LibroDirecciones?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idLD != 0 || this.modalidad == "CONSULTAR" && registro.fld_idCargo != 0 || this.modalidad == "CONSULTAR" && registro.fld_idDpto != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idLD);
                    lret = registro.borrarDatos(Convert.ToInt32(registro.fld_idCargo));
                    lret = registro.borrarDatos(Convert.ToInt32(registro.fld_idDpto));
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtCedRNC.Focus();

        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.LibroDBRWfrm dlgfrm = new vistas.LibroDBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idLD, true))
                {
                    MessageBox.Show(" Cedula o RNC no existe", "Libro Direcciones", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtCedRNC.Focus();
                }
            }
        }
        //Bnt Salir
        private void btnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void txtidLD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtidLD_LostFocus(sender, e);
            }
        }
        private void txtidLD_LostFocus(object sender, RoutedEventArgs e)
        {
            int idGrupo = 0;

            if (!Int32.TryParse(txtidLD.Text, out idGrupo))
            {
                idGrupo = 0;
            }
            if (idGrupo != registro.fld_idDpto)
            {
                registro.fld_idDpto = idGrupo;
                bool found = registro.buscar(idGrupo, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Libro no existe", "Grupo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idGrupo, true);

                    mostrar();
                    txtidLD.Focus();
                }
            }
        }
        private void txtCedRNC_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtCedRNC.Text.Equals(registro.fld_Ced_Rnc))
            {
                registro.fld_Ced_Rnc = txtCedRNC.Text;

                bool found = registro.buscar(txtCedRNC.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Registro no existe", "LibroDirecciones", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtCedRNC.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("  Registro ya existe.", "LibroDirecciones", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtCedRNC.Text = "";
                        txtCedRNC.Focus();
                    }
                }
            }
        }
        private void txtCedRNC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtCedRNC_LostFocus(sender, e);
            }
        }
        #endregion

        #region Metodo Mostrar

        private void mostrar()
        {
            txtCedRNC.Text = registro.fld_Ced_Rnc;
            txtidLD.Text = Convert.ToInt16(registro.fld_idLD).ToString();
            txtNombre.Text = registro.fld_Nombres;
            txtApellido.Text = registro.fld_Apellidos;
            checkCliente.IsEnabled = registro.fld_escliente;
            checkEmpleado.IsEnabled = registro.fld_esEmpleado;
            checkProveedor.IsEnabled = registro.fld_esProovedor;
            txtsueldo.Text = Convert.ToString(registro.fld_sueldo).ToString();


            foreach (CBoxNullItem lobj in cbidCargo.Items)
                if ((int?)lobj.Value == registro.fld_idCargo)
                {
                    cbidCargo.SelectedValue = lobj;
                    break;
                }


            foreach (CBoxNullItem lobj in cbidDpto.Items)
                if ((int?)lobj.Value == registro.fld_idDpto)
                {
                    cbidDpto.SelectedValue = lobj;
                    break;
                }
        }
        #endregion
        #region  Llenando Los Combobox

        //Combobox CbidCargo
        private void cbidCargo_Loaded(object sender, RoutedEventArgs e)
        {
            llenaCbidCargo();
        }
        private void llenaCbidCargo()
        {
            cbidCargo.Items.Clear();

            SqlDataReader dr =
            datamanager.ConsultaLeer("select  estado, idCargo from LibroDirecciones union  select'N/A' as Prueba,cast(null as int ) as idCargo ");
            string col1 = "";
            int? col2 = null;
            while (dr != null && dr.Read())
            {
                col1 = dr["idCargo"].ToString();

                // Estamos usando valores nulos, por eso en la 
                // conversión a entero lanza una excepcion 
                // la aprovechamos para asignar el valor nulo.
                try { col2 = (int?)dr["IdCargo"]; }
                catch (Exception) { col2 = null; }

                cbidCargo.Items.Add(new CBoxNullItem(col1, col2));
            }
            cbidCargo.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        private void cbidCargo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbidCargo.SelectedValue != null)
            {
                int? selectedValue = (int?)((CBoxNullItem)cbidCargo.SelectedItem).Value;
                registro.fld_idCargo = selectedValue;
            }
        }
        // combobox Estado
        private void cbestado_Loaded(object sender, RoutedEventArgs e)
        {
            cbestado.Items.Clear();
            SqlDataReader dr =
            datamanager.ConsultaLeer("select  estado from LDEstado union  select'N/A' as Prueba");
            string col1 = "";
            //int? col2 = null;
            while (dr != null && dr.Read())
            {
                col1 = dr["estado"].ToString();

                cbestado.Items.Add(col1);
            }
            cbestado.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        private void cbestado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbestado.SelectedValue != null)
            {
                int? selectedValue = (int?)((CBoxNullItem)cbidCargo.SelectedItem).Value;
                registro.fld_estado = Convert.ToString(selectedValue);
            }
        }
        //Combobox Departamento
        private void cbidDpto_Loaded(object sender, RoutedEventArgs e)
        {
            LlenaridDpto();
        }
        private void LlenaridDpto()
        {
            cbidDpto.Items.Clear();

            SqlDataReader dr =
            datamanager.ConsultaLeer("select  Nombres, idDpto from LibroDirecciones union  select'N/A' as Prueba,cast(null as int ) as idCargo ");
            string col1 = "";
            int? col2 = null;
            while (dr != null && dr.Read())
            {
                col1 = dr["idDpto"].ToString();

                // Estamos usando valores nulos, por eso en la 
                // conversión a entero lanza una excepcion 
                // la aprovechamos para asignar el valor nulo.
                try { col2 = (int?)dr["idDpto"]; }
                catch (Exception) { col2 = null; }

                cbidDpto.Items.Add(new CBoxNullItem(col1, col2));
            }
            cbidDpto.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        private void cbidDpto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbidDpto.SelectedValue != null)
            {
                int? selectedValue = (int?)((CBoxNullItem)cbidDpto.SelectedItem).Value;
                registro.fld_idDpto = selectedValue;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Registro de LibroDirecciones?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idLD != 0 || this.modalidad == "CONSULTAR" && registro.fld_idCargo != 0 || this.modalidad == "CONSULTAR" && registro.fld_idDpto != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idLD);
                    lret = registro.borrarDatos(Convert.ToInt32(registro.fld_idCargo));
                    lret = registro.borrarDatos(Convert.ToInt32(registro.fld_idDpto));
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtCedRNC.Focus();
        }
    }
}
#endregion
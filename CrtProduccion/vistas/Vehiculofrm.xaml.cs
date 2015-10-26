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
    /// Interaction logic for Vehiculofrm.xaml
    /// </summary>
    public partial class Vehiculofrm : Window
    {  
        
        
        
        
        #region Declaraciones de variables y Propiedades

        private entidades.dmVehiculo registro { get; set; }

        string idSegItem = "AT";

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
                        txtidVehiculo.IsEnabled = false;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtidVehiculo.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion

        #region Constructor y Loader
        //   Constructor del Formulario
        public Vehiculofrm()
        {
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmVehiculo();
            registro.buscarUltimo();
            llenarcbEstado();
            LlenarCombo();
            
            llenandocbMarca();
            LlenandocbColor();

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

            if (registro.fld_idVehiculo == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion


        #region Funcionalidades de los Botones

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidVehiculo = registro.fld_idVehiculo;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtFicha.Focus();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idVehiculo, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtFicha.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidVehiculo = registro.fld_idVehiculo;
            modalidad = "MODIFICAR";
            txtidVehiculo.Focus();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Ficha = txtFicha.Text;
            registro.fld_Descripcion = txtDescripcion.Text;
            registro.fld_modelo = txtmodelo.Text;
            registro.fld_placa = txtPlaca.Text;
            registro.fld_ano = Convert.ToInt32(txtaño.Text);
            registro.fld_chasis = txtchasis.Text;
            registro.fld_Llantas = Convert.ToString((txtidllantas.Text));
            registro.fld_FiltAceite = Convert.ToString(txtidAceite.Text);
            registro.fld_SegVence = Convert.ToDateTime(segVence.Text);
            registro.fld_ultMant = Convert.ToDateTime(ultmant.Text);
            registro.fld_Kilometraje = Convert.ToDouble(txtkilometraj.Text);
            

            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret)
                {
                    txtidVehiculo.Text = registro.fld_idVehiculo.ToString();
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
                MessageBox.Show(registro.errormsg,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
        bool lret = false;
        if (MessageBox.Show("Seguro que quieres eliminar este Registro?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {

            if (this.modalidad == "CONSULTAR" && registro.fld_idVehiculo != 0)
            {
                lret = registro.borrarDatos(registro.fld_idVehiculo);
            }

            if (lret)
            {
                MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                mostrar();
            }
        }
        txtFicha.Focus();
    }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnbuscarllan_Click(object sender, RoutedEventArgs e)
        {
            vistas.LlantasBRW dlgfrm = new vistas.LlantasBRW();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {

                registro.fld_idLllantas = dlgfrm.idLlantas;
                registro.fld_Llantas = dlgfrm.NombreL;
                mostrar();
            }
        }
        private void btnbuscarAceit_Click(object sender, RoutedEventArgs e)
        {
            vistas.LlantasBRW dlgfrm = new vistas.LlantasBRW();
            dlgfrm.ShowDialog();

            registro.fld_idParte = dlgfrm.idfiltro;
            registro.fld_FiltAceite = dlgfrm.FiltroN;
            mostrar();
        }
        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.VehiculoBRW dlgfrm = new vistas.VehiculoBRW();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idvehiculos, true))
                {
                    MessageBox.Show("Nombre de Vehiculo no existe", "Vehiculos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtFicha.Focus();
                }
            }
        }
        #endregion



        #region Validaciones

        private void txtidVehiculo_LostFocus(object sender, RoutedEventArgs e)
        {
            int idDpto = 0;

            if (!Int32.TryParse(txtidVehiculo.Text, out idDpto))
            {
                idDpto = 0;
            }

            if (idDpto != registro.fld_idVehiculo)
            {
                registro.fld_idVehiculo = idDpto;
                bool found = registro.buscar(idDpto, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Vehiculo no existe", "Vehiculo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idDpto, true);

                    mostrar();
                    txtidVehiculo.Focus();
                }

            }
        }
        private void txtidVehiculo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtidVehiculo_LostFocus(sender, e);
            }
        }

        private void txtFicha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtFicha.Text.Equals(registro.fld_Ficha))
            {
                registro.fld_Ficha = txtFicha.Text;

                bool found = registro.buscar(txtFicha.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Vehiculo no existe", "Vehiculo", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtFicha.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("i dVehiculo  ya existe.", "Vehiculo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtFicha.Text = "";
                        txtFicha.Focus();
                    }
                }
            }

        }
        private void txtFicha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtFicha_LostFocus(sender, e);
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
            txtidVehiculo.Text = Convert.ToInt32(registro.fld_idVehiculo).ToString();
            txtFicha.Text = registro.fld_Ficha;
            txtDescripcion.Text = registro.fld_Descripcion;
            txtmodelo.Text = registro.fld_modelo;
            txtPlaca.Text = registro.fld_placa;
            txtaño.Text = Convert.ToInt32(registro.fld_ano).ToString();
            txtchasis.Text = registro.fld_chasis;
            segVence.Text = Convert.ToString((registro.fld_SegVence.ToString()));
            ultmant.Text = Convert.ToString((registro.fld_ultMant.ToString()));
            txtkilometraj.Text = Convert.ToString(registro.fld_Kilometraje.ToString());
            txtidllantas.Text = Convert.ToString((registro.fld_Llantas).ToString());
            txtidAceite.Text = Convert.ToString(registro.fld_FiltAceite).ToString();



            // cb estado
            foreach (CBoxStrItem lobj in cbEstado.Items)
                if ((string)lobj.Value == registro.fld_idEstado)
                {
                    cbEstado.SelectedValue = lobj;
                    break;
                }
            //Fin

            // cb Marca
            foreach (CBoxNullItem lobj in cbMarca.Items)
                if ((int)lobj.Value == registro.fld_idMarca)
                {
                    cbMarca.SelectedValue = lobj;
                    break;
                }
            //fin

            //cb Color
            foreach (CBoxNullItem lobj in cbColor.Items)
                if ((int)lobj.Value == registro.fld_idColor)
                {
                    cbColor.SelectedValue = lobj;
                    break;
                }
            //fin
                // CbidTipoVehiculo
            foreach (CBoxNullItem lobj in cbidTipoVehiculo.Items)
                if ((int)lobj.Value == registro.fld_idTipoVehiculo)
                {
                    cbidTipoVehiculo.SelectedValue = lobj;
                    break;
                }
            //fin

            
        }

        #endregion



        #region Llenando CB

        // ComboBox Estado
        public void llenarcbEstado()
        {
            cbEstado.Items.Clear();
            SqlDataReader dr =
            datamanager.ConsultaLeer("select  idestado , Descripcion from Vehiculo_Estado order by Descripcion");
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
                col2 = dr["idestado"].ToString();
                cbEstado.Items.Add(new CBoxStrItem(col1, col2));

            }

            // Cuando se carga del load esta linea genera un error.
            // cbidCargo.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        private void cbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbEstado.SelectedValue != null)
            {
                string selectedValue = Convert.ToString(((CBoxStrItem)cbEstado.SelectedItem).Value);
                registro.fld_idEstado = selectedValue;
            }
        }
        // Cb Marca
        public void llenandocbMarca()
        {
         cbMarca.Items.Clear();
         SqlDataReader dr =
        datamanager.ConsultaLeer(" select  idMarca ,descripcion from Vehiculo_Marca order by descripcion");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {
                
                col1 = dr["Descripcion"].ToString();


                try { col2 = (int)dr["idMarca"]; }
                catch (Exception) { col2 = 0; }

                cbMarca.Items.Add(new CBoxNullItem(col1, col2));
            }

            datamanager.ConexionCerrar();

        }
        private void cbMarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMarca.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbMarca.SelectedItem).Value;
                registro.fld_idMarca = Convert.ToInt32(selectedValue);
            }
        }
        //CbtipoVehiculo
        public void LlenarCombo()
        {
            cbidTipoVehiculo.Items.Clear();
            SqlDataReader dr =
           datamanager.ConsultaLeer("select  idTipoVehiculo ,descripcion from Vehiculo_Tipo order by descripcion");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {
                
                col1 = dr["Descripcion"].ToString();


                try { col2 = (int)dr["idTipoVehiculo"]; }
                catch (Exception) { col2 = 0; }

                cbidTipoVehiculo.Items.Add(new CBoxNullItem(col1, col2));
            }
            
            datamanager.ConexionCerrar();
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbidTipoVehiculo.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbidTipoVehiculo.SelectedItem).Value;
                registro.fld_idTipoVehiculo = Convert.ToInt32(selectedValue);
            }

        }
        //  Cbcolor
        public void LlenandocbColor()
        {
            cbColor.Items.Clear();
            SqlDataReader dr =
           datamanager.ConsultaLeer("select  idColor ,descripcion from color order by descripcion");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {
                

                col1 = dr["Descripcion"].ToString();


                try { col2 = (int)dr["idColor"]; }
                catch (Exception) { col2 = 0; }

                cbColor.Items.Add(new CBoxNullItem(col1, col2));
            }
            datamanager.ConexionCerrar();
        }
        private void cbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbColor.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbColor.SelectedItem).Value;
                registro.fld_idColor = Convert.ToInt32(selectedValue);
            }
        }
        private void txtaño_KeyUp(object sender, KeyEventArgs e)
        {
            //Validando Solo numero
            comunes.libreria.soloNumero(txtaño.Text,e);
        }

       
    }
    
}
#endregion






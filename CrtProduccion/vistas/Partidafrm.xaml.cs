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
using System.Data;

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Partidafrm.xaml
    /// </summary>
    public partial class Partidafrm : Window
    {

        #region Declaraciones de variables y Propiedades

        private entidades.dmPartidas registro { get; set; }

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
                       Txtid .IsEnabled = false;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                       Txtid .IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }

        #endregion



        #region Constructor y Loader
        //   Constructor del Formulario
        public Partidafrm()
        {
            InitializeComponent();

            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmPartidas();
            registro.buscarUltimo();


            LlenanoPartidaTipCb();
            LlenandocbMedidas();


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

            if (registro.fld_idpartida == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }

        #endregion



        #region Funcionalidades de los Botones


        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidPartida = registro.fld_idpartida;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            TxtCodigo.Focus();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idpartida, true);
            mostrar();
            modalidad = "CONSULTAR";
            TxtCodigo.Focus();
        }
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidPartida = registro.fld_idpartida;
            modalidad = "MODIFICAR";
            Txtid.Focus();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_descripcion = txtDescripcion.Text;
            registro.fld_codigo = TxtCodigo.Text;
            registro.fld_Precio = Convert.ToDouble(txtPrecio.Text);

            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret)
                {
                    Txtid.Text = registro.fld_idpartida.ToString();
                }

            }
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del la  Partida fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
   
    private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar esta partida?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idpartida != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idpartida);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            TxtCodigo.Focus();
        }
    

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.PartidaBRWfrm dlgfrm = new vistas.PartidaBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idPartida, true))
                {
                    MessageBox.Show("Nombre de Partida no existe", "Partida", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    TxtCodigo.Focus();
                }
            }
        }
        #endregion



        #region Validaciones




        private void TxtidCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            int idDpto = 0;

            if (!Int32.TryParse(Txtid.Text, out idDpto))
            {
                idDpto = 0;
            }

            if (idDpto != registro.fld_idpartida)
            {
                registro.fld_idpartida = idDpto;
                bool found = registro.buscar(idDpto, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Partida no existe", "Partida", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idDpto, true);

                    mostrar();
                    Txtid.Focus();
                }
            }
        }
        private void TxtidCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtidCodigo_LostFocus(sender, e);
            }
        }
        private void TxtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtCodigo.Text.Equals(registro.fld_descripcion))
            {
                registro.fld_descripcion = TxtCodigo.Text;

                bool found = registro.buscar(TxtCodigo.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Partida no existe", "Partidas", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    TxtCodigo.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Partida ya existe.", "Partidas", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtDescripcion.Text = "";
                        TxtCodigo.Focus();
                    }
                }
            }
        }
       private void TxtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtCodigo_LostFocus(sender, e);
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

            Txtid.Text = Convert.ToString(registro.fld_idpartida);
            TxtCodigo.Text = registro.fld_codigo;
            txtDescripcion.Text = registro.fld_descripcion;
            txtPrecio.Text = Convert.ToString(registro.fld_Precio);


            //probando...
            foreach (CBoxNullItem lobj in cbTipoPartida.Items)
                if ((int?)lobj.Value == registro.fld_idPartidaTipo)
                {
                    cbTipoPartida.SelectedValue = lobj;
                    break;
                }
            foreach (CBoxStrItem lobj in cbMedida.Items)
                if ((string)lobj.Value == registro.fld_medida)
                {
                    cbMedida.SelectedValue = lobj;
                    break;
                }
        }

        #endregion




        #region Llenando CB


        public void LlenanoPartidaTipCb()
        {
            cbTipoPartida.Items.Clear();
            SqlDataReader dr =
              datamanager.ConsultaLeer("select idPartidaTipo, Descripcion from partida_tipo " +
                                       "union " +
                                       "select cast(null as int), 'N/A' as descripcion " +
                                       "order by descripcion");
            string col1 = "";
            int? col2 = null;
            while (dr != null && dr.Read())
            {

                col1 = dr["Descripcion"].ToString();


                try { col2 = (int?)dr["idPartidaTipo"]; }
                catch (Exception) { col2 = null; }

                cbTipoPartida.Items.Add(new CBoxNullItem(col1, col2));
            }

            datamanager.ConexionCerrar();

        }
        private void cbTipoPartida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTipoPartida.SelectedValue != null)
            {
                int? selectedValue = (int?)((CBoxNullItem)cbTipoPartida.SelectedItem).Value;
                registro.fld_idPartidaTipo = selectedValue;
            }
        }
        public void LlenandocbMedidas()
        {
            cbMedida.Items.Clear();
            SqlDataReader dr =
            datamanager.ConsultaLeer("select  idMedida, descripcion  from Medidas order by descripcion");
            string col1 = "";
            string col2 = "";
            while (dr.Read())
            {
              
                col1 = dr["Descripcion"].ToString();
                col2 = dr["idMedida"].ToString();
                cbMedida.Items.Add(new CBoxStrItem(col1, col2));

            }
            datamanager.ConexionCerrar();
        }
        private void cbMedida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMedida.SelectedValue != null)
            {
                string selectedValue = Convert.ToString(((CBoxStrItem)cbMedida.SelectedItem).Value);
                registro.fld_medida = selectedValue;
            }
        }

        private void txtPrecio_KeyUp(object sender, KeyEventArgs e)
        {
            comunes.libreria.soloNumero(txtPrecio.Text,e);
        }
    }
}
#endregion


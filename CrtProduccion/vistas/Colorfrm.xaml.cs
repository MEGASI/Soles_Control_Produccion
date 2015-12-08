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
using System.Drawing;




namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Colorfrm.xaml
    /// </summary>
    public partial class Colorfrm : Window
    {
        #region Declaraciones de variables y Propiedades

        private entidades.dmColor registro { get; set; }

        string idSegItem = "AT0101";

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
                        TxtidColor.IsEnabled = false;
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        TxtidColor.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }

        }
        #endregion



        #region Constructor y Loader
        //   Constructor del Formulario
        public Colorfrm()
        {
            // Cargar los permisos del Color para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");




            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmColor();
            registro.buscarUltimo();
           // txtDescripcion.Text = Convert.ToString(Colorpick.Foreground.GetType());

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

            if (registro.fld_idColor == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }


        #endregion


        #region Funcionalidades de los Botones
        //btnNuevo

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidColor = registro.fld_idColor;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtDescripcion.Focus();

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idColor, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtDescripcion.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)


        {

            registro.fld_oldidColor = registro.fld_idColor;
            modalidad = "MODIFICAR";
            TxtidColor.Focus();

        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Asignar los valores de los conroles del formulario a los campos.
                registro.fld_Descripcion = txtDescripcion.Text;
                registro.fld_ValorRGB = Convert.ToInt32(txtvalor.Text);

                // Validar los valores asignados.
                bool lret = registro.validar();
                if (lret && this.modalidad == "CREAR")
                    lret = registro.crearDatos() > 0;
                if (lret && this.modalidad == "MODIFICAR")
                    lret = registro.actualizarDatos();

                if (lret)
                {
                    modalidad = "CONSULTAR";
                    MessageBox.Show("Información del Color fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Campo valor RGB no puede contenter letras o  signos!@#$%"); }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Color del registro?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idColor != 0)
                {
                    lret = registro.borrarDatos(registro.fld_idColor);
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
            vistas.ColorBRWfrm dlgfrm = new vistas.ColorBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idColor, true))
                {
                    MessageBox.Show("Descripcion de Color no existe", "Color", MessageBoxButton.OK, MessageBoxImage.Information);
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


        private void TxtidColor_LostFocus(object sender, RoutedEventArgs e)
        {
            byte IdColor = 0;
            if (!byte.TryParse(TxtidColor.Text, out IdColor))
            {
                IdColor = 0;
            }
            if (IdColor != registro.fld_idColor)
            {
                registro.fld_idColor = IdColor;
                bool found = registro.buscar(IdColor, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Color no existe", "Color", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(IdColor, true);

                    mostrar();
                    TxtidColor.Focus();
                }
            }
        }

        private void TxtidColor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TxtidColor_LostFocus(sender, e);
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
                        MessageBox.Show("Descripcion de Color no existe", "Color", MessageBoxButton.OK, MessageBoxImage.Information);

                    mostrar();
                    txtDescripcion.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Color ya existe.", "Color", MessageBoxButton.OK, MessageBoxImage.Information);
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

       

        #region Valores Extraidos de la BD

        /// <summary>
        /// Muestra los valores que se traen desde la base de datos
        /// Asignando el campo equivalente de cada control en el formulario.
        /// </summary>
        private void mostrar()
        {
            txtDescripcion.Text = registro.fld_Descripcion;
            TxtidColor.Text = Convert.ToString((registro.fld_idColor).ToString());
            txtvalor.Text = Convert.ToString((registro.fld_ValorRGB).ToString());
        }
        private void txtvalor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            //if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)
            //    e.Handled = false;
            //else

            //    e.Handled = true;
        }
        private void txtDescripcion_TextChanged(object sender, TextChangedEventArgs e)
        {
            //txtDescripcion.Text = Convert.ToString(Colorpick.Foreground.GetType());
        } 
        private void txtvalor_TextChanged(object sender, TextChangedEventArgs e)
        {
            //System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#FFCC66");
            //System.Windows.Media.Color color = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString("#FFDFD991");



            //txtvalor.Text = Convert.ToString(Colorpick.set());
            //txtvalor.Text = Convert.ToString(Colorpick.SetValue.Triggers());            
        }
        private void Colorpick_Drop(object sender, DragEventArgs e)
        {

        }
        //public static DependencyProperty Color = DependencyProperty.Register("Name", typeof(String), typeof(String));
        //public String Name
        //{
        //    set { SetValue(Color, value); }
        //    get { return (String)GetValue(Color); }
           
    

        private void btnscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.Colordiag dlg = new vistas.Colordiag();
            dlg.ShowDialog();
        }
    }
    }
#endregion
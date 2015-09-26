using System;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupofrm.xaml
    /// </summary>
    public partial class segGrupofrm : Window
    {
        entidades.dmGrupo registro = new entidades.dmGrupo();
        string idSegItem = "HS0102";

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
                        btnBorrar_png.IsEnabled = false;
                        btnSalir.IsEnabled = false;
                        btnSalir_png.IsEnabled = false;
                        btnbuscar.IsEnabled = false;


                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;

                        btnCancelar.Visibility = Visibility.Visible;
                        btnGuardar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, true);
                        if (value == "MODIFICAR")
                        {
                            NameGroup.IsEnabled = false;
                        }

                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnBorrar_png.IsEnabled = btnBorrar.IsEnabled;

                        btnSalir.IsEnabled = true;
                        btnSalir_png.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        btnCancelar.Visibility = Visibility.Hidden;
                        btnGuardar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, false);
                        NameGroup.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }
        //   InitializeComponent();
        public segGrupofrm()
        {

            // Cargar los permisos del grupo para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();

            // Operaciones permitidas en este formulario.
            // Crear
            btnNuevo.IsEnabled = permiteCrear;
           //btnNuevo_png.IsEnabled = btnNuevo.IsEnabled;
            // Modificar
            btnModificar.IsEnabled = permiteModificar;
            btnModificar_png.IsEnabled = btnModificar.IsEnabled;
            // Borrar
            btnBorrar.IsEnabled = permiteBorrar;
            btnBorrar_png.IsEnabled = btnBorrar.IsEnabled;


            if (registro.Fld_idGrupo == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";

        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void mostrar()
        {
            NameGroup.Text = registro.Fld_NombreGrupo;
            ID_Group.Text = Convert.ToInt16(registro.Fld_idGrupo).ToString();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.Fld_idGrupo);
            mostrar();
            modalidad = "CONSULTAR";
            NameGroup.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.Fld_idGrupo = registro.Fld_idGrupo;
            modalidad = "MODIFICAR";



        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;

            if (this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
            }

            if (this.modalidad == "MODIFICAR")
            {
                lret = registro.actualizarDatos();
            }

            if (lret)
            {
                modalidad = "CONSULTAR";
                MessageBox.Show("Información del usuario fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            }
       
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este usuario?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.Fld_idGrupo != 0)
                {
                    lret = registro.borrarDatos(registro.Fld_idGrupo);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameGroup_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void NameGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NameGroup.Text.Equals(registro.Fld_NombreGrupo))
            {
                if (!registro.buscar(NameGroup.Text))
                    MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);

                mostrar();
                NameGroup.Focus();
            }
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            segGrupoBRWfrm dlgfrm = new segGrupoBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm._idGrupo))
                {
                    MessageBox.Show("Nombre de usuario no existe", "Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    NameGroup.Focus();
                }
            }
        }

    

    }
}




   
       
    

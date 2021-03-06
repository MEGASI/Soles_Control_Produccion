﻿using System;
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
    /// Interaction logic for Proyectofrm.xaml
    /// </summary>
    public partial class Proyectofrm : Window
    {
        #region Declaraciones de variables y Propiedades

        private entidades.dmproyecto registro { get; set; }

        string idSegItem = "AP0101";

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
                        txtidProyecto.IsEnabled = false;
                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;

                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);
                        txtidProyecto.IsEnabled = true;
                    }
                }
                _modalidad = value;
            }
        }
        #endregion


        #region Constructor y Loader
        //   Constructor del Fromulario
        public Proyectofrm()
        {
            // Cargar los permisos del Proyecto para este formulario.
            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmproyecto();
            registro.buscarUltimo();
            //registro.BuscarCRTL();


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

            if (registro.fld_idProyecto == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }
        #endregion


        #region Funcionalidades de los Botones

        // Click del boton Nuevo
        private void btnNuevo_Click_1(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidProyecto = registro.fld_idProyecto;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            txtNombre.Focus();
        }
        // Click del Boton Cancelar
        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_idProyecto, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtNombre.Focus();
        }
        // Click Boton Modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_oldidProyecto = registro.fld_idProyecto;
            modalidad = "MODIFICAR";
            txtidProyecto.Focus();
        }
        // Click Boton Gurdar
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Asignar los valores de los conroles del formulario a los campos.
            registro.fld_Descripcion = txtNombre.Text;
          
            // Validar los valores asignados.
            bool lret = registro.validar();
            if (lret && this.modalidad == "CREAR")
                lret = registro.crearDatos() > 0;
            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();

            if (lret)
            {
                llenaCbIdProyectoCTRL();
                modalidad = "CONSULTAR";
                // Busca y asigna.               
                registro.buscar(registro.fld_idProyecto, true);
                mostrar();

                MessageBox.Show("Información del Proyecto fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Click Boton Borrar
        private void btnBorrar_Click_(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar este Proyecto ?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_idProyecto != 0 || this.modalidad == "CONSULTAR" && registro.fld_idProyectoCRTL!= 0)
                {
                    lret = registro.borrarDatos(registro.fld_idProyecto);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                }
            }
            txtNombre.Focus();
        }
        // Click boton Salir
        private void btnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        ///  la funcion que realiza es buscar de la tabla Proyecto el nombre 
        /// </summary>
        // Click Boton Buscar
        private void btnbuscar_Click_1(object sender, RoutedEventArgs e)
        {
            vistas.proyectoBRWfrm dlgfrm = new vistas.proyectoBRWfrm();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idProyecto, true))
                {
                    MessageBox.Show("Nombre de Proyecto no existe", "Proyecto", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();
                    txtNombre.Focus();
                }
            }
        }
        #endregion



        #region Validaciones

     private void txtidProyecto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtidProyecto_LostFocus(sender, e);
            }
        }
        private void txtidProyecto_LostFocus(object sender, RoutedEventArgs e)
        {
            int idProyecto = 0;
            if (!Int32.TryParse(txtidProyecto.Text, out idProyecto))
            {
                idProyecto = 0;
            }
            if (idProyecto != registro.fld_idProyecto)
            {
                registro.fld_idProyecto = idProyecto;
                bool found = registro.buscar(idProyecto, false);
                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Id del Proyecto no existe", "Proyecto", MessageBoxButton.OK, MessageBoxImage.Information);
                    else registro.buscar(idProyecto, true);

                    mostrar();
                    txtidProyecto.Focus();
                }
            }
        }
        private void NameGroup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtNombre.Text.Equals(registro.fld_Descripcion))
            {
                registro.fld_Descripcion = txtNombre.Text;

                bool found = registro.buscar(txtNombre.Text, false);

                if (modalidad.Equals("CONSULTAR"))
                {
                    if (!found)
                        MessageBox.Show("Nombre de Proyecto no existe", "Proyecto", MessageBoxButton.OK, MessageBoxImage.Information);
            
                    mostrar();
                    txtNombre.Focus();
                }
                else
                {
                    if (found)
                    {
                        MessageBox.Show("Proyecto ya existe.", "Proyecto", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNombre.Text = "";
                        txtNombre.Focus();
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
            txtNombre.Text = registro.fld_Descripcion;
            txtidProyecto.Text = Convert.ToInt16(registro.fld_idProyecto).ToString();
            
            // Deberia de haber una forma mas facil de hacer esto
            // para poder ubicar el item al que corresponde un codigo de proyecto.
            foreach (CBoxNullItem lobj in CbidProyectoCTRL.Items)
              if ((int?)lobj.Value == registro.fld_idProyectoCRTL) {
                    CbidProyectoCTRL.SelectedValue = lobj;
                    break;
                }
          
            // hasta qui
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                NameGroup_LostFocus(sender, e);
            }
        }
        #endregion


        #region Llenando Cb


        private void CbidProyectoCTRL_Loaded(object sender, RoutedEventArgs e)
        {
            llenaCbIdProyectoCTRL();
        }
        private void llenaCbIdProyectoCTRL() {

            CbidProyectoCTRL.Items.Clear();
            SqlDataReader dr =
            datamanager.ConsultaLeer("select Descripcion, idProyecto from proyecto" +
                         " Union  " +
                         "Select 'N/A' as Descripcion, cast(null  as int) as idProyecto ");
            string col1 = "";
            int? col2 = null;
            while (dr != null && dr.Read())
            {
                col1 = dr["Descripcion"].ToString();

                // Estamos usando valores nulos, por eso en la 
                // conversión a entero lanza una excepcion 
                // la aprovechamos para asignar el valor nulo.
                try { col2 = (int?)dr["idProyecto"]; }
                catch (Exception) { col2 = null; }

                CbidProyectoCTRL.Items.Add(new CBoxNullItem(col1, col2));
            }
            CbidProyectoCTRL.SelectedIndex = 0;
            datamanager.ConexionCerrar();
        }
        private void CbidProyectoCTRL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbidProyectoCTRL.SelectedValue != null)
            {
                int? selectedValue = (int?)((CBoxNullItem)CbidProyectoCTRL.SelectedItem).Value;
                registro.fld_idProyectoCRTL = selectedValue;
            }
        }

    }
}
#endregion
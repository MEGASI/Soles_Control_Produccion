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
    /// Interaction logic for IDPfrm.xaml
    /// </summary>
    public partial class IDPfrm : Window
    {

        #region Declaraciones de variables y Propiedades

        public class dgBFila

        {
            public int id { get; set; }
            public int Secuencia { get; set; }
            public int idPartida { get; set; }
            public string Codigo { get; set; }
            public double Costo { get; set; }
            public double cantidad { get; set; }
            public string Noposte { get; set; }
            public double Precio { get; set; }
            public double efecto { get; set; }
            public string Descripcion { get; set; }
            public string Direccion { get; set; }

        }


        private entidades.dmIDP registro { get; set; }
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        string idSegItem = "PF0102";

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
                        btnAceptar.IsEnabled = true;
                        btnCanceB.IsEnabled = true;


                        btnNuevo.Visibility = Visibility.Hidden;
                        btnModificar.Visibility = Visibility.Hidden;

                        comunes.libreria.estadoControles(this, true);
                        //TxtidCodigo.IsEnabled = false;
                        //Activa.IsEnabled = true;


                    }

                    if (value == "CONSULTAR")
                    {
                        btnBorrar.IsEnabled = true && permiteBorrar;
                        btnSalir.IsEnabled = true;
                        btnbuscar.IsEnabled = true;
                        btnAceptar.IsEnabled = false;
                        btnCanceB.IsEnabled = false;


                        btnNuevo.Visibility = Visibility.Visible;
                        btnModificar.Visibility = Visibility.Visible;

                        comunes.libreria.estadoControles(this, false);


                    }
                }
                _modalidad = value;
            }
        }

        #endregion



        #region Constructor y Loader


        public IDPfrm()
        {

            permiteModificar = datamanager.probarPermiso(idSegItem, "modificar");
            permiteCrear = datamanager.probarPermiso(idSegItem, "crear");
            permiteBorrar = datamanager.probarPermiso(idSegItem, "borrar");


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            registro = new entidades.dmIDP();
            registro.buscarUltimo();
            LlenandoProyecto();
            LlenandoSuperVlocal();
            LlenandoSuperEdes();
            Creargrid();
            Txsecuencia.Text = "0";
            btnbuscarB.Visibility = Visibility.Hidden;
            
         


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

            if (registro.fld_id == 0 && permiteCrear)
                modalidad = "CREAR";
            else
                modalidad = "CONSULTAR";
        }

        #endregion


        #region Funciones y Botones


        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_olid = registro.fld_id;
            registro.fld_oldBrigada = registro.fld_idBrigada;
            registro.limpiar();
            mostrar();
            modalidad = "CREAR";
            Txtidp.Focus();
            registro.limpiar();
            txtFecha.Text = string.Empty;
      
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            registro.buscar(registro.fld_id, true);
            mostrar();
            modalidad = "CONSULTAR";
            txtFecha.Text = string.Empty;

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_olid = registro.fld_id;
            registro.fld_oldBrigada = registro.fld_idBrigada;
            modalidad = "MODIFICAR";

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            // Asignar los valores de los conroles del formulario a los campos.

            registro.fld_idp = Convert.ToInt32(Txtidp.Text);
            registro.fld_Fecha = Convert.ToDateTime(txtFecha.Text);
            registro.fld_circuito = TxtNoCircuito.Text.Trim();
            registro.fld_Observacion = txtObservación.Text.Trim();
            registro.fld_diaferiado = Convert.ToBoolean(Feriachk.IsEnabled);
            





            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos() > 0;
                if (lret)
                {
                    Txtid.Text = registro.fld_id.ToString();
                   
                }
            }

            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos();
            if (lret)
            {

                modalidad = "CONSULTAR";
                MessageBox.Show("Información del  Informe Diario de Producción fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);

               this.GuardarBrigada(registro.fld_idBrigada);
                this.GuardarGRID(registro.fld_id);

            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            bool lret = false;
            if (MessageBox.Show("Seguro que quieres eliminar esta partida?", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (this.modalidad == "CONSULTAR" && registro.fld_id != 0)
                {
                    lret = registro.borrarDatos(registro.fld_id);
                }

                if (lret)
                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                    mostrar();
                    registro.limpiar();
                }
            }

        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnbuscar_Click(object sender, RoutedEventArgs e)
        {
            vistas.IDPfrmBRW dlgfrm = new vistas.IDPfrmBRW();
            dlgfrm.ShowDialog();

            if (dlgfrm.DialogResult.HasValue && dlgfrm.DialogResult.Value)
            {
                // Si el Usuario presiona Aceptar
                if (!registro.buscar(dlgfrm.idIDP, true))
                {
                    MessageBox.Show(" IDP existe", "Informe Diario Producción", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    mostrar();

                }
            }
        }
        private void btnbuscarB_Click(object sender, RoutedEventArgs e)
        {


            vistas.partidasIDPBRW dlgfrm = new vistas.partidasIDPBRW();
            dlgfrm.ShowDialog();

            registro.fld_idPartida = dlgfrm.partida;
            registro.fld_codigo = dlgfrm.codigo;
            registro.fld_descripcion = dlgfrm.Descripcion;
            registro.fld_precio = dlgfrm.Precio;


            mostrar();


        }


        private void TxCantidad_KeyUp(object sender, KeyEventArgs e)
        {
            comunes.libreria.soloNumero(TxCantidad.Text, e);
        }

        private void TxCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Tab)
                {
                    double Result = Double.Parse(TxPreci.Text) * Double.Parse(TxCantidad.Text);

                    TxCost.Text = Result.ToString();
                }
            }
            catch { MessageBox.Show("Verifique  la Cantidad"); }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            txtFecha.Text = string.Empty;
            this.GuardarGRID(registro.fld_id);
        }

        private void TxCodIDP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                vistas.partidasIDPBRW dlgfrm = new vistas.partidasIDPBRW();
                dlgfrm.ShowDialog();

                registro.fld_idPartida = dlgfrm.partida;
                registro.fld_codigo = dlgfrm.codigo;
                registro.fld_descripcion = dlgfrm.Descripcion;
                registro.fld_precio = dlgfrm.Precio;
                MostrarPartidas();
                TxCantidad.Focus();

            }

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            registro.fld_id = Convert.ToInt32(Txtid.Text);
            registro.fld_secuencia = Convert.ToInt32(Txsecuencia.Text);
            registro.fld_idBrigada = Convert.ToInt32(TxtB2.Text);

            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos1() > 0;
                if (lret)
                {

                    TxtB2.Text = registro.fld_idBrigada.ToString();
                }
            }

            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos1();
            if (lret)
            {

                modalidad = "CONSULTAR";
                MessageBox.Show("Información del la  Partida fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.GuardarGRID(registro.fld_id);

            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void GuardarBrigada( int pidBrigada)
        {
            registro.fld_id = Convert.ToInt32(Txtid.Text);
            registro.fld_idBrigada = Convert.ToInt32(TxtB2.Text);
            registro.fld_secuencia = Convert.ToInt32(Txsecuencia.Text);


            // Validar los valores asignados.
            bool lret = registro.validar();


            if (lret && this.modalidad == "CREAR")
            {
                lret = registro.crearDatos1() > 0;
                if (lret)
                {

                    TxtB2.Text = registro.fld_idBrigada.ToString();
                }
            }

            if (lret && this.modalidad == "MODIFICAR")
                lret = registro.actualizarDatos1();
            if (lret)
            {

                modalidad = "CONSULTAR";
                MessageBox.Show("Información del la  Partida fue almacenada.", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            else
                MessageBox.Show(registro.errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void btnbuscarBrigada_Click(object sender, RoutedEventArgs e)
        {
            vistas.brigadaBRW dlgfrm = new vistas.brigadaBRW();
            dlgfrm.ShowDialog();
            registro.fld_idB2 = dlgfrm.idBrigada;
            MostrarBrigada();

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            this.GuardarBrigada(registro.fld_idBrigada);
        }

        #endregion

        #region Metodo Mostrar


        /// <summary>
        /// Muestra los valores que se traen desde la base de datos.
        /// Asignando el campo equivalente de cada control en el formulario.
        /// </summary>
        private void mostrar()
        {
            Txtid.Text = Convert.ToString(registro.fld_id);            
            Txtidp.Text = Convert.ToString(registro.fld_idp);
            txtFecha.Text = Convert.ToDateTime(registro.fld_Fecha).ToString();
            TxtNoCircuito.Text = registro.fld_circuito;
            txtObservación.Text = registro.fld_Observacion;
            TxidPartida.Text = Convert.ToString(registro.fld_idPartida);
            TxCodIDP.Text = Convert.ToString(registro.fld_codigo);
            TxDesc.Text = registro.fld_descripcion;
            TxPreci.Text = registro.fld_precio;
            TxtB2.Text = Convert.ToString(registro.fld_idBrigada);


            foreach (CBoxNullItem lobj in cbProyecto.Items)
                if ((int)lobj.Value == registro.fld_idProyecto)
                {
                    cbProyecto.SelectedValue = lobj;
                    break;
                }
            foreach (CBoxNullItem lobj in cbSuplcal.Items)
                if ((int)lobj.Value == registro.fld_idSupervisorLocal)
                {
                    cbSuplcal.SelectedValue = lobj;
                    break;
                }

            foreach (CBoxNullItem lobj in cbSupEde.Items)
                if ((int)lobj.Value == registro.fld_idSupervisorEdeste)
                {
                    cbSupEde.SelectedValue = lobj;
                    break;
                }



            
        }
        #endregion

        #region MostrarPardidas

        public void MostrarPartidas()
        {

            TxidPartida.Text = Convert.ToString(registro.fld_idPartida);
            TxCodIDP.Text = Convert.ToString(registro.fld_codigo);
            TxDesc.Text = registro.fld_descripcion;
            TxPreci.Text = registro.fld_precio;

        }

        #endregion


        #region  MostrarBrigada

         public void MostrarBrigada()
        {

          TxtB2.Text = Convert.ToString(registro.fld_idB2);
            

        }

        #endregion

        #region Llenando CB


        public void LlenandoProyecto()
        {
            cbProyecto.Items.Clear();
            SqlDataReader dr =
              datamanager.ConsultaLeer(" Select idProyecto, Descripcion from Proyecto order by Descripcion");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {

                col1 = dr["Descripcion"].ToString();


                try { col2 = (int)dr["idProyecto"]; }
                catch (Exception) { col2 = 0; }

                cbProyecto.Items.Add(new CBoxNullItem(col1, col2));
            }

            datamanager.ConexionCerrar();

        }

        private void cbProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProyecto.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbProyecto.SelectedItem).Value;
                registro.fld_idProyecto = selectedValue;
            }
        }

        public void LlenandoSuperVlocal()
        {
            cbSuplcal.Items.Clear();
            SqlDataReader dr =
              datamanager.ConsultaLeer("select idLD , nombres   from  vLDC where idcargo = 6 ");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {

                col1 = dr["nombres"].ToString();


                try { col2 = (int)dr["idLD"]; }
                catch (Exception) { col2 = 0; }

                cbSuplcal.Items.Add(new CBoxNullItem(col1, col2));



            }
            datamanager.ConexionCerrar();
        }

        private void cbSuplcal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSuplcal.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbSuplcal.SelectedItem).Value;
                registro.fld_idSupervisorLocal = selectedValue;
            }
        }
        public void LlenandoSuperEdes()
        {
            cbSupEde.Items.Clear();
            SqlDataReader dr =
              datamanager.ConsultaLeer(" Select idLD , nombres   from  vLDC where idcargo = 7");
            string col1 = "";
            int col2 = 0;
            while (dr != null && dr.Read())
            {

                col1 = dr["nombres"].ToString();


                try { col2 = (int)dr["idLD"]; }
                catch (Exception) { col2 = 0; }

                cbSupEde.Items.Add(new CBoxNullItem(col1, col2));



            }
            datamanager.ConexionCerrar();
        }

        private void cbSupEde_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSupEde.SelectedValue != null)
            {
                int selectedValue = (int)((CBoxNullItem)cbSupEde.SelectedItem).Value;
                registro.fld_idSupervisorEdeste = selectedValue;
            }

        }

        #endregion


        #region  Metodo  GuardarGrid


        public void GuardarGRID(int pid)
        {
            // Recorriendo el DgGrid y  Guardando y 
            if (datamanager.ConexionAbrir())

            {
                SqlCommand Cmd1 = new SqlCommand();

                // Conectar a SQL y preparar la ejecucion del procedimiento SQL
                Cmd1.Connection = datamanager.ConexionSQL;
                Cmd1.CommandText = "dbo.IDP_DC";
                Cmd1.CommandType = CommandType.StoredProcedure;

                // Creando los parametros en el mismo orden que estan en el procedure
                Cmd1.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                Cmd1.Parameters.Add("@secuencia", SqlDbType.Int).Value = 0;
                Cmd1.Parameters.Add("@idPartida", SqlDbType.Int).Value = 0;
                Cmd1.Parameters.Add("@cantidad", SqlDbType.Money).Value = 0;
                Cmd1.Parameters.Add("@direccion", SqlDbType.VarChar).Value = "";
                Cmd1.Parameters.Add("@numPoste", SqlDbType.VarChar).Value = "";
                Cmd1.Parameters.Add("@precio", SqlDbType.Money).Value = 0;
                Cmd1.Parameters.Add("@efecto", SqlDbType.Money).Value = 0;



                // Recorrer el DataGrid por completo
                for (int i = 0; i < DG_IDP.Items.Count + 1; i++)
                {

                    // Extrarer valor de cada fila (celda) del datagrid y ponerlo en una variable

                    try
                    {

                        int id = pid;
                        int secuencia = (DG_IDP.Items[i] as dgBFila).Secuencia;
                        int idPartida = (DG_IDP.Items[i] as dgBFila).idPartida;
                        double cantidad = (DG_IDP.Items[i] as dgBFila).cantidad;
                        string Direccion = (DG_IDP.Items[i] as dgBFila).Direccion;
                        string Noposte = (DG_IDP.Items[i] as dgBFila).Noposte;
                        double Precio = (DG_IDP.Items[i] as dgBFila).Precio;
                        double efecto = (DG_IDP.Items[i] as dgBFila).efecto;



                        Cmd1.Parameters["@id"].Value = Convert.ToInt32(id);
                        Cmd1.Parameters["@idPartida"].Value = Convert.ToInt32(idPartida);
                        Cmd1.Parameters["@secuencia"].Value = Convert.ToInt32(secuencia);
                        Cmd1.Parameters["@cantidad"].Value = Convert.ToDouble(cantidad);
                        Cmd1.Parameters["@direccion"].Value = (Direccion);
                        Cmd1.Parameters["@numPoste"].Value = (Noposte);
                        Cmd1.Parameters["@Precio"].Value = Convert.ToDouble(Precio);
                        Cmd1.Parameters["@efecto"].Value = Convert.ToDouble(efecto);

                        Cmd1.ExecuteScalar();
                    }
                    catch (Exception)
                    { }
                    }
                datamanager.ConexionCerrar();
            }
        }
        #endregion



        #region Interactuando Con el Grid

        private void btnCanceB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var grid = DG_IDP;
                var mygrid = DG_IDP;
                if (grid.SelectedIndex >= 0)
                {
                    for (int i = 0; i <= grid.SelectedItems.Count; i++)
                    {
                        mygrid.Items.Remove(grid.SelectedItems[i]);
                    };

                    int Contar = int.Parse(Txsecuencia.Text);
                    int contar1 = Contar - 1;

                    Contar--;
                    Txsecuencia.Text = contar1.ToString();

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                int Contar = int.Parse(Txsecuencia.Text);
                int contar1 = Contar + 1;
                Contar++;
                Txsecuencia.Text = contar1.ToString();

                DG_IDP.Items.Add(new dgBFila()
                {
                    Codigo = TxCodIDP.Text,
                    Descripcion = TxDesc.Text,
                    Precio = Convert.ToDouble(TxPreci.Text),
                    cantidad = Convert.ToDouble(TxCantidad.Text),
                    Costo = Convert.ToDouble(TxCost.Text),
                    Direccion = TxDireccion.Text,
                    Noposte = TxNPost.Text,
                    id = Convert.ToInt32(Txtid.Text),
                    Secuencia = Convert.ToInt32(Txsecuencia.Text),
                    idPartida = Convert.ToInt32(TxidPartida.Text)


                });

                TxCodIDP.Text = "";
                TxDesc.Text = "";
                TxPreci.Text = "";
                TxCantidad.Text = "";
                TxCost.Text = "";
                TxDireccion.Text = "";
                TxNPost.Text = "";


            }
            catch (Exception)
            {
            }

        }

        #endregion


        #region CreandoGrid


        public void Creargrid()
        {

            DataGridTextColumn textColumn = new DataGridTextColumn();

            DataGridTextColumn c1 = new DataGridTextColumn();
          //  c1.Header = "Codigo";
            c1.Binding = new Binding("Codigo");
            //c1.Visibility = Visibility.Hidden;
            c1.Width = 73;
            DG_IDP.Columns.Add(c1);

            DataGridTextColumn c2 = new DataGridTextColumn();
         //   c2.Header = "Descripcion";
            c2.Binding = new Binding("Descripcion ");
            c2.Width = 270;

            DG_IDP.Columns.Add(c2);

            DataGridTextColumn c3 = new DataGridTextColumn();
          //  c3.Header = "Precio";
            c3.Binding = new Binding("Precio");
            c3.Width = 75;
            // c3.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c3);


            DataGridTextColumn c4 = new DataGridTextColumn();
          //  c4.Header = "Cantidad";
            c4.Binding = new Binding("cantidad");
            c4.Width = 75;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c4);



            DataGridTextColumn c5 = new DataGridTextColumn();
         //   c5.Header = "Costo";
            c5.Binding = new Binding("Costo");
            c5.Width = 76;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c5);

            DataGridTextColumn c6 = new DataGridTextColumn();
         //   c6.Header = "Direccion";
            c6.Binding = new Binding("Direccion");
            c6.Width = 185;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c6);


            DataGridTextColumn c7 = new DataGridTextColumn();
        //    c7.Header = "No. Poste";
            c7.Binding = new Binding("Noposte");
            c7.Width = 85;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c7);


            DataGridTextColumn c8 = new DataGridTextColumn();
         //   c8.Header = "secuencia";
            c8.Binding = new Binding("Secuencia");
            c8.Width = 70;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c8);


            DataGridTextColumn c9 = new DataGridTextColumn();
        //    c9.Header = "id";
            c9.Binding = new Binding("id");
            c9.Width = 70;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c9);

            DataGridTextColumn c10 = new DataGridTextColumn();
            //c10.Header = "idPartida";
            c10.Binding = new Binding("idPartida");
            c10.Width = 70;
            //c4.Visibility = Visibility.Hidden;
            DG_IDP.Columns.Add(c10);








        }


        #endregion



        #region Llenando dgBrigada

        public void llenaGrid()
        {
            dsGrid.Clear();
            dsGrid = datamanager.ConsultaDatos(" select  nombres from vbrigadaD order by idBrigada asc");



            DG_IDP.ItemsSource = dsGrid.Tables[0].DefaultView;

            ////dgBrigada.CanUserAddRows = false;
            ////dgBrigada.Columns[0].Width = 175;
            ////dgBrigada.Columns[0].IsReadOnly = true;
            ////dgBrigada.Columns[0].Header = "Codigo";
            ////dgBrigada.Columns[0].CanUserResize = false;


            ////dgBrigada.Columns[1].IsReadOnly = true;
            ////dgBrigada.Columns[1].Width = 200;
            ////dgBrigada.Columns[1].Header = "Nombres";
            ////dgBrigada.Columns[1].CanUserResize = false;


            ////dgBrigada.Columns[2].IsReadOnly = true;
            ////dgBrigada.Columns[2].Width = 125;
            ////dgBrigada.Columns[2].Header = "Secuencia";
            ////dgBrigada.Columns[2].CanUserResize = false;

            datamanager.ConexionCerrar();

        }

      
    }
}

#endregion

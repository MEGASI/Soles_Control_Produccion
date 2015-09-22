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

namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for FrmEditGroup.xaml
    /// </summary>
    public partial class FrmEditGroup : Window
    {


        public string Nombre { get; set; }

        SqlCommand Cmd = new SqlCommand();
        SqlConnection Cnn = new SqlConnection();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataSet dsGrid = new DataSet();
        SqlDataReader reader;

        public FrmEditGroup()
        {


            InitializeComponent();
        }


        //Cargar Tabla SegGrupo

        public void CargarForm() {
            
            //this.dataGrid.Columns.Clear();
            {
                /*reader = datamanager.ConsultaLeer("Select * from segGrupo");
                
                datamanager.cadenadeconexion="Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
                datamanager.ConexionAbrir();
                adapter.SelectCommand = Cmd;
                adapter.Fill(ds);
                datamanager.ConexionCerrar*/
                

                reader = datamanager.ConsultaLeer("Select * from segGrupo");
                Cnn.ConnectionString = "Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
                Cmd.Connection = Cnn;
                adapter.SelectCommand = Cmd;
                adapter.Fill(ds);

                Cnn.Close();

            }
        }
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.CargarForm();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Actualizar Grupos
        private void button_Click_2(object sender, RoutedEventArgs e)
        { try
            {
                Group G = new Group();
                G.ID_Group = Convert.ToInt16(ID_Group.Text.Trim());
                G.NombreG = NameGroup.Text.Trim();

                int resultado = IGrupo.Modificar(G);
                if (resultado > 0)
                {
                    MessageBox.Show("Datos Guardados Correctamente", "Guardados", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Error al Guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch { MessageBox.Show("ID Incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }

        }


        private void a(object sender, RoutedEventArgs e)
        {
            try
            {
                Group IG = new Group();
                IG.ID_Group = Convert.ToInt16(ID_Group.Text.Trim());
                IG.NombreG = NameGroup.Text.Trim();



                int resultado = IGrupo.Eliminar(IG);

                if (resultado > 0)

                {
                    MessageBox.Show("Datos Elimnados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error al Eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                }

            }
            catch { MessageBox.Show("Error al Eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
        }


        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            checkBox.IsChecked = true;
            if (checkBox.IsEnabled == true)
            {

            }
            this.CargarForm();
        }
        // Evento Key.Enter
        private void ID_Group_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                this.findcodigo(ID_Group.Text);
            }

        }

        private bool findcodigo(string pcodigo) {
            bool found = false;

            try
            {
                var reader = datamanager.ConsultaLeer("select  Nombre  from segGrupo where  idGrupo = " + pcodigo);
                found = reader.HasRows;

                if (reader.Read())
                {
                    NameGroup.Text = reader.GetString(0);
                    found = true;
                }
                else
                {
                    NameGroup.Text = "";
                    MessageBox.Show("Código de  grupo no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch {

                MessageBox.Show("Código de  grupo Incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return found;
        }

        private void CerrarBtn(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CargarFormBtn(object sender, RoutedEventArgs e)
        {
            FrmViewEG f = new FrmViewEG();
            if (f.ShowDialog() == DialogResult.HasValue)
            {
                this.findcodigo(f.ID.ToString());
                //string nombre = f.Nombre; //lee la propiedad
                // ID_Group.Text = nombre; //la pone en el título        
            }
        }


        private void button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        //Boton Eliminar Grupo
               private void eliminarBtn_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                Group G = new Group();
                G.ID_Group = Convert.ToInt16(ID_Group.Text.Trim());
                G.NombreG = NameGroup.Text.Trim();

                int resultado = IGrupo.Eliminar(G);
                if (resultado > 0)
                {
                    MessageBox.Show("Datos Eliminados Correctamente", "Eliminando", MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                {

                    MessageBox.Show("Error al Eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }


            }
            catch { MessageBox.Show("ID Incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }

        }
    }
}
   

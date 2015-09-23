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
using System.Windows.Forms;
namespace CrtProduccion
{
    /// <summary>
    /// Interaction logic for segGrupoBRWfrm.xaml
    /// </summary>
    public partial class segGrupoBRWfrm : Window
    {

        public string Nombre { get; set; }
        public int ID { get; set; }

        SqlCommand Cmd = new SqlCommand();
        SqlConnection Cnn = new SqlConnection();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataSet dsGrid = new DataSet();



        public segGrupoBRWfrm()
        {
            InitializeComponent();
        }


        //Cargar Form
        public void CargarForm()
        {

            {
                Cmd.CommandText = " Select * from segGrupo";
                Cnn.ConnectionString = "Data Source=Server;Initial Catalog=dbCtrlServicios;User ID=Tech;Password=Soles2015";
                Cmd.Connection = Cnn;
                adapter.SelectCommand = Cmd;
                adapter.Fill(ds);
                dataGrid.ItemsSource = ds.Tables[0].DefaultView;

                Cnn.Close();
            }
        }
        //this.Hide();
        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            

            DialogResult = DialogResult.HasValue;
            this.Close();
        }
        //this.Hide();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CargarForm();
        }



        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {




            }
        //Obteniendo valor de la grid   y añadiendolo al TextBox
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            object item = dataGrid.SelectedItem;
            object item1 = dataGrid.SelectedItem;

            string ID = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            textBox.Text = ID.ToString();

            //string ID1 = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            //textBox.Text = ID1.ToString();



        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Nombre = textBox.Text;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
    }

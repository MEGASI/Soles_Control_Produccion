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
using System.Data;
using System.Data.SqlClient;

namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for SuplidorfrmBRW.xaml
    /// </summary>
    public partial class SuplidorfrmBRW : Window
    {
        public int idSuplidor = 0;
        public string nombre = "";
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public SuplidorfrmBRW()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.seleccion();
        }

        private void seleccion() {

            
            this.DialogResult = true;
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.idSuplidor  = 0;
            this.DialogResult = false;
        }
        public void llenaGrid()
        {
            dsGrid.Clear();
            dsGrid = datamanager.ConsultaDatos(" Select idLD,Nombres  from LibroDirecciones where esProveedor  = 'true'");

            

            DataG.ItemsSource = dsGrid.Tables[0].DefaultView;

            DataG.CanUserAddRows = false;
            DataG.Columns[0].Width = 50;
            DataG.Columns[0].IsReadOnly = true;
            DataG.Columns[0].Header = "Codigo";
            DataG.Columns[0].CanUserResize = false;


            DataG.Columns[1].IsReadOnly = true;
            DataG.Columns[1].Width = 175;
            DataG.Columns[1].Header = "Suplidor";
            DataG.Columns[1].CanUserResize = false;

            datamanager.ConexionCerrar();
        }
        private void DataG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            object item1 = DataG.SelectedItem;

            string sidSuplidor = (DataG.SelectedCells[0].Column.GetCellContent(item1) as TextBlock).Text;
            nombre = (DataG.SelectedCells[1].Column.GetCellContent(item1) as TextBlock).Text;

        
         
             if (!Int32.TryParse(sidSuplidor, out idSuplidor))
            {
                idSuplidor = 0;
            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
            
        }
        private void DataG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                this.seleccion();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }

        private void txtCampo_TextChanged(object sender, TextChangedEventArgs e)
        {
            dsGrid.Clear();
            if (cbFiltro.Text == "Codigo")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" Select vp.idSuplidor, LD.Nombres as Suplidor from " +
                                                            " Vehiculo_Partes vp" +
                                                            " inner join LibroDirecciones LD  on vp.idSuplidor = Ld.idLD"+
                                                            " where vp.idSuplidor Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;

            }
            else if (cbFiltro.Text == "NombreS")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" Select vp.idSuplidor, LD.Nombres as Suplidor from " +
                                                            " Vehiculo_Partes vp" +
                                                            " inner join LibroDirecciones LD  on vp.idSuplidor = Ld.idLD" +
                                                            " where LD.Nombres Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataG.ItemsSource = dt.DefaultView;
            }
        }
    }
}

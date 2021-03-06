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
    /// Interaction logic for segUsuarioBRWfrm.xaml
    /// </summary>
    /// 

    #region Metodos Funciones y  Botones 

    public partial class segUsuarioBRWfrm : Window
    {
        public int idUsuario = 0;
        System.Data.DataSet dsGrid = new System.Data.DataSet();

        public segUsuarioBRWfrm()
        {
            InitializeComponent();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.idUsuario = 0;
            this.DialogResult = false;
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


        #endregion


        #region LlenandoGrid


        public void llenaGrid()
        {

            dsGrid.Clear();

            dsGrid = datamanager.ConsultaDatos("select nombre, idUsuario from segUsuario");

            dataGrid.ItemsSource = dsGrid.Tables[0].DefaultView;

            dataGrid.CanUserAddRows = false;
            dataGrid.Columns[0].Width = 152;
            dataGrid.Columns[0].IsReadOnly = true;
            dataGrid.Columns[0].Header = "Usuario";
            dataGrid.Columns[0].CanUserResize = false;

            dataGrid.Columns[1].IsReadOnly = true;
            dataGrid.Columns[1].Width = 48;
            dataGrid.Columns[1].Header = "Número";
            dataGrid.Columns[1].CanUserResize = false;

            datamanager.ConexionCerrar();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenaGrid();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dataGrid.SelectedItem;
            object item1 = dataGrid.SelectedItem;

            string sidUsuario = (dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;


            if (!Int32.TryParse(sidUsuario, out idUsuario))
            {
                idUsuario = 0;
            }
            else
            {
                btnAceptar.IsEnabled = true;
                btnAceptar_png.IsEnabled = true;
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                this.DialogResult = true;
            }
        }
        #endregion


        #region  LlenandoCB


        private void txtCampo_TextChanged(object sender, TextChangedEventArgs e)
        {
            dsGrid.Clear();
            if (cbFiltro.Text == "Codigo")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" select idUsuario,Nombre from SegUsuario  where idUsuario Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;

            }
            else if (cbFiltro.Text == "Usuario")
            {
                SqlDataAdapter adapter = new SqlDataAdapter(" select idUsuario,Nombre from SegUsuario  where Nombre Like '" + txtCampo.Text + "%'", datamanager.cadenadeconexion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;

            }

        }
    }
}

#endregion
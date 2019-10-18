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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data; 

namespace WPFDBParte2
{
   
    public partial class MainWindow : Window
    {
        OleDbConnection con; 
        DataTable dt;   
        public MainWindow()
        {
            InitializeComponent();
            
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Bachillerato 8.mdb";
            MostrarDatos();
        }
     
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Progra";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void LimpiaTodo()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            cbArea.SelectedIndex = 0;
            txtCta.Text = "";
            txtEdad.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (cbArea.Text != "Selecciona una area")
                    {
                        cmd.CommandText = "insert into Progra(Id,Nombre,Area,Cta,Edad) " +
                            "Values(" + txtId.Text + ",'" + txtNombre.Text + "','" + cbArea.Text + "'," + txtCta.Text + ",'" + txtEdad.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("El alumno ha sido agregado");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Selecciona el area");
                    }
                }
                else
                {
                    cmd.CommandText = "update Progra set Nombre='" + txtNombre.Text + "',Area='" + cbArea.Text + "',Cta=" + txtCta.Text
                       + ",Edad='" + txtEdad.Text + "' where Id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Se ha actualizado correctamente");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Inserta el ID");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0]; txtId.Text = row["Id"].ToString(); txtCta.Text = row["Cta"].ToString(); txtNombre.Text = row["Nombre"].ToString(); cbArea.Text = row["Area"].ToString(); txtEdad.Text = row["Edad"].ToString(); txtId.IsEnabled = false; btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Selecciona a alguien");
            }

        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0]; OleDbCommand cmd = new OleDbCommand(); if (con.State != ConnectionState.Open) con.Open();

                cmd.Connection = con;
                cmd.CommandText = "delete from progra where Id=" + row["Id"].ToString(); cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("El alunmo ha sido eliminado");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Selecciona a alguien");
            }


        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
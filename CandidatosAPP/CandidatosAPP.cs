using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace CandidatosAPP
{
    public partial class CandidatosAPP : Form
    {
        public CandidatosAPP()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Verificar si base de datos existe
            if (!File.Exists("CandidatosAPPDB.sqlite"))
            {
                SQLiteConnection.CreateFile("CandidatosAPPDB.sqlite");
                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=CandidatosAPPDB.sqlite;Version=3;");
                dbConnection.Open();
                //Creación de tabla Candidatos
                string sqlCrearTabla = "CREATE TABLE Candidatos (candidatoID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "cedula TEXT NOT NULL UNIQUE, nombre TEXT NOT NULL, apellido TEXT NOT NULL, " +
                    "fechaNacimiento TEXT NOT NULL, trabajoActual TEXT, expectativaSalarial INT, observaciones TEXT)";

                SQLiteCommand queryCrearTabla = new SQLiteCommand(sqlCrearTabla, dbConnection);
                queryCrearTabla.ExecuteNonQuery();
            }
            
            Recargar();
        }

        //Método para alimentar DataGridView con los datos de la tabla Candidatos
        private void Recargar()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=CandidatosAPPDB.sqlite;Version=3;");
            con.Open();

            string query = "SELECT * FROM Candidatos";
            SQLiteCommand cmd = new SQLiteCommand(query, con);


            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Cedula";
            dataGridView1.Columns[2].HeaderText = "Nombre";
            dataGridView1.Columns[3].HeaderText = "Apellido";
            dataGridView1.Columns[4].HeaderText = "Fecha de Nacimiento";
            dataGridView1.Columns[5].HeaderText = "Trabajo Actual";
            dataGridView1.Columns[6].HeaderText = "Expectativa Salarial";
            dataGridView1.Columns[7].HeaderText = "Observaciones";
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;            
        }
        
        //Mostrar formulario para Agregar Candidato    
        private void button2_Click(object sender, EventArgs e)
        {
            Formularios.AgregarFrm oAgregarFrm = new Formularios.AgregarFrm();
            oAgregarFrm.ShowDialog();

            Recargar();
        }

            
        //Formulario para Editar Candidato
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                //Identificar linea de fila seleccionada
                int indexLineaSeleccionada = dataGridView1.SelectedCells[0].RowIndex;
                string candidatoIDValue = dataGridView1.Rows[indexLineaSeleccionada].Cells[0].Value.ToString();

                //Seleccionar datos de Candidato Seleccionado y asignar datos a variables
                SQLiteConnection con = new SQLiteConnection("Data Source=CandidatosAPPDB.sqlite;Version=3;");
                con.Open();
                SQLiteCommand queryDelete = new SQLiteCommand(con);
                queryDelete.CommandText = string.Format("SELECT * from Candidatos WHERE candidatoID = {0}", candidatoIDValue);
                DataTable dt = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryDelete);
                adapter.Fill(dt);

                string cedula = dt.Rows[0].Field<string>("cedula");
                string nombre = dt.Rows[0].Field<string>("nombre");
                string apellido = dt.Rows[0].Field<string>("apellido");
                string fechaNacimiento = dt.Rows[0].Field<string>("fechaNacimiento");
                string trabajoActual = dt.Rows[0].Field<string>("trabajoActual");
                int expectativaSalarial = dt.Rows[0].Field<int>("expectativaSalarial");
                string observaciones = dt.Rows[0].Field<string>("observaciones");

                //Mostrar Formulario Editar Candidato y asignarle datos de variables
                Formularios.EditarFrm oEditarFrm = new Formularios.EditarFrm();
                oEditarFrm.cedula = cedula;
                oEditarFrm.nombre = nombre;
                oEditarFrm.apellido = apellido;
                oEditarFrm.fechaNacimiento = fechaNacimiento;
                oEditarFrm.trabajoActual = trabajoActual;
                oEditarFrm.expectativaSalarial = expectativaSalarial.ToString();
                oEditarFrm.observaciones = observaciones;
                oEditarFrm.candidatoIDValue = candidatoIDValue;
                oEditarFrm.ShowDialog();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("No existe registro para Editar");
            }                      
                                                 
                       
        }

        //Eliminar Candidato Seleccionado
        private void button4_Click(object sender, EventArgs e)
        {                       
            try
            {
                int indexLineaSeleccionada = dataGridView1.SelectedCells[0].RowIndex;

                string candidatoIDValue = dataGridView1.Rows[indexLineaSeleccionada].Cells[0].Value.ToString();

                SQLiteConnection con = new SQLiteConnection("Data Source=CandidatosAPPDB.sqlite;Version=3;");
                con.Open();


                SQLiteCommand queryDelete = new SQLiteCommand(con);
                queryDelete.CommandText = string.Format("DELETE FROM Candidatos WHERE candidatoID = {0}", candidatoIDValue);

                var confirmarDelete = MessageBox.Show("¿Está seguro de que desea eliminar este Candidato?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (confirmarDelete == DialogResult.Yes)
                {
                    queryDelete.ExecuteNonQuery();
                }                          
                Recargar();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("No existe registro para borrar");
            }

            
                                    
        }

        private void CandidatosAPP_Activated(object sender, EventArgs e)
        {
            Recargar();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}

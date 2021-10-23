using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandidatosAPP.Formularios
{
    public partial class EditarFrm : Form
    {
        public EditarFrm()
        {
            InitializeComponent();

            this.cedula = cedula;
            this.nombre = nombre;
            this.apellido = apellido;
            this.fechaNacimiento = fechaNacimiento;
            this.trabajoActual = trabajoActual;
            this.expectativaSalarial = expectativaSalarial;
            this.observaciones = observaciones;
            this.candidatoIDValue = candidatoIDValue;
        }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fechaNacimiento { get; set; }
        public string trabajoActual { get; set; }
        public string expectativaSalarial { get; set; }
        public string observaciones { get; set; }
        public string candidatoIDValue { get; set; }
        public string guardarCedula { get; set; }

        private void EditarFrm_Load(object sender, EventArgs e)
        {
            mskTxtCedula.Text = cedula;
            txtNombre.Text = nombre;
            txtApellido.Text = apellido;
            dtpFechaNacimiento.Text = fechaNacimiento;
            txtTrabajoActual.Text = trabajoActual;
            txtExpectativaSalarial.Text = expectativaSalarial;
            txtObservaciones.Text = observaciones;
            guardarCedula = mskTxtCedula.Text;
            dtpFechaNacimiento.MaxDate = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);
        }
        public static bool validarCedula(string sCedula)
        {
            int vnTotal = 0;
            int pLongCed = sCedula.Trim().Length;
            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };

            if (pLongCed < 11 || pLongCed > 11)
                return false;

            for (int vDig = 1; vDig <= pLongCed; vDig++)
            {
                int vCalculo = Int32.Parse(sCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }

            if (vnTotal % 10 == 0)
                return true;
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            SQLiteConnection con = new SQLiteConnection("Data Source=CandidatosAPPDB.sqlite;Version=3;");
            con.Open();

            if (String.IsNullOrEmpty(mskTxtCedula.Text) || String.IsNullOrEmpty(txtNombre.Text)
                || String.IsNullOrEmpty(txtApellido.Text))
            {
                System.Windows.Forms.MessageBox.Show("Por favor completar todos los campos requeridos");
            }
            else 
            {
                if (mskTxtCedula.Text == "00000000000")
                {
                    System.Windows.Forms.MessageBox.Show("La cedula ingresada no es válida.");
                    
                }
                else if(validarCedula(mskTxtCedula.Text))
                {
                    SQLiteCommand cmdVerificarCedula = new SQLiteCommand(con);
                    cmdVerificarCedula.CommandText = string.Format("SELECT count(*) FROM Candidatos WHERE cedula='{0}'", mskTxtCedula.Text);
                    int count = Convert.ToInt32(cmdVerificarCedula.ExecuteScalar());
                    if (mskTxtCedula.Text == guardarCedula)
                    {
                        string query = string.Format("UPDATE Candidatos SET " +
                            "nombre = '{0}', apellido = '{1}', fechaNacimiento = '{2}', " +
                            "trabajoActual = '{3}', expectativaSalarial = {4}, " +
                            "observaciones = '{5}' WHERE candidatoID = {6};", txtNombre.Text, txtApellido.Text,
                             dtpFechaNacimiento.Value.ToLongDateString(), txtTrabajoActual.Text, txtExpectativaSalarial.Text,
                            txtObservaciones.Text, candidatoIDValue);
                        SQLiteCommand cmd = new SQLiteCommand(query, con);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            this.Close();
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("No utilice caracteres especiales.\r\n" +
                                "La cedula no puede contener letras, caracteres especiales, o espacios.");
                        }
                    }
                    else if (count == 0)
                    {
                        string query = string.Format("UPDATE Candidatos SET cedula = '{0}'," +
                            "nombre = '{1}', apellido = '{2}', fechaNacimiento = '{3}', " +
                            "trabajoActual = '{4}', expectativaSalarial = {5}, " +
                            "observaciones = '{6}' WHERE candidatoID = {7};", mskTxtCedula.Text, txtNombre.Text, txtApellido.Text,
                             dtpFechaNacimiento.Value.ToShortDateString(), txtTrabajoActual.Text, txtExpectativaSalarial.Text,
                            txtObservaciones.Text, candidatoIDValue);
                        SQLiteCommand cmd = new SQLiteCommand(query, con);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            this.Close();
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("No utilice caracteres especiales.\r\n" +
                                "La cedula no puede contener letras, caracteres especiales, o espacios.");
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Ya existe esta cedula. No pueden ser iguales");
                    }
                   
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("La cedula ingresada no es válida.");
                }                          
                               
            }
        }
    }
}

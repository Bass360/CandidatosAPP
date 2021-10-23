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

namespace CandidatosAPP.Formularios
{
    public partial class AgregarFrm : Form
    {
        public AgregarFrm()
        {
            InitializeComponent();

            dtpFechaNacimiento.MaxDate = new DateTime(DateTime.Now.Year-18, DateTime.Now.Month, DateTime.Now.Day);
        }
        

        private void AgregarFrm_Load(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

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

            
            if (String.IsNullOrEmpty(mskTxtCedula.Text) || String.IsNullOrWhiteSpace(txtNombre.Text)
                || String.IsNullOrWhiteSpace(txtApellido.Text))
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
                    if (count == 0)
                    {
                        string query = string.Format("INSERT INTO Candidatos(cedula,nombre,apellido,fechaNacimiento,trabajoActual,expectativaSalarial,observaciones)" +
                                         "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", mskTxtCedula.Text, txtNombre.Text,
                                         txtApellido.Text, dtpFechaNacimiento.Value.ToLongDateString(), txtTrabajoActual.Text, txtExpectativaSalarial.Text, txtObservaciones.Text);
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

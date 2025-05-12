using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace TrustWell_Hospital_Pharmacy
{
    public partial class Medicine : Form
    {
        private int resid;
        private string patientName;

        public Medicine(int resID, string patientname)
        {
            InitializeComponent();
            this.resid = resID;
            this.patientName = patientname;
        }
        private void Medicine_Load(object sender, EventArgs e)
        {
            string query = "SELECT medicalprescription.medicalresID , Patients.PatientName , Doctors.DoctorName , medicalprescription.mediList FROM medicalprescription JOIN Patients ON medicalprescription.PatientID  JOIN Doctors ON medicalprescription.DoctorID = Doctors.DoctorID WHERE medicalresID = @resID ";
            MySqlParameter[] parameters = {
                new MySqlParameter("@resID", resid)
                };
            DataTable dt = Database.ExecuteQuery(query, parameters);

            if (dt.Rows.Count>0)
            {
                DataRow row = dt.Rows[0];

                richTextBox1.Text = row["mediList"].ToString();
                gunaLabel1.Text = row["PatientName"].ToString();
                gunaLabel2.Text = row["DoctorName"].ToString();


            }

        }

        private void gunaLabel1_Click(object sender, EventArgs e)
        {

        }

        private void gunaLabel3_Click(object sender, EventArgs e)
        {

        }

        private void gunaLabel4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Medicine delivered successfully.", "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Close();
        }
    }
}

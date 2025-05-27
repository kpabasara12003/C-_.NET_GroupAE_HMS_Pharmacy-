using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WindowsFormsApp1;

namespace TrustWell_Hospital_Pharmacy
{
    public partial class Home : Form
    {
        private DateTimeDisplay dateTimeDisplay;

        public Home()
        {
            InitializeComponent();
            dateTimeDisplay = new DateTimeDisplay(label1, label6); // label1 = time, label2 = date
            LoadData();
        }

        private void LoadData()
        {
            string query = @"
            SELECT 
                mp.medicalresID,
                p.PatientName,
                p.ContactNumber
            FROM 
                medicalprescription mp
            JOIN 
                Patients p ON mp.PatientID = p.PatientID
            WHERE 
                mp.Distribution = 'pending'";

            DataTable dt = Database.ExecuteQuery(query, null);
            gunaDataGridView1.DataSource = dt;

            if (!gunaDataGridView1.Columns.Contains("Medicine"))
            {
                DataGridViewButtonColumn medBtn = new DataGridViewButtonColumn
                {
                    HeaderText = "Medicine",
                    Text = "View",
                    Name = "Medicine",
                    UseColumnTextForButtonValue = true
                };
                gunaDataGridView1.Columns.Add(medBtn);
            }

            if (!gunaDataGridView1.Columns.Contains("DummyCheck"))
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Confirm",
                    Name = "DummyCheck"
                };
                gunaDataGridView1.Columns.Add(chk);
            }

            if (!gunaDataGridView1.Columns.Contains("Deliver"))
            {
                DataGridViewButtonColumn deliverBtn = new DataGridViewButtonColumn
                {
                    HeaderText = "Action",
                    Text = "Deliver",
                    Name = "Deliver",
                    UseColumnTextForButtonValue = true
                };
                gunaDataGridView1.Columns.Add(deliverBtn);
            }

            if (!gunaDataGridView1.Columns.Contains("Cancel"))
            {
                DataGridViewButtonColumn cancelBtn = new DataGridViewButtonColumn
                {
                    HeaderText = "Cancel",
                    Text = "Cancel",
                    Name = "Cancel",
                    UseColumnTextForButtonValue = true
                };
                gunaDataGridView1.Columns.Add(cancelBtn);
            }

            gunaDataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int resID = Convert.ToInt32(gunaDataGridView1.Rows[e.RowIndex].Cells["medicalresID"].Value);
            string patientName = gunaDataGridView1.Rows[e.RowIndex].Cells["PatientName"].Value.ToString();

            if (gunaDataGridView1.Columns[e.ColumnIndex].Name == "Medicine")
            {
                Medicine medForm = new Medicine(resID, patientName);
                medForm.StartPosition = FormStartPosition.CenterParent;
                medForm.ShowDialog();
            }
            else if (gunaDataGridView1.Columns[e.ColumnIndex].Name == "Deliver")
            {
                string query = @"UPDATE medicalprescription SET Distribution='delivered', StaffID=@staffid WHERE medicalresID=@ID";
                MySqlParameter[] param = {
                    new MySqlParameter("@ID", resID),
                    new MySqlParameter("@staffid", UserSession.StaffId)
                };
                Database.ExecuteNonQuery(query, param);
                MessageBox.Show("Marked as delivered.");
                LoadData();
            }
            else if (gunaDataGridView1.Columns[e.ColumnIndex].Name == "Cancel")
            {
                string query = @"UPDATE medicalprescription SET Distribution='canceled', StaffID=@staffid WHERE medicalresID=@ID";
                MySqlParameter[] param = {
                    new MySqlParameter("@ID", resID),
                    new MySqlParameter("@staffid", UserSession.StaffId)
                };
                Database.ExecuteNonQuery(query, param);
                MessageBox.Show("Delivery canceled.");
                LoadData();
            }
        }


        private void btnlogout_Click(object sender, EventArgs e)
        {
        }



        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void btnlogout_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
        }

        public class DateTimeDisplay
        {
            private Timer timer;
            private Label timeLabel;
            private Label dateLabel;

            public DateTimeDisplay(Label timeLabel, Label dateLabel)
            {
                this.timeLabel = timeLabel;
                this.dateLabel = dateLabel;

                timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                timeLabel.Text = DateTime.Now.ToString("hh:mm tt");
                dateLabel.Text = DateTime.Now.ToString("dd MMM yyyy");
            }
        }

        private void btnlogout_Click_1(object sender, EventArgs e)
        {

        }

        private void gunaGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cuiButton1_Click(object sender, EventArgs e)
        {

        }
    }
}

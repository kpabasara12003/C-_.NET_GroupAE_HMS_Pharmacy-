using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WindowsFormsApp1;

namespace TrustWell_Hospital_Pharmacy
{
    public partial class Home : Form
    {
        private DateTimeDisplay dateTimeDisplay;
        private bool columnsAdded = false;

        public Home()
        {
            InitializeComponent();
            dateTimeDisplay = new DateTimeDisplay(label1, label6); // label1 = time, label6 = date
        }

        private void Home_Load(object sender, EventArgs e)
        {
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

            // Hide the internal ID column
            if (gunaDataGridView1.Columns.Contains("medicalresID"))
                gunaDataGridView1.Columns["medicalresID"].Visible = false;

            // Update column headers for readability
            if (gunaDataGridView1.Columns.Contains("PatientName"))
                gunaDataGridView1.Columns["PatientName"].HeaderText = "Patient's Name";

            if (gunaDataGridView1.Columns.Contains("ContactNumber"))
                gunaDataGridView1.Columns["ContactNumber"].HeaderText = "Contact Number";

            // Prevent adding duplicate columns on every data load
            if (!columnsAdded)
            {
                gunaDataGridView1.Columns.Add(new DataGridViewButtonColumn
                {
                    HeaderText = "Medicine",
                    Text = "View",
                    Name = "Medicine",
                    UseColumnTextForButtonValue = true
                });

                gunaDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Confirm",
                    Name = "DummyCheck"
                });

                gunaDataGridView1.Columns.Add(new DataGridViewButtonColumn
                {
                    HeaderText = "Action",
                    Text = "Deliver",
                    Name = "Deliver",
                    UseColumnTextForButtonValue = true
                });

                gunaDataGridView1.Columns.Add(new DataGridViewButtonColumn
                {
                    HeaderText = "Cancel",
                    Text = "Cancel",
                    Name = "Cancel",
                    UseColumnTextForButtonValue = true
                });

                gunaDataGridView1.CellContentClick += dataGridView1_CellContentClick;
                columnsAdded = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = gunaDataGridView1.Rows[e.RowIndex];
           // int patientID = Convert.ToInt32(row.Cells["PatientID"].Value);
            int resID = Convert.ToInt32(row.Cells["medicalresID"].Value);
          //  string patientName = row.Cells["PatientName"].Value.ToString();

            string columnName = gunaDataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Medicine")
            {
                Medicine medForm = new Medicine(resID)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                medForm.ShowDialog();
            }
            else if (columnName == "Deliver")
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
            else if (columnName == "Cancel")
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

        private void cuiButton1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            LoadData();
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

                timer = new Timer
                {
                    Interval = 1000
                };
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                timeLabel.Text = DateTime.Now.ToString("hh:mm tt");
                dateLabel.Text = DateTime.Now.ToString("dd MMM yyyy");
            }
        }
    }
}

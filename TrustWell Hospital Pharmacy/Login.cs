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
using BCrypt.Net;
using WindowsFormsApp1;

namespace TrustWell_Hospital_Pharmacy
{
    public partial class Login: Form
    {
        public Login()
        {
            InitializeComponent();
          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       
        private void LogUserActivity(string staffId)
        {
            try
            {
                string activityQuery = "INSERT INTO login_activity (application, login_time, StaffID) VALUES ('Pharmacy', NOW(), @StaffID)";
                MySqlParameter[] parameters =
                {
                new MySqlParameter("@StaffID", staffId)
            };

                Database.ExecuteNonQuery(activityQuery, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to log: " + ex.Message, "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password.Length < 6 || password.Length > 9)
            {
                MessageBox.Show("Password must be between 6 and 9 characters long.", "Invalid Password Length", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@Email", email)
                };

                string query = "SELECT * FROM Users WHERE Role = 'Pharmacist' AND Email = @Email";
                DataTable result = Database.ExecuteQuery(query, parameters);

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow row = result.Rows[0];
                string storedPassword = row["Password"].ToString();
                int userId = Convert.ToInt32(row["Userid"]);
                string username = row["Username"].ToString();
                string role = row["Role"].ToString();
                string staffId = row["StaffID"].ToString();

                if (BCrypt.Net.BCrypt.Verify(password, storedPassword))
                {

                    UserSession.UserId = userId;
                    UserSession.Username = username;
                    UserSession.Role = role;
                    UserSession.StaffId = staffId;


                    LogUserActivity(staffId);


                    this.Hide();
                    Home dashboard = new Home();
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();

                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (NullReferenceException nullEx)
            {
                MessageBox.Show("Unexpected null value: " + nullEx.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (FormatException formatEx)
            {
                MessageBox.Show("Data formats are failed: " + formatEx.Message, "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

      
    }
}

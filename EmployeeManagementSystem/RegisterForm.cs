using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class RegisterForm : Form
    {
        // Connection string to the database
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\WINDOWS 10\Documents\employee.mdf;Integrated Security=True;Connect Timeout=30";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signup_loginBtn_Click(object sender, EventArgs e)
        {
            // Navigate to the login form
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // Form load event handler (if needed)
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            // Check for empty input fields
            if (string.IsNullOrWhiteSpace(signup_username.Text) || string.IsNullOrWhiteSpace(signup_password.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Open the SQL connection
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();

                    // Check if the username already exists
                    string selectUsername = "SELECT COUNT(id) FROM userss WHERE username = @user";

                    using (SqlCommand checkUser = new SqlCommand(selectUsername, connect))
                    {
                        checkUser.Parameters.AddWithValue("@user", signup_username.Text.Trim());
                        int count = (int)checkUser.ExecuteScalar();

                        if (count >= 1)
                        {
                            // Notify if the username is already taken
                            MessageBox.Show($"{signup_username.Text.Trim()} is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            // Insert the new user into the database
                            DateTime today = DateTime.Today;
                            string insertData = "INSERT INTO userss (username, password, date_register) VALUES (@username, @password, @dateReg)";

                            using (SqlCommand cmd = new SqlCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@username", signup_username.Text.Trim());
                                cmd.Parameters.AddWithValue("@password", signup_password.Text.Trim()); // Password should be hashed in real applications
                                cmd.Parameters.AddWithValue("@dateReg", today);

                                cmd.ExecuteNonQuery();

                                // Notify the user of successful registration
                                MessageBox.Show("Registered successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Navigate to the login form
                                Form1 loginForm = new Form1();
                                loginForm.Show();
                                this.Hide();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void signup_showPass_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility
            signup_password.PasswordChar = signup_showPass.Checked ? '\0' : '*';
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Click event for label3 (if necessary)
        }
    }
}

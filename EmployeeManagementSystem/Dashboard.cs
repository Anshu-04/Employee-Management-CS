using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class Dashboard : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\Documents\employee.mdf;Integrated Security=True;Connect Timeout=30");

        public Dashboard()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayTE();
            displayAE();
            displayIE();
        }

        private void displayTE()
        {
            try
            {
                int count = GetEmployeeCount("SELECT COUNT(id) FROM employees WHERE delete_date IS NULL", null);
                dashboard_TE.Text = count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayAE()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@status", "Active")
                };

                int count = GetEmployeeCount("SELECT COUNT(id) FROM employees WHERE status = @status AND delete_date IS NULL", parameters);
                dashboard_AE.Text = count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayIE()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@status", "Inactive")
                };

                int count = GetEmployeeCount("SELECT COUNT(id) FROM employees WHERE status = @status AND delete_date IS NULL", parameters);
                dashboard_IE.Text = count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetEmployeeCount(string query, SqlParameter[] parameters)
        {
            int count = 0;

            if (connect.State != ConnectionState.Open)
                connect.Open();

            using (SqlCommand cmd = new SqlCommand(query, connect))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                count = (int)cmd.ExecuteScalar();
            }

            connect.Close();
            return count;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Event handler for panel paint, if necessary
        }
    }
}

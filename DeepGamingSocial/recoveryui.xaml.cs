using DeepGamingSocial.registermodule;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for recoveryui.xaml
    /// </summary>
    public partial class recoveryui : Page
    {
        recoverybll recbll;
        public recoveryui()
        {
            InitializeComponent();
            recbll = new recoverybll();
        }

        private void Recoverybtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
            try
            {
                if (dc.State == ConnectionState.Closed)
                    dc.Open();
                string query = "Select COUNT(1) from account where username = @username AND recoveryques=@recoveryques AND recoverypass=@recoverypass";
                SqlCommand cmd = new SqlCommand(query, dc);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@username", Accountbox.Text);
                cmd.Parameters.AddWithValue("@recoveryques", RecQues.Text);
                cmd.Parameters.AddWithValue("@recoverypass", RecAnswer.Text);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    useraccount t = new useraccount();
                    t.username = Accountbox.Text;
                    t.password = NewPassWord.Password;
                    t.recoverypass = RecAnswer.Text;
                    t.recoveryques = RecQues.Text;
                    if (recbll.recoveryacc(t))
                        MessageBox.Show("Recovery success!");
                    else
                        MessageBox.Show("Can not recovery!");
                }
                else
                    MessageBox.Show("Wrong username or recover question, answer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }
        private void Loginbtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
            try
            {
                if (dc.State == ConnectionState.Closed)
                    dc.Open();
                string query = "Select COUNT(1) from account where username = @username AND passwords = @password";
                SqlCommand cmd = new SqlCommand(query, dc);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@username", AccountBox.Text);
                cmd.Parameters.AddWithValue("@password", PassBox.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    string path = "C:\\DeepGamingData\\user\\user.bat";
                    using var stream = new FileStream(path: path, FileMode.OpenOrCreate);
                    export(AccountBox.Text, stream);
                    stream.Close();
                    this.NavigationService.Navigate(new mainfeedui());                 
                }
                else
                    MessageBox.Show("Wrong password or username");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void export(string s, Stream stream)
        {
            var byte_text = Encoding.UTF8.GetBytes(s);
            var byte_leng = BitConverter.GetBytes(byte_text.Length);
            stream.Write(byte_leng, 0, 4);
            stream.Write(byte_text, 0, byte_text.Length);
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new registerui());
        }

        private void Recovery_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new recoveryui());
        }

    }
}

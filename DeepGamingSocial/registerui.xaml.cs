using DeepGamingSocial.registermodule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for registerui.xaml
    /// </summary>
    public partial class registerui : Page
    {
        registerbll rebll;
        public registerui()
        {
            InitializeComponent();
            rebll = new registerbll();
        }
        public bool checker()
        {
            if (string.IsNullOrEmpty(Accountbox.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Passwordbox.Password))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Recoveryquesbox.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Recoverypassbox.Text))
            {
                return false;
            }
            return true;
        }

        private void Registerbtn_Click(object sender, RoutedEventArgs e)
        {

            if (checker() && confirm.IsChecked == true)
            {
                useraccount t = new useraccount();
                t.username = Accountbox.Text;
                t.password = Passwordbox.Password;
                t.recoveryques = Recoveryquesbox.Text;
                t.recoverypass = Recoverypassbox.Text;
                if (rebll.registeraccount(t))
                {
                    MessageBox.Show("Register Sucess!");
                    SetProfile(Accountbox.Text, Accountbox.Text);
                    Setava(Accountbox.Text);
                }            
                else
                    MessageBox.Show("Error! Please try again.");
            }
            else
                MessageBox.Show("Please confirm your register");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }
        private void SetProfile(string username, string nickname)
        {
            SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True"); 
            try
            {
                if(dc.State == ConnectionState.Closed)
                    dc.Open();
                string query = "Insert into profiles values (@username, @nickname, @favgame, @ingame, @ava)";
                SqlCommand cmd = new SqlCommand(query, dc);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@nickname", nickname);
                cmd.Parameters.AddWithValue("@favgame", " ");
                cmd.Parameters.AddWithValue("@ingame", " ");
                cmd.Parameters.AddWithValue("@ava", username + "ava");
                cmd.ExecuteNonQuery();
                dc.Close();
            }catch(Exception ex) { 
                MessageBox.Show(ex.Message);
            }
            
        }
        private void Setava(string a)
        {
            string sourceDir = @"C:\DeepGamingData\Default\defaultava.png";
            string finalDir = $@"C:\DeepGamingData\avatar\{a}ava.png";
            try
            {
                File.Copy(sourceDir, finalDir); 
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for profileui.xaml
    /// </summary>
    public partial class profileui : Page
    {
        public profileui()
        {
            InitializeComponent();
            allinfor();
            yourstt.NavigationService.Navigate(new Blog());
        }
        SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        SqlCommand cmd;
        string path = "C:\\DeepGamingData\\user\\user.bat";
        private string username;
        private void allinfor()
        {
            getnickname();
            getingame();
            getvidcount();
            getblogcount();
            getava();
        }
        private void getava()
        {
            using var stream = new FileStream(path: path, FileMode.Open);
            string username = import(stream);
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            source.UriSource = new Uri($@"C:\DeepGamingData\avatar\{username}ava.png", UriKind.Absolute);
            source.EndInit();
            sttava.ImageSource = source;
        }
        private void getnickname()
        {
            string query = "select nickname from profiles where username = @username";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            if (dc.State == System.Data.ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(query,dc);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.CommandType = System.Data.CommandType.Text;
            string temp = Convert.ToString(cmd.ExecuteScalar());
            nickname.Text += temp;
            dc.Close();
            stream.Close();
        }
        private void getingame()
        {
            string query = "select ingame from profiles where username = @username";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            if (dc.State == System.Data.ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(query, dc);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.CommandType = System.Data.CommandType.Text;
            string temp = Convert.ToString(cmd.ExecuteScalar());
            ingame.Text += temp;
            dc.Close();
            stream.Close();
        }
        private void getblogcount()
        {
            string query = "select count(*) from userpost where username = @username";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            if (dc.State == System.Data.ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(query, dc);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.CommandType = System.Data.CommandType.Text;
            string temp = Convert.ToString(cmd.ExecuteScalar());
            ttblog.Text += temp;
            dc.Close();
            stream.Close();
        }
        private void getvidcount()
        {
            string query = "select count(*) from video where username = @username";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            if (dc.State == System.Data.ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(query, dc);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.CommandType = System.Data.CommandType.Text;
            string temp = Convert.ToString(cmd.ExecuteScalar());
            ttvid.Text += temp;
            dc.Close();
            stream.Close();
        }
        private void Blog_Click(object sender, RoutedEventArgs e)
        {
            yourstt.NavigationService.Navigate(new Blog());
        }

        private void Vid_Click(object sender, RoutedEventArgs e)
        {
            yourstt.NavigationService.Navigate(new Vid());
        }
        string import(Stream stream)
        {
            var byte_leng = new byte[4];
            stream.Read(byte_leng, 0, 4);
            int leng = BitConverter.ToInt32(byte_leng, 0);

            var byte_text = new byte[leng];
            stream.Read(byte_text, 0, leng);
            string text;
            text = Encoding.UTF8.GetString(byte_text);
            return text;
        }
    }
}

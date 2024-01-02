using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for Vid.xaml
    /// </summary>
    public partial class Vid : Page
    {
        public Vid()
        {
            InitializeComponent();
            string path = "C:\\DeepGamingData\\user\\user.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            loadvid();
        }
        string username;
        private int temp;
        private bool isplay = false;
        SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        SqlCommand cmd;
        private void countvid()
        {
            string query = "select count(*) from video where username = @username";
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd = new SqlCommand(query,connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.CommandType = CommandType.Text;
                this.temp = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadvid()
        {
            this.countvid();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                for (int index = 1; index <= this.temp; ++index)
                {
                    string query = "select videoname from video where id =@id AND username= @username";                 
                    cmd= new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("id", index);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.CommandType = CommandType.Text;
                    string path = "D:\\WPF\\DeepGamingSocial\\DeepGamingSocial\\vid\\" + Convert.ToString(cmd.ExecuteScalar()) + ".mp4";
                    this.addvid(path, username);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addvid(string s, string user)
        {
            MediaElement vid = new MediaElement();
            TextBlock username = new TextBlock();
            username.Text = "User:" + user;
            username.FontSize = 20.0;
            vid.Source = new Uri(s);
            vid.LoadedBehavior = MediaState.Manual;
            vid.Height = 480.0;
            vid.Width = 854.0;
            vid.MouseDown += new MouseButtonEventHandler(this.T_MouseDown);
            vid.Tag = (object)s;
            vid.Pause();
            this.vid.Children.Add(username);
            this.vid.Children.Add(vid);
            this.vid.Height += 490.0;
        }

        private void T_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isplay = !this.isplay;
            if (this.isplay)
                (sender as MediaElement).Play();
            else
                (sender as MediaElement).Pause();
        }
        private string import(Stream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            int int32 = BitConverter.ToInt32(buffer, 0);
            byte[] numArray = new byte[int32];
            stream.Read(numArray, 0, int32);
            return Encoding.UTF8.GetString(numArray);
        }
    }
}

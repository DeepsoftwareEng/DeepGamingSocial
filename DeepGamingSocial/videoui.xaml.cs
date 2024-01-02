using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DeepGamingSocial
{
    public partial class videoui : Page 
    {
        private int temp;
        private bool isplay = false;
        public OpenFileDialog upvid = new OpenFileDialog();


        public videoui()
        {
            this.InitializeComponent();
            this.loadvid();
        }
        private void Userbox_Click(object sender, RoutedEventArgs e) => this.chatbox.NavigationService.Navigate((object)new DeepGamingSocial.chatbox());
        private void countvid()
        {
            SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand sqlCommand = new SqlCommand("select count(*) from video", connection);
                sqlCommand.CommandType = CommandType.Text;
                this.temp = Convert.ToInt32(sqlCommand.ExecuteScalar());
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
            SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                for (int index = 1; index <= this.temp; ++index)
                {
                    string cmdText = "select videoname from video where id ='"+index+"'";
                    SqlCommand sqlCommand1 = new SqlCommand("select username from video", connection);
                    SqlCommand sqlCommand2 = new SqlCommand(cmdText, connection);
                    sqlCommand2.CommandType = CommandType.Text;
                    string user = Convert.ToString(sqlCommand1.ExecuteScalar());
                    string path = "D:\\WPF\\DeepGamingSocial\\DeepGamingSocial\\vid\\" + Convert.ToString(sqlCommand2.ExecuteScalar()) + ".mp4";
                    this.addvid(path, user);
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
            this.VideoFeed.Children.Add(username);
            this.VideoFeed.Children.Add(vid);
            this.VideoFeed.Height += 490.0;
        }

        private void T_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isplay = !this.isplay;
            if (this.isplay)
                (sender as MediaElement).Play();
            else
                (sender as MediaElement).Pause();
        }

        private void choose_Click(object sender, RoutedEventArgs e)
        {
            bool? nullable = this.upvid.ShowDialog();
            bool flag = true;
            if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
                return;
            Rename.Text = this.upvid.FileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
            try
            {
                string cmdText = "insert into video values(@username,@videoname)";
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (FileStream fileStream = new FileStream("C:\\DeepGamingData\\user\\user.bat", FileMode.Open))
                {
                    SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                    sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = (object)this.import((Stream)fileStream);
                    sqlCommand.Parameters.Add("@videoname", SqlDbType.NVarChar).Value = (object)(this.import((Stream)fileStream) + this.Rename.Text);
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    Directory.Move(this.upvid.FileName, "D:\\WPF\\DeepGamingSocial\\DeepGamingSocial\\vid\\" + this.import(fileStream) + this.Rename.Text + ".mp4");
                    this.Rename.Text = "";
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
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



using Azure.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for mainfeedui.xaml
    /// </summary>
    public partial class mainfeedui : Page
    {
        public mainfeedui()
        {
            InitializeComponent();
            setava();
            countfeed();
            loadfeed();
            countfriend();
            loadfriend();
        }
        private SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        SqlCommand cmd;
        int cmts;
        private int temp;
        public int id;
        private void setava()
        {
            string path = "C:\\DeepGamingData\\user\\user.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            string username = import(stream);
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            source.UriSource = new Uri($@"C:\DeepGamingData\avatar\{username}ava.png", UriKind.Absolute);
            source.EndInit();
            sttava.ImageSource= source;
            mainava.ImageSource= source;
        }

        private void countfeed()
        {
            try
            {
                if (dc.State == System.Data.ConnectionState.Closed)
                    dc.Open();
                string query = "select count(*) from userpost";
                SqlCommand count = new SqlCommand(query, dc);
                count.CommandType = System.Data.CommandType.Text;
                temp = Convert.ToInt32(count.ExecuteScalar());
                dc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("countfeed");
                MessageBox.Show(ex.Message);
            }
        }
        private void addfeed(string user, string content, int index)
        {
            string query = $"select likes from react where id ={index}";
            if (dc.State == ConnectionState.Closed)
                dc.Open();
            SqlCommand cmd = new SqlCommand(query, dc);
            cmd.CommandType = CommandType.Text;
            int likescount = Convert.ToInt32(cmd.ExecuteScalar());
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#E1E1E1");
            brush.Freeze();
            Brush brush1 = (Brush)bc.ConvertFrom("#676769");
            brush1.Freeze();
            TextBlock userbox = new TextBlock();
            TextBlock contentbox = new TextBlock();
            StackPanel react = new StackPanel();
            react.Height = 25;
            react.Orientation = Orientation.Horizontal;
            Button like= new Button();
            Button cmt = new Button();
            react.Children.Add(like);
            react.Children.Add(cmt);
            like.Content = "like "+likescount;
            cmt.Content = "Comment";
            like.Width = 600;
            cmt.Width= 600;
            like.Height = 20;
            cmt.Height= 20;
            like.FontSize = 12;
            like.Tag = index;
            cmt.Tag =  index;
            cmt.FontSize = 12;
            like.Click += like_click;
            cmt.Click += Cmt_Click;
            userbox.Text = "User: " + user;
            contentbox.Text = "          " + content;
            userbox.FontSize = 20;
            contentbox.FontSize = 20;
            userbox.Foreground = brush;
            userbox.FontWeight= FontWeights.Bold;
            contentbox.Foreground= brush;
            Line l = new Line();
            l.X1 = 0;
            l.X2 = 1100;
            l.Y1 = 0;
            l.Y2 = 0;
            l.Stroke = brush1;
            l.StrokeThickness = 4;
            newfeed.Children.Add(userbox);
            newfeed.Children.Add(contentbox);   
            newfeed.Children.Add(react);
            newfeed.Children.Add((Line)l);
        }
        private void Cmt_Click(object sender, RoutedEventArgs e)
        {         
            id = Convert.ToInt32((sender as Button).Tag.ToString());
            string path = "cmtid.bat";
            using var stream = new FileStream(path: path, FileMode.OpenOrCreate);
            export(stream, id);
            stream.Close();
            Comment.NavigationService.Navigate(new comment());
        }
       
        private void like_click(object sender, RoutedEventArgs e)
        {
            if(dc.State == ConnectionState.Closed)
                dc.Open();    
            string getid = $"select id from userpost where id = {(sender as Button).Tag}";
            string getlike = $"select likes from react where id = {(sender as Button).Tag}";
            string query = "Update React set likes=@likes where id=@id";
            SqlCommand cmd = new SqlCommand( getid, dc);
            SqlCommand cmd2 = new SqlCommand(query,dc);
            SqlCommand cmd3 = new SqlCommand(getlike, dc);
            cmd.CommandType = CommandType.Text;
            cmd3.CommandType = CommandType.Text;
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            int like = Convert.ToInt32(cmd3.ExecuteScalar());
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.Parameters.AddWithValue("@likes", like+1);
            cmd2.ExecuteNonQuery();
            dc.Close();
        }

        private void loadfeed()
        {
            try
            {
                if (dc.State == System.Data.ConnectionState.Closed)
                    dc.Open();
                for (int i = temp; i>= 1; i--)
                {
                    string query = "select username from userpost where id =" + i;
                    string query2 = "select content from userpost where id =" + i;
                    SqlCommand cmd = new SqlCommand(query, dc);
                    SqlCommand cmd2 = new SqlCommand(query2, dc);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandType = System.Data.CommandType.Text;
                    string user = Convert.ToString(cmd.ExecuteScalar());
                    string content = Convert.ToString(cmd2.ExecuteScalar());
                    addfeed(user, content,i);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void post_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dc.State == System.Data.ConnectionState.Closed)
                    dc.Open();
                string path = "C:\\DeepGamingData\\user\\user.bat";
                using var stream = new FileStream(path: path, FileMode.Open);
                string usernamepost = import(stream);
                stream.Close();
                string query = "Insert into userpost values(@username, @content)";
                string query2 = "Insert into React values (@id,@userreact, @likes,@cmtid)";
                string query3 = "select count(*) from userpost";
                SqlCommand stt = new SqlCommand(query, dc);
                SqlCommand react = new SqlCommand(query2, dc);
                SqlCommand test = new SqlCommand(query3, dc);
                test.CommandType = CommandType.Text;
                int test1 = Convert.ToInt32(test.ExecuteScalar())+1;
                stt.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = usernamepost;
                stt.Parameters.Add("@content", System.Data.SqlDbType.NVarChar).Value = Status.Text;
                stt.ExecuteNonQuery();
                react.Parameters.Add("@id",SqlDbType.Int).Value=test1;
                react.Parameters.Add("@userreact", SqlDbType.NVarChar).Value=usernamepost;
                react.Parameters.Add("@likes", SqlDbType.Int).Value = 0;
                react.Parameters.Add("@cmtid", SqlDbType.Int).Value = test1;                             
                react.ExecuteNonQuery();
                Status.Text = " ";               
                dc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void countfriend()
        {
            try
            {
                if (dc.State == ConnectionState.Closed)
                    dc.Open();
                using (FileStream fileStream = new FileStream("C:\\DeepGamingData\\user\\user.bat", FileMode.Open))
                {
                    string str = this.import((Stream)fileStream);
                    fileStream.Close();
                    SqlCommand sqlCommand = new SqlCommand("select count(*) from friends where username = '" + str + "'", dc);
                    sqlCommand.CommandType = CommandType.Text;
                    temp = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    dc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadfriend()
        {
            try
            {
                if (dc.State == ConnectionState.Closed)
                    dc.Open();
                for (int index = 1; index <= this.temp; ++index)
                {
                    using (FileStream fileStream = new FileStream("C:\\DeepGamingData\\user\\user.bat", FileMode.Open))
                    {
                        string str = this.import(fileStream);
                        SqlCommand sqlCommand = new SqlCommand("select friendname from friends inner join account on  friends.username = account.username where account.username ='" + str + "'", dc);
                        sqlCommand.CommandType = CommandType.Text;
                        this.addfriend(Convert.ToString(sqlCommand.ExecuteScalar()));
                        fileStream.Close(); 
                        dc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addfriend(string s)
        {
            Button element = new Button();
            element.Content = (object)s;
            element.FontSize = 20.0;
            element.Background = (Brush)Brushes.DarkGray;
            element.Click += new RoutedEventHandler(this.Userbox_Click);
            this.Friend.Children.Add((UIElement)element);
            this.Friend.Height += 10;
        }

        private void Userbox_Click(object sender, RoutedEventArgs e) => this.chatbox.NavigationService.Navigate((object)new DeepGamingSocial.chatbox());
      
        public string import(Stream stream)
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
        void export(Stream stream, int i)
        {
            var byte_i = BitConverter.GetBytes(i);
            stream.Write(byte_i, 0, 4);
        }
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            ChangePage.NavigationService.Navigate(new profileui());
        }

        private void guide_Click(object sender, RoutedEventArgs e)
        {
            ChangePage.NavigationService.Navigate(new guidefriendui());
        }

        private void Vid_Click(object sender, RoutedEventArgs e)
        {
            ChangePage.NavigationService.Navigate(new videoui());
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            ChangePage.NavigationService.Navigate(new settingui());
        }

        private void Status_GotFocus(object sender, RoutedEventArgs e)
        {
            Status.Text = "";
            Status.Foreground = Brushes.Black;
        }

        private void Newfeed_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new mainfeedui());
        }


    }
}

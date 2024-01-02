using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for Blog.xaml
    /// </summary>
    public partial class Blog : Page
    {
        public Blog()
        {          
            InitializeComponent();
            string path = "C:\\DeepGamingData\\user\\user.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            username = import(stream);
            countfeed();
            loadfeed();
        }
        private SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        int temp;
        string username;
        SqlCommand cmd;
        private void loadfeed()
        {
            try
            {
                if (dc.State == System.Data.ConnectionState.Closed)
                    dc.Open();
                for (int i = temp; i >= 1; i--)
                {
                    string query2 = "select content from userpost where id=@id AND username= @username";
                    cmd = new SqlCommand(query2, dc);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@id", i);
                    cmd.CommandType = System.Data.CommandType.Text;
                    string content = Convert.ToString(cmd.ExecuteScalar());
                    dc.Close();
                    addfeed(username, content, i);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void countfeed()
        {
           
            try
            {
                if (dc.State == System.Data.ConnectionState.Closed)
                    dc.Open();
                string query = "select count(*) from userpost where username = @username";
                SqlCommand count = new SqlCommand(query, dc);
                count.Parameters.AddWithValue("@username",username);
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
            TextBlock userbox = new TextBlock();
            TextBlock contentbox = new TextBlock();
            StackPanel react = new StackPanel();
            react.Height = 20;
            react.Orientation = Orientation.Horizontal;
            Button like = new Button();
            Button cmt = new Button();
            react.Children.Add(like);
            react.Children.Add(cmt);
            like.Content = "like " + likescount;
            cmt.Content = "Comment";
            like.Width = 600;
            cmt.Width = 600;
            like.FontSize = 12;
            like.Tag = index;
            cmt.Tag = index;
            cmt.FontSize = 12;
            like.Click += like_click;
            //cmt.Click += Cmt_Click;
            userbox.Text = "User: " + user;
            contentbox.Text = "          " + content;
            userbox.FontSize = 20;
            contentbox.FontSize = 20;
            userbox.Foreground = brush;
            userbox.FontWeight = FontWeights.Bold;
            contentbox.Foreground = brush;
            Line l = new Line();
            l.X1 = 0;
            l.X2 = 1100;
            l.Y1 = 0;
            l.Y2 = 0;
            l.Stroke = Brushes.White;
            l.StrokeThickness = 4;
            status.Children.Add(userbox);
            status.Children.Add(contentbox);
            status.Children.Add(react);
            status.Children.Add((Line)l);
        }
        private void like_click(object sender, RoutedEventArgs e)
        {
            if (dc.State == ConnectionState.Closed)
                dc.Open();
            string getid = $"select id from userpost where id = {(sender as Button).Tag}";
            string getlike = $"select likes from react where id = {(sender as Button).Tag}";
            string query = "Update React set likes=@likes where id=@id";
            SqlCommand cmd = new SqlCommand(getid, dc);
            SqlCommand cmd2 = new SqlCommand(query, dc);
            SqlCommand cmd3 = new SqlCommand(getlike, dc);
            cmd.CommandType = CommandType.Text;
            cmd3.CommandType = CommandType.Text;
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            int like = Convert.ToInt32(cmd3.ExecuteScalar());
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.Parameters.AddWithValue("@likes", like + 1);
            cmd2.ExecuteNonQuery();
            dc.Close();
        }
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
    }
}

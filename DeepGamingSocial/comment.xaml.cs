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
using System.Xml.Serialization;

namespace DeepGamingSocial
{
    /// <summary>
    /// Interaction logic for comment.xaml
    /// </summary>
    public partial class comment : Page
    {
        public comment()
        {
            InitializeComponent();
            string path = "cmtid.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            id = import(stream);
            stream.Close();
            loadcmt(id);
        }
        private SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        SqlCommand cmd;
        int cmts;
        int id;
        private void loadcmt(int id)
        {      
            countcmt(id);
            if (dc.State == ConnectionState.Closed)
                dc.Open();
            for (int i = cmts; i >= 1; i--)
            {
                string query = @$"Select userreact from (select ROW_NUMBER() Over(order by react.id)Number, 
											React.userreact  
						from (React inner join comment on React.id = comment.reactid)
									inner join userpost on React.id =  userpost.id	
						Where react.id = {id}
									) As T
						where Number ={i} ";
                string query2 = @$"Select cmtcontent  from (select ROW_NUMBER() Over(order by react.id)Number,  
											comment.cmtcontent   
						from (React inner join comment on React.id = comment.reactid)
									inner join userpost on React.id =  userpost.id	
						Where react.id = {id}
									) As T
						where Number ={i}";
                cmd = new SqlCommand(query, dc);
                cmd.CommandType = CommandType.Text;
                string userract = Convert.ToString(cmd.ExecuteScalar());
                cmd = new SqlCommand(query2, dc);
                cmd.CommandType = CommandType.Text;
                string content = Convert.ToString(cmd.ExecuteScalar());
                addcmt(userract, content);           
            }
            dc.Close();
        }
        private void addcmt(string userract, string content)
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#E1E1E1");
            brush.Freeze();
            TextBlock cmter = new TextBlock();
            TextBlock cmtcontent = new TextBlock();
            Line l1 = new Line();
            cmter.Text = "USER: "+userract;
            cmtcontent.Text = "     "+content;
            cmter.FontSize = 12;
            cmtcontent.FontSize = 12;
            cmter.Foreground = brush;
            cmtcontent.Foreground = brush;
            l1.X1 = 0;
            l1.X2 = 1100;
            l1.Y1 = 0;
            l1.Y2 = 0;
            l1.Stroke = Brushes.White;
            l1.StrokeThickness = 4;
            commentbox.Children.Add(cmter);
            commentbox.Children.Add(cmtcontent);
            commentbox.Children.Add(l1);
        }
        private void countcmt(int index)
        {
            string countcmt = $@"Select count(*)from (select ROW_NUMBER() Over(order by react.id)Number   
						from (React inner join comment on React.id = comment.reactid)
									inner join userpost on React.id =  userpost.id	
						Where react.id = {index}
									) As T";
            if (dc.State == ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(countcmt, dc);
            cmts = Convert.ToInt32(cmd.ExecuteScalar());
            dc.Close();
        }
        private int import(Stream stream)
        {
            var byte_i = new byte[4];
            stream.Read(byte_i, 0, 4);
            return BitConverter.ToInt32(byte_i,0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility= Visibility.Hidden;
        }

        private void post_Click(object sender, RoutedEventArgs e)
        {
            string query = "insert into comment values(@reactid, @cmtcontent)";
            if (dc.State == ConnectionState.Closed)
                dc.Open();
            cmd = new SqlCommand(query, dc);
            cmd.Parameters.AddWithValue("@reactid", id);
            cmd.Parameters.AddWithValue("@cmtcontent", postcmt.Text.ToString());
            cmd.ExecuteNonQuery();
            dc.Close();
        }
    }
}

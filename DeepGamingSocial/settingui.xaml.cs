using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for settingui.xaml
    /// </summary>
    public partial class settingui : Page
    {
        public settingui()
        {
            InitializeComponent();
        }
        SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True");
        SqlCommand cmd;

        private OpenFileDialog upava = new OpenFileDialog();
        private void avabtn_Click(object sender, RoutedEventArgs e)
        {
            bool? response = upava.ShowDialog();
            if (response == true)
            {
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(upava.FileName, UriKind.Absolute);
                source.EndInit();
                ava.Source = source;
                ava.Height = 100;
                ava.Width = 100;
                ava.Stretch = System.Windows.Media.Stretch.Uniform;

            }

        }

        private void Savechanges_Click(object sender, RoutedEventArgs e)
        {
            string path = "C:\\DeepGamingData\\user\\user.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            string username = import(stream);
            stream.Close();
            if (!string.IsNullOrEmpty(nicknamebox.Text))
            {
                try
                {
                    if (dc.State == System.Data.ConnectionState.Closed)
                        dc.Open();
                    string query = "Update profiles Set nickname = @nickname where username=@username";
                    cmd = new SqlCommand(query, dc);
                    cmd.Parameters.AddWithValue("@nickname", nicknamebox.Text);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                    dc.Close();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            if (!string.IsNullOrEmpty(gamebox.Text))
            {
                try
                {
                    if (dc.State == System.Data.ConnectionState.Closed)
                        dc.Open();
                    string query = "Update profiles Set favgame = @favgame where username=@username";
                    cmd = new SqlCommand(query, dc);
                    cmd.Parameters.AddWithValue("@favgame", gamebox.Text);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                    dc.Close();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            if (!string.IsNullOrEmpty(ingame.Text))
            {
                try
                {
                    if (dc.State == System.Data.ConnectionState.Closed)
                        dc.Open();
                    string query = "Update profiles Set ingame = @ingame where username=@username";
                    SqlCommand cmd = new SqlCommand(query, dc);
                    cmd.Parameters.AddWithValue("@ingame", ingame.Text);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                    dc.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(upava.FileName))
            {
                try
                {
                    if (dc.State == System.Data.ConnectionState.Closed)
                        dc.Open();
                    string query = "Update profiles Set ava = @ava where username=@username";
                    cmd = new SqlCommand(query, dc);
                    cmd.Parameters.AddWithValue("@ava", username + "ava");
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                    dc.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Directory.Exists($@"C:\DeepGamingData\avatar\{username}ava.png"))
                {
                    Directory.Delete($@"C:\DeepGamingData\avatar\{username}ava.png", true);
                    Directory.Move(upava.FileName, $@"C:\DeepGamingData\avatar\{username}ava.png");
                }
                else
                    File.Copy(upava.FileName, $@"C:\DeepGamingData\avatar\{username}ava.png", true);
            }
            if (!string.IsNullOrEmpty(oldpass.Password) && !string.IsNullOrEmpty(newpass.Password))
            {
                try
                {
                    string querycheck = $"select passwords from account where username =@username";
                    if (dc.State == System.Data.ConnectionState.Closed)
                        dc.Open();
                    cmd = new SqlCommand(querycheck, dc);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.CommandType = System.Data.CommandType.Text;
                    string temp = Convert.ToString(cmd.ExecuteScalar());
                    if (oldpass.Password == temp)
                    {
                        string query = "Update account Set passwords = @passwords where username=@username";
                        cmd = new SqlCommand(query, dc);
                        cmd.Parameters.AddWithValue("@passwords", newpass.Password);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.ExecuteNonQuery();
                        dc.Close();
                        oldpass.Password = "";
                        newpass.Password = "";
                    }
                    else
                    {
                        MessageBox.Show("Old password and New password does not match!");
                        dc.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
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
        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new mainfeedui());
        }
    }
}

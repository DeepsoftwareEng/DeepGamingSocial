using System;
using System.Collections.Generic;
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
    /// Interaction logic for guidefriendui.xaml
    /// </summary>
    public partial class guidefriendui : Page
    {
        public guidefriendui()
        {
            InitializeComponent();
        }
        private void Finding_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();           
        }
    }
}

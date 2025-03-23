using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Resident.Models;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        PrnContext _context = new PrnContext();
        List<User> users;
        public ChangeUserWindow()
        {
            InitializeComponent();
            ChangeUserWindow_Loaded();
        }
        public void ChangeUserWindow_Loaded()
        {
            users = new List<User>();
            dtUserImport.ItemsSource = _context.Users.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }
    }
}

using Galery.VM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Galery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainVM MainVm;

        public MainWindow()
        {
            InitializeComponent();
            MainVm = (MainVM)this.DataContext;
            MainVm.SetPassFunc(() => PasswordBox.Password, ()=>PasswordBox.Password = string.Empty);
            MainVm.ContentChanged += () => MainScrollViewer.ScrollToTop();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var content = (VM.MenuItem)MenuListBox.SelectedItem;
            MainVm.Content = content?.GetContent;
        }
    }
}

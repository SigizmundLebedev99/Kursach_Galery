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

namespace Galery.Pages
{
    /// <summary>
    /// Логика взаимодействия для TopPictures.xaml
    /// </summary>
    public partial class TopPictures : UserControl
    {
        readonly TopPicturesVM Context;

        internal TopPictures()
        {
            Context = new TopPicturesVM();
            DataContext = Context;
            InitializeComponent();
            this.Loaded += LoadDataFromServer;
        }

        private async void LoadDataFromServer(object sender, RoutedEventArgs e)
        {
            try
            {
                await Context.LoadData();
            }
            catch
            {
                await App.ShowErrorMessage("Не удалось подключиться к серверу");
            }
        }
    }
}

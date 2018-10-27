using Galery.VM;
using Galery.VM.Helpers;
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
    /// Логика взаимодействия для Picture.xaml
    /// </summary>
    public partial class PictureInfo : UserControl
    {
        internal PictureInfo()
        {
            InitializeComponent();
        }

        internal PictureInfo(MainVM mainVM, int pictureId, Roles role)
        { 
            InitializeComponent();
            this.DataContext = new PictureInfoVM(mainVM, pictureId, role);
        }
    }
}

﻿using Galery.VM;
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
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        internal UserInfo()
        {
            InitializeComponent();
        }

        internal UserInfo(MainVM mainVM,int userId, Roles role)
        {
            InitializeComponent();
            this.DataContext = new UserInfoVM(mainVM, userId, role);
        }
    }
}

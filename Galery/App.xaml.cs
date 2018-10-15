using Galery.ClientLogic;
using Galery.ClientLogic.Concreate;
using Galery.Server.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Galery
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            ServerAdress = ConfigurationManager.ConnectionStrings["ServerAdress"].ConnectionString;
            ClientService = new ClientService(ServerAdress);
        }

        internal static IClientService ClientService { get; private set; }
        internal static TokenResponse User { get; private set; }

        internal static event Action<bool> LoggedIn;
        internal static void LogIn(TokenResponse user)
        {
            ClientService.SetAutenticationHeader(user);
            User = user;
            LoggedIn?.Invoke(user.Roles.Any(r=>r == "admin"));
        }

        internal static string ServerAdress { get; private set; }
    }
}

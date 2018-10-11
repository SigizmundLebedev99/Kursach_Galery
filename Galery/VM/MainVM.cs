using Galery.ClientLogic;
using Galery.ClientLogic.Concreate;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows.Input;

namespace Galery.VM
{
    class MainVM : BaseVM
    {
        IClientService service;

        public MainVM()
        {
            service = new ClientService("http://localhost:49676/");
        }

        public string Path { get; set; }

        string _result;
        public string Result { get { return _result; }  set { _result = value; OnPropertyChanged("Result"); } }

        public ICommand Send
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    byte[] bytes = File.ReadAllBytes(@"C:\Users\hp\Pictures\images\moss4.jpg");
                    var res =  await service.Load.LoadImage(bytes);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = await res.Content.ReadAsAsync<string>();
                        if (string.IsNullOrEmpty(result))
                            Result = "O shit";
                    }
                    else
                        Result = res.ReasonPhrase + $":\n{await res.Content.ReadAsStringAsync()}";
                });
            }
        }
    }
}

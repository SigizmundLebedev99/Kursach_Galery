using Galery.Server.Service.DTO.PictureDTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    abstract class BasePictureListVM : BasePageVM
    {
        private const int PAGE_LENGTH = 50;

        public BasePictureListVM(MainVM mainVM) : base(mainVM)
        {}

        public IEnumerable<PictureInfoDTO> PicturesList { get; protected set; }

        private Visibility loadingVis;
        public Visibility LoadingVisibility { get => loadingVis; protected set { loadingVis = value; OnPropertyChanged(); } }

        private string header;

        public string Header { get=>header; protected set { header = value; OnPropertyChanged(); } }

        protected int Skip;

        public ICommand LoadMore
        {
            get
            {
                return new DelegateCommand(async obj=> 
                {
                    var newData = await LoadMoreData();
                    int count = newData.Count();
                    if (newData == null || count == 0)
                        return;
                    Skip += count;
                    PicturesList = PicturesList.Concat(newData);
                    OnPropertyChanged("PicturesList");
                });
            }
        }

        protected abstract Task<IEnumerable<PictureInfoDTO>> LoadMoreData();       
    }
}

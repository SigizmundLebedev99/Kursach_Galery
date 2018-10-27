using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Galery.VM
{
    class LikeVM : BaseVM
    {
        private bool isLiked;
        private bool isLikeEnabled;
        private int pictureId;
        private int likes;
        private bool IsLiked { get => isLiked; set { isLiked = value;OnPropertyChanged("ButtonColor"); } }

        public int Likes { get => likes; private set { likes = value;OnPropertyChanged(); } }
        public bool IsLikeEnabled { get=>isLikeEnabled; private set { isLikeEnabled = value; OnPropertyChanged(); } }
        public string ButtonColor { get { return isLiked ? "#FF673AB7" : "#DDFFFFFF"; } }

        public LikeVM(int likes)
        {
            Likes = likes;
            IsLikeEnabled = false;
        }

        public LikeVM(bool isLiked, int likes, int pictureId)
        {
            this.pictureId = pictureId;
            IsLiked = isLiked;
            Likes = likes;
            IsLikeEnabled = true;
        }

        public ICommand LikeSwitch
        {
            get
            {
                return new DelegateCommand(async obj =>
                {
                    IsLikeEnabled = false;
                    HttpResponseMessage res = null;
                    if (isLiked)
                    {
                        res = await App.ClientService.Picture.RemoveLike(pictureId);
                        if (res.IsSuccessStatusCode)
                        {
                            IsLiked = false;
                            Likes = Likes - 1;
                        }
                    }
                    else
                    {
                        res = await App.ClientService.Picture.SetLike(pictureId);
                        if (res.IsSuccessStatusCode)
                        {
                            IsLiked = true;
                            Likes = Likes + 1;
                        }
                    }
                    IsLikeEnabled = true;
                });
            }
        }
    }
}

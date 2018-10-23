using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.VM.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Galery.VM
{
    class PictureVM : BasePageVM
    {
        public Visibility CommonLoading { get; private set; } = Visibility.Visible;
        public Visibility CommonVis { get; private set; } = Visibility.Collapsed;
        public Visibility CommentPad { get; private set; } = Visibility.Visible;

        public PictureFullInfoDTO Picture { get; private set; }
        public string CommentText { get; set; }

        public ObservableCollection<CommentInfoDTO> CommentList { get; private set; }

        int userId;
        int pictureId;
        private event Action<int, Roles> onLoad;

        public PictureVM(MainVM mainVm, int pictureId, Roles role) : base(mainVm)
        {
            onLoad += async (i,r) => await LoadData(i,r);
            this.pictureId = pictureId;
            if(role == Roles.Unauthorized)
            {
                CommentPad = Visibility.Collapsed;
            }
            else
            {
                userId = App.User.UserId;
            }
            onLoad(pictureId, role);
        }

        public ICommand CreateComment
        {
            get
            {
                return new DelegateCommand(async obj=>
                {
                    var res = await App.ClientService.Comment.CreateComment(new CreateCommentDTO
                    {
                        PictureId =pictureId,
                        Text = this.CommentText,
                        UserId = userId});

                    if (res.IsSuccessStatusCode)
                    {
                        var comment = await res.Content.ReadAsAsync<CommentInfoDTO>();
                        CommentList.Add(comment);
                    }
                    else
                    {
                        await App.ShowErrorMessage("Не удалось добавить комментарий\n" + res.ReasonPhrase);
                    }
                });
            }
        }

        private async Task LoadData(int pictureId, Roles role)
        {
            var res = await App.ClientService.Picture.GetPictureByIdAnon(pictureId);
            if (res.IsSuccessStatusCode)
            {
                Picture = await res.Content.ReadAsAsync<PictureFullInfoDTO>();
                CommentList = new ObservableCollection<CommentInfoDTO>(Picture.CommentList);
                OnPropertyChanged("CommentList");
                OnPropertyChanged("Picture");
                CommonLoading = Visibility.Collapsed;
                OnPropertyChanged("CommonLoading");
                CommonVis = Visibility.Visible;
                OnPropertyChanged("CommonVis");
            }
            else
            {
                await App.ShowErrorMessage("Не удалось загрузить данные");
            }
        }
    }
}

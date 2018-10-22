using Galery.Resources;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Galery.VM
{
    class CreatePictureVM : BasePageVM
    {
        public CreatePictureDTO Model { get; set; }

        public Visibility LoadingVisibility { get; private set; } = Visibility.Visible;

        public Tag[] AllTags { get; private set; }

        public event Action<Picture> PictureCreated;

        private event Action onLoad;

        private List<int> selectedIds = new List<int>();

        public object Content { get; private set; }

        public CreatePictureVM(int userId, MainVM main) : base(main)
        {
            onLoad += async () => await LoadData();
            Model = new CreatePictureDTO { UserId = userId };
            onLoad();
        }

        private async Task LoadData()
        {
            var res = await App.ClientService.Tag.GetAllTags();
            if (res.IsSuccessStatusCode)
            {
                AllTags = await res.Content.ReadAsAsync<Tag[]>();
                LoadingVisibility = Visibility.Collapsed;
                OnPropertyChanged("LoadingVisibility");
                OnPropertyChanged("AllTags");
            }
        }

        public ICommand SelectTag
        {
            get
            {
                return new DelegateCommand(obj=>
                {
                    var id = (int)obj;
                    if (selectedIds.Contains(id))
                        selectedIds.Remove(id);
                    else
                        selectedIds.Add(id);
                });
            }
        }

        public ICommand CreatePicture
        {
            get
            {
                return new DelegateCommand(async obj=> 
                {
                    Model.TagIds = selectedIds;
                    var res = await App.ClientService.Picture.CreatePicture(Model);
                    if (res.IsSuccessStatusCode)
                    {
                        var createdPic = await res.Content.ReadAsAsync<Picture>();
                        PictureCreated?.Invoke(createdPic);
                        BackToPrevious.Execute(null);
                    }
                    else
                    {
                        await App.ShowErrorMessage("Не удалось добавить картинку\n" + res.ReasonPhrase);
                    }
                },
                obj => !string.IsNullOrEmpty(Model.ImagePath) && !string.IsNullOrEmpty(Model.Name));
            }
        }

        readonly OpenFileDialog Dialog = new OpenFileDialog
        {
            Filter = ""
        };

        private bool isLoadImage = false;


        public ICommand ChoosePicture
        {
            get
            {
                return new DelegateCommand(async obj =>
                {
                    bool? dialogRes = Dialog.ShowDialog();
                    if (dialogRes != null && dialogRes.Value)
                    {
                        isLoadImage = true;
                        Content = new Loading();
                        OnPropertyChanged("Content");
                        var res = await App.ClientService.Load.LoadImage(File.ReadAllBytes(Dialog.FileName),
                            Path.GetFileName(Dialog.FileName));
                        if (res.IsSuccessStatusCode)
                        {
                            Model.ImagePath = await res.Content.ReadAsAsync<string>();
                            Content = new Image
                            {
                                Height = 160,
                                Stretch = Stretch.Uniform,
                                Source = (ImageSource)new ImageSourceConverter().ConvertFromString(Dialog.FileName)
                            };
                            OnPropertyChanged("Content");
                        }
                        else
                        {
                            await App.ShowErrorMessage("Не удалось загрузить изображение: " + res.ReasonPhrase);
                        }
                        isLoadImage = false;
                    }
                }, obj => !isLoadImage
                );
            }
        }
    }
}

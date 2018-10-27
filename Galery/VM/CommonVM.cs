using Galery.Pages;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.VM.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Galery.VM
{
    class CommonVM
    {
        readonly MainVM _mainVm;

        public Roles CurrentRole { get; set; }

        private UserInfo userInfoPage;
        private PictureInfo pictureInfoPage;
        private PictureList pictureListForTag;

        public CommonVM(MainVM mainVM)
        {
            _mainVm = mainVM;      
        }

        public ICommand SelectUser
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {
                        int userId = (int)obj;

                        if (userInfoPage == null)
                        {
                            userInfoPage = new UserInfo();
                        }

                        userInfoPage.DataContext = new UserInfoVM(_mainVm, userId, CurrentRole);
                        _mainVm.Content = userInfoPage;
                    }
                    );
            }
        }

        public ICommand SelectTag
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {   
                        if (pictureListForTag == null)
                        {
                            pictureListForTag = new PictureList();
                        }

                        Tag tag = null;

                        switch (obj)
                        {
                            case Tag t:
                                {
                                    tag = t;
                                    break;
                                }
                            case TagDTO t:
                                {
                                    tag = new Tag { Id = t.Id, Name = t.Name };
                                    break;
                                }
                            default:
                                throw new InvalidCastException();
                        }

                        pictureListForTag.DataContext = new PictureListForTagVM(_mainVm, tag);
                        _mainVm.Content = pictureListForTag;
                    }
                    );
            }
        }

        public ICommand SelectPicture
        {
            get
            {
                return new DelegateCommand
                    (
                    obj =>
                    {
                        int pictureId = (int)obj;

                        if (pictureInfoPage == null)
                        {
                            pictureInfoPage = new PictureInfo();
                        }

                        pictureInfoPage.DataContext = new PictureInfoVM(_mainVm, pictureId, CurrentRole);
                        _mainVm.Content = pictureInfoPage;          
                    }
                    );
            }
        }
    }
}

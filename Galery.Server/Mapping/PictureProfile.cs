using AutoMapper;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.CommentDTO;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galery.Server.Mapping
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<CreatePictureDTO, Picture>().ForMember(e => e.ImagePath, c => c.Ignore());
            CreateMap<Picture, PictureInfoDTO>();
            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<Comment, CommentInfoDTO>();
            CreateMap<CreateUserDTO, User>().ForMember(e => e.Avatar, c => c.Ignore());
        }
    }
}

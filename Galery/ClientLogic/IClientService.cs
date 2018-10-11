namespace Galery.ClientLogic
{
    interface IClientService
    {
        IAccountClient Account { get; set; }
        ILoadClient Load { get; set; }
        IPictureClient Picture { get; set; }
        ICommentClient Comment { get; set; }
        ISubscribeClient Subscribe { get; set; }
        ITagClient Tag { get; set; }
    }
}

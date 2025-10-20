using AutoMapper;

using ZConnector.Models.Client;
using ZConnector.Models.Entities;
using ZConnector.Models.JWT;


namespace ZConnector.Data.Transaction
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<RegisterCredentials, User>();
            CreateMap<LoginCredentials, User>();
        }
    }
}

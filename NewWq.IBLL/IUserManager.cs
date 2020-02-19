using NewWq.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.IBLL
{
    public interface IUserManager
    {
        Task Register(string email, string password);
        bool Login(string email, string password, out Guid userid);
        Task ChangePassword(string email, string oldPwd, string newPwd);
        Task ChangeUserInformation(string email, string siteName, string imagePath);
        Task<UserInformationDto> GetUserByEmail(string email);

        Task<UserInformationDto> GetUserByOpenId(string openid, Dictionary<string, string> userInfo);
        Task<List<UserInformationDto>> GetAllUsers();
        Task<UserInformationDto> GetOneUserById(Guid id);
        Task UserEdit(Guid userid, string email, string imagepath, string sitename, int type);
        Task UserDelete(Guid id);
    }
}

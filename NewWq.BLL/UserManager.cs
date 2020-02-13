using NewWq.Dto;
using NewWq.IBLL;
using NewWq.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.BLL
{
    public class UserManager : IUserManager
    {
        public async Task Register(string email, string password)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                await userSvc.CreateAsync(new User()
                {
                    Email = email,
                    Password = password,
                    SiteName = "默认的用户",
                    ImagePath = "default.png"
                });
            }
        }

        public bool Login(string email, string password, out Guid userid)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                var user = userSvc.GetAll(m => m.Email == email && m.Password == password).FirstOrDefaultAsync();
                user.Wait();
                var data = user.Result;
                if (data == null)
                {
                    userid = new Guid();
                    return false;
                }
                else
                {
                    userid = data.Id;
                    return true;
                }
            }
        }

        public async Task ChangePassword(string email, string oldPwd, string newPwd)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                if (await userSvc.GetAll(m => m.Email == email && m.Password == oldPwd).AnyAsync())
                {
                    var user = await userSvc.GetAll().FirstAsync(m => m.Email == email);
                    user.Password = newPwd;
                    await userSvc.EditAsync(user);
                }
            }
        }

        public async Task ChangeUserInformation(string email, string siteName, string imagePath)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                var user = await userSvc.GetAll().FirstAsync(m => m.Email == email);
                user.SiteName = siteName;
                user.ImagePath = imagePath;
                await userSvc.EditAsync(user);
            }
        }

        public async Task<UserInformationDto> GetUserByEmail(string email)
        {
            using (IDAL.IUserService userSvc = new DAL.UserService())
            {
                if (await userSvc.GetAll(m => m.Email == email).AnyAsync())
                {
                    return await userSvc.GetAll().Where(m => m.Email == email).Select(m => new UserInformationDto()
                    {
                        Id = m.Id,
                        Email = m.Email,
                        FansCount = m.FansCount,
                        ImagePath = m.ImagePath,
                        SiteName = m.SiteName,
                        FocusCount = m.FocusCount,
                        Type = m.Type
                    }).FirstAsync();
                }

                return null;
            }
        }


        public async Task<List<UserInformationDto>> GetAllUsers()
        {
            using (IDAL.IUserService userService = new DAL.UserService())
            {
                return await userService.GetAll().Select(m => new UserInformationDto()
                {
                    Id = m.Id,
                    Email = m.Email,
                    ImagePath = m.ImagePath,
                    SiteName = m.SiteName,
                    FansCount = m.FansCount,
                    FocusCount = m.FocusCount,
                    Type = m.Type
                }).ToListAsync();
            }
        }

        public async Task<UserInformationDto> GetOneUserById(Guid id)
        {
            using (IDAL.IUserService userService = new DAL.UserService())
            {
                var user = await userService.GetOneByIdAsync(id);
                return new UserInformationDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    ImagePath = user.ImagePath,
                    SiteName = user.SiteName,
                    FansCount = user.FansCount,
                    FocusCount = user.FocusCount,
                    Type = user.Type
                };
            }
        }

        public async Task UserEdit(Guid userid, string email, string imagepath, string sitename, int type)
        {
            using (IDAL.IUserService userService = new DAL.UserService())
            {
                var user = await userService.GetOneByIdAsync(userid);
                user.Email = email;
                user.ImagePath = imagepath;
                user.SiteName = sitename;
                user.Type = type;
                await userService.EditAsync(user);

            }
        }

        public async Task UserDelete(Guid id)
        {
            using (IDAL.IUserService userService = new DAL.UserService())
            {
                await userService.RemoveAsync(id);
            }
        }


    }
}

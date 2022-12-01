using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBasedBasicAuthenticationWebAPI.Models
{
    public class UserValidate
    {
        public static bool Login(string username, string password)
        {
            UserBL userBL=new UserBL();
            var userList = userBL.GetUsers();
            return userList.Any(user=>user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password==password);
        }
        public static User GetUserDetails(string username, string password)
        {
            UserBL userBL=new UserBL();
            return userBL.GetUsers().FirstOrDefault(user=>user.UserName.Equals(username,StringComparison.OrdinalIgnoreCase)
            && user.Password==password);
        }
    }
}
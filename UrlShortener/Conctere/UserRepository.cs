using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Conctere
{
    public class UserRepository : IRepository<UserModel>
    {
        private ApplicationContext appContext;
        public UserRepository(ApplicationContext context)
        {
            appContext = context;
        }
        public void CreateRecord(UserModel user)
        {
            appContext.Users.Add(user);
        }

        public void DeleteRecord(int id)
        {
            UserModel user = appContext.Users.Find(id);
            if (user != null)
                appContext.Users.Remove(user);
        }

        public UserModel GetRecord(int id)
        {
            return appContext.Users.Find(id);
        }

        public IEnumerable<UserModel> GetRecords()
        {
            return appContext.Users;
        }

        public void UpdateRecord(UserModel user)
        {
            appContext.Entry(user).State = EntityState.Modified;
        }
    }
}

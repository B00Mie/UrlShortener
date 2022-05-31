using System;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Conctere
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private ApplicationContext appContext;
        private UrlRepository urlRepo;
        private UserRepository userRepo;

        private bool disposed = false;

        public DatabaseRepository(ApplicationContext context)
        {
            appContext = context;
        }


        public UrlRepository Urls
        {
            get
            {
                if (urlRepo == null)
                    urlRepo = new UrlRepository(appContext);
                return urlRepo;
            }
        }

        public UserRepository Users
        {
            get
            {
                if (userRepo == null)
                    userRepo = new UserRepository(appContext);
                return userRepo;
            }
        }

        public void Save()
        {
            appContext.SaveChanges();
        }

    }
}

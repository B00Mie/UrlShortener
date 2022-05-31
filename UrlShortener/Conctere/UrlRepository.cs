using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Conctere
{
    public class UrlRepository : IRepository<UrlModel>
    {
        private ApplicationContext appContext;
        public UrlRepository(ApplicationContext context)
        {
            appContext = context;
        }
        public void CreateRecord(UrlModel url)
        {
            appContext.Urls.Add(url);
        }

        public void DeleteRecord(int id)
        {
            UrlModel url = appContext.Urls.Find(id);
            if (url != null)
                appContext.Urls.Remove(url);
        }

        public UrlModel GetRecord(int id)
        {
            return appContext.Urls.Find(id);
        }

        public IEnumerable<UrlModel> GetRecords()
        {
            return appContext.Urls;
        }

        public void UpdateRecord(UrlModel url)
        {
            UrlModel data = appContext.Urls.Where(x => x.Id == url.Id).First();
            data.UrlModded = url.UrlModded;
            data.UrlOriginal = url.UrlOriginal;
        }
    }
}

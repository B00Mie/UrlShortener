using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Conctere;

namespace UrlShortener.Abstract
{
    public interface IDatabaseRepository
    {
        public UserRepository Users { get; }

        public UrlRepository Urls { get; }
        public void Save();

    }
}

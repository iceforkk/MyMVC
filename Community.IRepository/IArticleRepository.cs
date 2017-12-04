using Community.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.IRepository
{
    public interface IArticleRepository : IRepository<Jy_Article>
    {
        List<Jy_Article> GetArtList(string articltName, int pageIndex, int pageSize, out int totalCount, int type = 0);
    }
}

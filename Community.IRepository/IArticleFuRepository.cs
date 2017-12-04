using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service;
using Community.Model;

namespace Community.IRepository
{
    public interface IArticleFuRepository: IRepository<Jy_Article_Fujian>
    {
        List<ArticleAndFujian> GetArticleAndFujian(int type, int pageIndex, int pageSize, out int totalCount);
    }
}

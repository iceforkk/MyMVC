using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service;
using Community.IRepository;
using Community.Model;


namespace Community.Repository
{
    public class ArticleFuRepository : RepositoryBase<Jy_Article_Fujian>, IArticleFuRepository
    {
        public List<ArticleAndFujian> GetArticleAndFujian(int type, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new eduEntities())
            {
                var list = (from a in db.Jy_Article
                            join b in db.JY_Atype on a.Type equals b.ID
                            join c in db.Jy_Article_Fujian on a.id equals c.ArticleID
                            where b.ID == type
                            select new ArticleAndFujian()
                            {
                                Fujian = c.fujian,
                                Id = a.id,
                                Name = a.title,
                                TypeId = b.ID,
                                TypeName = b.TypeName,
                                IsTop=a.IsTop.Value
                            }
                           );
                totalCount = list.Count();
                var data = list.OrderByDescending(m => m.IsTop).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return data;
            }
        }
    }
}

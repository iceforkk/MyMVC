using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service;
using Community.IRepository;
using Community.Common;

namespace Community.Repository
{
    public class ArticleRepository : RepositoryBase<Jy_Article>, IArticleRepository
    {
        public List<Jy_Article> GetArtList(string articltName, int pageIndex, int pageSize, out int totalCount, int type = 0)
        {
            using (var db = new eduEntities())
            {
                IQueryable<Jy_Article> list = db.Jy_Article;
                if (type != 0)
                {
                    list = list.Where(m => m.Type == type);
                }
                if (!string.IsNullOrEmpty(articltName))
                {
                    list = list.Where(m => m.title.Contains(articltName));
                }

                totalCount = list.Count();
                var date = list.OrderBy(" IsTop desc,time desc").Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var fujian = new Jy_Article_Fujian();
                date.ForEach(p =>
                {
                    fujian = db.Jy_Article_Fujian.Where(pp => pp.ArticleID == p.id).FirstOrDefault();
                    if (fujian != null)
                    {
                        p.url = fujian.fujian;
                    }
                    if (!p.ClickRate.HasValue && p.ClickRate < 0)
                    {
                        p.ClickRate = 0;
                    }
                });
                return date;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service;
using Community.IRepository;

namespace Community.Repository
{
    public class ArticleCommentRepository : RepositoryBase<tblArticleComment>, IArticleCommentRepository
    {
    }
}

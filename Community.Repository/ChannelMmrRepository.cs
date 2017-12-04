using Community.IRepository;
using Community.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Community.Repository
{
    public class ChannelMmrRepository: RepositoryBase<JY_UrlMms>, IChannelMmrRepository
    {
        public JY_UrlMms Getfirst(decimal id)
        {
            using (var db = new eduEntities())
            {
                var date = db.JY_UrlMms.Where(m => m.CourseID == id).FirstOrDefault();
                return date;
            }
        }
    }
}

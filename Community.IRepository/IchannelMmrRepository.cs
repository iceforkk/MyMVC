using Community.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.IRepository
{
    public interface IChannelMmrRepository: IRepository<JY_UrlMms>
    {
        JY_UrlMms Getfirst(decimal id);
    }
}

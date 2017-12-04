using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service;
using Community.Model;

namespace Community.IRepository
{
    public interface ICourseRepository : IRepository<COURSEWARE>
    {
        List<CourserWareModel> RelatedFinishCourseList(string userid, int isFinish, int pageIndex, int pageSize, out int totalCount);

        List<CourserWareModel> GetCourseWareList(string name, string teacher, int channel, int pageIndex, int pageSize, out int totalCount);

        CourserWareModel GetCourserModel(decimal courserId, string userid);
    }
}

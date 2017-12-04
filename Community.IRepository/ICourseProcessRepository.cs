using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Service; 

namespace Community.IRepository
{
    public interface ICourseProcessRepository:IRepository<AICC_J_HIGH_SCORE>
    {
        string IsAny(string userid, string courseId);
    }
}

using Community.IRepository;
using Community.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Repository
{
    public class UserRepository : RepositoryBase<USERS>, IUserRepository
    {
        public int Resister(USERS user)
        {
            using (var db = new eduEntities())
            {


                string strsql = " exec  InsertMobileUSERS @USER_STATUS,@LAST_LOGIN_TIME,@USER_GROUP_ID,@USER_ID,@USER_TYPE,@USER_NAME,@USER_PWD,@USER_TEL,@LOGIN_TIMES,@id_card,@zj";

                SqlParameter[] parms = new SqlParameter[11];

                parms[0] = new SqlParameter("@USER_STATUS", user.user_state.Value);
                parms[1] = new SqlParameter("@LAST_LOGIN_TIME", user.LAST_LOGIN_TIME.Value);
                parms[2] = new SqlParameter("@USER_GROUP_ID", user.USER_GROUP_ID.Value);
                parms[3] = new SqlParameter("@USER_ID", user.USER_ID);
                parms[4] = new SqlParameter("@USER_TYPE", user.USER_TYPE.Value);
                parms[5] = new SqlParameter("@USER_NAME", user.USER_NAME);
                parms[6] = new SqlParameter("@USER_PWD", user.USER_PWD);
                parms[7] = new SqlParameter("@USER_TEL", user.USER_TEL);
                parms[8] = new SqlParameter("@LOGIN_TIMES", user.LOGIN_TIMES.Value);
                parms[9] = new SqlParameter("@id_card", user.id_card);
                parms[10] = new SqlParameter("@zj", user.zj.Value);

                return db.Database.ExecuteSqlCommand(strsql, parms);

            }
        }
    }
}

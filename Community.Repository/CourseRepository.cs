using Community.Service;
using Community.IRepository;
using Community.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Model;

namespace Community.Repository
{
    public class CourseRepository : RepositoryBase<COURSEWARE>, ICourseRepository
    {
        public List<CourserWareModel> RelatedFinishCourseList(string userid, int isFinish, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new eduEntities())
            {
                DateTime apptime = Convert.ToDateTime("2017-1-1");
                var list = (from a in db.AICC_J_HIGH_SCORE
                            join b in db.COURSEWARE on a.COURSE_ID equals b.COURSE_NUMBER
                            join d in db.JY_Lession on a.COURSE_ID equals d.Course_number
                            join e in db.jy_lessionimage on d.id equals e.LID
                            where a.STUDENT_ID == userid && a.OBJ_FIRST_DATE > apptime
                            && (isFinish == 1 ? a.length <= a.timems : a.length > a.timems)
                            orderby a.OBJ_FIRST_DATE descending
                            select new CourserWareModel
                            {
                                channelName = "",
                                ClickCount = b.topnum.Value,
                                CourseSize = b.LIMIT_TIME.ToString(),
                                CreateDate = b.COURSE_CREATEDATE.Value,
                                Credit = b.credit_hour.Value,
                                Exam = b.exam_id.ToString(),
                                Id = b.COURSE_ID,
                                Img = e.Path,
                                Description = b.DESCRIPTION,
                                Name = b.COURSE_NAME,
                                Teacher = b.teachername,
                                Standards = b.TYPE_ID.Value,
                                Duration = b.LIMIT_TIME.Value,
                                CommentCredit = b.Recommend,
                                Times = a.timems.Value,
                                Length = a.length.Value,
                                Type = b.COURSE_TYPE
                            });
                totalCount = list.Count();
                return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<CourserWareModel> GetCourseWareList(string name, string teacher, int channel, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new eduEntities())
            {
                var list = (from a in db.COURSEWARE
                            join b in db.JY_Channellesson on a.COURSE_ID equals (decimal)b.lessionid
                            join c in db.JY_Channel on b.Channelid equals (short)c.ChannelID
                            join d in db.JY_Lession on a.COURSE_NUMBER equals d.Course_number
                            join e in db.jy_lessionimage on d.id equals e.LID
                            where (channel != 0 ? b.Channelid == channel : b.Channelid == 58)
                            && (!string.IsNullOrEmpty(teacher) ? a.teachername == teacher : true)
                            && (!string.IsNullOrEmpty(name) ? a.COURSE_NAME.Contains(name) : true)
                            select new CourserWareModel
                            {
                                channelName = c.ChannelName,
                                ClickCount = a.topnum != null ? a.topnum.Value : 0,
                                CourseSize = a.LIMIT_TIME.ToString(),
                                CreateDate = a.COURSE_CREATEDATE.Value,
                                Credit = a.credit_hour != null ? a.credit_hour.Value : 0,
                                Exam = a.exam_id.ToString(),
                                Id = a.COURSE_ID,
                                Img = e.Path,
                                Description = a.DESCRIPTION,
                                Name = a.COURSE_NAME,
                                Learning = "0",
                                Teacher = a.teachername,
                                Standards = a.TYPE_ID != null ? a.TYPE_ID.Value : 0,
                                Duration = a.LIMIT_TIME != null ? a.LIMIT_TIME.Value : 0,
                                CommentCredit = a.Recommend,
                                Type = a.COURSE_TYPE
                            }).OrderByDescending(m => m.CreateDate);

                totalCount = list.Count();
                return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }


        public CourserWareModel GetCourserModel(decimal courserId, string userid)
        {
            using (var db = new eduEntities())
            {
                var model = (from a in db.COURSEWARE
                             join b in db.JY_Channellesson on a.COURSE_ID equals (decimal)b.lessionid
                             join c in db.JY_Channel on b.Channelid equals (short)c.ChannelID
                             join d in db.JY_Lession on a.COURSE_NUMBER equals d.Course_number
                             join e in db.jy_lessionimage on d.id equals e.LID
                             where a.COURSE_ID == courserId
                             select new CourserWareModel
                             {
                                 channelName = c.ChannelName,
                                 ClickCount = a.topnum.Value,
                                 CourseSize = a.LIMIT_TIME.ToString(),
                                 CreateDate = a.COURSE_CREATEDATE.Value,
                                 Credit = a.credit_hour.Value,
                                 Exam = a.exam_id.ToString(),
                                 Id = a.COURSE_ID,
                                 Img = e.Path,
                                 Description = a.DESCRIPTION,
                                 Name = a.COURSE_NAME,
                                 Learning = db.AICC_J_HIGH_SCORE.Where(m => m.COURSE_ID == a.COURSE_NUMBER && m.STUDENT_ID == userid).FirstOrDefault() == null ? "0" : db.AICC_J_HIGH_SCORE.Where(m => m.COURSE_ID == a.COURSE_NUMBER && m.STUDENT_ID == userid).FirstOrDefault().timems.Value.ToString(),
                                 Teacher = a.teachername,
                                 Standards = a.TYPE_ID.Value,
                                 Duration = a.LIMIT_TIME.Value,
                                 CommentCredit = a.Recommend,
                                 Type = a.COURSE_TYPE,
                                 Score = a.Recommend.ToString()
                             }).FirstOrDefault();

                return model;

            }
        }
    }
}

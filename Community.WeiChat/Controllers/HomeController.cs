using Community.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Community.Model;
using Community.Service;
using System.Web.Security;
using Community.WeiChat.Common;
using Newtonsoft.Json.Converters;

namespace Community.WeiChat.Controllers
{
    public class HomeController : BaseController
    {
        readonly IArticleRepository _articleRepository;
        readonly IArticleTypeRepository _articleTypeRepository;
        readonly IArticleCommentRepository _articleCommentRepository;
        readonly IUserRepository _userRepository;
        readonly IArticleFuRepository _articleFuRepository;
        readonly IArtFabulousRepository _artFabulousRepository;
        readonly IUserAppealRepository _userAppealRepository;
        readonly ICourseRepository _courseRepository;
        readonly IChannelRepository _channelRepository;
        readonly ICourseProcessRepository _courseProcessRepository;
        readonly IChannelMmrRepository _channelMmrRepository;
        readonly IChannelLiessonRepository _channelLiessonRepository;
        public HomeController(IArticleRepository articleRepository,
          IArticleTypeRepository articleTypeRepository,
          IArticleCommentRepository articleCommentRepository,
          IUserRepository userRepository,
          IArticleFuRepository articleFuRepository,
          IArtFabulousRepository artFabulousRepository,
          IUserAppealRepository userAppealRepository,
          ICourseRepository courseRepository,
          IChannelRepository channelRepository,
          ICourseProcessRepository courseProcessRepository,
          IChannelMmrRepository channelMmrRepository,
          IChannelLiessonRepository channelLiessonRepository
          )
        {
            _articleRepository = articleRepository;
            _articleTypeRepository = articleTypeRepository;
            _articleCommentRepository = articleCommentRepository;
            _userRepository = userRepository;
            _articleFuRepository = articleFuRepository;
            _artFabulousRepository = artFabulousRepository;
            _userAppealRepository = userAppealRepository;
            _courseRepository = courseRepository;
            _channelRepository = channelRepository;
            _courseProcessRepository = courseProcessRepository;
            _channelMmrRepository = channelMmrRepository;
            _channelLiessonRepository = channelLiessonRepository;
        }
        public ActionResult Index(int type)
        {
            ViewBag.type = type;
            ViewBag.titleName = _articleTypeRepository.GetEntity(m => m.ID == type).TypeName;
            ViewBag.titleImg = _articleFuRepository.GetArticleAndFujian(143, 1, 5, out int totalCount);
            return View();
        }

        [HttpGet]
        public ActionResult BaoMing()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BaoMing(string userid, string userName, string userphone)
        {
            var user = _articleCommentRepository.GetEntities(m => m.Context.Equals(userphone)).ToList();
            if (user.Count() > 0)
            {
                return Json(new { date = 0, message = "请勿重复报名！" });
            }
            else
            {
                var commentmodel = new tblArticleComment
                {
                    FromUser = userid,
                    ArtId = 5827,
                    Context = userphone,
                    CreatTime = DateTime.Now,
                    ParentId = 0,
                    thumbsup = 0,
                    ToUser = userName
                };
                if (_articleCommentRepository.Insert(commentmodel))
                {
                    return Json(new { date = 1, message = "报名成功" });
                }
                else
                {
                    return Json(new { date = 1, message = "报名失败" });
                }
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string userid, string userPwd, string userName, string userphone)
        {
            var isuser = _userRepository.GetEntities(m => m.USER_ID == userid).ToList();
            if (isuser.Count() > 0)
            {
                return Json(new { date = 0, message = "用户名已存在" });
            }

            var regUserModel = new USERS
            {
                USER_ID = userid,
                USER_PWD = userPwd,
                USER_TEL = userphone,
                USER_NAME = userName,
                USER_GROUP_ID = 0,
                id_card = "",
                USER_TYPE = 1,//1表示前台用户
                user_state = 0,//0表示正常
                LAST_LOGIN_TIME = DateTime.Now,
                LOGIN_TIMES = 0,
                zj = 0
            };
            //注册表  由于用户表没有注册时间 
            var userAppeal = new UserAppeal
            {
                USER_ID = userid,
                USER_PWD = userPwd,
                USER_TEL = userphone,
                USER_NAME = userName,
                id_card = "0",
                time = DateTime.Now,
            };
            bool symbol = false;

            int flag = _userRepository.Resister(regUserModel);

            symbol = _userAppealRepository.Insert(userAppeal);
            if (symbol == true && flag > 0)
            {
                //保存身份票据
                SetAuthenticationToken(regUserModel.USER_ID);
                //保存登录名 
                HttpCookie cookie = new HttpCookie("UserName");
                cookie.Value = regUserModel.USER_ID;
                cookie.Expires = DateTime.Now.AddDays(5);

                Response.Cookies.Set(cookie);
                return Json(new { date = 1, message = "注册成功" });
            }
            else
            {
                return Json(new { date = 1, message = "注册失败" });
            }
        }

        /// <summary>
        /// 课程频道列表
        /// </summary>
        /// <param name="parentid">父级频道编号</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetChannelInfoList()
        {
            APIInvokeResult result = new APIInvokeResult();
            //if (!string.IsNullOrWhiteSpace(model.ParentCode))
            //{
            //    var channelId = int.Parse(model.ParentCode);
            //    var channel = _channelRepository.GetEntity(m => m.ChannelID == channelId);
            //    if (channel != null)
            //    {
            //        model.ParentId = channel.ChannelID;
            //    }
            //}
            var date = _channelRepository.GetChannelList(out int totalCount).Select(s => new CourseCategoryResult
            {
                Id = s.ChannelID,
                ParentId = s.parentid.Value,
                Img = "",
                Name = s.ChannelName
            }
            ).ToList();
    
            var json = new ResultInfo<CourseChannelInfo>
            {
                TotalCount = totalCount,
                List = date.Where(m=>m.ParentId==1).Select(s => new CourseChannelInfo
                {
                    ChannelId = s.Id,
                    ChannelName = s.Name,
                    ParentId =   s.ParentId.ToString() ,
                    ImgUrl = "",
                    CourseCount = ""
                }).ToList()
            };
            result.Data = json;
            return Json(result);
        }
        /// <summary>
        /// 提交Mp4进度
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTimeNode(Mp4CourseProcessDataModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var courseid = model.CourseId;
            //string playingCourseKey = string.Format(CacheKey.PlayingCourseKey, LoginUserId);

            //JYCache.OneCacheMananger().Add(playingCourseKey, courseid);
            //var course = ServicesItems.CourseServices.GetCourse(courseid);
            var course = _courseRepository.GetEntity(m => m.COURSE_ID == courseid);
            string coruse_number = course.COURSE_NUMBER;
            var userid = LoginUserInfo();
            var userNm = _userRepository.GetEntity(j => j.USER_ID == userid).USER_NM;
            decimal time = 0;
            var videoLength = _channelMmrRepository.GetEntity(n => n.CourseID == courseid);
            var len = videoLength.Video_Length > 0 ? videoLength.Video_Length : 100;
            if (course != null && !string.IsNullOrEmpty(model.TimeNode))
            {
                var h = Convert.ToDecimal(model.TimeNode.Substring(0, 2));
                var m = Convert.ToDecimal(model.TimeNode.Substring(2, 2));
                var s = Convert.ToDecimal(model.TimeNode.Substring(4, 2));
                time = h * 3600 + m * 60 + s;
                //Debug.WriteLine(time);
                //  Console.WriteLine(time + ":::::");
                CourseProcess process = new CourseProcess
                {
                    //PortalId = PortalId,
                    PortalId = "",
                    UserId = LoginUserInfo(),
                    Position = time,
                    CourseId = courseid
                };

                var courseProcess = _courseProcessRepository.GetEntity(mm => mm.COURSE_ID == coruse_number && mm.STUDENT_ID == userid);
                if (courseProcess != null)
                {

                    courseProcess.timems = int.Parse(time.ToString());
                    _courseProcessRepository.Update(courseProcess);
                }
                else
                {
                    AICC_J_HIGH_SCORE pp = new AICC_J_HIGH_SCORE
                    {
                        COURSE_ID = coruse_number,
                        length = int.Parse(len.ToString()),
                        OBJ_FIRST_DATE = DateTime.Now,
                        OBJ_HIGH_SCORE = "0",
                        OBJ_OBJECTIVE_ID = "1",
                        SESSION_ID = "0",
                        STUDENT_ID = userid,
                        timems = int.Parse(time.ToString())
                    };
                    _courseProcessRepository.Insert(pp);

                }

            }
            var courseProcesss = _courseProcessRepository.GetEntity(mm => mm.COURSE_ID == coruse_number && mm.STUDENT_ID == userid);

            UserStudyInfo usi = new UserStudyInfo();
            usi.CourseId = courseid.ToString();
            usi.StartTime = courseProcesss.OBJ_FIRST_DATE != null ? Convert.ToDateTime(courseProcesss.OBJ_FIRST_DATE) : Convert.ToDateTime("1900-01-01");
            usi.CurrentProgress = ((decimal)courseProcesss.timems.Value / (decimal)videoLength.Video_Length.Value).ToString("F2");
            usi.LastLoation = courseProcesss.timems.ToString();
            usi.LastNodeId = "0";
            usi.NodeList = new List<NodeInfo>();
            result.Data = usi;
            return Json(result);
        }
        /// <summary>
        /// 获取课程详细
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        public ActionResult CourseDetial(int Id)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (Id == 0)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "请求错误";
                return Json(result);
            }
            var userLoginId = LoginUserInfo(); 
            var course = _courseRepository.GetCourserModel(Id, userLoginId);
            if (course == null)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "课程已下线";
                return Json(result);
            }
            var courseDownloadUrl = "";
            var coursePlayUrl = _channelMmrRepository.Getfirst(course.Id).Mms.Replace("\\", "/");
            var videoLength = _channelMmrRepository.Getfirst(Id);

            var len = videoLength.Video_Length > 0 ? videoLength.Video_Length : 100;

            string coursenumber = _courseRepository.GetEntity(p => p.COURSE_ID == Id).COURSE_NUMBER;
            if (_courseProcessRepository.IsAny(LoginUserInfo(), coursenumber) == "未选")
            {
                AICC_J_HIGH_SCORE pp = new AICC_J_HIGH_SCORE
                {
                    COURSE_ID = coursenumber,
                    length = int.Parse(len.ToString()),
                    OBJ_FIRST_DATE = DateTime.Now,
                    OBJ_HIGH_SCORE = "0",
                    OBJ_OBJECTIVE_ID = "1",
                    SESSION_ID = "0",
                    STUDENT_ID = LoginUserInfo(),
                    timems = 1
                };
                _courseProcessRepository.Insert(pp);
            }

            var json = new CourseInfo
            {
                CourseId = course.Id.ToString(),
                CourseName = course.Name,
                CourseType = coursePlayUrl.Contains("index.html") ? "H5" : "Mp4",
                RequiredFlag = "1",
                Credit = course.Credit.ToString("0.0"),
                CreateDate = course.CreateDate,
                DownloadUrl = courseDownloadUrl,
                DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                OnlineUrl = coursePlayUrl ?? "",
                Description = course.Description,
                Duration = ((decimal)len / 60).ToString("F0") + '.' + ((decimal)len % 60).ToString("F0"),
                TeacherName = course.Teacher,
                CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + course.Img,
                ExamId = course.Exam,
                CommentCredit = Convert.ToSingle(course.CommentCredit).ToString(),
                SelectFlag = _courseProcessRepository.IsAny(userLoginId, coursenumber),
                BrowseScore = course.Learning + "%",
                UserCount = 0,
                //课程分类 
                ChannelName = course.channelName,
                Remainder = ""

            };
            //获取进度
            var entity = _courseProcessRepository.GetEntity(m => m.COURSE_ID == coursenumber && m.STUDENT_ID == userLoginId);
            var lastpostion = entity != null ? entity.timems.Value : 0;
            var lastnodeid = "S001"; 
            //最后的位置
            json.LastLocation = lastpostion;////TODO 2015-02-28 09:24:08 添加LastNodeID
            json.BrowseScore = entity != null ? ((decimal)lastpostion / (decimal)len * 100).ToString("F2") + "%" : "0%";
            json.LastNodeId = lastnodeid ?? "S001";// lastnodeid,
            //评分
            json.AvgScore = course.Score;
            json.CourseSize = course.CourseSize ?? "0";
            json.ClickCount = course.ClickCount.ToString();
            result.Data = json;
            return View(json); 
        }
        /// <summary>
        /// 获取用户课程列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserCourseInfoList(UserCourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            List<CourserWareModel> list;
            int count = 0;
            var id = LoginUserInfo();
            list = _courseRepository.RelatedFinishCourseList(id, model.Finish, model.Page, model.Rows, out int totalcount);
            count = totalcount;
            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = count,
                List = list.Select((s, i) =>
                {
                    CourseProcess cpquery = new CourseProcess();
                    cpquery.CourseId = int.Parse(s.Id.ToString());
                    cpquery.UserId = LoginUserInfo();
                    var cplist = s.Learning;
                    var lastpostion = cplist;
                    var lastnodeid = "S001";
                    //var examstring = string.Empty;
                    //foreach (var exam in s.Course.Exam.Where(w=>w.PortalExam.Any(a=>a.PortalId==PortalId)))
                    //{
                    //  //  examstring+=exam.Name+""+exam
                    //}
                    var channel = s.channelName;
                    var onlineurl = "";
                    //if (s.Course.Standards == CourseStandards.Office.ToString())
                    //{
                    //    if (!string.IsNullOrWhiteSpace(onlineurl))
                    //    {
                    //        onlineurl = "/api/mobile/GetOffice?file=" + VerifyEncrypt.UrlEncode(onlineurl);
                    //        // onlineurl = "/Content/plugins/pdf/web/viewer.html?file=" + VerifyEncrypt.UrlEncode(onlineurl);
                    //    }
                    //}
                    return new CourseInfo
                    {
                        Index = (i + 1),
                        CourseId = s.Id.ToString(),
                        CourseName = s.Name,
                        Credit = s.Credit.ToString(),
                        CourseType = "Mp4",
                        CreateDate = s.CreateDate,
                        FinishDate = DateTime.Now,
                        BrowseScore = ((decimal)s.Times / (decimal)s.Length * 100).ToString("F2") + "%",
                        LastLocation = 0,////TODO 2015-02-28 09:24:08 添加LastNodeID
                        LastNodeId = lastnodeid ?? "S001",// lastnodeid,
                        TeacherName = s.Teacher,
                        Description = s.Description,
                        Duration = s.Length.ToString(),
                        OnlineUrl = onlineurl,// online != null ? online.Url : "",
                        CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img,
                        ChannelName = s.channelName,
                        RequiredFlag = "",
                        DownloadUrl = "",
                        DownloadUrlLow = "",
                        ExamId = "",
                        CommentCredit = "",
                        SelectFlag = "已选",
                        AvgScore = "0",
                        ClickCount = s.ClickCount.ToString(),
                        CourseSize = "",
                        Remainder = (s.Length - s.Times).ToString()
                    };
                }).ToList()
            };
            result.Data = json;
            return Json(result);
        }

        public ActionResult CourserWareList()
        {
            return View();
        }
        /// <summary>
        /// 获取课件列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetCourseInfoList(CourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (model.ChannelId < 0)
            {
                model.ChannelId = 0;
            }
            if (!string.IsNullOrWhiteSpace(model.ChannelCode))
            {
                var channelId = int.Parse(model.ChannelCode);
                var channel = _channelRepository.GetEntity(m => m.ChannelID == channelId);
                if (channel == null)
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "无此频道";
                    return Json(result);
                }
                model.ChannelId = channel.ChannelID;
            }
         
            var userid = LoginUserInfo();
            var date = _courseRepository.GetCourseWareList(model.Keyword, model.TeacherName, model.ChannelId, model.Page, model.Rows, out int totalcount);
           
          


            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    var flag = _courseProcessRepository.IsAny(userid, s.Id.ToString());
                    // var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = "Mp4",
                            RequiredFlag = s.RequiredFlag ? "1" : "0",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = "",
                            Description = s.Description ?? "",
                            Duration = s.Duration.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = flag,
                            BrowseScore = s.Learning + "%",
                            AvgScore = s.CommentCredit.ToString(),
                            CourseSize = s.CourseSize ?? "0",
                            ClickCount = s.ClickCount.ToString(),
                            NodeList = null,
                            //课程分类 
                            ChannelName = s.channelName,
                            Remainder = "",
                            LastNodeId = ""
                        };
                        return ci;
                    }
                })
            };
            result.Data = json;
            return Json(result);
        }
        public string GetArticleList(int pagenum, int type)
        {
            var list = _articleRepository.GetArtList("", pagenum, 7, out int totalCount, type);
            var jsonResult = new JsonResultModel(JsonResultType.success, list);
            IsoDateTimeConverter timejson = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };
            return JsonConvert.SerializeObject(jsonResult, timejson);
        }
        public string GetArticleCommentList(int artId)
        {
            var list = _articleCommentRepository.GetEntities(m => m.ArtId == artId).OrderByDescending(d => d.CreatTime);

            var json = new
            {
                date = list.Where(s => s.ParentId == 0).Select(m => new ArticleCommentModel
                {
                    ArtId = m.ArtId.Value,
                    ParentId = m.ParentId.Value,
                    Context = m.Context,
                    CreatTime = Convert.ToDateTime(m.CreatTime.ToString("yyyy-MM-dd HH:MM")),
                    FromUser = m.FromUser,
                    Id = m.Id,
                    ToUser = m.ToUser,
                    UpNum = m.thumbsup.Value,
                    ThisComment = list.Where(mm => mm.ParentId == m.Id).Select(s => new ArticleCommentModel
                    {
                        ArtId = s.ArtId.Value,
                        ParentId = s.ParentId.Value,
                        Context = s.Context,
                        CreatTime = Convert.ToDateTime(s.CreatTime.ToString("yyyy-MM-dd HH:MM")),
                        FromUser = s.FromUser,
                        Id = s.Id,
                        ToUser = s.ToUser,
                        UpNum = s.thumbsup.Value
                    }).ToList()
                }).ToList()
            };
            var jsonResult = new JsonResultModel(JsonResultType.success, json);
            IsoDateTimeConverter timejson = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };
            return JsonConvert.SerializeObject(jsonResult, timejson);
        }
        public ActionResult UserLogin()
        {
            return View();
        }
        public ActionResult AddComment(int artId, string text, string touser, string parentId)
        {
            var userID = LoginUserInfo();
            if (string.IsNullOrEmpty(userID))
            {
                return Json(new { date = 2, message = "请登录" });

            }
            tblArticleComment model = new tblArticleComment
            {
                ArtId = artId,
                Context = text,
                CreatTime = DateTime.Now,
                FromUser = userID,
                ParentId = parentId != "" ? int.Parse(parentId) : 0,
                thumbsup = 0,
                ToUser = touser
            };
            _articleCommentRepository.Insert(model);
            return Json(new { date = 1, message = "发送成功" });
        }
        public ActionResult AddArtFabulous(int artId)
        {
            var userID = LoginUserInfo();
            if (string.IsNullOrEmpty(userID))
            {
                return Json(new { date = 2, message = "请登录" });

            }
            var isfourse = _artFabulousRepository.GetEntities(m => m.ArtId == artId && m.UserId == userID);
            if (isfourse.Count() > 0)
            {
                return Json(new { date = 3, message = "只能点赞一次" });
            }
            Jy_Article_Fabulous model = new Jy_Article_Fabulous
            {
                ArtId = artId,
                UpdateTime = DateTime.Now,
                UserId = userID,
                UserName = _userRepository.GetEntity(m => m.USER_ID == userID).USER_NAME
            };
            _artFabulousRepository.Insert(model);
            return Json(new { date = 1, message = "点赞成功" });
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="userLogin"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        public ActionResult Login(string pwd, string userLogin)
        {

            if (string.IsNullOrWhiteSpace(userLogin))
                return Json(new { date = 0, message = "请输入账号" });
            var entity = _userRepository.GetEntity(m => m.USER_ID == userLogin);
            if (entity == null)
            {
                return Json(new { date = 0, message = "用户不存在" });
            }
            if (entity.USER_PWD != pwd)
            {
                return Json(new { date = 0, message = "密码错误" });
            }
            //保存身份票据
            SetAuthenticationToken(entity.USER_ID);
            //保存登录名

            HttpCookie cookie = new HttpCookie("UserName");
            cookie.Value = entity.USER_ID;
            cookie.Expires = DateTime.Now.AddDays(5);

            Response.Cookies.Set(cookie);
            return Json(new { date = 1, message = "登录成功" });
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            var cookie = Request.Cookies["UserName"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Index");
        }

        /// <summary>
        /// 注册登录票据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="createPersistentCookie"></param>
        public void SetAuthenticationToken(string token, bool createPersistentCookie = false)
        {
            string roles = string.Empty;

            FormsAuthentication.SetAuthCookie(token, true, FormsAuthentication.FormsCookiePath);


            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, token, DateTime.Now, DateTime.Now.AddDays(1), true, roles);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }


        public ActionResult ArtDetial(int id)
        {
            var model = _articleRepository.GetEntity(m => m.id == id);
            var TopList = _articleRepository.GetEntities(m => m.IsTop > 0).Take(5).ToList();
            if (model != null)
            {
                ViewBag.Art = model;
                ViewBag.fourseList = _artFabulousRepository.GetEntities(m => m.ArtId == id);
            }
            ViewBag.TopList = TopList;

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
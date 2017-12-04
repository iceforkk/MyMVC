using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Model
{
   public class ArticleAndFujian
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文章名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 附件图片地址
        /// </summary>
        public string Fujian { get; set; }

        /// <summary>
        /// 微信推荐
        /// </summary>
        public int IsTop { get; set; }
    }
}

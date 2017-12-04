using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Model
{
    public class ArticleCommentModel
    {
        public int Id { get; set; }

        public string FromUser { get; set; }

        public string ToUser { get; set; }

        public string Context { get; set; }

        public DateTime CreatTime { get; set; }

        public int ArtId { get; set; }

        public int ParentId { get; set; }

        public List<ArticleCommentModel> ThisComment { get; set; }

        public int UpNum { get; set; }


    } 
}

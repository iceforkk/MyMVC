//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Community.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class bookName
    {
        public int BookNameID { get; set; }
        public Nullable<int> BookTypeID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string AutoName { get; set; }
        public int IsActive { get; set; }
        public string Picture { get; set; }
        public Nullable<int> ByteCount { get; set; }
        public Nullable<int> ClickCount { get; set; }
        public Nullable<int> FlowerCount { get; set; }
        public string keyname { get; set; }
    }
}

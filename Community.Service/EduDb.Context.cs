﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eduEntities : DbContext
    {
        public eduEntities()
            : base("name=eduEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AICC_J_HIGH_SCORE> AICC_J_HIGH_SCORE { get; set; }
        public virtual DbSet<BookContent> BookContent { get; set; }
        public virtual DbSet<bookName> bookName { get; set; }
        public virtual DbSet<BookTitle> BookTitle { get; set; }
        public virtual DbSet<BookType> BookType { get; set; }
        public virtual DbSet<COURSEWARE> COURSEWARE { get; set; }
        public virtual DbSet<Jy_Article> Jy_Article { get; set; }
        public virtual DbSet<Jy_Article_Fabulous> Jy_Article_Fabulous { get; set; }
        public virtual DbSet<Jy_Article_Fujian> Jy_Article_Fujian { get; set; }
        public virtual DbSet<JY_Atype> JY_Atype { get; set; }
        public virtual DbSet<JY_Channel> JY_Channel { get; set; }
        public virtual DbSet<JY_Channellesson> JY_Channellesson { get; set; }
        public virtual DbSet<JY_Lession> JY_Lession { get; set; }
        public virtual DbSet<jy_lessionimage> jy_lessionimage { get; set; }
        public virtual DbSet<JY_UrlMms> JY_UrlMms { get; set; }
        public virtual DbSet<tblArticleComment> tblArticleComment { get; set; }
        public virtual DbSet<USER_COURSE_REG> USER_COURSE_REG { get; set; }
        public virtual DbSet<USER_GROUP> USER_GROUP { get; set; }
        public virtual DbSet<user_xueli> user_xueli { get; set; }
        public virtual DbSet<user_zj> user_zj { get; set; }
        public virtual DbSet<UserAppeal> UserAppeal { get; set; }
        public virtual DbSet<UserCredit> UserCredit { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }
    }
}

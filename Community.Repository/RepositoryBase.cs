using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity;
using Community.Service;

namespace Community.Repository
{
    public abstract class RepositoryBase<T>
        : IRepository.IRepository<T> where T : class, new()
    {
        private DbContext db = DbContextFactory.Create();
        #region 查询普通实现方案(基于Lambda表达式的Where查询)
        /// <summary>
        /// 获取所有Entity
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns>返回IEnumerable类型</returns>
        public virtual IEnumerable<T> GetEntities(Expression<Func<T, bool>> exp)
        {
            return db.Set<T>().Where(exp).ToList();
        }
        public virtual IEnumerable<T> GetALL()
        {

            return db.Set<T>().ToList();

        }
        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual int GetEntitiesCount(Expression<Func<T, bool>> exp)
        {

            return db.Set<T>().Where(exp).ToList().Count();

        }
        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param name="pageNumber">当前页</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderName">lambda排序名称</param>
        /// <param name="sortOrder">排序(升序or降序)</param>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetEntitiesForPaging(int pageNumber, int pageSize, Expression<Func<T, string>> orderName, string sortOrder, Expression<Func<T, bool>> exp, out long totalCount)
        {

            totalCount = FindBy(exp).Count();
            if (sortOrder == "asc") //升序排列
            {
                return db.Set<T>().Where(exp).OrderBy(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(exp).OrderByDescending(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

        }
        public virtual T FindOneBy(Expression<Func<T, bool>> predicate)
        {
            return FindBy(predicate).FirstOrDefault();
        }
        /// <summary>
        /// 根据条件查找满足条件的一个entites
        /// </summary>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual T GetEntity(Expression<Func<T, bool>> exp)
        {

            return db.Set<T>().Where(exp).SingleOrDefault();

        }
        /// <summary>
        /// 获取前几条
        /// </summary>
        /// <param name="top"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> FindTopBy(int top, Expression<Func<T, bool>> predicate)
        {
            return FindBy(predicate).Take(top);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual IQueryable<T> FindBy(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, out long totalCount)
        {
            totalCount = FindBy(predicate).Count();
            return FindBy(predicate).Take(pageSize).Skip((pageIndex - 1) * pageSize);
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }
        public virtual IQueryable<T> FindAll()
        {
            return db.Set<T>();
        }
        #endregion 
        #region 增改删实现
        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {

            var obj = db.Set<T>();
            obj.Add(entity);
            return db.SaveChanges() > 0;

        }
        /// <summary>
        /// 更新Entity(注意这里使用的傻瓜式更新,可能性能略低)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {

            var obj = db.Set<T>();
            obj.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;

        }
        /// <summary>
        /// 删除Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {

            var obj = db.Set<T>();
            if (entity != null)
            {
                obj.Attach(entity);

                obj.Remove(entity);
                return db.SaveChanges() > 0;
            }
            return false;

        }
        #endregion 
    }
}

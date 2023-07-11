using BaseProjectApp.Library.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected BaseProjectDBContext context { get; set; }
        private readonly DbSet<T> table = null;
        public GenericRepository(BaseProjectDBContext _context)
        {
            this.context = _context;
            table = _context.Set<T>();
        }

        public virtual List<T> GetAllById(Expression<Func<T, bool>> conditions = null)
        {
            if (conditions != null)
                return table.Where(conditions).ToList();
            else
                return table.ToList();
        }
        public virtual T GetByIdWithPredicate(Expression<Func<T, bool>> conditions = null)
        {
            Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
            if (typeof(T).GetProperty("Deleted") != null)
            {
                var epx = Expression.Parameter(typeof(T), "x");
                Expression left = Expression.PropertyOrField(epx, "Deleted");
                Expression right = Expression.Constant(false);
                Expression e1 = Expression.Equal(left, right);
                notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
            }

            if (conditions != null)
                return table.Where(notDeletBaseProjectAppredicate).Where(conditions).FirstOrDefault();
            else
                return table.Where(notDeletBaseProjectAppredicate).FirstOrDefault();
        }
        public virtual T GetById(int id)
        {
            return table.Find(id);
        }
        public virtual T GetByIdString(string id)
        {
            return table.Find(id);
        }
        public virtual void Insert(T entity)
        {
            table.Add(entity);
        }
        public virtual void InsertBulk(List<T> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }
        public virtual Tuple<bool, string> Delete(int id, bool HardDelete = false)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                    return new Tuple<bool, string>(false, "Not Exist");
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    table.Attach(entity);
                }
                Type t = entity.GetType();
                PropertyInfo property = t.GetProperty("Deleted");
                if (property != null && HardDelete == false)
                {
                    property.SetValue(entity, true);
                    context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }

            }
            catch (System.Exception ex)
            {
                var error = ex.Message + " " + ex.InnerException;
                return new Tuple<bool, string>(false, error);
            }
            return new Tuple<bool, string>(true, ""); ;
        }
        public virtual Tuple<bool, string> Delete(T entity, bool HardDelete = false)
        {
            try
            {
                if (entity == null)
                    return new Tuple<bool, string>(false, "Not Exist");

                if (context.Entry(entity).State == EntityState.Detached)
                {
                    table.Attach(entity);
                }

                Type t = entity.GetType();
                PropertyInfo property = t.GetProperty("Deleted");

                if (property != null && HardDelete == false)
                {
                    property.SetValue(entity, true);
                    context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    context.Entry(entity).State = EntityState.Deleted;
                }

            }
            catch (System.Exception ex)
            {
                var error = ex.Message + " " + ex.InnerException;
                return new Tuple<bool, string>(false, error);
            }
            return new Tuple<bool, string>(true, ""); ;
        }
        public virtual Tuple<bool, string> Delete(Expression<Func<T, bool>> expression = null, bool HardDelete = false)
        {
            var entities = GetAllById(expression);
            return DeleteBulk(entities , HardDelete: HardDelete);
        } 
        public virtual Tuple<bool, string> DeleteBulk(List<T> entities, bool HardDelete = false)
        {
            foreach (var entity in entities)
            {
                var res = Delete(entity , HardDelete: HardDelete);
                if (res.Item1 == false)
                {
                    return res;
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        public virtual bool Exist(Expression<Func<T, bool>> conditions)
        {
            return table.Any(conditions);
        }
        public virtual int Count(Expression<Func<T, bool>> conditions)
        {
            try
            {
                return table.Count(conditions);
            }
            catch
            {
                return 0;
            }
        }

        public virtual bool Any(Expression<Func<T, bool>> conditions)
        {
            try
            {
                return table.Any(conditions);
            }
            catch
            {
                return false;
            }
        }
         
        public virtual void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }

        public virtual void UpdateExist(T item, int id)
        {
            var old = table.Find(id);
            context.Entry(old).CurrentValues.SetValues(item);
            //return Task.Run(() => _context.Entry(old).CurrentValues.SetValues(item));  
        }

        public virtual void UpdateExistString(T item,string id)
        {
            var old = table.Find(id);
            context.Entry(old).CurrentValues.SetValues(item);
            //return Task.Run(() => _context.Entry(old).CurrentValues.SetValues(item));  
        }
        public virtual void Attach(T entity)
        {
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }


        public virtual T Detach(T entity, bool SetId = true)
        {
            context.Entry(entity).State = EntityState.Detached;
            if (entity.GetType().GetProperty("Id") != null && SetId)
            {
                entity.GetType().GetProperty("Id").SetValue(entity, 0);
            }
            return entity;
        }
        public virtual List<T> DetachBulk(List<T> entities, bool SetIds = true)
        {
            foreach (var entity in entities)
            {
                Detach(entity, SetIds);
            }
            return entities;
        }

        public async virtual Task<ICollection<TType>> SelectAll<TType>(Expression<Func<T, TType>> select , Expression<Func<T, bool>> conditions= null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includes) where TType : class
        {
            try
            {
                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }


                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);

                if (orderBy != null)
                    query = orderBy(query);
                 
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).Select(select).ToListAsync();
                 
            }

            catch (System.Exception ex)
            {
                return (ICollection<TType>)Enumerable.Empty<T>();
            }
        }

        public async virtual Task<ICollection<TType>> SelectAllPagging<TType>(Expression<Func<T, TType>> select , Expression<Func<T, bool>> conditions = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, ParameterPagination parameterPagination = null, params string[] includes) where TType : class
        {
            try
            {
                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }


                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);
                 
                if (orderBy != null)
                    query = orderBy(query); 

                if (parameterPagination == null)
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).Select(select).ToListAsync();

                else
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).Skip((parameterPagination.PageNumber - 1) * parameterPagination.PageSize)
                                   .Take(parameterPagination.PageSize).Select(select).ToListAsync();

            }

            catch (System.Exception ex)
            {
                return (ICollection<TType>)Enumerable.Empty<T>();
            }
        }

        public async virtual Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> conditions = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includes)
        {
            try
            {
                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }

                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);
                 
                if (orderBy != null)
                    query = orderBy(query);
                 
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
              

            }
            catch (System.Exception ex)
            {
                return Enumerable.Empty<T>();
            }

        }


        public async virtual Task<IEnumerable<T>> GetAllPagging(Expression<Func<T, bool>> conditions = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, ParameterPagination parameterPagination = null, params string[] includes)
        {
            try
            {
                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }

                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);

                if (orderBy != null)
                    query = orderBy(query);
                 
                if (parameterPagination == null)
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();

                else
                    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty))
                         .Skip((parameterPagination.PageNumber - 1) * parameterPagination.PageSize)
                               .Take(parameterPagination.PageSize).ToListAsync();
            }
            catch (System.Exception ex)
            {
                return Enumerable.Empty<T>();
            }

        }

        public async virtual Task<T> GetFirst(Expression<Func<T, bool>> conditions = null, params string[] includes)
        {
            try
            {
                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }

                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);



                return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }

        public async virtual Task<TType> SelectFirst<TType>(Expression<Func<T, TType>> select = null , Expression<Func<T, bool>> conditions = null, params string[] includes) where TType : class
        {
            try
            {

                Expression<Func<T, bool>> notDeletBaseProjectAppredicate = t => true;
                if (typeof(T).GetProperty("Deleted") != null)
                {
                    var epx = Expression.Parameter(typeof(T), "x");
                    Expression left = Expression.PropertyOrField(epx, "Deleted");
                    Expression right = Expression.Constant(false);
                    Expression e1 = Expression.Equal(left, right);
                    notDeletBaseProjectAppredicate = Expression.Lambda<Func<T, bool>>(e1, new ParameterExpression[] { epx });
                }


                IQueryable<T> query;
                if (conditions != null)
                    query = table.Where(notDeletBaseProjectAppredicate).Where(conditions);
                else
                    query = table.Where(notDeletBaseProjectAppredicate);

                return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).Select(select)?.FirstOrDefaultAsync();

            }
            catch (System.Exception ex)
            {
                return null;
            }

        }

        public virtual bool HardDelete(Expression<Func<T, bool>> expression = null)
        {
            try
            {
                context.Remove(table.Single(expression));
                return true;
            }

            catch
            {
                return false;
            }

        }

    }
}

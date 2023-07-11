
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Repositories.Base
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAllById(Expression<Func<T, bool>> expression = null);
        T GetByIdWithPredicate(Expression<Func<T, bool>> expression = null);
        T GetById(int id);
        T GetByIdString(string id);
        void Insert(T entity);
        void InsertBulk(List<T> entities);
        void Update(T entity);
        Tuple<bool, string> Delete(int id, bool HardDelete = false);
        Tuple<bool, string> Delete(T entity, bool HardDelete = false);
        Tuple<bool, string> Delete(Expression<Func<T, bool>> expression = null , bool HardDelete = false);
        Tuple<bool, string> DeleteBulk(List<T> entities, bool HardDelete = false);
        bool Exist(Expression<Func<T, bool>> expression);
        int Count(Expression<Func<T, bool>> expression);
        bool Any(Expression<Func<T, bool>> expression);
        T Detach(T entity, bool SetId = true);
        List<T> DetachBulk(List<T> entities, bool SetIds = true);


        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeExpressions); 
        Task<IEnumerable<T>> GetAllPagging(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, ParameterPagination parameterPagination = null, params string[] includeExpressions);
        Task<T> GetFirst(Expression<Func<T, bool>> expression = null, params string[] includeExpressions);

        
        Task<ICollection<TType>> SelectAll<TType>(Expression<Func<T, TType>> select , Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeExpressions) where TType : class;
        Task<ICollection<TType>> SelectAllPagging<TType>(Expression<Func<T, TType>> select , Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, ParameterPagination parameterPagination = null, params string[] includeExpressions) where TType : class; 
        Task<TType> SelectFirst<TType>(Expression<Func<T, TType>> select , Expression<Func<T, bool>> conditions = null, params string[] includes) where TType : class;




        bool HardDelete(Expression<Func<T, bool>> expression = null);
        void Attach(T entity);
        void UpdateExist(T item, int id);
        void UpdateExistString(T item, string id);
    }
}

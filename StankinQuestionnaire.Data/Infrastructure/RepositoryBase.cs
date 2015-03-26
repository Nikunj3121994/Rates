using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Diagnostics;

namespace StankinQuestionnaire.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private StankinQuestionnaireEntities dataContext;
        private readonly IDbSet<T> dbset;

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected StankinQuestionnaireEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();//TODO
            foreach (T obj in objects)
            {
                dbset.Remove(obj);
            }
        }

        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where = null)
        {
            dataContext.Database.Log += s => Debug.WriteLine(s);
            if (where == null)
            {
                return dbset.ToList();
            }
            return dbset.Where(where).ToList();

        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault();
        }
    }
}

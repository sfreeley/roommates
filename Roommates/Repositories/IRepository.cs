using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Repositories
{
    //interface that allows each Type (ie Roommate and room) to perform each CRUD function
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();
        TEntity GetById(int id);
        void Insert(TEntity entry);
        void Update(TEntity entry);
        void Delete(int id);
    }
}

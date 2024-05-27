using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        ResultPattern<T> Get(Func<T, bool> predicate);
        ResultPattern<Paging<T>> GetAll(GridifyQuery query);
        ResultPattern<T> Post(T entity);
        ResultPattern<T> Update(T entity);
    }
}

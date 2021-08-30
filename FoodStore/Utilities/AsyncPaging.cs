using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FoodStore.Utilities
{
    public static class AsyncPaging
    {
        //extension Method -> this
        public static async Task<List<T>> ReturnPages<T>(this IQueryable<T> inputCollection, int pageNumber, int pageSize)
        {
            return await inputCollection.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
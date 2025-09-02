using Messager_Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager_Project.Repository
{
    //This class should be temple to all other repositoris
    public abstract class BaseRepository
    {
        protected DbContext DbContext;

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

    }
}

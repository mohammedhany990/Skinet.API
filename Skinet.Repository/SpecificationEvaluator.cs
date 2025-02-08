using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Specifications;

namespace Skinet.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification?.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }
            if (specification?.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification?.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPaginationEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            query = specification?.Includes.Aggregate(query, (currentQ, exp) => currentQ.Include(exp));
            
            
            return query;
        }
    }
}

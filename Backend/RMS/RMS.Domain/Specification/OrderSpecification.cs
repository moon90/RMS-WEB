using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Specification
{
    public class OrderSpecification<T> where T : class
    {
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public bool IsDescending { get; set; } = false;

        public OrderSpecification()
        {
        }

        public OrderSpecification(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public OrderSpecification(Expression<Func<T, object>> orderBy, bool isDesc)
        {
            OrderBy = orderBy;
            IsDescending = isDesc;
        }
    }
}

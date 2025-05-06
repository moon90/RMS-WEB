using RMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Specification
{
    public class BaseSpecification<T> : OrderSpecification<T>, ISpecification<T> where T : class
    {
        private readonly string _defaultStringValue = $"Type: {0} - Value {1}. ";
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = [];
        public List<string> IncludesStrings { get; } = [];

        public override string ToString()
        {
            var result = new StringBuilder();

            if (Criteria is not null)
            {
                result.Append(string.Format(_defaultStringValue, nameof(Criteria), Criteria.ToString()));
            }

            return result.ToString();
        }
    }
}

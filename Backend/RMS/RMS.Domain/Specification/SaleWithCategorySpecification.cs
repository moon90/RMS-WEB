
using RMS.Domain.Entities;

namespace RMS.Domain.Specification
{
    public class SaleWithCategorySpecification : BaseSpecification<Sale>
    {
        public SaleWithCategorySpecification()
        {
            Includes.Add(s => s.Category);
        }
    }
}

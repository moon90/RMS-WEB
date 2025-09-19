
using RMS.Domain.Entities;

namespace RMS.Domain.Specification
{
    public class PurchaseWithCategorySpecification : BaseSpecification<Purchase>
    {
        public PurchaseWithCategorySpecification()
        {
            Includes.Add(p => p.Category);
        }
    }
}

using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IBranchIslandServices : IGenericInterface<BranchIsland>
    {
        bool BranchIslandCodeMustBeUnique(BranchIsland branchIsland);
    }
}

using iot.solution.model.Models;
using iot.solution.model.Repository.Interface;
using component.logger;

namespace iot.solution.model.Repository.Implementation
{
    public class KitTypeAttributeRepository : GenericRepository<KitTypeAttribute>, IKitTypeAttributeRepository
    {
        private readonly ILogger logger;
        public KitTypeAttributeRepository(IUnitOfWork unitOfWork, ILogger logManager) : base(unitOfWork, logManager)
        {
            logger = logManager;
            _uow = unitOfWork;
        }
    }
}
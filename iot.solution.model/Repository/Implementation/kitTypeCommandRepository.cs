using iot.solution.model.Models;
using iot.solution.model.Repository.Interface;
using component.logger;

namespace iot.solution.model.Repository.Implementation
{
    public class kitTypeCommandRepository : GenericRepository<KitTypeCommand>, IkitTypeCommandRepository
    {
        private readonly ILogger logger;
        public kitTypeCommandRepository(IUnitOfWork unitOfWork, ILogger logManager) : base(unitOfWork, logManager)
        {
            logger = logManager;
            _uow = unitOfWork;
        }
    }
}
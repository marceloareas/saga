using saga.Infrastructure.Extensions;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.ResearchLine
{
    /// <inheritdoc />
    public class ResearchLineRepository : BaseRepository<ResearchLineEntity>, IResearchLineRepository
    {
        public ResearchLineRepository(ContexRepository dbContext) : base(dbContext)
        {
        }
    }
}

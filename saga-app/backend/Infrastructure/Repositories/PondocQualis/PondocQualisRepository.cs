using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.PondocQualis
{
    /// <inheritdoc />
    public class PondocQualisRepository : BaseRepository<PondocQualisEntity>, IPondocQualisRepository
    {
        public PondocQualisRepository(ContexRepository dbContext) : base(dbContext)
        { }
    }
}

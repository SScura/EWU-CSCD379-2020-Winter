using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System.Threading.Tasks;

namespace SecretSanta.Business.Services
{
    public class GroupService : EntityService<Group>, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        { }
    }
}

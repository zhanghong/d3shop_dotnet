using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin
{


    public interface IMenuRepository : IRepository<Menu, MenuId>;

    public class MenuRepository(ApplicationDbContext context) : RepositoryBase<Menu, MenuId, ApplicationDbContext>(context), IMenuRepository
    {
    }
}

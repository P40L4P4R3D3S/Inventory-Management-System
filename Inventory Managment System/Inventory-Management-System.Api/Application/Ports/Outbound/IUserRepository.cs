using System.Collections.Generic;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Application.Ports.Outbound
{
    public interface IUserRepository
    {
        IReadOnlyList<User> GetAll();
        void SaveAll(IEnumerable<User> users);
    }
}

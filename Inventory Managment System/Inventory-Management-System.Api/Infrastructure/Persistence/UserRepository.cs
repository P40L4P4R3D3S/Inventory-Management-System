using System.Collections.Generic;
using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Domain.Entities;

namespace Inventory_Management_System.Api.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly JsonRepository<User> _repository;

        public UserRepository(string filePath)
        {
            _repository = new JsonRepository<User>(filePath);
        }

        public IReadOnlyList<User> GetAll()
        {
            return _repository.GetAll();
        }

        public void SaveAll(IEnumerable<User> users)
        {
            _repository.SaveAll(users);
        }
    }
}

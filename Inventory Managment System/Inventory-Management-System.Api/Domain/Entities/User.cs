using System;
using System.Text.Json.Serialization;
using Inventory_Management_System.Api.Domain.Enums;
using Inventory_Management_System.Api.Domain.Validation;

namespace Inventory_Management_System.Api.Domain.Entities
{
    public class User
    {
        [JsonInclude]
        public int Id { get; private set; }

        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public string PasswordHash { get; private set; }

        [JsonInclude]
        public Role Role { get; private set; }

        [JsonConstructor]
        public User(string name, string passwordHash, Role role)
        {
            UserValidators.ValidateName(name);
            UserValidators.ValidatePasswordHash(passwordHash);
            UserValidators.ValidateRole(role);

            Name = name.Trim();
            PasswordHash = passwordHash;
            Role = role;
        }

        public void AssignId(int id)
        {
            UserValidators.ValidateId(id);

            if (Id != 0)
            {
                throw new InvalidOperationException("The user already has an ID.");
            }

            Id = id;
        }

        public void UpdateName(string name)
        {
            UserValidators.ValidateName(name);

            Name = name.Trim();
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            UserValidators.ValidatePasswordHash(passwordHash);

            PasswordHash = passwordHash;
        }

        public void UpdateRole(Role role)
        {
            UserValidators.ValidateRole(role);

            Role = role;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Domain.Enums;
using Inventory_Management_System.Api.Domain.Exceptions;
using Inventory_Management_System.Api.Domain.Validation;
using Microsoft.AspNetCore.Identity;

namespace Inventory_Management_System.Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;

        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        private int _nextUserId;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _users = _userRepository.GetAll().ToList();
            _nextUserId = GetNextUserId();
        }

        public AuthenticationResult Register(
            string name,
            string password,
            Role role,
            int? performedByUserId = null
        )
        {
            UserValidators.ValidateName(name);
            UserValidators.ValidatePassword(password);
            UserValidators.ValidateRole(role);

            string normalizedName = name.Trim();

            bool nameExists = _users.Any(user =>
                user.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase)
            );

            if (nameExists)
            {
                throw new DuplicateException("name", normalizedName);
            }

            User user = new(normalizedName, "PENDING_PASSWORD_HASH", role);

            string passwordHash = _passwordHasher.HashPassword(user, password);

            user.UpdatePasswordHash(passwordHash);
            user.AssignId(_nextUserId++);

            _users.Add(user);
            SaveChanges();

            string token = _tokenService.GenerateToken(user);

            return new AuthenticationResult(user.Id, user.Name, user.Role, token);
        }

        public AuthenticationResult Login(string name, string password)
        {
            UserValidators.ValidateName(name);

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            string normalizedName = name.Trim();

            User user =
                _users.FirstOrDefault(existingUser =>
                    existingUser.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase)
                ) ?? throw new UnauthorizedAccessException("Invalid user name or password.");

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid user name or password.");
            }

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                string updatedHash = _passwordHasher.HashPassword(user, password);

                user.UpdatePasswordHash(updatedHash);

                SaveChanges();
            }

            string token = _tokenService.GenerateToken(user);

            return new AuthenticationResult(user.Id, user.Name, user.Role, token);
        }

        public User GetUserById(int id)
        {
            UserValidators.ValidateId(id);

            return FindUserById(id);
        }

        public IReadOnlyList<User> GetAllUsers(int authenticatedUserId)
        {
            User authenticatedUser = FindUserById(authenticatedUserId);

            if (authenticatedUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only administrators can view all users.");
            }

            return _users.OrderBy(user => user.Id).ToList();
        }

        public User UpdateCurrentUser(int authenticatedUserId, string? name, string? password)
        {
            User user = FindUserById(authenticatedUserId);

            bool hasName = !string.IsNullOrWhiteSpace(name);

            bool hasPassword = !string.IsNullOrWhiteSpace(password);

            if (!hasName && !hasPassword)
            {
                throw new ArgumentException("At least one field must be provided.");
            }

            if (hasName)
            {
                UpdateName(user, name!);
            }

            if (hasPassword)
            {
                UpdatePassword(user, password!);
            }

            SaveChanges();

            return user;
        }

        public User UpdateUserRole(int authenticatedAdminId, int targetUserId, Role role)
        {
            UserValidators.ValidateRole(role);

            User admin = FindUserById(authenticatedAdminId);

            if (admin.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only administrators can change user roles.");
            }

            if (authenticatedAdminId == targetUserId)
            {
                throw new InvalidOperationException(
                    "An administrator cannot change their own role."
                );
            }

            User targetUser = FindUserById(targetUserId);

            targetUser.UpdateRole(role);

            SaveChanges();

            return targetUser;
        }

        public void DeleteCurrentUser(int authenticatedUserId)
        {
            User user = FindUserById(authenticatedUserId);

            _users.Remove(user);

            SaveChanges();
        }

        private void UpdateName(User user, string name)
        {
            UserValidators.ValidateName(name);

            string normalizedName = name.Trim();

            bool nameExists = _users.Any(existingUser =>
                existingUser.Id != user.Id
                && existingUser.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase)
            );

            if (nameExists)
            {
                throw new DuplicateException("name", normalizedName);
            }

            user.UpdateName(normalizedName);
        }

        private void UpdatePassword(User user, string password)
        {
            UserValidators.ValidatePassword(password);

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            if (verificationResult != PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException(
                    "The new password must be different " + "from the current password."
                );
            }

            string passwordHash = _passwordHasher.HashPassword(user, password);

            user.UpdatePasswordHash(passwordHash);
        }

        private User FindUserById(int id)
        {
            UserValidators.ValidateId(id);

            return _users.FirstOrDefault(user => user.Id == id)
                ?? throw new NotFoundException($"User with ID '{id}' was not found.");
        }

        private int GetNextUserId()
        {
            return _users.Select(user => user.Id).DefaultIfEmpty(0).Max() + 1;
        }

        private void SaveChanges()
        {
            _userRepository.SaveAll(_users);
        }
    }
}

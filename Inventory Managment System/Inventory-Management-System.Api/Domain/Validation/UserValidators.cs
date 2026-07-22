using System;
using System.Text.RegularExpressions;
using Inventory_Management_System.Api.Domain.Enums;

namespace Inventory_Management_System.Api.Domain.Validation
{
    public static partial class UserValidators
    {
        private const int MinimumNameLength = 3;
        private const int MaximumNameLength = 100;

        private const int MinimumPasswordLength = 8;
        private const int MaximumPasswordLength = 128;

        public static void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "User ID must be greater than zero."
                );
            }
        }

        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("User name cannot be empty.", nameof(name));
            }

            string normalizedName = name.Trim();

            if (normalizedName.Length < MinimumNameLength)
            {
                throw new ArgumentException(
                    $"User name must contain at least " + $"{MinimumNameLength} characters.",
                    nameof(name)
                );
            }

            if (normalizedName.Length > MaximumNameLength)
            {
                throw new ArgumentException(
                    $"User name cannot contain more than " + $"{MaximumNameLength} characters.",
                    nameof(name)
                );
            }
        }

        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            if (password.Length < MinimumPasswordLength)
            {
                throw new ArgumentException(
                    $"Password must contain at least " + $"{MinimumPasswordLength} characters.",
                    nameof(password)
                );
            }

            if (password.Length > MaximumPasswordLength)
            {
                throw new ArgumentException(
                    $"Password cannot contain more than " + $"{MaximumPasswordLength} characters.",
                    nameof(password)
                );
            }

            if (!UppercaseRegex().IsMatch(password))
            {
                throw new ArgumentException(
                    "Password must contain at least one uppercase letter.",
                    nameof(password)
                );
            }

            if (!LowercaseRegex().IsMatch(password))
            {
                throw new ArgumentException(
                    "Password must contain at least one lowercase letter.",
                    nameof(password)
                );
            }

            if (!NumberRegex().IsMatch(password))
            {
                throw new ArgumentException(
                    "Password must contain at least one number.",
                    nameof(password)
                );
            }
        }

        public static void ValidatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
            }
        }

        public static void ValidateRole(Role role)
        {
            if (!Enum.IsDefined(role))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(role),
                    "The selected role is invalid."
                );
            }
        }

        [GeneratedRegex("[A-Z]")]
        private static partial Regex UppercaseRegex();

        [GeneratedRegex("[a-z]")]
        private static partial Regex LowercaseRegex();

        [GeneratedRegex("[0-9]")]
        private static partial Regex NumberRegex();
    }
}

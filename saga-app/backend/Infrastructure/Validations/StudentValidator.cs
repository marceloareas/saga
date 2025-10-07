using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for students.
    /// </summary>
    public class StudentValidator
    {
        private readonly IRepository _repository;

        public StudentValidator(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool ok, string message)> CanCreate(StudentDto dto)
        {
            if (dto == null) return (false, "Invalid payload.");
            if (string.IsNullOrWhiteSpace(dto.Email)) return (false, "Email is required.");
            if (string.IsNullOrWhiteSpace(dto.Cpf)) return (false, "CPF is required.");
            if (string.IsNullOrWhiteSpace(dto.Registration)) return (false, "Registration is required.");

            // Email
            var existingByEmail = await _repository.User.GetUserByEmail(dto.Email);
            if (existingByEmail is not null) return (false, "E-mail already in use.");

            // CPF
            var usersWithCpf = await _repository.User.GetAllAsync(u => u.Cpf == dto.Cpf);
            if (usersWithCpf?.Any() == true) return (false, "CPF already in use.");

            // Registration
            var regClash = await _repository.Student.GetAllAsync(s => s.Registration == dto.Registration);
            if (regClash?.Any() == true) return (false, "Registration already in use.");

            return (true, string.Empty);
        }

        public async Task<(bool ok, string message)> CanUpdate(StudentDto dto, Guid studentId)
        {
            if (dto == null) return (false, "Invalid payload.");
            var existingStudent = await _repository.Student.GetByIdAsync(studentId, s => s.User);
            if (existingStudent is null) return (false, "Student not found.");

            var currentUserId = existingStudent.UserId;

            // Email (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Email) && !string.Equals(dto.Email, existingStudent.User?.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingByEmail = await _repository.User.GetUserByEmail(dto.Email);
                if (existingByEmail is not null && existingByEmail.Id != currentUserId)
                    return (false, "E-mail already in use.");
            }

            // CPF (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Cpf) && !string.Equals(dto.Cpf, existingStudent.User?.Cpf, StringComparison.OrdinalIgnoreCase))
            {
                var usersWithCpf = await _repository.User.GetAllAsync(u => u.Cpf == dto.Cpf);
                if (usersWithCpf?.Any(u => u.Id != currentUserId) == true)
                    return (false, "CPF already in use.");
            }

            // Registration (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Registration) && !string.Equals(dto.Registration, existingStudent.Registration, StringComparison.OrdinalIgnoreCase))
            {
                var regClash = await _repository.Student.GetAllAsync(s => s.Registration == dto.Registration);
                if (regClash?.Any(s => s.Id != studentId) == true)
                    return (false, "Registration already in use.");
            }

            return (true, string.Empty);
        }
    }
}

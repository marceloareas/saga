using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for professors.
    /// </summary>
    public class ProfessorValidator
    {
        private readonly IRepository _repository;

        public ProfessorValidator(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool ok, string message)> CanCreate(ProfessorDto dto)
        {
            if (dto == null) return (false, "Invalid payload.");
            if (string.IsNullOrWhiteSpace(dto.Email)) return (false, "Email is required.");
            if (string.IsNullOrWhiteSpace(dto.Cpf)) return (false, "CPF is required.");
            if (string.IsNullOrWhiteSpace(dto.Siape)) return (false, "SIAPE is required.");

            // Email
            var existingByEmail = await _repository.User.GetUserByEmail(dto.Email);
            if (existingByEmail is not null) return (false, "E-mail already in use.");

            // CPF
            var usersWithCpf = await _repository.User.GetAllAsync(u => u.Cpf == dto.Cpf);
            if (usersWithCpf?.Any() == true) return (false, "CPF already in use.");

            // SIAPE
            var siapeClash = await _repository.Professor.GetAllAsync(p => p.Siape == dto.Siape);
            if (siapeClash?.Any() == true) return (false, "SIAPE already in use.");

            return (true, string.Empty);
        }

        public async Task<(bool ok, string message)> CanUpdate(ProfessorDto dto, Guid professorId)
        {
            if (dto == null) return (false, "Invalid payload.");
            var existingProfessor = await _repository.Professor.GetByIdAsync(professorId, p => p.User);
            if (existingProfessor is null) return (false, "Professor not found.");

            var currentUserId = existingProfessor.UserId;

            // Email (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Email) && !string.Equals(dto.Email, existingProfessor.User?.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingByEmail = await _repository.User.GetUserByEmail(dto.Email);
                if (existingByEmail is not null && existingByEmail.Id != currentUserId)
                    return (false, "E-mail already in use.");
            }

            // CPF (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Cpf) && !string.Equals(dto.Cpf, existingProfessor.User?.Cpf, StringComparison.OrdinalIgnoreCase))
            {
                var usersWithCpf = await _repository.User.GetAllAsync(u => u.Cpf == dto.Cpf);
                if (usersWithCpf?.Any(u => u.Id != currentUserId) == true)
                    return (false, "CPF already in use.");
            }

            // SIAPE (se mudou)
            if (!string.IsNullOrWhiteSpace(dto.Siape) && !string.Equals(dto.Siape, existingProfessor.Siape, StringComparison.OrdinalIgnoreCase))
            {
                var siapeClash = await _repository.Professor.GetAllAsync(p => p.Siape == dto.Siape);
                if (siapeClash?.Any(p => p.Id != professorId) == true)
                    return (false, "SIAPE already in use.");
            }

            return (true, string.Empty);
        }
    }
}

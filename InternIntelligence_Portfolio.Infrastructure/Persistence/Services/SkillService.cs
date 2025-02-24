using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Enums;
using System.Transactions;
using InternIntelligence_Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class SkillService(IJwtSession jwtSession, IUnitOfWork unitOfWork) : ISkillService
    {
        private readonly IJwtSession _jwtSession = jwtSession;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRepository<Skill> _skillRepository = 
            unitOfWork.GetRepository<Skill>();

        public async Task<Result<Guid>> CreateAsync(CreateSkillRequestDTO createSkillRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var userIdResult = _jwtSession.GetUserId();
            if (userIdResult.IsFailure) return Result<Guid>.Failure(userIdResult.Error);

            if (!Enum.TryParse<Proficiency>(createSkillRequest.ProficiencyLevel, true, out var proficiency))
            {
                return Result<Guid>.Failure(Error.BadRequestError("Invalid proficiency"));
            }

            var existedSkill = await _skillRepository.GetWhereAsync(s => s.Name.ToLower() == createSkillRequest.Name.ToLower(), cancellationToken);

            if (existedSkill != null)
                return Result<Guid>.Failure(Error.ConflictError($"Skill '{existedSkill.Name}' already exists."));

            var skill = Skill.Create(createSkillRequest.Name, createSkillRequest.Description, proficiency, createSkillRequest.YearsOfExperience);
            skill.UserId = userIdResult.Value;

            var isAdded = await _skillRepository.AddAsync(skill, cancellationToken);
            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isAdded || !isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("New skill could not be created."));

            scope.Complete();

            return Result<Guid>.Success(skill.Id);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<bool>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var skill = await _skillRepository.GetByIdAsync(id, cancellationToken);
            if (skill is null)
                return Result<bool>.Failure(Error.NotFoundError("Skill is not found."));

            var isDeleted = _skillRepository.Delete(skill);
            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isDeleted || !isSaved)
                return Result<bool>.Failure(Error.UnexpectedError("Skill could not be deleted."));

            scope.Complete();
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<GetSkillResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure)
                return Result<IEnumerable<GetSkillResponseDTO>>.Failure(isSuperAdminResult.Error);

            var skills = _skillRepository.Table.AsNoTracking();
            var skillDtos = await skills.Select(s => new GetSkillResponseDTO(s)).ToListAsync(cancellationToken);

            return Result<IEnumerable<GetSkillResponseDTO>>.Success(skillDtos);
        }

        public async Task<Result<GetSkillResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<GetSkillResponseDTO>.Failure(isSuperAdminResult.Error);

            var skill = await _skillRepository.Table.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (skill is null)
                return Result<GetSkillResponseDTO>.Failure(Error.NotFoundError("Skill not found."));

            var skillDto = new GetSkillResponseDTO(skill);
            return Result<GetSkillResponseDTO>.Success(skillDto);
        }

        public async Task<Result<Guid>> UpdateAsync(Guid id, UpdateSkillRequestDTO updateSkillRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) 
                return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var skill = await _skillRepository.GetByIdAsync(id, cancellationToken);
            if (skill is null)
                return Result<Guid>.Failure(Error.NotFoundError("Skill is not found."));

            if (!string.IsNullOrEmpty(updateSkillRequest.Name) && skill.Name != updateSkillRequest.Name)
            {
                skill.Name = updateSkillRequest.Name;

                var existedSkill = await _skillRepository.GetWhereAsync(s => s.Name.ToLower() == updateSkillRequest.Name.ToLower(), cancellationToken);

                if (existedSkill != null) 
                    return Result<Guid>.Failure(Error.ConflictError($"Skill '{existedSkill.Name}' already exists."));
            }

            if (!string.IsNullOrEmpty(updateSkillRequest.Description) && skill.Description != updateSkillRequest.Description)
            {
                skill.Description = updateSkillRequest.Description;
            }

            if (!string.IsNullOrEmpty(updateSkillRequest.ProficiencyLevel) && Enum.TryParse<Proficiency>(updateSkillRequest.ProficiencyLevel, true, out var proficiency) && skill.ProficiencyLevel != proficiency)
            {
                skill.ProficiencyLevel = proficiency;
            }

            if (updateSkillRequest.YearsOfExperience.HasValue && skill.YearsOfExperience != updateSkillRequest.YearsOfExperience.Value)
            {
                skill.YearsOfExperience = updateSkillRequest.YearsOfExperience.Value;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
            return Result<Guid>.Success(skill.Id);
        }
    }
}

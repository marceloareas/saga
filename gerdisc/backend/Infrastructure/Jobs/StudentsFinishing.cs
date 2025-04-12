using System.Text;
using saga.Infrastructure.Providers.Interfaces;
using saga.Infrastructure.Repositories;
using saga.Models.Entities;
using saga.Models.Enums;

namespace Infrastructure.Jobs
{
    public class StudentsFinishing : HangfireJobBase
    {
        private readonly IRepository _repository;
        private readonly IEmailSender _emailSender;

        public StudentsFinishing(ILogger<StudentsFinishing> logger, IRepository repository, IEmailSender emailSender) : base(logger)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        protected override async Task ProcessJobAsync()
        {
            DateTime dangerousDate = DateTime.UtcNow.Date.AddDays(30);

            var endOfCourseStudents = await _repository.Student.GetAllAsync(x =>
                (x.ProjectDefenceDate <= dangerousDate || x.ProjectQualificationDate <= dangerousDate)
                && x.Status == StatusEnum.Active, x => x.User);
            var orientations = await _repository.Orientation.GetAllAsync(x =>
                endOfCourseStudents.Select(x => x.UserId).Contains(x.StudentId),
                x => x.Student, x => x.Professor);

            var studentInfo = endOfCourseStudents.ToDictionary(x => x.UserId);
            foreach (var orientationGroup in orientations.GroupBy(x => x.ProfessorId))
            {
                await NotifyProfessorAsync(orientationGroup, studentInfo);
            }

            foreach (var student in endOfCourseStudents)
            {
                if (student.LastNotification == null && DateTime.UtcNow.Date.AddDays(-7) > student.LastNotification)
                {
                    _logger.LogInformation($"End of Course Student: {student.Id}");
                    await NotifyStudentAsync(student);
                    await UpdateStudentAsync(student);
                }
            }
        }

        private async Task NotifyProfessorAsync(IGrouping<Guid, OrientationEntity> groupedOrientations, Dictionary<Guid, StudentEntity> studentInfo)
        {
            string emailSubject = "Próximas Defesas de Alunos";
            var body = new StringBuilder();
            body.AppendLine("Os seguintes estudantes estão concluindo o curso:");

            string emailBody = EmailTemplates.EmailTemplates.StudentsFinishingFromProfessorEmailTemplate(groupedOrientations, studentInfo);
            string professorEmail = groupedOrientations?.FirstOrDefault()?.Professor?.Email;
            await _emailSender.SendEmail(professorEmail, emailSubject, emailBody).ConfigureAwait(false);
        }

        private async Task NotifyStudentAsync(StudentEntity student)
        {
            var defenseTypes = new List<string>();

            if (student.ProjectDefenceDate <= DateTime.UtcNow.Date.AddDays(30))
            {
                defenseTypes.Add("Defesa");
            }
            if (student.ProjectQualificationDate <= DateTime.UtcNow.Date.AddDays(30))
            {
                defenseTypes.Add("Qualificação");
            }

            string defenseTypeText = string.Join(" e ", defenseTypes);
            string emailSubject = $"Data limite de {defenseTypeText} se aproximando.";
            string emailBody = EmailTemplates.EmailTemplates.UpcomingDefenseEmailTemplate(student.User?.FirstName, defenseTypeText, student.ProjectQualificationDate, student.ProjectDefenceDate);

            await _emailSender.SendEmail(student.User.Email, emailSubject, emailBody).ConfigureAwait(false);
        }

        private async Task UpdateStudentAsync(StudentEntity student)
        {
            student.LastNotification = DateTime.UtcNow;
            await _repository.Student.UpdateAsync(student).ConfigureAwait(false);
        }
    }
}


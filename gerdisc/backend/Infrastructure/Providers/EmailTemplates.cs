using System.Text;
using saga.Models.Entities;

namespace Infrastructure.EmailTemplates
{
    public static class EmailTemplates
    {
        public static string WelcomeEmailTemplate(string resetPasswordPath, string token)
        {
            return $@"<html>
                        <body>
                            <p>Bem-vindo(a) à Pós-Graduação do CEFET RJ!</p>
                            <p>Para acessar sua conta, por favor, defina sua senha clicando no seguinte link:</p>
                            <p><a href=""{resetPasswordPath}?token={token}"">{resetPasswordPath}?token={token}</a></p>
                        </body>
                    </html>";
        }

        public static string ResetPasswordEmailTemplate(string resetPasswordPath, string token)
        {
            return $@"<html>
                        <body>
                            <p>Você solicitou a redefinição da sua senha. Por favor, clique no seguinte link para redefinir sua senha:</p>
                            <p><a href=""{resetPasswordPath}?token={token}"">{resetPasswordPath}?token={token}</a></p>
                        </body>
                    </html>";
        }

        public static string UpcomingDefenseEmailTemplate(string? firstName, string defenseTypeText, DateTime? qualificationDate, DateTime? defenseDate)
        {
            return $@"<html>
                        <body>
                            <p>Prezado(a) {firstName},</p>
                            <p>Este é um lembrete de que a data da sua {defenseTypeText} está se aproximando. Por favor, certifique-se de se preparar e estar pronto(a) para a sua apresentação.</p>
                            <p>Se tiver alguma dúvida ou precisar de ajuda, sinta-se à vontade para entrar em contato com o seu orientador.</p>
                            <p>Se você precisar de mais tempo para se preparar adequadamente, pode solicitar uma prorrogação entrando em contato com o seu orientador acadêmico.</p>
                            <p>Data da sua Qualificação: {qualificationDate}</p>
                            <p>Data da sua Defesa: {defenseDate}</p>
                            <br>
                            <p>Atenciosamente,</p>
                            <p>A Equipe Acadêmica</p>
                        </body>
                    </html>";
        }

        public static string StudentsFinishingFromProfessorEmailTemplate(IGrouping<Guid, OrientationEntity> orientations, Dictionary<Guid, StudentEntity> students)
        {
            var body = new StringBuilder();

            body.AppendLine("Os seguintes estudantes estão concluindo o curso:");
            body.AppendLine("<table>");
            body.AppendLine("<tr>");
            body.AppendLine("<th>Nome</th>");
            body.AppendLine("<th>Sobrenome</th>");
            body.AppendLine("<th>Email</th>");
            body.AppendLine("<th>Data de Defesa</th>");
            body.AppendLine("<th>Data de Qualificação</th>");
            body.AppendLine("</tr>");

            foreach (var orientation in orientations)
            {
                if (students.TryGetValue(orientation.StudentId, out var student))
                {
                    body.AppendLine("<tr>");
                    body.AppendLine($"<td>{student.User?.FirstName}</td>");
                    body.AppendLine($"<td>{student.User?.LastName}</td>");
                    body.AppendLine($"<td>{student.User?.Email}</td>");
                    body.AppendLine($"<td>{student.ProjectDefenceDate}</td>");
                    body.AppendLine($"<td>{student.ProjectQualificationDate}</td>");
                    body.AppendLine("</tr>");
                }
            }

            body.AppendLine("</table>");
            return body.ToString();
        }
    }
}


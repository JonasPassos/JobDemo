using Quartz;
using System.Net.Mail;

public class EmailJob : IJob
{
    public async Task Execute(IJobExecutionContext context) // Método que será executado quando o trabalho for disparado
    {
        string remetenteEmail = Environment.GetEnvironmentVariable("EMAIL_REMETENTE"); // Define o e-mail do remetente
        string remetenteSenha = Environment.GetEnvironmentVariable("EMAIL_SENHA"); // Define a senha do remetente (use uma senha de aplicativo se necessário)
        //IF se esta correto
        if (string.IsNullOrEmpty(remetenteEmail) || string.IsNullOrEmpty(remetenteSenha))
        {
            Console.WriteLine("Erro: As variáveis de ambiente para o e-mail ou senha não estão definidas.");
            return; // Retorna se as variáveis de ambiente não estiverem definidas
        }

        string destino = Environment.GetEnvironmentVariable("EMAIL_DESTINO"); // Define o e-mail do destinatário
        

        using (var mensagem = new MailMessage())
        {
            mensagem.From = new MailAddress(remetenteEmail); // Define o remetente da mensagem

            mensagem.To.Add(destino); // Adiciona o destinatário à mensagem
            mensagem.Subject = "Passando para te lembrar!"; // Define o assunto do e-mail
            mensagem.Body = @"
            <html>
                <body style='font-family: Arial, sans-serif; text-align: center;'>
                <h2 style='color: #333;'>Olá amore, Te amoo! 👋</h2>
                    <img src='https://media.giphy.com/media/ASd0Ukj0y3qMM/giphy.gif'
                         alt='Banner' style='width: 100%; max-width: 600px; border-radius: 8px;'>

                    <p>Posso ser chato e esquecer algumas coisas,mas sempre penso em VOCÊ.</p>
                </body>
            </html>";
            mensagem.IsBodyHtml = true; // Define que o corpo do e-mail é HTML

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true; // Habilita o uso de SSL para segurança
                smtp.UseDefaultCredentials = false; // Não usa as credenciais padrão do sistema
                smtp.Credentials = new System.Net.NetworkCredential(remetenteEmail, remetenteSenha); // Define as credenciais do remetente
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // Define o método de entrega como rede
                try
                {
                    await smtp.SendMailAsync(mensagem); // Envia a mensagem
                    Console.WriteLine($"[{DateTime.Now}] E-mail enviado com sucesso!"); // Exibe uma mensagem de sucesso no console
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}"); // Exibe uma mensagem de erro no console se ocorrer um problema ao enviar o e-mail
                }
            }
        }
    }
} // Fim da classe EmailJob
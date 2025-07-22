using Quartz;
using Quartz.Impl;

internal class Program
{
    private static async Task Main(string[] args)
    {
        StdSchedulerFactory factory = new StdSchedulerFactory(); // Cria uma instância do StdSchedulerFactory para configurar o agendador
        IScheduler scheduler = await factory.GetScheduler(); // Obtém uma instância do agendador
        await scheduler.Start(); // Inicia o agendador

        // Define um trabalho do tipo EmailJob que será executado periodicamente
        IJobDetail job = JobBuilder.Create<EmailJob>() // Cria um trabalho do tipo EmailJo
            .WithIdentity("emailJob", "group1") // Define a identidade do trabalho
            .Build();

        var startime = DateBuilder.TodayAt(6, 30, 0); // Define o horário de início do trabalho para as 6:30 AM

        // Define um gatilho que dispara o trabalho a cada 10 segundos
        ITrigger trigger = TriggerBuilder.Create() // Cria um gatilho
            .WithIdentity("emailTrigger", "group1") // Define a identidade do gatilho
            .StartAt(startime)
            //.WithCronSchedule("0 0 0/4 * * ?") // especifica a expressão cron para disparar o trabalho diariamente às 6:30 AM
            .WithSimpleSchedule(x => x.WithIntervalInHours(24) // Define o intervalo de repetição do gatilho para 24 horas
                .RepeatForever()) // Repete indefinidamente
            .Build();

        await scheduler.ScheduleJob(job, trigger); // Agenda o trabalho com o gatilho definido

        Console.WriteLine("Pressione qualquer tecla para sair..."); // Exibe uma mensagem no console
        Console.ReadLine(); // Aguarda o usuário pressionar uma tecla
    }
}
using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Service;
using SchoolScheduleApp.Exceptions;
using System;

namespace SchoolScheduleApp
{
    internal class ScheduleConsole
    {
        private bool isWorking;

        private readonly ConsoleCommander consoleCommander;

        private ScheduleConsole(ConsoleCommander consoleCommander)
        {
            this.consoleCommander = consoleCommander;
        }

        public static ScheduleConsole Create()
        {
            var service = ScheduleService.Create(
                b => b.UseNpgsql("host=localhost;database=sched5;username=schedule;password=schedule;"));
            var commander = new ConsoleCommander(service, RequestInput, Console.Write, Console.WriteLine);

            return new ScheduleConsole(commander);
        }

        public void Start()
        {
            isWorking = true;

            while (isWorking)
            {
                var input = RequestInput("Введите команду");

                try
                {
                    consoleCommander.Execute(input);
                }
                catch (CommandException e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                }
            }
        }

        /*private void SaveSchoolClass(List<string> args)
        {
            var id = RequestInput("Введите ID");
            var classNumber = RequestInput("Введите номер");
            var letter = RequestInput("Введите букву");

            var schoolClass = new SchoolClass
            {
                Id = id,
                Letter = letter,
                ClassNumber = int.Parse(classNumber)
            };

            scheduleService.Save(schoolClass);
        }*/

        private static string RequestInput(string requestText)
        {
            Console.WriteLine($"{requestText}:");
            Console.Write("->");
            return Console.ReadLine();
        }
    }
}
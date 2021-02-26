using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Entity;
using SchoolSchedule.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolScheduleApp
{
    internal class ScheduleConsole
    {
        private bool isWorking;

        private readonly ScheduleService scheduleService;

        private static readonly string LESSON = "lesson";
        private static readonly string CLASS = "class";

        private readonly Dictionary<string, Action<List<string>>> saveMap;

        private static readonly Dictionary<string, Action<List<string>>> getMap =
            new Dictionary<string, Action<List<string>>>()
            {
            };

        private static readonly Dictionary<string, Action<List<string>>> removeMap =
            new Dictionary<string, Action<List<string>>>()
            {
            };

        private readonly Dictionary<string, Dictionary<string, Action<List<string>>>> keysMap;

        public ScheduleConsole()
        {
            scheduleService = ScheduleService.Create(
                b => b.UseNpgsql("host=localhost;database=sched4;username=schedule;password=schedule;"));

            saveMap = new Dictionary<string, Action<List<string>>>()
            {
                [CLASS] = SaveSchoolClass
            };

            keysMap = new Dictionary<string, Dictionary<string, Action<List<string>>>>()
            {
                ["save"] = saveMap,
                ["get"] = getMap,
                ["remove"] = removeMap,

                ["exit"] = new Dictionary<string, Action<List<string>>>() { ["do"] = args => isWorking = false }
            };
        }

        public void StartConsoleCycle()
        {
            isWorking = true;

            while (isWorking)
            {
                Console.Write("Введите команду: ");
                var input = Console.ReadLine();
                var parts = input.Split(' ')
                    .Where(p => p.Any())
                    .ToList();

                if (!parts.Any()) continue;

                var key = parts.First().ToLower();

                if (!keysMap.ContainsKey(key))
                {
                    Console.WriteLine($"Неизвестный ключ '{key}' в команде '{input}'");
                    continue;
                }

                var actionMap = keysMap[key];
                var args = parts.Skip(1);

                if (!args.Any())
                {
                    actionMap.Values.First()(new List<string>());
                    continue;
                }
                else
                {
                    var subKey = args.First();

                    if (!actionMap.ContainsKey(subKey))
                    {
                        Console.WriteLine($"Неизвестный второй ключ '{subKey}' в команде '{input}'");
                        continue;
                    }

                    actionMap[subKey](args.Skip(1).ToList());
                }
            }
        }

        private void SaveSchoolClass(List<string> args)
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
        }

        private string RequestInput(string requestText)
        {
            Console.WriteLine(requestText);
            Console.Write("-->");
            return Console.ReadLine();
        }
    }
}
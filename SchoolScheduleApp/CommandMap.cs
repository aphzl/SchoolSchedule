using SchoolScheduleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using static SchoolScheduleApp.ConsoleCommander;

namespace SchoolScheduleApp
{
    class CommandMap<CommandDelegate> where CommandDelegate : class
    {
        private readonly Dictionary<string, CommandDelegate> map = new Dictionary<string, CommandDelegate>();

        private static readonly string HELP = "help";

        public CommandMap()
        {
        }

        public CommandDelegate this[string command]
        {
            get
            {
                var cmd = command.ToLower();
                if (cmd == HELP.ToLower())
                {
                    foreach (var k in map.Keys)
                        Console.WriteLine(k);

                    return null;
                }

                if (map.ContainsKey(cmd)) return map[cmd];
                else throw new CommandException($"Неизвестная команда '{command.ToLower()}'");
            }
            set
            {
                var cmd = command.ToLower();
                if (cmd == HELP.ToLower()) throw new Exception($"command '{HELP}' reserved");
                map[command.ToLower()] = value;
            }
        }
    }
}

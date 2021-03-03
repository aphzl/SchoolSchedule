using SchoolSchedule.Service;
using System.Linq;

namespace SchoolScheduleApp
{
    class ConsoleCommander
    {
        public delegate void Command(params string[] args);
        public delegate void MainCommand(params string[] args);

        private readonly ScheduleService service;

        private readonly CommandMap<MainCommand> mainCommandsMap;
        private readonly CommandMap<Command> createCommandsMap;
        private readonly CommandMap<Command> updateCommandsMap;
        private readonly CommandMap<Command> getCommandsMap;
        private readonly CommandMap<Command> deleteCommandsMap;

        public ConsoleCommander(ScheduleService service)
        {
            this.service = service;

            mainCommandsMap = new CommandMap<MainCommand>
            {
                ["create"] = Create,
                ["update"] = Update,
                ["get"] = Get,
                ["delete"] = Delete
            };

            createCommandsMap = new CommandMap<Command>
            {
                ["demo"] = CreateDemo,
                ["student"] = CreateStudent
            };

            updateCommandsMap = new CommandMap<Command>
            {

            };

            getCommandsMap = new CommandMap<Command>
            {

            };

            deleteCommandsMap = new CommandMap<Command>
            {

            };

        }

        public void Execute(string line)
            => ExecuteMain(
                line.Split(' ')
                    .Where(p => p != "")
                    .Select(p => p.Trim().ToLower())
                    .ToArray()
            );

        private void ExecuteMain(params string[] prms)
        {
            if (prms.Length == 0) return;

            var cmd = mainCommandsMap[prms[0]];
            if (cmd == null) return;
            cmd(prms.Skip(1).ToArray());
        }

        private void Execute(CommandMap<Command> map, params string[] prms)
        {
            if (prms.Length == 0) return;

            var cmd = map[prms[0]];
            if (cmd == null) return;
            cmd(prms.Skip(1).ToArray());
        }

        private MainCommand Create => args => Execute(createCommandsMap, args);

        private MainCommand Update => args => Execute(updateCommandsMap, args);

        private MainCommand Get => args => Execute(getCommandsMap, args);

        private MainCommand Delete => args => Execute(deleteCommandsMap, args);

        private Command CreateDemo = args =>
        {

        };

        private Command CreateStudent = args =>
        {

        };
    }
}

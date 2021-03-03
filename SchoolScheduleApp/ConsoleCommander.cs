using SchoolSchedule.Model.Entity;
using SchoolSchedule.Service;
using SchoolScheduleApp.Exceptions;
using System;
using System.Linq;

namespace SchoolScheduleApp
{
    class ConsoleCommander
    {
        public delegate void Command(params string[] args);
        public delegate void MainCommand(params string[] args);

        private readonly CommandMap<MainCommand> mainCommandsMap;
        private readonly CommandMap<Command> createCommandsMap;
        private readonly CommandMap<Command> updateCommandsMap;
        private readonly CommandMap<Command> getCommandsMap;
        private readonly CommandMap<Command> deleteCommandsMap;

        private const string CREATE = "create";
        private const string UPDATE = "update";
        private const string GET = "get";
        private const string DELETE = "delete";

        private const string DEMO = "demo";
        private const string STUDENT= "student";
        private const string CLASS = "class";
        private const string LESSON = "lesson";
        private const string TEACHER = "teacher";
        private const string TEACHER_LESSON = "teacherlesson";
        private const string EXERCISE = "exercise";

        private const string STUDENT_SHEDULE = "studentschedule";
        private const string TEACHER_SCHEDULE = "tacherschedule";
        private const string CLASS_SCHEDULE = "classschedule";
        private const string FULL_SCHEDULE = "classschedule";

        public ConsoleCommander(
            ScheduleService service,
            Func<string, string> requestInput,
            Action<string> write,
            Action<string> writeln)
        {
            mainCommandsMap = new CommandMap<MainCommand>
            {
                [CREATE] = Create,
                [UPDATE] = Update,
                [GET] = Get,
                [DELETE] = Delete
            };

            createCommandsMap = new CommandMap<Command>
            {
                [DEMO] = CreateDemo,
                [STUDENT] = CreateStudent(service, requestInput),
                [CLASS] = CreateClass(service, requestInput),
                [LESSON] = CreateLesson(service, requestInput),
                [TEACHER] = CreateTeacher(service, requestInput),
                [TEACHER_LESSON] = CreateTeacherLesson(service, requestInput),
                [EXERCISE] = CreateExercise(service, requestInput)
            };

            updateCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = UpdateStudent,
                [CLASS] = UpdateClass,
                [LESSON] = UpdateLesson,
                [TEACHER] = UpdateTeacher,
                [TEACHER_LESSON] = UpdateTeacherLesson,
                [EXERCISE] = UpdateExercise
            };

            getCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = GetStudent,
                [CLASS] = GetClass,
                [LESSON] = GetLesson,
                [TEACHER] = GetTeacher,
                [TEACHER_LESSON] = GetTeacherLesson,
                [EXERCISE] = GetExercise,
                [STUDENT_SHEDULE] = GetStudentSchedule,
                [TEACHER_SCHEDULE] = GetTeacherSchedule,
                [CLASS_SCHEDULE] = GetClassSchedule,
                [FULL_SCHEDULE] = GetFullSchedule,
            };

            deleteCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = DeleteStudent,
                [CLASS] = DeleteClass,
                [LESSON] = DeleteLesson,
                [TEACHER] = DeleteTeacher,
                [TEACHER_LESSON] = DeleteTeacherLesson,
                [EXERCISE] = DeleteExercise
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

        private Command CreateStudent(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var firstName = requestInput("Введите имя");
                var midName = requestInput("Введите отчество");
                var lastName = requestInput("Введите фамилию");
                var classId = requestInput("Введите ID класса");

                SchoolClass schoolClass = null;

                if (classId != "")
                {
                    schoolClass = service.Find((SchoolClass c) => c.Id == classId);

                    if (schoolClass == null) throw new CommandException($"Класс с Id={classId} не найден");
                }

                var student = new Student
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = firstName,
                    MidName = midName,
                    LastName = lastName,
                    SchoolClass = schoolClass
                };

                Save(() => service.Save(student));
            };

        private Command CreateClass(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var numberString = requestInput("Введите номер класса");
                if (!int.TryParse(numberString, out int number))
                    throw new CommandException($"Неверный формат номера класса '{numberString}'");

                var letter = requestInput("Введите букву класса");

                var sClass = new SchoolClass
                {
                    Id = Guid.NewGuid().ToString(),
                    ClassNumber = number,
                    Letter = letter
                };

                Save(() => service.Save(sClass));
            };

        private Command CreateLesson(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var name = requestInput("Введите название урока");

                var lesson = new Lesson
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };

                Save(() => service.Save(lesson));
            };

        private Command CreateTeacher(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var firstName = requestInput("Введите имя");
                var midName = requestInput("Введите отчество");
                var lastName = requestInput("Введите фамилию");

                var teacher = new Teacher
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = firstName,
                    MidName = midName,
                    LastName = lastName
                };

                Save(() => service.Save(teacher));
            };

        private Command CreateTeacherLesson(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var teacherId = requestInput("Введите ID преподавателя");
                var lessonId = requestInput("Введите ID предмета");

                Save(() => service.AssignLessonToTeacher(new Lesson { Id = lessonId }, new Teacher { Id = teacherId }));
            };

        private Command CreateExercise(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var dayStr = requestInput("Введите день недели (число)");
                if (!int.TryParse(dayStr, out int day))
                    throw new CommandException($"Неверный формат числа {dayStr}");

                var numberStr = requestInput("Введите номер занятия");
                if (!int.TryParse(numberStr, out int number))
                    throw new CommandException($"Неверный формат числа {numberStr}");

                var classId = requestInput("Введите ID класса");
                SchoolClass schoolClass = null;

                if (classId != "")
                {
                    schoolClass = service.Find((SchoolClass c) => c.Id == classId);

                    if (schoolClass == null) throw new CommandException($"Класс с Id={classId} не найден");
                }

                var auditoryStr = requestInput("Введите номер аудитории");
                if (!int.TryParse(auditoryStr, out int auditory))
                    throw new CommandException($"Неверный формат числа {auditoryStr}");

                var teacherId = requestInput("Введите ID преподавателя");
                var lessonId = requestInput("Введите ID предмета");

                var exercise = new Exercise
                {
                    Id = Guid.NewGuid().ToString(),
                    DayOfWeek = day,
                    ExerciseNumber = number,
                    SchoolClass = schoolClass,
                    Auditory = auditory,
                    TeacherId = teacherId,
                    LessonId = lessonId
                };

                Save(() => service.Save(exercise));
            };


        private Command UpdateStudent = args =>
        {

        };

        private Command UpdateClass = args =>
        {

        };

        private Command UpdateLesson = args =>
        {

        };

        private Command UpdateTeacher = args =>
        {

        };

        private Command UpdateTeacherLesson = args =>
        {

        };

        private Command UpdateExercise = args =>
        {

        };


        private Command GetStudent = args =>
        {

        };

        private Command GetClass = args =>
        {

        };

        private Command GetLesson = args =>
        {

        };

        private Command GetTeacher = args =>
        {

        };

        private Command GetTeacherLesson = args =>
        {

        };

        private Command GetExercise = args =>
        {

        };

        private Command GetStudentSchedule = args =>
        {

        };

        private Command GetTeacherSchedule = args =>
        {

        };

        private Command GetClassSchedule = args =>
        {

        };

        private Command GetFullSchedule = args =>
        {

        };


        private Command DeleteStudent = args =>
        {

        };

        private Command DeleteClass = args =>
        {

        };

        private Command DeleteLesson = args =>
        {

        };

        private Command DeleteTeacher = args =>
        {

        };

        private Command DeleteTeacherLesson = args =>
        {

        };

        private Command DeleteExercise = args =>
        {

        };

        private static void Save(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw new CommandException("Не удалось сохранить объект");
            }
        }
    }
}

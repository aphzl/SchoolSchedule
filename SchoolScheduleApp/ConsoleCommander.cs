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
        //private readonly CommandMap<Command> updateCommandsMap;
        private readonly CommandMap<Command> getCommandsMap;
        private readonly CommandMap<Command> deleteCommandsMap;

        private const string CREATE = "create";
        private const string UPDATE = "update";
        private const string GET = "get";
        private const string DELETE = "delete";

        private const string DEMO = "demo";
        private const string STUDENT= "student";
        private const string STUDENTS = "students";
        private const string CLASS = "class";
        private const string CLASSES = "classes";
        private const string LESSON = "lesson";
        private const string LESSONS = "lessons";
        private const string TEACHER = "teacher";
        private const string TEACHERS = "teachers";
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
                //[UPDATE] = Update,
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

            /*updateCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = UpdateStudent,
                [CLASS] = UpdateClass,
                [LESSON] = UpdateLesson,
                [TEACHER] = UpdateTeacher,
                [TEACHER_LESSON] = UpdateTeacherLesson,
                [EXERCISE] = UpdateExercise
            };*/

            getCommandsMap = new CommandMap<Command>
            {
                [STUDENTS] = GetStudents(service, writeln),
                [CLASSES] = GetClasses(service, writeln),
                [LESSONS] = GetLessons(service, writeln),
                [TEACHERS] = GetTeachers(service, writeln),
                [STUDENT_SHEDULE] = GetStudentSchedule,
                [TEACHER_SCHEDULE] = GetTeacherSchedule,
                [CLASS_SCHEDULE] = GetClassSchedule,
                [FULL_SCHEDULE] = GetFullSchedule,
            };

            deleteCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = DeleteStudent(service, requestInput),
                [CLASS] = DeleteClass(service, requestInput),
                [LESSON] = DeleteLesson(service, requestInput),
                [TEACHER] = DeleteTeacher(service, requestInput),
                [TEACHER_LESSON] = DeleteTeacherLesson(service, requestInput),
                [EXERCISE] = DeleteExercise(service, requestInput)
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

        //private MainCommand Update => args => Execute(updateCommandsMap, args);

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


        /*private Command UpdateStudent = args =>
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

        };*/


        private Command GetStudents(
            ScheduleService service,
            Action<string> writeln)
            => args =>
            {
                foreach (var student in service.FindStudents())
                {
                    WriteStudentInfo(student, writeln);
                    writeln("");
                }
            };

        private Command GetClasses(
            ScheduleService service,
            Action<string> writeln)
            => args =>
            {
                foreach (var sClass in service.FindClasses())
                {
                    WriteClassInfo(sClass, writeln);
                    writeln("");
                }
            };

        private Command GetLessons(
            ScheduleService service,
            Action<string> writeln)
            => args =>
            {
                foreach (var lesson in service.FIndLessons())
                {
                    WriteLessonsInfo(lesson, writeln, true);
                    writeln("");
                }
            };

        private Command GetTeachers(
            ScheduleService service,
            Action<string> writeln)
            => args =>
            {
            foreach (var teacher in service.FindTeachers())
            {
                WriteTeacherInfo(teacher, writeln, true);
                writeln("");
            }
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


        private Command DeleteStudent(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var id = requestInput("Введите ID ученика для удаления");
                Del(() => service.Delete(new Student { Id = id }));
            };

        private Command DeleteClass(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var id = requestInput("Введите ID класса для удаления");
                Del(() => service.Delete(new SchoolClass { Id = id }));
            };

        private Command DeleteLesson(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var id = requestInput("Введите ID класса для удаления");
                Del(() => service.Delete(new SchoolClass { Id = id }));
            };

        private Command DeleteTeacher(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var id = requestInput("Введите ID преподавателя для удаления");
                Del(() => service.Delete(new Teacher { Id = id }));
            };

        private Command DeleteTeacherLesson(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var teacherId = requestInput("Введите ID преподавателя");
                var lessonId = requestInput("Введите ID предмета");

                var teacherLesson = new TeacherLesson
                {
                    Teacher = new Teacher { Id = teacherId },
                    Lesson = new Lesson { Id = lessonId }
                };

                Del(() => service.Delete(teacherLesson));
            };

        private Command DeleteExercise(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var id = requestInput("Введите ID удаляемого занятия");
                Del(() => service.Delete(new Exercise { Id = id }));
            };

        private static void Save(Action action) => DoWithHandleException(action, "Не удалось сохранить объект");

        private static void Del(Action action) => DoWithHandleException(action, "Не удалось удалить объект");

        private static void DoWithHandleException(Action action, string message)
        {
            try
            {
                action();
            }
            catch
            {
                throw new CommandException(message);
            }
        }

        private static void WriteStudentInfo(
            Student student, Action<string> writeln)
        {
            writeln("---Информация о студенте---");
            writeln($"ID: {student.Id}");
            writeln($"Имя: {student.FirstName}");
            writeln($"Отчество: {student.MidName}");
            writeln($"Фамилия: {student.LastName}");

            
            if (student.SchoolClass != null)
            {
                WriteClassInfo(student.SchoolClass, writeln);
            }
        }

        private static void WriteClassInfo(
            SchoolClass sClass, Action<string> writeln)
        {
            writeln("---Информация о классе---");
            writeln($"ID касса: {sClass.Id}");
            writeln($"Номер: {sClass.ClassNumber}");
            writeln($"Буква: {sClass.Letter}");
        }

        private static void WriteLessonsInfo(Lesson lesson, Action<string> writeln, bool writeTeacherInfo)
        {
            writeln("---Информация о предмете---");
            writeln($"ID предмета: {lesson.Id}");
            writeln($"Название: {lesson.Name}");

            if (writeTeacherInfo)
            {
                foreach (var teacher in lesson.Teachers)
                {
                    WriteTeacherInfo(teacher, writeln, false);
                }
            }
        }

        private static void WriteTeacherInfo(Teacher teacher, Action<string> writeln, bool wtiteLessonsInfo)
        {
            writeln("---Информация о преподавателе---");
            writeln($"ID: {teacher.Id}");
            writeln($"Имя: {teacher.FirstName}");
            writeln($"Отчество: {teacher.MidName}");
            writeln($"Фамилия: {teacher.LastName}");

            if (wtiteLessonsInfo)
            {
                foreach (var lesson in teacher.Lessons)
                {
                    WriteLessonsInfo(lesson, writeln, false);
                }
            }
        }
    }
}

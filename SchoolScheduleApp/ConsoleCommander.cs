using SchoolSchedule.Model.Dto;
using SchoolSchedule.Model.Entity;
using SchoolSchedule.Service;
using SchoolScheduleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolScheduleApp
{
    class ConsoleCommander
    {
        public delegate void Command(params string[] args);
        public delegate void MainCommand(params string[] args);

        private readonly CommandMap<MainCommand> mainCommandsMap;
        private readonly CommandMap<Command> createCommandsMap;
        private readonly CommandMap<Command> getCommandsMap;
        private readonly CommandMap<Command> deleteCommandsMap;

        private const string CREATE = "create";
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
        private const string ALL = "all";

        private const string TEACHER_SCHEDULE = "teacherschedule";
        private const string CLASS_SCHEDULE = "classschedule";
        private const string FULL_SCHEDULE = "fullschedule";

        private static readonly Dictionary<int, string> days = new Dictionary<int, string>
        {
            [1] = "Понедельник",
            [2] = "Вторник",
            [3] = "Среда",
            [4] = "Четверг",
            [5] = "Пятница",
            [6] = "Суббота",
        };

        public ConsoleCommander(
            ScheduleService service,
            Func<string, string> requestInput,
            Action<string> writeln)
        {
            mainCommandsMap = new CommandMap<MainCommand>
            {
                [CREATE] = Create,
                [GET] = Get,
                [DELETE] = Delete
            };

            createCommandsMap = new CommandMap<Command>
            {
                [DEMO] = CreateDemo(service, requestInput, writeln),
                [STUDENT] = CreateStudent(service, requestInput),
                [CLASS] = CreateClass(service, requestInput),
                [LESSON] = CreateLesson(service, requestInput),
                [TEACHER] = CreateTeacher(service, requestInput),
                [TEACHER_LESSON] = CreateTeacherLesson(service, requestInput),
                [EXERCISE] = CreateExercise(service, requestInput)
            };

            getCommandsMap = new CommandMap<Command>
            {
                [STUDENTS] = GetStudents(service, writeln),
                [CLASSES] = GetClasses(service, writeln),
                [LESSONS] = GetLessons(service, writeln),
                [TEACHERS] = GetTeachers(service, writeln),
                [TEACHER_SCHEDULE] = GetTeacherSchedule(service, requestInput, writeln),
                [CLASS_SCHEDULE] = GetClassSchedule(service, requestInput, writeln),
                [FULL_SCHEDULE] = GetFullSchedule(service, writeln),
            };

            deleteCommandsMap = new CommandMap<Command>
            {
                [STUDENT] = DeleteStudent(service, requestInput),
                [CLASS] = DeleteClass(service, requestInput),
                [LESSON] = DeleteLesson(service, requestInput),
                [TEACHER] = DeleteTeacher(service, requestInput),
                [TEACHER_LESSON] = DeleteTeacherLesson(service, requestInput),
                [EXERCISE] = DeleteExercise(service, requestInput),
                [ALL] = DeleteAll(service, requestInput)
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

        private MainCommand Get => args => Execute(getCommandsMap, args);

        private MainCommand Delete => args => Execute(deleteCommandsMap, args);

        private Command CreateDemo(
            ScheduleService service,
            Func<string, string> requestInput,
            Action<string> writeln)
            => args =>
            {
                writeln("Перед созданием демо-базы будет очищена существующая.");
                var answer = requestInput("Выполнить? (да/нет)");

                if (answer.ToLower() != "да") return;

                DoWithHandleException(() => service.CleanDb(), "Не удалось очистить базу");
                DoWithHandleException(() => service.CreateDemoBase(), "Не удалось создать демо-базу");
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

        private Command GetTeacherSchedule(
            ScheduleService service,
            Func<string, string> requestInput,
            Action<string> writeln)
            => args =>
            {
                var firstName = requestInput("Введите имя");
                var midName = requestInput("Введите отчество");
                var lastName = requestInput("Введите фамилию");

                var schedule = service.GetTeacherWeekSchedule(firstName, midName, lastName);

                WriteScheduleInfo(schedule, writeln);
            };

        private Command GetClassSchedule(
            ScheduleService service,
            Func<string, string> requestInput,
            Action<string> writeln)
            => args =>
            {
                var numberString = requestInput("Введите номер класса");
                if (!int.TryParse(numberString, out int number))
                    throw new CommandException($"Неверный формат номера класса '{numberString}'");

                var letter = requestInput("Введите букву класса");

                var schedule = service.GetClassWeekSchedule(number, letter);

                WriteScheduleInfo(schedule, writeln);
            };

        private Command GetFullSchedule(
            ScheduleService service,
            Action<string> writeln)
            => args =>
            {
                var schedule = service.GetFullWeekSchedule();
                WriteScheduleInfo(schedule, writeln);
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

        private Command DeleteAll(
            ScheduleService service,
            Func<string, string> requestInput)
            => args =>
            {
                var answer = requestInput("Это действие очистит БД\nПродолжить? (да/нет)");

                if (answer.ToLower() != "да") return;

                DoWithHandleException(() => service.CleanDb(), "Не удалось очистить базу");
            };

        private static void Save(Action action) => DoWithHandleException(action, "Не удалось сохранить объект");

        private static void Del(Action action) => DoWithHandleException(action, "Не удалось удалить объект");

        private static void DoWithHandleException(Action action, string message)
        {
            try
            {
                action();
            }
            catch (Exception e)
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

        private static void WriteScheduleInfo(WeekSchedule schedule, Action<string> writeln)
        {
            if (schedule == null) return;

            var list = schedule.ScheduleByDay
                .OrderBy(s => s.Key)
                .ToList();

            foreach (var s in list)
            {
                WriteDayScheduleInfo(s.Value, writeln, (int) s.Key);
                writeln("--");
            }
        }

        private static void WriteDayScheduleInfo(DaySchedule schedule, Action<string> writeln, int day)
        {
            var exercises = schedule.ExerciseByLessonNumber
                .OrderBy(e => e.Key)
                .ToList();

            writeln($"---{days[day]}---");

            foreach (var pair in exercises)
            {
                var list = pair.Value
                    .OrderBy(e => e.SchoolClass.Id)
                    .ToList();

                writeln($"---Занятие №{pair.Key}");

                foreach (var e in list)
                {
                    WriteExerciseInfo(e, writeln);
                    writeln("-");
                }

                writeln("-");
            }
        }

        private static void WriteExerciseInfo(Exercise exercise, Action<string> writeln)
        {
            writeln($"---Информация о занятии---");
            writeln($"ID: {exercise.Id}");
            writeln($"Аудитория: {exercise.Auditory}");
            if (exercise.SchoolClass != null) WriteClassInfo(exercise.SchoolClass, writeln);
            if (exercise.Teacher != null) WriteTeacherInfo(exercise.Teacher, writeln, false);
            if (exercise.Lesson != null) WriteLessonsInfo(exercise.Lesson, writeln, false);
        }
    }
}

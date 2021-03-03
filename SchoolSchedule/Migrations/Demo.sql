INSERT INTO lesson VALUES ('2d1fb086-9035-474f-969b-e9075226e572', 'Русский язык');
INSERT INTO lesson VALUES ('36992aab-0fb2-47b4-b708-fd940e29385a', 'Математика');
INSERT INTO lesson VALUES ('6069ce14-a0bb-4013-88a9-003aedb0b14c', 'Литература');
INSERT INTO lesson VALUES ('46502add-4192-4818-a513-a7af766bccee', 'История');
INSERT INTO lesson VALUES ('64f88e58-415f-458c-9b68-67dbb46c4acc', 'География');


INSERT INTO teacher VALUES ('0320ca05-c65d-42f8-98da-05c64e49a038', 'Надежда', 'Ивановна', 'Петрова');
INSERT INTO teacher VALUES ('55090f8f-405f-42ab-949b-48bea0c9d8fc', 'Людмила', 'Юрьевна', 'Афанасьева');
INSERT INTO teacher VALUES ('59451862-c09e-4df9-8f64-ba83794e4664', 'Николай', 'Михвйлович', 'Семенов');
INSERT INTO teacher VALUES ('c5d1c514-75e4-4cab-a786-b35609796266', 'Анна', 'Юрьевна', 'Кудряшева');


INSERT INTO teacher_lesson VALUES ('0320ca05-c65d-42f8-98da-05c64e49a038', '2d1fb086-9035-474f-969b-e9075226e572');
INSERT INTO teacher_lesson VALUES ('0320ca05-c65d-42f8-98da-05c64e49a038', '6069ce14-a0bb-4013-88a9-003aedb0b14c');
INSERT INTO teacher_lesson VALUES ('55090f8f-405f-42ab-949b-48bea0c9d8fc', '36992aab-0fb2-47b4-b708-fd940e29385a');
INSERT INTO teacher_lesson VALUES ('59451862-c09e-4df9-8f64-ba83794e4664', '46502add-4192-4818-a513-a7af766bccee');
INSERT INTO teacher_lesson VALUES ('c5d1c514-75e4-4cab-a786-b35609796266', '64f88e58-415f-458c-9b68-67dbb46c4acc');


INSERT INTO school_class VALUES ('af054b01-fae2-4d10-b6a1-12a96354ce52', '5', 'А');
INSERT INTO school_class VALUES ('1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '5', 'Б');
INSERT INTO school_class VALUES ('846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '7', 'А');


INSERT INTO student VALUES (uuid(), 'Петр', 'Петрович', 'Агапов', 'af054b01-fae2-4d10-b6a1-12a96354ce52');
INSERT INTO student VALUES (uuid(), 'Михаил', 'Александрович', 'Нестеров', 'af054b01-fae2-4d10-b6a1-12a96354ce52');
INSERT INTO student VALUES (uuid(), 'Павел', 'Иванович', 'Ушаков', 'af054b01-fae2-4d10-b6a1-12a96354ce52');
INSERT INTO student VALUES (uuid(), 'Александр', 'Андреевич', 'Пушкин', 'af054b01-fae2-4d10-b6a1-12a96354ce52');
INSERT INTO student VALUES (uuid(), 'Никита', 'Сергеевич', 'Палкин', 'af054b01-fae2-4d10-b6a1-12a96354ce52');
INSERT INTO student VALUES (uuid(), 'Анна', 'Ивановна', 'Михалкова', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Людмила', 'Игнатьевна', 'Столова', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Антон', 'Александрович', 'Лукашев', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Юлия', 'Ивановна', 'Порошкова', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Дмитрий', 'Петрович', 'Дорогов', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Алексей', 'Григорьевич', 'Гончаров', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd');
INSERT INTO student VALUES (uuid(), 'Петр', 'Кириллович', 'Высоцкий', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Анастасия', 'Александровна', 'Долговая', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Анна', 'Петровна', 'Иконова', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Надежда', 'Сергеевна', 'Бабкина', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Григорий', 'Федорович', 'Полещук', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Федор', 'Михайлович', 'Озерный', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');
INSERT INTO student VALUES (uuid(), 'Руслан', 'Маратович', 'Николаев', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338');


INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '1', '1', '223');
INSERT INTO exercise VALUES (uuid(), '6069ce14-a0bb-4013-88a9-003aedb0b14c', '0320ca05-c65d-42f8-98da-05c64e49a038', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '1', '2', '222');
INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '1', '3', '456');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '2', '1', '223');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '2', '2', '111');
INSERT INTO exercise VALUES (uuid(), '6069ce14-a0bb-4013-88a9-003aedb0b14c', '0320ca05-c65d-42f8-98da-05c64e49a038', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '2', '3', '222');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '3', '2', '333');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '3', '3', '423');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '4', '1', '123');
INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '4', '2', '111');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '4', '3', '432');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', 'af054b01-fae2-4d10-b6a1-12a96354ce52', '5', '1', '256');

INSERT INTO exercise VALUES (uuid(), '6069ce14-a0bb-4013-88a9-003aedb0b14c', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '1', '1', '321');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '1', '2', '213');
INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '1', '3', '245');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '2', '1', '267');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '2', '2', '432');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '2', '3', '387');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '3', '1', '955');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '3', '2', '567');
INSERT INTO exercise VALUES (uuid(), '6069ce14-a0bb-4013-88a9-003aedb0b14c', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '3', '3', '432');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '4', '1', '122');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '5', '1', '987');
INSERT INTO exercise VALUES (uuid(), '6069ce14-a0bb-4013-88a9-003aedb0b14c', '0320ca05-c65d-42f8-98da-05c64e49a038', '1fc2fea0-e2d5-4e60-a112-d0548c9831dd', '5', '2', '737');

INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '1', '1', '676');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '1', '2', '666');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '2', '1', '777');
INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '2', '2', '425');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '2', '3', '563');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '3', '1', '234');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '3', '2', '745');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '3', '3', '863');
INSERT INTO exercise VALUES (uuid(), '36992aab-0fb2-47b4-b708-fd940e29385a', '55090f8f-405f-42ab-949b-48bea0c9d8fc', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '4', '1', '303');
INSERT INTO exercise VALUES (uuid(), '46502add-4192-4818-a513-a7af766bccee', '59451862-c09e-4df9-8f64-ba83794e4664', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '5', '1', '456');
INSERT INTO exercise VALUES (uuid(), '64f88e58-415f-458c-9b68-67dbb46c4acc', 'c5d1c514-75e4-4cab-a786-b35609796266', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '5', '2', '732');
INSERT INTO exercise VALUES (uuid(), '2d1fb086-9035-474f-969b-e9075226e572', '0320ca05-c65d-42f8-98da-05c64e49a038', '846b31e4-f6a6-4e41-9473-b4ccf5d5a338', '5', '3', '334');
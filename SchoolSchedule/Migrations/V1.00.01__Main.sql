CREATE TABLE exercise (
	id varchar NOT NULL,
	lesson_id varchar,
	teacher_id varchar,
	day_of_week numeric,
	exercise_number numeric,
	auditory numeric
);

CREATE TABLE lesson (
	id varchar NOT NULL,
	name varchar NOT NULL
);

CREATE TABLE school_class (
	id varchar NOT NULL,
	class_number numeric NOT NULL,
	letter varchar NOT NULL
);

CREATE TABLE student (
	id varchar NOT NULL,
	first_name varchar NOT NULL,
	mid_name varchar NOT NULL,
	last_name varchar NOT NULL,
	school_class_id varchar
);

CREATE TABLE teacher (
	id varchar NOT NULL,
	first_name varchar NOT NULL,
	mid_name varchar NOT NULL,
	last_name varchar NOT NULL
);

CREATE TABLE teacher_lesson (
	teacher_id varchar NOT NULL,
	lesson_id varchar NOT NULL
);

ALTER TABLE exercise ADD CONSTRAINT exercise_id_pkey PRIMARY KEY (id);
ALTER TABLE lesson ADD CONSTRAINT lesson_id_pkey PRIMARY KEY (id);
ALTER TABLE school_class ADD CONSTRAINT school_classe_id_pkey PRIMARY KEY (id);
ALTER TABLE school_class ADD CONSTRAINT school_classe_number_letter_ukey UNIQUE (class_number, letter);
ALTER TABLE student ADD CONSTRAINT student_id_pkey PRIMARY KEY (id);
ALTER TABLE teacher ADD CONSTRAINT teacher_id_pkey PRIMARY KEY (id);
ALTER TABLE teacher_lesson ADD CONSTRAINT teacher_lesson_id_pkey PRIMARY KEY (teacher_id, lesson_id);

ALTER TABLE exercise
	ADD CONSTRAINT exercise_lesson_fkey
	FOREIGN KEY (lesson_id) REFERENCES lesson(id);

ALTER TABLE exercise
	ADD CONSTRAINT exercise_teacher_fkey
	FOREIGN KEY (teacher_id) REFERENCES teacher(id);

ALTER TABLE student
	ADD CONSTRAINT student_school_class_fkey
	FOREIGN KEY (school_class_id) REFERENCES school_class(id);

ALTER TABLE teacher_lesson
	ADD CONSTRAINT teacher_lesson_teacher_fkey
	FOREIGN KEY (teacher_id) REFERENCES teacher(id) ON DELETE CASCADE;

ALTER TABLE teacher_lesson
	ADD CONSTRAINT teacher_lesson_lesson_fkey
	FOREIGN KEY (lesson_id) REFERENCES lesson(id) ON DELETE CASCADE;
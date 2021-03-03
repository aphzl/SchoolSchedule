CREATE TABLE lesson (
	id varchar NOT NULL PRIMARY KEY,
	name varchar NOT NULL
);

CREATE TABLE teacher (
	id varchar NOT NULL PRIMARY KEY,
	first_name varchar NOT NULL,
	mid_name varchar NOT NULL,
	last_name varchar NOT NULL
);

CREATE TABLE teacher_lesson (
	teacher_id varchar NOT NULL,
	lesson_id varchar NOT NULL,
	PRIMARY KEY (teacher_id, lesson_id),
	CONSTRAINT teacher_id_fkey FOREIGN KEY (teacher_id) REFERENCES teacher(id) ON DELETE CASCADE,
	CONSTRAINT lesson_id_fkey FOREIGN KEY (lesson_id) REFERENCES lesson(id) ON DELETE CASCADE
);

CREATE TABLE school_class (
	id varchar NOT NULL PRIMARY KEY,
	class_number numeric NOT NULL,
	letter varchar NOT NULL,
	UNIQUE (class_number, letter)
);

CREATE TABLE exercise (
	id varchar NOT NULL PRIMARY KEY,
	lesson_id varchar,
	teacher_id varchar,
	school_class_id varchar,
	day_of_week numeric,
	exercise_number numeric,
	auditory numeric,
	CONSTRAINT teacher_id_lesson_id_fkey FOREIGN KEY (teacher_id, lesson_id) REFERENCES teacher_lesson(teacher_id, lesson_id),
	CONSTRAINT school_class_id_fkey FOREIGN KEY (school_class_id) REFERENCES school_class(id)
);

CREATE TABLE student (
	id varchar NOT NULL PRIMARY KEY,
	first_name varchar NOT NULL,
	mid_name varchar NOT NULL,
	last_name varchar NOT NULL,
	school_class_id varchar,
	FOREIGN KEY (school_class_id) REFERENCES school_class(id)
);
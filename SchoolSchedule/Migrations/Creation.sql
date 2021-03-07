CREATE TABLE lesson (
	id varchar NOT NULL PRIMARY KEY,
	name varchar(30) NOT NULL
);
CREATE INDEX lesson_pk_idx ON lesson (id);

CREATE TABLE teacher (
	id varchar NOT NULL PRIMARY KEY,
	first_name varchar(30) NOT NULL,
	mid_name varchar(30) NOT NULL,
	last_name varchar(30) NOT NULL
);
CREATE INDEX teacher_pk_idx ON teacher (id);
CREATE INDEX teacher_first_name_idx ON teacher (first_name);
CREATE INDEX teacher_mid_name_idx ON teacher (mid_name);
CREATE INDEX teacher_last_name_idx ON teacher (last_name);

CREATE TABLE teacher_lesson (
	teacher_id varchar NOT NULL,
	lesson_id varchar NOT NULL,
	PRIMARY KEY (teacher_id, lesson_id),
	CONSTRAINT teacher_id_fkey FOREIGN KEY (teacher_id) REFERENCES teacher(id) ON DELETE CASCADE,
	CONSTRAINT lesson_id_fkey FOREIGN KEY (lesson_id) REFERENCES lesson(id) ON DELETE CASCADE
);
CREATE INDEX teacher_id_idx ON teacher_lesson (teacher_id);
CREATE INDEX lesson_id_idx ON teacher_lesson (lesson_id);

CREATE TABLE school_class (
	id varchar NOT NULL PRIMARY KEY,
	class_number smallint NOT NULL,
	letter varchar(1) NOT NULL,
	UNIQUE (class_number, letter)
);
CREATE INDEX school_class_pk_idx ON school_class (id);

CREATE TABLE exercise (
	id varchar NOT NULL PRIMARY KEY,
	lesson_id varchar,
	teacher_id varchar,
	school_class_id varchar,
	day_of_week smallint,
	exercise_number smallint,
	auditory smallint,
	CONSTRAINT teacher_id_lesson_id_fkey FOREIGN KEY (teacher_id, lesson_id) REFERENCES teacher_lesson(teacher_id, lesson_id),
	CONSTRAINT school_class_id_fkey FOREIGN KEY (school_class_id) REFERENCES school_class(id)
);

CREATE TABLE student (
	id varchar NOT NULL PRIMARY KEY,
	first_name varchar(30) NOT NULL,
	mid_name varchar(30) NOT NULL,
	last_name varchar(30) NOT NULL,
	school_class_id varchar,
	FOREIGN KEY (school_class_id) REFERENCES school_class(id)
);
CREATE INDEX student_first_name_idx ON student (first_name);
CREATE INDEX student_mid_name_idx ON student (mid_name);
CREATE INDEX student_last_name_idx ON student (last_name);
CREATE INDEX school_class_id_idx ON student (school_class_id);

CREATE TABLE migration_schema (name varchar NOT NULL PRIMARY KEY);

INSERT INTO migration_schema VALUES ('V1_00_01__Main');
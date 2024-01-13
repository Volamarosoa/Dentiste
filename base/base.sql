create database dentiste;
\c dentiste;

create table type (
    id int primary key,
    type varchar(50)
);

create table dents (
    id int primary key,
    nom varchar(30),
    type int references type(id),
    etat int references type(id)
);

create table prix (
    id serial,
    date date,
    idDent int references dents(id),
    traitement double precision default 0,
    remplacement double precision default 0
);

drop table controle;

create table patients (
    id varchar(10) primary key,
    nom varchar(50),
    prenom varchar(50),
    date_naissance date,
    idGenre int,
    date date
);

CREATE SEQUENCE seqPatient
increment by 1
start WITH 1
minValue 1;

create or replace function nextID(sequence text, prefix text, taille integer) 
returns text AS
$$
    Declare
        nextId int;
        nextIdString text;
        id text;
        i integer;
    BEGIN
        SELECT coalesce(nextval(sequence),1) into nextId;
        nextIdString := nextId::varchar;
        taille := taille - LENGTH(prefix) - LENGTH(nextIdString);
        id := prefix;
        FOR i IN 1..taille LOOP
            id := id || '0'; 
        END LOOP;
        id := id || nextIdString; 
        return id;
    END
$$ LANGUAGE plpgsql;

CREATE SEQUENCE seqProbleme
increment by 1
start WITH 1
minValue 1;

CREATE SEQUENCE seqControle
increment by 1
start WITH 1
minValue 1;

create table probleme_dentaire (
    id serial primary key,
    date date,
    numero int,
    idPatient varchar(10) references patients(id),
    idDent int references dents(id),
    probleme int references type(id),
    etat int default 0
);

create table controle (
    id serial,
    idProbleme int references probleme_dentaire(id)
);


insert into type values
(1, 'Droite'),
(2, 'Gauche'),
(10, 'Haut'),
(20, 'Bas');

insert into type values
(25, 'Traitememt'),
(30, 'Remplacemnet');

insert into dents values 
(11, 'Inscisive centrale', 1, 10),
(12, 'Inscisive laterale', 1, 10),
(13, 'Canine', 1, 10),
(14, 'Premiere premolaire', 1, 10),
(15, 'Seconde premolaire', 1, 10),
(16, 'Premiere molaire', 1, 10),
(17, 'Seconde molaire', 1, 10),
(18, 'Troisime molaire', 1, 10),

(21, 'Inscisive centrale', 2, 10),
(22, 'Inscisive laterale', 2, 10),
(23, 'Canine', 2, 10),
(24, 'Premiere premolaire', 2, 10),
(25, 'Seconde premolaire', 2, 10),
(26, 'Premiere molaire', 2, 10),
(27, 'Seconde molaire', 2, 10),
(28, 'Troisime molaire', 2, 10),

(41, 'Inscisive centrale', 1, 20),
(42, 'Inscisive laterale', 1, 20),
(43, 'Canine', 1, 20),
(44, 'Premiere premolaire', 1, 20),
(45, 'Seconde premolaire', 1, 20),
(46, 'Premiere molaire', 1, 20),
(47, 'Seconde molaire', 1, 20),
(48, 'Troisime molaire', 1, 20),

(31, 'Inscisive centrale', 2, 20),
(32, 'Inscisive laterale', 2, 20),
(33, 'Canine', 2, 20),
(34, 'Premiere premolaire', 2, 20),
(35, 'Seconde premolaire', 2, 20),
(36, 'Premiere molaire', 2, 20),
(37, 'Seconde molaire', 2, 20),
(38, 'Troisime molaire', 2, 20);

insert into prix (date, idDent, traitement, remplacement) values
('2324-01-04', 11, 7000, 12000),
('2324-01-04', 12, 7000, 12000),
('2324-01-04', 13, 7000, 12000),
('2324-01-04', 14, 8000, 15000),
('2324-01-04', 15, 8500, 13000),
('2324-01-04', 16, 9000, 15000),
('2324-01-04', 17, 9000, 15000),
('2324-01-04', 18, 9000, 15000),

('2324-01-04', 21, 7000, 12000),
('2324-01-04', 22, 7000, 12000),
('2324-01-04', 23, 7000, 12000),
('2324-01-04', 24, 8000, 15000),
('2324-01-04', 25, 8500, 13000),
('2324-01-04', 26, 9000, 15000),
('2324-01-04', 27, 9000, 15000),
('2324-01-04', 28, 9000, 15000),

('2324-01-04', 31, 7000, 12000),
('2324-01-04', 32, 7000, 12000),
('2324-01-04', 33, 7000, 12000),
('2324-01-04', 34, 8000, 15000),
('2324-01-04', 35, 8500, 13000),
('2324-01-04', 36, 9000, 15000),
('2324-01-04', 37, 9000, 15000),
('2324-01-04', 38, 9000, 15000),

('2324-01-04', 41, 7000, 12000),
('2324-01-04', 42, 7000, 12000),
('2324-01-04', 43, 7000, 12000),
('2324-01-04', 44, 8000, 15000),
('2324-01-04', 45, 8500, 13000),
('2324-01-04', 46, 9000, 15000),
('2324-01-04', 47, 9000, 15000),
('2324-01-04', 48, 9000, 15000);
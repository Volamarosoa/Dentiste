create database dentiste;
\c dentiste;

create table type (
    id int primary key,
    type varchar(50)
);

create table etat (
    id int primary key,
    etat varchar(50)
);

create table type_etat (
    id serial,
    idType int references type(id),
    idEtat int references etat(id),
    isValid int default 1
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
    idType int references type(id),
    prix double precision default 0
);

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

create table etat_dentaire (
    id serial primary key,
    date date,
    idPatient varchar(10) references patients (id),
    idDent int references dents (id),
    etat int references etat (id),
    isLast int default 1
);

create table controle_dentaire (
    id serial,
    idEtat_dentaire int references etat_dentaire (id),
    prix double precision default 0
);

SELECT e.id ,date, idpatient, iddent, e.etat, idDent%10 as reste, d.type, d.etat as place 
    FROM Etat_Dentaire e join dents d on e.idDent = d.id where idPatient = 'PT00000003' and isLast = 1 order by idDent asc;

create view liste_etat_dentaire as
SELECT e.id ,date, idpatient, iddent, e.etat, idDent%10 as reste, d.type, d.etat as place 
    FROM Etat_Dentaire e join dents d on e.idDent = d.id where isLast = 1
    Group by idPatient, e.id, date, idDent, e.etat, reste, type, place;

insert into type values
(1, 'Droite'),
(2, 'Gauche'),
(10, 'Haut'),
(20, 'Bas');

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

insert into type values
(25, 'Nettoyage'),
(30, 'Reparation'),
(35, 'Enlevement'),
(40, 'Remplacement');

insert into Etat values
(0, 'Banga'),
(1, 'Potika be'),
(2, 'Potika'),
(3, 'Potipotika'),
(4, 'Simba be'),
(5, 'Simba'),
(6, 'Simba simba'),
(7, 'Maloto be'),
(8, 'Maloto'),
(9, 'Malotoloto'),
(10, 'Tsara be');

insert into type_etat (idType, idEtat) values
(25, 9),
(25, 8),
(25, 7),
(30, 6),
(30, 5),
(30, 4),
(35, 3),
(35, 2),
(35, 1),
(40, 0);



insert into prix (date, idDent, prix, idType) values
('2324-01-04', 11, 12000, 40),
('2324-01-04', 12, 12000, 40),
('2324-01-04', 13, 12000, 40),
('2324-01-04', 14, 15000, 40),
('2324-01-04', 15, 13000, 40),
('2324-01-04', 16, 15000, 40),
('2324-01-04', 17, 15000, 40),
('2324-01-04', 18, 15000, 40),

('2324-01-04', 21, 12000, 40),
('2324-01-04', 22, 12000, 40),
('2324-01-04', 23, 12000, 40),
('2324-01-04', 24, 15000, 40),
('2324-01-04', 25, 13000, 40),
('2324-01-04', 26, 15000, 40),
('2324-01-04', 27, 15000, 40),
('2324-01-04', 28, 15000, 40),

('2324-01-04', 31, 12000, 40),
('2324-01-04', 32, 12000, 40),
('2324-01-04', 33, 12000, 40),
('2324-01-04', 34, 15000, 40),
('2324-01-04', 35, 13000, 40),
('2324-01-04', 36, 15000, 40),
('2324-01-04', 37, 15000, 40),
('2324-01-04', 38, 15000, 40),

('2324-01-04', 41, 12000, 40),
('2324-01-04', 42, 12000, 40),
('2324-01-04', 43, 12000, 40),
('2324-01-04', 44, 15000, 40),
('2324-01-04', 45, 13000, 40),
('2324-01-04', 46, 15000, 40),
('2324-01-04', 47, 15000, 40),
('2324-01-04', 48, 15000, 40);

insert into prix (date, idDent, prix, idType) values
('2324-01-04', 11, 5000, 35),
('2324-01-04', 12, 5000, 35),
('2324-01-04', 13, 5000, 35),
('2324-01-04', 14, 7000, 35),
('2324-01-04', 15, 8000, 35),
('2324-01-04', 16, 10000, 35),
('2324-01-04', 17, 10000, 35),
('2324-01-04', 18, 10000, 35),

('2324-01-04', 21, 5000, 35),
('2324-01-04', 22, 5000, 35),
('2324-01-04', 23, 5000, 35),
('2324-01-04', 24, 7000, 35),
('2324-01-04', 25, 8000, 35),
('2324-01-04', 26, 10000, 35),
('2324-01-04', 27, 10000, 35),
('2324-01-04', 28, 10000, 35),

('2324-01-04', 31, 5000, 35),
('2324-01-04', 32, 5000, 35),
('2324-01-04', 33, 5000, 35),
('2324-01-04', 34, 7000, 35),
('2324-01-04', 35, 8000, 35),
('2324-01-04', 36, 10000, 35),
('2324-01-04', 37, 10000, 35),
('2324-01-04', 38, 10000, 35),

('2324-01-04', 41, 5000, 35),
('2324-01-04', 42, 5000, 35),
('2324-01-04', 43, 5000, 35),
('2324-01-04', 44, 7000, 35),
('2324-01-04', 45, 8000, 35),
('2324-01-04', 46, 10000, 35),
('2324-01-04', 47, 10000, 35),
('2324-01-04', 48, 10000, 35);

insert into prix (date, idDent, prix, idType) values
('2324-01-04', 11, 3500, 30),
('2324-01-04', 12, 3500, 30),
('2324-01-04', 13, 3500, 30),
('2324-01-04', 14, 3500, 30),
('2324-01-04', 15, 4000, 30),
('2324-01-04', 16, 4500, 30),
('2324-01-04', 17, 4500, 30),
('2324-01-04', 18, 4500, 30),

('2324-01-04', 21, 3500, 30),
('2324-01-04', 22, 3500, 30),
('2324-01-04', 23, 3500, 30),
('2324-01-04', 24, 3500, 30),
('2324-01-04', 25, 4000, 30),
('2324-01-04', 26, 4500, 30),
('2324-01-04', 27, 4500, 30),
('2324-01-04', 28, 4500, 30),

('2324-01-04', 31, 3500, 30),
('2324-01-04', 32, 3500, 30),
('2324-01-04', 33, 3500, 30),
('2324-01-04', 34, 3500, 30),
('2324-01-04', 35, 4000, 30),
('2324-01-04', 36, 4500, 30),
('2324-01-04', 37, 4500, 30),
('2324-01-04', 38, 4500, 30),

('2324-01-04', 41, 3500, 30),
('2324-01-04', 42, 3500, 30),
('2324-01-04', 43, 3500, 30),
('2324-01-04', 44, 3500, 30),
('2324-01-04', 45, 4000, 30),
('2324-01-04', 46, 4500, 30),
('2324-01-04', 47, 4500, 30),
('2324-01-04', 48, 4500, 30);

insert into prix (date, idDent, prix, idType) values
('2324-01-04', 11, 2000, 25),
('2324-01-04', 12, 2000, 25),
('2324-01-04', 13, 2000, 25),
('2324-01-04', 14, 2000, 25),
('2324-01-04', 15, 2500, 25),
('2324-01-04', 16, 3000, 25),
('2324-01-04', 17, 3000, 25),
('2324-01-04', 18, 3000, 25),

('2324-01-04', 21, 2000, 25),
('2324-01-04', 22, 2000, 25),
('2324-01-04', 23, 2000, 25),
('2324-01-04', 24, 2000, 25),
('2324-01-04', 25, 2500, 25),
('2324-01-04', 26, 3000, 25),
('2324-01-04', 27, 3000, 25),
('2324-01-04', 28, 3000, 25),

('2324-01-04', 31, 2000, 25),
('2324-01-04', 32, 2000, 25),
('2324-01-04', 33, 2000, 25),
('2324-01-04', 34, 2000, 25),
('2324-01-04', 35, 2500, 25),
('2324-01-04', 36, 3000, 25),
('2324-01-04', 37, 3000, 25),
('2324-01-04', 38, 3000, 25),

('2324-01-04', 41, 2000, 25),
('2324-01-04', 42, 2000, 25),
('2324-01-04', 43, 2000, 25),
('2324-01-04', 44, 2000, 25),
('2324-01-04', 45, 2500, 25),
('2324-01-04', 46, 3000, 25),
('2324-01-04', 47, 3000, 25),
('2324-01-04', 48, 3000, 25);
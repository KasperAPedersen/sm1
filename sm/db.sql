DROP DATABASE IF EXISTS OOP2; -- Deletes db oop2, if exists
CREATE DATABASE OOP2; -- Creates new database oop2
USE OOP2; -- Use database oop2

-- Create tables
CREATE TABLE City (
    PostalID int AUTO_INCREMENT PRIMARY KEY,
    Postal int,
    City varchar(255)
);

CREATE TABLE Education (
    EducationID int AUTO_INCREMENT PRIMARY KEY,
    EducationName varchar(255)
);

CREATE TABLE Job (
    JobID int AUTO_INCREMENT PRIMARY KEY,
    JobName varchar(255)
);

CREATE TABLE customer (
    PersonID int AUTO_INCREMENT PRIMARY KEY,
    FirstName varchar(255),
    LastName varchar(255),
    Street varchar(255),
    PostalID int,
    FOREIGN KEY (PostalID) REFERENCES City(PostalID),
    EducationID int,
    FOREIGN KEY (EducationID) REFERENCES Education(EducationID),
    EducationEnd date,
    JobID int,
    FOREIGN KEY (JobID) REFERENCES Job(JobID),
    JobStart date,
    JobEnd date
);

-- Insert template rows
INSERT INTO Job (JobName) VALUES ('Data technician'), ('Developer'), ('Supporter');
INSERT INTO Education (EducationName) VALUES ('Data technician'), ('Computer Science'), ('IT Supporter'), ('IT Infrastucture');
INSERT INTO City (Postal, City) VALUES (2770, 'Kastrup'), (4100, 'Ringsted'), (2300, 'København'), (2630, 'Taastrup');

-- Insert test users
INSERT INTO customer (FirstName, LastName, Street, PostalID, EducationID, EducationEnd, JobID, JobStart, JobEnd) VALUES 
                     ('John', 'Doe', 'Johndoestreet 13', 2, 1, '2020-01-24', 1, '2020/02/13', '2024/06/06');

INSERT INTO customer (FirstName, LastName, Street, PostalID, EducationID, EducationEnd, JobID, JobStart, JobEnd) VALUES 
                     ('Carsten', 'Hansen', 'abcvej 25', 3, 2, '1979-09-14', 2, '2013/07/09', '2022/01/17');

INSERT INTO customer (FirstName, LastName, Street, PostalID, EducationID, EducationEnd, JobID, JobStart, JobEnd) VALUES 
                     ('Bent', 'Bentesen', 'nnnnnn 13', 1, 3, '2001-11-19', 3, '2009/09/01', '2023/09/11');
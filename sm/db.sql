DROP DATABASE IF EXISTS OOP2;
CREATE DATABASE OOP2;
USE OOP2;
 
CREATE TABLE city (
  PostalCode INT PRIMARY KEY,
  CityName VARCHAR(255)
);
 
CREATE TABLE schools (
  educationID INT AUTO_INCREMENT PRIMARY KEY,
  schoolsName VARCHAR(255)
);
 
CREATE TABLE education (
  customerid INT,
  educationName int,
  educationEnd DATE,
  FOREIGN KEY (educationName) REFERENCES schools(educationID)
);
 
CREATE TABLE jobs (
  JobID INT AUTO_INCREMENT PRIMARY KEY,
  JobName VARCHAR(255)
);
 
CREATE TABLE employment (
  customerid INT,
  EmploymentName int,
  EmploymentStart DATE,
  EmploymentEnd DATE,
  FOREIGN KEY (EmploymentName) REFERENCES jobs(JobID)
);
 
CREATE TABLE customer (
  id int AUTO_INCREMENT PRIMARY KEY,
  FirstName VARCHAR(255),
  LastName VARCHAR(255),
  Street VARCHAR(255),
  PostalID INT,
  FOREIGN KEY (PostalID) REFERENCES city(PostalCode)
);
 
-- Insert data into the tables
INSERT INTO city (PostalCode, CityName) VALUES (2300, 'København');
INSERT INTO city (PostalCode, CityName) VALUES (2630, 'Taastrup');
INSERT INTO city (PostalCode, CityName) VALUES (2770, 'Kastrup');
INSERT INTO schools (schoolsName) VALUES ('Københavns universitet');
INSERT INTO schools (schoolsName) VALUES ('Aalborg universitet');
INSERT INTO schools (schoolsName) VALUES ('ZBC');
INSERT INTO jobs (JobName) VALUES ('Programmør');
INSERT INTO jobs (JobName) VALUES ('Supporter');

INSERT INTO `education` (`customerid`, `educationName`, `educationEnd`) VALUES ('1', '1', '2024-06-15');
INSERT INTO `education` (`customerid`, `educationName`, `educationEnd`) VALUES ('2', '3', '2021-01-02');

INSERT INTO `employment` (`customerid`, `EmploymentName`, `EmploymentStart`, `EmploymentEnd`) VALUES ('1', '1', '2024-06-14', '2024-06-29');
INSERT INTO `employment` (`customerid`, `EmploymentName`, `EmploymentStart`, `EmploymentEnd`) VALUES ('2', '2', '2024-06-28', '2024-06-30');

INSERT INTO `customer` (`FirstName`, `LastName`, `Street`, `PostalID`) VALUES ('Kasper', 'Pedersen', 'gade 13', '2770');
INSERT INTO `customer` (`FirstName`, `LastName`, `Street`, `PostalID`) VALUES ('John', 'Larsen', 'vej 14', '2630');
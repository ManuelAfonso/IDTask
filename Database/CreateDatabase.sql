CREATE DATABASE IliDigitalTest;
USE IliDigitalTest;
CREATE SCHEMA SoldierInfo;
CREATE SCHEMA SoldierLocation;


CREATE TABLE SoldierInfo.[Rank] (
	Id int not null,
	[Description] varchar(100) not null,
	CONSTRAINT PK_Rank PRIMARY KEY (Id)
);
CREATE TABLE SoldierInfo.Country (
	Id int not null,
	[Name] varchar(100) not null,
	CONSTRAINT PK_Country PRIMARY KEY (Id)
);
CREATE TABLE SoldierInfo.SensorType (
	Id int not null,
	[Description] varchar(100) not null,
	CONSTRAINT PK_SensorType PRIMARY KEY (Id)
);
CREATE TABLE SoldierInfo.Soldier (
	Id uniqueidentifier not null,
	[Name] varchar(100) not null,
	RankId int not null,
	CountryId int not null,
	TrainingInfo varchar(max) not null,
	SensorName varchar(100) not null,
	SensorTypeId int not null,
	CONSTRAINT PK_Soldier PRIMARY KEY (Id),
	CONSTRAINT FK_Rank FOREIGN KEY (RankId) REFERENCES SoldierInfo.[Rank],
	CONSTRAINT FK_Country FOREIGN KEY (CountryId) REFERENCES SoldierInfo.[Country],
	CONSTRAINT FK_SendorType FOREIGN KEY (SensorTypeId) REFERENCES SoldierInfo.[SensorType]
);
CREATE TABLE SoldierLocation.SourceType (
	Id int not null,
	[Description] varchar(100) not null,
	CONSTRAINT PK_SourceType PRIMARY KEY (Id)
);
CREATE TABLE SoldierLocation.SoldierLocation (
	Id bigint identity(1,1),
	SoldierId uniqueidentifier not null,
	Latitude decimal(9,6) not null,
	Longitude decimal(9,6) not null,
	MovementDate datetime2(2) not null,
	SourceTypeId int not null,
	Active bit not null,
	CONSTRAINT PK_SoldierLocation PRIMARY KEY (Id),
	CONSTRAINT FK_SourceType FOREIGN KEY (SourceTypeId) REFERENCES SoldierLocation.SourceType
);


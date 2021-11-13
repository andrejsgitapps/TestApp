IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TestAppDb')
BEGIN
	CREATE DATABASE [TestAppDb]
END
GO

USE [TestAppDb]
GO

CREATE TABLE LookupWords (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Word NVARCHAR(512) NOT NULL
)

CREATE TABLE SearchStrings (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LookupWordId INT NOT NULL FOREIGN KEY REFERENCES LookupWords(Id),
	String NVARCHAR(512) NOT NULL,
	Weight INT NOT NULL DEFAULT(0)
)

CREATE INDEX idx_LookpWords_SWL
ON LookupWords(Word)

CREATE INDEX idx_SearchStrings_SWL
ON SearchStrings(String, Weight, LookupWordId)
GO

INSERT INTO LookupWords(Word) VALUES(N'Microphone');
DECLARE @microphoneRecordId INT = @@IDENTITY
INSERT INTO LookupWords(Word) VALUES(N'Microsoft');
DECLARE @microsoftRecordId INT = @@IDENTITY
INSERT INTO LookupWords(Word) VALUES(N'Microscope');
DECLARE @microscopeRecordId INT = @@IDENTITY
INSERT INTO LookupWords(Word) VALUES(N'Microspot');
INSERT INTO LookupWords(Word) VALUES(N'Antimicrobial');
INSERT INTO LookupWords(Word) VALUES(N'Atomic');
INSERT INTO LookupWords(Word) VALUES(N'Mimicry');

INSERT INTO SearchStrings(LookupWordId, String, Weight) VALUES(@microphoneRecordId, N'mic', 10)
INSERT INTO SearchStrings(LookupWordId, String, Weight) VALUES(@microscopeRecordId, N'mic', 5)
INSERT INTO SearchStrings(LookupWordId, String, Weight) VALUES(@microscopeRecordId, N'micr', 11)
INSERT INTO SearchStrings(LookupWordId, String, Weight) VALUES(@microsoftRecordId, N'micr', 1)

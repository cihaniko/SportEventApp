
CREATE TABLE SportEvents (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(100),
    StartTime DATETIME,
    Capacity INT
);

GO

CREATE TABLE SportEventRegistrations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    RegisterDate DATETIME NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_SportEventRegistrations_SportEvents
        FOREIGN KEY (EventId)
        REFERENCES SportEvents(Id)
);

GO

CREATE PROCEDURE sp_GetAllSportEvents
AS
BEGIN
    SELECT Id, Name, Location, StartTime, Capacity FROM SportEvents;
END;

GO

CREATE OR ALTER PROCEDURE sp_GetSportEventById
    @Id INT
AS
BEGIN
    SELECT Id, Title, Description, CreatedAt
    FROM SportEvents
    WHERE Id = @Id;
END


GO

CREATE OR ALTER PROCEDURE sp_RegisterUserToEvent
    @EventId INT,
    @UserName NVARCHAR(100)
AS
BEGIN
    INSERT INTO SportEventRegistrations (EventId, UserName)
    VALUES (@EventId, @UserName);
END

GO

CREATE OR ALTER PROCEDURE sp_GetRegisteredUsersByEventId
    @EventId INT
AS
BEGIN
    SELECT UserName
    FROM SportEventRegistrations
    WHERE EventId = @EventId;
END

GO


INSERT INTO SportEvents (Id, Name, Location, StartTime, Capacity)
VALUES 
(1, 'Basketbol Turnuvası', 'İstanbul', '2025-06-01 10:00:00', 10),
(2, 'Futbol Maçı', 'Ankara', '2025-06-02 14:00:00', 22),
(3, 'Tenis Müsabakası', 'İzmir', '2025-06-03 09:00:00', 4);

USE [dcbadge]
GO

WHILE (SELECT COUNT(ID) FROM Codesdev) < 50 
BEGIN  
INSERT INTO [dbo].[Codesdev]
           ([requestcode])
     VALUES
           (NEWID()) 
END  

SELECT requestcode FROM Codesdev

GO

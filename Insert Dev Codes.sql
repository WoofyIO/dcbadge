USE [dcbadge]
GO

WHILE (SELECT COUNT(ID) FROM Codes) < 100 
BEGIN  
INSERT INTO [dbo].[Codes]
           ([maxqantity],[requestcode])
     VALUES
           ('2',NEWID()) 
END  

SELECT requestcode FROM Codes WHERE [maxqantity] = '2'

GO

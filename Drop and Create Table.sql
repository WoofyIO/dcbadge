USE [dcbadge]
GO

/****** Object:  Table [dbo].[Codes]    Script Date: 4/26/2017 10:45:16 AM ******/
DROP TABLE [dbo].[Codes]
GO

/****** Object:  Table [dbo].[Codes]    Script Date: 4/26/2017 10:45:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Codes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[requestcode] [varchar](255) UNIQUE NOT NULL,
	[maxqantity] [int] DEFAULT '2' NOT NULL,
	[codeused] [bit] DEFAULT 0 NOT NULL,
	[collected] [bit] DEFAULT 0 NOT NULL,
	[qantity] [int] DEFAULT 0 NOT NULL,
	[price] [float] DEFAULT 0 NOT NULL,
	[email] [varchar](255) NULL,
	[custcode] [varchar](255) NULL,
	[paycode] [varchar](255) NULL,
	[datepayed] [datetime] NULL,
	[datecollect] [datetime] NULL,
	[qrcode] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO



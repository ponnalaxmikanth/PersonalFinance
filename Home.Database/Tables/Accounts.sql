CREATE TABLE [dbo].[Accounts]
(
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[DisplayName] [nvarchar](250) NULL,
	[ExcelMapping] [nvarchar](50) NULL,
	[ClosingDay] [int] NULL,
	[DueDay] [int] NULL,
	[DisplayOrder] [int] NOT NULL,
	[OpenDate] [datetime] NOT NULL,
	[Status] [char](1) NOT NULL,
	[Limit] [decimal](18, 2) NULL,
	[LimitIncreaseDate] [datetime] NULL,
	[LimitIncreaseStatus] [nvarchar](50) NULL,
	[CreateDate] DATETIME CONSTRAINT [DF_Accounts_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[ModifiedDate] DATETIME CONSTRAINT [DF_Accounts_ModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_AccountTypes] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountTypes] ([AccountTypeId])
GO

ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_AccountTypes]
GO

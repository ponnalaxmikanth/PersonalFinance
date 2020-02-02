CREATE TABLE [dbo].[AccountTypes]
(
	[AccountTypeId] [int] IDENTITY(1,1) NOT NULL,
	[AccountType] [nvarchar](100) NOT NULL,
	[CreateDate] [datetime] CONSTRAINT [DF_AccountTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[ModifiedDate] [datetime] CONSTRAINT [DF_AccountTypes_ModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_AccountTypes] PRIMARY KEY CLUSTERED 
(
	[AccountTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

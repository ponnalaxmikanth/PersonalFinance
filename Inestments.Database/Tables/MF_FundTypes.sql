CREATE TABLE [dbo].[MF_FundTypes](
	[FundTypeId] [int] IDENTITY(1,1) NOT NULL,
	[FundType] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [PK_FundType] PRIMARY KEY CLUSTERED 
(
	[FundTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


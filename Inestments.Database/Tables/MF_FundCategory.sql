CREATE TABLE [dbo].[MF_FundCategory](
	[FundClassId] [int] IDENTITY(1,1) NOT NULL,
	[FundClass] [nvarchar](100) NOT NULL,
	[IsSectorCategory] [bit] NULL,
	[Description] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [PK_FundClassId] PRIMARY KEY CLUSTERED 
(
	[FundClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


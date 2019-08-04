CREATE TABLE [dbo].[MF_FundOptions](
	[OptionId] [int] IDENTITY(1,1) NOT NULL,
	[FundOption] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [PK_FundOptions] PRIMARY KEY CLUSTERED 
(
	[OptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[MF_Dividends](
	[DividendId] [int] IDENTITY(1,1) NOT NULL,
	[FundId] [int] NOT NULL,
	[DividendDate] [date] NOT NULL,
	[Nav] [decimal](10, 4) NOT NULL,
	[Dividend] [decimal](10, 4) NOT NULL,
	[CreateDateTime] [datetime]  CONSTRAINT [DF_MF_Dividends_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastUpdateDateTime] [datetime] CONSTRAINT [DF_MF_Dividends_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
	[IsActive] [nvarchar](5) NULL,
 CONSTRAINT [PK_MF_Dividends] PRIMARY KEY CLUSTERED 
(
	[DividendId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MF_Dividends]  WITH CHECK ADD  CONSTRAINT [FK_MF_Dividends_MF_Funds] FOREIGN KEY([FundId])
REFERENCES [dbo].[MF_Funds] ([FundId])
GO

ALTER TABLE [dbo].[MF_Dividends] CHECK CONSTRAINT [FK_MF_Dividends_MF_Funds]
GO


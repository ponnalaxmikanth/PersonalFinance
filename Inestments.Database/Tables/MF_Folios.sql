CREATE TABLE [dbo].[MF_Folios](
	[FolioId] [int] IDENTITY(1,1) NOT NULL,
	[FolioNumber] [nvarchar](50) NOT NULL,
	[FundHouseId] [int] NULL,
	[portfolioId] [int] NULL,
	[isDefaultFolio] [nvarchar](1) NULL CONSTRAINT [DF_MF_Folios_isDefaultFolio]  DEFAULT (N'N'),
	[RegistredOwner] [nvarchar](100) NULL,
	[Owner] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [FolioId] PRIMARY KEY CLUSTERED 
(
	[FolioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


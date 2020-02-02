CREATE TABLE [dbo].[MF_Portfolios](
	[PortfolioId] [int] IDENTITY(1,1) NOT NULL,
	[Portfolio] [nvarchar](50) NOT NULL,
	[IsActive] [nvarchar](2) NULL,
	[Description] [nvarchar](150) NULL,
	[CreatedDate] [datetime] CONSTRAINT [DF_MF_Portfolios_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastModifiedDate] DATETIME CONSTRAINT [DF_MF_Portfolios_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_PortfolioId] PRIMARY KEY CLUSTERED 
(
	[PortfolioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[MF_DataDumpDates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[FundType] [int] NOT NULL,
	[Count] [int] NULL,
	[CreateDate] [datetime] CONSTRAINT [DF_MF_DataDumpDates_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastModifiedDate] DATETIME CONSTRAINT [DF_MF_DataDumpDates_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_MF_DataDumpDates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


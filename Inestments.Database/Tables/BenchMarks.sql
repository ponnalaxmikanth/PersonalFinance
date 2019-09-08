
CREATE TABLE [dbo].[BenchMarks](
	[BenchMarkId] [int] IDENTITY(1,1) NOT NULL,
	[BenchMark] [nvarchar](100) NOT NULL,
	[IsGenerateGraph] [nvarchar](1) NOT NULL CONSTRAINT [DF_BenchMarks_IsGenerateGraph]  DEFAULT (N'N'),
	[LastUpdatedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BenchMarks] PRIMARY KEY CLUSTERED 
(
	[BenchMarkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



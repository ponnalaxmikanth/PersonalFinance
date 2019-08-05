CREATE TABLE [dbo].[TLogMessage](
	[LogMessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[ShortMessage] [nvarchar](100) NOT NULL,
	[LongMessage] [nvarchar](max) NOT NULL,
	[Component] [nvarchar](100) NOT NULL,
	[Application] [nvarchar](100) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TLogMessage] PRIMARY KEY CLUSTERED 
(
	[LogMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


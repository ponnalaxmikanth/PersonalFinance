CREATE TABLE [dbo].[HomeTransactions]
(
	[TransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [date] NOT NULL default GETDATE(),
	[PostedDate] [date] NOT NULL DEFAULT GETDATE() ,
	[AccountId] [int] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Debit] [decimal](18, 4) NULL,
	[Credit] [decimal](18, 4) NULL,
	[Total] [decimal](18, 4) NULL,
	[Group] [nvarchar](100) NULL,
	[SubGroup] [nvarchar](100) NULL,
	[TransactedBy] [nvarchar](50) NOT NULL,
	[Store] [nvarchar](50) NOT NULL,
	[Comments] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL default GETDATE(),
	[LastModifiedDate] [datetime] NOT NULL default GETDATE(),
 CONSTRAINT [PK_HomeTransactions] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[HomeTransactions]  WITH CHECK ADD  CONSTRAINT [FK_HomeTransactions_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
GO

ALTER TABLE [dbo].[HomeTransactions] CHECK CONSTRAINT [FK_HomeTransactions_Accounts]
GO

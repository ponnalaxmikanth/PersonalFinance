CREATE TABLE [dbo].[HomeTransactions] (
    [TransactionId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [RowNumber]        INT             NULL,
    [PostedDate]       DATE             CONSTRAINT [DF_HomeTransactions_PostedDate] DEFAULT (getdate()) NOT NULL,
    [AccountId]        INT             NOT NULL,
    [Description]      NVARCHAR (100)  NULL,
    [Debit]            DECIMAL (18, 4) NULL,
    [Credit]           DECIMAL (18, 4) NULL,
    [Total]            DECIMAL (18, 4) NULL,
    [Group]            NVARCHAR (100)  NULL,
    [SubGroup]         NVARCHAR (100)  NULL,
    [TransactedBy]     NVARCHAR (50)   NOT NULL,
    [Store]            NVARCHAR (50)   NOT NULL,
    [Comments]         NVARCHAR (100)  NULL,
    [CreatedDate]      DATETIME        CONSTRAINT [DF_HomeTransactions_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastModifiedDate] DATETIME        CONSTRAINT [DF_HomeTransactions_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_HomeTransactions] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    CONSTRAINT [FK_HomeTransactions_Accounts] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([AccountId])
);



GO

GO


GO

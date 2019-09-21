CREATE TABLE [dbo].[HomeTransactions] (
    [TransactionId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [RowNumber]        INT             NULL,
    [PostedDate]       DATE            CONSTRAINT [DF__HomeTrans__Poste__1ED998B2] DEFAULT (getdate()) NOT NULL,
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
    [CreatedDate]      DATETIME        CONSTRAINT [DF__HomeTrans__Creat__182C9B23] DEFAULT (getdate()) NOT NULL,
    [LastModifiedDate] DATETIME        CONSTRAINT [DF__HomeTrans__LastM__1920BF5C] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_HomeTransactions] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    CONSTRAINT [FK_HomeTransactions_Accounts] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([AccountId])
);



GO

GO


GO

CREATE TABLE [dbo].[HealthInsurance] (
    [TransactionId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [PostedDate]       DATE            CONSTRAINT [DF_HealthInsurance_PostedDate] DEFAULT (getdate()) NOT NULL,
    [Description]      NVARCHAR (100)  NULL,
    [Debit]            DECIMAL (18, 4) NULL,
    [Credit]           DECIMAL (18, 4) NULL,
    [Total]            DECIMAL (18, 4) NULL,
    [Group]            NVARCHAR (100)  NULL,
    [SubGroup]         NVARCHAR (100)  NULL,
    [TransactedBy]     NVARCHAR (50)   NOT NULL,
    [Comments]         NVARCHAR (100)  NULL,
    [CreatedDate]      DATETIME        CONSTRAINT [DF_HealthInsurance_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastModifiedDate] DATETIME        CONSTRAINT [DF_HealthInsurance_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_HealthInsurance] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


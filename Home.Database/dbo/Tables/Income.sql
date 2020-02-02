CREATE TABLE [dbo].[Income] (
    [IncomeId]        INT            IDENTITY (1, 1) NOT NULL,
    [PayDate]         DATE           NOT NULL,
    [FromDate]        DATE           NOT NULL,
    [ToDate]          DATE           NOT NULL,
    [Billing]         DECIMAL (8, 4) NOT NULL,
    [Share]           DECIMAL (8, 4) NOT NULL,
    [Hours]           DECIMAL (8, 4) NOT NULL,
    [Insurance]       DECIMAL (8, 4) NULL,
    [FederalTax]      DECIMAL (8, 4) NULL,
    [SocialTax]       DECIMAL (8, 4) NULL,
    [Medicare]        DECIMAL (8, 4) NULL,
    [StateTax]        DECIMAL (8, 4) NULL,
    [Reimbursment]    DECIMAL (8, 4) NULL,
    [Miscelaneous]    DECIMAL (8, 4) NULL,
    [Comments]        NVARCHAR (100) NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_Income_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       CONSTRAINT [DF_Income_LastUpdatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Income] PRIMARY KEY CLUSTERED ([IncomeId] ASC)
);


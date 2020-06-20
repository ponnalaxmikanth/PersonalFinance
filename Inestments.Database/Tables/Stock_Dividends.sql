CREATE TABLE [dbo].[Stock_Dividends] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [StockID]            VARCHAR (30)    NOT NULL,
    [DividendDate]       DATE            NULL,
    [DividendPerStock]   DECIMAL (18, 4) CONSTRAINT [DF__Stock_Div__Divid__0A9D95DB] DEFAULT ((0)) NULL,
    [Dividend]           DECIMAL (18, 4) NULL,
    [CreateDateTime]     DATETIME        CONSTRAINT [DF_Stock_Dividends_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME        CONSTRAINT [DF_Stock_Dividends_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Stock_Di__3214EC07809F9FEF] PRIMARY KEY CLUSTERED ([Id] ASC)
);



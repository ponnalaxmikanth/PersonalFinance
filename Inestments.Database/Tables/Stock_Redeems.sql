CREATE TABLE [dbo].[Stock_Redeems] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [StockID]            VARCHAR (30)    NOT NULL,
    [PurchaseDate]       DATE            NOT NULL,
    [Quantity]           INT             NOT NULL,
    [Price]              MONEY           NOT NULL,
    [Dividend]           MONEY           CONSTRAINT [DF__Stock_Red__Divid__0D7A0286] DEFAULT ((0)) NULL,
    [DividendPerStock]   DECIMAL (18, 4) NULL,
    [SellDate]           DATE            NOT NULL,
    [SellPrice]          MONEY           NOT NULL,
    [CreateDateTime]     DATETIME        CONSTRAINT [DF_Stock_Redeems_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME        CONSTRAINT [DF_Stock_Redeems_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Stock_Re__3214EC07FDBD1470] PRIMARY KEY CLUSTERED ([Id] ASC)
);



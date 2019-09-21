CREATE TABLE [dbo].[Digital_Currency_Redeems] (
    [TransactionId] INT             IDENTITY (1, 1) NOT NULL,
    [Currency]      NVARCHAR (50)   NOT NULL,
    [PruchaseDate]  DATETIME        CONSTRAINT [DF_Digital_Currency_PurchaseDate] DEFAULT (getdate()) NOT NULL,
    [Size]          DECIMAL (10, 8) NOT NULL,
    [Price]         DECIMAL (18, 4) NOT NULL,
    [Fees]          DECIMAL (4, 4)  CONSTRAINT [DF_Digital_Currency_Redeems_Fees] DEFAULT ((0.0)) NOT NULL,
    [SellDate]      DATETIME        CONSTRAINT [DF_Digital_Currency_Redeems_SellDate] DEFAULT (getdate()) NOT NULL,
    [SellPrice]     DECIMAL (18, 4) CONSTRAINT [DF_Digital_Currency_Redeems_SellPrice] DEFAULT ((0.0)) NULL,
    [SellFees]      DECIMAL (4, 4)  CONSTRAINT [DF_Digital_Currency_Redeems_SellFees] DEFAULT ((0.0)) NOT NULL,
    CONSTRAINT [PK_Digital_Currency_Redeem] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


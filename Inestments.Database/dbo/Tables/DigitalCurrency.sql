CREATE TABLE [dbo].[DigitalCurrency] (
    [TransactionId] INT             IDENTITY (1, 1) NOT NULL,
    [Currency]      NVARCHAR (50)   NOT NULL,
    [PurchaseDate]  DATETIME        NOT NULL,
    [Size]          DECIMAL (10, 8) NOT NULL,
    [Price]         DECIMAL (18, 4) NOT NULL,
    [Fees]          DECIMAL (4, 4)  CONSTRAINT [DF_DigitalCurrency_Fees] DEFAULT ((0.0)) NOT NULL,
    [CurrentPrice]  DECIMAL (18, 4) CONSTRAINT [DF_DigitalCurrency_CurrentPrice] DEFAULT ((0.0)) NULL,
	[CreateDate] DATETIME CONSTRAINT [DF_DigitalCurrency_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[ModifiedDate] DATETIME CONSTRAINT [DF_DigitalCurrency_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DigitalCurrency] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


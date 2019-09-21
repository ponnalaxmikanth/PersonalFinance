CREATE TABLE [dbo].[Budget] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [FromDate]    DATE            CONSTRAINT [DF_Budget_FromDate] DEFAULT (getdate()) NOT NULL,
    [ToDate]      DATE            CONSTRAINT [DF_Budget_ToDate] DEFAULT (getdate()) NOT NULL,
    [Group]       NVARCHAR (100)  CONSTRAINT [DF_Budget_Group] DEFAULT (' ') NOT NULL,
    [Amount]      DECIMAL (18, 4) CONSTRAINT [DF_Budget_Amount] DEFAULT ((0)) NOT NULL,
    [CreatedDate] DATETIME        CONSTRAINT [DF_Budget_CreatedDate] DEFAULT (getdate()) NOT NULL
);




GO
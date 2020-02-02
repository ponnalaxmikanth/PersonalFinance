
CREATE TABLE [dbo].[BenchMarks] (
    [BenchMarkId]     INT            IDENTITY (1, 1) NOT NULL,
    [BenchMark]       NVARCHAR (100) NOT NULL,
    [Sensex]          NVARCHAR (50)  NULL,
    [IsGenerateGraph] NVARCHAR (1)   CONSTRAINT [DF_BenchMarks_IsGenerateGraph] DEFAULT (N'N') NOT NULL,
    [LastUpdatedDate] DATETIME       NOT NULL CONSTRAINT [DF_BenchMarks_LastUpdatedDate] DEFAULT (getdate()) ,
    [CreatedDate]     DATETIME       NOT NULL CONSTRAINT [DF_BenchMarks_CreatedDate] DEFAULT (getdate()) ,
    CONSTRAINT [PK_BenchMarks] PRIMARY KEY CLUSTERED ([BenchMarkId] ASC)
);



GO



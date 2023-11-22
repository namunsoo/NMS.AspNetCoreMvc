CREATE TABLE [dbo].[Board] (
    [No]             INT              IDENTITY (1, 1) NOT NULL,
    [Category]       NVARCHAR (50)    NOT NULL,
    [Title]          NVARCHAR (50)    NOT NULL,
    [BoardContent]   NVARCHAR (MAX)   NOT NULL,
    [CreateUserCode] UNIQUEIDENTIFIER NOT NULL,
    [CreateUserName] NVARCHAR (50)    NOT NULL,
    [CreateDate]     DATETIME2 (7)    NOT NULL,
    [UpdateUserCode] UNIQUEIDENTIFIER NULL,
    [UpdateUserName] NVARCHAR (50)    NULL,
    [UpdateDate]     DATETIME2 (7)    NULL
);


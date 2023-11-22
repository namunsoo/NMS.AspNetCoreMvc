CREATE TABLE [dbo].[BoardReply] (
    [No]             INT              IDENTITY (1, 1) NOT NULL,
    [BoardNo]        INT              NULL,
    [ReplyNo]        INT              NULL,
    [Text]           NVARCHAR (150)   NOT NULL,
    [CreateUserCode] UNIQUEIDENTIFIER NOT NULL,
    [CreateUserName] NVARCHAR (50)    NOT NULL,
    [CreateDate]     DATETIME2 (7)    NOT NULL,
    [UpdateUserCode] UNIQUEIDENTIFIER NULL,
    [UpdateUserName] NVARCHAR (50)    NULL,
    [UpdateDate]     DATETIME2 (7)    NULL
);


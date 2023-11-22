CREATE TABLE [dbo].[QuestionCards] (
    [No]         INT              IDENTITY (1, 1) NOT NULL,
    [UserCode]   UNIQUEIDENTIFIER NOT NULL,
    [Question]   NVARCHAR (MAX)   NOT NULL,
    [Answer]     NVARCHAR (MAX)   NOT NULL,
    [Importance] INT              NOT NULL,
    [Memorize]   BIT              NOT NULL
);


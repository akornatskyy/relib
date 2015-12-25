SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE SCHEMA [HistoryLog] AUTHORIZATION [dbo]
GO

CREATE TABLE [HistoryLog].[Event](
        [EventId] [smallint] NOT NULL,
        [Name] [varchar](50) COLLATE Latin1_General_CI_AI NOT NULL,
        [Format] [varchar](255) COLLATE Latin1_General_CI_AI NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED
(
        [EventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [HistoryLog].[Item](
        [ItemId] [int] IDENTITY(1,1) NOT NULL,
        [EventId] [smallint] NOT NULL,
        [OriginatorId] [varchar](50) COLLATE Latin1_General_CI_AI NOT NULL,        
        [Ip] [int] NOT NULL,
        [Timestamp] [datetime] NOT NULL,
        [RelatedTo] [varchar](50) COLLATE Latin1_General_CI_AI NULL,
        [Arguments] [varchar](255) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED
(
        [ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [HistoryLog].[ItemExtraIp](
        [ItemId] [int] NOT NULL,
        [Ip] [int] NOT NULL,
 CONSTRAINT [PK_ItemExtraIp] PRIMARY KEY CLUSTERED
(
        [ItemId] ASC,
        [Ip] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [HistoryLog].[ItemExtraIp]  WITH CHECK ADD CONSTRAINT [FK_ItemExtraIp_Item] 
FOREIGN KEY([ItemId])
REFERENCES [HistoryLog].[Item] ([ItemId])
GO

ALTER TABLE [HistoryLog].[ItemExtraIp] CHECK CONSTRAINT [FK_ItemExtraIp_Item]
GO

ALTER TABLE [HistoryLog].[Item]  WITH CHECK ADD CONSTRAINT [FK_Item_Event] 
FOREIGN KEY([EventId])
REFERENCES [HistoryLog].[Event] ([EventId])
GO

ALTER TABLE [HistoryLog].[Item] CHECK CONSTRAINT [FK_Item_Event]
GO

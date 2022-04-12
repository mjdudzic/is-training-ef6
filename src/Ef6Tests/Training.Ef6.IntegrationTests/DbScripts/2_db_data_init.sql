SET IDENTITY_INSERT [dbo].[LinkTypes] ON 
GO
INSERT [dbo].[LinkTypes] ([Id], [Type]) VALUES (1, N'Linked')
GO
INSERT [dbo].[LinkTypes] ([Id], [Type]) VALUES (3, N'Duplicate')
GO
SET IDENTITY_INSERT [dbo].[LinkTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[PostTypes] ON 
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (1, N'Question')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (2, N'Answer')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (3, N'Wiki')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (4, N'TagWikiExerpt')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (5, N'TagWiki')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (6, N'ModeratorNomination')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (7, N'WikiPlaceholder')
GO
INSERT [dbo].[PostTypes] ([Id], [Type]) VALUES (8, N'PrivilegeWiki')
GO
SET IDENTITY_INSERT [dbo].[PostTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[VoteTypes] ON 
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (1, N'AcceptedByOriginator')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (2, N'UpMod')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (3, N'DownMod')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (4, N'Offensive')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (5, N'Favorite')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (6, N'Close')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (7, N'Reopen')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (8, N'BountyStart')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (9, N'BountyClose')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (10, N'Deletion')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (11, N'Undeletion')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (12, N'Spam')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (13, N'InformModerator')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (15, N'ModeratorReview')
GO
INSERT [dbo].[VoteTypes] ([Id], [Name]) VALUES (16, N'ApproveEditSuggestion')
GO
SET IDENTITY_INSERT [dbo].[VoteTypes] OFF
GO

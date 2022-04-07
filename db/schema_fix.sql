ALTER TABLE [dbo].[Badges]  WITH CHECK ADD  CONSTRAINT [FK_Badges_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Badges] CHECK CONSTRAINT [FK_Badges_Users]
GO

ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Posts] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Posts]
GO

ALTER TABLE [dbo].[PostLinks]  WITH CHECK ADD  CONSTRAINT [FK_PostLinks_Posts] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([Id])
GO
ALTER TABLE [dbo].[PostLinks] CHECK CONSTRAINT [FK_PostLinks_Posts]
GO
ALTER TABLE [dbo].[PostLinks]  WITH CHECK ADD  CONSTRAINT [FK_PostLinks_Posts2] FOREIGN KEY([RelatedPostId])
REFERENCES [dbo].[Posts] ([Id])
GO
ALTER TABLE [dbo].[PostLinks] CHECK CONSTRAINT [FK_PostLinks_Posts2]
GO
ALTER TABLE [dbo].[PostLinks]  WITH CHECK ADD  CONSTRAINT [FK_PostLinks_LinkTypes] FOREIGN KEY([LinkTypeId])
REFERENCES [dbo].[LinkTypes] ([Id])
GO
ALTER TABLE [dbo].[PostLinks] CHECK CONSTRAINT [FK_PostLinks_LinkTypes]
GO

ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Posts1] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Posts] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Posts1]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Posts2] FOREIGN KEY([AcceptedAnswerId])
REFERENCES [dbo].[Posts] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Posts2]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Users1] FOREIGN KEY([OwnerUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Users1]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Users2] FOREIGN KEY([LastEditorUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Users2]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_PostTypes] FOREIGN KEY([PostTypeId])
REFERENCES [dbo].[PostTypes] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_PostTypes]
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_VoteTypes] FOREIGN KEY([VoteTypeId])
REFERENCES [dbo].[VoteTypes] ([Id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_VoteTypes]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Users]
GO

/*****************************/
IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Posts_CreationDate')   
    DROP INDEX CreationDate ON dbo.Posts;   
GO    
CREATE NONCLUSTERED INDEX IX_Posts_CreationDate   
    ON dbo.Posts (CreationDate);   
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Users_Location')   
    DROP INDEX IX_Users_Location ON dbo.Users;   
GO     
CREATE NONCLUSTERED INDEX IX_Users_Location   
    ON dbo.Users (Location);   
GO  

ALTER TABLE [dbo].[Users] ADD [Region] varchar(30) null;
GO
update [dbo].[Users] set Region = 'Europe' 
where Location like '%Poland%' or
Location like '%Germany%' or
Location like '%Czech%'

update [dbo].[Users] set Region = 'North America' 
where Location like '%US%' or
Location like '%USA%' or
Location like '%Mexico%'
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Users_Region')   
    DROP INDEX IX_Users_Region ON dbo.Users;   
GO     
CREATE NONCLUSTERED INDEX IX_Users_Region   
    ON dbo.Users (Region);   
GO 

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Users_Reputation')   
    DROP INDEX IX_Users_Reputation ON dbo.Users;   
GO     
CREATE NONCLUSTERED INDEX IX_Users_Reputation   
    ON dbo.Users (Reputation);   
GO  

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Users_UpVotes')   
    DROP INDEX IX_Users_UpVotes ON dbo.Users;   
GO     
CREATE NONCLUSTERED INDEX IX_Users_UpVotes   
    ON dbo.Users (UpVotes);   
GO  

/***************************/
CREATE FUNCTION [dbo].[GetDelimitedStringValues] (@ValuesString VARCHAR(MAX))
RETURNS @Values TABLE ([value] [nvarchar](500))
AS
BEGIN
	insert into @Values
	select value from string_split(@ValuesString, ',')
 RETURN
END
GO

CREATE PROCEDURE [dbo].[GetUserByReputationWithLastPost]
	@MinReputation int
AS
    select * 
	from dbo.Users
	where Reputation >= @MinReputation;

	with top_posts as (
		select 
			 p.*
			,ROW_NUMBER() over(partition by p.OwnerUserId order by p.Id desc) as 'RowNumber' 
		from dbo.Posts p
		where OwnerUserId in (
			select u.Id 
			from dbo.Users u
			where u.Reputation >= @MinReputation
		)
	)
    select * 
	from top_posts p
	where p.RowNumber = 1
GO

CREATE PROCEDURE [dbo].[GetDictionaries]
AS
    select *
	from dbo.PostTypes

	select *
	from dbo.LinkTypes

	select *
	from dbo.VoteTypes
GO
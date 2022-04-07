SET IDENTITY_INSERT [dbo].[Users] ON;  
GO 

declare @now datetime = getdate();

INSERT INTO [dbo].[Users]
([Id],[AboutMe],[Age],[CreationDate],[DisplayName],[DownVotes],[EmailHash],[LastAccessDate],[Location],[Reputation],[UpVotes],[Views],[WebsiteUrl],[AccountId])
select distinct p.LastEditorUserId, null, 22, @now, 'user' + cast(p.LastEditorUserId as nvarchar), 0, null, @now, 'Space', 1, 1, 1, null, null
from dbo.Posts p 
where p.LastEditorUserId not in (select u.Id from dbo.Users u) 

INSERT INTO [dbo].[Users]
([Id],[AboutMe],[Age],[CreationDate],[DisplayName],[DownVotes],[EmailHash],[LastAccessDate],[Location],[Reputation],[UpVotes],[Views],[WebsiteUrl],[AccountId])
select distinct p.OwnerUserId, null, 22, @now, 'user' + cast(p.OwnerUserId as nvarchar), 0, null, @now, 'Space', 1, 1, 1, null, null
from dbo.Posts p 
where p.OwnerUserId not in (select u.Id from dbo.Users u) 

INSERT INTO [dbo].[Users]
([Id],[AboutMe],[Age],[CreationDate],[DisplayName],[DownVotes],[EmailHash],[LastAccessDate],[Location],[Reputation],[UpVotes],[Views],[WebsiteUrl],[AccountId])
select distinct b.UserId, null, 22, @now, 'user' + cast(b.UserId as nvarchar), 0, null, @now, 'Space', 1, 1, 1, null, null
from dbo.Badges b 
where b.UserId not in (select u.Id from dbo.Users u) 

INSERT INTO [dbo].[Users]
([Id],[AboutMe],[Age],[CreationDate],[DisplayName],[DownVotes],[EmailHash],[LastAccessDate],[Location],[Reputation],[UpVotes],[Views],[WebsiteUrl],[AccountId])
select distinct c.UserId, null, 22, @now, 'user' + cast(c.UserId as nvarchar), 0, null, @now, 'Space', 1, 1, 1, null, null
from dbo.Comments c 
where c.UserId not in (select u.Id from dbo.Users u) 

INSERT INTO [dbo].[Users]
([Id],[AboutMe],[Age],[CreationDate],[DisplayName],[DownVotes],[EmailHash],[LastAccessDate],[Location],[Reputation],[UpVotes],[Views],[WebsiteUrl],[AccountId])
select distinct v.UserId, null, 22, @now, 'user' + cast(v.UserId as nvarchar), 0, null, @now, 'Space', 1, 1, 1, null, null
from dbo.Votes v 
where v.UserId not in (select u.Id from dbo.Users u) 
GO

declare @now datetime = getdate();

INSERT INTO [dbo].[Posts] ([Body],[CreationDate],[LastActivityDate],[PostTypeId],[Score],[ViewCount])
values ('And the answer for your question is ...', @now, @now, 2, 1, 1)

declare @answer_placeholder_id int = SCOPE_IDENTITY();

INSERT INTO [dbo].[Posts] ([Body],[CreationDate],[LastActivityDate],[PostTypeId],[Score],[ViewCount])
values ('I am your father', @now, @now, 1, 1, 1)

declare @parent_placeholder_id int = SCOPE_IDENTITY();

INSERT INTO [dbo].[Posts] ([Body],[CreationDate],[LastActivityDate],[PostTypeId],[Score],[ViewCount])
values ('This is question about nothing so quite important. is it?', @now, @now, 1, 1, 1)

declare @generic_post_placeholder_id int = SCOPE_IDENTITY();

update dbo.Posts
set AcceptedAnswerId = @answer_placeholder_id
where AcceptedAnswerId not in (select i.Id from dbo.Posts i)

update dbo.Posts
set ParentId = @parent_placeholder_id
where ParentId not in (select i.Id from dbo.Posts i)

update dbo.PostLinks
set PostId = @generic_post_placeholder_id
where PostId not in (select i.Id from dbo.Posts i)

update dbo.PostLinks
set RelatedPostId = @generic_post_placeholder_id
where RelatedPostId not in (select i.Id from dbo.Posts i)

update dbo.Comments
set PostId = @generic_post_placeholder_id
where PostId not in (select i.Id from dbo.Posts i)

update dbo.Comments
set PostId = @generic_post_placeholder_id
where PostId not in (select i.Id from dbo.Posts i)

GO

SET IDENTITY_INSERT [dbo].[Users] OFF;  
GO 

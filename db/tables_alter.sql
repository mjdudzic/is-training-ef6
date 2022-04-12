alter table dbo.Votes add RowVersion timestamp not null
go

alter table dbo.PostLinks add IsDeleted bit null
go
update dbo.PostLinks set IsDeleted=0
go
alter table dbo.PostLinks alter column IsDeleted bit not null
go
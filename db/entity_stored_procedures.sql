CREATE PROCEDURE [dbo].[InsertBadges]
    @Name nvarchar(40),
    @UserId int,
	@Date datetime
AS
BEGIN
    SET NOCOUNT ON;

        INSERT INTO [dbo].[Badges]([Name],[UserId],[Date])
        VALUES(@Name, @UserId, @Date)

    SELECT SCOPE_IDENTITY() AS Id

END
GO

CREATE PROCEDURE [dbo].[UpdateBadges]
    @Id int,
	@Name nvarchar(40),
    @UserId int,
	@Date datetime
AS
BEGIN
    SET NOCOUNT ON;

        UPDATE [dbo].[Badges] SET
		[Name] = @Name,
		[UserId] = @UserId,
		[Date] = @Date
        WHERE [Id] = @Id
END
GO

CREATE PROCEDURE [dbo].[DeleteBadges]
    @Id int
AS
BEGIN
    SET NOCOUNT ON;

        DELETE FROM [dbo].[Badges] 
        WHERE [Id] = @Id
END
GO
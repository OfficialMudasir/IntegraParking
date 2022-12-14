USE [master]
GO
/****** Object:  Database [integraMobile]    Script Date: 08/10/2013 12:29:18 ******/
CREATE DATABASE [integraMobile] ON  PRIMARY 
( NAME = N'integraMobile', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL10.SQLEXPRESS2008\MSSQL\DATA\integraMobile.mdf' , SIZE = 1048576KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'integraMobile_log', FILENAME = N'c:\Program Files (x86)\Microsoft SQL Server\MSSQL10.SQLEXPRESS2008\MSSQL\DATA\integraMobile_log.ldf' , SIZE = 181184KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [integraMobile] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [integraMobile].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [integraMobile] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [integraMobile] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [integraMobile] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [integraMobile] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [integraMobile] SET ARITHABORT OFF 
GO
ALTER DATABASE [integraMobile] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [integraMobile] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [integraMobile] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [integraMobile] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [integraMobile] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [integraMobile] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [integraMobile] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [integraMobile] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [integraMobile] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [integraMobile] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [integraMobile] SET  DISABLE_BROKER 
GO
ALTER DATABASE [integraMobile] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [integraMobile] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [integraMobile] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [integraMobile] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [integraMobile] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [integraMobile] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [integraMobile] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [integraMobile] SET RECOVERY FULL 
GO
ALTER DATABASE [integraMobile] SET  MULTI_USER 
GO
ALTER DATABASE [integraMobile] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [integraMobile] SET DB_CHAINING OFF 
GO
USE [integraMobile]
GO
/****** Object:  User [integraMobile]    Script Date: 08/10/2013 12:29:18 ******/
CREATE USER [integraMobile] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [integraMaintenance]    Script Date: 08/10/2013 12:29:18 ******/
CREATE USER [integraMaintenance] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [aspnet_WebEvent_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_WebEvent_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Roles_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Roles_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Roles_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Profile_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Profile_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Profile_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Personalization_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Personalization_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Membership_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Membership_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE ROLE [aspnet_Membership_BasicAccess]
GO
ALTER ROLE [db_owner] ADD MEMBER [integraMobile]
GO
ALTER ROLE [db_datareader] ADD MEMBER [integraMobile]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [integraMobile]
GO
ALTER ROLE [db_owner] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_datareader] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [integraMaintenance]
GO
ALTER ROLE [aspnet_Roles_BasicAccess] ADD MEMBER [aspnet_Roles_FullAccess]
GO
ALTER ROLE [aspnet_Roles_ReportingAccess] ADD MEMBER [aspnet_Roles_FullAccess]
GO
ALTER ROLE [aspnet_Profile_BasicAccess] ADD MEMBER [aspnet_Profile_FullAccess]
GO
ALTER ROLE [aspnet_Profile_ReportingAccess] ADD MEMBER [aspnet_Profile_FullAccess]
GO
ALTER ROLE [aspnet_Personalization_BasicAccess] ADD MEMBER [aspnet_Personalization_FullAccess]
GO
ALTER ROLE [aspnet_Personalization_ReportingAccess] ADD MEMBER [aspnet_Personalization_FullAccess]
GO
ALTER ROLE [aspnet_Membership_BasicAccess] ADD MEMBER [aspnet_Membership_FullAccess]
GO
ALTER ROLE [aspnet_Membership_ReportingAccess] ADD MEMBER [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Membership_BasicAccess]
GO
/****** Object:  Schema [aspnet_Membership_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Membership_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Personalization_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Personalization_BasicAccess]
GO
/****** Object:  Schema [aspnet_Personalization_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Personalization_FullAccess]
GO
/****** Object:  Schema [aspnet_Personalization_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Profile_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Profile_BasicAccess]
GO
/****** Object:  Schema [aspnet_Profile_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Profile_FullAccess]
GO
/****** Object:  Schema [aspnet_Profile_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Profile_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Roles_BasicAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Roles_BasicAccess]
GO
/****** Object:  Schema [aspnet_Roles_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Roles_FullAccess]
GO
/****** Object:  Schema [aspnet_Roles_ReportingAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_Roles_ReportingAccess]
GO
/****** Object:  Schema [aspnet_WebEvent_FullAccess]    Script Date: 08/10/2013 12:29:18 ******/
CREATE SCHEMA [aspnet_WebEvent_FullAccess]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_AnyDataInTables]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N'aspnet_Membership'
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N'aspnet_Roles'
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N'aspnet_Profile'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N'aspnet_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N'aspnet_WebEvent_Events'
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N'aspnet_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N'aspnet_Applications'
            RETURN
        END
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Applications_CreateApplication]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_CheckSchemaVersion]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        -- select user ID from aspnet_users table
        SELECT TOP 1 @UserId = u.UserId
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId

        SELECT m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut, m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  @UserId = u.UserId AND u.UserId = m.UserId 
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Paths_CreatePath]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM dbo.aspnet_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT dbo.aspnet_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM dbo.aspnet_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Personalization_GetApplicationId]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM aspnet_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM aspnet_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_FindState]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM dbo.aspnet_Paths Paths,
             ((SELECT Paths.PathId
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM dbo.aspnet_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM aspnet_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetUserState]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationAllUsers p WHERE p.PathId = @PathId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteInactiveProfiles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT  0
        RETURN
    END

    DELETE
    FROM    dbo.aspnet_Profile
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    dbo.aspnet_Users u
                WHERE   ApplicationId = @ApplicationId
                        AND (LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteProfiles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
    @ApplicationName        nvarchar(256),
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N',', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC dbo.aspnet_Users_DeleteUser @ApplicationName, @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT 0
        RETURN
    END

    SELECT  COUNT(*)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
    WHERE   ApplicationId = @ApplicationId
        AND u.UserId = p.UserId
        AND (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProfiles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
        WHERE   ApplicationId = @ApplicationId
            AND u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProperties]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProperties]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         dbo.aspnet_Profile
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_SetProperties]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, @IsUserAnonymous, @LastActivityDate, @UserId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   dbo.aspnet_Profile
               WHERE  UserId = @UserId))
        UPDATE dbo.aspnet_Profile
        SET    PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString,
               PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO dbo.aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate)
             VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_RegisterSchemaVersion]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RemoveAllRoleMembers]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RestorePermissions]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UnRegisterSchemaVersion]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_CreateUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_DeleteUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_UsersInRoles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
GO
/****** Object:  StoredProcedure [dbo].[aspnet_WebEvent_LogEvent]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        dbo.aspnet_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END
GO
/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COUNTRIES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COUNTRIES](
	[COU_ID] [numeric](18, 0) NOT NULL,
	[COU_DESCRIPTION] [varchar](50) NOT NULL,
	[COU_CODE] [varchar](10) NULL,
	[COU_TEL_PREFIX] [varchar](10) NULL,
	[COU_CUR_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_COUNTRIES] PRIMARY KEY CLUSTERED 
(
	[COU_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CURRENCIES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CURRENCIES](
	[CUR_ID] [numeric](18, 0) NOT NULL,
	[CUR_NAME] [varchar](50) NOT NULL,
	[CUR_ISO_CODE] [varchar](10) NOT NULL,
 CONSTRAINT [PK_CURRENCIES_1] PRIMARY KEY CLUSTERED 
(
	[CUR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTOMER_INSCRIPTIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTOMER_INSCRIPTIONS](
	[CUSINS_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CUSINS_NAME] [varchar](50) NOT NULL,
	[CUSINS_SURNAME1] [varchar](50) NOT NULL,
	[CUSINS_SURNAME2] [varchar](50) NULL,
	[CUSINS_DOC_ID] [varchar](50) NOT NULL,
	[CUSINS_MAIN_TEL_COUNTRY] [numeric](18, 0) NOT NULL,
	[CUSINS_MAIN_TEL] [varchar](50) NOT NULL,
	[CUSINS_SECUND_TEL_COUNTRY] [numeric](18, 0) NULL,
	[CUSINS_SECUND_TEL] [varchar](50) NULL,
	[CUSINS_EMAIL] [varchar](50) NOT NULL,
	[CUSINS_ACTIVATION_CODE] [varchar](20) NOT NULL,
	[CUSINS_URL_PARAMETER] [varchar](100) NOT NULL,
	[CUSINS_ACTIVATION_RETRIES] [numeric](18, 0) NULL,
	[CUSINS_LAST_SENT_DATE] [datetime] NULL,
	[CUISINS_CULTURE] [varchar](20) NOT NULL,
	[CUISINS_CUS_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CUSTOMER_INSCRIPTIONS] PRIMARY KEY CLUSTERED 
(
	[CUSINS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTOMER_INVOICES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTOMER_INVOICES](
	[CUSINV_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CUSINV_CUS_ID] [numeric](18, 0) NOT NULL,
	[CUSINV_OPR_ID] [numeric](18, 0) NOT NULL,
	[CUSINV_INV_NUMBER] [varchar](50) NOT NULL,
	[CUSINV_INV_DATE] [datetime] NOT NULL,
	[CUSINV_GENERATION_DATE] [datetime] NOT NULL,
	[CUSINV_DATEINI] [datetime] NOT NULL,
	[CUSINV_DATEEND] [datetime] NOT NULL,
	[CUSINV_OPR_INITIAL_INVOICE_NUMBER] [numeric](18, 0) NOT NULL,
	[CUSINV_OPR_END_INVOICE_NUMBER] [numeric](18, 0) NOT NULL,
	[CUSINV_INV_AMOUNT] [int] NOT NULL,
	[CUSINV_CUR_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_CUSTOMER_INVOICES] PRIMARY KEY CLUSTERED 
(
	[CUSINV_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTOMER_PAYMENT_MEANS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTOMER_PAYMENT_MEANS](
	[CUSPM_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CUSPM_CUS_ID] [numeric](18, 0) NOT NULL,
	[CUSPM_PAT_ID] [int] NOT NULL,
	[CUSPM_PAST_ID] [int] NOT NULL,
	[CUSPM_DESCRIPTION] [varchar](50) NULL,
	[CUSPM_LAST_TIME_USERD] [datetime] NULL,
	[CUSPM_AUTOMATIC_RECHARGE] [int] NOT NULL,
	[CUSPM_AMOUNT_TO_RECHARGE] [int] NULL,
	[CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS] [int] NULL,
	[CUSPM_CUR_ID] [numeric](18, 0) NOT NULL,
	[CUSPM_TOKEN_CARD_HASH] [varchar](100) NULL,
	[CUSPM_TOKEN_CARD_REFERENCE] [varchar](100) NULL,
	[CUSPM_TOKEN_MASKED_CARD_NUMBER] [varchar](50) NULL,
	[CUSPM_TOKEN_CARD_EXPIRATION_DATE] [datetime] NULL,
	[CUSPM_TOKEN_CARD_SCHEMA] [varchar](50) NULL,
	[CUSPM_TOKEN_PAYPAL_ID] [varchar](100) NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY] [varchar](100) NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_START_DATE] [datetime] NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_END_DATE] [datetime] NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_NUMBER_PAYMENTS] [int] NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_TOTAL_AMOUNT] [numeric](18, 2) NULL,
	[CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_AMOUNT_PER_PAYMENT] [numeric](18, 2) NULL,
	[CUSPM_AUTOMATIC_FAILED_RETRIES] [int] NOT NULL,
	[CUSPM_VALID] [int] NOT NULL,
	[CUSPM_ENABLED] [int] NOT NULL,
 CONSTRAINT [PK_CUSTOMER_PAYMENT_MEANS] PRIMARY KEY CLUSTERED 
(
	[CUSPM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES](
	[CUSPMR_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CUSPMR_CUS_ID] [numeric](18, 0) NULL,
	[CUSPMR_CUSPM_ID] [numeric](18, 0) NULL,
	[CUSPMR_RCOUP_ID] [numeric](18, 0) NULL,
	[CUSPMR_USR_ID] [numeric](18, 0) NULL,
	[CUSPMR_AMOUNT] [int] NOT NULL,
	[CUSPMR_CUR_ID] [numeric](18, 0) NOT NULL,
	[CUSPMR_DATE] [datetime] NULL,
	[CUSPMR_OP_REFERENCE] [varchar](50) NULL,
	[CUSPMR_TRANSACTION_ID] [varchar](50) NOT NULL,
	[CUSPMR_GATEWAY_DATE] [varchar](50) NOT NULL,
	[CUSPMR_AUTH_CODE] [varchar](50) NULL,
	[CUSPMR_AUTH_RESULT] [varchar](50) NULL,
	[CUSPMR_SECOND_OP_REFERENCE] [varchar](50) NULL,
	[CUSPMR_SECOND_TRANSACTION_ID] [varchar](50) NULL,
	[CUSPMR_SECOND_GATEWAY_DATE] [varchar](50) NULL,
	[CUSPMR_SECOND_AUTH_RESULT] [varchar](50) NULL,
	[CUSPMR_CARD_HASH] [varchar](50) NULL,
	[CUSPMR_CARD_REFERENCE] [varchar](50) NULL,
	[CUSPMR_CARD_SCHEME] [varchar](50) NULL,
	[CUSPMR_MASKED_CARD_NUMBER] [varchar](50) NULL,
	[CUSPMR_CARD_EXPIRATION_DATE] [datetime] NULL,
	[CUSPMR_PAYPAL_3T_TOKEN] [varchar](50) NULL,
	[CUSPMR_PAYPAL_3T_PAYER_ID] [varchar](50) NULL,
	[CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY] [varchar](50) NULL,
	[CUSPMR_PAT_FIXED_FEE] [numeric](18, 0) NOT NULL,
	[CUSPMR_PAT_PERC_FEE] [numeric](18, 5) NOT NULL,
	[CUSPMR_PAST_FIXED_FEE] [numeric](18, 0) NOT NULL,
	[CUSPMR_PAST_PERC_FEE] [numeric](18, 5) NOT NULL,
	[CUSPMR_PERC_VAT] [numeric](18, 5) NOT NULL,
	[CUSPMR_TOTAL_AMOUNT_CHARGED] [numeric](18, 0) NOT NULL,
	[CUSPMR_SUSCRIPTION_TYPE] [int] NOT NULL,
	[CUSPMR_BALANCE_BEFORE] [int] NOT NULL,
	[CUSPMR_TRANS_STATUS] [int] NOT NULL,
	[CUSPMR_STATUS_DATE] [datetime] NOT NULL,
	[CUSPMR_RETRIES_NUM] [int] NOT NULL,
	[CUSPMR_CUSINV_ID] [numeric](18, 0) NULL,
	[CUSPMR_INSERTION_UTC_DATE] [datetime] NULL,
 CONSTRAINT [PK_CUSTOMER_PAYMENT_MEANS_RECHARGES] PRIMARY KEY CLUSTERED 
(
	[CUSPMR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTOMERS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTOMERS](
	[CUS_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CUS_TYPE] [numeric](3, 0) NOT NULL,
	[CUS_USR_ID] [numeric](18, 0) NULL,
	[CUS_COU_ID] [numeric](18, 0) NOT NULL,
	[CUS_DOC_ID] [varchar](50) NOT NULL,
	[CUS_DOC_ID_TYPE] [numeric](3, 0) NOT NULL,
	[CUS_NAME] [varchar](50) NOT NULL,
	[CUS_SURNAME1] [varchar](50) NULL,
	[CUS_SURNAME2] [varchar](50) NULL,
	[CUS_STREET] [varchar](50) NOT NULL,
	[CUS_STREE_NUMBER] [int] NOT NULL,
	[CUS_LEVEL_NUM] [int] NULL,
	[CUS_DOOR] [varchar](10) NULL,
	[CUS_LETTER] [varchar](10) NULL,
	[CUS_STAIR] [varchar](10) NULL,
	[CUS_CITY] [varchar](50) NOT NULL,
	[CUS_STATE] [varchar](50) NOT NULL,
	[CUS_ZIPCODE] [varchar](20) NOT NULL,
	[CUS_ENABLED] [int] NOT NULL,
 CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY CLUSTERED 
(
	[CUS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EXTERNAL_PARKING_OPERATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EXTERNAL_PARKING_OPERATIONS](
	[EPO_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[EPO_INS_ID] [numeric](18, 0) NOT NULL,
	[EPO_PLATE] [varchar](50) NOT NULL,
	[EPO_ZONE] [numeric](18, 0) NULL,
	[EPO_TARIFF] [numeric](18, 0) NULL,
	[EPO_INIDATE] [datetime] NULL,
	[EPO_ENDDATE] [datetime] NOT NULL,
	[EPO_INIDATE_UTC] [datetime] NULL,
	[EPO_ENDDATE_UTC] [datetime] NOT NULL,
	[EPO_AMOUNT] [int] NULL,
	[EPO_TIME] [int] NULL,
	[EPO_INSERTION_NOTIFIED] [int] NOT NULL,
	[EPO_ENDING_NOTIFIED] [int] NOT NULL,
 CONSTRAINT [PK_EXTERNAL_PARKING_OPERATIONS] PRIMARY KEY CLUSTERED 
(
	[EPO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EXTERNAL_TICKET]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EXTERNAL_TICKET](
	[EXTI_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[EXTI_INS_ID] [numeric](18, 0) NOT NULL,
	[EXTI_PLATE] [varchar](50) NOT NULL,
	[EXTI_DATE] [datetime] NOT NULL,
	[EXTI_DATE_UTC] [datetime] NOT NULL,
	[EXTI_TICKET_NUMBER] [varchar](50) NOT NULL,
	[EXTI_AMOUNT] [int] NULL,
	[EXTI_LIMIT_DATE] [datetime] NOT NULL,
	[EXTI_LIMIT_DATE_UTC] [datetime] NOT NULL,
	[EXTI_ARTICLE_TYPE] [varchar](512) NULL,
	[EXTI_ARTICLE_DESCRIPTION] [varchar](512) NULL,
	[EXTI_INSERTION_NOTIFIED] [int] NOT NULL,
 CONSTRAINT [PK_EXTERNAL_TICKET] PRIMARY KEY CLUSTERED 
(
	[EXTI_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GROUPS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GROUPS](
	[GRP_ID] [numeric](18, 0) NOT NULL,
	[GRP_DESCRIPTION] [varchar](50) NOT NULL,
	[GRP_INS_ID] [numeric](18, 0) NOT NULL,
	[GRP_LIT_ID] [numeric](18, 0) NOT NULL,
	[GRP_EXT_ID] [varchar](20) NULL,
	[GRP_EXT2_ID] [varchar](20) NULL,
 CONSTRAINT [PK_GROUPS] PRIMARY KEY CLUSTERED 
(
	[GRP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GROUPS_GEOMETRY]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GROUPS_GEOMETRY](
	[GRGE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[GRGE_GRP_ID] [numeric](18, 0) NOT NULL,
	[GRGE_ORDER] [numeric](18, 0) NOT NULL,
	[GRGE_LATITUDE] [numeric](18, 12) NOT NULL,
	[GRGE_LONGITUDE] [numeric](18, 12) NOT NULL,
	[GRGE_INI_APPLY_DATE] [datetime] NOT NULL,
	[GRGE_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_GROUPS_GEOMETRY] PRIMARY KEY CLUSTERED 
(
	[GRGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GROUPS_HIERARCHY]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GROUPS_HIERARCHY](
	[GRHI_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[GRHI_GPR_ID_PARENT] [numeric](18, 0) NULL,
	[GRHI_GPR_ID_CHILD] [numeric](18, 0) NOT NULL,
	[GRHI_INI_APPLY_DATE] [datetime] NOT NULL,
	[GRHI_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_GROUPS_HIERARCHY] PRIMARY KEY CLUSTERED 
(
	[GRHI_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS](
	[GTET_IN_GRP_ID] [numeric](18, 0) NOT NULL,
	[GTET_IN_TAR_ID] [numeric](18, 0) NOT NULL,
	[GTET_OUT_GRP_EXT_ID] [varchar](20) NOT NULL,
	[GTET_OUT_TAR_EXT_ID] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GROUPS_TYPES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GROUPS_TYPES](
	[GRPT_ID] [numeric](18, 0) NOT NULL,
	[GRPT_INS_ID] [numeric](18, 0) NOT NULL,
	[GRPT_DESCRIPTION] [varchar](50) NULL,
 CONSTRAINT [PK_GROUPS_TYPES] PRIMARY KEY CLUSTERED 
(
	[GRPT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GROUPS_TYPES_ASSIGNATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GROUPS_TYPES_ASSIGNATIONS](
	[GTA_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[GTA_DESCRIPTION] [varchar](50) NOT NULL,
	[GTA_GRP_ID] [numeric](18, 0) NOT NULL,
	[GTA_GRPT_ID] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_GROUPS_TYPES_ASSIGNATIONS] PRIMARY KEY CLUSTERED 
(
	[GTA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[INSTALLATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[INSTALLATIONS](
	[INS_ID] [numeric](18, 0) NOT NULL,
	[INS_DESCRIPTION] [varchar](50) NOT NULL,
	[INS_SHORTDESC] [varchar](50) NOT NULL,
	[INS_OPR_ID] [numeric](18, 0) NOT NULL,
	[INS_CUR_ID] [numeric](18, 0) NOT NULL,
	[INS_LIT_ID] [numeric](18, 0) NOT NULL,
	[INS_VERSION] [int] NOT NULL,
	[INS_TAR_VERSION] [int] NOT NULL,
	[INS_TIMEZONE_ID] [varchar](50) NOT NULL,
	[INS_CULTURE_LANG] [varchar](20) NOT NULL,
	[INS_FINE_WS_SIGNATURE_TYPE] [int] NOT NULL,
	[INS_FINE_WS_URL] [varchar](500) NULL,
	[INS_FINE_WS_AUTH_HASH_KEY] [varchar](50) NULL,
	[INS_FINE_WS_HTTP_USER] [varchar](50) NULL,
	[INS_FINE_WS_HTTP_PASSWORD] [varchar](50) NULL,
	[INS_PARK_WS_SIGNATURE_TYPE] [int] NOT NULL,
	[INS_PARK_WS_URL] [varchar](500) NULL,
	[INS_PARK_WS_AUTH_HASH_KEY] [varchar](50) NULL,
	[INS_PARK_WS_HTTP_USER] [varchar](50) NULL,
	[INS_PARK_WS_HTTP_PASSWORD] [varchar](50) NULL,
	[INS_PARK_CONFIRM_WS_SIGNATURE_TYPE] [int] NOT NULL,
	[INS_PARK_CONFIRM_WS_URL] [varchar](500) NULL,
	[INS_PARK_CONFIRM_WS_AUTH_HASH_KEY] [varchar](50) NULL,
	[INS_PARK_CONFIRM_WS_HTTP_USER] [varchar](50) NULL,
	[INS_PARK_CONFIRM_WS_HTTP_PASSWORD] [varchar](50) NULL,
	[INS_PLATE_UPDATE_WS_SIGNATURE_TYPE] [int] NOT NULL,
	[INS_PLATE_UPDATE_WS_URL] [varchar](500) NULL,
	[INS_PLATE_UPDATE_WS_AUTH_HASH_KEY] [varchar](50) NULL,
	[INS_STANDARD_3rdP_CITY_ID] [varchar](50) NULL,
	[INS_EYSA_CONTRATA_ID] [varchar](50) NULL,
	[INS_EYSA_USER_NAME] [varchar](50) NULL,
	[INS_EYSA_SERIAL_NUMBER] [numeric](18, 0) NULL,
	[INS_EYSA_USER_ID] [varchar](50) NULL,
	[INS_EYSA_PASSWORD] [varchar](50) NULL,
	[INS_EYSA_COMPANY_NAME_TO_SEND] [varchar](50) NULL,
	[INS_OPT_PARK] [int] NOT NULL,
	[INS_OPT_UNPARK] [int] NOT NULL,
	[INS_OPT_TICKET] [int] NOT NULL,
	[INS_OPT_RECHARGE] [int] NOT NULL,
	[INS_OPT_MORE_FUNCTS] [int] NOT NULL,
	[INS_OPT_CUR_OPERS] [int] NOT NULL,
	[INS_OPT_HIS_OPERS] [int] NOT NULL,
	[INS_OPT_OCUP] [int] NOT NULL,
	[INS_OPT_PARKBYPLATE] [int] NOT NULL,
	[INS_OPT_PARKBYPLATELISTTYPE] [int] NOT NULL,
	[INS_OPT_PARKBYSPACE] [int] NOT NULL,
	[INS_OPT_PARKBYZONEANDSECTOR] [int] NOT NULL,
	[INS_OPT_PARKBYQR] [int] NOT NULL,
	[INS_OPT_PARKBYMAP] [int] NOT NULL,
	[INS_OPT_PARKBYSPACEFORMAT] [int] NOT NULL,
	[INS_OPT_PARKBYSPACEFORMAT_FORMAT] [int] NOT NULL,
	[INS_OPT_PARKISZONEMANDATORY] [int] NOT NULL,
	[INS_OPT_PARKISSECTORMANDATORY] [int] NOT NULL,
	[INS_OPT_PARKZONECRIT] [int] NOT NULL,
	[INS_OPT_PARKPAYBYQRFORMATMANDATORY] [int] NOT NULL,
	[INS_OPT_PARKPAYBYQRFORMAT] [int] NOT NULL,
	[INS_OPT_PARKMULTITARIFF_NUM] [int] NOT NULL,
	[INS_OPT_TICKET_NUM] [int] NOT NULL,
	[INS_OPT_TICKET_QR] [int] NOT NULL,
	[INS_OPT_RECHARGE_QR] [int] NOT NULL,
	[INS_OPT_RECHARGE_CODE] [int] NOT NULL,
	[INS_OPT_RECHARGE_PAYMENT_MEAN] [int] NOT NULL,
	[INS_OPT_QRPARKZONEFORMAT] [int] NULL,
	[INS_OPT_QRTICKETFORMAT] [int] NULL,
	[INS_OPT_QRRECHARGEFORMAT] [int] NULL,
 CONSTRAINT [PK_INSTALLATIONS] PRIMARY KEY CLUSTERED 
(
	[INS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[INSTALLATIONS_GEOMETRY]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INSTALLATIONS_GEOMETRY](
	[INSGE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[INSGE_INS_ID] [numeric](18, 0) NOT NULL,
	[INSGE_ORDER] [numeric](18, 0) NOT NULL,
	[INSGE_LATITUDE] [numeric](18, 12) NOT NULL,
	[INSGE_LONGITUDE] [numeric](18, 12) NOT NULL,
	[INSGE_INI_APPLY_DATE] [datetime] NOT NULL,
	[INSGE_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_INSTALLATIONS_GEOMETRY] PRIMARY KEY CLUSTERED 
(
	[INSGE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LANGUAGES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LANGUAGES](
	[LAN_ID] [numeric](18, 0) NOT NULL,
	[LAN_DESCRIPTION] [varchar](50) NOT NULL,
	[LAN_CULTURE] [varchar](20) NOT NULL,
 CONSTRAINT [PK_LANGUAGES] PRIMARY KEY CLUSTERED 
(
	[LAN_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LITERAL_LANGUAGES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LITERAL_LANGUAGES](
	[LITL_LIT_ID] [numeric](18, 0) NOT NULL,
	[LITL_LAN_ID] [numeric](18, 0) NOT NULL,
	[LITL_LITERAL] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_LITERALS] PRIMARY KEY CLUSTERED 
(
	[LITL_LIT_ID] ASC,
	[LITL_LAN_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LITERALS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LITERALS](
	[LIT_ID] [numeric](18, 0) NOT NULL,
	[LIT_DESCRIPTION] [varchar](50) NULL,
 CONSTRAINT [PK_LITERALS_1] PRIMARY KEY CLUSTERED 
(
	[LIT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MOBILE_SESSIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MOBILE_SESSIONS](
	[MOSE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[MOSE_SESSIONID] [varchar](50) NOT NULL,
	[MOSE_USR_ID] [numeric](18, 0) NOT NULL,
	[MOSE_CREATION_TIME] [datetime] NOT NULL,
	[MOSE_LAST_UPDATE_TIME] [datetime] NOT NULL,
	[MOSE_CELL_WIFI_MAC] [varchar](100) NULL,
	[MOSE_CELL_IMEI] [varchar](100) NULL,
	[MOSE_CELL_MODEL] [varchar](100) NULL,
	[MOSE_OS] [int] NULL,
	[MOSE_OS_VERSION] [varchar](100) NULL,
	[MOSE_STATUS] [int] NOT NULL,
 CONSTRAINT [PK_MOBILE_SESSIONS] PRIMARY KEY CLUSTERED 
(
	[MOSE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OPERATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OPERATIONS](
	[OPE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[OPE_USR_ID] [numeric](18, 0) NOT NULL,
	[OPE_USRP_ID] [numeric](18, 0) NULL,
	[OPE_INS_ID] [numeric](18, 0) NOT NULL,
	[OPE_TYPE] [int] NOT NULL,
	[OPE_GRP_ID] [numeric](18, 0) NULL,
	[OPE_TAR_ID] [numeric](18, 0) NULL,
	[OPE_DATE] [datetime] NOT NULL,
	[OPE_INIDATE] [datetime] NOT NULL,
	[OPE_ENDDATE] [datetime] NOT NULL,
	[OPE_AMOUNT] [int] NOT NULL,
	[OPE_TIME] [int] NOT NULL,
	[OPE_AMOUNT_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPE_BALANCE_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPE_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPE_CHANGE_FEE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPE_FINAL_AMOUNT] [int] NOT NULL,
	[OPE_EXTERNAL_ID] [varchar](50) NULL,
	[OPE_INSERTION_UTC_DATE] [datetime] NULL,
	[OPE_CUSPMR_ID] [numeric](18, 0) NULL,
	[OPE_OPEDIS_ID] [numeric](18, 0) NULL,
	[OPE_BALANCE_BEFORE] [int] NOT NULL,
	[OPE_SUSCRIPTION_TYPE] [int] NOT NULL,
 CONSTRAINT [PK_OPERATIONS] PRIMARY KEY CLUSTERED 
(
	[OPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OPERATIONS_DISCOUNTS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OPERATIONS_DISCOUNTS](
	[OPEDIS_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[OPEDIS_USR_ID] [numeric](18, 0) NOT NULL,
	[OPEDIS_DATE] [datetime] NOT NULL,
	[OPEDIS_AMOUNT] [int] NOT NULL,
	[OPEDIS_AMOUNT_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPEDIS_BALANCE_CUR_ID] [numeric](18, 0) NOT NULL,
	[OPEDIS_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPEDIS_CHANGE_FEE_APPLIED] [numeric](18, 6) NOT NULL,
	[OPEDIS_FINAL_AMOUNT] [int] NOT NULL,
	[OPEDIS_INSERTION_UTC_DATE] [datetime] NULL,
	[OPEDIS_SUSCRIPTION_TYPE] [int] NOT NULL,
	[OPEDIS_BALANCE_BEFORE] [int] NOT NULL,
 CONSTRAINT [PK_OPERATIONS_DISCOUNTS] PRIMARY KEY CLUSTERED 
(
	[OPEDIS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OPERATIONS_SESSION_INFO]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OPERATIONS_SESSION_INFO](
	[OSI_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[OSI_MOSE_ID] [numeric](18, 0) NOT NULL,
	[OSI_UTC_DATE] [datetime] NOT NULL,
	[OSI_PLATE] [varchar](50) NOT NULL,
	[OSI_GRP_ID] [numeric](18, 0) NOT NULL,
	[OSI_TAR_ID] [numeric](18, 0) NOT NULL,
	[OSI_OPE_TYPE] [int] NOT NULL,
	[OSI_INS_DATE] [datetime] NOT NULL,
	[OSI_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_OPERATIONS_SESSION_INFO] PRIMARY KEY CLUSTERED 
(
	[OSI_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OPERATORS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OPERATORS](
	[OPR_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[OPR_DESCRIPTION] [varchar](50) NULL,
	[OPR_VAT_PERC] [numeric](18, 5) NOT NULL,
	[OPR_INITIAL_INVOICE_NUMBER] [numeric](18, 0) NOT NULL,
	[OPR_END_INVOICE_NUMBER] [numeric](18, 0) NOT NULL,
	[OPR_CURRENT_INVOICE_NUMBER] [numeric](18, 0) NOT NULL,
	[OPR_VAT_NUMBER] [varchar](50) NULL,
	[OPR_NAME_FOR_INVOICE] [varchar](100) NULL,
	[OPR_ADDRESS_FOR_INVOICE] [varchar](200) NULL,
	[OPR_INVOICE_NUMBER_FORMAT] [varchar](50) NULL,
	[OPR_INVOICE_FORMAT_LAST_PAGE_FILE] [varchar](255) NULL,
	[OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE] [varchar](255) NULL,
 CONSTRAINT [PK_OPERATORS] PRIMARY KEY CLUSTERED 
(
	[OPR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PARAMETERS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PARAMETERS](
	[PAR_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PAR_NAME] [varchar](100) NOT NULL,
	[PAR_VALUE] [varchar](1024) NULL,
	[PAR_DESCRIPTION] [varchar](1024) NULL,
 CONSTRAINT [PK_PARAMETERS] PRIMARY KEY CLUSTERED 
(
	[PAR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAYMENT_SUBTYPES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAYMENT_SUBTYPES](
	[PAST_ID] [int] NOT NULL,
	[PAST_PAT_ID] [int] NOT NULL,
	[PAST_DESCRIPTION] [varchar](50) NOT NULL,
	[PAST_LIT_ID] [numeric](18, 0) NULL,
	[PAST_FIXED_FEE] [numeric](18, 0) NULL,
	[PAST_PERC_FEE] [numeric](18, 5) NULL,
 CONSTRAINT [PK_PAYMENT_SUBTYPES] PRIMARY KEY CLUSTERED 
(
	[PAST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAYMENT_TYPES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAYMENT_TYPES](
	[PAT_ID] [int] NOT NULL,
	[PAT_DESCRIPCION] [varchar](50) NOT NULL,
	[PAT_LIT_ID] [numeric](18, 0) NULL,
	[PAT_FIXED_FEE] [numeric](18, 0) NULL,
	[PAT_PERC_FEE] [numeric](18, 5) NULL,
 CONSTRAINT [PK_PAYMENT_TYPES] PRIMARY KEY CLUSTERED 
(
	[PAT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PLATES_TARIFFS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PLATES_TARIFFS](
	[PLTA_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PLTA_PLATE] [varchar](50) NOT NULL,
	[PLTA_GRP_ID] [numeric](18, 0) NULL,
	[PLTA_GRPT_ID] [numeric](18, 0) NULL,
	[PLTA_TAR_ID] [numeric](18, 0) NOT NULL,
	[PLTA_INI_APPLY_DATE] [datetime] NOT NULL,
	[PLTA_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_PLATES_TARIFFS] PRIMARY KEY CLUSTERED 
(
	[PLTA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PUSHID_NOTIFICATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PUSHID_NOTIFICATIONS](
	[PNO_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PNO_UTNO_ID] [numeric](18, 0) NOT NULL,
	[PNO_OS] [int] NOT NULL,
	[PNO_PUSHID] [varchar](512) NOT NULL,
	[PNO_WP_TEXT1] [varchar](512) NULL,
	[PNO_WP_TEXT2] [varchar](512) NULL,
	[PNO_WP_PARAM] [varchar](512) NULL,
	[PNO_WP_COUNT] [int] NULL,
	[PNO_WP_TILE_TITLE] [varchar](512) NULL,
	[PNO_WP_BACKGROUND_IMAGE] [varchar](512) NULL,
	[PNO_WP_RAW_DATA] [varchar](1024) NULL,
	[PNO_ANDROID_RAW_DATA] [varchar](1024) NULL,
	[PNO_iOS_RAW_DATA] [varchar](1024) NULL,
	[PNO_LIMITDATETIME] [datetime] NULL,
	[PNO_STATUS] [int] NOT NULL,
	[PNO_RETRIES] [int] NOT NULL,
	[PNO_LAST_RETRY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_PUSHID_TOAST_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[PNO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RECHARGE_COUPONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RECHARGE_COUPONS](
	[RCOUP_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[RCOUP_CODE] [varchar](100) NOT NULL,
	[RCOUP_COUPS_ID] [int] NOT NULL,
	[RCOUP_VALUE] [numeric](18, 0) NOT NULL,
	[RCOUP_CUR_ID] [numeric](18, 0) NOT NULL,
	[RCOUP_START_DATE] [datetime] NOT NULL,
	[RCOUP_EXP_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_RECHARGE_COUPONS] PRIMARY KEY CLUSTERED 
(
	[RCOUP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RECHARGE_COUPONS_STATUS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RECHARGE_COUPONS_STATUS](
	[RCOUPS_ID] [int] NOT NULL,
	[RCOUPS_DESCRIPTION] [varchar](50) NOT NULL,
	[RCOUPS_LIT_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_RECHARGE_COUPONS_STATUS] PRIMARY KEY CLUSTERED 
(
	[RCOUPS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RECHARGE_COUPONS_USES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RECHARGE_COUPONS_USES](
	[RCOUPU_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[RCOUPU_USR_ID] [numeric](18, 0) NOT NULL,
	[RCOUPU_RCOUP_ID] [numeric](18, 0) NOT NULL,
	[RCOUPU_DATE] [datetime] NOT NULL,
	[RCOUPU_CODE] [varchar](50) NOT NULL,
	[RCOUPU_MOSE_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_RECHARGE_COUPONS_USES] PRIMARY KEY CLUSTERED 
(
	[RCOUPU_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RECHARGE_VALUES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RECHARGE_VALUES](
	[RECHVAL_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[RECHVAL_INS_ID] [numeric](18, 0) NOT NULL,
	[RECHVAL_VALUE_TYPE] [int] NOT NULL,
	[RECHVAL_VALUE] [int] NOT NULL,
 CONSTRAINT [PK_RECHARGE_VALUES] PRIMARY KEY CLUSTERED 
(
	[RECHVAL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SERVICE_CHARGE_TYPES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERVICE_CHARGE_TYPES](
	[SECHT_ID] [int] NOT NULL,
	[SECHT_DESCRIPCION] [varchar](50) NOT NULL,
	[SECHT_LIT_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_SERVICE_CHARGE_TYPES] PRIMARY KEY CLUSTERED 
(
	[SECHT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SERVICE_CHARGES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_CHARGES](
	[SECH_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[SECH_SECHT_ID] [int] NOT NULL,
	[SECH_USR_ID] [numeric](18, 0) NOT NULL,
	[SECH_DATE] [datetime] NOT NULL,
	[SECH_AMOUNT] [int] NOT NULL,
	[SECH_AMOUNT_CUR_ID] [numeric](18, 0) NOT NULL,
	[SECH_BALANCE_CUR_ID] [numeric](18, 0) NOT NULL,
	[SECH_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
	[SECH_CHANGE_FEE_APPLIED] [numeric](18, 6) NOT NULL,
	[SECH_FINAL_AMOUNT] [int] NOT NULL,
	[SECH_INSERTION_UTC_DATE] [datetime] NULL,
	[SECH_CUSPMR_ID] [numeric](18, 0) NULL,
	[SECH_SUSCRIPTION_TYPE] [int] NOT NULL,
	[SECH_BALANCE_BEFORE] [int] NOT NULL,
 CONSTRAINT [PK_SERVICE_CHARGES] PRIMARY KEY CLUSTERED 
(
	[SECH_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TARIFFS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TARIFFS](
	[TAR_ID] [numeric](18, 0) NOT NULL,
	[TAR_DESCRIPTION] [varchar](50) NOT NULL,
	[TAR_LIT_ID] [numeric](18, 0) NOT NULL,
	[TAR_EXT_ID] [varchar](50) NULL,
 CONSTRAINT [PK_TARIFFS] PRIMARY KEY CLUSTERED 
(
	[TAR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TARIFFS_IN_GROUPS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TARIFFS_IN_GROUPS](
	[TARGR_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TARGR_GRP_ID] [numeric](18, 0) NULL,
	[TARGR_GRPT_ID] [numeric](18, 0) NULL,
	[TARGR_TAR_ID] [numeric](18, 0) NOT NULL,
	[TARGR_LIT_ID] [numeric](18, 0) NOT NULL,
	[TARGR_USER_SELECTABLE] [int] NOT NULL,
	[TARGR_INI_APPLY_DATE] [datetime] NOT NULL,
	[TARGR_END_APPLY_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_TARIFFS_IN_GROUPS] PRIMARY KEY CLUSTERED 
(
	[TARGR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TICKET_PAYMENTS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TICKET_PAYMENTS](
	[TIPA_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TIPA_USR_ID] [numeric](18, 0) NOT NULL,
	[TIPA_INS_ID] [numeric](18, 0) NOT NULL,
	[TIPA_DATE] [datetime] NOT NULL,
	[TIPA_USRP_ID] [numeric](18, 0) NULL,
	[TIPA_PLATE_STRING] [varchar](50) NULL,
	[TIPA_TICKET_NUMBER] [varchar](50) NOT NULL,
	[TIPA_TICKET_DATA] [varchar](300) NULL,
	[TIPA_AMOUNT] [int] NOT NULL,
	[TIPA_AMOUNT_CUR_ID] [numeric](18, 0) NOT NULL,
	[TIPA_BALANCE_CUR_ID] [numeric](18, 0) NOT NULL,
	[TIPA_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
	[TIPA_CHANGE_FEE_APPLIED] [numeric](18, 6) NOT NULL,
	[TIPA_FINAL_AMOUNT] [int] NOT NULL,
	[TIPA_EXTERNAL_ID] [varchar](50) NULL,
	[TIPA_INSERTION_UTC_DATE] [datetime] NULL,
	[TIPA_CUSPMR_ID] [numeric](18, 0) NULL,
	[TIPA_OPEDIS_ID] [numeric](18, 0) NULL,
	[TIPA_SUSCRIPTION_TYPE] [int] NOT NULL,
	[TIPA_BALANCE_BEFORE] [int] NOT NULL,
 CONSTRAINT [PK_TICKET_PAYMENTS] PRIMARY KEY CLUSTERED 
(
	[TIPA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TICKET_PAYMENTS_SESSION_INFO]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TICKET_PAYMENTS_SESSION_INFO](
	[TPSI_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TPSI_MOSE_ID] [numeric](18, 0) NOT NULL,
	[TPSI_UTC_DATE] [datetime] NOT NULL,
	[TPSI_TICKET_NUMBER] [varchar](50) NOT NULL,
	[TPSI_PLATE] [varchar](50) NOT NULL,
	[TPSI_AMOUNT] [numeric](18, 0) NOT NULL,
	[TPSI_ARTICLE_TYPE] [varchar](512) NULL,
	[TPSI_ARTICLE_DESCRIPTION] [varchar](512) NULL,
	[TPSI_INS_DATE] [datetime] NOT NULL,
	[TPSI_CHANGE_APPLIED] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_TICKET_PAYMENTS_SESSION_INFO] PRIMARY KEY CLUSTERED 
(
	[TPSI_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USER_PLATE_MOVS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USER_PLATE_MOVS](
	[USRPM_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USRPM_MOV_TYPE] [varchar](1) NOT NULL,
	[USRPM_DATE] [datetime] NOT NULL,
	[USRPM_PLATE] [varchar](50) NOT NULL,
	[USRPM_SEND_INSERTION] [int] NOT NULL,
 CONSTRAINT [PK_USER_PLATE_MOVS] PRIMARY KEY CLUSTERED 
(
	[USRPM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USER_PLATE_MOVS_SENDING]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_PLATE_MOVS_SENDING](
	[USRPMS_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USRPMS_USRPMD_ID] [numeric](18, 0) NOT NULL,
	[USRPMS_INS_ID] [numeric](18, 0) NOT NULL,
	[USRPMS_LAST_DATE] [datetime] NOT NULL,
	[USRPMS_STATUS] [int] NOT NULL,
 CONSTRAINT [PK_USER_PLATE_MOVS_SENDING] PRIMARY KEY CLUSTERED 
(
	[USRPMS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USER_PLATES]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USER_PLATES](
	[USRP_ID] [numeric](18, 0) IDENTITY(7,1) NOT NULL,
	[USRP_USR_ID] [numeric](18, 0) NOT NULL,
	[USRP_PLATE] [varchar](50) NOT NULL,
	[USRP_IS_DEFAULT] [int] NOT NULL,
	[USRP_ENABLED] [int] NOT NULL,
 CONSTRAINT [PK_USER_PLATES] PRIMARY KEY CLUSTERED 
(
	[USRP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS](
	[USR_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USR_CUS_ID] [numeric](18, 0) NOT NULL,
	[USR_COU_ID] [numeric](18, 0) NOT NULL,
	[USR_EMAIL] [varchar](50) NOT NULL,
	[USR_MAIN_TEL_COUNTRY] [numeric](18, 0) NOT NULL,
	[USR_MAIN_TEL] [varchar](50) NOT NULL,
	[USR_SECUND_TEL_COUNTRY] [numeric](18, 0) NULL,
	[USR_SECUND_TEL] [varchar](50) NULL,
	[USR_USERNAME] [varchar](50) NOT NULL,
	[USR_SUSCRIPTION_TYPE] [int] NULL,
	[USR_CUSPM_ID] [numeric](18, 0) NULL,
	[USR_BALANCE] [int] NOT NULL,
	[USR_CUR_ID] [numeric](18, 0) NOT NULL,
	[USR_CULTURE_LANG] [varchar](20) NOT NULL,
	[USR_UTC_OFFSET] [int] NOT NULL,
	[USR_ENABLED] [int] NOT NULL,
 CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED 
(
	[USR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS_EMAILS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS_EMAILS](
	[USRE_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USRE_USR_ID] [numeric](18, 0) NULL,
	[USRE_CUSINS_ID] [numeric](18, 0) NULL,
	[USRE_DATE] [datetime] NULL,
	[USRE_RECIPIENT_ADDRESS] [varchar](50) NOT NULL,
	[USRE_SUBJECT] [varchar](512) NOT NULL,
	[USRE_BODY] [varchar](8000) NOT NULL,
	[USRE_STATUS] [int] NOT NULL,
	[USRE_SENDER_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_USERS_EMAILS] PRIMARY KEY CLUSTERED 
(
	[USRE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS_NOTIFICATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS_NOTIFICATIONS](
	[UNO_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UNO_USR_ID] [numeric](18, 0) NOT NULL,
	[UNO_WP_TEXT1] [varchar](512) NULL,
	[UNO_WP_TEXT2] [varchar](512) NULL,
	[UNO_WP_PARAM] [varchar](512) NULL,
	[UNO_WP_COUNT] [int] NULL,
	[UNO_WP_TILE_TITLE] [varchar](512) NULL,
	[UNO_WP_BACKGROUND_IMAGE] [varchar](512) NULL,
	[UNO_WP_RAW_DATA] [varchar](1024) NULL,
	[UNO_ANDROID_RAW_DATA] [varchar](1024) NULL,
	[UNO_iOS_RAW_DATA] [varchar](1024) NULL,
	[UNO_STARTDATETIME] [datetime] NULL,
	[UNO_LIMITDATETIME] [datetime] NULL,
	[UNO_STATUS] [int] NOT NULL,
 CONSTRAINT [PK_USERS_TOAST_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[UNO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS_PUSH_ID]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS_PUSH_ID](
	[UPID_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UPID_OS] [int] NOT NULL,
	[UPID_CELL_WIFI_MAC] [varchar](100) NULL,
	[UPID_CELL_IMEI] [varchar](100) NULL,
	[UPID_PUSHID] [varchar](512) NOT NULL,
	[UPID_USR_ID] [numeric](18, 0) NOT NULL,
	[UPID_LAST_UPDATE_DATETIME] [datetime] NOT NULL,
	[UPID_LAST_SUCESSFUL_PUSH] [datetime] NULL,
	[UPID_PUSH_RETRIES] [int] NOT NULL,
	[UPID_LAST_RETRY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_USERS_PUSH_ID] PRIMARY KEY CLUSTERED 
(
	[UPID_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS_SECURITY_OPERATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS_SECURITY_OPERATIONS](
	[USOP_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USOP_OP_TYPE] [int] NOT NULL,
	[USOP_STATUS] [int] NOT NULL,
	[USOP_UTCDATETIME] [datetime] NOT NULL,
	[USOP_USR_ID] [numeric](18, 0) NOT NULL,
	[USOP_NEW_MAIN_TEL_COUNTRY] [numeric](18, 0) NULL,
	[USOP_NEW_MAIN_TEL] [varchar](50) NULL,
	[USOP_NEW_EMAIL] [varchar](50) NULL,
	[USOP_URL_PARAMETER] [varchar](100) NULL,
	[USOP_ACTIVATION_CODE] [varchar](20) NULL,
	[USOP_ACTIVATION_RETRIES] [numeric](18, 0) NOT NULL,
	[USOP_LAST_SENT_DATE] [datetime] NULL,
 CONSTRAINT [PK_USERS_SECURITY_OPERATIONS] PRIMARY KEY CLUSTERED 
(
	[USOP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USERS_SMSS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USERS_SMSS](
	[USRS_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[USRS_USR_ID] [numeric](18, 0) NULL,
	[USRS_CUSINS_ID] [numeric](18, 0) NULL,
	[USRS_DATE] [datetime] NULL,
	[USRS_RECIPIENT_TELEPHONE] [varchar](50) NOT NULL,
	[USRS_SMS] [varchar](512) NOT NULL,
	[USRS_STATUS] [int] NOT NULL,
	[USRS_SENDER_ID] [numeric](18, 0) NULL,
 CONSTRAINT [PK_USERS_SMSS] PRIMARY KEY CLUSTERED 
(
	[USRS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[ALL_OPERATIONS]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ALL_OPERATIONS]
AS
SELECT        OPE_TYPE, OPE_USR_ID, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, OPE_DATE, OPE_INIDATE, OPE_ENDDATE, - OPE_AMOUNT OPE_AMOUNT, 
                         OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - OPE_FINAL_AMOUNT OPE_FINAL_AMOUNT, 
                         OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL 
                         TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, GRP_DESCRIPTION, TAR_DESCRIPTION, 
                         OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE
FROM            dbo.OPERATIONS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.OPERATIONS.OPE_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID, INSTALLATIONS, GROUPS, TARIFFS
WHERE        OPE_INS_ID = INS_ID AND GRP_ID = OPE_GRP_ID AND TAR_ID = OPE_TAR_ID
UNION ALL
SELECT        4 OPE_TYPE, TIPA_USR_ID, TIPA_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, TIPA_DATE, NULL, NULL, - TIPA_AMOUNT TIPA_AMOUNT, NULL, 
                         TIPA_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - TIPA_FINAL_AMOUNT, TIPA_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, TIPA_CHANGE_APPLIED, USRP_ID, TIPA_PLATE_STRING, TIPA_TICKET_NUMBER, 
                         TIPA_TICKET_DATA, NULL, NULL, NULL, NULL, NULL, TIPA_SUSCRIPTION_TYPE, TIPA_BALANCE_BEFORE, TIPA_INSERTION_UTC_DATE
FROM            dbo.TICKET_PAYMENTS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.TICKET_PAYMENTS.TIPA_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON TIPA_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON TIPA_BALANCE_CUR_ID = CUR2.CUR_ID, INSTALLATIONS
WHERE        TIPA_INS_ID = INS_ID
UNION ALL
SELECT        5 OPE_TYPE, CUSPMR_USR_ID, NULL, NULL, NULL, CUSPMR_DATE, NULL, NULL, CUSPMR_AMOUNT, NULL, CUSPMR_CUR_ID, 
                         CUR_ISO_CODE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CUSPMR_SUSCRIPTION_TYPE, 
                         CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE
FROM            dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES, dbo.CURRENCIES
WHERE        CUSPMR_CUR_ID = CUR_ID AND (CUSPMR_SUSCRIPTION_TYPE = 1 OR
                         CUSPMR_SUSCRIPTION_TYPE IS NULL) AND CUSPMR_TRANS_STATUS IN (1, 2, 3, 4)
UNION ALL
SELECT        6 OPE_TYPE, SECH_USR_ID, NULL, NULL, NULL, SECH_DATE, NULL, NULL, - SECH_AMOUNT, NULL, SECH_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - SECH_FINAL_AMOUNT, SECH_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, SECH_CHANGE_APPLIED, NULL, NULL, NULL, NULL, SECH_SECHT_ID, NULL, NULL, NULL, NULL, 
                         SECH_SUSCRIPTION_TYPE, SECH_BALANCE_BEFORE, SECH_INSERTION_UTC_DATE
FROM            dbo.SERVICE_CHARGES INNER JOIN
                         dbo.CURRENCIES CUR1 ON SECH_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON SECH_BALANCE_CUR_ID = CUR2.CUR_ID
UNION ALL
SELECT        7 OPE_TYPE, OPEDIS_USR_ID, NULL, NULL, NULL, OPEDIS_DATE, NULL, NULL, OPEDIS_AMOUNT, NULL, OPEDIS_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
                         OPEDIS_SUSCRIPTION_TYPE, OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE
FROM            dbo.OPERATIONS_DISCOUNTS INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPEDIS_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPEDIS_BALANCE_CUR_ID = CUR2.CUR_ID
                         

GO
/****** Object:  View [dbo].[ALL_OPERATIONS_EXT]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ALL_OPERATIONS_EXT]
AS
                         
SELECT        OPE_TYPE, OPE_USR_ID, OPE_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, OPE_DATE, OPE_INIDATE, OPE_ENDDATE, - OPE_AMOUNT OPE_AMOUNT, 
                         OPE_TIME, OPE_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - OPE_FINAL_AMOUNT OPE_FINAL_AMOUNT, 
                         OPE_BALANCE_CUR_ID, CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, OPE_CHANGE_APPLIED, USRP_ID, USRP_PLATE, NULL 
                         TIPA_TICKET_NUMBER, NULL TIPA_TICKET_DATA, NULL SECH_SECHT_ID, GRP_ID, TAR_ID, GRP_DESCRIPTION, TAR_DESCRIPTION, 
                         OPE_SUSCRIPTION_TYPE, OPE_BALANCE_BEFORE, OPE_INSERTION_UTC_DATE, OPE_CUSPMR_ID, OPE_OPEDIS_ID,
                        CUSPMR_DATE,CUSPMR_AMOUNT, CUSPMR_CUR_ID, CUR3.CUR_ISO_CODE CUSPMR_AMOUNT_ISO_CODE,CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE,
                        OPEDIS_DATE, OPEDIS_AMOUNT,  OPEDIS_AMOUNT_CUR_ID, CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, 
                        CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE
FROM            dbo.OPERATIONS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.OPERATIONS.OPE_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPE_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPE_BALANCE_CUR_ID = CUR2.CUR_ID LEFT OUTER JOIN
                         dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON dbo.OPERATIONS.OPE_CUSPMR_ID  = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID LEFT OUTER JOIN
                         dbo.OPERATIONS_DISCOUNTS ON dbo.OPERATIONS.OPE_OPEDIS_ID  = OPERATIONS_DISCOUNTS.OPEDIS_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID,
                         INSTALLATIONS,GROUPS, TARIFFS
WHERE        OPE_INS_ID = INS_ID AND GRP_ID = OPE_GRP_ID AND TAR_ID = OPE_TAR_ID
UNION ALL
SELECT        4 OPE_TYPE, TIPA_USR_ID, TIPA_INS_ID, INS_DESCRIPTION, INS_SHORTDESC, TIPA_DATE, NULL, NULL, - TIPA_AMOUNT TIPA_AMOUNT, NULL, 
                         TIPA_AMOUNT_CUR_ID, CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - TIPA_FINAL_AMOUNT, TIPA_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, TIPA_CHANGE_APPLIED, USRP_ID, TIPA_PLATE_STRING, TIPA_TICKET_NUMBER, 
                         TIPA_TICKET_DATA, NULL, NULL, NULL, NULL, NULL, TIPA_SUSCRIPTION_TYPE, TIPA_BALANCE_BEFORE, TIPA_INSERTION_UTC_DATE,TIPA_CUSPMR_ID,TIPA_OPEDIS_ID,
                         CUSPMR_DATE,CUSPMR_AMOUNT, CUSPMR_CUR_ID, CUR3.CUR_ISO_CODE,CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE,
                        OPEDIS_DATE, OPEDIS_AMOUNT,  OPEDIS_AMOUNT_CUR_ID, CUR4.CUR_ISO_CODE OPEDIS_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, 
                        CUR5.CUR_ISO_CODE OPEDIS_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE

FROM            dbo.TICKET_PAYMENTS LEFT OUTER JOIN
                         dbo.USER_PLATES ON dbo.TICKET_PAYMENTS.TIPA_USRP_ID = USER_PLATES.USRP_ID INNER JOIN
                         dbo.CURRENCIES CUR1 ON TIPA_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON TIPA_BALANCE_CUR_ID = CUR2.CUR_ID LEFT OUTER JOIN
                         dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON dbo.TICKET_PAYMENTS.TIPA_CUSPMR_ID  = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID LEFT OUTER JOIN
                         dbo.OPERATIONS_DISCOUNTS ON dbo.TICKET_PAYMENTS.TIPA_OPEDIS_ID  = OPERATIONS_DISCOUNTS.OPEDIS_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR4 ON OPEDIS_AMOUNT_CUR_ID = CUR4.CUR_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR5 ON OPEDIS_BALANCE_CUR_ID = CUR5.CUR_ID,
                         INSTALLATIONS 
WHERE        TIPA_INS_ID = INS_ID
UNION ALL
SELECT        5 OPE_TYPE, CUSPMR_USR_ID, NULL, NULL, NULL, CUSPMR_DATE, NULL, NULL, CUSPMR_AMOUNT, NULL, CUSPMR_CUR_ID, 
                         CUR_ISO_CODE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CUSPMR_SUSCRIPTION_TYPE, 
                         CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                         NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
FROM            dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES, dbo.CURRENCIES
WHERE        CUSPMR_CUR_ID = CUR_ID AND (CUSPMR_SUSCRIPTION_TYPE = 1 OR
                         CUSPMR_SUSCRIPTION_TYPE IS NULL) AND CUSPMR_TRANS_STATUS IN (1, 2, 3, 4)
UNION ALL
SELECT        6 OPE_TYPE, SECH_USR_ID, NULL, NULL, NULL, SECH_DATE, NULL, NULL, - SECH_AMOUNT, NULL, SECH_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, - SECH_FINAL_AMOUNT, SECH_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, SECH_CHANGE_APPLIED, NULL, NULL, NULL, NULL, SECH_SECHT_ID, NULL, NULL, NULL, NULL, 
                         SECH_SUSCRIPTION_TYPE, SECH_BALANCE_BEFORE, SECH_INSERTION_UTC_DATE,SECH_CUSPMR_ID,NULL,
                         CUSPMR_DATE,CUSPMR_AMOUNT, CUSPMR_CUR_ID, CUR3.CUR_ISO_CODE,CUSPMR_BALANCE_BEFORE, CUSPMR_INSERTION_UTC_DATE,
                         NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
FROM            dbo.SERVICE_CHARGES INNER JOIN
                         dbo.CURRENCIES CUR1 ON SECH_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON SECH_BALANCE_CUR_ID = CUR2.CUR_ID  LEFT OUTER JOIN
                         dbo.CUSTOMER_PAYMENT_MEANS_RECHARGES ON dbo.SERVICE_CHARGES.SECH_CUSPMR_ID  = CUSTOMER_PAYMENT_MEANS_RECHARGES.CUSPMR_ID LEFT OUTER JOIN
                         dbo.CURRENCIES CUR3 ON CUSPMR_CUR_ID = CUR3.CUR_ID
UNION ALL
SELECT        7 OPE_TYPE, OPEDIS_USR_ID, NULL, NULL, NULL, OPEDIS_DATE, NULL, NULL, OPEDIS_AMOUNT, NULL, OPEDIS_AMOUNT_CUR_ID, 
                         CUR1.CUR_ISO_CODE OPE_AMOUNT_CUR_ISO_CODE, OPEDIS_FINAL_AMOUNT, OPEDIS_BALANCE_CUR_ID, 
                         CUR2.CUR_ISO_CODE OPE_BALANCE_CUR_ISO_CODE, OPEDIS_CHANGE_APPLIED, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
                         OPEDIS_SUSCRIPTION_TYPE, OPEDIS_BALANCE_BEFORE, OPEDIS_INSERTION_UTC_DATE,NULL,NULL,
                         NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
FROM            dbo.OPERATIONS_DISCOUNTS INNER JOIN
                         dbo.CURRENCIES CUR1 ON OPEDIS_AMOUNT_CUR_ID = CUR1.CUR_ID INNER JOIN
                         dbo.CURRENCIES CUR2 ON OPEDIS_BALANCE_CUR_ID = CUR2.CUR_ID                         

GO
/****** Object:  View [dbo].[vw_aspnet_Applications]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[aspnet_Applications].[ApplicationName], [dbo].[aspnet_Applications].[LoweredApplicationName], [dbo].[aspnet_Applications].[ApplicationId], [dbo].[aspnet_Applications].[Description]
  FROM [dbo].[aspnet_Applications]
  
GO
/****** Object:  View [dbo].[vw_aspnet_MembershipUsers]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_MembershipUsers]
  AS SELECT [dbo].[aspnet_Membership].[UserId],
            [dbo].[aspnet_Membership].[PasswordFormat],
            [dbo].[aspnet_Membership].[MobilePIN],
            [dbo].[aspnet_Membership].[Email],
            [dbo].[aspnet_Membership].[LoweredEmail],
            [dbo].[aspnet_Membership].[PasswordQuestion],
            [dbo].[aspnet_Membership].[PasswordAnswer],
            [dbo].[aspnet_Membership].[IsApproved],
            [dbo].[aspnet_Membership].[IsLockedOut],
            [dbo].[aspnet_Membership].[CreateDate],
            [dbo].[aspnet_Membership].[LastLoginDate],
            [dbo].[aspnet_Membership].[LastPasswordChangedDate],
            [dbo].[aspnet_Membership].[LastLockoutDate],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptWindowStart],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptWindowStart],
            [dbo].[aspnet_Membership].[Comment],
            [dbo].[aspnet_Users].[ApplicationId],
            [dbo].[aspnet_Users].[UserName],
            [dbo].[aspnet_Users].[MobileAlias],
            [dbo].[aspnet_Users].[IsAnonymous],
            [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Membership] INNER JOIN [dbo].[aspnet_Users]
      ON [dbo].[aspnet_Membership].[UserId] = [dbo].[aspnet_Users].[UserId]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Profiles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Profiles]
  AS SELECT [dbo].[aspnet_Profile].[UserId], [dbo].[aspnet_Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([dbo].[aspnet_Profile].[PropertyNames])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesString])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesBinary])
  FROM [dbo].[aspnet_Profile]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Roles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Users]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[aspnet_Users].[ApplicationId], [dbo].[aspnet_Users].[UserId], [dbo].[aspnet_Users].[UserName], [dbo].[aspnet_Users].[LoweredUserName], [dbo].[aspnet_Users].[MobileAlias], [dbo].[aspnet_Users].[IsAnonymous], [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Users]
  
GO
/****** Object:  View [dbo].[vw_aspnet_UsersInRoles]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Paths]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Paths]
  AS SELECT [dbo].[aspnet_Paths].[ApplicationId], [dbo].[aspnet_Paths].[PathId], [dbo].[aspnet_Paths].[Path], [dbo].[aspnet_Paths].[LoweredPath]
  FROM [dbo].[aspnet_Paths]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Shared]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Shared]
  AS SELECT [dbo].[aspnet_PersonalizationAllUsers].[PathId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationAllUsers].[PageSettings]), [dbo].[aspnet_PersonalizationAllUsers].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationAllUsers]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_User]    Script Date: 08/10/2013 12:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_User]
  AS SELECT [dbo].[aspnet_PersonalizationPerUser].[PathId], [dbo].[aspnet_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationPerUser].[PageSettings]), [dbo].[aspnet_PersonalizationPerUser].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationPerUser]
  
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Applications_Index]    Script Date: 08/10/2013 12:29:18 ******/
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications]
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Membership_index]    Script Date: 08/10/2013 12:29:18 ******/
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership]
(
	[ApplicationId] ASC,
	[LoweredEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Paths_index]    Script Date: 08/10/2013 12:29:18 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths]
(
	[ApplicationId] ASC,
	[LoweredPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_index1]    Script Date: 08/10/2013 12:29:18 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_PersonalizationPerUser_index1] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[PathId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Roles_index1]    Script Date: 08/10/2013 12:29:18 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles]
(
	[ApplicationId] ASC,
	[LoweredRoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Users_Index]    Script Date: 08/10/2013 12:29:18 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LoweredUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_ncindex2]    Script Date: 08/10/2013 12:29:18 ******/
CREATE UNIQUE NONCLUSTERED INDEX [aspnet_PersonalizationPerUser_ncindex2] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[UserId] ASC,
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_Users_Index2]    Script Date: 08/10/2013 12:29:18 ******/
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LastActivityDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_UsersInRoles_index]    Script Date: 08/10/2013 12:29:18 ******/
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD  DEFAULT ((0)) FOR [PasswordFormat]
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD  DEFAULT (newid()) FOR [PathId]
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD  DEFAULT (newid()) FOR [RoleId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (NULL) FOR [MobileAlias]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT ((0)) FOR [IsAnonymous]
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS] ADD  CONSTRAINT [DF_CUSTOMER_INSCRIPTIONS_CUSINS_ACTIVATION_RETRIES]  DEFAULT ((0)) FOR [CUSINS_ACTIVATION_RETRIES]
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS] ADD  CONSTRAINT [DF_CUSTOMER_INSCRIPTIONS_CUISINS_CULTURE]  DEFAULT ('en-US') FOR [CUISINS_CULTURE]
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES] ADD  CONSTRAINT [DF_CUSTOMER_INVOICES_CUSINV_INV_AMOUNT]  DEFAULT ((0)) FOR [CUSINV_INV_AMOUNT]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_CUSPM_PAST_ID]  DEFAULT ((1)) FOR [CUSPM_PAST_ID]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_CUS_CUR_ID]  DEFAULT ((50)) FOR [CUSPM_CUR_ID]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_CUSPM_AUTOMATIC_FAILED_RETRIES]  DEFAULT ((0)) FOR [CUSPM_AUTOMATIC_FAILED_RETRIES]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_CUSPM_VALID]  DEFAULT ((1)) FOR [CUSPM_VALID]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_CUSPM_ENABLED]  DEFAULT ((1)) FOR [CUSPM_ENABLED]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_AMOUNT]  DEFAULT ((0)) FOR [CUSPMR_AMOUNT]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_PAT_FIXED_FEE]  DEFAULT ((0)) FOR [CUSPMR_PAT_FIXED_FEE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_PAT_PERC_FEE]  DEFAULT ((0)) FOR [CUSPMR_PAT_PERC_FEE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_PAST_FIXED_FEE]  DEFAULT ((0)) FOR [CUSPMR_PAST_FIXED_FEE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_PAST_PERC_FEE]  DEFAULT ((0)) FOR [CUSPMR_PAST_PERC_FEE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_PERC_VAT]  DEFAULT ((0)) FOR [CUSPMR_PERC_VAT]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_TOTAL_AMOUNT_CHARGED]  DEFAULT ((0)) FOR [CUSPMR_TOTAL_AMOUNT_CHARGED]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [CUSPMR_SUSCRIPTION_TYPE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_TIPA_BALANCE_BEFORE]  DEFAULT ((0)) FOR [CUSPMR_BALANCE_BEFORE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_TRANS_STATUS]  DEFAULT ((4)) FOR [CUSPMR_TRANS_STATUS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_STATUS_DATE]  DEFAULT (getdate()) FOR [CUSPMR_STATUS_DATE]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSPMR_RETRIES_NUM]  DEFAULT ((0)) FOR [CUSPMR_RETRIES_NUM]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ADD  CONSTRAINT [DF_CUSTOMER_PAYMENT_MEANS_RECHARGES_SECH_INSERTION_UTC_DATE]  DEFAULT (getdate()) FOR [CUSPMR_INSERTION_UTC_DATE]
GO
ALTER TABLE [dbo].[CUSTOMERS] ADD  CONSTRAINT [DF_CUSTOMERS_CUS_ENABLED]  DEFAULT ((1)) FOR [CUS_ENABLED]
GO
ALTER TABLE [dbo].[EXTERNAL_PARKING_OPERATIONS] ADD  CONSTRAINT [DF_EXTERNAL_PARKING_OPERATIONS_EXTI_INSERTION_NOTIFIED]  DEFAULT ((0)) FOR [EPO_INSERTION_NOTIFIED]
GO
ALTER TABLE [dbo].[EXTERNAL_PARKING_OPERATIONS] ADD  CONSTRAINT [DF_EXTERNAL_PARKING_OPERATIONS_EPO_INSERTION_NOTIFIED1]  DEFAULT ((0)) FOR [EPO_ENDING_NOTIFIED]
GO
ALTER TABLE [dbo].[EXTERNAL_TICKET] ADD  CONSTRAINT [DF_EXTERNAL_TICKET_EXTI_INSERTION_NOTIFIED]  DEFAULT ((0)) FOR [EXTI_INSERTION_NOTIFIED]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_VERSION]  DEFAULT ((1)) FOR [INS_VERSION]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_TAR_VERSION]  DEFAULT ((1)) FOR [INS_TAR_VERSION]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_CULTURE_LAND]  DEFAULT ('es-ES') FOR [INS_CULTURE_LANG]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_FINE_WS_SIGNATURE]  DEFAULT ((1)) FOR [INS_FINE_WS_SIGNATURE_TYPE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_FINE_WS_SIGNATURE_TYPE1]  DEFAULT ((1)) FOR [INS_PARK_WS_SIGNATURE_TYPE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARK_WS_SIGNATURE_TYPE1_1]  DEFAULT ((1)) FOR [INS_PARK_CONFIRM_WS_SIGNATURE_TYPE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARK_WS_SIGNATURE_TYPE1]  DEFAULT ((1)) FOR [INS_PLATE_UPDATE_WS_SIGNATURE_TYPE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN]  DEFAULT ((1)) FOR [INS_OPT_PARK]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN1]  DEFAULT ((1)) FOR [INS_OPT_UNPARK]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN2]  DEFAULT ((1)) FOR [INS_OPT_TICKET]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN3]  DEFAULT ((1)) FOR [INS_OPT_RECHARGE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN31]  DEFAULT ((1)) FOR [INS_OPT_MORE_FUNCTS]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_CUR_OPERS]  DEFAULT ((1)) FOR [INS_OPT_CUR_OPERS]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_CUR_OPERS1]  DEFAULT ((1)) FOR [INS_OPT_HIS_OPERS]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_CUR_OPERS2]  DEFAULT ((1)) FOR [INS_OPT_OCUP]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN32]  DEFAULT ((1)) FOR [INS_OPT_PARKBYPLATE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN33]  DEFAULT ((1)) FOR [INS_OPT_PARKBYPLATELISTTYPE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN34]  DEFAULT ((1)) FOR [INS_OPT_PARKBYSPACE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN35]  DEFAULT ((1)) FOR [INS_OPT_PARKBYZONEANDSECTOR]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN36]  DEFAULT ((1)) FOR [INS_OPT_PARKBYQR]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN37]  DEFAULT ((1)) FOR [INS_OPT_PARKBYMAP]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN38]  DEFAULT ((1)) FOR [INS_OPT_PARKBYSPACEFORMAT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN39]  DEFAULT ((1)) FOR [INS_OPT_PARKBYSPACEFORMAT_FORMAT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN310]  DEFAULT ((1)) FOR [INS_OPT_PARKISZONEMANDATORY]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN311]  DEFAULT ((1)) FOR [INS_OPT_PARKISSECTORMANDATORY]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN312]  DEFAULT ((1)) FOR [INS_OPT_PARKZONECRIT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN313]  DEFAULT ((1)) FOR [INS_OPT_PARKPAYBYQRFORMATMANDATORY]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN314]  DEFAULT ((1)) FOR [INS_OPT_PARKPAYBYQRFORMAT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN315]  DEFAULT ((1)) FOR [INS_OPT_PARKMULTITARIFF_NUM]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN316]  DEFAULT ((1)) FOR [INS_OPT_TICKET_NUM]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_PARKEN317]  DEFAULT ((1)) FOR [INS_OPT_TICKET_QR]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_TICKET_QR5]  DEFAULT ((1)) FOR [INS_OPT_RECHARGE_QR]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_TICKET_QR4]  DEFAULT ((1)) FOR [INS_OPT_RECHARGE_CODE]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_TICKET_QR3]  DEFAULT ((1)) FOR [INS_OPT_RECHARGE_PAYMENT_MEAN]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_QRPARKZONEFORMAT]  DEFAULT ((1)) FOR [INS_OPT_QRPARKZONEFORMAT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_QRPARKZONEFORMAT1]  DEFAULT ((1)) FOR [INS_OPT_QRTICKETFORMAT]
GO
ALTER TABLE [dbo].[INSTALLATIONS] ADD  CONSTRAINT [DF_INSTALLATIONS_INS_OPT_QRPARKZONEFORMAT2]  DEFAULT ((1)) FOR [INS_OPT_QRRECHARGEFORMAT]
GO
ALTER TABLE [dbo].[MOBILE_SESSIONS] ADD  CONSTRAINT [DF_MOBILE_SESSIONS_MOSE_STATUS]  DEFAULT ((1)) FOR [MOSE_STATUS]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_TYPE]  DEFAULT ((1)) FOR [OPE_TYPE]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_AMOUNT_CUR_ID]  DEFAULT ((50)) FOR [OPE_AMOUNT_CUR_ID]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_BALANCE_CUR_ID]  DEFAULT ((50)) FOR [OPE_BALANCE_CUR_ID]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_CHANGE_APPLIED]  DEFAULT ((1)) FOR [OPE_CHANGE_APPLIED]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_BALANCE_BEFORE]  DEFAULT ((0)) FOR [OPE_BALANCE_BEFORE]
GO
ALTER TABLE [dbo].[OPERATIONS] ADD  CONSTRAINT [DF_OPERATIONS_OPE_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [OPE_SUSCRIPTION_TYPE]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] ADD  CONSTRAINT [DF_OPERATIONS_DISCOUNTS_SECH_AMOUNT_CUR_ID]  DEFAULT ((50)) FOR [OPEDIS_AMOUNT_CUR_ID]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] ADD  CONSTRAINT [DF_OPERATIONS_DISCOUNTS_SECH_BALANCE_CUR_ID]  DEFAULT ((50)) FOR [OPEDIS_BALANCE_CUR_ID]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] ADD  CONSTRAINT [DF_OPERATIONS_DISCOUNTS_SECH_CHANGE_APPLIED]  DEFAULT ((1)) FOR [OPEDIS_CHANGE_APPLIED]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] ADD  CONSTRAINT [DF_OPERATIONS_DISCOUNTS_TIPA_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [OPEDIS_SUSCRIPTION_TYPE]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] ADD  CONSTRAINT [DF_OPERATIONS_DISCOUNTS_TIPA_BALANCE_BEFORE]  DEFAULT ((0)) FOR [OPEDIS_BALANCE_BEFORE]
GO
ALTER TABLE [dbo].[OPERATIONS_SESSION_INFO] ADD  CONSTRAINT [DF_OPERATIONS_SESSION_INFO_OSI_OPE_TYPE]  DEFAULT ((1)) FOR [OSI_OPE_TYPE]
GO
ALTER TABLE [dbo].[OPERATORS] ADD  CONSTRAINT [DF_OPERATORS_OPR_VAT_PERC]  DEFAULT ((0)) FOR [OPR_VAT_PERC]
GO
ALTER TABLE [dbo].[OPERATORS] ADD  CONSTRAINT [DF_OPERATORS_OPR_INITIAL_INVOICE_NUMBER]  DEFAULT ((0)) FOR [OPR_INITIAL_INVOICE_NUMBER]
GO
ALTER TABLE [dbo].[OPERATORS] ADD  CONSTRAINT [DF_OPERATORS_OPR_INITIAL_INVOICE_NUMBER1]  DEFAULT ((0)) FOR [OPR_END_INVOICE_NUMBER]
GO
ALTER TABLE [dbo].[OPERATORS] ADD  CONSTRAINT [DF_OPERATORS_OPR_END_INVOICE_NUMBER11]  DEFAULT ((0)) FOR [OPR_CURRENT_INVOICE_NUMBER]
GO
ALTER TABLE [dbo].[PUSHID_NOTIFICATIONS] ADD  CONSTRAINT [DF_PUSHID_TOAST_NOTIFICATIONS_UTNO_STATUS]  DEFAULT ((10)) FOR [PNO_STATUS]
GO
ALTER TABLE [dbo].[PUSHID_NOTIFICATIONS] ADD  CONSTRAINT [DF_PUSHID_TOAST_NOTIFICATIONS_PTNO_RETRIES]  DEFAULT ((0)) FOR [PNO_RETRIES]
GO
ALTER TABLE [dbo].[RECHARGE_VALUES] ADD  CONSTRAINT [DF_RECHARGE_VALUES_RECHVAL_VALUE_TYPE]  DEFAULT ((1)) FOR [RECHVAL_VALUE_TYPE]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] ADD  CONSTRAINT [DF_SERVICE_CHARGES_OPE_AMOUNT_CUR_ID]  DEFAULT ((50)) FOR [SECH_AMOUNT_CUR_ID]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] ADD  CONSTRAINT [DF_SERVICE_CHARGES_OPE_BALANCE_CUR_ID]  DEFAULT ((50)) FOR [SECH_BALANCE_CUR_ID]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] ADD  CONSTRAINT [DF_SERVICE_CHARGES_OPE_CHANGE_APPLIED]  DEFAULT ((1)) FOR [SECH_CHANGE_APPLIED]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] ADD  CONSTRAINT [DF_SERVICE_CHARGES_TIPA_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [SECH_SUSCRIPTION_TYPE]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] ADD  CONSTRAINT [DF_SERVICE_CHARGES_TIPA_BALANCE_BEFORE]  DEFAULT ((0)) FOR [SECH_BALANCE_BEFORE]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] ADD  CONSTRAINT [DF_TICKET_PAYMENTS_OPE_AMOUNT_CUR_ID]  DEFAULT ((50)) FOR [TIPA_AMOUNT_CUR_ID]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] ADD  CONSTRAINT [DF_TICKET_PAYMENTS_OPE_BALANCE_CUR_ID]  DEFAULT ((50)) FOR [TIPA_BALANCE_CUR_ID]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] ADD  CONSTRAINT [DF_TICKET_PAYMENTS_OPE_CHANGE_APPLIED]  DEFAULT ((1)) FOR [TIPA_CHANGE_APPLIED]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] ADD  CONSTRAINT [DF_TICKET_PAYMENTS_TIPA_SUSCRIPTION_TYPE]  DEFAULT ((1)) FOR [TIPA_SUSCRIPTION_TYPE]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] ADD  CONSTRAINT [DF_TICKET_PAYMENTS_OPE_BALANCE_BEFORE]  DEFAULT ((0)) FOR [TIPA_BALANCE_BEFORE]
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS] ADD  CONSTRAINT [DF_USER_PLATE_MOVS_USRPM_SEND_INSERTION]  DEFAULT ((0)) FOR [USRPM_SEND_INSERTION]
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS_SENDING] ADD  CONSTRAINT [DF_USER_PLATE_MOVS_SENDING_USRPMS_STATUS]  DEFAULT ((1)) FOR [USRPMS_STATUS]
GO
ALTER TABLE [dbo].[USER_PLATES] ADD  DEFAULT ((1)) FOR [USRP_ENABLED]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USR_BALANCE]  DEFAULT ((0)) FOR [USR_BALANCE]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USR_CUR_ID]  DEFAULT ((50)) FOR [USR_CUR_ID]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USR_UTC_OFFSET]  DEFAULT ((0)) FOR [USR_UTC_OFFSET]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USR_ENABLED]  DEFAULT ((1)) FOR [USR_ENABLED]
GO
ALTER TABLE [dbo].[USERS_EMAILS] ADD  CONSTRAINT [DF_USERS_EMAILS_USRE_STATUS]  DEFAULT ((0)) FOR [USRE_STATUS]
GO
ALTER TABLE [dbo].[USERS_NOTIFICATIONS] ADD  CONSTRAINT [DF_USERS_TOAST_NOTIFICATIONS_UTNO_STATUS]  DEFAULT ((10)) FOR [UNO_STATUS]
GO
ALTER TABLE [dbo].[USERS_PUSH_ID] ADD  CONSTRAINT [DF_USERS_PUSH_ID_UPID_PUSH_RETRIES]  DEFAULT ((0)) FOR [UPID_PUSH_RETRIES]
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS] ADD  CONSTRAINT [DF_USERS_SECURITY_OPERATIONS_USOP_OP_TYPE]  DEFAULT ((1)) FOR [USOP_OP_TYPE]
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS] ADD  CONSTRAINT [DF_USERS_SECURITY_OPERATIONS_USOP_STATUS]  DEFAULT ((1)) FOR [USOP_STATUS]
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS] ADD  CONSTRAINT [DF_USERS_SECURITY_OPERATIONS_USOP_ACTIVATION_RETRIES]  DEFAULT ((0)) FOR [USOP_ACTIVATION_RETRIES]
GO
ALTER TABLE [dbo].[USERS_SMSS] ADD  CONSTRAINT [DF_USERS_SMSS_USRS_STATUS]  DEFAULT ((0)) FOR [USRS_STATUS]
GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[COUNTRIES]  WITH CHECK ADD  CONSTRAINT [FK_COUNTRIES_CURRENCIES] FOREIGN KEY([COU_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[COUNTRIES] CHECK CONSTRAINT [FK_COUNTRIES_CURRENCIES]
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_COUNTRIES_MAIN_TEL] FOREIGN KEY([CUSINS_MAIN_TEL_COUNTRY])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS] CHECK CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_COUNTRIES_MAIN_TEL]
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_COUNTRIES_SECUND_TEL] FOREIGN KEY([CUSINS_SECUND_TEL_COUNTRY])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS] CHECK CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_COUNTRIES_SECUND_TEL]
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_CUSTOMERS] FOREIGN KEY([CUISINS_CUS_ID])
REFERENCES [dbo].[CUSTOMERS] ([CUS_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INSCRIPTIONS] CHECK CONSTRAINT [FK_CUSTOMER_INSCRIPTIONS_CUSTOMERS]
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INVOICES_CURRENCIES] FOREIGN KEY([CUSINV_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES] CHECK CONSTRAINT [FK_CUSTOMER_INVOICES_CURRENCIES]
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INVOICES_CUSTOMERS] FOREIGN KEY([CUSINV_CUS_ID])
REFERENCES [dbo].[CUSTOMERS] ([CUS_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES] CHECK CONSTRAINT [FK_CUSTOMER_INVOICES_CUSTOMERS]
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_INVOICES_OPERATORS] FOREIGN KEY([CUSINV_OPR_ID])
REFERENCES [dbo].[OPERATORS] ([OPR_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_INVOICES] CHECK CONSTRAINT [FK_CUSTOMER_INVOICES_OPERATORS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_CURRENCIES] FOREIGN KEY([CUSPM_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_CURRENCIES]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_CUSTOMERS] FOREIGN KEY([CUSPM_CUS_ID])
REFERENCES [dbo].[CUSTOMERS] ([CUS_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_CUSTOMERS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_PAYMENT_SUBTYPES] FOREIGN KEY([CUSPM_PAST_ID])
REFERENCES [dbo].[PAYMENT_SUBTYPES] ([PAST_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_PAYMENT_SUBTYPES]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_PAYMENT_TYPES] FOREIGN KEY([CUSPM_PAT_ID])
REFERENCES [dbo].[PAYMENT_TYPES] ([PAT_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_PAYMENT_TYPES]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMER_INVOICES] FOREIGN KEY([CUSPMR_CUSINV_ID])
REFERENCES [dbo].[CUSTOMER_INVOICES] ([CUSINV_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMER_INVOICES]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMER_PAYMENT_MEANS] FOREIGN KEY([CUSPMR_CUSPM_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS] ([CUSPM_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMER_PAYMENT_MEANS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMERS] FOREIGN KEY([CUSPMR_CUS_ID])
REFERENCES [dbo].[CUSTOMERS] ([CUS_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_CUSTOMERS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_RECHARGE_COUPONS] FOREIGN KEY([CUSPMR_RCOUP_ID])
REFERENCES [dbo].[RECHARGE_COUPONS] ([RCOUP_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_RECHARGE_COUPONS]
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_USERS] FOREIGN KEY([CUSPMR_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] CHECK CONSTRAINT [FK_CUSTOMER_PAYMENT_MEANS_RECHARGES_USERS]
GO
ALTER TABLE [dbo].[CUSTOMERS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMERS_COUNTRIES] FOREIGN KEY([CUS_COU_ID])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[CUSTOMERS] CHECK CONSTRAINT [FK_CUSTOMERS_COUNTRIES]
GO
ALTER TABLE [dbo].[CUSTOMERS]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMERS_USERS] FOREIGN KEY([CUS_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[CUSTOMERS] CHECK CONSTRAINT [FK_CUSTOMERS_USERS]
GO
ALTER TABLE [dbo].[EXTERNAL_PARKING_OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_EXTERNAL_PARKING_OPERATIONS_INSTALLATIONS] FOREIGN KEY([EPO_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[EXTERNAL_PARKING_OPERATIONS] CHECK CONSTRAINT [FK_EXTERNAL_PARKING_OPERATIONS_INSTALLATIONS]
GO
ALTER TABLE [dbo].[EXTERNAL_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_EXTERNAL_TICKET_INSTALLATIONS] FOREIGN KEY([EXTI_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[EXTERNAL_TICKET] CHECK CONSTRAINT [FK_EXTERNAL_TICKET_INSTALLATIONS]
GO
ALTER TABLE [dbo].[GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_INSTALLATIONS] FOREIGN KEY([GRP_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[GROUPS] CHECK CONSTRAINT [FK_GROUPS_INSTALLATIONS]
GO
ALTER TABLE [dbo].[GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_LITERALS] FOREIGN KEY([GRP_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[GROUPS] CHECK CONSTRAINT [FK_GROUPS_LITERALS]
GO
ALTER TABLE [dbo].[GROUPS_GEOMETRY]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_GEOMETRY_GROUPS] FOREIGN KEY([GRGE_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[GROUPS_GEOMETRY] CHECK CONSTRAINT [FK_GROUPS_GEOMETRY_GROUPS]
GO
ALTER TABLE [dbo].[GROUPS_HIERARCHY]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_HIERARCHY_CHILD_GROUPS] FOREIGN KEY([GRHI_GPR_ID_CHILD])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[GROUPS_HIERARCHY] CHECK CONSTRAINT [FK_GROUPS_HIERARCHY_CHILD_GROUPS]
GO
ALTER TABLE [dbo].[GROUPS_HIERARCHY]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_HIERARCHY_PARENT_GROUPS] FOREIGN KEY([GRHI_GPR_ID_PARENT])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[GROUPS_HIERARCHY] CHECK CONSTRAINT [FK_GROUPS_HIERARCHY_PARENT_GROUPS]
GO
ALTER TABLE [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS_GROUPS] FOREIGN KEY([GTET_IN_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS] CHECK CONSTRAINT [FK_GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS_GROUPS]
GO
ALTER TABLE [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS_TARIFFS] FOREIGN KEY([GTET_IN_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO
ALTER TABLE [dbo].[GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS] CHECK CONSTRAINT [FK_GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS_TARIFFS]
GO
ALTER TABLE [dbo].[GROUPS_TYPES]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_TYPES_INSTALLATIONS] FOREIGN KEY([GRPT_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[GROUPS_TYPES] CHECK CONSTRAINT [FK_GROUPS_TYPES_INSTALLATIONS]
GO
ALTER TABLE [dbo].[GROUPS_TYPES_ASSIGNATIONS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_TYPES_ASSIGNATIONS_GROUPS] FOREIGN KEY([GTA_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[GROUPS_TYPES_ASSIGNATIONS] CHECK CONSTRAINT [FK_GROUPS_TYPES_ASSIGNATIONS_GROUPS]
GO
ALTER TABLE [dbo].[GROUPS_TYPES_ASSIGNATIONS]  WITH CHECK ADD  CONSTRAINT [FK_GROUPS_TYPES_ASSIGNATIONS_GROUPS_TYPES] FOREIGN KEY([GTA_GRPT_ID])
REFERENCES [dbo].[GROUPS_TYPES] ([GRPT_ID])
GO
ALTER TABLE [dbo].[GROUPS_TYPES_ASSIGNATIONS] CHECK CONSTRAINT [FK_GROUPS_TYPES_ASSIGNATIONS_GROUPS_TYPES]
GO
ALTER TABLE [dbo].[INSTALLATIONS]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_CURRENCIES] FOREIGN KEY([INS_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[INSTALLATIONS] CHECK CONSTRAINT [FK_INSTALLATIONS_CURRENCIES]
GO
ALTER TABLE [dbo].[INSTALLATIONS]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_LITERALS] FOREIGN KEY([INS_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[INSTALLATIONS] CHECK CONSTRAINT [FK_INSTALLATIONS_LITERALS]
GO
ALTER TABLE [dbo].[INSTALLATIONS]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_OPERATORS] FOREIGN KEY([INS_OPR_ID])
REFERENCES [dbo].[OPERATORS] ([OPR_ID])
GO
ALTER TABLE [dbo].[INSTALLATIONS] CHECK CONSTRAINT [FK_INSTALLATIONS_OPERATORS]
GO
ALTER TABLE [dbo].[INSTALLATIONS_GEOMETRY]  WITH CHECK ADD  CONSTRAINT [FK_INSTALLATIONS_GEOMETRY_INSTALLATIONS] FOREIGN KEY([INSGE_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[INSTALLATIONS_GEOMETRY] CHECK CONSTRAINT [FK_INSTALLATIONS_GEOMETRY_INSTALLATIONS]
GO
ALTER TABLE [dbo].[LITERAL_LANGUAGES]  WITH CHECK ADD  CONSTRAINT [FK_LITERAL_LANGUAGES_LITERALS] FOREIGN KEY([LITL_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[LITERAL_LANGUAGES] CHECK CONSTRAINT [FK_LITERAL_LANGUAGES_LITERALS]
GO
ALTER TABLE [dbo].[LITERAL_LANGUAGES]  WITH CHECK ADD  CONSTRAINT [FK_LITERALS_LANGUAGES] FOREIGN KEY([LITL_LAN_ID])
REFERENCES [dbo].[LANGUAGES] ([LAN_ID])
GO
ALTER TABLE [dbo].[LITERAL_LANGUAGES] CHECK CONSTRAINT [FK_LITERALS_LANGUAGES]
GO
ALTER TABLE [dbo].[MOBILE_SESSIONS]  WITH CHECK ADD  CONSTRAINT [FK_MOBILE_SESSIONS_USERS] FOREIGN KEY([MOSE_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[MOBILE_SESSIONS] CHECK CONSTRAINT [FK_MOBILE_SESSIONS_USERS]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_CURRENCIES_1] FOREIGN KEY([OPE_AMOUNT_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_CURRENCIES_1]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_CURRENCIES_2] FOREIGN KEY([OPE_BALANCE_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_CURRENCIES_2]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_CUSTOMER_PAYMENT_MEANS_RECHARGES] FOREIGN KEY([OPE_CUSPMR_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ([CUSPMR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_CUSTOMER_PAYMENT_MEANS_RECHARGES]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_GROUPS] FOREIGN KEY([OPE_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_GROUPS]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_INSTALLATIONS] FOREIGN KEY([OPE_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_INSTALLATIONS]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_OPERATIONS_DISCOUNTS] FOREIGN KEY([OPE_OPEDIS_ID])
REFERENCES [dbo].[OPERATIONS_DISCOUNTS] ([OPEDIS_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_OPERATIONS_DISCOUNTS]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_TARIFFS] FOREIGN KEY([OPE_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_TARIFFS]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_USER_PLATES] FOREIGN KEY([OPE_USRP_ID])
REFERENCES [dbo].[USER_PLATES] ([USRP_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_USER_PLATES]
GO
ALTER TABLE [dbo].[OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_USERS] FOREIGN KEY([OPE_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS] CHECK CONSTRAINT [FK_OPERATIONS_USERS]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_DISCOUNTS_CURRENCIES] FOREIGN KEY([OPEDIS_AMOUNT_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] CHECK CONSTRAINT [FK_OPERATIONS_DISCOUNTS_CURRENCIES]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_DISCOUNTS_CURRENCIES1] FOREIGN KEY([OPEDIS_BALANCE_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] CHECK CONSTRAINT [FK_OPERATIONS_DISCOUNTS_CURRENCIES1]
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_DISCOUNTS_USERS] FOREIGN KEY([OPEDIS_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[OPERATIONS_DISCOUNTS] CHECK CONSTRAINT [FK_OPERATIONS_DISCOUNTS_USERS]
GO
ALTER TABLE [dbo].[OPERATIONS_SESSION_INFO]  WITH CHECK ADD  CONSTRAINT [FK_OPERATIONS_SESSION_INFO_MOBILE_SESSIONS] FOREIGN KEY([OSI_MOSE_ID])
REFERENCES [dbo].[MOBILE_SESSIONS] ([MOSE_ID])
GO
ALTER TABLE [dbo].[OPERATIONS_SESSION_INFO] CHECK CONSTRAINT [FK_OPERATIONS_SESSION_INFO_MOBILE_SESSIONS]
GO
ALTER TABLE [dbo].[PAYMENT_SUBTYPES]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_SUBTYPES_LITERALS] FOREIGN KEY([PAST_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[PAYMENT_SUBTYPES] CHECK CONSTRAINT [FK_PAYMENT_SUBTYPES_LITERALS]
GO
ALTER TABLE [dbo].[PAYMENT_SUBTYPES]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_SUBTYPES_PAYMENT_TYPES] FOREIGN KEY([PAST_PAT_ID])
REFERENCES [dbo].[PAYMENT_TYPES] ([PAT_ID])
GO
ALTER TABLE [dbo].[PAYMENT_SUBTYPES] CHECK CONSTRAINT [FK_PAYMENT_SUBTYPES_PAYMENT_TYPES]
GO
ALTER TABLE [dbo].[PAYMENT_TYPES]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENT_TYPES_LITERALS] FOREIGN KEY([PAT_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[PAYMENT_TYPES] CHECK CONSTRAINT [FK_PAYMENT_TYPES_LITERALS]
GO
ALTER TABLE [dbo].[PLATES_TARIFFS]  WITH CHECK ADD  CONSTRAINT [FK_PLATES_TARIFFS_GROUPS] FOREIGN KEY([PLTA_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[PLATES_TARIFFS] CHECK CONSTRAINT [FK_PLATES_TARIFFS_GROUPS]
GO
ALTER TABLE [dbo].[PLATES_TARIFFS]  WITH CHECK ADD  CONSTRAINT [FK_PLATES_TARIFFS_GROUPS_TYPES] FOREIGN KEY([PLTA_GRPT_ID])
REFERENCES [dbo].[GROUPS_TYPES] ([GRPT_ID])
GO
ALTER TABLE [dbo].[PLATES_TARIFFS] CHECK CONSTRAINT [FK_PLATES_TARIFFS_GROUPS_TYPES]
GO
ALTER TABLE [dbo].[PLATES_TARIFFS]  WITH CHECK ADD  CONSTRAINT [FK_PLATES_TARIFFS_TARIFFS] FOREIGN KEY([PLTA_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO
ALTER TABLE [dbo].[PLATES_TARIFFS] CHECK CONSTRAINT [FK_PLATES_TARIFFS_TARIFFS]
GO
ALTER TABLE [dbo].[PUSHID_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_PUSHID_TOAST_NOTIFICATIONS_USERS_TOAST_NOTIFICATIONS] FOREIGN KEY([PNO_UTNO_ID])
REFERENCES [dbo].[USERS_NOTIFICATIONS] ([UNO_ID])
GO
ALTER TABLE [dbo].[PUSHID_NOTIFICATIONS] CHECK CONSTRAINT [FK_PUSHID_TOAST_NOTIFICATIONS_USERS_TOAST_NOTIFICATIONS]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_CURRENCIES] FOREIGN KEY([RCOUP_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_CURRENCIES]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_RECHARGE_COUPONS_STATUS] FOREIGN KEY([RCOUP_COUPS_ID])
REFERENCES [dbo].[RECHARGE_COUPONS_STATUS] ([RCOUPS_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_RECHARGE_COUPONS_STATUS]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_STATUS]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_STATUS_LITERALS] FOREIGN KEY([RCOUPS_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_STATUS] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_STATUS_LITERALS]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_USES_MOBILE_SESSIONS] FOREIGN KEY([RCOUPU_MOSE_ID])
REFERENCES [dbo].[MOBILE_SESSIONS] ([MOSE_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_USES_MOBILE_SESSIONS]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_USES_RECHARGE_COUPONS] FOREIGN KEY([RCOUPU_RCOUP_ID])
REFERENCES [dbo].[RECHARGE_COUPONS] ([RCOUP_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_USES_RECHARGE_COUPONS]
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_COUPONS_USES_USERS] FOREIGN KEY([RCOUPU_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[RECHARGE_COUPONS_USES] CHECK CONSTRAINT [FK_RECHARGE_COUPONS_USES_USERS]
GO
ALTER TABLE [dbo].[RECHARGE_VALUES]  WITH CHECK ADD  CONSTRAINT [FK_RECHARGE_VALUES_INSTALLATIONS] FOREIGN KEY([RECHVAL_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[RECHARGE_VALUES] CHECK CONSTRAINT [FK_RECHARGE_VALUES_INSTALLATIONS]
GO
ALTER TABLE [dbo].[SERVICE_CHARGE_TYPES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGE_TYPES_LITERALS] FOREIGN KEY([SECHT_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGE_TYPES] CHECK CONSTRAINT [FK_SERVICE_CHARGE_TYPES_LITERALS]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGES_CURRENCIES] FOREIGN KEY([SECH_AMOUNT_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] CHECK CONSTRAINT [FK_SERVICE_CHARGES_CURRENCIES]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGES_CURRENCIES1] FOREIGN KEY([SECH_BALANCE_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] CHECK CONSTRAINT [FK_SERVICE_CHARGES_CURRENCIES1]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGES_CUSTOMER_PAYMENT_MEANS_RECHARGES] FOREIGN KEY([SECH_CUSPMR_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ([CUSPMR_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] CHECK CONSTRAINT [FK_SERVICE_CHARGES_CUSTOMER_PAYMENT_MEANS_RECHARGES]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGES_SERVICE_CHARGE_TYPES] FOREIGN KEY([SECH_SECHT_ID])
REFERENCES [dbo].[SERVICE_CHARGE_TYPES] ([SECHT_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] CHECK CONSTRAINT [FK_SERVICE_CHARGES_SERVICE_CHARGE_TYPES]
GO
ALTER TABLE [dbo].[SERVICE_CHARGES]  WITH CHECK ADD  CONSTRAINT [FK_SERVICE_CHARGES_USERS] FOREIGN KEY([SECH_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[SERVICE_CHARGES] CHECK CONSTRAINT [FK_SERVICE_CHARGES_USERS]
GO
ALTER TABLE [dbo].[TARIFFS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_LITERALS] FOREIGN KEY([TAR_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[TARIFFS] CHECK CONSTRAINT [FK_TARIFFS_LITERALS]
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_GROUPS_GROUPS] FOREIGN KEY([TARGR_GRP_ID])
REFERENCES [dbo].[GROUPS] ([GRP_ID])
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS] CHECK CONSTRAINT [FK_TARIFFS_IN_GROUPS_GROUPS]
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_GROUPS_GROUPS_TYPES] FOREIGN KEY([TARGR_GRPT_ID])
REFERENCES [dbo].[GROUPS_TYPES] ([GRPT_ID])
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS] CHECK CONSTRAINT [FK_TARIFFS_IN_GROUPS_GROUPS_TYPES]
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_GROUPS_LITERALS] FOREIGN KEY([TARGR_LIT_ID])
REFERENCES [dbo].[LITERALS] ([LIT_ID])
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS] CHECK CONSTRAINT [FK_TARIFFS_IN_GROUPS_LITERALS]
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS]  WITH CHECK ADD  CONSTRAINT [FK_TARIFFS_IN_GROUPS_TARIFFS] FOREIGN KEY([TARGR_TAR_ID])
REFERENCES [dbo].[TARIFFS] ([TAR_ID])
GO
ALTER TABLE [dbo].[TARIFFS_IN_GROUPS] CHECK CONSTRAINT [FK_TARIFFS_IN_GROUPS_TARIFFS]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_CURRENCIES_1] FOREIGN KEY([TIPA_AMOUNT_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_CURRENCIES_1]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_CURRENCIES_2] FOREIGN KEY([TIPA_BALANCE_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_CURRENCIES_2]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_CUSTOMER_PAYMENT_MEANS_RECHARGES] FOREIGN KEY([TIPA_CUSPMR_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS_RECHARGES] ([CUSPMR_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_CUSTOMER_PAYMENT_MEANS_RECHARGES]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_INSTALLATIONS] FOREIGN KEY([TIPA_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_INSTALLATIONS]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_OPERATIONS_DISCOUNTS] FOREIGN KEY([TIPA_OPEDIS_ID])
REFERENCES [dbo].[OPERATIONS_DISCOUNTS] ([OPEDIS_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_OPERATIONS_DISCOUNTS]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_USERS] FOREIGN KEY([TIPA_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_USERS]
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS_SESSION_INFO]  WITH CHECK ADD  CONSTRAINT [FK_TICKET_PAYMENTS_SESSION_INFO_MOBILE_SESSIONS] FOREIGN KEY([TPSI_MOSE_ID])
REFERENCES [dbo].[MOBILE_SESSIONS] ([MOSE_ID])
GO
ALTER TABLE [dbo].[TICKET_PAYMENTS_SESSION_INFO] CHECK CONSTRAINT [FK_TICKET_PAYMENTS_SESSION_INFO_MOBILE_SESSIONS]
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS_SENDING]  WITH CHECK ADD  CONSTRAINT [FK_USER_PLATE_MOVS_SENDING_INSTALLATIONS] FOREIGN KEY([USRPMS_INS_ID])
REFERENCES [dbo].[INSTALLATIONS] ([INS_ID])
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS_SENDING] CHECK CONSTRAINT [FK_USER_PLATE_MOVS_SENDING_INSTALLATIONS]
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS_SENDING]  WITH CHECK ADD  CONSTRAINT [FK_USER_PLATE_MOVS_SENDING_USER_PLATE_MOVS] FOREIGN KEY([USRPMS_USRPMD_ID])
REFERENCES [dbo].[USER_PLATE_MOVS] ([USRPM_ID])
GO
ALTER TABLE [dbo].[USER_PLATE_MOVS_SENDING] CHECK CONSTRAINT [FK_USER_PLATE_MOVS_SENDING_USER_PLATE_MOVS]
GO
ALTER TABLE [dbo].[USER_PLATES]  WITH CHECK ADD  CONSTRAINT [FK_USER_PLATES_USERS] FOREIGN KEY([USRP_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USER_PLATES] CHECK CONSTRAINT [FK_USER_PLATES_USERS]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_COUNTRIES] FOREIGN KEY([USR_COU_ID])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_COUNTRIES]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_COUNTRIES_MAIN_TEL] FOREIGN KEY([USR_MAIN_TEL_COUNTRY])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_COUNTRIES_MAIN_TEL]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_COUNTRIES_SECUND_TEL] FOREIGN KEY([USR_SECUND_TEL_COUNTRY])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_COUNTRIES_SECUND_TEL]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_CURRENCIES] FOREIGN KEY([USR_CUR_ID])
REFERENCES [dbo].[CURRENCIES] ([CUR_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_CURRENCIES]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_CUSTOMER_PAYMENT_MEANS] FOREIGN KEY([USR_CUSPM_ID])
REFERENCES [dbo].[CUSTOMER_PAYMENT_MEANS] ([CUSPM_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_CUSTOMER_PAYMENT_MEANS]
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_CUSTOMERS] FOREIGN KEY([USR_CUS_ID])
REFERENCES [dbo].[CUSTOMERS] ([CUS_ID])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERS_CUSTOMERS]
GO
ALTER TABLE [dbo].[USERS_EMAILS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_EMAILS_CUSTOMER_INSCRIPTIONS] FOREIGN KEY([USRE_CUSINS_ID])
REFERENCES [dbo].[CUSTOMER_INSCRIPTIONS] ([CUSINS_ID])
GO
ALTER TABLE [dbo].[USERS_EMAILS] CHECK CONSTRAINT [FK_USERS_EMAILS_CUSTOMER_INSCRIPTIONS]
GO
ALTER TABLE [dbo].[USERS_EMAILS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_EMAILS_USERS] FOREIGN KEY([USRE_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USERS_EMAILS] CHECK CONSTRAINT [FK_USERS_EMAILS_USERS]
GO
ALTER TABLE [dbo].[USERS_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_TOAST_NOTIFICATIONS_USERS] FOREIGN KEY([UNO_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USERS_NOTIFICATIONS] CHECK CONSTRAINT [FK_USERS_TOAST_NOTIFICATIONS_USERS]
GO
ALTER TABLE [dbo].[USERS_PUSH_ID]  WITH CHECK ADD  CONSTRAINT [FK_USERS_PUSH_ID_USERS] FOREIGN KEY([UPID_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USERS_PUSH_ID] CHECK CONSTRAINT [FK_USERS_PUSH_ID_USERS]
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_SECURITY_OPERATIONS_COUNTRIES] FOREIGN KEY([USOP_NEW_MAIN_TEL_COUNTRY])
REFERENCES [dbo].[COUNTRIES] ([COU_ID])
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS] CHECK CONSTRAINT [FK_USERS_SECURITY_OPERATIONS_COUNTRIES]
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_SECURITY_OPERATIONS_USERS] FOREIGN KEY([USOP_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USERS_SECURITY_OPERATIONS] CHECK CONSTRAINT [FK_USERS_SECURITY_OPERATIONS_USERS]
GO
ALTER TABLE [dbo].[USERS_SMSS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_SMSS_CUSTOMER_INSCRIPTIONS] FOREIGN KEY([USRS_CUSINS_ID])
REFERENCES [dbo].[CUSTOMER_INSCRIPTIONS] ([CUSINS_ID])
GO
ALTER TABLE [dbo].[USERS_SMSS] CHECK CONSTRAINT [FK_USERS_SMSS_CUSTOMER_INSCRIPTIONS]
GO
ALTER TABLE [dbo].[USERS_SMSS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_SMSS_USERS] FOREIGN KEY([USRS_USR_ID])
REFERENCES [dbo].[USERS] ([USR_ID])
GO
ALTER TABLE [dbo].[USERS_SMSS] CHECK CONSTRAINT [FK_USERS_SMSS_USERS]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COUNTRIES', @level2type=N'COLUMN',@level2name=N'COU_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COUNTRIES', @level2type=N'COLUMN',@level2name=N'COU_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COUNTRIES', @level2type=N'COLUMN',@level2name=N'COU_CODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Telephone prefix' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COUNTRIES', @level2type=N'COLUMN',@level2name=N'COU_TEL_PREFIX'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currency' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'COUNTRIES', @level2type=N'COLUMN',@level2name=N'COU_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CURRENCIES', @level2type=N'COLUMN',@level2name=N'CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currencie name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CURRENCIES', @level2type=N'COLUMN',@level2name=N'CUR_NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currency Iso Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CURRENCIES', @level2type=N'COLUMN',@level2name=N'CUR_ISO_CODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Surname 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_SURNAME1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Surname 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_SURNAME2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Alternative telephone number without prefix' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_DOC_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile Telephone Prefix Country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_MAIN_TEL_COUNTRY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile Telephone without prefix' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_MAIN_TEL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Alternative Phone Prefix Country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_SECUND_TEL_COUNTRY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Alternative telephone number without prefix' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_SECUND_TEL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_EMAIL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activation Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_ACTIVATION_CODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL String Parameter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_URL_PARAMETER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of Activation Retries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_ACTIVATION_RETRIES'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of last send of email and sms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUSINS_LAST_SENT_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Culture for going on on step 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INSCRIPTIONS', @level2type=N'COLUMN',@level2name=N'CUISINS_CULTURE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_CUS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operator ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_OPR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_INV_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice Legal Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_INV_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice Generation Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_GENERATION_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Period covered ini date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_DATEINI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Period covered end date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_DATEEND'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operator invoice initial number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_OPR_INITIAL_INVOICE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operator invoice end number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_OPR_END_INVOICE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total amount of the invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_INV_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currency of the amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_INVOICES', @level2type=N'COLUMN',@level2name=N'CUSINV_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_CUS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type: 1:Credit Card,2: Paypal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_PAT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Payment Subtype ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_PAST_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Description for identification' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of last use' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_LAST_TIME_USERD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatic Recharge?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_AUTOMATIC_RECHARGE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantity to recharge automatically' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_AMOUNT_TO_RECHARGE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recharge when the amount is less than' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currency' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ekashu Card Hash' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_CARD_HASH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ekashu Card Reference' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_CARD_REFERENCE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ekashu Masked card number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_MASKED_CARD_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ekashu Card Expiration Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_CARD_EXPIRATION_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Card Type: VISA, Mastercard, Maestro, AMEX' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_CARD_SCHEMA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key Start of Validity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_START_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key End of Validity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_END_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key Max Number of Payments' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_NUMBER_PAYMENTS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key Max Total Amount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_TOTAL_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Paypal Preapproval Key Max Amount Per Payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_AMOUNT_PER_PAYMENT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutive Failed Automatic Retries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_AUTOMATIC_FAILED_RETRIES'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valid by default. After maximum failed retries:invalid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_VALID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Payment is enabled?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS', @level2type=N'COLUMN',@level2name=N'CUSPM_ENABLED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice who belongs to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER_PAYMENT_MEANS_RECHARGES', @level2type=N'COLUMN',@level2name=N'CUSPMR_CUSINV_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type: 1->Individual, 2:Company' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Main Usr ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_COU_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Documentation ID for individuals or Tax Id for companys' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_DOC_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Document Id type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_DOC_ID_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of individual (or Company name)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Surname for individuals, empty for companies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_SURNAME1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Surname 2 for individuals' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_SURNAME2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_STREET'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Street number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_STREE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Level number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_LEVEL_NUM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Door id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_DOOR'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Letter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_LETTER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stair' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_STAIR'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_CITY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_STATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Zip Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMERS', @level2type=N'COLUMN',@level2name=N'CUS_ZIPCODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Zone' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_ZONE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_TARIFF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Start Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_INIDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_ENDDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Start Date in UTC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_INIDATE_UTC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date in UTC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_ENDDATE_UTC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount Payed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time got' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_TIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Insertion notification' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_INSERTION_NOTIFIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ending notification' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_PARKING_OPERATIONS', @level2type=N'COLUMN',@level2name=N'EPO_ENDING_NOTIFIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date UTC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_DATE_UTC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_TICKET_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount to pay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Limit Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_LIMIT_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Limit Date UTC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_LIMIT_DATE_UTC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_ARTICLE_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_ARTICLE_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Insertion has been notified?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EXTERNAL_TICKET', @level2type=N'COLUMN',@level2name=N'EXTI_INSERTION_NOTIFIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation where it belongs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group External Id When communicating with external systems' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_EXT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group External ID 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS', @level2type=N'COLUMN',@level2name=N'GRP_EXT2_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_GRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in the sequence of points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_ORDER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_LATITUDE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_LONGITUDE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_INI_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'GRGE_END_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_HIERARCHY', @level2type=N'COLUMN',@level2name=N'GRHI_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parent Group ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_HIERARCHY', @level2type=N'COLUMN',@level2name=N'GRHI_GPR_ID_PARENT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Child Group ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_HIERARCHY', @level2type=N'COLUMN',@level2name=N'GRHI_GPR_ID_CHILD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_HIERARCHY', @level2type=N'COLUMN',@level2name=N'GRHI_INI_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_HIERARCHY', @level2type=N'COLUMN',@level2name=N'GRHI_END_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES', @level2type=N'COLUMN',@level2name=N'GRPT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES', @level2type=N'COLUMN',@level2name=N'GRPT_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Type Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES', @level2type=N'COLUMN',@level2name=N'GRPT_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'GTA_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'GTA_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'GTA_GRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Type Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GROUPS_TYPES_ASSIGNATIONS', @level2type=N'COLUMN',@level2name=N'GTA_GRPT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order in the sequence of points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_ORDER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_LATITUDE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitude' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_LONGITUDE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_INI_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INSTALLATIONS_GEOMETRY', @level2type=N'COLUMN',@level2name=N'INSGE_END_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LANGUAGES', @level2type=N'COLUMN',@level2name=N'LAN_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Language Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LANGUAGES', @level2type=N'COLUMN',@level2name=N'LAN_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'en-US, es-ES, fr-FR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LANGUAGES', @level2type=N'COLUMN',@level2name=N'LAN_CULTURE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LITERAL_LANGUAGES', @level2type=N'COLUMN',@level2name=N'LITL_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Language Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LITERAL_LANGUAGES', @level2type=N'COLUMN',@level2name=N'LITL_LAN_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LITERAL_LANGUAGES', @level2type=N'COLUMN',@level2name=N'LITL_LITERAL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LITERALS', @level2type=N'COLUMN',@level2name=N'LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LITERALS', @level2type=N'COLUMN',@level2name=N'LIT_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SessionID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_SESSIONID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Session Creation Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_CREATION_TIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Session Last update time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_LAST_UPDATE_TIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile WIFI MAC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_CELL_WIFI_MAC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile IMEI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_CELL_IMEI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile Model' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_CELL_MODEL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: Android; 2: Windows Phone; 3: iOS; 4:Blackberry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_OS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OS Version' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_OS_VERSION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Status: 1: Open; 2:Close' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MOBILE_SESSIONS', @level2type=N'COLUMN',@level2name=N'MOSE_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAT Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_VAT_PERC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Range init of invoices' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_INITIAL_INVOICE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Range End of numbers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_END_INVOICE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current invoice number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_CURRENT_INVOICE_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAT Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_VAT_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description for invoices' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_NAME_FOR_INVOICE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address for printing in invoices' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_ADDRESS_FOR_INVOICE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice Number Format' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_INVOICE_NUMBER_FORMAT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Format file for  last page invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_INVOICE_FORMAT_LAST_PAGE_FILE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Format file for no last page of invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OPERATORS', @level2type=N'COLUMN',@level2name=N'OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PARAMETERS', @level2type=N'COLUMN',@level2name=N'PAR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parameter name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PARAMETERS', @level2type=N'COLUMN',@level2name=N'PAR_NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parameter value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PARAMETERS', @level2type=N'COLUMN',@level2name=N'PAR_VALUE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Parameter Explaination' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PARAMETERS', @level2type=N'COLUMN',@level2name=N'PAR_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Payment Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_PAT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fixed Fee in cents to apply to Charges' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_FIXED_FEE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Percentual Fee to apply to charges' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_SUBTYPES', @level2type=N'COLUMN',@level2name=N'PAST_PERC_FEE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_TYPES', @level2type=N'COLUMN',@level2name=N'PAT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_TYPES', @level2type=N'COLUMN',@level2name=N'PAT_DESCRIPCION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_TYPES', @level2type=N'COLUMN',@level2name=N'PAT_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fixed Fee in cents to apply to Charges' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_TYPES', @level2type=N'COLUMN',@level2name=N'PAT_FIXED_FEE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Percentual Fee to apply to charges' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PAYMENT_TYPES', @level2type=N'COLUMN',@level2name=N'PAT_PERC_FEE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_GRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Type Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_GRPT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_TAR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_INI_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PLATES_TARIFFS', @level2type=N'COLUMN',@level2name=N'PLTA_END_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Toast Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_UTNO_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: Android; 2: Windows Phone; 3: iOS; 4:Blackberry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_OS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PushID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_PUSHID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WPText 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_TEXT1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WP Text 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_TEXT2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WP_Param' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_PARAM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Count for WP Tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_COUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title for WP tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_TILE_TITLE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Background for WP tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_BACKGROUND_IMAGE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Windows Phone Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_WP_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Android Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_ANDROID_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'iOS Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_iOS_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DateTime limit to send notification. After this has no sense' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_LIMITDATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'10: Inserted; 20: Sent: 30: Answer OK; 40: Queued; 50: Device Disconnected; 60: Answer KO; 70 Waiting next retry; 80: failed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of sending retries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_RETRIES'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Datetime of last retry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PUSHID_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'PNO_LAST_RETRY_DATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Coupon Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_CODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Coupon Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_COUPS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Coupon value in Cents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_VALUE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Currency ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Validity Start Date in UTC Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_START_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Expiration Date In UTC Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS', @level2type=N'COLUMN',@level2name=N'RCOUP_EXP_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_STATUS', @level2type=N'COLUMN',@level2name=N'RCOUPS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_STATUS', @level2type=N'COLUMN',@level2name=N'RCOUPS_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_STATUS', @level2type=N'COLUMN',@level2name=N'RCOUPS_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User trying to use recharge coupon' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recharge Coupon Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_RCOUP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of try' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Code Generated for using recharge coupon' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_CODE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile Session if applies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_COUPONS_USES', @level2type=N'COLUMN',@level2name=N'RCOUPU_MOSE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_VALUES', @level2type=N'COLUMN',@level2name=N'RECHVAL_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_VALUES', @level2type=N'COLUMN',@level2name=N'RECHVAL_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: Recharge Value; 2: Auotmatic Recharge Value; 3: Automatic Recharge Below' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RECHARGE_VALUES', @level2type=N'COLUMN',@level2name=N'RECHVAL_VALUE_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS', @level2type=N'COLUMN',@level2name=N'TAR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS', @level2type=N'COLUMN',@level2name=N'TAR_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS', @level2type=N'COLUMN',@level2name=N'TAR_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'External ID When communicationg external systems' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS', @level2type=N'COLUMN',@level2name=N'TAR_EXT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_GRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group Type Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_GRPT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tariff Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_TAR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Literal Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_LIT_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User is going to have the possiblity of select in front other chances' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_USER_SELECTABLE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ini Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_INI_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'End Date of application Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TARIFFS_IN_GROUPS', @level2type=N'COLUMN',@level2name=N'TARGR_END_APPLY_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate related with ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_USRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'String of the plate related with the ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_PLATE_STRING'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_TICKET_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_TICKET_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Payed amount in Cents' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount Currency' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_AMOUNT_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Balance Currency' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_BALANCE_CUR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Change Applied between currencies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_CHANGE_APPLIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Change Fee Applied to the ticket Payment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_CHANGE_FEE_APPLIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Final Amount with change and fee applied' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_FINAL_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID returnted by third party service' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_EXTERNAL_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Insertion UTC DATE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS', @level2type=N'COLUMN',@level2name=N'TIPA_INSERTION_UTC_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Session ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_MOSE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_UTC_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_TICKET_NUMBER'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate used in query' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount to be paid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_AMOUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fine Article Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_ARTICLE_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fine Article Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_ARTICLE_DESCRIPTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Query installation datetime' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_INS_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Change Applied in operation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TICKET_PAYMENTS_SESSION_INFO', @level2type=N'COLUMN',@level2name=N'TPSI_CHANGE_APPLIED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS', @level2type=N'COLUMN',@level2name=N'USRPM_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mov Type: I or D' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS', @level2type=N'COLUMN',@level2name=N'USRPM_MOV_TYPE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date of movement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS', @level2type=N'COLUMN',@level2name=N'USRPM_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS', @level2type=N'COLUMN',@level2name=N'USRPM_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate sending has been generated?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS', @level2type=N'COLUMN',@level2name=N'USRPM_SEND_INSERTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS_SENDING', @level2type=N'COLUMN',@level2name=N'USRPMS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plate mov reg' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS_SENDING', @level2type=N'COLUMN',@level2name=N'USRPMS_USRPMD_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Installation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS_SENDING', @level2type=N'COLUMN',@level2name=N'USRPMS_INS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last date of change' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS_SENDING', @level2type=N'COLUMN',@level2name=N'USRPMS_LAST_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Status: 1: Inserted; 2: Sending; 3:Sent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATE_MOVS_SENDING', @level2type=N'COLUMN',@level2name=N'USRPMS_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATES', @level2type=N'COLUMN',@level2name=N'USRP_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATES', @level2type=N'COLUMN',@level2name=N'USRP_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vehicle Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATES', @level2type=N'COLUMN',@level2name=N'USRP_PLATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is the default vehicle id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATES', @level2type=N'COLUMN',@level2name=N'USRP_IS_DEFAULT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is the plate enabled' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USER_PLATES', @level2type=N'COLUMN',@level2name=N'USRP_ENABLED'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificaor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Inscription Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_CUSINS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sending time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_RECIPIENT_ADDRESS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email Subject' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_SUBJECT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email Body' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_BODY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sending Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sender Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_EMAILS', @level2type=N'COLUMN',@level2name=N'USRE_SENDER_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WPText 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_TEXT1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WP Text 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_TEXT2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Toast WP_Param' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_PARAM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Count for WP Tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_COUNT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title for WP tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_TILE_TITLE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Background for WP tile notifications' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_BACKGROUND_IMAGE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Windows Phone Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_WP_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Android Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_ANDROID_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'iOS Raw Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_iOS_RAW_DATA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Start Date where the notification can be send' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_STARTDATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DateTime limit to send notification. After this has no sense' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_LIMITDATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'10: Inserted: 20: Generated; 30: Sending ; 40: Finished Partially: 50: Finished Completely' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_NOTIFICATIONS', @level2type=N'COLUMN',@level2name=N'UNO_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: Android; 2: Windows Phone; 3: iOS; 4:Blackberry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_OS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile WIFI MAC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_CELL_WIFI_MAC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mobile IMEI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_CELL_IMEI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Push Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_PUSHID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last update from mobile' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_LAST_UPDATE_DATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last sucessfull push' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_LAST_SUCESSFUL_PUSH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of consecutive failed retries' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_PUSH_RETRIES'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last Retry DateTime' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_PUSH_ID', @level2type=N'COLUMN',@level2name=N'UPID_LAST_RETRY_DATETIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificator' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_USR_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Customer Inscription Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_CUSINS_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sending Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_DATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recipient Telephone number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_RECIPIENT_TELEPHONE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMS Text' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_SMS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sending Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sender Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS_SMSS', @level2type=N'COLUMN',@level2name=N'USRS_SENDER_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 37
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2280
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ALL_OPERATIONS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ALL_OPERATIONS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ALL_OPERATIONS_EXT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ALL_OPERATIONS_EXT'
GO
USE [master]
GO
ALTER DATABASE [integraMobile] SET  READ_WRITE 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveRegisterDetails]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[SaveRegisterDetails] AS'
END
GO

/*-- =============================================
-- Author:		Xu JunJun
-- Create date: 18 Jul 2021
-- Description:	Save Register info 
-- ============================================= */

ALTER PROCEDURE [dbo].[SaveRegisterDetails] (
	 @Id uniqueIdentifier
	,@FirstName nvarchar(100)
	,@LastName nvarchar(100)
	,@BirthDate decimal(8,0)
	,@Sexy nvarchar(10)
    ,@Email nvarchar(100)
	,@PhoneNumber decimal(12,0)
	,@Password nvarchar(100)
	,@Balance decimal(10,2)
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY

	    IF EXISTS (SELECT 1 FROM [User] WHERE (Email = @Email OR PhoneNumber = @PhoneNumber) AND IsDeleted = 0) 
          BEGIN
               SELECT 'User with same email or phone number already registered.'
          END  
		ELSE
		   BEGIN
                INSERT INTO [User] 
				VALUES(@Id, @FirstName, @LastName,@BirthDate, @Sexy,@Email,@PhoneNumber,@Password,@Balance,GETDATE(),GETDATE(),GETDATE(),0)
				SELECT 'Registered successfully.' 
          END  
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		-- if error occurred in any step, rollback all transaction
		ROLLBACK TRANSACTION
		SELECT 'Error occurred, please contact our administrator for help'
		--TODO save detail error into error table for investigation
	END CATCH
END
GO
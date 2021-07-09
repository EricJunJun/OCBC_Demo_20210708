SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateBalance]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateBalance] AS'
END
GO

/*-- =============================================
-- Author:		Xu JunJun
-- Create date: 18 Jul 2021
-- Description:	Update balance by user id
-- ============================================= */

ALTER PROCEDURE [dbo].[UpdateBalance] (
	 @Id uniqueIdentifier
	,@Amount decimal(10,2)
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY

	    IF NOT EXISTS (SELECT 1 FROM [User] WHERE Id = @Id AND IsDeleted = 0) 
          BEGIN
               SELECT 'User not found.'
          END  
		ELSE
		   BEGIN
				IF (@Amount = 0 OR @Amount > 99999) 
                  BEGIN
                       SELECT 'Value should be greater than 0 and less than 999999.'
                  END  
		        ELSE
		          BEGIN
				  	   UPDATE [User] SET Balance = Balance + @Amount WHERE Id = @Id
					   SELECT 'Save successfully|' + Convert(varchar(100),Balance) FROM [User] WHERE Id = @Id
		          END
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
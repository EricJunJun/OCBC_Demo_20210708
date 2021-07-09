SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddTransactionInfo]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[AddTransactionInfo] AS'
END
GO

/*-- =============================================
-- Author:		Xu JunJun
-- Create date: 18 Jul 2021
-- Description:	Insert transaction info 
-- ============================================= */

ALTER PROCEDURE [dbo].[AddTransactionInfo] (
	 @Id uniqueIdentifier
	,@SenderId uniqueIdentifier
	,@RecipientEmail nvarchar(100)
	,@TransferAmount decimal(10,2)
	)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
	    DECLARE @RecipientId uniqueIdentifier

	    IF NOT EXISTS (SELECT 1 FROM [User] WHERE Email = @RecipientEmail AND IsDeleted = 0) 
          BEGIN
               SELECT 'Recipient email not found.'
          END  
		ELSE
		   BEGIN
		        SET @RecipientId = (SELECT Id FROM [User] WHERE Email = @RecipientEmail AND IsDeleted = 0)
				    IF (@TransferAmount> (SELECT Balance FROM [User] WHERE Id = @SenderId AND IsDeleted = 0)) 
                      BEGIN
                           SELECT 'Cannot tranfer more than your balance value.'
                      END  
		            ELSE
		              BEGIN
					  	   UPDATE [User] SET Balance = Balance - @TransferAmount WHERE Id = @SenderId
				           UPDATE [User] SET Balance = Balance + @TransferAmount WHERE Id = @RecipientId
                           INSERT INTO TransferHistory 
				           VALUES(NEWID(), @SenderId, @RecipientId, GETDATE(), @TransferAmount, GETDATE(), GETDATE(), 0)
						   SELECT 'Transfer successfully|' + Convert(varchar(100),Balance) FROM [User] WHERE Id = @SenderId
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
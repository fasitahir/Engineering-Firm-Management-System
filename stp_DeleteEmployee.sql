-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fasi Tahir
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE stp_DeleteEmployee
	@EmployeeId INT 
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY

		BEGIN Transaction
			UPDATE Employee 
			SET IsActive = 0 
			WHERE EmployeeID = @EmployeeId;


			DELETE FROM Credentials 
			WHERE EmployeeID = @EmployeeId
		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		if @@Trancount > 0
			BEGIN
				ROLLBACK TRANSACTION;
			END
		
		DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH;
END

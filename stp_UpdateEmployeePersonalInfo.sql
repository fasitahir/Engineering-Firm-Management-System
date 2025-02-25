USE [FinalProjectDB]
GO
/****** Object:  StoredProcedure [dbo].[spt_UpdateEmployeePersonalInfo]    Script Date: 10/05/2024 2:42:47 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fasi Tahir
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[stp_UpdateEmployeePersonalInfo] 
	-- Add the parameters for the stored procedure here
	@FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Address NVARCHAR(255),
    @PrimaryPhone NVARCHAR(15),
    @AlternatePhone NVARCHAR(15) = NULL,
    @CNIC NVARCHAR(13),
    @Email NVARCHAR(255),
	@EmployeeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY

    -- Insert statements for procedure here
		UPDATE Person
		SET FirstName = @FirstName, LastName = @LastName, CNIC = @CNIC, PrimaryPhone = @PrimaryPhone, AlternatePhone = @AlternatePhone, Address = @Address, Email = @Email
		WHERE PersonID = @EmployeeId
	
	END TRY

	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(MAX);
		SET @ErrorMessage = Error_Message();
		RAISERROR(@ErrorMessage, 16, 1);
	END CATCH
END

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
CREATE PROCEDURE stp_AddEmployee 
	@Designation INT,
    @EmployeeId INT,
    @EmployeeNo NVARCHAR(50),
    @StartDate DATETIME,
    @BaseSalary DECIMAL(8, 2),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Gender INT,
    @DateOfBirth DATETIME,
    @Address NVARCHAR(255),
    @PrimaryPhone NVARCHAR(15),
    @AlternatePhone NVARCHAR(15) = NULL,
    @CNIC NVARCHAR(13),
    @Email NVARCHAR(255),
    @Relation INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @PersonId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if a person with the same CNIC and email exists
        DECLARE @ExistingPersonId INT;
        SELECT @ExistingPersonId = PersonID 
        FROM Person 
        WHERE CNIC = @CNIC OR Email = @Email;

        -- Insert employee details
        INSERT INTO Employee
        VALUES (@EmployeeId, @Designation, null, null, @EmployeeNo, @StartDate, null, 1, @BaseSalary);

        -- If a person with the same CNIC and email already exists, use their ID
        IF @ExistingPersonId IS NULL
        BEGIN
            -- Insert new person details
            INSERT INTO Person
            VALUES (@Gender, @CNIC, @FirstName, @LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address);

            -- Get the ID of the newly inserted person
            SET @PersonId = SCOPE_IDENTITY();
        END
        
		ELSE
        BEGIN
            SET @PersonId = @ExistingPersonId;
        END

        -- Insert emergency contact details
        INSERT INTO EmergencyContact
        VALUES (@PersonId, @EmployeeId, @Relation);

        -- Update Applicant
        UPDATE Applicant
        SET IsSelected = 1
        WHERE ApplicantID = @EmployeeId;

        -- Commit transaction
        COMMIT TRANSACTION;
    END TRY
   
    BEGIN CATCH
        -- Rollback transaction if an error occurs
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Raise error
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH;

END;
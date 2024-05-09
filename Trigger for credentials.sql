CREATE TRIGGER AddCredentialsOnEmployeeInsert
ON Employee
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmployeeId INT;
    DECLARE @PersonId INT;
    DECLARE @FirstName NVARCHAR(50);
    DECLARE @Username NVARCHAR(100);
    DECLARE @Password NVARCHAR(10); -- Assuming password will be a 4-digit pin
    DECLARE @UpdatedBy NVARCHAR(50);
    DECLARE @UpdatedOn DATETIME;

    -- Get the newly inserted employee's information
    SELECT @EmployeeId = inserted.EmployeeID
    FROM inserted;

    -- Retrieve the first name of the person associated with the newly inserted employee
    SELECT @FirstName = FirstName
    FROM Person
    WHERE PersonID = @EmployeeId;

    -- Generate the username (using first name and employee ID)
    SET @Username = @FirstName + CAST(@EmployeeId AS NVARCHAR(50));

    -- Generate a random 4-digit pin for the password
    SET @Password = RIGHT('0000' + CAST(CAST(RAND() * 10000 AS INT) AS NVARCHAR(4)), 4);

    -- Set the UpdatedBy and UpdatedOn fields to NULL for now
    SET @UpdatedBy = NULL;
    SET @UpdatedOn = NULL;

    -- Insert the credentials into the Credentials table
    INSERT INTO Credentials (Username, Password, EmployeeID, UpdatedBy, UpdatedOn)
    VALUES (@Username, @Password, @EmployeeId, @UpdatedBy, @UpdatedOn);
END;

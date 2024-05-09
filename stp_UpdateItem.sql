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
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE stp_UpdateItem
	@ItemName nvarchar(100),
	@ItemID int,
	@Description nvarchar(300),
	@MeasurementUnit nvarchar(50),
	@SalePrice decimal(8,2),
	@UpdatedBy int,
	@UpdatedOn date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		UPDATE Item 
		SET ItemName = @ItemName, Description = @Description, MeasurementUnit = @MeasurementUnit, SalePrice = @SalePrice, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
		WHERE @ItemID = ItemID
END
GO

USE [ReqGraduaciones]
GO
/****** Object:  UserDefinedFunction [dbo].[fnNotificaciones_NotificacionBienvenidaRegistrada]    Script Date: 26/07/2023 10:14:07 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 15 / 11 / 2022
-- Description:	Verificar si se ha mostrado notificacion de bienvenida a prospecto 
-- =============================================
ALTER FUNCTION [dbo].[fnNotificaciones_NotificacionBienvenidaRegistrada]
(
	@Matricula varchar(50),
	@TipoCorreo INT
)
RETURNS BIT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result BIT = 0

	-- Add the T-SQL statements to compute the return value here
		IF (SELECT COUNT(*) FROM CorreosEnviadosHabilitador WHERE Matricula = @Matricula AND TipoCorreo = @TipoCorreo) > 0
		SET @Result = 0
	ELSE 
		SET @Result = 1
	-- Return the result of the function
	RETURN @Result 

END


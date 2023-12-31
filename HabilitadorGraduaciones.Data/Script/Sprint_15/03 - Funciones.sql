USE [ReqGraduaciones]
GO
PRINT('[dbo].[fnUsuarios_ObtenerClaveCampusHistorial]  ');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar el historial de las claves de campus que ha tenido el usuario 
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerClaveCampusHistorial]
(
	@IdUsuario INT,
	@IdHistorico INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX) 

	-- Add the T-SQL statements to compute the return value here
	IF (SELECT COUNT(Id) FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario) > 0
	BEGIN
		SET @Result = (SELECT STUFF(
			(SELECT DISTINCT CAST(', ' AS VARCHAR(MAX)) + C.CLAVE_CAMPUS
			FROM [Replicas].[dbo].CAMPUS_SEDES C
			INNER JOIN UsuarioCampusSede UC ON C.CLAVE_CAMPUS = UC.ClaveCampus
			WHERE UC.IdUsuario = @IdUsuario AND UC.IdHistorial = @IdHistorico
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END

GO

PRINT('[dbo].[fnUsuarios_ObtenerClaveNivelesHistorial]');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar la clave nivel que tiene el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerClaveNivelesHistorial]
(
	@IdUsuario INT,
	@IdHistorico INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX) 

	-- Add the T-SQL statements to compute the return value here
	IF (SELECT COUNT(Id) FROM UsuarioNivel WHERE IdUsuario = @IdUsuario) > 0
	BEGIN
		SET @Result = (SELECT STUFF(
			(SELECT CAST(', ' AS VARCHAR(MAX)) + N.CLAVE_NIVEL_ACADEMICO
			FROM [Replicas].[dbo].DIM_R_ESCO_NIVEL_ACADEMICO N
			INNER JOIN UsuarioNivel UN ON UN.ClaveNivel = N.CLAVE_NIVEL_ACADEMICO
			WHERE UN.IdUsuario = @IdUsuario AND UN.IdHistorial = @IdHistorico
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END



GO

PRINT('[dbo].[fnUsuarios_ObtenerIdRolesHistorial]');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar historial de id de roles que ha tenido el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerIdRolesHistorial]
(
	@IdUsuario INT,
	@IdHistorico INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX) 

	-- Add the T-SQL statements to compute the return value here
	IF (SELECT COUNT(Id) FROM UsuarioRoles WHERE IdUsuario = @IdUsuario) > 0
	BEGIN
		SET @Result = (SELECT STUFF(
			(SELECT CAST(', ' AS VARCHAR(MAX)) + CAST(R.IdRol AS VARCHAR(10))
			FROM Roles R
			INNER JOIN UsuarioRoles UR ON R.IdRol = UR.IdRol
			WHERE UR.IdUsuario = @IdUsuario AND UR.IdHistorial = @IdHistorico
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END



GO

PRINT('[dbo].[fnUsuarios_ObtenerClaveSedesHistorial]');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar historico de clave sedes que ha tenido el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerClaveSedesHistorial]
(
	@IdUsuario INT,
	@IdHistorico INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX) 

	-- Add the T-SQL statements to compute the return value here
	IF (SELECT COUNT(Id) FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario) > 0
	BEGIN
		SET @Result = (SELECT STUFF(
			(SELECT CAST(', ' AS VARCHAR(MAX)) + S.CLAVE_TIPO_SEDE
			FROM [Replicas].[dbo].CAMPUS_SEDES S
			INNER JOIN UsuarioCampusSede UC ON S.CLAVE_TIPO_SEDE = UC.ClaveSede
			WHERE UC.IdUsuario = @IdUsuario AND UC.IdHistorial = @IdHistorico
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END
USE [ReqGraduaciones]
GO
PRINT('Funciones')
PRINT('[dbo].[fnRoles_ObtenerNombreUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Carlos Bernadac
-- Create date: 06/06/2023
-- Description:	Obtiene el Nombre de un  Usuario en especifico 
-- =============================================
CREATE FUNCTION [dbo].[fnRoles_ObtenerNombreUsuario]
(
	@usuario varchar(9)
)
RETURNS Varchar(150)
AS
BEGIN

	DECLARE @NombreCompleto	Varchar(150)

	SET @NombreCompleto = (SELECT
								CONCAT(A.NOMBRE, ' ', A.APELLIDO_PATERNO, ' ', A.APELLIDO_MATERNO) 
							FROM 
								[Replicas].[dbo].R_IDEN_PERSONA A
							LEFT JOIN 
								[Replicas].[dbo].R_IDEN_AFILIACION B 
							ON	
								B.CLAVE_IDENTIDAD = A.CLAVE_IDENTIDAD
							WHERE
								B.ID_AFILIACION = @usuario)
	RETURN @NombreCompleto

END
		
GO
PRINT(' [dbo].[fnRoles_ObtenerUsuariosPorRol] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Carlos Bernadac
-- Create date: 05/06/2023
-- Description:	Obtiene el total de Usuarios de un rol especifico 
-- =============================================
CREATE FUNCTION [dbo].[fnRoles_ObtenerUsuariosPorRol]
(
	@idRol int
)
RETURNS Int
AS
BEGIN

	DECLARE @Total	Int

	SET @Total = (SELECT COUNT([IdUsuario])  FROM [dbo].[UsuarioRoles]  WHERE [IdRol] = @idRol  AND Estatus = 1)

	RETURN @Total

END
GO
PRINT(' [dbo].[fnUsuarios_ObtenerCampusUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24 / 05 / 2023
-- Description:	Consultar los campus asignados que tiene el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerCampusUsuario]
(
	@IdUsuario INT
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
			(SELECT DISTINCT CAST(', ' AS VARCHAR(MAX)) + C.DESC_CAMPUS
			FROM [Replicas].[dbo].CAMPUS_SEDES C
			INNER JOIN UsuarioCampusSede UC ON C.CLAVE_CAMPUS = UC.ClaveCampus
			WHERE UC.IdUsuario = @IdUsuario AND UC.Estatus = 1
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END

GO
PRINT('[dbo].[fnUsuarios_ObtenerCampusUsuarioHistorial] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar el historial de los campus que ha tenido el usuario 
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerCampusUsuarioHistorial]
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
			(SELECT DISTINCT CAST(', ' AS VARCHAR(MAX)) + C.DESC_CAMPUS
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
PRINT(' [dbo].[fnUsuarios_ObtenerNivelesUsuarioHistorial] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar los roles asignados que tiene el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerNivelesUsuarioHistorial]
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
			(SELECT CAST(', ' AS VARCHAR(MAX)) + N.DESC_NIVEL_ACADEMICO
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
PRINT(' [dbo].[fnUsuarios_ObtenerRolesPorUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Carlos Bernadac
-- Create date: 05/06/2023
-- Description:	Obtiene el total de Roles que tiene un Usuario en especifico 
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerRolesPorUsuario]
(
	@idUsuario int
)
RETURNS Int
AS
BEGIN

	DECLARE @Total	Int

	SET @Total = (SELECT COUNT([IdRol])  FROM [dbo].[UsuarioRoles]  WHERE[IdUsuario] = @idUsuario  AND Estatus = 1)

	RETURN @Total

END
GO
PRINT(' [dbo].[fnUsuarios_ObtenerRolesUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24 / 05 / 2023
-- Description:	Consultar los roles asignados que tiene el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerRolesUsuario]
(
	@IdUsuario INT
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
			(SELECT CAST(', ' AS VARCHAR(MAX)) + R.Descripcion
			FROM Roles R
			INNER JOIN UsuarioRoles UR ON R.IdRol = UR.IdRol
			WHERE UR.IdUsuario = @IdUsuario AND UR.Estatus = 1
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END

GO
PRINT(' [dbo].[fnUsuarios_ObtenerRolesUsuarioHistorial] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar historial de roles que ha tenido el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerRolesUsuarioHistorial]
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
			(SELECT CAST(', ' AS VARCHAR(MAX)) + R.Descripcion
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
PRINT(' [dbo].[fnUsuarios_ObtenerSedesUsuarioHistorial] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 01 / 06 / 2023
-- Description:	Consultar historico de sedes que ha tenido el usuario
-- =============================================
CREATE FUNCTION [dbo].[fnUsuarios_ObtenerSedesUsuarioHistorial]
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
			(SELECT CAST(', ' AS VARCHAR(MAX)) + S.DESC_TIPO_SEDE
			FROM [Replicas].[dbo].CAMPUS_SEDES S
			INNER JOIN UsuarioCampusSede UC ON S.CLAVE_TIPO_SEDE = UC.ClaveSede
			WHERE UC.IdUsuario = @IdUsuario AND UC.IdHistorial = @IdHistorico
			FOR XML PATH('')
			), 1, 1, ''))
	END
	-- Return the result of the function
	RETURN @Result 

END
GO

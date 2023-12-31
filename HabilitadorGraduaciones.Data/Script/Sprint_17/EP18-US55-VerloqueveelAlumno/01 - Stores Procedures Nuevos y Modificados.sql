USE [ReqGraduaciones]
GO
PRINT(' [dbo].[spUsuarios_ObtenerUsuarioPorMatriculaONombre] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Antonio Mendoza
-- Create date: 24/11/2022
-- Description:	Obten Alumnos prospecto candidatos por matricula o nombre
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_ObtenerUsuarioPorMatriculaONombre]
	@IsMatricula		bit,
	@Busqueda			varchar(200),
	@pIdUsuario			int
AS
BEGIN
	SET NOCOUNT ON;
	IF @IsMatricula = 0
		BEGIN

			SELECT 
				P.MATRICULA, 
				ISNULL(PE.NOMBRE,'') + ' ' + ISNULL(PE.APELLIDO_PATERNO,'') + ' ' + ISNULL(PE.APELLIDO_MATERNO,'') as NOMBRE, 
				P.DESC_MAJR_CODE AS CARRERA
			FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
				LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
			WHERE 
				UPPER(CONCAT(PE.NOMBRE,' ',PE.APELLIDO_PATERNO,' ',PE.APELLIDO_MATERNO)) LIKE '%'+UPPER(@Busqueda) +'%'
			AND P.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)
			ORDER BY  PE.APELLIDO_PATERNO, PE.APELLIDO_MATERNO, PE.NOMBRE

		END
	ELSE
		BEGIN 
			
			SELECT 
				P.MATRICULA, 
				ISNULL(PE.NOMBRE,'') + ' ' + ISNULL(PE.APELLIDO_PATERNO,'') + ' ' + ISNULL(PE.APELLIDO_MATERNO,'') as NOMBRE, 
				P.DESC_MAJR_CODE AS CARRERA
			FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
				LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
			WHERE p.MATRICULA = @Busqueda
			AND P.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)
			ORDER BY PE.APELLIDO_PATERNO, PE.APELLIDO_MATERNO, PE.NOMBRE

		END
END
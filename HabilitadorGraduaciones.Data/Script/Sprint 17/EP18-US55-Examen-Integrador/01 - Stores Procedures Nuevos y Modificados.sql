USE [ReqGraduaciones]
GO
PRINT('[dbo].[spExamenIntegrador_ObtenExamenesActivos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        Jose Antonio Mendoza Guevara
-- Create date: 03-11-2022
-- Description:    Obtener todos los examenes de la tabla examen integrador
-- =============================================
ALTER PROCEDURE [dbo].[spExamenIntegrador_ObtenExamenesActivos]
	@pIdUsuario			int
AS
BEGIN
	SET NOCOUNT ON;

   	SELECT EI.MATRICULA
		,EI.PERIODO_GRADUACION
		,EI.NIVEL_ACADEMICO
		,EI.NOMBRE_REQUISITO
		,EI.ESTATUS
		,EI.FECHA_EXAMEN
	FROM [dbo].[ExamenIntegrador] EI
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = EI.MATRICULA 
	WHERE 
	EI.ACTIVO = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	
END

USE [ReqGraduaciones]
GO
PRINT('[dbo].[spSedes_ObtenerSedesPorCampusNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene las Sedes que tiene un usuario en específico por su numero de nomina  y Campus
-- =============================================
ALTER PROCEDURE [dbo].[spSedes_ObtenerSedesPorCampusNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9),
	@pClaveCampus			Varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT 
			UC.[ClaveSede]
			,S.[DESC_TIPO_SEDE] AS Descripcion
		FROM 
			Usuarios U
		LEFT JOIN 
			UsuarioCampusSede UC 
		ON 
			U.IdUsuario = UC.IdUsuario AND UC.Estatus = 1
		LEFT JOIN 
			[Replicas].[dbo].CAMPUS_SEDES S 
		ON 
			UC.ClaveSede = S.CLAVE_TIPO_SEDE
		WHERE 
			UC.ClaveCampus = @pClaveCampus  
		AND 
			U.NumeroNomina = @pNumeroNomina
		AND 
			U.Estatus = 1
END

GO
PRINT(' [dbo].[spAlumnosProspCandidatos_ObtenerPorIdUsuarioMatricula] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 04-07-2023
-- Description:	Verifica si el alumno concuerda con los permisos del administrador (Campus, Sedes y Nivel)
-- =============================================
ALTER PROCEDURE [dbo].[spAlumnosProspCandidatos_ObtenerPorIdUsuarioMatricula]
	@pIdUsuario		int, 
	@pMatricula		varchar(09)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		COUNT([MATRICULA]) AS Existe
	FROM 									
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] 
	WHERE
		MATRICULA = @pMatricula 
		AND CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
		AND SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
		AND NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)					
END

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
	ACTIVO = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)	
END

GO
PRINT('[dbo].[spExpediente_ObtenExpedientes]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Antonio Mendoza
-- Create date: 25/10/2022
-- Description:	Obtiene todos los expedientes
-- =============================================
ALTER PROCEDURE [dbo].[spExpediente_ObtenExpedientes]
	@pIdUsuario			int
AS
BEGIN
	SET NOCOUNT ON

	SELECT E.MATRICULA
		,E.ESTATUS
		,E.DETALLE
		,E.FECHA_REGISTRO AS 'ULTIMAACTUALIZACION'
	FROM [dbo].[Expedientes]	E
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = E.Matricula 
	WHERE 
		ACTIVO = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)	
END

GO
PRINT('[dbo].[spNivel_ObtenerNivelesPorNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los Niveles que tiene un usuario en específico por su numero de nomina  
-- =============================================
ALTER PROCEDURE [dbo].[spNivel_ObtenerNivelesPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT 
			UN.[ClaveNivel] 
			,N.[DESC_NIVEL_ACADEMICO] AS Descripcion
		FROM 
			Usuarios U
		LEFT JOIN 
			UsuarioNivel UN 
		ON 
			U.IdUsuario = UN.IdUsuario AND UN.Estatus = 1
		LEFT JOIN 
			[Replicas].[dbo].DIM_R_ESCO_NIVEL_ACADEMICO N 
		ON 
			UN.[ClaveNivel] = N.[CLAVE_NIVEL_ACADEMICO]
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
		AND 
			U.Estatus = 1
END

GO
PRINT('[dbo].[spSolicitudCambioDatosPersonales_ObtenerPendientes]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 06-03-2023
-- Description:	Cuenta las solicitudes Pendientes (No Abiertas y En Revision)
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudCambioDatosPersonales_ObtenerPendientes]

	@pTotalPendientes	int	OutPut,
	@pIdUsuario			int
AS
BEGIN

	SET NOCOUNT ON;

		SET @pTotalPendientes = ISNULL((SELECT COUNT(S.IdSolicitud)
								FROM 
									SolicitudCambioDatosPersonales S
								INNER JOIN 
									[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
								ON 
									a.MATRICULA = s.Matricula 
								WHERE s.[IdEstatusSolicitud] IN (1,2) AND s.[Activa] = 1
								AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
								AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
								AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)),0)	
END

GO
PRINT('[dbo].[spSolicitudesDatosPersonales_Obtener]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 28-02-2023
-- Description:	Selecciona las solicitudes segun su estatus de solicitud 
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudesDatosPersonales_Obtener]
	-- Add the parameters for the stored procedure here
	@pIdEstatusSolicitud		int,
	@pIdUsuario					int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		S.[IdSolicitud]
		,S.[NumeroSolicitud]
		,S.[Matricula]
		,S.[PeriodoGraduacion]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[UltimaActualizacion]
		,S.[IdEstatusSolicitud]
		,ES.[Descripcion] AS Estatus
	FROM
		[dbo].[SolicitudCambioDatosPersonales] S
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON
	 S.[IdEstatusSolicitud] = ES.[IdEstatusSolicitud]
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = S.Matricula 
	WHERE 
		S.[IdEstatusSolicitud] = @pIdEstatusSolicitud
	AND 
		S.[Activa] = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)	

END

GO
PRINT('[dbo].[spSolicitudesDatosPersonales_ObtenerPendientes]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 06-03-2023
-- Description:	Selecciona las solicitudes Pendientes (No Abiertas y En Revision)
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudesDatosPersonales_ObtenerPendientes]

	@pIdUsuario			int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		S.[IdSolicitud]
		,S.[NumeroSolicitud]
		,S.[Matricula]
		,S.[PeriodoGraduacion]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[UltimaActualizacion]
		,S.[IdEstatusSolicitud]
		,ES.[Descripcion] AS Estatus
	FROM
		[dbo].[SolicitudCambioDatosPersonales] S
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON
	 S.[IdEstatusSolicitud] = ES.[IdEstatusSolicitud]
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = S.Matricula 
	WHERE 
		S.[IdEstatusSolicitud] IN (1,2)	 -- 1)No Abiertas 2)En Revision
	AND 
		S.[Activa] = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario AND Estatus = 1)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario AND Estatus = 1)	
END

GO
PRINT('[dbo].[spCampus_ObtenerCampusPorNomina] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los Campus que tiene un usuario en específico por su numero de nomina  
-- =============================================
ALTER PROCEDURE [dbo].[spCampus_ObtenerCampusPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT
			UC.[ClaveCampus]
			,C.[DESC_CAMPUS] AS Descripcion
		FROM 
			[dbo].[Usuarios] U
		LEFT JOIN 
			[dbo].[UsuarioCampusSede] UC
		ON 
			U.[IdUsuario] = UC.[IdUsuario] AND  UC.[Estatus] = 1
		LEFT JOIN 
			[Replicas].[dbo].CAMPUS_SEDES C 
		ON 
			UC.[ClaveCampus] = C.[CLAVE_CAMPUS]
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
		AND 
			U.[Estatus] = 1
END

GO
PRINT('[dbo].[spRoles_ObtenerRolesPorNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los roles que tiene un usuario en específico por su numero de nomina  
-- =============================================
ALTER PROCEDURE [dbo].[spRoles_ObtenerRolesPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT 
			R.[IdRol]
			,R.[Descripcion]
		FROM 
			[dbo].[Usuarios] U
		LEFT JOIN 
			[dbo].[UsuarioRoles] UR 
		ON 
			U.[IdUsuario] = UR.[IdUsuario] AND  UR.[Estatus] = 1
		LEFT JOIN 
			Roles R 
		ON 
			UR.[IdRol] = R.[IdRol] AND R.[Activo] = 1
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
		AND 
			U.[Estatus] = 1
END

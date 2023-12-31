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
			AND P.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)
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
			AND P.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
			AND P.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)
			ORDER BY PE.APELLIDO_PATERNO, PE.APELLIDO_MATERNO, PE.NOMBRE

		END
END

GO
PRINT(' [dbo].[spExamenIntegrador_ObtenExamenesActivos] ')
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
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	
END

GO
PRINT(' [dbo].[spExpediente_ObtenExpedientes]  ')
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
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	
END

GO
PRINT(' [dbo].[spSolicitudesDatosPersonales_Obtener] ')
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
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	

END

GO
PRINT(' [dbo].[spSolicitudesDatosPersonales_ObtenerPendientes] ')
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
	-- Add the parameters for the stored procedure here
	@pIdUsuario			int
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
		S.[IdEstatusSolicitud] IN (1,2)	 -- 1)No Abiertas 2)En Revision
	AND 
		S.[Activa] = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	
END

GO
PRINT(' [dbo].[spSolicitudCambioDatosPersonales_ObtenerPendientes] ')
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
	-- Add the parameters for the stored procedure here
	@pTotalPendientes	int	OutPut,
	@pIdUsuario			int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
		--SET @pTotalPendientes = ISNULL((SELECT COUNT([IdEstatusSolicitud]) FROM SolicitudCambioDatosPersonales
		--						WHERE [IdEstatusSolicitud] IN(1,2) AND [Activa] = 1),0)	
		SET @pTotalPendientes = ISNULL((SELECT COUNT(S.IdSolicitud)
								FROM 
									SolicitudCambioDatosPersonales S
								INNER JOIN 
									[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
								ON 
									a.MATRICULA = s.Matricula 
								WHERE s.[IdEstatusSolicitud] IN (1,2) AND s.[Activa] = 1
								AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
								AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
								AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)),0)	
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
CREATE PROCEDURE [dbo].[spAlumnosProspCandidatos_ObtenerPorIdUsuarioMatricula]
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
		AND CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
		AND SITE_CODE IN (SELECT [ClaveSede]  FROM [dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
		AND NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)					
END
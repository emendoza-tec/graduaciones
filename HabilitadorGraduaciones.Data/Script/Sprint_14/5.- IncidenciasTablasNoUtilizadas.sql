USE [ReqGraduaciones]

PRINT('TABLES')

PRINT(' [dbo].[ProspectosGraduacionPeriodo]')
DROP TABLE ProspectosGraduacionPeriodo;

PRINT(' [dbo].[AlumnosProspectoGraduacion]')
DROP TABLE AlumnosProspectoGraduacion;


GO
PRINT('STORED PROCEDURES')
PRINT(' [dbo].[spReporte_EstimadoDeGraduacion]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Iris Yesenia Renteria Herrera
-- Create date: 20/04/2023
-- Description:	Obtener la información del prospecto para el reporte de estimado de graduación
-- =============================================
ALTER PROCEDURE [dbo].[spReporte_EstimadoDeGraduacion]
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		P.PIDM AS ID, P.MATRICULA, CONCAT(PE.NOMBRE, ' ', PE.APELLIDO_PATERNO, ' ',  PE.APELLIDO_MATERNO) AS NOMBRE,
		P.DESC_MAJR_CODE AS CARRERA,  DC.DESC_CAMPUS AS CAMPUS,	 DS.DESC_TIPO_SEDE AS SEDE,
		APG.PeriodoEstimado AS PERIODO_ESTIMADO,
		CASE WHEN APG.Id IS NULL THEN 'No' ELSE 'Si' END AS CONFIRMACION,
		APG.PeriodoElegido AS PERIODO_CONFIRMADO_G,
		CASE WHEN APG.FechaModificacion IS NULL THEN APG.FechaRegistro ELSE APG.FechaModificacion END AS FECHA_CONFIRMACION_CAMBIO,
		APG.MotivoCambioPeriodo AS MOTIVO, 
		CANTIDAD_CAMBIOS_REALIZADOS = (select count(*) from AlumnosPeriodoGraduacion where Matricula = P.Matricula),
		APG.EleccionAsistenciaCeremonia AS REGISTRO_ASISTENCIA_PI, APG.MotivoNoAsistirCeremonia AS MOTIVO_PERIODO_INTENSIVO
	FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
		LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].[DIM_R_ESCO_CAMPUS] DC ON P.CLAVE_CAMPUS  = DC.CLAVE_CAMPUS
		LEFT JOIN [Replicas].[dbo].[DIM_R_ESCO_SEDE] DS ON DC.CLAVE_SEDE_PRINCIPAL = DS.CLAVE_TIPO_SEDE
		LEFT JOIN AlumnosPeriodoGraduacion APG ON P.MATRICULA = APG.Matricula AND APG.Estatus = 1
END


GO
PRINT(' [dbo].[spUsuarios_ObtenerUsuario]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 20/10/2022
-- Description:	Consultar información del usuario al iniciar sesión
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_ObtenerUsuario]
	@Matricula varchar(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		P.PIDM AS ID, P.MATRICULA, PE.NOMBRE, PE.APELLIDO_PATERNO, PE.APELLIDO_MATERNO ,
		PE.CURP, PA.NOMBRE_PROGRAMA_ACAD_ESP AS PROGRAMAACADEMICO, P.MAJR_CODE AS CARRERAID, P.DESC_MAJR_CODE AS CARRERA,
		CONCAT(S.SPRIDEN_LAST_NAME, ' ', S.SPRIDEN_FIRST_NAME, ' ', S.SPRIDEN_MI) AS MENTOR,
		CONCAT(D.SPRIDEN_LAST_NAME, ' ', D.SPRIDEN_FIRST_NAME, ' ', D.SPRIDEN_MI) AS DIRECTOR,
		[dbo].fnR_IDEN_CORREO_ELECTRONICO_ObtenCorreo(P.PIDM) AS CORREO,
		[dbo].fnR_IDEN_CORREO_ELECTRONICO_ObtenCorreo(P.PIDM_DIRECTOR) AS CORREODIRECTOR,
		[dbo].fnR_IDEN_CORREO_ELECTRONICO_ObtenCorreo([dbo].fnPIDM_MENTORES_ObtenPrimerValor(P.PIDM_MENTORES)) AS CORREOMENTOR,
		PERIODO_AUDITORIA AS LAST_TERM_CURSADO, PA.CLAVE_PROGRAMA_ACADEMICO,
		P.NIVEL_ACADEMICO, P.CLAVE_CAMPUS,	
		G.PeriodoCeremonia AS PERIODO_GRADUACION, 
		(select [dbo].[fnPeriodo_ObtenerPeriodoActual] (P.MATRICULA)) AS PERIODO_ACTUAL,
		P.CLAVE_ESTATUS_GRADUACION 
	FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
		LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].PROGRAMA_ACADEMICO PA ON PA.CLAVE_PROGRAMA_ACADEMICO = P.CLAVE_PROGRAMA
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] S ON [dbo].fnPIDM_MENTORES_ObtenPrimerValor(P.PIDM_MENTORES) = S.SPRIDEN_PIDM
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] D ON P.PIDM_DIRECTOR = D.SPRIDEN_PIDM
		LEFT JOIN AlumnosPeriodoGraduacion G ON G.Matricula = P.MATRICULA
	WHERE P.MATRICULA = @Matricula
END
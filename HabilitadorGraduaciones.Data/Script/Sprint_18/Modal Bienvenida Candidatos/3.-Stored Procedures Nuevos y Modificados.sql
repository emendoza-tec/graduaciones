
GO
PRINT('[dbo].[spPeriodos_Insertar]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 27/04/2023
-- Description:	Insertar historico de periodo de graduacion del alumno 
-- =============================================
ALTER PROCEDURE [dbo].[spPeriodos_Insertar]
	@Matricula VARCHAR(50),
	@PeriodoElegido VARCHAR(50) = NULL,
	@PeriodoEstimado VARCHAR(50) = NULL,
	@PeriodoCeremonia VARCHAR(50) = NULL,
	@MotivoCambioPeriodo VARCHAR(255) = NULL,
	@EleccionAsistenciaCeremonia VARCHAR(20) = NULL, 
	@MotivoNoAsistirCeremonia VARCHAR(255) = NULL,
	@OrigenActualiacionPeriodoId int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IsCeremonia BIT = 0;
	DECLARE @IsRegular bit = (select RA.PeriodoRegular from  RelacionAnioTipoPeriodo R
					INNER JOIN CatAniosEjercicioAcademico A ON R.AnioId = A.Id
					INNER JOIN CatTipoPeriodo T ON R.tipoId = T.Id
					INNER JOIN CatRangosTipoPeriodo RA ON R.RangoId = RA.Id
				WHERE CONCAT(A.Descripcion, T.IndicadorPeriodo, RA.IndicadorRango) = @PeriodoElegido );

	IF EXISTS (SELECT * FROM AlumnosPeriodoGraduacion WHERE Matricula = @Matricula AND ((PeriodoElegido != @PeriodoElegido OR PeriodoElegido IS NULL)OR PeriodoEstimado != @PeriodoEstimado 
				OR PeriodoCeremonia != @PeriodoCeremonia OR MotivoCambioPeriodo != @MotivoCambioPeriodo OR EleccionAsistenciaCeremonia !=  @EleccionAsistenciaCeremonia
				OR MotivoNoAsistirCeremonia != @MotivoNoAsistirCeremonia OR OrigenActualizacionPeriodoId!= @OrigenActualiacionPeriodoId))
	BEGIN

		UPDATE AlumnosPeriodoGraduacion 
			SET Estatus = 0, FechaModificacion = GETDATE()
		WHERE Matricula = @Matricula AND Estatus = 1;

		INSERT INTO AlumnosPeriodoGraduacion (Matricula, PeriodoElegido, PeriodoEstimado, PeriodoCeremonia, MotivoCambioPeriodo,
					EleccionAsistenciaCeremonia, MotivoNoAsistirCeremonia, OrigenActualizacionPeriodoId, FechaRegistro, Estatus, PeriodoElegidoRegular )
			VALUES (@Matricula,@PeriodoElegido,@PeriodoEstimado, @PeriodoCeremonia, @MotivoCambioPeriodo, 
					@EleccionAsistenciaCeremonia, @MotivoNoAsistirCeremonia, @OrigenActualiacionPeriodoId,  GETDATE(), 1, @IsRegular);
	END
	ELSE 
	BEGIN
		IF @Matricula IS NOT NULL AND @Matricula != ''
		BEGIN
			
			IF NOT EXISTS (SELECT * FROM AlumnosPeriodoGraduacion WHERE Matricula = @Matricula AND Estatus = 1)
			BEGIN

				INSERT INTO AlumnosPeriodoGraduacion (Matricula, PeriodoElegido, PeriodoEstimado, PeriodoCeremonia, MotivoCambioPeriodo,
						EleccionAsistenciaCeremonia, MotivoNoAsistirCeremonia, OrigenActualizacionPeriodoId, FechaRegistro, Estatus, PeriodoElegidoRegular )
				VALUES (@Matricula,@PeriodoElegido,@PeriodoEstimado, @PeriodoCeremonia, @MotivoCambioPeriodo, 
						@EleccionAsistenciaCeremonia, @MotivoNoAsistirCeremonia, @OrigenActualiacionPeriodoId,  GETDATE(), 1, @IsRegular);

			END
	
		END
	END



END


Print('[dbo].[spUsuarios_ObtenerUsuario]');
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
		CASE WHEN G.PeriodoElegido IS NULL THEN G.PeriodoEstimado ELSE G.PeriodoElegido END AS PERIODO_GRADUACION, 
		(select [dbo].[fnPeriodo_ObtenerPeriodoActual] (P.MATRICULA)) AS PERIODO_ACTUAL,
		P.CLAVE_ESTATUS_GRADUACION 
	FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
		LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].PROGRAMA_ACADEMICO PA ON PA.CLAVE_PROGRAMA_ACADEMICO = P.CLAVE_PROGRAMA
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] S ON [dbo].fnPIDM_MENTORES_ObtenPrimerValor(P.PIDM_MENTORES) = S.SPRIDEN_PIDM
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] D ON P.PIDM_DIRECTOR = D.SPRIDEN_PIDM
		LEFT JOIN AlumnosPeriodoGraduacion G ON G.Matricula = P.MATRICULA AND ESTATUS = 1
	WHERE P.MATRICULA = @Matricula
END
USE [ReqGraduaciones]
GO
PRINT('[spPeriodoEstimado_GenerarPeriodoEstimadoAlumno]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 23/05/2023
-- Description:	Genera periodo estimado de graduación de alumno cuando ingresa o se modifica en replica
-- =============================================
CREATE PROCEDURE [dbo].[spPeriodoEstimado_GenerarPeriodoEstimadoAlumno](
	@Matricula VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Mensaje VARCHAR(MAX), @Result BIT;
	DECLARE @CreditosFaltantes NUMERIC(11,3), @CarreraAlumno VARCHAR(4),  @CreditosPorPeriodo INT, @PeriodosFaltantes INT, @PeriodoEstimado VARCHAR(6);
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRAN
			DECLARE @ListaPeriodos TABLE(PeriodoId VARCHAR(6), Descripcion VARCHAR(500), Regular BIT, CreditosPeriodo INT, IndicadorPeriodo INT, FecahInicio DATETIME, FechaFin DATETIME)      
				INSERT INTO @ListaPeriodos        
				EXEC [dbo].[spPeriodos_ObtenerPeriodos] @Matricula = @Matricula;
		
			SET @CreditosFaltantes = (SELECT CREDITOS_PLAN - CREDITOS_ACREDITADOS - CREDITOS_INSCRITOS FROM [Replicas].[dbo].ALUMNOS_PROSP_CANDIDATOS WHERE MATRICULA = @Matricula);
			SET @CarreraAlumno = (SELECT MAJR_CODE FROM [Replicas].[dbo].ALUMNOS_PROSP_CANDIDATOS WHERE MATRICULA = @Matricula);

			IF NOT EXISTS (SELECT Carrera FROM [ReqGraduaciones].[dbo].CatCarrerasClinicaPeriodos WHERE Carrera = @CarreraAlumno)
			BEGIN
				SET @CreditosPorPeriodo =  (SELECT CreditosRequeridos FROM [ReqGraduaciones].[dbo].CatTipoPeriodo WHERE IndicadorPeriodo = 1);
				SET @PeriodosFaltantes = ROUND(@CreditosFaltantes / @CreditosPorPeriodo, 0);

				SET @PeriodoEstimado = (SELECT TOP 1 PeriodoId FROM 
					(SELECT TOP (@PeriodosFaltantes + 1) PeriodoId FROM @ListaPeriodos WHERE Regular = 1 AND IndicadorPEriodo = 1 ORDER BY PeriodoId) AS Periodo ORDER BY PeriodoId DESC);
			
				IF @PeriodoEstimado IS NOT NULL 
				BEGIN
					IF NOT EXISTS (SELECT PeriodoElegido FROM AlumnosPeriodoGraduacion WHERE Matricula = @Matricula AND Estatus = 1)
					BEGIN 
						INSERT INTO AlumnosPeriodoGraduacion (Matricula, PeriodoEstimado, OrigenActualizacionPeriodoId, Estatus, FechaRegistro) 
						VALUES(@Matricula, @PeriodoEstimado, 5, 1, GETDATE());
					END 
					ELSE 
					BEGIN
						UPDATE AlumnosPeriodoGraduacion SET PeriodoEstimado = @PeriodoEstimado, FechaModificacion = GETDATE() WHERE Matricula = @Matricula AND Estatus = 1
					END
					
				END
				ELSE
				BEGIN
					SET @Mensaje = 'Error al obtener periodo estimado: ' + ERROR_MESSAGE();
					SET @Result = 0;
				END
			
			END

			ELSE
			BEGIN
				SET @CreditosPorPeriodo =  (SELECT CreditosRequeridos FROM [ReqGraduaciones].[dbo].CatTipoPeriodo WHERE IndicadorPeriodo = 6);
				SET @PeriodosFaltantes = ROUND(@CreditosFaltantes / @CreditosPorPeriodo, 0);

				SET @PeriodoEstimado = (SELECT TOP 1 PeriodoId FROM 
					(SELECT TOP (@PeriodosFaltantes + 1) PeriodoId FROM @ListaPeriodos WHERE Regular = 1 AND IndicadorPEriodo = 6 ORDER BY PeriodoId) AS Periodo ORDER BY PeriodoId DESC);
				
				IF @PeriodoEstimado IS NOT NULL 
				BEGIN
					IF NOT EXISTS (SELECT PeriodoElegido FROM AlumnosPeriodoGraduacion WHERE Matricula = @Matricula AND Estatus = 1)
					BEGIN 
						INSERT INTO AlumnosPeriodoGraduacion (Matricula, PeriodoEstimado, OrigenActualizacionPeriodoId, Estatus, FechaRegistro) 
						VALUES(@Matricula, @PeriodoEstimado, 5, 1, GETDATE());
					END 
					ELSE 
					BEGIN
						UPDATE AlumnosPeriodoGraduacion SET PeriodoEstimado = @PeriodoEstimado, FechaModificacion = GETDATE() WHERE Matricula = @Matricula AND Estatus = 1
					END
					
				END
				ELSE
				BEGIN
					SET @Mensaje = 'Error al obtener periodo estimado: ' + ERROR_MESSAGE();
					SET @Result = 0;
				END
			END

		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
			SET @Mensaje = 'Error al guardar periodo estimado: ' + ERROR_MESSAGE();
			SET @Result = 0;
	END CATCH
	
	SELECT @Result AS Result, @Mensaje AS Mensaje;
END




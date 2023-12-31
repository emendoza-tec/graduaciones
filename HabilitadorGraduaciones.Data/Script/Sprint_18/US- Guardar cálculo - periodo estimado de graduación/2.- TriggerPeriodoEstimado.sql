USE [Replicas]
GO
PRINT('[dbo].[trgActualizacionPeriodoEstimado]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgActualizacionPeriodoEstimado]
ON [dbo].[ALUMNOS_PROSP_CANDIDATOS]
AFTER INSERT, UPDATE
AS

IF (UPDATE(MATRICULA) OR UPDATE(CREDITOS_ACREDITADOS) OR UPDATE(CREDITOS_INSCRITOS)  OR UPDATE(CREDITOS_PLAN) )
BEGIN
			
	BEGIN TRY
		BEGIN TRAN

			INSERT INTO [ReqGraduaciones].[dbo].AlumnosPeriodoEstimado (Matricula, Estatus, FechaRegistro, Accion) 
			SELECT MATRICULA, 1, GETDATE(), 
				CASE WHEN (SELECT COUNT(Matricula) FROM [ReqGraduaciones].[dbo].AlumnosPeriodoEstimado WHERE Matricula = MATRICULA AND Estatus = 1) > 0
				THEN 'Update' ELSE 'Insert' END
			FROM INSERTED;


			DECLARE @tempData TABLE(MATRICULA VARCHAR(9));

			INSERT INTO @tempData (MATRICULA)
			SELECT MATRICULA FROM Inserted;

			DECLARE @Matricula VARCHAR(9);

			WHILE EXISTS(SELECT MATRICULA FROM @tempData)
			BEGIN
				SET @Matricula = (SELECT TOP 1 MATRICULA FROM @tempData)
	
				EXEC [ReqGraduaciones].[dbo].spPeriodoEstimado_GenerarPeriodoEstimadoAlumno @Matricula = @Matricula

				DELETE @tempData WHERE MATRICULA = @Matricula
			END
		COMMIT TRAN
    END TRY
    BEGIN CATCH
	
        DECLARE @ErrorMsg VARCHAR(MAX), @ErrorNumber INT, @ErrorProc sysname, @ErrorLine INT 

        SELECT @ErrorMsg = ERROR_MESSAGE(), @ErrorNumber = ERROR_NUMBER(), @ErrorProc = ERROR_PROCEDURE(), @ErrorLine = ERROR_LINE();
       
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
    END CATCH
END;
		

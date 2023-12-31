USE [ReqGraduaciones]
GO
PRINT('  StoredProcedure [dbo].[spSolicitudesDatosPersonales_Insertar] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Deldin		
-- Create date: 28-02-2023
-- Description:	inserta en la Tabla de SolicitudCambioDatosPersonales
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudesDatosPersonales_Insertar]
	-- Add the parameters for the stored procedure here
	@pMatricula				varchar(9),
    @pPeriodoGraduacion		varchar(6),
    @pIdDatosPersonales		int,
    @pDatoIncorrecto		varchar(250),
    @pDatoCorrecto			varchar(250),
	@OK						bit				OutPut,
	@Error					varchar(max)	OutPut,
	@pIdSolicitud			int				OutPut,
	@pNumeroSolicitud		int				OutPut,
	@pIdCorreo				int				Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRAN;
    -- Insert statements for procedure here
	BEGIN TRY  
		
		SET @pNumeroSolicitud = ISNULL((SELECT TOP 1 [NumeroSolicitud]+1  FROM [ReqGraduaciones].[dbo].[SolicitudCambioDatosPersonales] Order by [NumeroSolicitud] desc),1);
		
		INSERT INTO [dbo].[SolicitudCambioDatosPersonales]
				   ([NumeroSolicitud],[Matricula],[PeriodoGraduacion],[IdDatosPersonales],[FechaSolicitud],[UltimaActualizacion],[IdEstatusSolicitud],[DatoIncorrecto],[DatoCorrecto],[UsarioRegistro],[FechaRegistro],[UsuarioModifico],[FechaModificacion],[Activa])
		VALUES
				   (@pNumeroSolicitud, @pMatricula, @pPeriodoGraduacion,@pIdDatosPersonales,GETDATE(), GETDATE(), 1, @pDatoIncorrecto, @pDatoCorrecto, @pMatricula,  GETDATE(), Null, Null, 1)

		SET @pIdSolicitud = SCOPE_IDENTITY();

		If @pIdCorreo = 0 
			Begin
				DECLARE @vDestinatario Varchar(128);

				SET @vDestinatario = (SELECT [dbo].fnR_IDEN_CORREO_ELECTRONICO_ObtenCorreo(P.PIDM)
										FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
											WHERE P.MATRICULA = @pMatricula)

				DECLARE @vNombre Varchar(128);

				SET @vNombre = (SELECT CONCAT(PE.NOMBRE, ' ',  PE.APELLIDO_PATERNO, ' ', PE.APELLIDO_MATERNO) AS Nombre
								FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
								LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
							WHERE MATRICULA = @pMatricula)

				--Insert Email
				INSERT INTO [dbo].[CorreoSolicitudCambioDatosPersonales]
				   ([IdSolicitud]
				   ,[Destinatario]
				   ,[Comentarios]
				   ,[Enviado]
				   ,[Nombre])
				VALUES
				   (@pIdSolicitud
				   ,@vDestinatario
				   ,''
				   ,0
				   ,@vNombre)
		
				SET @pIdCorreo = SCOPE_IDENTITY();
			End

		SET @OK = 1;
		SET @Error = '';
		COMMIT TRAN;
	END TRY  
	BEGIN CATCH  
		SET @OK = 0;
		SET @Error = 'Error ' + ERROR_MESSAGE();
		SET @pIdSolicitud = 0;
		SET @pIdCorreo = 0;
		ROLLBACK TRAN;
	END CATCH  
END


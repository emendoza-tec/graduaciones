USE [ReqGraduaciones]
GO
PRINT('[dbo].[spSolicitudesDatosPersonales_Insertar] ')
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
	@pNumeroSolicitud		int				OutPut
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRAN;
    -- Insert statements for procedure here
	BEGIN TRY  
		--DECLARE @pNumeroSolicitud		int;
		SET @pNumeroSolicitud= ISNULL((SELECT [NumeroSolicitud]+1 FROM [dbo].[SolicitudCambioDatosPersonales] WHERE [IdSolicitud] = IDENT_CURRENT('[dbo].[SolicitudCambioDatosPersonales]')),1);
		--PRINT @pNumeroSolicitud
		INSERT INTO [dbo].[SolicitudCambioDatosPersonales]
				   ([NumeroSolicitud],[Matricula],[PeriodoGraduacion],[IdDatosPersonales],[FechaSolicitud],[UltimaActualizacion],[IdEstatusSolicitud],[DatoIncorrecto],[DatoCorrecto],[UsarioRegistro],[FechaRegistro],[UsuarioModifico],[FechaModificacion],[Activa])
		VALUES
				   (@pNumeroSolicitud, @pMatricula, @pPeriodoGraduacion,@pIdDatosPersonales,GETDATE(), GETDATE(), 1, @pDatoIncorrecto, @pDatoCorrecto, @pMatricula,  GETDATE(), Null, Null, 1)

		SET @pIdSolicitud = SCOPE_IDENTITY();
		SET @OK = 1;
		SET @Error = '';
		COMMIT TRAN;
	END TRY  
	BEGIN CATCH  
		SET @OK = 0;
		SET @Error = 'Error ' + ERROR_MESSAGE();
		SET @pIdSolicitud = 0;
		ROLLBACK TRAN;
	END CATCH  
END

GO
PRINT('[dbo].[spCorreoSolicitudDatosPersonales_ObtenerCorreoSolicitudDatosPersonales] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 28-02-2023
-- Description:	Obtine el correo de la Solicitud deseada 
-- =============================================
ALTER PROCEDURE [dbo].[spCorreoSolicitudDatosPersonales_ObtenerCorreoSolicitudDatosPersonales]
	-- Add the parameters for the stored procedure here
	@pIdCorreo		int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[IdCorreo]
		,[IdSolicitud]
		,[Destinatario]
		,[Comentarios]
		,[Enviado]
	FROM 
		[dbo].[CorreoSolicitudCambioDatosPersonales]
	WHERE 
		[IdCorreo] = @pIdCorreo
END


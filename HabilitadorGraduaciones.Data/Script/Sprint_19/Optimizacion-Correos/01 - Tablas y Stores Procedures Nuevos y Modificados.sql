USE [ReqGraduaciones]
GO
PRINT('  StoredProcedure [dbo].[spCorreoSolicitudDatosPersonales_ObtenerCorreoSolicitudDatosPersonales] ')
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
		,[Nombre]
	FROM 
		[dbo].[CorreoSolicitudCambioDatosPersonales]
	WHERE 
		[IdCorreo] = @pIdCorreo
END

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
		--DECLARE @pNumeroSolicitud		int;
		SET @pNumeroSolicitud= ISNULL((SELECT [NumeroSolicitud]+1 FROM [dbo].[SolicitudCambioDatosPersonales] WHERE [IdSolicitud] = IDENT_CURRENT('[dbo].[SolicitudCambioDatosPersonales]')),1);
		--PRINT @pNumeroSolicitud
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

GO
PRINT('  StoredProcedure [dbo].[spDetalleSolicitudDatosPersonales_ObtenerDatos] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin 
-- Create date: 28-02-2023
-- Description:	Selecciona los detalles de la Solicitud deseada 
-- =============================================
ALTER PROCEDURE [dbo].[spDetalleSolicitudDatosPersonales_ObtenerDatos] 
	-- Add the parameters for the stored procedure here
	@pIdSolicitud	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		S.[IdSolicitud]
		,S.[NumeroSolicitud]
		,DS.[IdDetalleSolicitud]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[DatoIncorrecto]
		,S.[DatoCorrecto]
		,S.[IdEstatusSolicitud]
		,ES.[Descripcion] AS Estatus
		,DS.[Documento]
		,DS.[Extension]
		,DS.[AzureStorage]
	FROM 
		[dbo].[SolicitudCambioDatosPersonales] S		 
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON 
		ES.[IdEstatusSolicitud] = S.[IdEstatusSolicitud] 
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	LEFT JOIN
		[dbo].[DetalleSolicitudCambioDatosPersonales] DS
	ON 
		DS.[IdSolicitud] = S.[NumeroSolicitud] 
	WHERE 
		S.[NumeroSolicitud] = @pIdSolicitud
	AND 
		S.[Activa] = 1
END

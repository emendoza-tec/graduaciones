USE [ReqGraduaciones]
GO
PRINT(' [dbo].[spDetalleSolicitudDatosPersonales_ObtenerDatos] ')
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
		,DS.[IdDetalleSolicitud]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[DatoIncorrecto]
		,S.[DatoCorrecto]
		,S.IdEstatusSolicitud
		,ES.[Descripcion] AS Estatus
		,DS.[Documento]
		,DS.[Extension]
		,DS.[AzureStorage]
	FROM 
		[dbo].[DetalleSolicitudCambioDatosPersonales] DS
	INNER JOIN
		[dbo].[SolicitudCambioDatosPersonales] S
	ON 
		DS.[IdSolicitud] = S.[NumeroSolicitud] 
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON 
		ES.[IdEstatusSolicitud] = S.[IdEstatusSolicitud] 
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	WHERE 
		DS.[IdSolicitud] = @pIdSolicitud
	AND 
		S.Activa = 1
END

USE [ReqGraduaciones]
GO
PRINT('[dbo].[spAccesoNomina_ObtenerNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 30-01-2023
-- Description: Obtiene de la Tabla Acceso Nomina y el Id de Usuario de la Tabla Usuarios por Matricula  
-- =============================================
ALTER PROCEDURE [dbo].[spAccesoNomina_ObtenerNomina] 
	@Matricula Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
      A.[Ambiente]
      ,A.[Acceso]
	  ,U.[IdUsuario]
	FROM 
		[dbo].[AccesosNomina] A
	LEFT JOIN 
		[dbo].[Usuarios] U
	ON 
		U.[NumeroNomina] = A.[Matricula]	
	WHERE 
		[Matricula] = @Matricula
END

USE [ReqGraduaciones]
GO
PRINT('[dbo].[spUsuarios_ObtenerNombrePorNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description:	Consultar nombre y correo por nomina para autompletado en formulario
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_ObtenerNombrePorNomina](
	@Nomina VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT DISTINCT TOP 5 A.CLAVE_IDENTIDAD, CONCAT(NOMBRE, ' ', APELLIDO_PATERNO, ' ', APELLIDO_MATERNO) AS NOMBRE,  B.ID_AFILIACION, CORREO_ELECTRONICO, C.IND_ESTATUS
	FROM  [Replicas].[dbo].R_IDEN_PERSONA A 
	INNER JOIN [Replicas].[dbo].R_IDEN_AFILIACION B on A.CLAVE_IDENTIDAD = B.CLAVE_IDENTIDAD
	INNER JOIN [Replicas].[dbo].R_IDEN_CORREO_ELECTRONICO C ON A.CLAVE_IDENTIDAD = C.CLAVE_IDENTIDAD AND C.FECHA_REPLICA = (SELECT MAX(FECHA_REPLICA) FROM [Replicas].[dbo].R_IDEN_CORREO_ELECTRONICO WHERE CLAVE_IDENTIDAD = A.CLAVE_IDENTIDAD )
	WHERE B.ID_AFILIACION LIKE '%' + @Nomina +'%' AND SUBSTRING(B.ID_AFILIACION, 1, 1) = 'L' AND B.ID_AFILIACION NOT IN (SELECT NumeroNomina FROM Usuarios WHERE Estatus = 1)
	
END
USE [ReqGraduaciones]
GO
Print('[dbo].[spRoles_ObtenerRolesPorNomina] ');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los roles que tiene un usuario en específico por su numero de nomina  
-- =============================================
ALTER PROCEDURE [dbo].[spRoles_ObtenerRolesPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT 
			R.[IdRol]
			,R.[Descripcion]
		FROM 
			[dbo].[Usuarios] U
		LEFT JOIN 
			[dbo].[UsuarioRoles] UR 
		ON 
			U.[IdUsuario] = UR.[IdUsuario] AND  UR.[Estatus] = 1
		LEFT JOIN 
			Roles R 
		ON 
			UR.[IdRol] = R.[IdRol] AND R.[Activo] = 1 AND R.[Estatus] = 1
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
		AND 
			U.[Estatus] = 1
END

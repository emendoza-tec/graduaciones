USE [ReqGraduaciones]
GO
PRINT('StoredProcedure [dbo].[spRoles_EliminarRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Elimina el Rol Deseado de la Tabla Roles por IdRol
-- =============================================
ALTER PROCEDURE [dbo].[spRoles_EliminarRol]
	@pIdRol				int,
	@pDescripcion		varchar(50),
    @pEstatus			bit,
    @pUsuarioModifico	varchar(9),
	@OK					bit				OutPut,
	@Error				varchar(max)	OutPut
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRAN;
    BEGIN TRY  	

		---Baja logica de Rol
		   UPDATE [dbo].[Roles]
		   SET [Activo] = 0
			  ,[UsuarioModifico] = @pUsuarioModifico
			  ,[FechaModificacion] = GETDATE()
			WHERE 
				[IdRol] = @pIdRol

			---Baja logica de Usuarios Rol 
			UPDATE [dbo].[UsuarioRoles]
			   SET 
				  [Estatus] = 0
				  ,[FechaModificacion] = GETDATE()
				  ,[UsuarioModificacion] = @pUsuarioModifico
			 WHERE 
				[IdUsuario] IN (SELECT UR.[IdUsuario]	FROM [dbo].[UsuarioRoles] UR	
														LEfT JOIN [dbo].[Usuarios] U ON UR.[IdUsuario] = U.[IdUsuario] 
														WHERE UR.[IdRol] = @pIdRol AND UR.[Estatus] = 1 AND U.[Estatus] = 1)
				AND [Estatus] = 1
				AND [IdRol] = @pIdRol

			---Inserta Movimientos de La tabla Rol
			INSERT INTO [dbo].[RolesMovimientos]
			   ([IdRol]
			   ,[Descripcion]
			   ,[Estatus]
			   ,[IdMovimiento]
			   ,[UsuarioMovimiento]
			   ,[FechaMovimiento])
			 VALUES
				   (@pIdRol
				   ,@pDescripcion
				   ,@pEstatus
				   ,2 -- Baja Logica
				   ,@pUsuarioModifico
				   ,GETDATE())

		SET @OK = 1;
		SET @Error = '';
		COMMIT TRAN;
	END TRY  
	BEGIN CATCH  
		SET @OK = 0;
		SET @Error = 'Error ' + ERROR_MESSAGE();
		ROLLBACK TRAN;
	END CATCH  
END

GO
PRINT('StoredProcedure [dbo].[spSubMenus_ObtenerMenu]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        José Antonio Mendoza Guevara
-- Create date: 03-11-2022
-- Description:    Obtiene los submenús de la tabla submenús, para cargar el menú dinámico del modo administrador
-- =============================================

ALTER PROCEDURE [dbo].[spSubMenus_ObtenerMenu]
@MenuPadre INT
AS

SET NOCOUNT ON
	SELECT ID
		,NOMBRE
		,[PATH]
		,ICONO
		,IDMENU
	FROM [dbo].[SubMenus]
	WHERE IDMENU = @MenuPadre
		AND ACTIVO = 1
		AND Seccion = 0

GO
PRINT('UserDefinedFunction [dbo].[fnRoles_ObtenerUsuariosPorRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Carlos Bernadac
-- Create date: 05/06/2023
-- Description:	Obtiene el total de Usuarios de un rol especifico 
-- =============================================
ALTER FUNCTION [dbo].[fnRoles_ObtenerUsuariosPorRol]
(
	@idRol int
)
RETURNS Int
AS
BEGIN

	DECLARE @Total	Int

	SET @Total = (SELECT COUNT(UR.[IdUsuario]) FROM
						[dbo].[UsuarioRoles] UR
					LEFT JOIN 
						[dbo].[Usuarios] U
					ON 
						UR.[IdUsuario] = U.[IdUsuario] AND UR.[Estatus] = 1
					WHERE
						U.Estatus  = 1
					AND
						UR.[IdRol] = @idRol)

	RETURN @Total

END

GO
PRINT('Cambio de Nombre de SP spDatosPersonalesEditables')
EXEC sp_rename 'spDatosPersonalesEditables', 'spDatosPersonalesEditables_ObtenerDatosPersonalesEditables';

USE [ReqGraduaciones]
GO
PRINT('[dbo].[spUsuarios_EliminarUsuario]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description: Baja lógica de usuario 
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_EliminarUsuario](
	@IdUsuario INT,
	@UsuarioElimino VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Mensaje VARCHAR(MAX), @Result BIT = 0;
	SET NOCOUNT ON;

	BEGIN TRY 
		BEGIN TRAN
		IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus = 1) 
		BEGIN
			UPDATE Usuarios SET Estatus = 0, FechaEliminacion = GETDATE(), UsuarioElimino = @UsuarioElimino
			WHERE IdUsuario = @IdUsuario;
			SET @Result = 1;
		END
		
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
			SET @Mensaje = 'Error al eliminar el registro: ' + ERROR_MESSAGE();
			SET @Result = 0;
	END CATCH
	SELECT @Result as Result, @Mensaje as Mensaje
END


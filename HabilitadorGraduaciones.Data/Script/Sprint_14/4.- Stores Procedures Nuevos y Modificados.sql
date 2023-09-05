USE [ReqGraduaciones]
GO
PRINT('STORED PROCEDURES')
PRINT(' [dbo].[spRoles_EliminarRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Elimina el Rol Deseado de la Tabla Roles por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_EliminarRol]
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
		
		   UPDATE [dbo].[Roles]
		   SET [Activo] = 0
			  ,[UsuarioModifico] = @pUsuarioModifico
			  ,[FechaModificacion] = GETDATE()
			WHERE 
				[IdRol] = @pIdRol

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
PRINT(' [dbo].[spRoles_InsertaRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Inserta registro en la tabla roles
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_InsertaRol]
	@pIdRol				int				OutPut,
	@pDescripcion		varchar(50),
    @pEstatus			bit,
    @pUsarioRegistro	varchar(9),
	@OK					bit				OutPut,
	@Error				varchar(max)	OutPut
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRAN;
    BEGIN TRY  	
		
		INSERT INTO [dbo].[Roles]
			   ([Descripcion]
			   ,[Estatus]
			   ,[UsuarioRegistro]
			   ,[FechaRegistro]
			   ,[UsuarioModifico]
			   ,[FechaModificacion]
			   ,[Activo])
		VALUES
			   (@pDescripcion
			   ,@pEstatus
			   ,@pUsarioRegistro
			   ,GETDATE()
			   ,NULL
			   ,NULL
			   ,1)
		
		SET @pIdRol = SCOPE_IDENTITY();

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
				   ,1 -- Alta
				   ,@pUsarioRegistro
				   ,GETDATE())

		SET @OK = 1;
		SET @Error = '';
		COMMIT TRAN;
	END TRY  
	BEGIN CATCH  
		SET @pIdRol = 0;
		SET @OK = 0;
		SET @Error = 'Error ' + ERROR_MESSAGE();
		ROLLBACK TRAN;
	END CATCH  

   
END
GO
PRINT(' [dbo].[spRoles_ModificaEstatusRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Modifica el Estatus del Rol Deseado de la Tabla Roles por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ModificaEstatusRol]
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
		
		   UPDATE [dbo].[Roles]
		   SET [Estatus] = @pEstatus
			  ,[UsuarioModifico] = @pUsuarioModifico
			  ,[FechaModificacion] = GETDATE()
			WHERE 
				[IdRol] = @pIdRol

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
				   ,4 -- Cambio de Estatus
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
PRINT(' [dbo].[spRoles_ModificaRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Modifica el rol deseado de la tabla roles por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ModificaRol]
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
		
		   UPDATE [dbo].[Roles]
		   SET [Descripcion] = @pDescripcion
			  ,[Estatus] = @pEstatus
			  ,[UsuarioModifico] = @pUsuarioModifico
			  ,[FechaModificacion] = GETDATE()
			WHERE 
				[IdRol] = @pIdRol
			
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
				   ,3 -- Modificacion
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
PRINT(' [dbo].[spRoles_ObtenerRoles] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Obtine Todos los Roles Activos 
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ObtenerRoles]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
		SELECT 
			  [IdRol]
			  ,[Descripcion]
			  ,[Estatus]
			  ,[dbo].fnRoles_ObtenerUsuariosPorRol([IdRol]) AS Usuarios
			  ,[dbo].fnRoles_ObtenerNombreUsuario([UsuarioRegistro]) AS UsuarioRegistro
			  ,[FechaRegistro]
			  ,[dbo].fnRoles_ObtenerNombreUsuario([UsuarioModifico]) AS UsuarioModifico
			  ,[FechaModificacion]
			  ,[Activo]
		  FROM 
				[dbo].[Roles] 
		  WHERE
			[Activo] = 1
END
GO
PRINT(' [dbo].[spRoles_ObtenerRolPorIdRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Selecciona de la Tabla Roles el Rol deseado por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ObtenerRolPorIdRol]
	@pIdRol				int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT 
			  r.[IdRol]
			  ,r.[Descripcion]
			  ,r.[Estatus]
		  FROM 
				[dbo].[Roles] r
		  WHERE
			[Activo] = 1  
		  AND 
			r.[IdRol] = @pIdRol
END
GO
PRINT(' [dbo].[spSecciones_InsertaroModificarPermisos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 25-05-2023
-- Description:	Inserta o Modifica según sea el caso los Permisos de las Sección por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spSecciones_InsertaroModificarPermisos]
	@pIdRol				int,
	@pIdMenu			int,
	@IdSubMenu			int,
    @pVer				bit,
	@pEditar			bit,
	@OK					bit				OutPut,
	@Error				varchar(max)	OutPut
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRAN;
    BEGIN TRY  	
		
		DECLARE @vRegistro int;
		SET @vRegistro = IsNull((SELECT [IdPermiso] FROM [dbo].[SeccionesPermisos] WHERE [IdRol] = @pIdRol
																				     AND [IdMenu] = @pIdMenu
																				     AND [IdSubMenu] = @IdSubMenu), 0)
		PRINT @vRegistro	
		---Si no existe el registro lo modifica, de lo contrario lo inserta 
		If @vRegistro <> 0
			BEGIN  
				UPDATE [dbo].[SeccionesPermisos]
					SET 
					    [Ver] = @pVer
						,[Editar] = @pEditar
					WHERE 
						[IdPermiso] = @vRegistro
			END  
		ELSE
			BEGIN  
				INSERT INTO [dbo].[SeccionesPermisos]
				   ([IdRol]
				   ,[IdMenu]
				   ,[IdSubMenu]
				   ,[Ver]
				   ,[Editar])
				VALUES
				   (@pIdRol
				   ,@pIdMenu
				   ,@IdSubMenu
				   ,@pVer
				   ,@pEditar)
			END  

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
PRINT(' [dbo].[spSecciones_ObtenerSecciones] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 24-05-2023
-- Description:	Selecciona todas las secciones activas
-- =============================================
CREATE PROCEDURE [dbo].[spSecciones_ObtenerSecciones] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		 SELECT 
			M.[ID] AS IdMenu
			,M.[NOMBRE] As NombreMenu
			,SM.[ID] AS IdSubMenu
			,SM.[NOMBRE] As NombreSubMenu
		FROM  
			[dbo].[Menus] M
		LEFT JOIN 
			[dbo].[SubMenus] SM
		ON
			M.ID = SM.IDMENU AND SM.ACTIVO = 1
		WHERE 
			M.ACTIVO = 1
END
GO
PRINT(' [dbo].[spSecciones_ObtenerSeccionesPorRol]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 25-05-2023
-- Description:	Selecciona de la Tabla Secciones los Permisos del rol por IdRol
-- =============================================
CREATE PROCEDURE [dbo].[spSecciones_ObtenerSeccionesPorRol]
	@pIdRol				int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 SELECT 
			sp.[IdPermiso]
			,sp.[IdRol]
			,m.[ID] AS IdMenu
			,m.[NOMBRE] As NombreMenu
			,sp.[IdSubMenu] AS IdSubMenu
			,sm.[NOMBRE] AS NombreSubMenu
            ,sp.[Ver]
			,sp.[Editar]
		FROM  
			[dbo].[SeccionesPermisos] sp
		LEFT JOIN
			[dbo].[Menus] m
		ON 
			m.[ID] = sp.IdMenu AND m.ACTIVO = 1
		LEFT JOIN
			[dbo].[SubMenus] sm
		ON
			sm.[ID] = sp.[IdSubMenu]
		WHERE 
			sp.[IdRol] = @pIdRol
END
GO
PRINT('[dbo].[spUsuarios_EliminarUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description: Baja lógica de usuario 
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_EliminarUsuario](
	@IdUsuario VARCHAR(9),
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
		IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE @IdUsuario = @IdUsuario AND Estatus = 1) 
		BEGIN
			UPDATE Usuarios SET Estatus = 0, FechaEliminacion = GETDATE(), UsuarioElimino = @UsuarioElimino
			WHERE IdUsuario = @IdUsuario;
			SET @Result = 1
		END

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
			SET @Mensaje = 'Error al eliminar el registro: ' + ERROR_MESSAGE();
			SET @Result = 0;
	END CATCH
	SELECT @Result as Result, @Mensaje as Mensaje
END

GO
PRINT(' [dbo].[spUsuarios_InsertarUsuario] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24/05/2023
-- Description:	Registrar usuario administrador e historial de cambios
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_InsertarUsuario](
	@IdUsuario INT,
	@Nomina VARCHAR(9),
	@Nombre VARCHAR(500),
	@Correo VARCHAR(250),
	@UsuarioModificacion VARCHAR(9),
	@UsuarioCampusSede UsuarioCampusSedeType READONLY,
	@UsuarioRol UsuarioRolType READONLY,
	@UsuarioNivel UsuarioNivelType READONLY)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Mensaje VARCHAR(MAX), @Result BIT, @IdReturn INT;
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRAN
		IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE NumeroNomina = @Nomina AND Estatus  = 1) --Si el usaurio existe con estatus 1 se hace update sino se registra como nuevo
		BEGIN
			DECLARE @IdHistorial INT = ISNULL((select IdHistorial FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus  = 1),0) + 1;
			UPDATE Usuarios SET FechaModificacion = GETDATE(), IdHistorial = @IdHistorial WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			--Actualizar historial de campus y sede
			UPDATE UsuarioCampusSede SET Estatus = 0, FechaModificacion = GETDATE(), UsuarioModificacion = @UsuarioModificacion
			WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			INSERT INTO UsuarioCampusSede (IdUsuario, ClaveCampus, ClaveSede, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT IdUsuario, ClaveCampus, ClaveSede, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioCampusSede;
			
			--Actualizar historial de rol
			UPDATE UsuarioRoles SET Estatus = 0, FechaModificacion = GETDATE(), UsuarioModificacion = @UsuarioModificacion
			WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			INSERT INTO UsuarioRoles (IdUsuario, IdRol, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT IdUsuario, Idrol, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioRol;

			--Actualizar historial de Nivel
			UPDATE UsuarioNivel SET Estatus = 0, FechaModificacion = GETDATE(), UsuarioModificacion = @UsuarioModificacion
			WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			INSERT INTO UsuarioNivel (IdUsuario, ClaveNivel, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT IdUsuario, ClaveNivel, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioNivel

			SET @Result = 1;

		END

		ELSE
		BEGIN
			--Insertar nuevo registro de usuario
			INSERT INTO Usuarios (NumeroNomina, Nombre, Correo, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			VALUES(@Nomina, @Nombre, @Correo, GETDATE(), @UsuarioModificacion, 1, 1 );

			DECLARE @ID INT;
			SELECT @ID = @@IDENTITY;

			INSERT INTO UsuarioCampusSede (IdUsuario, ClaveCampus, ClaveSede, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT @ID, ClaveCampus, ClaveSede, GETDATE(), @UsuarioModificacion, 1, 1 FROM @UsuarioCampusSede;

			INSERT INTO UsuarioRoles (IdUsuario, IdRol, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT @ID, Idrol, GETDATE(), @UsuarioModificacion, 1, 1 FROM @UsuarioRol;

			
			INSERT INTO UsuarioNivel (IdUsuario, ClaveNivel, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT @ID, ClaveNivel, GETDATE(), @UsuarioModificacion, 1, 1 FROM @UsuarioNivel;

			SET @Result = 1;
			SET @IdReturn = @ID;
		END
		commit;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
			SET @Mensaje = 'Error a insertar el registro: ' + ERROR_MESSAGE();
			SET @Result = 0;
	END CATCH

	SELECT @Result as Result, @Mensaje as Mensaje, @IdReturn AS IdUsuario
END

GO
PRINT('[dbo].[spUsuarios_ObtenerCampus] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description:	Consultar Campus para llenado de combo
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerCampus]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT DISTINCT CLAVE_CAMPUS AS ClaveCampus, DESC_CAMPUS AS Descripcion
	FROM [Replicas].[dbo].CAMPUS_SEDES
	GROUP BY CLAVE_CAMPUS, DESC_CAMPUS
	ORDER BY DESC_CAMPUS 

END
GO
PRINT(' [dbo].[spUsuarios_ObtenerHistorial] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description:	Consultar historial de roles, campus, sedes y nivel del usaurio 
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerHistorial](
	@IdUsuario INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT UC.IdHistorial, U.IdUsuario, U.Nombre, 
		[dbo].[fnUsuarios_ObtenerCampusUsuarioHistorial](U.IdUsuario, UC.IdHistorial) AS Campus,
		[dbo].[fnUsuarios_ObtenerRolesUsuarioHistorial](U.IdUsuario, UC.IdHistorial) AS Roles,	
		[dbo].[fnUsuarios_ObtenerSedesUsuarioHistorial](U.IdUsuario, UC.IdHistorial) AS Sedes,
		[dbo].[fnUsuarios_ObtenerNivelesUsuarioHistorial](U.IdUsuario, UC.IdHistorial) AS Niveles,
		(SELECT TOP 1 FechaRegistro FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND IdHistorial = UC.IdHistorial) AS FechaModificacion,
		CONCAT(A.NOMBRE, ' ', A.APELLIDO_PATERNO, ' ', A.APELLIDO_MATERNO) AS UsuarioModifico,
		UC.UsuarioRegistro
		FROM Usuarios U
		LEFT JOIN UsuarioCampusSede UC ON U.IdUsuario = UC.IdUsuario
		LEFT JOIN UsuarioRoles UR ON U.IdUsuario = UR.IdUsuario
		LEFT JOIN UsuarioNivel UN ON U.IdUsuario = UN.IdUsuario
		LEFT JOIN [Replicas].[dbo].R_IDEN_AFILIACION B ON B.ID_AFILIACION = UC.UsuarioRegistro 
		LEFT JOIN  [Replicas].[dbo].R_IDEN_PERSONA A ON A.CLAVE_IDENTIDAD = B.CLAVE_IDENTIDAD
		WHERE U.IdUsuario = @IdUsuario
		ORDER BY IdHistorial DESC
END

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
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerNombrePorNomina](
	@Nomina VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT TOP 5 CONCAT(NOMBRE, ' ', APELLIDO_PATERNO, ' ', APELLIDO_MATERNO) AS NOMBRE, A.CLAVE_IDENTIDAD, B.ID_AFILIACION, CORREO_ELECTRONICO
	FROM  [Replicas].[dbo].R_IDEN_PERSONA A 
	INNER JOIN [Replicas].[dbo].R_IDEN_AFILIACION B on A.CLAVE_IDENTIDAD = B.CLAVE_IDENTIDAD
	INNER JOIN [Replicas].[dbo].R_IDEN_CORREO_ELECTRONICO C ON A.CLAVE_IDENTIDAD = C.CLAVE_IDENTIDAD
	WHERE B.ID_AFILIACION LIKE '%'+@Nomina+'%' AND SUBSTRING(B.ID_AFILIACION, 1, 1) = 'L' AND B.ID_AFILIACION NOT IN (SELECT NumeroNomina FROM Usuarios WHERE Estatus = 1)

	
END
GO
PRINT(' [dbo].[spUsuarios_ObtenerSede] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description:	Consultar sedes para llenado de combo
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerSede]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT CLAVE_CAMPUS AS ClaveCampus, CLAVE_TIPO_SEDE AS ClaveSede, DESC_TIPO_SEDE AS Descripcion
	FROM [Replicas].[dbo].CAMPUS_SEDES 
	ORDER BY DESC_TIPO_SEDE

END

GO
PRINT(' [dbo].[spUsuarios_ObtenerUsuarioAdministrador] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24/05/2023
-- Description:	Consultar información de usuario administrador por matricula
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerUsuarioAdministrador](
	@IdUsuario VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus  = 1) 
	BEGIN
		
		SELECT U.IdUsuario, NumeroNomina, Nombre, Correo, U.Estatus, U.FechaModificacion
		FROM Usuarios U
		WHERE U.IdUsuario = @IdUsuario AND Estatus = 1
	

		SELECT DISTINCT UC.ClaveCampus, C.DESC_CAMPUS AS Descripcion
		FROM  UsuarioCampusSede UC 
		LEFT JOIN [Replicas].[dbo].CAMPUS_SEDES C ON UC.ClaveCampus = C.CLAVE_CAMPUS
		WHERE UC.IdUsuario = @IdUsuario AND UC.Estatus = 1;


		SELECT DISTINCT UC.ClaveSede, UC.ClaveCampus, S.DESC_TIPO_SEDE AS Descripcion
		FROM  UsuarioCampusSede UC 
		LEFT JOIN [Replicas].[dbo].CAMPUS_SEDES S ON UC.ClaveSede = S.CLAVE_TIPO_SEDE
		WHERE UC.IdUsuario = @IdUsuario  AND UC.Estatus = 1;

	
		SELECT DISTINCT UN.ClaveNivel , N.DESC_NIVEL_ACADEMICO AS Descripcion
		FROM Usuarios U
		LEFT JOIN UsuarioNivel UN ON U.IdUsuario = UN.IdUsuario AND UN.Estatus = 1
		LEFT JOIN [Replicas].[dbo].DIM_R_ESCO_NIVEL_ACADEMICO N ON UN.ClaveNivel = N.CLAVE_NIVEL_ACADEMICO
		WHERE U.IdUsuario = @IdUsuario;
	
		SELECT UR.IdRol, R.Descripcion
		FROM Usuarios U
		LEFT JOIN UsuarioRoles UR ON U.IdUsuario = UR.IdUsuario AND UR.Estatus = 1
		LEFT JOIN Roles R ON UR.IdRol = R.IdRol
		WHERE U.IdUsuario = @IdUsuario;
	END 


END


GO
PRINT(' [dbo].[spUsuarios_ObtenerUsuarios] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 23/05/2023
-- Description:	Consultar usuarios administradores actuales
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerUsuarios]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT DISTINCT U.IdUsuario, NumeroNomina, Nombre, Correo, U.Estatus, U.FechaRegistro,
	[dbo].[fnUsuarios_ObtenerRolesUsuario](U.IdUsuario) AS Roles,
	[dbo].[fnUsuarios_ObtenerCampusUsuario](U.IdUsuario) AS Campus,
	(select count(Id) from UsuarioRoles WHERE IdUsuario = U.IdUsuario AND Estatus = 1) as CantidadRoles
	FROM Usuarios U
	LEFT JOIN UsuarioRoles UR ON U.IdUsuario = UR.IdUsuario AND UR.Estatus = 1
	LEFT JOIN UsuarioCampusSede UC ON U.IdUsuario = UC.IdUsuario AND UC.Estatus = 1
	LEFT JOIN UsuarioNivel UN ON U.IdUsuario = UN.IdUsuario AND UN.Estatus = 1
	WHERE U.Estatus = 1
END

GO

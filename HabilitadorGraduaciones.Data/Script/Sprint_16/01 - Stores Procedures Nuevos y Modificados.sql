USE [ReqGraduaciones]
GO
PRINT('StoredProcedure [dbo].[spAccesoNomina_ObtenerNomina] ')
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
	INNER JOIN 
		[dbo].[Usuarios] U
	ON 
		U.[NumeroNomina] = A.[Matricula]	
	WHERE 
		[Matricula] = @Matricula
END

GO
PRINT(' StoredProcedure [dbo].[spSolicitudCambioDatosPersonales_ActualizarEstatus]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Deldin		
-- Create date: 28-02-2023
-- Description:	Modifica en la Tabla de SolicitudCambioDatosPersonales y Crea Correo para Avisar al Alumno del cambio de Estatus de la solicitud
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudCambioDatosPersonales_ActualizarEstatus]
	-- Add the parameters for the stored procedure here
		@pIdSolicitud			int,
		@pMatricula				varchar(9),
		@pIdEstatusSolicitud	int,
		@pUsarioRegistro		varchar(9),
		@pComentarios			varchar(250),
		@OK						bit				OutPut,
		@Error					varchar(max)	OutPut,
		@pIdCorreo				int				OutPut
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRAN;
    BEGIN TRY  

		--Update
		UPDATE 
			[dbo].[SolicitudCambioDatosPersonales]
		SET 
			[UsuarioModifico] = @pUsarioRegistro
			,[FechaModificacion] = GETDATE()
			,[Activa] = 0
		WHERE
			[IdSolicitud] = @pIdSolicitud		

		--Insert Solicitud para Historial 
		INSERT INTO [dbo].[SolicitudCambioDatosPersonales]
			([NumeroSolicitud]
			,[Matricula]
			,[PeriodoGraduacion]
			,[IdDatosPersonales]
			,[FechaSolicitud]
			,[UltimaActualizacion]
			,[IdEstatusSolicitud]
			,[DatoIncorrecto]
			,[DatoCorrecto]
			,[UsarioRegistro]
			,[FechaRegistro]
			,[UsuarioModifico]
			,[FechaModificacion]
			,[Activa])
		SELECT 
			[NumeroSolicitud]
			,[Matricula]
			,[PeriodoGraduacion]
			,[IdDatosPersonales]
			,[FechaSolicitud]
			,GETDATE()
			,@pIdEstatusSolicitud
			,[DatoIncorrecto]
			,[DatoCorrecto]
			,@pUsarioRegistro
			,GETDATE()
			,Null
			,Null
			,1
		 FROM
			[dbo].[SolicitudCambioDatosPersonales]
		WHERE
			IdSolicitud = @pIdSolicitud
					
		SET @pIdSolicitud = SCOPE_IDENTITY();

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
           ,@pComentarios
           ,0
		   ,@vNombre)
		
		SET @pIdCorreo = SCOPE_IDENTITY();
		
		SET @OK = 1;
		SET @Error = '';
		COMMIT TRAN;
	END TRY  
	BEGIN CATCH  
		SET @OK = 0;
		SET @Error = 'Error ' + ERROR_MESSAGE();
		SET @pIdCorreo = 0;
		SET @pIdSolicitud = 0;
		ROLLBACK TRAN;
	END CATCH  
END

GO
PRINT(' StoredProcedure [dbo].[spSolicitudCambioDatosPersonales_ObtenerPendientes] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 06-03-2023
-- Description:	Cuenta las solicitudes Pendientes (No Abiertas y En Revision)
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudCambioDatosPersonales_ObtenerPendientes]
	-- Add the parameters for the stored procedure here
	@pTotalPendientes	int	OutPut,
	@pIdUsuario			int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SET @pTotalPendientes = ISNULL((SELECT COUNT(S.IdSolicitud)
								FROM 
									SolicitudCambioDatosPersonales S
								INNER JOIN 
									[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
								ON 
									a.MATRICULA = s.Matricula 
								WHERE s.[IdEstatusSolicitud] IN (1,2) AND s.[Activa] = 1
								AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
								AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
								AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)),0)	
END

GO
PRINT(' StoredProcedure [dbo].[spSolicitudesDatosPersonales_Insertar]')
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

		SET @pNumeroSolicitud= (SELECT [NumeroSolicitud]+1 FROM [dbo].[SolicitudCambioDatosPersonales] WHERE [IdSolicitud] = IDENT_CURRENT('[dbo].[SolicitudCambioDatosPersonales]'));

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
PRINT('  StoredProcedure [dbo].[spSolicitudesDatosPersonales_Obtener]  ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 28-02-2023
-- Description:	Selecciona las solicitudes segun su estatus de solicitud 
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudesDatosPersonales_Obtener]
	-- Add the parameters for the stored procedure here
	@pIdEstatusSolicitud		int,
	@pIdUsuario					int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		S.[IdSolicitud]
		,S.[NumeroSolicitud]
		,S.[Matricula]
		,S.[PeriodoGraduacion]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[UltimaActualizacion]
		,S.[IdEstatusSolicitud]
		,ES.[Descripcion] AS Estatus
	FROM
		[dbo].[SolicitudCambioDatosPersonales] S
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON
	 S.[IdEstatusSolicitud] = ES.[IdEstatusSolicitud]
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = S.Matricula 
	WHERE 
		S.[IdEstatusSolicitud] = @pIdEstatusSolicitud
	AND 
		S.[Activa] = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	

END

GO
PRINT('  StoredProcedure [dbo].[spSolicitudesDatosPersonales_ObtenerPendientes] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin	
-- Create date: 06-03-2023
-- Description:	Selecciona las solicitudes Pendientes (No Abiertas y En Revision)
-- =============================================
ALTER PROCEDURE [dbo].[spSolicitudesDatosPersonales_ObtenerPendientes]
	-- Add the parameters for the stored procedure here
	@pIdUsuario			int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		S.[IdSolicitud]
		,S.[NumeroSolicitud]
		,S.[Matricula]
		,S.[PeriodoGraduacion]
		,S.[IdDatosPersonales]
		,D.[Descripcion]
		,S.[FechaSolicitud]
		,S.[UltimaActualizacion]
		,S.[IdEstatusSolicitud]
		,ES.[Descripcion] AS Estatus
	FROM
		[dbo].[SolicitudCambioDatosPersonales] S
	INNER JOIN
		[dbo].[EstatusSolicitudDatosPersonales] ES
	ON
	 S.[IdEstatusSolicitud] = ES.[IdEstatusSolicitud]
	INNER JOIN
		[dbo].[DatosPersonalesEditables] D
	ON
		D.[IdDatosPersonales] = S.[IdDatosPersonales]
	INNER JOIN 
		[Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] A
	ON 
		A.MATRICULA = S.Matricula 
	WHERE 
		S.[IdEstatusSolicitud] IN (1,2)	 -- 1)No Abiertas 2)En Revision
	AND 
		S.[Activa] = 1
	AND A.CLAVE_CAMPUS IN (SELECT [ClaveCampus]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.SITE_CODE IN (SELECT [ClaveSede]  FROM [ReqGraduaciones].[dbo].[UsuarioCampusSede] where [IdUsuario] = @pIdUsuario)
	AND A.NIVEL_ACADEMICO IN (SELECT [ClaveNivel]  FROM [ReqGraduaciones].[dbo].[UsuarioNivel] where [IdUsuario] = @pIdUsuario)	
END

GO
PRINT('[dbo].[spUsuarios_InsertarUsuario]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24/05/2023
-- Description:	Registrar usuario administrador e historial de cambios
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_InsertarUsuario](
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
	
	DECLARE @Mensaje VARCHAR(MAX), @Result BIT, @IdReturn INT, @IdUsuarioModificacion INT;
	SET NOCOUNT ON;
	
	IF @Nombre IS NULL OR @Nombre = ''
	BEGIN
		DECLARE @resultNombre table(CLAVE_IDENTIDAD INT, Nombre VARCHAR(500), ID_AFILIACION VARCHAR(9), CORREO_ELECTRONICO VARCHAR(500), IND_ESTATUS VARCHAR(10) )
		INSERT INTO @resultNombre(CLAVE_IDENTIDAD, Nombre, ID_AFILIACION, CORREO_ELECTRONICO, IND_ESTATUS) exec spUsuarios_ObtenerNombrePorNomina @Nomina
	
		SET @Nombre = (select Nombre FROM @resultNombre);
	END
	
	SET @IdUsuarioModificacion = (SELECT TOP 1 IdUsuario FROM Usuarios WHERE NumeroNomina = @UsuarioModificacion AND Estatus = 1)

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
			SELECT @IdUsuario, ClaveCampus, ClaveSede, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioCampusSede;
			
			--Actualizar historial de rol
			UPDATE UsuarioRoles SET Estatus = 0, FechaModificacion = GETDATE(), UsuarioModificacion = @UsuarioModificacion
			WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			INSERT INTO UsuarioRoles (IdUsuario, IdRol, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT @IdUsuario, Idrol, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioRol;

			--Actualizar historial de Nivel
			UPDATE UsuarioNivel SET Estatus = 0, FechaModificacion = GETDATE(), UsuarioModificacion = @UsuarioModificacion
			WHERE IdUsuario = @IdUsuario AND Estatus  = 1;

			INSERT INTO UsuarioNivel (IdUsuario, ClaveNivel, FechaRegistro, UsuarioRegistro, Estatus, IdHistorial)
			SELECT @IdUsuario, ClaveNivel, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioNivel;


			INSERT INTO UsuarioMovimientos(UsuarioCreado, UsuarioId, Nivel, RolAsignado, CampusAsignados, SedesAsignadas, UsuarioModificaId, FechaModificacion, MovimientoId)
			SELECT @Nomina, @IdUsuario, dbo.fnUsuarios_ObtenerClaveNivelesHistorial(@IdUsuario, @IdHistorial),
					dbo.fnUsuarios_ObtenerIdRolesHistorial(@IdUsuario, @IdHistorial),
					dbo.fnUsuarios_ObtenerClaveCampusHistorial(@IdUsuario, @IdHistorial), 
					dbo.fnUsuarios_ObtenerClaveSedesHistorial(@IdUsuario, @IdHistorial), 
					@IdUsuarioModificacion, GETDATE(), 3;

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

			INSERT INTO UsuarioMovimientos(UsuarioCreado, UsuarioId, Nivel, RolAsignado, CampusAsignados, SedesAsignadas, UsuarioRegistraId, FechaRegistro, MovimientoId)
			SELECT @Nomina, @ID, dbo.fnUsuarios_ObtenerClaveNivelesHistorial(@ID, 1),
					dbo.fnUsuarios_ObtenerIdRolesHistorial(@ID, 1),
					dbo.fnUsuarios_ObtenerClaveCampusHistorial(@ID, 1), 
					dbo.fnUsuarios_ObtenerClaveSedesHistorial(@ID, 1), 
					@IdUsuarioModificacion, GETDATE(), 1;

			SET @Result = 1;
			SET @IdReturn = @ID;
		END
		
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
			SET @Mensaje = 'Error a insertar el registro: ' + ERROR_MESSAGE();
			SET @Result = 0;
	END CATCH

	SELECT @Result as Result, @Mensaje as Mensaje, @IdReturn AS IdUsuario
END



GO
PRINT('[dbo].[spUsuarios_ObtenerUsuarioAdministrador]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24/05/2023
-- Description:	Consultar información completa de usuario (Campus, sedes, niveles, roles) administrador por matricula
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_ObtenerUsuarioAdministrador](
	@IdUsuario INT
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

PRINT('[dbo].[spUsuarios_ObtenerUsuarioAdministradorPorNomina]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 24/05/2023
-- Description:	Consultar información de usuario administrador por matricula
-- =============================================
CREATE PROCEDURE [dbo].[spUsuarios_ObtenerUsuarioAdministradorPorNomina](
	@Nomina VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE NumeroNomina = @Nomina AND Estatus  = 1) 
	BEGIN
		
		SELECT U.IdUsuario, NumeroNomina, Nombre, Correo, U.Estatus, U.FechaModificacion
		FROM Usuarios U
		WHERE U.NumeroNomina = @Nomina AND Estatus = 1
	END 
END

PRINT('[dbo].[spAvisos_ObtenerAvisos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Allan Mendoza
-- Create date: 20/10/2022
-- Description: Obtiene los avisos de una matrícula específica en la tabla "Avisos" segun las caracteristicas del 
--				alumno de esa matricula y despues checa si en "Avisos_Usuarios" se le ha enviado avisos a ese alumno 
--				en espesifico y se combinan para obtener los avisos a mostrar
-- =============================================
ALTER procedure [dbo].[spAvisos_ObtenerAvisos]
  @matricula VARCHAR(255)
  AS
  BEGIN

  SET NOCOUNT ON

	  DECLARE @vcampus VARCHAR(3)
      DECLARE @vnivel VARCHAR(2)
      DECLARE @vsede VARCHAR(3)
      DECLARE @vprograma VARCHAR(12)
      DECLARE @vescuela VARCHAR(2)

      --Obtener los datos de una sola matricula
      SELECT @vcampus = CLAVE_CAMPUS,
             @vnivel = NIVEL_ACADEMICO,
             @vsede = SITE_CODE,
             @vprograma = CLAVE_PROGRAMA,
             @vescuela = COLL_CODE
      FROM   Replicas.dbo.ALUMNOS_PROSP_CANDIDATOS
      WHERE  MATRICULA = @matricula;

      SELECT DISTINCT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		Habilitador, Nivel, Cumple, Requisito
      FROM   (SELECT DISTINCT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
				SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
				Habilitador, Nivel, Cumple, Requisito
              FROM   (SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
						SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
						Habilitador, Nivel, Cumple, Requisito
                      FROM   Avisos a
                      WHERE @vcampus = IIF(CampusId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveCampus FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveCampus = @vcampus), CampusId)
							AND IIF(Nivel != '', Nivel, @vnivel) = @vnivel
							AND @vsede = IIF(SedeId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveSede = @vsede), CampusId)
							AND IIF(ProgramaId != '', ProgramaId, @vprograma) = @vprograma
							AND IIF(EscuelasId != '', EscuelasId, @vescuela) = @vescuela
							AND a.Habilitador = 1
							AND a.AvisoId NOT IN (SELECT AvisoId FROM Avisos_Usuarios WHERE Activo = 1)
                      UNION ALL
                      --Obtengo Avisos de AvisosUsuarios que es solamente por matricula 
                      SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
						SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
						Habilitador, Nivel, Cumple, Requisito
                      FROM   dbo.Avisos a
                      WHERE  a.AvisoId IN (SELECT AvisoId FROM Avisos_Usuarios WHERE Matricula = @matricula AND activo = 1) AND a.Habilitador = 1) x
              UNION ALL
              SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
					SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
					Habilitador, Nivel, Cumple, Requisito
              FROM   Avisos
              WHERE  CcCampusId = @vcampus
                     AND Habilitador = 1) y
      ORDER  BY FechaCreacion DESC
  END

PRINT('[dbo].[spAvisos_Obtener3Avisos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Allan Mendoza
-- Create date: 20/10/2022
-- Description: Obtiene los últimos 3 avisos de una matrícula específica en la tabla "Avisos" segun las caracteristicas del alumno 
--              de esa matricula y despues checa si en "Avisos_Usuarios" se le ha enviado avisos a ese alumno en espesifico y se 
--	            combinan para obtener los 3 ultimos a mostrar
-- =============================================
ALTER PROCEDURE [dbo].[spAvisos_Obtener3Avisos]
  @matricula VARCHAR(255)
  AS
  BEGIN
	SET NOCOUNT ON
	
	DECLARE @vcampus VARCHAR(3)
	DECLARE @vnivel VARCHAR(2)
	DECLARE @vsede VARCHAR(3)
	DECLARE @vprograma VARCHAR(12)
	DECLARE @vescuela VARCHAR(2)

  
  --Obtener los datos de una sola matricula
   SELECT 
	 @vcampus=clave_campus
	,@vnivel= NIVEL_ACADEMICO
	,@vsede=SITE_CODE 
	,@vprograma=CLAVE_PROGRAMA
	,@vescuela=COLL_CODE
  FROM Replicas.dbo.ALUMNOS_PROSP_CANDIDATOS 
  WHERE matricula=@matricula;

  SELECT DISTINCT TOP 3 AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		Habilitador, Nivel, Cumple, Requisito
	FROM
	(
		SELECT DISTINCT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		    SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		    Habilitador, Nivel, Cumple, Requisito
		FROM
		(
			SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		        SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		        Habilitador, Nivel, Cumple, Requisito
			FROM Avisos a
			WHERE @vcampus = IIF(CampusId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveCampus FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveCampus = @vcampus), CampusId)
				AND IIF(Nivel != '', Nivel, @vnivel) = @vnivel
				AND @vsede = IIF(SedeId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveSede = @vsede), CampusId)
				AND IIF(ProgramaId != '', ProgramaId, @vprograma) = @vprograma
				AND IIF(EscuelasId != '', EscuelasId, @vescuela) = @vescuela
				AND a.Habilitador = 1
				AND a.AvisoId NOT IN (SELECT AvisoId FROM Avisos_Usuarios WHERE Activo = 1)
			UNION ALL
			--Obtengo Avisos de AvisosUsuarios que es solamente por matricula 
			SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		        SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		        Habilitador, Nivel, Cumple, Requisito
			FROM dbo.Avisos a
			WHERE a.AvisoId IN (
							  SELECT AvisoId
							  FROM Avisos_Usuarios
							  WHERE Matricula = @matricula
									and Activo = 1
						  )
				  AND a.Habilitador = 1
		) x
		UNION ALL
		SELECT AvisoId, Titulo, Texto, UrlImagen, FechaCreacion, CampusId, 
		    SedeId, RequisitoId, ProgramaId, EscuelasId,CcRolesId, CcCampusId, Correo, 
		    Habilitador, Nivel, Cumple, Requisito
		FROM Avisos
		WHERE CcCampusId = @vcampus
			  AND Habilitador = 1
	) y
	ORDER BY FechaCreacion DESC
  END

PRINT('[dbo].[spAvisos_InsertarAviso]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        Allan Mendoza
-- Create date: 20/10/2022
-- Description:    Inserta un aviso en la tabla "Avisos", tambien en "Avisos_Usuarios" en caso de que sea a un alumno en espesifico y a "Correos_Enviados" cuando debe ser enviado por correo el aviso
-- =============================================
ALTER procedure [dbo].[spAvisos_InsertarAviso]
@titulo varchar(255),
@texto varchar(max),
@urlImagen varchar(max),
@fechaCreacion datetime,
@matricula varchar(max),
@campusId varchar(3),
@sedeId varchar(3),
@requisitoId varchar(5),
@programaId varchar(12),
@escuelasId varchar(2),
@cc_rolesId varchar(5),
@cc_camposId varchar(3),
@nivel varchar(2),
@correo bit,
@habilitador bit,
@cumple varchar(2),
@IdUsuario int,
@id int out
as
DECLARE @LASTID int
begin
INSERT INTO [dbo].[Avisos]
           ([Titulo]
           ,[Texto]
           ,[UrlImagen]
           ,[FechaCreacion]
           ,[CampusId]
           ,[SedeId]
           ,[RequisitoId]
           ,[ProgramaId]
           ,[EscuelasId]
           ,[CcRolesId]
           ,[CcCampusId]
           ,[Correo]
           ,[Habilitador]
		   ,[Nivel]
		   ,[IdUsuario])
     VALUES
           (@titulo
           ,@texto
           ,@urlImagen
           ,@fechaCreacion
           ,@campusId
           ,@sedeId
           ,@requisitoId
           ,@programaId
           ,@escuelasId
           ,@cc_rolesId
           ,@cc_camposId
           ,@correo
           ,@habilitador
		   ,@nivel
		   ,@IdUsuario)

	SET @LASTID = SCOPE_IDENTITY()
	IF @matricula != ''
    begin
	insert into [Avisos_Usuarios](Matricula,[AvisoId],[Activo])
		select value,@LASTID,1 from string_split(@matricula,',')
	end

	--Insertar los Ext
	IF @requisitoId != ''
	BEGIN
		UPDATE Avisos
		SET 
		Requisito = @requisitoId
		,Cumple = @cumple
		WHERE AvisoId = @LASTID
	END;

	--Insertar en CorreosEnviados
	if @correo = 1
	begin
		if @matricula != ''
		begin
			insert into Correos_Enviados(AvisoId, Correo, Enviado, Nombre)
			(
			select 
			@LASTID
			,c.CORREO_ELECTRONICO		--
			,0
			,p.nombre		--
			FROM replicas.dbo.[R_IDEN_PERSONA] p
			join replicas.dbo.[R_IDEN_CORREO_ELECTRONICO] c
			on p.[CLAVE_IDENTIDAD] = c.[CLAVE_IDENTIDAD]
			join replicas.dbo.[R_IDEN_AFILIACION] a
			on p.[CLAVE_IDENTIDAD] = a.[CLAVE_IDENTIDAD]
			where  
				a.[ID_AFILIACION] in (select value from string_split(@matricula,','))
	)
		end
		else
		begin

			DECLARE @CampusSedesPermiso dbo.UsuarioCampusSedeType;
			IF @campusId = '' OR @sedeId = ''
			BEGIN
				INSERT INTO @CampusSedesPermiso Select IdUsuario, ClaveCampus, ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND Estatus = 1 AND IIF(@campusId != '', @campusId, ClaveCampus) = ClaveCampus
			END
			ELSE
			BEGIN
				INSERT INTO @CampusSedesPermiso(IdUsuario, ClaveCampus,ClaveSede) Select @IdUsuario, @campusId, @sedeId
			END

			IF @cc_camposId != ''
			BEGIN
				 IF (SELECT COUNT(IdUsuario) FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND Estatus = 1 AND ClaveCampus = @cc_camposId) > 0
				 BEGIN
					INSERT INTO @CampusSedesPermiso(IdUsuario, ClaveCampus, ClaveSede) Select @IdUsuario, ClaveCampus, ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND Estatus = 1 AND ClaveCampus = @cc_camposId
				END
			END

			insert into Correos_Enviados(AvisoId, Correo, Enviado, Nombre)
			(
			select distinct
			@LASTID
			,c.CORREO_ELECTRONICO		--
			,0
			,p.nombre		--
			FROM replicas.dbo.[R_IDEN_PERSONA] p
			join replicas.dbo.[R_IDEN_CORREO_ELECTRONICO] c
			on p.[CLAVE_IDENTIDAD] = c.[CLAVE_IDENTIDAD]
			join replicas.dbo.[R_IDEN_AFILIACION] a
			on p.[CLAVE_IDENTIDAD] = a.[CLAVE_IDENTIDAD]
			where  
				a.[ID_AFILIACION] in(
				select Matricula
					from replicas.[dbo].[ALUMNOS_PROSP_CANDIDATOS] a
					where 
						a.CLAVE_CAMPUS IN (Select ClaveCampus FROM @CampusSedesPermiso)
					and a.site_code IN (Select ClaveSede FROM @CampusSedesPermiso)
					and IIF(@programaId!='',@programaId,a.clave_programa)=a.clave_programa
					and IIF(@escuelasId!='',@escuelasId,a.coll_code)=a.coll_code
					and IIF(@nivel != '', @nivel, a.NIVEL_ACADEMICO) = a.NIVEL_ACADEMICO
					/*
					Falta añadir aqui el filtro de @requisitoId, aun no nos entregan el origen de estos
					*/
				))

		end
	end
	set @id=@LASTID;
	return @id;
end
;

PRINT('[dbo].[spAvisos_ObtenerCatalogoMatriculas]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernández
-- Create date: 20/10/2022
-- Description:	Consultar el catálogo de matrículas para la vista de avisos de administrador en base a los demas catalogos
-- =============================================
ALTER PROCEDURE [dbo].[spAvisos_ObtenerCatalogoMatriculas]
	@Nivel varchar(2),
	@Campus varchar(3),
	@Sedes varchar(3),
	@Escuela varchar(2),
	@Programa varchar(12),
	@IdUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CampusSedesPermiso dbo.UsuarioCampusSedeType

	IF @Campus = '' OR @Sedes = ''
	BEGIN
		INSERT INTO @CampusSedesPermiso Select IdUsuario, ClaveCampus, ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND Estatus = 1 AND IIF(@Campus != '', @Campus, ClaveCampus) = ClaveCampus
	END
	ELSE
	BEGIN
		INSERT INTO @CampusSedesPermiso(IdUsuario, ClaveCampus,ClaveSede) Select @IdUsuario, @Campus, @Sedes
	END
	
	SELECT DISTINCT 
		MATRICULA AS CLAVE, MATRICULA AS DESCRIPCION
	FROM [Replicas].dbo.ALUMNOS_PROSP_CANDIDATOS 
	WHERE	IIF(@Nivel !='', @Nivel, NIVEL_ACADEMICO) = NIVEL_ACADEMICO
		AND CLAVE_CAMPUS IN (Select ClaveCampus FROM @CampusSedesPermiso)
		AND SITE_CODE IN (Select ClaveSede FROM @CampusSedesPermiso)
		AND IIF(@Escuela !='', @Escuela, COLL_CODE) = COLL_CODE
		AND IIF(@Programa !='', @Programa, CLAVE_PROGRAMA) = CLAVE_PROGRAMA
	ORDER BY MATRICULA	
END

GO
PRINT('[dbo].[spReporte_EstimadoDeGraduacion]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Iris Yesenia Renteria Herrera
-- Create date: 20/04/2023
-- Description:	Obtener la información del prospecto para el reporte de estimado de graduación
-- =============================================
ALTER PROCEDURE [dbo].[spReporte_EstimadoDeGraduacion](
	@UsuarioCampusSede UsuarioCampusSedeType READONLY,
	@UsuarioNivel UsuarioNivelType READONLY)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		P.PIDM AS ID, P.MATRICULA, CONCAT(PE.NOMBRE, ' ', PE.APELLIDO_PATERNO, ' ',  PE.APELLIDO_MATERNO) AS NOMBRE,
		P.DESC_MAJR_CODE AS CARRERA,  DC.DESC_CAMPUS AS CAMPUS,	 DS.DESC_TIPO_SEDE AS SEDE,
		APG.PeriodoEstimado AS PERIODO_ESTIMADO,
		CASE WHEN APG.Id IS NULL THEN 'No' ELSE 'Si' END AS CONFIRMACION,
		APG.PeriodoElegido AS PERIODO_CONFIRMADO_G,
		CASE WHEN APG.FechaModificacion IS NULL THEN APG.FechaRegistro ELSE APG.FechaModificacion END AS FECHA_CONFIRMACION_CAMBIO,
		APG.MotivoCambioPeriodo AS MOTIVO, 
		CANTIDAD_CAMBIOS_REALIZADOS = (select count(*) from AlumnosPeriodoGraduacion where Matricula = P.Matricula),
		APG.EleccionAsistenciaCeremonia AS REGISTRO_ASISTENCIA_PI, APG.MotivoNoAsistirCeremonia AS MOTIVO_PERIODO_INTENSIVO
	FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
		LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].[CAMPUS_SEDES] DC ON P.CLAVE_CAMPUS  = DC.CLAVE_CAMPUS
		LEFT JOIN [Replicas].[dbo].[CAMPUS_SEDES] DS ON DC.CLAVE_TIPO_SEDE = DS.CLAVE_TIPO_SEDE
		LEFT JOIN AlumnosPeriodoGraduacion APG ON P.MATRICULA = APG.Matricula AND APG.Estatus = 1
	WHERE 
		((SELECT COUNT(ClaveCampus) FROM @UsuarioCampusSede) = 0 OR P.CLAVE_CAMPUS  IN (SELECT DISTINCT ClaveCampus FROM @UsuarioCampusSede))
		AND ((SELECT COUNT(ClaveSede) FROM @UsuarioCampusSede) = 0 OR  DC.CLAVE_TIPO_SEDE IN (SELECT ClaveSede FROM @UsuarioCampusSede))
		AND ((SELECT COUNT(ClaveNivel) FROM @UsuarioNivel) = 0 OR  P.NIVEL_ACADEMICO IN (SELECT ClaveNivel FROM @UsuarioNivel))
END




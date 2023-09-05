USE [ReqGraduaciones]
GO
PRINT('  StoredProcedure [dbo].[spEstatusSolicitudDatosPersonales] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 28-02-2023
-- Description:	Seleciona los Estatus de Solicitud de Modificacion menos el No Abierto
-- =============================================
ALTER PROCEDURE [dbo].[spEstatusSolicitudDatosPersonales_ObtenerEstatus]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[IdEstatusSolicitud]
		,[Descripcion]
	FROM 
		[dbo].[EstatusSolicitudDatosPersonales]
	WHERE 
		[IdEstatusSolicitud] IN(2, 3, 4)	
END

GO
PRINT('[dbo].[spUsuarios_InsertarUsuario]')
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
			SELECT IdUsuario, ClaveNivel, GETDATE(), @UsuarioModificacion, 1, @IdHistorial FROM @UsuarioNivel;


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
PRINT('[dbo].[spUsuarios_EliminarUsuario]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description: Baja logica de usuario 
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_EliminarUsuario](
	@IdUsuario INT,
	@UsuarioElimino VARCHAR(9)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	DECLARE @Mensaje VARCHAR(MAX), @Result BIT = 0, @IdUsuarioModificacion INT, @IdHistorialActual INT, @Nomina VARCHAR(9);
	SET NOCOUNT ON;
	
	SET @IdUsuarioModificacion = (SELECT TOP 1 IdUsuario FROM Usuarios WHERE NumeroNomina = @UsuarioElimino AND Estatus = 1)
	SET @IdHistorialActual = (SELECT TOP 1 IdHistorial FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus = 1)
	SET @Nomina = (SELECT TOP 1 NumeroNomina FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus = 1)

	BEGIN TRY 
		BEGIN TRAN
		IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE IdUsuario = @IdUsuario AND Estatus = 1) 
		BEGIN
			UPDATE Usuarios SET Estatus = 0, FechaEliminacion = GETDATE(), UsuarioElimino = @UsuarioElimino
			WHERE IdUsuario = @IdUsuario;

			INSERT INTO UsuarioMovimientos(UsuarioCreado, UsuarioId, Nivel, RolAsignado, CampusAsignados, SedesAsignadas, UsuarioModificaId, FechaModificacion, MovimientoId)
			SELECT @Nomina, @IdUsuario, dbo.fnUsuarios_ObtenerClaveNivelesHistorial(@IdUsuario, @IdHistorialActual),
					dbo.fnUsuarios_ObtenerIdRolesHistorial(@IdUsuario, @IdHistorialActual),
					dbo.fnUsuarios_ObtenerClaveCampusHistorial(@IdUsuario, @IdHistorialActual), 
					dbo.fnUsuarios_ObtenerClaveSedesHistorial(@IdUsuario, @IdHistorialActual), 
					@IdUsuarioModificacion, GETDATE(), 2;

			SET @Result = 1
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

GO
GO
PRINT('[dbo].[spPeriodos_ObtenerCarreraClinicas]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 09/11/2022
-- Description:	Cosultar informacion de carreras de clinica
-- =============================================
ALTER PROCEDURE [dbo].[spCatCarrerasClinicaPeriodos_ObtenerPorCarrera]
	@Carrera VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

		SELECT Id, Carrera, CantidadTrimestres, CreditosRequeridosPromedio FROM CatCarrerasClinicaPeriodos WHERE CARRERA = @Carrera
END


GO
PRINT('[dbo].[spPeriodos_ObtenerCarrerasClinicas]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 09/11/2022
-- Description:	Cosultar informacion de carreras de clinica
-- =============================================
ALTER PROCEDURE [dbo].[spCatCarrerasClinicaPeriodos_ObtenerTodos]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

		SELECT Id, Carrera, CantidadTrimestres, CreditosRequeridosPromedio FROM CatCarrerasClinicaPeriodos
END

GO
PRINT('[dbo].[spAvisos_Obtener3Avisos]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Allan Mendoza
-- Create date: 20/10/2022
-- Description: Obtiene los ultimos 3 avisos de una matricula especifica en la tabla "Avisos" segun las caracteristicas del alumno 
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
			WHERE IIF(CampusId != '', CampusId, @vcampus) = @vcampus
				  AND IIF(Nivel != '', Nivel, @vnivel) = @vnivel
				  AND IIF(SedeId != '', SedeId, @vsede) = @vsede
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

GO
PRINT('[dbo].[spAvisos_ObtenerAvisos]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Allan Mendoza
-- Create date: 20/10/2022
-- Description: Obtiene los avisos de una matricula especofica en la tabla "Avisos" segun las caracteristicas del 
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
                      WHERE  Iif(campusid != '', campusid, @vcampus) = @vcampus
                             AND Iif([nivel] != '', [nivel], @vnivel) = @vnivel
                             AND Iif([sedeid] != '', [sedeid], @vsede) = @vsede
                             AND Iif([programaid] != '', [programaid], @vprograma) = @vprograma
                             AND Iif([escuelasid] != '', [escuelasid], @vescuela) = @vescuela
                             AND a.Habilitador = 1
                             AND a.Avisoid NOT IN (SELECT AvisoId FROM Avisos_Usuarios WHERE  Activo = 1)
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

GO
PRINT('[dbo].[spAvisos_ObtenerCatalogoMatriculas]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernendez
-- Create date: 20/10/2022
-- Description:	Consultar el catalogo de matriculas para la vista de avisos de administrador en base a los demas catalogos
-- =============================================
ALTER PROCEDURE [dbo].[spAvisos_ObtenerCatalogoMatriculas]
	@Nivel varchar(2),
	@Campus varchar(3),
	@Sedes varchar(3),
	@Escuela varchar(2),
	@Programa varchar(12)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT DISTINCT 
		MATRICULA AS CLAVE, MATRICULA AS DESCRIPCION
	FROM [Replicas].dbo.ALUMNOS_PROSP_CANDIDATOS 
	WHERE	IIF(@Nivel !='', @Nivel, NIVEL_ACADEMICO) = NIVEL_ACADEMICO
		AND IIF(@Campus !='', @Campus, CLAVE_CAMPUS) = CLAVE_CAMPUS
		AND IIF(@Sedes !='', @Sedes, SITE_CODE) = SITE_CODE
		AND IIF(@Escuela !='', @Escuela, COLL_CODE) = COLL_CODE
		AND IIF(@Programa !='', @Programa, CLAVE_PROGRAMA) = CLAVE_PROGRAMA
	ORDER BY MATRICULA	
END

GO
PRINT('[dbo].[spAvisosUsuario_InsertarAvisoUsuario]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Allan Mendoza
-- Create date: 20/10/2022
-- Description:	Insertar en la tabla de Avisos_Usuarios los avisos que van dirigidos a matriculas en especifico
-- =============================================
ALTER PROCEDURE [dbo].[spAvisosUsuario_InsertarAvisoUsuario]
	@usuarioId VARCHAR(9),
	@avisoId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO Avisos_Usuarios(Matricula,AvisoId,Activo)
		VALUES(@usuarioId,@avisoId,1);

END

GO
PRINT('[dbo].[spCalendarios_InsertarCalendario]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernendez
-- Create date: 28/02/2023
-- Description:	Inserta en la tabla de Calendarios los liks de Prospectos o Candidatos para cada Campus
-- =============================================
ALTER PROCEDURE [dbo].[spCalendarios_InsertarCalendario] 
	 @CLAVE_CAMPUS			VARCHAR(3)
	,@LINK_PROSPECTO		VARCHAR(MAX)
	,@LINK_CANDIDATO		VARCHAR(MAX)
	,@ID_USUARIO			VARCHAR(50)
	AS
BEGIN
	SET NOCOUNT ON	

	DECLARE @ID_CALENDARIO INT;
	SET @ID_CALENDARIO = (SELECT TOP 1 CalendarioId FROM Calendarios WHERE ClaveCampus = @CLAVE_CAMPUS)
	
	IF @ID_CALENDARIO IS NULL OR @ID_CALENDARIO = 0
	BEGIN	
		INSERT INTO Calendarios(ClaveCampus,LinkProspecto,LinkCandidato,UsuarioRegistroId,FechaRegistro,Activo)
			 VALUES(@CLAVE_CAMPUS, @LINK_PROSPECTO, @LINK_CANDIDATO, @ID_USUARIO, GETDATE(), 1);
	END
	ELSE
	BEGIN
		UPDATE Calendarios
			SET LinkCandidato = @LINK_CANDIDATO
				,LinkProspecto = @LINK_PROSPECTO
				,UsuarioModificoId = @ID_USUARIO
				,FechaModifico = GETDATE()
				,ACTIVO = 1
			WHERE ClaveCampus = @CLAVE_CAMPUS
	END
END

GO
PRINT('[dbo].[spCalendarios_ObtenerCalendarioAlumno]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernandez
-- Create date: 28/02/2023
-- Description:	Obtiene de la tabla de Calendarios los links correspondiantes de Candidato o Prospecto en base a una Matricula
-- =============================================
ALTER PROCEDURE [dbo].[spCalendarios_ObtenerCalendarioAlumno]
	@Matricula varchar(10)
AS
BEGIN
	SET NOCOUNT ON	

	DECLARE @CLAVE_CAMPUS Varchar(3);

	SET @CLAVE_CAMPUS = (SELECT TOP 1 CLAVE_CAMPUS FROM Replicas.dbo.ALUMNOS_PROSP_CANDIDATOS WHERE MATRICULA = @Matricula);

	SELECT 
		CAST(C.CalendarioId AS varchar) as CalendarioId, C.ClaveCampus, CC.DESC_CAMPUS AS Campus, LinkProspecto, LinkCandidato
	FROM dbo.Calendarios AS C
		INNER JOIN Replicas.dbo.DIM_R_ESCO_CAMPUS  CC ON CC.CLAVE_CAMPUS = C.ClaveCampus
	WHERE C.Activo = 1 AND ClaveCampus = @CLAVE_CAMPUS 
	ORDER BY DESC_CAMPUS ASC
END

GO
PRINT('[dbo].[spCalendarios_ObtenerCalendarios]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernandez
-- Create date: 28/02/2023
-- Description:	Obtiene de la tabla de Calendarios todos los links de Prospectos o Candidatos de cada Campus
-- =============================================
ALTER PROCEDURE [dbo].[spCalendarios_ObtenerCalendarios] 
AS
BEGIN
	SET NOCOUNT ON	

	SELECT 
		CAST(C.CalendarioId AS varchar) as CalendarioId, C.ClaveCampus, CC.DESC_CAMPUS AS Campus, LinkProspecto, LinkCandidato
	FROM dbo.Calendarios AS C
		INNER JOIN Replicas.dbo.DIM_R_ESCO_CAMPUS  CC ON CC.CLAVE_CAMPUS = C.ClaveCampus
	WHERE C.Activo = 1
	ORDER BY DESC_CAMPUS ASC
END

GO
PRINT('[dbo].[spProgramaAcademico_Obtener]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mario Gil Alcala Escarcega
-- Create date: <Create Date,,>
-- Description:	Obtiene el programa academico filtrando por la clave academica 05
-- =============================================
ALTER PROCEDURE [dbo].[spProgramaAcademico_Obtener]
AS
BEGIN
	SET NOCOUNT ON
SELECT DISTINCT 
    TRIM (SUBSTRING(PA.CLAVE_PROGRAMA_ACADEMICO,1 ,3))AS PROGRAMAS, CAST(RI.ID_NIVEL_INGLES as varchar(2)) as ID_NIVEL_INGLES    
 FROM dbo.[RequisitoInglesGuardado] as RI  
  LEFT JOIN [Replicas].[dbo].[PROGRAMA_ACADEMICO] as PA 
  ON RI.CLAVE_PROGRAMA_ACADEMICO = TRIM(SUBSTRING(PA.CLAVE_PROGRAMA_ACADEMICO,1 ,3))

  WHERE CLAVE_NIVEL_ACAD_CURSO = 05
  ORDER BY
  PROGRAMAS ASC;
END

GO
PRINT('[dbo].[spNivelIngles_Obtener]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mario Gil Alcala Escarcega
-- Create date: <Create Date,,>
-- Description:	Obtiene el nivel de ingles y cuando aparece NEX lo convierte en Sin Examen Autorizado (SEA)
-- =============================================
ALTER PROCEDURE [dbo].[spNivelIngles_Obtener]
@Matricula varchar (15)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
APCN.MATRICULA AS MATRICULA, 
TRIM(SUBSTRING(APCN.CLAVE_PROGRAMA,1 ,3)) AS PROGRAMA,
CASE GNI.NIVEL_IDIOMA_REQ_GRAD WHEN 'NEX' THEN 'Sin Examen Autorizado (SEA)' ELSE GNI.NIVEL_IDIOMA_REQ_GRAD END AS NIVEL_IDIOMA_REQ_GRAD,
CNI.NIVEL_INGLES AS CLAVE_NIVEL_ACAD_ALUMNO,
GNI.FECHA_ULTIMA_MODIFICACION AS FECHA_ULTIMA_MODIFICACION,
CAST (CASE WHEN GNI.NIVEL_IDIOMA_REQ_GRAD = CNI.NIVEL_INGLES THEN 1 ELSE 0 END AS bit) AS IND_CUMPLE_REQ_GRAD


FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] AS APCN
INNER JOIN [ReqGraduaciones].[dbo].[RequisitoInglesGuardado] AS RIG ON RIG.CLAVE_PROGRAMA_ACADEMICO = TRIM(SUBSTRING(APCN.CLAVE_PROGRAMA,1 ,3))
INNER JOIN [ReqGraduaciones].[dbo].[Cat_Nivel_Ingles] AS CNI ON CNI.ID_NIVEL_INGLES = RIG.ID_NIVEL_INGLES 
INNER JOIN [Replicas].[dbo].[GRADUADOS_NIVEL_INGLES] AS GNI ON GNI.MATRICULA = APCN.MATRICULA
WHERE APCN.MATRICULA LIKE @Matricula

END

GO
PRINT('[dbo].[spRequisitoInglesGuardado_Insertar]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mario Gil Alcala Escarcega
-- Create date: <Create Date,,>
-- Description:	Se insertan o se actualizan los registros de nivel de ingles del programa academico
-- =============================================
ALTER PROCEDURE [dbo].[spRequisitoInglesGuardado_Insertar] 
	 --@ID_REQUISITO_GUARDADO		INT
	 @ID_NIVEL_INGLES				INT
	,@CLAVE_PROGRAMA_ACADEMICO		VARCHAR(12)
	,@ID_USUARIO			VARCHAR(10)
	AS
BEGIN
	SET NOCOUNT ON	

	DECLARE @ID_REQUISITO_GUARDADO INT;
	SET @ID_REQUISITO_GUARDADO = (SELECT TOP 1 ID_REQUISITO_GUARDADO FROM RequisitoInglesGuardado WHERE CLAVE_PROGRAMA_ACADEMICO = @CLAVE_PROGRAMA_ACADEMICO)
	
	IF @ID_REQUISITO_GUARDADO IS NULL OR @ID_REQUISITO_GUARDADO = 0
	BEGIN	
		INSERT INTO RequisitoInglesGuardado (ID_NIVEL_INGLES, CLAVE_PROGRAMA_ACADEMICO, ID_USUARIO_REGISTRA, ID_USUARIO_MODIFICA, FECHA_REGISTRO,FECHA_MODIFICACION, ACTIVO)
			VALUES (@ID_NIVEL_INGLES, @CLAVE_PROGRAMA_ACADEMICO, @ID_USUARIO, @ID_USUARIO, GETDATE(), GETDATE(), 1)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[RequisitoInglesGuardado]
			SET ID_NIVEL_INGLES = @ID_NIVEL_INGLES
				,CLAVE_PROGRAMA_ACADEMICO = @CLAVE_PROGRAMA_ACADEMICO
				,ID_USUARIO_MODIFICA = @ID_USUARIO
				,FECHA_MODIFICACION = GETDATE()
				,ACTIVO = 1
			WHERE ID_REQUISITO_GUARDADO = @ID_REQUISITO_GUARDADO
	END
END

GO
PRINT('  StoredProcedure [dbo].[spRoles_ObtenerDescripcionRoles]  ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 16-06-2023
-- Description:	Obtiene la Descripcion de todos Roles Activos 
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ObtenerDescripcionRoles]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
		SELECT 
			  [IdRol]
			  ,[Descripcion]
		  FROM 
				[dbo].[Roles] 
		  WHERE
			[Activo] = 1
		 AND
			[Estatus] = 1
END


GO
PRINT('  StoredProcedure [dbo].[spRoles_ObtenerUsuariosPorRol] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 12-06-2023
-- Description:	Selecciona los Usuarios de un Rol en especifico y el total de roles de cada usuario 
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ObtenerUsuariosPorRol] 
	-- Add the parameters for the stored procedure here
	@pIdRol				int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		UR.[Id]
		,UR.[IdUsuario]
		,UR.[IdRol]
		,U.[NumeroNomina]
		,U.[Nombre]
		,[dbo].fnUsuarios_ObtenerRolesPorUsuario(UR.[IdUsuario]) AS Roles
	FROM 
		[dbo].[UsuarioRoles] UR
	LEFT JOIN 
		[dbo].[Usuarios] U
	ON 
		UR.[IdUsuario] = U.[IdUsuario] AND UR.[Estatus] = 1
	WHERE
		U.Estatus  = 1
	AND
		UR.[IdRol] = @pIdRol
END

GO
PRINT(' StoredProcedure [dbo].[spReporteSabana] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Carlos Bernadac
-- Create date: 23/12/2022
-- Description: Reporte de SABANA
-- =============================================
ALTER PROCEDURE [dbo].[spReporteSabana]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  DISTINCT
		P.MATRICULA, 
		CONCAT(PE.NOMBRE, ' ', PE.APELLIDO_PATERNO, ' ',  PE.APELLIDO_MATERNO) AS NOMBRE_COMPLETO,
		--PA.DESC_PROGRAMA_ACADEMICO AS PROGRAMAACADEMICO, 
		PA.CLAVE_PROGRAMA_ACADEMICO,
		--P.MAJR_CODE AS CARRERAID, 
		--P.DESC_MAJR_CODE AS CARRERA,		
		dbo.fnConcentraciones_ObtenerConcentracionesAlumno(P.PIDM, 1) AS CONCENTRACION_UNO,
		dbo.fnConcentraciones_ObtenerConcentracionesAlumno(P.PIDM, 2) AS CONCENTRACION_DOS,
		dbo.fnConcentraciones_ObtenerConcentracionesAlumno(P.PIDM, 3) AS CONCENTRACION_TRES,
		CASE 
		 WHEN DA.CODIGO_DISTINCION IS NULL THEN ''
		 WHEN DA.CODIGO_DISTINCION = 'ULEAD' THEN 'SC'
		 ELSE 'NC' END AS ULEAD,
		CASE
		 WHEN DA.CODIGO_DISTINCION IS NULL THEN '' 
		 WHEN DA.CODIGO_DISTINCION != 'ULEAD' THEN 'SC'
		 ELSE 'NC' END AS DIPLOMA_INTERNACIONAL,
		PE.CLAVE_GENERO AS GENERO,
		PE.CLAVE_NACIONALIDAD_PRINCIPAL AS NACIONALIDAD,
		[dbo].[fnR_TELEFONO](P.PIDM) AS TELEFONO,
		[dbo].fnR_IDEN_CORREO_ELECTRONICO_ObtenCorreo(P.PIDM) AS CORREO,
		P.LAST_TERM_CURSADO AS PERIODO,
		C.DESC_CAMPUS AS CAMPUS,
		NA.DESC_NIVEL_ACADEMICO AS NIVEL_ACADEMICO,
		P.[CREDITOS_PLAN] AS CREDITOS_PLAN,
		P.[CREDITOS_FALTANTES] AS CREDITOS_PENDIENTES,
		P.[CREDITOS_ACREDITADOS] AS CREDITOS_ACREDITADOS,
		P.[CREDITOS_FALTANTES] AS CREDITOS_FALTANTES,
		'' AS CREDITOS_PERIODO,
		ST.[TOTAL_SEMANAS_TEC18_ACREDITADAS] + ST.[TOTAL_SEMANAS_TEC_ACREDITADAS] AS SEMANAS_TEC,
		SS.HorasAcumuladas AS SERVICIO_SOCIAL_HT,
		CASE WHEN SS.HorasAcumuladas >= 480 THEN 'SC' ELSE 'NC' END AS SERVICIO_SOCIAL_ESTATUS,
		GNI.NIVEL_IDIOMA_REQ_GRAD AS EXAMEN_INGLES,
		GNI.IND_CUMPLE_REQUISITO_GRAD AS EXAMEN_INGLES_ESTATUS,
		'' AS EXAMEN_INGLES_FECHA,
		'' AS EXAMEN_INGLES_PUNTAJE,
		CASE WHEN CEN.PIDM IS NOT NULL THEN CEN.CLAVE_EXAMEN 
			 WHEN CEN_N.PIDM IS NOT NULL THEN CEN_N.CLAVE_EXAMEN 
			 ELSE ''
		END AS CENEVAL,
		CASE WHEN CEN.PIDM IS NOT NULL THEN 'SC'
			 WHEN CEN_N.PIDM IS NOT NULL THEN 'NC'
			 ELSE ''
		END  AS CENEVALE_ESTATUS,
		EI.ESTATUS AS EXAMEN_INTEGRADOR,
		GNI.NIVEL_IDIOMA_REQ_GRAD,
		'' AS IDIOMA_DIST_ESP,
		P.CREDITOS_CURSA_EXTRANJERO,
		'' AS PROMEDIO,
		P.FECHA_AUDITORIA AS FECHA_REGISTRO,
		P.CLAVE_ESTATUS_GRADUACION AS SHADEGR,
		APG.PeriodoCeremonia AS PERIODO_CEREMONIA
	FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] P
		LEFT JOIN [Replicas].[dbo].R_IDEN_PERSONA PE ON P.PIDM = PE.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].PROGRAMA_ACADEMICO PA ON PA.CLAVE_PROGRAMA_ACADEMICO = P.CLAVE_PROGRAMA
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] S ON [dbo].fnPIDM_MENTORES_ObtenPrimerValor(P.PIDM_MENTORES) = S.SPRIDEN_PIDM
		LEFT JOIN [Replicas].[dbo].[SPRIDEN] D ON P.PIDM_DIRECTOR = D.SPRIDEN_PIDM
		LEFT JOIN [Replicas].[dbo].R_IDEN_TELEFONO IT ON P.PIDM = IT.CLAVE_IDENTIDAD
		LEFT JOIN [Replicas].[dbo].DIM_R_ESCO_CAMPUS C ON C.CLAVE_CAMPUS = P.CLAVE_CAMPUS
		LEFT JOIN [Replicas].[dbo].DIM_R_ESCO_NIVEL_ACADEMICO NA ON NA.CLAVE_NIVEL_ACADEMICO = P.NIVEL_ACADEMICO
		LEFT JOIN [dbo].ExamenIntegrador EI ON EI.MATRICULA = P.MATRICULA 
		LEFT JOIN [Replicas].[dbo].GRADUADOS_NIVEL_INGLES GNI ON GNI.MATRICULA = P.MATRICULA
		LEFT JOIN [dbo].[AlumnosPeriodoGraduacion] APG ON APG.Matricula = P.MATRICULA AND APG.Estatus = 1
		LEFT JOIN [Replicas].[dbo].[SEMANAS_TEC] ST ON P.PIDM = ST.PIDM
		LEFT JOIN [Replicas].[dbo].[CENEVAL_ACREDITADO] CEN ON P.PIDM = CEN.PIDM
		LEFT JOIN [Replicas].[dbo].[CENEVAL_NO_ACREDITADO] CEN_N ON P.PIDM = CEN_N.PIDM
		LEFT JOIN [Replicas].[dbo].[ALUMNOS_SS] SS ON P.PIDM = SS.PIDM
		LEFT JOIN [Replicas].[dbo].[DISTINCIONES_ACADEMICAS] DA ON P.PIDM = DA.PIDM

END

GO
PRINT('  [dbo].[spCatPriodosGraduacion_ObtenerTodos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Antonio Mendoza
-- Create date: 02/01/2023
-- Description:	Obtener todos los periodos y su descripcion de la tabla ejercicio academico
-- =============================================
ALTER PROCEDURE [dbo].[spCatPriodosGraduacion_ObtenerTodos]	
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID
	, PERIODO_GRADUACION
	, DESC_PERIODO_GRADUACION
	FROM [dbo].[Cat_Periodos_Graduacion]
END

GO
PRINT('  StoredProcedure [dbo].[spAvisos_ObtenerCatalogo] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spAvisos_ObtenerCatalogo]
	@Opcion int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--NIVEL
	IF @Opcion = 1
	BEGIN
		SELECT DISTINCT CLAVE_NIVEL_ACADEMICO AS CLAVE, DESC_NIVEL_ACADEMICO AS DESCRIPCION
		FROM Replicas.dbo.DIM_R_ESCO_NIVEL_ACADEMICO
		WHERE IND_ACTIVO = 'A'	
		ORDER BY CLAVE_NIVEL_ACADEMICO ASC
	END

	--CAMPUS
	IF @Opcion = 2
	BEGIN
		SELECT DISTINCT CLAVE_CAMPUS AS CLAVE, DESC_CAMPUS AS DESCRIPCION
		FROM Replicas.dbo.DIM_R_ESCO_CAMPUS
		WHERE IND_ACTIVO = 'A'
		ORDER BY DESC_CAMPUS ASC	
	END

	--SEDES
	IF @Opcion = 3
	BEGIN
		SELECT DISTINCT CLAVE_TIPO_SEDE AS CLAVE, DESC_TIPO_SEDE AS DESCRIPCION
		FROM Replicas.dbo.DIM_R_ESCO_SEDE
		WHERE IND_ACTIVO = 'A'	
		ORDER BY DESC_TIPO_SEDE ASC
	END

	--PROGRAMAS
	IF @Opcion = 4
	BEGIN
		SELECT DISTINCT CLAVE_PROGRAMA_ACADEMICO AS CLAVE, CONCAT(CLAVE_PROGRAMA_ACADEMICO,'-',DESC_PROGRAMA_ACADEMICO) AS DESCRIPCION
		FROM Replicas.dbo.PROGRAMA_ACADEMICO
		ORDER BY CONCAT(CLAVE_PROGRAMA_ACADEMICO,'-',DESC_PROGRAMA_ACADEMICO) ASC
	END

	--MATRICULA
	IF @Opcion = 5
	BEGIN
		SELECT DISTINCT MATRICULA AS CLAVE, MATRICULA AS DESCRIPCION
		FROM [Replicas].dbo.ALUMNOS_PROSP_CANDIDATOS 
		WHERE INDICADOR_CANDIDATO = 1 
		ORDER BY MATRICULA
	END

	--ESCUELA
	IF @Opcion = 6
	BEGIN
		SELECT DISTINCT CLAVE_DIVISION AS CLAVE, DESC_DIVISION AS DESCRIPCION
		FROM [Replicas].dbo.DIM_R_ESCO_DIVISION 
		WHERE IND_ACTIVO = 'A' 
		ORDER BY DESC_DIVISION ASC
	END

	--REQUISITOS
	IF @Opcion = 7
	BEGIN
		SELECT DISTINCT CAST(ID AS varchar) AS CLAVE, TARJETA AS DESCRIPCION
		FROM dbo.Tarjetas
		ORDER BY TARJETA ASC
	END
END

GO
PRINT('[dbo].[spUsuarios_ObtenerHistorial]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 26/05/2023
-- Description:	Consultar historial de roles, campus, sedes y nivel del usaurio 
-- =============================================
ALTER PROCEDURE [dbo].[spUsuarios_ObtenerHistorial](
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
		(SELECT TOP 1 FechaRegistro FROM UsuarioCampusSede WHERE IdUsuario = @IdUsuario AND IdHistorial = U.IdHistorial) AS FechaModificacion,
		CONCAT(A.NOMBRE, ' ', A.APELLIDO_PATERNO, ' ', A.APELLIDO_MATERNO) AS UsuarioModifico,
		UC.UsuarioRegistro
		FROM Usuarios U
		LEFT JOIN UsuarioCampusSede UC ON U.IdUsuario = UC.IdUsuario
		LEFT JOIN [Replicas].[dbo].R_IDEN_AFILIACION B ON B.ID_AFILIACION = UC.UsuarioRegistro 
		LEFT JOIN  [Replicas].[dbo].R_IDEN_PERSONA A ON A.CLAVE_IDENTIDAD = B.CLAVE_IDENTIDAD
		WHERE U.IdUsuario = @IdUsuario
		ORDER BY IdHistorial DESC
END

GO
PRINT('[dbo].[spAccesoNomina_ObtenerUsuarioAdministrador]');
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ulises Mireles Cruz
-- Create date: 30-01-2023
-- Description: Obtiene el acceso de un usuario administrador por Nomina  
-- =============================================
CREATE PROCEDURE [dbo].[spAccesoNomina_ObtenerUsuarioAdministrador] 
	@Nomina Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Acceso BIT = 0;
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT IdUsuario FROM Usuarios WHERE NumeroNomina = @Nomina AND Estatus = 1)
	BEGIN
		SET @Acceso = 1;
	END
	SELECT @Acceso AS Acceso, '' AS Ambiente;
END
GO
PRINT(' [dbo].[spCampus_ObtenerCampusPorNomina] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los Campus que tiene un usuario en específico por su numero de nomina  
-- =============================================
CREATE PROCEDURE [dbo].[spCampus_ObtenerCampusPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT
			UC.[ClaveCampus]
			,C.[DESC_CAMPUS] AS Descripcion
		FROM 
			[dbo].[Usuarios] U
		LEFT JOIN 
			[dbo].[UsuarioCampusSede] UC
		ON 
			U.[IdUsuario] = UC.[IdUsuario] AND  UC.[Estatus] = 1 AND U.[Estatus] = 1
		LEFT JOIN 
			[Replicas].[dbo].CAMPUS_SEDES C 
		ON 
			UC.[ClaveCampus] = C.[CLAVE_CAMPUS]
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
END

GO
PRINT(' [dbo].[spNivel_ObtenerNivelesPorNomina] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los Niveles que tiene un usuario en específico por su numero de nomina  
-- =============================================
CREATE PROCEDURE [dbo].[spNivel_ObtenerNivelesPorNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT 
			UN.[ClaveNivel] 
			,N.[DESC_NIVEL_ACADEMICO] AS Descripcion
		FROM 
			Usuarios U
		LEFT JOIN 
			UsuarioNivel UN 
		ON 
			U.IdUsuario = UN.IdUsuario AND UN.Estatus = 1
		LEFT JOIN 
			[Replicas].[dbo].DIM_R_ESCO_NIVEL_ACADEMICO N 
		ON 
			UN.[ClaveNivel] = N.[CLAVE_NIVEL_ACADEMICO]
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
END

GO
PRINT(' [dbo].[spRoles_ObtenerRolesPorNomina] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene los roles que tiene un usuario en específico por su numero de nomina  
-- =============================================
CREATE PROCEDURE [dbo].[spRoles_ObtenerRolesPorNomina]
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
			U.[IdUsuario] = UR.[IdUsuario] AND  UR.[Estatus] = 1 AND U.[Estatus] = 1
		LEFT JOIN 
			Roles R 
		ON 
			UR.[IdRol] = R.[IdRol] AND R.[Activo] = 1
		WHERE 
			U.[NumeroNomina] = @pNumeroNomina
END

GO
PRINT(' [dbo].[spSedes_ObtenerSedesPorCampusNomina] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 19-06-2023
-- Description:	Obtiene las Sedes que tiene un usuario en específico por su numero de nomina  y Campus
-- =============================================
CREATE PROCEDURE [dbo].[spSedes_ObtenerSedesPorCampusNomina]
	-- Add the parameters for the stored procedure here
	@pNumeroNomina			Varchar(9),
	@pClaveCampus			Varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT DISTINCT 
			UC.[ClaveSede]
			,S.[DESC_TIPO_SEDE] AS Descripcion
		FROM 
			Usuarios U
		LEFT JOIN 
			UsuarioCampusSede UC 
		ON 
			U.IdUsuario = UC.IdUsuario AND UC.Estatus = 1
		LEFT JOIN 
			[Replicas].[dbo].CAMPUS_SEDES S 
		ON 
			UC.ClaveSede = S.CLAVE_TIPO_SEDE
		WHERE 
			UC.ClaveCampus = @pClaveCampus  
		AND 
			U.NumeroNomina = @pNumeroNomina
END

GO
PRINT(' [dbo].[spSecciones_InsertarActualizarPermisos] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 25-05-2023
-- Description:	Inserta o Modifica según sea el caso los Permisos de las Sección por IdRol
-- =============================================
ALTER PROCEDURE [dbo].[spSecciones_InsertarActualizarPermisos]
	@pIdRol				int,
	@pIdMenu			int,
	@IdSubMenu			int,
    @pVer				bit,
	@pEditar			bit,
	@pActiva			bit,
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
																				     AND [IdSubMenu] = @IdSubMenu
																					 AND [Activa] = 1), 0)
		---Si no existe el registro lo modifica, de lo contrario lo inserta 
		If @vRegistro > 0
			BEGIN  
				UPDATE [dbo].[SeccionesPermisos]
					SET 
					    [Ver] = @pVer
						,[Editar] = @pEditar
						,[Activa] = @pActiva
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
				   ,[Editar]
				   ,[Activa])
				VALUES
				   (@pIdRol
				   ,@pIdMenu
				   ,@IdSubMenu
				   ,@pVer
				   ,@pEditar
				   ,1)
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
PRINT(' [dbo].[spSecciones_ObtenerSeccionesPorRol] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 25-05-2023
-- Description:	Selecciona de la Tabla Secciones los Permisos del rol por IdRol
-- =============================================
ALTER PROCEDURE [dbo].[spSecciones_ObtenerSeccionesPorRol]
	@pIdRol				int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 SELECT 
			sp.[IdPermiso]
			,sp.[IdRol]
			,sp.[IdMenu]
			,m.[NOMBRE] As NombreMenu
			,m.[PATH] As PathMenu
			,m.[ICONO] As IconoMenu
			,sp.[IdSubMenu]
			,sm.[NOMBRE] AS NombreSubMenu
			,sm.[PATH] As PathSubMenu
			,sm.[ICONO] As IconoSubMenu
			,sm.[Seccion]
            ,sp.[Ver]
			,sp.[Editar]
			,sp.[Activa]
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
		AND 
			sp.[Activa] = 1
END

GO
PRINT(' [dbo].[spExpedientes_ObtenerComentariosExpedientes] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        José Antonio Mendoza Guevara
-- Create date: 03-11-2022
-- Description:    Consulta los últimos 3 comentarios del Expediente deseado de la Tabla Expedientes 
-- =============================================

ALTER PROCEDURE [dbo].[spExpedientes_ObtenerComentariosExpedientes] 
	@Matricula	VARCHAR(9)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT TOP 3 
			DETALLE AS COMENTARIO, FECHA_REGISTRO 
		FROM 
			Expedientes 
		WHERE 
			MATRICULA = @Matricula 
		AND 
			DETALLE != '' 
		ORDER BY FECHA_REGISTRO DESC, Id DESC
END

GO
PRINT(' [dbo].[spExamenIntegrador_ObtenerPorMatricula] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin
-- Create date: 03-11-2022
-- Description:	Obtiene el regristro deseado de la -tabla Examen Integrador por Matricula
-- =============================================
ALTER PROCEDURE [dbo].[spExamenIntegrador_ObtenerPorMatricula] 
	@MATRICULA varchar(9)
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
	   [MATRICULA]
      ,[PERIODO_GRADUACION]
      ,[NIVEL_ACADEMICO]
      ,[NOMBRE_REQUISITO]
      ,[ESTATUS]
      ,[FECHA_EXAMEN]
      ,[FECHA_REGISTRO]
      ,[ID_USUARIO_REGISTRO]
      ,[ID_USUARIO_MODIFICO]
      ,[FECHA_MODIFICO]
      ,[ACTIVO]
	  ,CAST (CASE
				WHEN [dbo].[ExamenIntegrador].[ESTATUS] = 'NA'  
				THEN 0
				WHEN EXISTS (SELECT Clave, IdTipoExamen FROM [dbo].[CarrerasConRequisitoExamen] WHERE IdTipoExamen = 2 AND Clave = (SELECT [MAJR_CODE] FROM [Replicas].[dbo].[ALUMNOS_PROSP_CANDIDATOS] WHERE [MATRICULA] = @MATRICULA))
				THEN 1
				ELSE 0
			 END AS bit) AS APLICA
  FROM 
	[dbo].[ExamenIntegrador] 
  WHERE
	[dbo].[ExamenIntegrador].[MATRICULA] = @MATRICULA
	AND 
	[ExamenIntegrador].[ACTIVO] = 1
	
END
GO
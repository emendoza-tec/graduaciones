USE [ReqGraduaciones]
GO
PRINT('StoredProcedure [dbo].[spAvisos_ObtenerCatalogoMatriculas] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE [ReqGraduaciones]
GO
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

	DECLARE @CampusSedesPermiso dbo.UsuarioCampusSedeType;
	DECLARE @NivelesPermiso dbo.UsuarioNivelType;

	IF @Nivel = '' 
	BEGIN
		INSERT INTO @NivelesPermiso SELECT @IdUsuario, ClaveNivel FROM UsuarioNivel WHERE IdUsuario = @IdUsuario AND Estatus = 1
	END
	ELSE
	BEGIN
		INSERT INTO @NivelesPermiso SELECT @IdUsuario, @Nivel
	END

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
	WHERE	NIVEL_ACADEMICO IN (SELECT ClaveNivel FROM @NivelesPermiso)
		AND CLAVE_CAMPUS IN (Select ClaveCampus FROM @CampusSedesPermiso)
		AND SITE_CODE IN (Select ClaveSede FROM @CampusSedesPermiso)
		AND IIF(@Escuela !='', @Escuela, COLL_CODE) = COLL_CODE
		AND IIF(@Programa !='', @Programa, CLAVE_PROGRAMA) = CLAVE_PROGRAMA
	ORDER BY MATRICULA	
END

GO
PRINT('  StoredProcedure [dbo].[spAvisos_ObtenerAvisos] ')
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
                      WHERE @vcampus = IIF(CampusId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveCampus FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveCampus = @vcampus), @vcampus)
							AND @vnivel = IIF(Nivel = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveNivel FROM UsuarioNivel WHERE IdUsuario = a.IdUsuario AND ClaveNivel = @vnivel), @vnivel)
							AND @vsede = IIF(SedeId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveSede = @vsede), @vsede)
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

GO
PRINT('StoredProcedure [dbo].[spAvisos_Obtener3Avisos] ')
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
			WHERE @vcampus = IIF(CampusId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveCampus FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveCampus = @vcampus), @vcampus)
				AND @vnivel = IIF(Nivel = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveNivel FROM UsuarioNivel WHERE IdUsuario = a.IdUsuario AND ClaveNivel = @vnivel), @vnivel)
				AND @vsede = IIF(SedeId = '' AND a.IdUsuario IS NOT NULL, (SELECT TOP 1 ClaveSede FROM UsuarioCampusSede WHERE IdUsuario = a.IdUsuario AND ClaveSede = @vsede), @vsede)
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
  END;

GO
PRINT('  StoredProcedure [dbo].[spAvisos_InsertarAviso] ')
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
			DECLARE @NivelesPermiso dbo.UsuarioNivelType;

			IF @nivel = ''
			BEGIN
				INSERT INTO @NivelesPermiso SELECT @IdUsuario, ClaveNivel FROM UsuarioNivel WHERE IdUsuario = @IdUsuario AND Estatus = 1
			END
			ELSE
			BEGIN
				INSERT INTO @NivelesPermiso SELECT @IdUsuario, @nivel
			END

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
					and a.NIVEL_ACADEMICO IN (Select ClaveNivel FROM @NivelesPermiso)
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
GO

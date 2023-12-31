USE [ReqGraduaciones]
GO
/****** Object:  StoredProcedure [dbo].[spServicioSocial_ObtenerHoras]    Script Date: 25/07/2023 10:03:33 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Misael Hernandez
-- Create date: 24/07/2023
-- Description:	Obtiene las horas acumuladas para acreditar el requisito de Servicio Social desde Replicas
-- =============================================
CREATE PROCEDURE [dbo].[spServicioSocial_ObtenerHoras]
	@Matricula varchar(10)
AS
BEGIN
	SET NOCOUNT ON	

	SELECT HorasAcumuladas
	FROM Replicas.dbo.ALUMNOS_SS 
	WHERE MatriculaAlumno = @Matricula

END
USE [ReqGraduaciones]
GO
----Cambia el Campo Periodo que sea nulo 
ALTER TABLE [dbo].[LogExcepciones] ALTER COLUMN FechaAlta datetime NOT NULL;

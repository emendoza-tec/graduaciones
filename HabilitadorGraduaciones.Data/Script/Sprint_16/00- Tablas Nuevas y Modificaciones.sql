USE [ReqGraduaciones]
GO
PRINT ('UPDATE TABLE [SolicitudCambioDatosPersonales]')
----Campo nuevo 
ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] ADD [NumeroSolicitud] [Int];

GO
PRINT ('UPDATE TABLE Igualar a todos los registros de la Columna [NumeroSolicitud] a [IdSolicitud]')
UPDATE [dbo].[SolicitudCambioDatosPersonales] SET [NumeroSolicitud] = [IdSolicitud]

GO
PRINT ('UPDATE TABLE Cambia el Campo [NumeroSolicitud] que no sea nulo')
ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] ALTER COLUMN [NumeroSolicitud] int NOT NULL;


GO
PRINT ('Eliminacion de la relacion con el IdSolicitud')
ALTER TABLE [dbo].[DetalleSolicitudCambioDatosPersonales]
DROP CONSTRAINT [FK_DetalleSolicitudCambioDatosPersonales_SolicitudCambioDatosPersonales];
GO

PRINT ('Tabla Modificada [Avisos]')
GO
ALTER TABLE Avisos
ADD IdUsuario int NULL;
GO
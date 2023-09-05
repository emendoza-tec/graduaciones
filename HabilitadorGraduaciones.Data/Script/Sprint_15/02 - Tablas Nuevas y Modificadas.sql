USE [ReqGraduaciones]
GO
PRINT('DELETE [dbo].[UsuarioMovimientos]');
DROP TABLE UsuarioMovimientos;

PRINT('[dbo].[UsuarioMovimientos]');
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsuarioMovimientos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioCreado] [varchar](9) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[Nivel] [varchar](MAX) NULL,
	[RolAsignado] [varchar](MAX) NULL,
	[CampusAsignados] [varchar](MAX) NULL,
	[SedesAsignadas] [varchar](MAX) NULL,
	[UsuarioRegistraId] [int] NULL,
	[UsuarioModificaId] [int] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaModificacion] [datetime] NULL,
	[MovimientoId] [int] NULL,
 CONSTRAINT [PK_UsuarioMovimiento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO
PRINT ('Tabla Modificada [SeccionesPermisos]')
----Campo nuevo 
ALTER TABLE [dbo].[SeccionesPermisos] ADD [Activa] [bit];

GO
PRINT ('UPDATE Cambia a todos los registros de la Columna Activa a 1')
UPDATE [dbo].[SeccionesPermisos] SET [Activa] = 1

GO
PRINT ('ALTER TABLE Cambia el Campo [Activa] que no sea nulo')
ALTER TABLE [dbo].[SeccionesPermisos] ALTER COLUMN [Activa] bit NOT NULL;

GO
PRINT ('Tabla Modificada [SubMenus]')
----Campo nuevo 
ALTER TABLE [dbo].[SubMenus] ADD [Seccion] [bit];

GO
PRINT ('UPDATE Cambia a todos los registros de la Columna [Seccion] a 0')
UPDATE [dbo].[SubMenus] SET [Seccion] = 0

GO
PRINT ('ALTER TABLE Cambia el Campo [Seccion] que no sea nulo')
ALTER TABLE [dbo].[SubMenus] ALTER COLUMN [Seccion] bit NOT NULL;

PRINT ('Registros nuevos en SubMenus')

GO
INSERT [dbo].[SubMenus] ([NOMBRE], [IDMENU], [PATH], [ICONO], [ACTIVO], [Seccion]) VALUES ( N'Reporte Sabana', 2, NULL, NULL, 1, 1)
GO
INSERT [dbo].[SubMenus] ([NOMBRE], [IDMENU], [PATH], [ICONO], [ACTIVO], [Seccion]) VALUES ( N'Reporte actualizado de Estimado de Graduación', 2, NULL, NULL, 1, 1)
GO
INSERT [dbo].[SubMenus] ([NOMBRE], [IDMENU], [PATH], [ICONO], [ACTIVO], [Seccion]) VALUES ( N'Panel de solicitud de modificación de datos', 1, NULL, NULL, 1, 1)
GO



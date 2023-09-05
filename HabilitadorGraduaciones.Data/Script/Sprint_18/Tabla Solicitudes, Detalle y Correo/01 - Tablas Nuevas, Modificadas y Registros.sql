USE [ReqGraduaciones]
GO
PRINT('Table [dbo].[DetalleSolicitudCambioDatosPersonales] ')
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleSolicitudCambioDatosPersonales]') AND type in (N'U'))
DROP TABLE [dbo].[DetalleSolicitudCambioDatosPersonales]
GO

ALTER TABLE [dbo].[CorreoSolicitudCambioDatosPersonales] DROP CONSTRAINT [FK_CorreoSolicitudCambioDatosPersonales_SolicitudCambioDatosPersonales]
GO

PRINT('Table [dbo].[CorreoSolicitudCambioDatosPersonales] ')
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CorreoSolicitudCambioDatosPersonales]') AND type in (N'U'))
DROP TABLE [dbo].[CorreoSolicitudCambioDatosPersonales]
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] DROP CONSTRAINT [FK_SolicitudCambioDatosPersonales_EstatusSolicitudDatosPersonales]
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] DROP CONSTRAINT [FK_SolicitudCambioDatosPersonales_DatosPersonalesEditables]
GO

PRINT('Table [dbo].[SolicitudCambioDatosPersonales]  ')
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudCambioDatosPersonales]') AND type in (N'U'))
DROP TABLE [dbo].[SolicitudCambioDatosPersonales]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
PRINT('Table [dbo].[DetalleSolicitudCambioDatosPersonales]')
CREATE TABLE [dbo].[DetalleSolicitudCambioDatosPersonales](
	[IdDetalleSolicitud] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitud] [int] NOT NULL,
	[Documento] [varchar](max) NOT NULL,
	[Extension] [varchar](5) NOT NULL,
	[AzureStorage] [varchar](max) NULL,
 CONSTRAINT [PK_DetalleSolicitudCambioDatosPersonales] PRIMARY KEY CLUSTERED 
(
	[IdDetalleSolicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GO
PRINT('Table [dbo].[SolicitudCambioDatosPersonales]')
CREATE TABLE [dbo].[SolicitudCambioDatosPersonales](
	[IdSolicitud] [int] IDENTITY(1,1) NOT NULL,
	[NumeroSolicitud] [int] NULL,
	[Matricula] [varchar](9) NOT NULL,
	[PeriodoGraduacion] [varchar](6) NOT NULL,
	[IdDatosPersonales] [int] NOT NULL,
	[FechaSolicitud] [date] NOT NULL,
	[UltimaActualizacion] [date] NULL,
	[IdEstatusSolicitud] [int] NOT NULL,
	[DatoIncorrecto] [varchar](250) NOT NULL,
	[DatoCorrecto] [varchar](250) NOT NULL,
	[UsarioRegistro] [varchar](9) NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[UsuarioModifico] [varchar](9) NULL,
	[FechaModificacion] [datetime] NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_SolicitudCambioDatosPersonales] PRIMARY KEY CLUSTERED 
(
	[IdSolicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudCambioDatosPersonales_DatosPersonalesEditables] FOREIGN KEY([IdDatosPersonales])
REFERENCES [dbo].[DatosPersonalesEditables] ([IdDatosPersonales])
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] CHECK CONSTRAINT [FK_SolicitudCambioDatosPersonales_DatosPersonalesEditables]
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudCambioDatosPersonales_EstatusSolicitudDatosPersonales] FOREIGN KEY([IdEstatusSolicitud])
REFERENCES [dbo].[EstatusSolicitudDatosPersonales] ([IdEstatusSolicitud])
GO

ALTER TABLE [dbo].[SolicitudCambioDatosPersonales] CHECK CONSTRAINT [FK_SolicitudCambioDatosPersonales_EstatusSolicitudDatosPersonales]
GO

GO
PRINT('Table [dbo].[CorreoSolicitudCambioDatosPersonales] ')
CREATE TABLE [dbo].[CorreoSolicitudCambioDatosPersonales](
	[IdCorreo] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitud] [int] NOT NULL,
	[Destinatario] [varchar](128) NOT NULL,
	[Comentarios] [varchar](250) NOT NULL,
	[Enviado] [bit] NOT NULL,
	[Nombre] [varchar](150) NULL,
 CONSTRAINT [PK_CorreoSolicitudCambioDatosPersonales] PRIMARY KEY CLUSTERED 
(
	[IdCorreo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CorreoSolicitudCambioDatosPersonales]  WITH CHECK ADD  CONSTRAINT [FK_CorreoSolicitudCambioDatosPersonales_SolicitudCambioDatosPersonales] FOREIGN KEY([IdSolicitud])
REFERENCES [dbo].[SolicitudCambioDatosPersonales] ([IdSolicitud])
GO

ALTER TABLE [dbo].[CorreoSolicitudCambioDatosPersonales] CHECK CONSTRAINT [FK_CorreoSolicitudCambioDatosPersonales_SolicitudCambioDatosPersonales]
GO
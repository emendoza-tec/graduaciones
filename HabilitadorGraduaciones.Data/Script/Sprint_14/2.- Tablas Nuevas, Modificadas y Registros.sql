USE [ReqGraduaciones]
GO
PRINT('Tablas, Indices y Llaves')
PRINT(' Table [dbo].[CatMovimientos] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CatMovimientos](
	[Id_Movimiento] [int] IDENTITY(1,1) NOT NULL,
	[Movimiento] [varchar](25) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Movimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[Roles]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Estatus] [bit] NOT NULL,
	[UsuarioRegistro] [varchar](9) NOT NULL,
	[FechaRegistro] [date] NOT NULL,
	[UsuarioModifico] [varchar](9) NULL,
	[FechaModificacion] [date] NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[RolesMovimientos] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolesMovimientos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Estatus] [bit] NOT NULL,
	[IdMovimiento] [int] NOT NULL,
	[UsuarioMovimiento] [varchar](9) NOT NULL,
	[FechaMovimiento] [datetime] NOT NULL,
 CONSTRAINT [PK__RolesMov__C15B915337B9A019] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[SeccionesPermisos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SeccionesPermisos](
	[IdPermiso] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[IdMenu] [int] NOT NULL,
	[IdSubMenu] [int] NOT NULL,
	[Ver] [bit] NOT NULL,
	[Editar] [bit] NOT NULL,
 CONSTRAINT [PK_SeccionesPermisos] PRIMARY KEY CLUSTERED 
(
	[IdPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[UsuarioCampusSede]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioCampusSede](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[ClaveCampus] [varchar](10) NULL,
	[ClaveSede] [varchar](10) NULL,
	[Estatus] [bit] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioRegistro] [varchar](9) NULL,
	[UsuarioModificacion] [varchar](9) NULL,
	[IdHistorial] [int] NULL,
 CONSTRAINT [PK_UsuarioCampusSede] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[UsuarioMovimientos]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioMovimientos](
	[Id_Registro_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[Usuario_Creado] [varchar](15) NOT NULL,
	[Nivel] [varchar](15) NULL,
	[Rol_Asignado] [varchar](25) NULL,
	[Id_Usuario_Registra] [int] NULL,
	[Id_Usuario_Modifica] [int] NULL,
	[Fecha_Registro] [date] NULL,
	[Fecha_Modificacion] [date] NULL,
	[Id_Usuario_Modifico] [int] NULL,
	[Id_Movimiento] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Registro_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[UsuarioNivel]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioNivel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[ClaveNivel] [varchar](10) NULL,
	[Estatus] [bit] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioRegistro] [varchar](9) NULL,
	[UsuarioModificacion] [varchar](9) NULL,
	[IdHistorial] [int] NULL,
 CONSTRAINT [PK_UsuarioNivel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[UsuarioRoles]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdRol] [int] NOT NULL,
	[Estatus] [bit] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioRegistro] [varchar](9) NULL,
	[UsuarioModificacion] [varchar](9) NULL,
	[IdHistorial] [int] NULL,
 CONSTRAINT [PK_UsuarioRol] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT('Table [dbo].[Usuarios]')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[NumeroNomina] [varchar](20) NULL,
	[Nombre] [varchar](250) NULL,
	[Correo] [varchar](250) NULL,
	[Estatus] [int] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaModificacion] [datetime] NULL,
	[IdHistorial] [int] NULL,
	[FechaEliminacion] [datetime] NULL,
	[UsuarioElimino] [varchar](9) NULL,
	[UsuarioRegistro] [varchar](9) NULL,
 CONSTRAINT [PK_UsuarioAdministrador] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RolesMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_RolesMovimientos_CatMovimientos] FOREIGN KEY([IdMovimiento])
REFERENCES [dbo].[CatMovimientos] ([Id_Movimiento])
GO
ALTER TABLE [dbo].[RolesMovimientos] CHECK CONSTRAINT [FK_RolesMovimientos_CatMovimientos]
GO
ALTER TABLE [dbo].[RolesMovimientos]  WITH CHECK ADD  CONSTRAINT [FK_RolesMovimientos_Roles] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[RolesMovimientos] CHECK CONSTRAINT [FK_RolesMovimientos_Roles]
GO
ALTER TABLE [dbo].[SeccionesPermisos]  WITH CHECK ADD  CONSTRAINT [FK_SeccionesPermisos_Menus] FOREIGN KEY([IdMenu])
REFERENCES [dbo].[Menus] ([ID])
GO
ALTER TABLE [dbo].[SeccionesPermisos] CHECK CONSTRAINT [FK_SeccionesPermisos_Menus]
GO
ALTER TABLE [dbo].[SeccionesPermisos]  WITH CHECK ADD  CONSTRAINT [FK_SeccionesPermisos_Roles] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[SeccionesPermisos] CHECK CONSTRAINT [FK_SeccionesPermisos_Roles]
GO
ALTER TABLE [dbo].[UsuarioCampusSede]  WITH CHECK ADD  CONSTRAINT [FK_RelacionUsuarioCampus] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioCampusSede] CHECK CONSTRAINT [FK_RelacionUsuarioCampus]
GO
ALTER TABLE [dbo].[UsuarioNivel]  WITH CHECK ADD  CONSTRAINT [FK_RelacionUsuarioNivel] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioNivel] CHECK CONSTRAINT [FK_RelacionUsuarioNivel]
GO
ALTER TABLE [dbo].[UsuarioRoles]  WITH CHECK ADD  CONSTRAINT [FK_RelacionRol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[UsuarioRoles] CHECK CONSTRAINT [FK_RelacionRol]
GO
ALTER TABLE [dbo].[UsuarioRoles]  WITH CHECK ADD  CONSTRAINT [FK_RelacionUsuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioRoles] CHECK CONSTRAINT [FK_RelacionUsuario]
GO

PRINT('INSERT SubMenus')
PRINT('INSERT Usuarios')
INSERT INTO [dbo].[SubMenus]
           ([NOMBRE]
           ,[IDMENU]
           ,[PATH]
           ,[ICONO]
           ,[ACTIVO])
     VALUES
           ('Usuarios'
           ,7
           ,'usuarios'
           ,'bi bi-person'
           ,1)
GO

GO
PRINT('INSERT Roles')
INSERT INTO [dbo].[SubMenus]
           ([NOMBRE]
           ,[IDMENU]
           ,[PATH]
           ,[ICONO]
           ,[ACTIVO])
     VALUES
           ('Roles'
           ,7
           ,'roles'
           ,'bi-file-check'
           ,1)
GO

GO
PRINT('INSERT CatMovimientos')
PRINT('INSERT ALTA')
INSERT INTO [dbo].[CatMovimientos]
           ([Movimiento])
     VALUES
           ('ALTA')
GO 

GO
PRINT('INSERT BAJA')
INSERT INTO [dbo].[CatMovimientos]
           ([Movimiento])
     VALUES
           ('BAJA')
GO 

GO
PRINT('INSERT MODIFICACION')
INSERT INTO [dbo].[CatMovimientos]
           ([Movimiento])
     VALUES
           ('MODIFICACION')
GO 

GO
PRINT('INSERT CAMBIO DE ESTATUS')
INSERT INTO [dbo].[CatMovimientos]
           ([Movimiento])
     VALUES
           ('CAMBIO DE ESTATUS')
GO

GO
PRINT('UPDATE Menus')
PRINT('UPDATE Por Categoria')
UPDATE [dbo].[Menus]
   SET [PATH] = 'porcategoria'
 WHERE 
	ID = 3
GO

GO
PRINT('UPDATE Roles')
UPDATE [dbo].[Menus]
   SET [NOMBRE] = 'Admin. de Roles'
      ,[PATH] = ''
WHERE [ID] = 7
GO

GO
PRINT('UPDATE Submenu Categorias por Requisitos')
UPDATE [dbo].[SubMenus]
   SET [IDMENU] = 5
 WHERE 
	[IDMENU] = 3
GO
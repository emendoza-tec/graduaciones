USE [ReqGraduaciones]
GO
PRINT('Types')
PRINT('Type [dbo].[UsuarioCampusSedeType]')
CREATE TYPE [dbo].[UsuarioCampusSedeType] AS TABLE(
	[IdUsuario] [int] NOT NULL,
	[ClaveCampus] [varchar](3) NULL,
	[ClaveSede] [varchar](3) NULL
)
GO
PRINT('Type  [dbo].[UsuarioNivelType]')
CREATE TYPE [dbo].[UsuarioNivelType] AS TABLE(
	[IdUsuario] [int] NOT NULL,
	[ClaveNivel] [varchar](2) NULL
)
GO
PRINT('Type [dbo].[UsuarioRolType]')
CREATE TYPE [dbo].[UsuarioRolType] AS TABLE(
	[IdUsuario] [int] NOT NULL,
	[IdRol] [int] NULL
)
GO

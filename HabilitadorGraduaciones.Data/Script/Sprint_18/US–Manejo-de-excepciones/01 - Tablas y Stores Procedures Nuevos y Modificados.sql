USE [ReqGraduaciones]
GO
PRINT(' Table [dbo].[LogExcepciones] ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogExcepciones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorControlado] [bit] NOT NULL,
	[MensajeUsuario] [varchar](500) NULL,
	[MensajeExcepcion] [varchar](max) NULL,
	[StackTrace] [varchar](max) NULL,
	[InnerException] [varchar](max) NULL,
	[HttpStatusCode] [varchar](50) NULL,
	[UsuarioAlta] [varchar](9) NULL,
	[FechaAlta] [date] NOT NULL,
 CONSTRAINT [PK_LogExcepciones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
PRINT('  StoredProcedure [dbo].[spLogExcepciones_InsertaExcepcion]  ')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Juan Carlos Bernadac Delfin 
-- Create date: 25-07-2023
-- Description:	Inserta registro en la Tabla LogExcepciones 
-- =============================================
CREATE PROCEDURE [dbo].[spLogExcepciones_InsertaExcepcion]
	@pErrorControlado			bit,
    @pMensajeUsuario			varchar(500),
    @pMensajeExcepcion			varchar(max),
    @pStackTrace				varchar(max),
    @pInnerException			varchar(max),
    @pHttpStatusCode			varchar(50),
    @pUsuarioAlta				varchar(9)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[LogExcepciones]
           ([ErrorControlado]
           ,[MensajeUsuario]
           ,[MensajeExcepcion]
           ,[StackTrace]
           ,[InnerException]
           ,[HttpStatusCode]
           ,[UsuarioAlta]
           ,[FechaAlta])
     VALUES
           (@pErrorControlado
           ,@pMensajeUsuario
           ,@pMensajeExcepcion
           ,@pStackTrace
           ,@pInnerException
           ,@pHttpStatusCode
           ,@pUsuarioAlta
           ,GETDATE())
END
GO

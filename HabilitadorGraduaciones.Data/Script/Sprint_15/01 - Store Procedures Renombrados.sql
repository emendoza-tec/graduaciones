
PRINT('Cambio de Nombre de SP spConteoSolicitudesDatosPersonalesPendientes')
EXEC sp_rename 'spConteoSolicitudesDatosPersonalesPendientes', 'spSolicitudCambioDatosPersonales_ObtenerPendientes';

PRINT('Cambio de Nombre de SP spCorreoSolicitudDatosPersonales')
EXEC sp_rename 'spCorreoSolicitudDatosPersonales', 'spCorreoSolicitudDatosPersonales_ObtenerCorreoSolicitudDatosPersonales';

PRINT('Cambio de Nombre de SP spDetalleSolicitudDatosPersonales')
EXEC sp_rename 'spDetalleSolicitudDatosPersonales', 'spDetalleSolicitudDatosPersonales_ObtenerDatos';

PRINT('Cambio de Nombre de SP spEstatusSolicitudDatosPersonales')
EXEC sp_rename 'spEstatusSolicitudDatosPersonales', 'spEstatusSolicitudDatosPersonales_ObtenerEstatus';

PRINT('Cambio de Nombre de SP spInsertarDetalleSolicitudDatosPersonales')
EXEC sp_rename 'spInsertarDetalleSolicitudDatosPersonales', 'spDetalleSolicitudDatosPersonales_Insertar';

PRINT('Cambio de Nombre de SP spInsertarSolicitudesDatosPersonales')
EXEC sp_rename 'spInsertarSolicitudesDatosPersonales', 'spSolicitudesDatosPersonales_Insertar';

PRINT('Cambio de Nombre de SP spSolicitudesDatosPersonales')
EXEC sp_rename 'spSolicitudesDatosPersonales', 'spSolicitudesDatosPersonales_Obtener';

PRINT('Cambio de Nombre de SP spSolicitudesDatosPersonalesPendientes')
EXEC sp_rename 'spSolicitudesDatosPersonalesPendientes', 'spSolicitudesDatosPersonales_ObtenerPendientes';

PRINT('Cambio de Nombre de SP spModificarEstatusSolicitudDatosPersonales')
EXEC sp_rename 'spModificarEstatusSolicitudDatosPersonales', 'spSolicitudCambioDatosPersonales_ActualizarEstatus';


PRINT('Cambio de Nombre de SP spTarjeta_ObtenDetalleMultilenguaje')
EXEC sp_rename 'spTarjeta_ObtenDetalleMultilenguaje', 'spTarjeta_ObtenerDetalleMultilenguaje';

PRINT('Cambio de Nombre de SP spExpediente_ObtenDocumentosMultilenguaje')
EXEC sp_rename 'spExpediente_ObtenDocumentosMultilenguaje', 'spExpedientesDocumentos_ObtenerDocumentosMultilenguaje';


PRINT('Cambio de Nombre de SP spLog_InsertarLogEnterado')
EXEC sp_rename 'spLog_InsertarLogEnterado', 'spLogEnteradoCreditosInsuficientes_Insertar'; 

PRINT('Cambio de Nombre de SP spNotificaciones_InsertNotificacion')
EXEC sp_rename 'spNotificaciones_InsertNotificacion', 'spNotificaciones_InsertarNotificacion'; 

PRINT('Cambio de Nombre de SP spNotificaciones_InsertNotificacionCorreo')
EXEC sp_rename 'spNotificaciones_InsertNotificacionCorreo', 'spNotificaciones_InsertarNotificacionCorreo';

PRINT('Cambio de Nombre de SP spPeriodos_InsertarPeriodo')
EXEC sp_rename 'spPeriodos_InsertarPeriodo', 'spPeriodos_Insertar'; 

PRINT('Cambio de Nombre de SP spPeriodos_ObtenerCarreraClinicas')
EXEC sp_rename 'spPeriodos_ObtenerCarreraClinicas', 'spCatCarrerasClinicaPeriodos_ObtenerPorCarrera'; 

PRINT('Cambio de Nombre de SP spPeriodos_ObtenerCarrerasClinicas')
EXEC sp_rename 'spPeriodos_ObtenerCarrerasClinicas', 'spCatCarrerasClinicaPeriodos_ObtenerTodos'; 

PRINT('Cambio de Nombre de SP spPeriodos_ObtenerPeridos')
EXEC sp_rename 'spPeriodos_ObtenerPeridos', 'spPeriodos_ObtenerPeriodos'; 

PRINT('Cambio de Nombre de SP spPeriodos_ObtenerPeriodosGraduacion')
EXEC sp_rename 'spPeriodos_ObtenerPeriodosGraduacion', 'spCatPriodosGraduacion_ObtenerTodos';

PRINT('Cambio de Nombre de SP spAccesoPorNomina')
EXEC sp_rename 'spAccesoPorNomina', 'spAccesoNomina_ObtenerNomina';

PRINT('Cambio de Nombre de SP spCalendarios_InsertCalendarios')
EXEC sp_rename 'spCalendarios_InsertCalendarios', 'spCalendarios_InsertarCalendario';

PRINT('Cambio de Nombre de SP spPrograma_NivelInglesAdmin')
EXEC sp_rename 'spPrograma_NivelInglesAdmin', 'spRequisitoInglesGuardado_Insertar';

PRINT('Cambio de Nombre de SP spSecciones_InsertaroModificarPermisos')
EXEC sp_rename 'spSecciones_InsertaroModificarPermisos', 'spSecciones_InsertarActualizarPermisos';

PRINT('Cambio de Nombre de SP spAccesoPorNomina')
EXEC sp_rename 'spTipoExamenMultilenguaje', 'spTipoExamen_ObtenerMultilenguaje';

PRINT('Cambio de Nombre de SP spSemanasTec_GetClaveYNivel')
EXEC sp_rename 'spSemanasTec_GetClaveYNivel', 'spSemanasTec_ObtenerClaveYNivel';

PRINT('Cambio de Nombre de SP spObtenerSessionPorMatricula')
EXEC sp_rename 'spObtenerSessionPorMatricula', 'spSesiones_ObtenerSesion';

PRINT('Cambio de Nombre de SP spObtenerTipoExamenPorClaveCarrera')
EXEC sp_rename 'spObtenerTipoExamenPorClaveCarrera', 'spCarrerasConRequisitoExamen_ObtenerPorClaveCarrera';

PRINT('Cambio de Nombre de SP spModificaSession')
EXEC sp_rename 'spModificaSession', 'spSesiones_ActualizarTokens';

PRINT('Cambio de Nombre de SP spMenu_ObtenMenuPadre')
EXEC sp_rename 'spMenu_ObtenMenuPadre', 'spMenus_ObtenerMenu';

PRINT('Cambio de Nombre de SP spMenu_ObtenMenuHijo')
EXEC sp_rename 'spMenu_ObtenMenuHijo', 'spSubMenus_ObtenerMenu';

PRINT('Cambio de Nombre de SP spInsertaSession')
EXEC sp_rename 'spInsertaSession', 'spSesiones_Insertar';


PRINT('Cambio de Nombre de SP spInsertaCampusCeremoniaGraduacion')
EXEC sp_rename 'spInsertaCampusCeremoniaGraduacion', 'spCampusCeremoniaGraduacion_Insertar';

PRINT('Cambio de Nombre de SP spExpediente_ObtenDocumentos')
EXEC sp_rename 'spExpediente_ObtenDocumentos', 'spExpedientesDocumentos_ObtenDocumentos';

PRINT('Cambio de Nombre de SP spExpediente_ConsultaCometariosExpedientes')
EXEC sp_rename 'spExpediente_ConsultaCometariosExpedientes', 'spExpedientes_ObtenerComentariosExpedientes';

PRINT('Cambio de Nombre de SP spExpediente_ByAlumno')
EXEC sp_rename 'spExpediente_ByAlumno', 'spExpedientes_ObtenerPorAlumno';

PRINT('Cambio de Nombre de SP spExamenIntegradorPorMatricula')
EXEC sp_rename 'spExamenIntegradorPorMatricula', 'spExamenIntegrador_ObtenerPorMatricula';

PRINT('Cambio de Nombre de SP spExamenIntegradorInserta')
EXEC sp_rename 'spExamenIntegradorInserta', 'spExamenIntegrador_InsertarActualizar';

PRINT('Cambio de Nombre de SP spExamenIntegrador_ObtenExamenes')
EXEC sp_rename 'spExamenIntegrador_ObtenExamenes', 'spExamenIntegrador_ObtenExamenesActivos';
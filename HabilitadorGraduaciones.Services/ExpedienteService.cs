using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Services.ProcesaExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Services
{
    public class ExpedienteService : IExpedienteService
    {
        public IConfiguration Configuration { get; }
        private const string Incompleto = "INCOMPLETO";
        private const string EnRevision = "EN REVISIÓN";
        private const string Completo = "COMPLETO";
        private readonly List<ExpedienteEntity> _expedientesNuevos = new List<ExpedienteEntity>();
        private readonly List<ExpedienteEntity> _expedientesaModificar = new List<ExpedienteEntity>();
        private readonly List<ExpedienteEntity> _expedientesIncompletoSinDetalle = new List<ExpedienteEntity>();
        private readonly List<ExpedienteEntity> _expedientesaCompleto = new List<ExpedienteEntity>();
        private readonly List<ExpedienteEntity> _expedientesdeCompleto = new List<ExpedienteEntity>();
        private readonly List<ExpedienteEntity> _expedientesErrorFiltro = new List<ExpedienteEntity>();
        private readonly IUsuarioService _usuarioService;
        private readonly IExpedienteRepository _expedienteRepository;
        private readonly INotificacionesService _notificacionesService;
        public ExpedienteService(IConfiguration configuration, IUsuarioService usuarioService, IExpedienteRepository expedienteRepository, INotificacionesService notificacionesService)
        {
            Configuration = configuration;
            _usuarioService = usuarioService;
            _expedienteRepository = expedienteRepository;
            _notificacionesService = notificacionesService;
        }

        public async Task<List<ExpedienteEntity>> GetExpedientes(int idUsuario)
        {
            return await _expedienteRepository.GetExpedientes(idUsuario);
        }
        public async Task<ExpedienteEntity> GetByAlumno(string matricula)
        {
            return await _expedienteRepository.GetByAlumno(matricula);
        }
        public async Task<List<ExpedienteEntity>> ConsultarComentarios(string matricula)
        {
            return await _expedienteRepository.ConsultarComentarios(matricula);
        }

        public async Task<ProcesosExpedienteEntity> ProcesaExcel(IFormFile archivo, int idUsuario)
        {
            var expedientesExcel = await ProcesaExpediente.ObtenExpedientes(archivo);
            var expedientesBD = await GetExpedientes(idUsuario);
            var procesosExpediente = new ProcesosExpedienteEntity();

            foreach (var expediente in expedientesExcel)
            {
                if (expediente.Estatus.ToUpper().Trim() == Incompleto && string.IsNullOrEmpty(expediente.Detalle.Trim()))
                {
                    _expedientesIncompletoSinDetalle.Add(expediente);
                }

                //Registros Nuevos
                if (!expedientesBD.Exists(f => f.Matricula == expediente.Matricula))
                {
                    await VerificaAlumna(idUsuario, expediente.Matricula, expediente);
                }

                //Registros a Modificar
                if (expedientesBD.Exists(f => f.Matricula == expediente.Matricula && (!(f.Estatus.Trim().ToUpper().Equals(expediente.Estatus.Trim().ToUpper())) || !(f.Detalle.Trim().ToUpper().Equals(expediente.Detalle.Trim().ToUpper())))))
                {
                    _expedientesaModificar.Add(expediente);
                    RegistrosModificadosDeEstatus(expedientesBD, expediente);
                }
            }

            procesosExpediente.ExpedienteNuevos = _expedientesNuevos;
            procesosExpediente.ExpedienteAtualizados = _expedientesaModificar;
            procesosExpediente.ExpedienteCambioaCompleto = _expedientesaCompleto;
            procesosExpediente.ExpedienteCambiodeCompleto = _expedientesdeCompleto;
            procesosExpediente.ExpedienteIncompletoSinDetalle = _expedientesIncompletoSinDetalle;
            procesosExpediente.ExpedienteErrorFiltro = _expedientesErrorFiltro;
            return procesosExpediente;
        }

        private async Task VerificaAlumna(int idUsuario, string matricula, ExpedienteEntity expediente)
        {
            ExisteAlumnoDto existeDto = await ExisteAlumno(idUsuario, matricula);
            if (existeDto.Existe >= 1)
            {
                _expedientesNuevos.Add(expediente);
            }
            else
            {
                _expedientesErrorFiltro.Add(expediente);
            }
        }

        public async Task<HttpResponseMessage> GuardaExpedientes(ArchivoEntity archivo, int idUsuario)
        {
            var expedientesExcel = await ProcesaExpediente.ObtenExpedientes(archivo.ArchivoRecibido);
            var expedientesBD = await GetExpedientes(idUsuario);
            var expedientesAInsertar = new List<ExpedienteEntity>();
            var response = new HttpResponseMessage();
            foreach (var expediente in expedientesExcel)
            {
                var existExpediente = expedientesBD.Exists(f => f.Matricula != expediente.Matricula);
                if (!existExpediente)
                {
                    expedientesAInsertar.Add(expediente);
                }
                var expedienteAModificar = expedientesBD.Exists(f => f.Matricula == expediente.Matricula && f.Estatus.Trim().ToUpper().Equals(expediente.Estatus.Trim().ToUpper()) && f.Detalle.Trim().ToUpper().Equals(expediente.Detalle.Trim().ToUpper()));

                if (!expedienteAModificar)
                {
                    expediente.isModificarAlumno = true;
                    expedientesAInsertar.Add(expediente);
                }
                await EnviarCorreoComentarioNuevo(expedientesBD, expediente);
            }

            if (expedientesAInsertar.Count == 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                return response;
            }
            await _expedienteRepository.GuardaExpedientes(expedientesAInsertar, archivo.UsuarioApplicacion);
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }

        private void RegistrosModificadosDeEstatus(List<ExpedienteEntity> expedientesBD, ExpedienteEntity expediente)
        {
            switch (expediente.Estatus.ToUpper())
            {
                case Incompleto:
                    {
                        if (expedientesBD.Exists(f => f.Matricula == expediente.Matricula && f.Estatus.Trim().ToUpper().Equals(Completo)))
                        {
                            _expedientesdeCompleto.Add(expediente);
                        }
                        break;
                    }
                case EnRevision:
                    {
                        if (expedientesBD.Exists(f => f.Matricula == expediente.Matricula && f.Estatus.Trim().ToUpper().Equals(Completo)))
                        {
                            _expedientesdeCompleto.Add(expediente);
                        }
                        break;
                    }
                case Completo:
                    {
                        if (expedientesBD.Exists(f => f.Matricula == expediente.Matricula && f.Estatus.Trim().ToUpper() != Completo))
                        {
                            _expedientesaCompleto.Add(expediente);
                        }
                        break;
                    }
            }
        }
        private async Task EnviarCorreoComentarioNuevo(List<ExpedienteEntity> expedientesBD, ExpedienteEntity expediente)
        {
            //Registros con comentarios nuevos para envio de correo 
            if (expedientesBD.Exists(f => f.Matricula == expediente.Matricula && !(f.Detalle.Trim().ToUpper().Equals(expediente.Detalle.Trim().ToUpper()))))
            {
                var usuario = await _usuarioService.ObtenerUsuario(expediente.Matricula);
                if (string.IsNullOrEmpty(usuario.Correo) && string.IsNullOrEmpty(expediente.Detalle.ToUpper()))
                {
                    _ = _notificacionesService.EnviarCorreo(new CorreoDto
                    {
                        Destinatario = usuario.Correo,
                        Asunto = "Actualizacion de comentarios en tu expediente",
                        Cuerpo = "Se agregó el siguiente comentario en tu sección de expediente: <br/>" + expediente.Detalle
                    });
                }
            }
        }

        public async Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula)
        {
            return await _expedienteRepository.ExisteAlumno(idUsuario, matricula);
        }
    }
}
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Services.ProcesaExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabilitadorGraduaciones.Services
{
    public class ExamenIntegradorService : IExamenIntegradorService
    {
        private const string SiCumple = "SC";
        private const string NoCumple = "NC";
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorNuevos = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorAModificar = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorNcAScYNP = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorScANcYNP = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorErrorFiltro = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorFormatoInvalido = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorNumeroInValido = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorAnioInValido = new List<ExamenIntegradorEntity>();
        private readonly List<ExamenIntegradorEntity> _examenesIntegradorPeriodoInValido = new List<ExamenIntegradorEntity>();

        private readonly IExamenIntegradorRepository _examenIntegradorRepository;
        public IConfiguration Configuration { get; }
        private readonly IPeriodosRepository _periodosData;

        public ExamenIntegradorService(IExamenIntegradorRepository examenIntegradorRepository, IConfiguration configuration, IPeriodosRepository periodosRepository)
        {
            _examenIntegradorRepository = examenIntegradorRepository;
            Configuration = configuration;
            _periodosData = periodosRepository;
        }

        public async Task<List<ExamenIntegradorEntity>> GetExamenesIntegrador(int idUsuario)
        {
            return await _examenIntegradorRepository.GetExamenesIntegrador(idUsuario);
        }

        public async Task<ProcesosExamenIntegradorEntity> ProcesaExcel(IFormFile archivo, int idUsuario)
        {
            var procesosExamenIntegrador = new ProcesosExamenIntegradorEntity();
            var examenesIntegradorExcel = await ProcesaExamenIntegrador.ObtenExamenesIntegrador(archivo);

            await ValidaPeriodoGraduacion(examenesIntegradorExcel);

            procesosExamenIntegrador = await ProcesaExamenesIntegrador(examenesIntegradorExcel, procesosExamenIntegrador, idUsuario);

            procesosExamenIntegrador.ExamenesIntegradorFormatoInvalido = _examenesIntegradorFormatoInvalido;
            procesosExamenIntegrador.ExamenesIntegradorNumeroInvalido = _examenesIntegradorNumeroInValido;
            procesosExamenIntegrador.ExamenesIntegradorAnioInvalido = _examenesIntegradorAnioInValido;
            procesosExamenIntegrador.ExamenesIntegradorPeriodoInvalido = _examenesIntegradorPeriodoInValido;

            return procesosExamenIntegrador;
        }

        public async Task<HttpResponseMessage> GuardaExamenesIntegrador(ArchivoEntity archivo, int idUsuario)
        {
            var examenesIntegradorExcel = await ProcesaExamenIntegrador.ObtenExamenesIntegrador(archivo.ArchivoRecibido);
            var examenesIntegradorBd = await GetExamenesIntegrador(idUsuario);
            var examenesIntegradorAInsertar = new List<ExamenIntegradorEntity>();
            var response = new HttpResponseMessage();

            foreach (var examenIntegradorExcel in examenesIntegradorExcel)
            {
                var existExamenIntegrador = examenesIntegradorBd.Exists(f => f.Matricula == examenIntegradorExcel.Matricula);
                if (!existExamenIntegrador)
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(examenIntegradorExcel.FechaExamen, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        examenIntegradorExcel.FechaExamenDate = dt;
                    }
                    examenesIntegradorAInsertar.Add(examenIntegradorExcel);
                }

                foreach (var examenIntegradorBd in examenesIntegradorBd)
                {
                    if (examenIntegradorBd.Matricula == examenIntegradorExcel.Matricula)
                    {
                        if (examenIntegradorBd.PeriodoGraduacion != examenIntegradorExcel.PeriodoGraduacion || examenIntegradorBd.Nivel != examenIntegradorExcel.Nivel || examenIntegradorBd.NombreRequisito != examenIntegradorExcel.NombreRequisito || examenIntegradorBd.Estatus != examenIntegradorExcel.Estatus || examenIntegradorBd.FechaExamen != examenIntegradorExcel.FechaExamen)
                        {
                            DateTime dt;
                            if (DateTime.TryParseExact(examenIntegradorExcel.FechaExamen, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                examenIntegradorExcel.FechaExamenDate = dt;
                            }
                            examenIntegradorExcel.UpdateFlag = true;
                            examenesIntegradorAInsertar.Add(examenIntegradorExcel);
                        }
                    }
                }
            }
            if (examenesIntegradorAInsertar.Count == 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                return response;
            }

            await _examenIntegradorRepository.GuardaExamenesIntegrador(examenesIntegradorAInsertar, archivo.UsuarioApplicacion);
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        private async Task ValidaPeriodoGraduacion(List<ExamenIntegradorEntity> examenesIntegrador)
        {

            foreach (var ei in examenesIntegrador)
            {
                if (ei.PeriodoGraduacion.Length == 0 || ei.PeriodoGraduacion.Length != 6)
                {
                    _examenesIntegradorFormatoInvalido.Add(ei);
                }
                else
                {
                    var isPeriodoNumber = int.TryParse(ei.PeriodoGraduacion, out int periodoGraduacionInt);
                    if (isPeriodoNumber)
                    {
                        var yearPeriodo = ei.PeriodoGraduacion.Substring(0, 4);
                        var tipoPeriodo = ei.PeriodoGraduacion.Substring(4, 2);

                        var yearPeriodoInt = int.Parse(yearPeriodo);
                        var tipoPeriodoInt = int.Parse(tipoPeriodo);

                        if (yearPeriodoInt < DateTime.Now.Year)
                        {
                            _examenesIntegradorAnioInValido.Add(ei);
                        }
                        else
                        {
                            var periodosGraduacion = await _periodosData.GetPeriodosGraduacion();

                            if (!periodosGraduacion.Exists(f => f.PeriodoGraduacion == tipoPeriodoInt))
                            {
                                _examenesIntegradorPeriodoInValido.Add(ei);
                            }

                        }
                    }
                    else
                    {
                        _examenesIntegradorNumeroInValido.Add(ei);
                    }
                }
            }
        }

        private async Task<ProcesosExamenIntegradorEntity> ProcesaExamenesIntegrador(List<ExamenIntegradorEntity> examenesIntegradorExcel, ProcesosExamenIntegradorEntity examenesProcesados, int idUsuario)
        {
            var examenesIntegradorBd = await GetExamenesIntegrador(idUsuario);
            var examenesFechaInvalida = new List<ExamenIntegradorEntity>();

            foreach (var examenIntegradorExcel in examenesIntegradorExcel)
            {
                if (examenIntegradorExcel.FechaExamen != string.Empty)
                {
                    bool isDateValid = Regex.IsMatch(examenIntegradorExcel.FechaExamen, @"^([0-2][0-9]|3[0-1])(\/|-)(0[1-9]|1[0-2])\2(\d{4})$", RegexOptions.None, TimeSpan.FromMilliseconds(5));

                    if (isDateValid)
                    {
                        DateTime dt;
                        if (DateTime.TryParseExact(examenIntegradorExcel.FechaExamen, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        {
                            examenIntegradorExcel.FechaExamenDate = dt;
                        }
                        else
                        {
                            examenesFechaInvalida.Add(examenIntegradorExcel);
                        }
                    }
                    else
                    {
                        examenesFechaInvalida.Add(examenIntegradorExcel);
                    }
                }
                else if (examenIntegradorExcel.FechaExamen == string.Empty && examenIntegradorExcel.Estatus == SiCumple)
                {
                    examenesFechaInvalida.Add(examenIntegradorExcel);
                }

                await AgregaExamenesNuevos(examenIntegradorExcel, examenesIntegradorBd, idUsuario);

                foreach (var examenIntegradorBd in examenesIntegradorBd)
                {
                    if (examenIntegradorBd.Matricula == examenIntegradorExcel.Matricula)
                    {
                        AgregarExamenesIntegradorAModificar(examenIntegradorBd, examenIntegradorExcel);

                        AgregaExamenesIntegradorEstatusDiferente(examenIntegradorBd, examenIntegradorExcel);
                    }
                }
            }
            examenesProcesados.ExamenesIntegradorNuevos = _examenesIntegradorNuevos;
            examenesProcesados.ExamenesIntegradorAModificar = _examenesIntegradorAModificar;
            examenesProcesados.ExamenesIntegradorNcAScYNP = _examenesIntegradorNcAScYNP;
            examenesProcesados.ExamenesIntegradorScANcYNP = _examenesIntegradorScANcYNP;
            examenesProcesados.ExamenesIntegradorFechaInvalida = examenesFechaInvalida;
            examenesProcesados.ExamenesIntegradorErrorFiltro = _examenesIntegradorErrorFiltro;

            return examenesProcesados;
        }

        private void AgregaExamenesIntegradorEstatusDiferente(ExamenIntegradorEntity examenIntegradorBd, ExamenIntegradorEntity examenIntegradorExcel)
        {
            switch (examenIntegradorBd.Estatus)
            {
                case SiCumple:
                    {
                        if (examenIntegradorBd.Estatus != examenIntegradorExcel.Estatus)
                        {
                            _examenesIntegradorScANcYNP.Add(examenIntegradorExcel);
                        }
                        break;
                    }
                case NoCumple:
                    {
                        if (examenIntegradorBd.Estatus != examenIntegradorExcel.Estatus)
                        {
                            _examenesIntegradorNcAScYNP.Add(examenIntegradorExcel);
                        }
                        break;
                    }
            }
        }

        private void AgregarExamenesIntegradorAModificar(ExamenIntegradorEntity examenIntegradorBd, ExamenIntegradorEntity examenIntegradorExcel)
        {
            if (examenIntegradorBd.PeriodoGraduacion != examenIntegradorExcel.PeriodoGraduacion || examenIntegradorBd.Nivel != examenIntegradorExcel.Nivel || examenIntegradorBd.NombreRequisito != examenIntegradorExcel.NombreRequisito || examenIntegradorBd.Estatus != examenIntegradorExcel.Estatus || examenIntegradorBd.FechaExamenDate.ToShortDateString() != examenIntegradorExcel.FechaExamenDate.ToShortDateString())
            {
                _examenesIntegradorAModificar.Add(examenIntegradorExcel);
            }
        }

        private async Task AgregaExamenesNuevos(ExamenIntegradorEntity examenesIntegradorExcel, List<ExamenIntegradorEntity> examenesIntegradorBd, int idUsuario)
        {
            if (!examenesIntegradorBd.Exists(f => f.Matricula == examenesIntegradorExcel.Matricula))
            {
               await VerificaAlumno(idUsuario, examenesIntegradorExcel.Matricula, examenesIntegradorExcel);
            }
        }

        public async Task<ExamenIntegradorEntity> GetMatricula(string matricula)
        {
            return await _examenIntegradorRepository.GetMatricula(matricula);
        }

        public async Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula)
        {
            return await _examenIntegradorRepository.ExisteAlumno(idUsuario, matricula);
        }

        private async Task VerificaAlumno(int idUsuario, string matricula, ExamenIntegradorEntity examenIntegrador)
        {
             ExisteAlumnoDto existeDto = await ExisteAlumno(idUsuario, matricula);
            if (existeDto.Existe >= 1)
            {
                _examenesIntegradorNuevos.Add(examenIntegrador);
            }
            else
            {
                _examenesIntegradorErrorFiltro.Add(examenIntegrador);
            }
        }
    }
}
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HabilitadorGraduaciones.Test.Controllers
{
    public class RolesControllerTest
    {
        private readonly Mock<IRolesService> _rolesService;
        private readonly RolesController _rolesController;

        public RolesControllerTest()
        {
            _rolesService = new Mock<IRolesService>();
            _rolesController = new RolesController(_rolesService.Object);
        }

        [Fact]
        public async Task ObtenRoles_Success()
        {
            var list = new List<RolesEntity>()
            {
                 new RolesEntity()
                {
                    IdRol = 1,
                    Descripcion = "Prueba API",
                    Estatus = true,
                    TotalUsuarios = 1,
                    UsuarioRegistro = "L03533706",
                    FechaRegistro = Convert.ToDateTime("2023-05-24"),
                    UsuarioModifico = null,
                    FechaModificacion = Convert.ToDateTime("2023-05-24"),
                    Activo = true,
                    Permisos = null,
                    Result = true,
                    Error = null
                }
            };

            _rolesService.Setup(m => m.ObtenRoles()).Returns(Task.FromResult(list));

            var responseController = await _rolesController.ObtenRoles();
            var actual = responseController.Result as ObjectResult;
            var response = (List<RolesEntity>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<RolesEntity>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task ObtenRoles_Failure()
        {
            var list = new List<RolesEntity>();

            _rolesService.Setup(m => m.ObtenRoles()).Returns(Task.FromResult(list));

            var responseController = await _rolesController.ObtenRoles();
            var actual = responseController.Result as ObjectResult;
            var response = (List<RolesEntity>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<RolesEntity>>(actual.Value);
            Assert.False(response.Count > 0);
        }

        [Fact]
        public async Task ObtenerRolesPorId_Success()
        {
            var listaPermisos = new List<Permisos>()
            {
                new Permisos()
                {
                    IdPermiso = 1,
                    IdMenu = 5,
                    NombreMenu = "Requisitos",
                    IdSubMenu = 1,
                    NombreSubMenu = "Plan de Estudios",
                    Ver = true,
                    Editar = true,
                    Ok = false,
                    Error = null
                },
                new Permisos()
                    {
                        IdPermiso = 2,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 2,
                        NombreSubMenu = "Semanas Tec",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 4,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 3,
                        NombreSubMenu = "Servicio Social",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 22,
                        IdMenu = 1,
                        NombreMenu = "Situaciones por resolver",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 23,
                        IdMenu = 2,
                        NombreMenu = "Reporte completo",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 24,
                        IdMenu = 3,
                        NombreMenu = "Por categoria",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 25,
                        IdMenu = 4,
                        NombreMenu = "Excepciones",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 26,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 4,
                        NombreSubMenu = "Nivel de Inglés",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 27,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 5,
                        NombreSubMenu = "Competencias de Egreso",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 28,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 6,
                        NombreSubMenu = "Expediente",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 29,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 8,
                        NombreSubMenu = "Examen Integrador",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 30,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 12,
                        NombreSubMenu = "Usuarios",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 31,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 13,
                        NombreSubMenu = "Roles",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 32,
                        IdMenu = 8,
                        NombreMenu = "Ver lo que ve el alumno",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 33,
                        IdMenu = 9,
                        NombreMenu = "Envío de avisos",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 34,
                        IdMenu = 10,
                        NombreMenu = "Configuración",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 35,
                        IdMenu = 11,
                        NombreMenu = "Calendario",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    }

            };

            var expectedData = new RolesEntity()
            {
                IdRol = 1,
                Descripcion = "Prueba API",
                Estatus = true,
                TotalUsuarios = 0,
                UsuarioRegistro = null,
                FechaRegistro = Convert.ToDateTime("0001-01-01T00:00:00"),
                UsuarioModifico = null,
                FechaModificacion = Convert.ToDateTime("0001-01-01T00:00:00"),
                Activo = false,
                Permisos = listaPermisos,
                Result = true,
                Error = null
            };

            _rolesService.Setup(m => m.ObtenerRolesPorId(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _rolesController.ObtenerRolesPorId(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (RolesEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<RolesEntity>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ObtenerRolesPorId_Failure()
        {
            var expectedData = new RolesEntity();

            _rolesService.Setup(m => m.ObtenerRolesPorId(It.IsAny<int>())).Returns(Task.FromResult(expectedData));

            var responseController = await _rolesController.ObtenerRolesPorId(It.IsAny<int>());
            var actual = responseController.Result as ObjectResult;
            var response = (RolesEntity)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<RolesEntity>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ObtenerSecciones_Success()
        {
            var listaSecciones = new List<SeccionesPermisosDto>()
            {
              new SeccionesPermisosDto(){
                IdMenu = 1,
                NombreMenu = "Situaciones por resolver",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 2,
                NombreMenu = "Reporte completo",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 3,
                NombreMenu = "Por categoria",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 4,
                NombreMenu = "Excepciones",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 1,
                NombreSubMenu = "Plan de Estudios",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 2,
                NombreSubMenu = "Semanas Tec",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 3,
                NombreSubMenu = "Servicio Social",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 4,
                NombreSubMenu = "Nivel de Inglés",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 5,
                NombreSubMenu = "Competencias de Egreso",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 6,
                NombreSubMenu = "Expediente",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 5,
                NombreMenu = "Requisitos",
                IdSubMenu = 8,
                NombreSubMenu = "Examen Integrador",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 7,
                NombreMenu = "Admin. de Roles",
                IdSubMenu = 12,
                NombreSubMenu = "Usuarios",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 7,
                NombreMenu = "Admin. de Roles",
                IdSubMenu = 13,
                NombreSubMenu = "Roles",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 8,
                NombreMenu = "Ver lo que ve el alumno",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 9,
                NombreMenu = "Envío de avisos",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 10,
                NombreMenu = "Configuración",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              },
              new SeccionesPermisosDto(){
                IdMenu = 11,
                NombreMenu = "Calendario",
                IdSubMenu = 0,
                NombreSubMenu = "",
                Ver = false,
                Editar = false,
                Result = true
              }
            };

            _rolesService.Setup(m => m.ObtenerSecciones()).Returns(Task.FromResult(listaSecciones));

            var responseController = await _rolesController.ObtenerSecciones();
            var actual = responseController.Result as ObjectResult;
            var response = (List<SeccionesPermisosDto>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SeccionesPermisosDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task ObtenerSecciones_Failure()
        {
            var listaSecciones = new List<SeccionesPermisosDto>();

            _rolesService.Setup(m => m.ObtenerSecciones()).Returns(Task.FromResult(listaSecciones));

            var responseController = await _rolesController.ObtenerSecciones();
            var actual = responseController.Result as ObjectResult;
            var response = (List<SeccionesPermisosDto>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<SeccionesPermisosDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }

        [Fact]
        public async Task GuardaRol_Success()
        {
            var listaPermisos = new List<Permisos>()
            {
                new Permisos()
                {
                    IdPermiso = 1,
                    IdMenu = 5,
                    NombreMenu = "Requisitos",
                    IdSubMenu = 1,
                    NombreSubMenu = "Plan de Estudios",
                    Ver = true,
                    Editar = false
                },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 2,
                        NombreSubMenu = "Semanas Tec",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 3,
                        NombreSubMenu = "Servicio Social",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 1,
                        NombreMenu = "Situaciones por resolver",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = true,
                        Ok = false,
                        Error = null
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 2,
                        NombreMenu = "Reporte completo",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 3,
                        NombreMenu = "Por categoria",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 4,
                        NombreMenu = "Excepciones",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 4,
                        NombreSubMenu = "Nivel de Inglés",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 5,
                        NombreSubMenu = "Competencias de Egreso",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 6,
                        NombreSubMenu = "Expediente",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 8,
                        NombreSubMenu = "Examen Integrador",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 12,
                        NombreSubMenu = "Usuarios",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 13,
                        NombreSubMenu = "Roles",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 8,
                        NombreMenu = "Ver lo que ve el alumno",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 9,
                        NombreMenu = "Envío de avisos",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 10,
                        NombreMenu = "Configuración",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 11,
                        NombreMenu = "Calendario",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = true,
                        Editar = false
                    }

            };

            var rol = new RolesEntity()
            {
                IdRol = 3,
                Descripcion = "Prueba Tres",
                Estatus = true,
                TotalUsuarios = 0,
                UsuarioRegistro = "L03533706",
                FechaRegistro = Convert.ToDateTime("2023-05-24"),
                UsuarioModifico = null,
                FechaModificacion = Convert.ToDateTime("2023-05-24"),
                Activo = true,
                Permisos = listaPermisos,
                Result = true,
                Error = null
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.GuardaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.GuardaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;
            //Verificacion

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task GuardaRol_Failure()
        {
            var rol = new RolesEntity();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.GuardaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.GuardaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ModificaRol_Success()
        {
            var listaPermisos = new List<Permisos>()
            {
                new Permisos()
                {
                    IdPermiso = 1,
                    IdMenu = 5,
                    NombreMenu = "Requisitos",
                    IdSubMenu = 1,
                    NombreSubMenu = "Plan de Estudios",
                    Ver = false,
                    Editar = true
                },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 2,
                        NombreSubMenu = "Semanas Tec",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 3,
                        NombreSubMenu = "Servicio Social",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 1,
                        NombreMenu = "Situaciones por resolver",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 2,
                        NombreMenu = "Reporte completo",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 3,
                        NombreMenu = "Por categoria",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 4,
                        NombreMenu = "Excepciones",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 4,
                        NombreSubMenu = "Nivel de Inglés",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 5,
                        NombreSubMenu = "Competencias de Egreso",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 6,
                        NombreSubMenu = "Expediente",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 5,
                        NombreMenu = "Requisitos",
                        IdSubMenu = 8,
                        NombreSubMenu = "Examen Integrador",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 12,
                        NombreSubMenu = "Usuarios",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 7,
                        NombreMenu = "Admin. de Roles",
                        IdSubMenu = 13,
                        NombreSubMenu = "Roles",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 8,
                        NombreMenu = "Ver lo que ve el alumno",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 9,
                        NombreMenu = "Envío de avisos",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 10,
                        NombreMenu = "Configuración",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    },
                new Permisos()
                    {
                        IdPermiso = 0,
                        IdMenu = 11,
                        NombreMenu = "Calendario",
                        IdSubMenu = 0,
                        NombreSubMenu = "",
                        Ver = false,
                        Editar = true
                    }

            };

            var rol = new RolesEntity()
            {
                IdRol = 3,
                Descripcion = "Prueba Tres",
                Estatus = true,
                TotalUsuarios = 0,
                UsuarioRegistro = "L03533706",
                FechaRegistro = Convert.ToDateTime("2023-05-24"),
                UsuarioModifico = "L03533706",
                FechaModificacion = Convert.ToDateTime("2023-05-24"),
                Activo = true,
                Permisos = listaPermisos,
                Result = true,
                Error = null
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.ModificaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.ModificaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task ModificaRol_Failure()
        {
            var rol = new RolesEntity();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.ModificaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.ModificaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task CambiaEstatusRol_Success()
        {

            var rol = new RolesEntity()
            {
                IdRol = 3,
                Descripcion = "Prueba Tres",
                Estatus = false,
                TotalUsuarios = 0,
                UsuarioRegistro = "L03533706",
                FechaRegistro = Convert.ToDateTime("2023-05-24"),
                UsuarioModifico = "L03533706",
                FechaModificacion = Convert.ToDateTime("2023-05-24"),
                Activo = true,
                Permisos = null,
                Result = true,
                Error = null
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.CambiaEstatusRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.CambiaEstatusRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task CambiaEstatusRol_Failure()
        {
            var rol = new RolesEntity();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.CambiaEstatusRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.CambiaEstatusRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task EliminaRol_Success()
        {

            var rol = new RolesEntity()
            {
                IdRol = 3,
                Descripcion = "Prueba Tres",
                Estatus = true,
                TotalUsuarios = 0,
                UsuarioRegistro = "L03533706",
                FechaRegistro = Convert.ToDateTime("2023-05-24"),
                UsuarioModifico = "L03533706",
                FechaModificacion = Convert.ToDateTime("2023-05-24"),
                Activo = false,
                Permisos = null,
                Result = true,
                Error = null
            };

            BaseOutDto res = new BaseOutDto { Result = true, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.EliminaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.EliminaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task EliminaRol_Failure()
        {
            var rol = new RolesEntity();

            BaseOutDto res = new BaseOutDto { Result = false, ErrorMessage = string.Empty };
            _rolesService.Setup(m => m.EliminaRol(rol)).Returns(Task.FromResult(res));

            var resultado = await _rolesController.EliminaRol(rol);
            var actual = resultado.Result as ObjectResult;
            var response = (BaseOutDto)actual?.Value;

            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<BaseOutDto>(actual.Value);
            Assert.False(response.Result);
        }

        [Fact]
        public async Task ObtenerDescripcionRoles_Success()
        {
            var list = new List<RolesDto>()
            {
                 new RolesDto()
                {
                    IdRol = 1,
                    Descripcion = "Prueba API",
                    Result = true
                }
            };

            _rolesService.Setup(m => m.ObtenerDescripcionRoles()).Returns(Task.FromResult(list));

            var responseController = await _rolesController.ObtenerDescripcionRoles();
            var actual = responseController.Result as ObjectResult;
            var response = (List<RolesDto>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<RolesDto>>(actual.Value);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public async Task ObtenerDescripcionRoles_Failure()
        {
            var list = new List<RolesDto>();

            _rolesService.Setup(m => m.ObtenerDescripcionRoles()).Returns(Task.FromResult(list));

            var responseController = await _rolesController.ObtenerDescripcionRoles();
            var actual = responseController.Result as ObjectResult;
            var response = (List<RolesDto>)actual?.Value;


            actual.Equals(StatusCodes.Status200OK);
            Assert.NotNull(actual.Value);
            Assert.IsType<List<RolesDto>>(actual.Value);
            Assert.False(response.Count > 0);
        }
    }
}
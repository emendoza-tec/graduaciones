import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { catchError, forkJoin, of } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";
import { DatePipe } from '@angular/common';

//Clases o Interfaces
import { ProgramaBGB } from 'src/app/classes/ProgramaBGB';
import {
  ExamenIntegrador, NivelIngles, SemanasTec, Distinciones, Correo, Expediente,
  ExpedienteDataModal, ModalConfirmConfiguration, Ceneval, DataModal,
  UserClaims, ModeloInformacionUsuario, Usuario, Comentario, Card, EnumTipoCorreo
} from '../../classes';

//Servicios
import { UsuarioService } from 'src/app/services/usuario.service';
import { AvisosService } from 'src/app/services/avisos.service';
import { DistincionesService } from '../../services/distinciones.service';
import { NotificacionesService } from '../../services/notificaciones.service';
import { PeriodosService } from 'src/app/services/periodos.service';
import {
  ExpedienteService, NivelInglesService, PlanDeEstudiosService, RequisitosService,
  SemanasTecService, ServicioSocialService
} from 'src/app/services/requisitos';

//Componentes
import { BienvenidaGraduacionComponent } from '../bienvenida-graduacion/bienvenida-graduacion.component';
import { ModalDetalleExpedienteComponent } from '../modal-DetalleExpediente/modal-DetalleExpediente.component';
import { ModalDetalleComponent } from '../modal-detalle/modal-detalle.component';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { EnumTipoEstatusIntegrador } from 'src/app/enums/EnumTipoEstatusIntegrador';
import { CalendariosService } from 'src/app/services/calendarios.service';
import { ModalCreditosInsuficientesComponent } from '../modal-creditos-insuficientes/modal-creditos-insuficientes.component';
import { UtilsService } from 'src/app/services/utils.service';
import { EnumRequisitos } from '../../enums/EnumRequisitos';
import { Endpoints } from '../../interfaces/Endpoints';
import { ExamenConocimientos, ExamenConocimientosDataModal } from '../../classes/ExamenConocimientos';
import { ModalExamenConocimientosComponent } from '../modal-examen-conocimientos/modal-examen-conocimientos.component';
import { EnumTipoExamenConocimientos } from '../../enums/EnumTipoExamenConocimientos';
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [DatePipe]
})

export class HomeComponent implements OnInit {

  @Input() matricula: string = '';
  isAdminHelp = false;
  isLoading = false;

  public r: any[] = [];
  public usuario: Usuario = <Usuario>{};
  isCargadoUsuario = false;
  public correo: Correo = <Correo>{};
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};
  public ListaCard: Card[] = [];
  public activeLang = navigator.language.split('-')[0];

  mensaje: any;
  movil: boolean = false;
  laptop: boolean = false;
  laptopCorreccion: boolean = false;
  public userClaims: UserClaims = <UserClaims>{};

  colorCumple = '#00C851';
  colorNoCumple = '#EF8F00';
  colorNoAplica = '#FFFFFF';
  titulo: any;
  fechaActualizacionSS: Date = new Date();
  isRequiredSS: boolean = false;
  arrayContacto: any[] | undefined;
  avisos: any;
  isNoAplica: boolean = true;
  fechaExamenIntegrador: string = 'Por definir';
  periodoActual: string = '';
  periodoActualId: string = '';
  bandMostrar: boolean = false;
  distincion: Distinciones = new Distinciones([], '', '', true, true, false, false, true);
  isCargadoDistincion = false;
  distincionesIconOk: string = 'bi-check-circle-fill green-icon';
  distincionesIconError: string = 'bi-exclamation-circle-fill yellow-icon';
  ProgramaAcademico: string = '';
  ClaveCarrera: string = '';

  barraProgreso: number = 0;
  barra_Progreso: string = '0%';
  barra_ProgresoTotal: string = '100%';
  barra_ProgresoPorcentaje: string = '';
  creditosAcademicos: number = 0;
  requisitosAcademicos: number = 0;
  barraTF = true;
  periodoConfirmado = false;


  urlTesoreria: string = "";
  urlPrestamo: string = "";
  urlDistinciones: string = "";
  tienePrestamo: boolean = false;
  textoTesoreria: string = "";
  dateTramitesAdmon: Date = new Date();
  tituloSemnasTec: string = "";
  tituloNivelIngles: string = "";
  tituloExpediente: string = "";
  tituloServicioSocial: string = "";
  tituloPlanEstudio: string = "";
  tituloCeneval: string = "";
  tituloExamenIntegrador: string = "";
  tituloIdiomaDistinto: string = "";
  tituloCreditosExtranjeros: string = "";
  informacionUsuario: ModeloInformacionUsuario;
  auxIntegrador: string = "";
  auxCeneval: string = "";
  auxExamenConocimiento: string = "";
  linkCalendario: string = "";
  creditosFaltantes: number = 0;
  creditosInscritos: number = 0;
  creditosRequisito: number = 0;
  creditosAcreditados: number = 0;
  periodoGraduacion: string = "";
  periodoTranscurridoActual: string = "";
  linkVacio: boolean = false;
  comentariosExpediente: Comentario[] = [];
  isNivelSea: boolean = true;

  bandAlumnoNormal: boolean = true;
  bandProspecto: boolean = false;
  bandCandidato: boolean = false;
  bandExploracion: boolean = false;
  sinExamenAutorizado: string = "Sin Examen Autorizado (SEA)";
  claveEstatusGraduacion: string = "";
  fechaExamenConocimiento: string = "";
  proximamente: string = "Próximamente ";

  paramsEndpoints: Endpoints = <Endpoints>{};
  tipoExamen: number = 0;
  carreraExenta: boolean = false;
  edicionDatosPersonales: boolean = true;
  bienvenidaPClose: boolean = false;
  constructor(
    private spinner: NgxSpinnerService,
    public dialog: MatDialog, private responsive: BreakpointObserver, public usuarioService: UsuarioService,
    private translate: TranslateService, private expedienteService: ExpedienteService, private semanasTecService: SemanasTecService,
    private servicioSocialService: ServicioSocialService, private nivelInglesService: NivelInglesService, private router: Router,
    private planDeEstudiosService: PlanDeEstudiosService, private tService: RequisitosService,
    private avisosService: AvisosService, private periodosService: PeriodosService, private notificacionService: NotificacionesService,
    private distincionesService: DistincionesService, private calendarioService: CalendariosService,
    private utilsService: UtilsService,
    private notificacionesService: NotificacionesService) {
    this.informacionUsuario = JSON.parse(sessionStorage.getItem("userInformation") || '{}');
    this.translate = usuarioService.translate;
  }

  ngOnInit() {
    this.translate.onLangChange.subscribe((event: LangChangeEvent) => {
      this.taducirTarjetas();
    });
    this.translate.setDefaultLang(this.activeLang);
    this.translate.currentLang = this.activeLang;
    this.obtenMatricula();

    this.responsive.observe([Breakpoints.XSmall, Breakpoints.HandsetLandscape, Breakpoints.TabletPortrait, Breakpoints.Handset, Breakpoints.HandsetLandscape,
    Breakpoints.Small, Breakpoints.Tablet, Breakpoints.Medium, Breakpoints.Web])
      .subscribe(result => {

        const breakpoints = result.breakpoints;
        this.movil = false;
        this.laptop = false;
        //movilBienvenida
        if (breakpoints[Breakpoints.XSmall] || breakpoints[Breakpoints.TabletPortrait] || breakpoints[Breakpoints.HandsetPortrait]
          || breakpoints[Breakpoints.Small] || breakpoints[Breakpoints.TabletPortrait]) {
          this.movil = true;
        }

        //laptop
        else if (breakpoints[Breakpoints.Medium] || breakpoints[Breakpoints.Web]) {
          this.laptop = true;
        }
      });
    this.responsive.observe(['(min-width: 1260px)']).subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.laptopCorreccion = true;
      } else {
        this.laptopCorreccion = false;
      }

    });
    this.responsive.observe(['(max-width: 1590px)']).subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.laptopCorreccion = true;
      } else {
        this.laptopCorreccion = false;
      }
    });
  }

  obtenMatricula(): void {
    if (!this.informacionUsuario.matricula.startsWith('L')) {
      this.matricula = this.informacionUsuario.matricula;
      this.cargaInformacionUsuario();
    } else {
      //ver lo que ve el alumno
      this.isAdminHelp = true;
      this.edicionDatosPersonales = false;
      this.cargaInformacionUsuario();
    }
  }

  ngOnChanges(changes: any): void {
    this.ListaCard = [];
    if (!changes.matricula.firstChange) {
      this.obtenMatricula();
    }
  }
  cargaInformacionUsuario(): void {
    sessionStorage.setItem("bienvenidaPClose", "false");
    this.usuarioService.obtenerUsuario(this.matricula).subscribe({
      next: (data: any) => {
        this.usuario = data;
        this.isCargadoUsuario = true;
        this.ClaveCarrera = data.carreraId;
        this.ProgramaAcademico = data.claveProgramaAcademico;
        this.periodoGraduacion = data.periodoGraduacion;
        this.periodoTranscurridoActual = data.periodoTranscurridoActual;
        this.claveEstatusGraduacion = data.claveEstatusGraduacion;

        this.paramsEndpoints =
        {
          NumeroMatricula: data.matricula,
          ClaveProgramaAcademico: data.claveProgramaAcademico,
          ClaveCarrera: data.carreraId,
          ClaveNivelAcademico: data.nivelAcademico,
          ClaveEjercicioAcademico: data.periodoActual,
          ClaveCampus: data.claveCampus,
          Correo: data.correo
        };
      },
      error: (err: HttpErrorResponse) => {
        console.error(err);
      },
      complete: () => {
        this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
          if (resuPA.descripcion == "" || resuPA.descripcion == null) {
            if(this.paramsEndpoints.NumeroMatricula) {
              this.periodosService.getPronosticoAlumno(this.paramsEndpoints).subscribe((res: any) => {
                this.periodoActual = res.descripcion;
                this.periodoActualId = res.periodoId;
                this.periodoConfirmado = false;
              });
            }
          }
          else {
            this.periodoActual = resuPA.descripcion;
            this.periodoActualId = resuPA.periodoId
            this.periodoConfirmado = true;
          }
        }, (err: HttpErrorResponse) => {
          console.error(err);
        });

        this.barra_ProgresoTotal = '100%';

        this.distincionesService.getDistinciones(this.paramsEndpoints).subscribe((s: any) => {
          if (s.result) {
            this.distincion = s;
            this.isCargadoDistincion = true;
            this.distincion.hasConcentracion = false;
            this.distincion.diplomaOk ? this.distincion.diplomaIcon = this.distincionesIconOk : this.distincion.diplomaIcon = this.distincionesIconError;
            this.distincion.uleadOk ? this.distincion.uleadIcon = this.distincionesIconOk : this.distincion.uleadIcon = this.distincionesIconError;
           
            if (this.distincion.lstConcentracion.length == 0) {
              this.distincion.hasConcentracion = false;
            }else{
              this.distincion.hasConcentracion = true;
            }
          }
        }, (err: HttpErrorResponse) => {
          console.error(err);
        });

        this.avisosService.getAvisos(this.matricula).subscribe({
          next: (resp: any) => {
            this.avisos = resp.lstAvisos;
          },
          error: (error: HttpErrorResponse) => {
            console.error(error);
          }         
        });

        this.tService.getLinks().subscribe({
          next: (links: any) => {
            this.urlPrestamo = links.prestamoEducativo;
            this.urlTesoreria = links.tesoreria;
            this.urlDistinciones = links.distinciones;
          },
          error: (error: HttpErrorResponse) => {
            console.error(error);
          }
        });
        
        this.notificacionService.bienvenidoGraduacion(this.usuario.matricula, EnumTipoCorreo.BienvenidaCandidatos).subscribe((r: any) => {
          if (!r.result) {
            this.edicionDatosPersonales = false;
          }
        });
        this.cargaRequisitos();
      }
    });
  }

  cargaRequisitos(): void {
    this.isLoading = true;
    this.spinner.show();
    let plandeEstudios = this.planDeEstudiosService.getPlanDeEstudios(this.paramsEndpoints);
    let semanasTec = this.semanasTecService.getSemanasTec(this.matricula);
    let servicioSocial = this.servicioSocialService.getServicioSocial(this.paramsEndpoints);
    let nivelIngles = this.nivelInglesService.getAlumnoNivelIngles(this.matricula);
    let expedienteUsuario = this.expedienteService.getByAlumno(this.matricula);
    let examenConocimiento = this.tService.getExamenConocimientos(this.paramsEndpoints);
    let programaBGB = this.tService.getProgramaBGB(this.paramsEndpoints);
    let expedienteComentarios = this.expedienteService.getComentarios(this.matricula);
    
    forkJoin(
      [plandeEstudios.pipe(catchError(e => of('error', e))), 
        semanasTec.pipe(catchError(e => of('error', e))), 
        servicioSocial.pipe(catchError(e => of('error', e))), 
        nivelIngles.pipe(catchError(e => of('error', e))), 
        expedienteUsuario.pipe(catchError(e => of('error', e))), 
        examenConocimiento.pipe(catchError(e => of('error', e))), 
        programaBGB.pipe(catchError(e => of('error', e))), 
        expedienteComentarios.pipe(catchError(e => of('error', e)))]
    )
      .subscribe((result: any[]) => {
        if (result[0].result) {
          this.ListaCard.push(this.configuraPlanDeEstudios(result[0]));
          this.cargaCalendario();
        }
        if (result[1].result) {
          this.ListaCard.push(this.configuraSemanasTec(result[1]));
        }
        if (result[2].result) {
          this.ListaCard.push(this.configuraServicioSocial(result[2]));
        }
        if (result[3].result) {
          this.ListaCard.push(this.configuraNivelIngles(result[3]));
        }
        if (result[4].result) {
          this.comentariosExpediente = result[7];
          this.ListaCard.push(this.configuraExpediente(result[4]));
        }
        if (result[5].result) {
          this.ListaCard.push(this.configuraExamenConocimiento(result[5]));
        }
        if (result[6].result) {
          if (result[6].clavePrograma.includes("BGB")) {
            let nivelIngles;
            if (result[3].result) {
              nivelIngles = result[3];
            }
            result[6].creditosCursadosExtranjero = result[6].creditosAprobadosProgramaInternacional;
            this.ListaCard.push(this.configuraProgramaBgbCreditos(result[6]));
            this.ListaCard.push(this.configuraProgramaBgbUF(result[6]));
          }
        }
        this.calculoBarraProgreso();
        this.isLoading = false;
        this.spinner.hide();
        this.taducirTarjetas();

      }, (err: HttpErrorResponse) => {
        this.spinner.hide();
        this.isLoading = false;
        console.error(err);
      });

  }

  configuraPlanDeEstudios(resuPE: any): Card {
    const creditosFaltantesPCG = resuPE.creditosRequisito - resuPE.creditosAcreditados;
    this.creditosRequisito = resuPE.creditosRequisito;
    this.creditosAcreditados = resuPE.creditosAcreditados;
    this.creditosFaltantes = creditosFaltantesPCG;
    this.creditosInscritos = resuPE.creditosInscritos;
    //Cálculo de Créditos Académicos para la Barra de Progreso
    this.barra_ProgresoTotal = '100%';
    if (resuPE.creditosRequisito <= 60) {
      this.creditosAcademicos = 0;
    } else if (resuPE.creditosAcreditados > resuPE.creditosRequisito) {
      this.creditosAcademicos = (80 * resuPE.creditosRequisito) / resuPE.creditosRequisito;
    } else {
      this.creditosAcademicos = (80 * resuPE.creditosAcreditados) / resuPE.creditosRequisito;
    }
    if (resuPE.isCumplePlanDeEstudios) {
      this.notificacionService.guardarNotificacion("Requisito de Plan de Estudio", "Ya cuentas con los créditos necesarios para graduarte", this.matricula).subscribe();
    }
    if (creditosFaltantesPCG >= 22 && creditosFaltantesPCG <= 49) {
      //Prospecto a graduar
      if (!resuPE.isCumplePlanDeEstudios) {
        this.notificacionService.guardarNotificacion("Requisito de Plan de Estudio", "No cuentas con los créditos necesarios para graduarte", this.matricula).subscribe();
      }
      //Prospecto Candidato a Graduar
      this.bandAlumnoNormal = false;
      this.bandProspecto = true;
      this.bandCandidato = false;
      this.bandExploracion = false;
      this.openDialogBienvenidaGP();
    }
    else if (creditosFaltantesPCG <= 21 && this.claveEstatusGraduacion === EnumRequisitos.GS) {
      //Candidato a Graduar
      this.bandAlumnoNormal = false;
      this.bandProspecto = false;
      this.bandCandidato = true;
      this.bandExploracion = false;
      this.notificacionService.bienvenidoGraduacion(this.matricula, EnumTipoCorreo.BienvenidaProspectos).subscribe({
        next: (r: any) => {
          if (r.result) {
            this.openDialogBienvenidaGP();
          }else{
              this.notificacionService.bienvenidoGraduacion(this.matricula, EnumTipoCorreo.BienvenidaCandidatos).subscribe((r: any) => {
                if (r.result) {
                  this.abrirModal('MiInformacion');
                }else{
                  if (this.periodoGraduacion == this.periodoTranscurridoActual) {
                    if (this.creditosFaltantes > this.creditosInscritos) {
                      this.abrirModalCreditosNoAlcanzan();
                    }
                  }
                }
              });
          }
        }
      });
      
    } else if (creditosFaltantesPCG <= 21 ) {
      //Prospecto Candidato a Graduar
        this.bandAlumnoNormal = false;
        this.bandProspecto = false;
        this.bandCandidato = true;
        this.bandExploracion = false;
        this.openDialogBienvenidaGP();
      }
      else if (creditosFaltantesPCG > 49) {
      //Alumno Normal
      this.bandAlumnoNormal = true;
      this.bandProspecto = false;
      this.bandCandidato = false;
      this.bandExploracion = false;
    }
    if (this.creditosRequisito <= 60) {
      this.bandAlumnoNormal = false;
      this.bandProspecto = false;
      this.bandCandidato = false;
      this.bandExploracion = true;
      return {
        Id: 5, icono: this.utilsService.getIcono(5), titulo: EnumRequisitos.PlanEstudios, descripcion: '', ultimaActualizacion: resuPE.ultimaActualizacionPE, isCumple: false, list: [{ nombre: 'Consulta tu avance de Plan de Estudios una vez que hayas selccionando tu carrera de egreso.', valor: '' }], color: this.colorNoCumple, orden: 1, aplica: true
      };
    } else {
      return {
        Id: 5, icono: this.utilsService.getIcono(5), titulo: EnumRequisitos.PlanEstudios, descripcion: '', ultimaActualizacion: resuPE.ultimaActualizacionPE, isCumple: resuPE.isCumplePlanDeEstudios, list: [{ nombre: 'Créditos requisito', valor: resuPE.creditosRequisito }, { nombre: 'Créditos acreditados', valor: resuPE.creditosAcreditados }], color: resuPE.isCumplePlanDeEstudios ? this.colorCumple : this.colorNoCumple, orden: 1, aplica: true
      };
    }
  }

  configuraSemanasTec(semanas: SemanasTec): Card {
    if (semanas.result) {
      if (semanas.semanasObtenidas >= semanas.semanasMaximas) {
        semanas.isCumple = true;
        semanas.color = this.colorCumple;
      } else {
        semanas.color = this.colorNoCumple;
      }
      return {
        Id: 1, icono: this.utilsService.getIcono(1), titulo: 'Semanas Tec', descripcion: '', ultimaActualizacion: new Date(semanas.ultimaActualizacion), isCumple: semanas.isCumple, list: [{ nombre: 'Semanas requisito', valor: semanas.semanasMaximas.toString() }, { nombre: 'Semanas acreditadas', valor: semanas.semanasObtenidas.toString() }], color: semanas.color, orden: 2, aplica: true
      };
    } else {
      return {
        Id: 1, icono: this.utilsService.getIcono(1), titulo: 'Semanas Tec', descripcion: '', ultimaActualizacion: new Date(semanas.ultimaActualizacion), isCumple: semanas.isCumple, list: [{ nombre: 'Semanas requisito', valor: semanas.semanasMaximas.toString() }, { nombre: 'Semanas acreditadas', valor: semanas.semanasObtenidas.toString() }], color: semanas.color, orden: 2, aplica: true
      };
    }
  }

  configuraServicioSocial(resu: any): Card {
    this.fechaActualizacionSS = new Date(resu.ultimaActualizacionSS);
    this.isRequiredSS = resu.isServicioSocial;

    if (this.isRequiredSS) {
      return {
        Id: 3, icono: this.utilsService.getIcono(3), titulo: 'Servicio Social', descripcion: '', ultimaActualizacion: resu.ultimaActualizacionSS, isCumple: resu.isCumpleSS, list: [{ nombre: resu.lista_Horas[0].horaRequisito, valor: resu.lista_Horas[0].valorRequisito }, { nombre: resu.lista_Horas[0].horaAcreditada, valor: resu.lista_Horas[0].valorAcreditada }], color: resu.isCumpleSS ? this.colorCumple : this.colorNoCumple, orden: 3, aplica: true
      };
    } else {
      return {
        Id: 3, icono: this.utilsService.getIcono(3), titulo: 'Servicio Social', descripcion: '', ultimaActualizacion: resu.ultimaActualizacionSS, isCumple: resu.isCumpleSS, list: [{ nombre: resu.lista_Horas[0].horaAcreditada, valor: "" }], color: resu.isCumpleSS ? this.colorCumple : this.colorNoCumple, orden: 3, aplica: true
      };
    }
  }

  configuraNivelIngles(nivelIngles: NivelIngles): Card {

    if (nivelIngles.nivelIdiomaAlumno >= nivelIngles.requisitoNvl) {
      nivelIngles.colorCumple = this.colorCumple;
      nivelIngles.isCumple = true;
      this.isNivelSea = false;
    } else {
      nivelIngles.colorCumple = this.colorNoCumple;
    }
    if (nivelIngles.nivelIdiomaAlumno === this.sinExamenAutorizado) {
      nivelIngles.colorCumple = this.colorNoCumple;
      nivelIngles.isCumple = false;
      this.isNivelSea = true;
    }
    if (nivelIngles.nivelIdiomaAlumno === this.sinExamenAutorizado) {
      return {
        Id: 2, icono: this.utilsService.getIcono(2), titulo: 'Nivel de inglés', descripcion: '', ultimaActualizacion: nivelIngles.fechaUltimaModificacion, isCumple: nivelIngles.isCumple, list: [{ nombre: 'Nivel requerido', valor: nivelIngles.requisitoNvl }, { nombre: 'Nivel acreditado', valor: "SEA" }], color: nivelIngles.colorCumple, orden: 4, aplica: true
      };
    } else {
      this.isNivelSea = false;
      return {
        Id: 2, icono: this.utilsService.getIcono(2), titulo: 'Nivel de inglés', descripcion: '', ultimaActualizacion: nivelIngles.fechaUltimaModificacion, isCumple: nivelIngles.isCumple, list: [{ nombre: 'Nivel requerido', valor: nivelIngles.requisitoNvl }, { nombre: 'Nivel acreditado', valor: nivelIngles.nivelIdiomaAlumno }], color: nivelIngles.colorCumple, orden: 4, aplica: true
      };
    }
  }

  configuraExpediente(expediente: Expediente): Card {
    if (expediente.estatus === 'Completo') {
      expediente.colorCumple = this.colorCumple;
      expediente.isCumple = true;
    } else {
      expediente.colorCumple = this.colorNoCumple;
      expediente.isCumple = false;
    }

    if (expediente.isCumple) {
      return {
        Id: 4, icono: this.utilsService.getIcono(4), titulo: 'Expediente', descripcion: '', ultimaActualizacion: expediente.ultimaActualizacion, isCumple: expediente.isCumple, list: [{ nombre: 'Tienes todos los documentos de tu expediente', valor: "" }], color: expediente.colorCumple, orden: 6, aplica: true
      };
    } else {
      return {
        Id: 4, icono: this.utilsService.getIcono(4), titulo: 'Expediente', descripcion: '', ultimaActualizacion: expediente.ultimaActualizacion, isCumple: expediente.isCumple, list: [{ nombre: 'No tienes todos los documentos de tu expediente', valor: "" }], color: expediente.colorCumple, orden: 6, aplica: true
      };
    }
  }

  configuraExamenConocimiento(examen: ExamenConocimientos): Card {    
    this.tipoExamen = examen.idTipoExamen;
    this.fechaExamenConocimiento = new Date(examen.fechaExamen).toLocaleDateString();
    if (Number(EnumTipoExamenConocimientos.CarreraExenta) == this.tipoExamen) {
      this.carreraExenta = true;
      examen.colorCumple = this.colorNoCumple;
      examen.isCumple = true;
      this.auxExamenConocimiento = EnumTipoEstatusIntegrador.noCumple;
      return {
        Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [], color: this.colorNoAplica, orden: 7, aplica: examen.cumpleRequisito
      };
    }
    else if (Number(EnumTipoExamenConocimientos.CENEVAL) == this.tipoExamen) {
      this.carreraExenta = false;
      if (this.fechaExamenConocimiento === '1-1-1' || this.fechaExamenConocimiento === '1/1/1') {
        this.fechaExamenConocimiento = this.proximamente;
      }
      if (examen.cumpleRequisito) {
        examen.colorCumple = this.colorCumple;
        examen.isCumple = true;
        this.auxExamenConocimiento = EnumTipoEstatusIntegrador.siCumple;
        return {
          Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [{ nombre: 'Fecha aplicación de examen', valor: new Date(examen.fechaExamen).toLocaleDateString() }], color: examen.colorCumple, orden: 7, aplica: examen.cumpleRequisito
        };
      } else {
        examen.colorCumple = this.colorNoCumple;
        examen.isCumple = false;
        this.auxExamenConocimiento = EnumTipoEstatusIntegrador.siCumple;
        return {
          Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [{ nombre: 'Fecha aplicación de examen', valor: this.fechaExamenConocimiento }], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
        };
      }
    }
    else {
      this.carreraExenta = false;
      //Integrador y IFOM
      if (examen.estatus === EnumTipoEstatusIntegrador.siCumple) {
        examen.colorCumple = this.colorCumple;
        examen.isCumple = true;
      } else {
        examen.colorCumple = this.colorNoCumple;
        examen.isCumple = false;
      }
      if (this.fechaExamenConocimiento === '1-1-1' || this.fechaExamenConocimiento === '1/1/1') {
        this.fechaExamenIntegrador = new Date(examen.fechaExamen).toLocaleDateString();
      }
      let dateNow = new Date();
      const [month, day, year] = new Date(examen.fechaExamen).toLocaleDateString().split('-');

      let fechaExamen = new Date(+year, +month, +day);
      if (examen.estatus === EnumTipoEstatusIntegrador.siCumple && examen.esRequisito) {
        if (this.fechaExamenConocimiento === '1-1-1' || this.fechaExamenConocimiento === '1/1/1') {
          this.fechaExamenConocimiento = this.proximamente;
          this.auxExamenConocimiento = EnumTipoEstatusIntegrador.siCumple;
          return {
            Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'Has presentado tu examen integrador', valor: "" }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
          };
        }
        if (fechaExamen < dateNow) {
          return {
            Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'Has presentado tu examen integrador', valor: "" }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
          };
        }
      }

      if (examen.estatus === EnumTipoEstatusIntegrador.noCumple && examen.esRequisito) {
        if (this.fechaExamenConocimiento === '1-1-1' || this.fechaExamenConocimiento === '1/1/1') {
          this.fechaExamenConocimiento = this.proximamente;
          this.auxExamenConocimiento = "NC,SF";
          return {
            Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'Fecha de presentación', valor: "Próximamente" }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
          };
        } else {
          this.auxExamenConocimiento = "NC,CF";
          return {
            Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'Fecha de presentación', valor: this.fechaExamenConocimiento }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
          };
        }
      }

      if (examen.estatus !== 'NA' && examen.esRequisito) {
        if (this.fechaExamenConocimiento === '1-1-1' || this.fechaExamenConocimiento === '1/1/1') {
          this.auxExamenConocimiento = "NP,SF";
          return { Id: 6, icono: this.utilsService.getIcono(7), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'Fecha de presentación', valor: "Próximamente" }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito };
        } else {
          this.auxExamenConocimiento = "NP,CF";
          return {
            Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaExamen, isCumple: examen.isCumple, list: [/*{ nombre: 'Fecha de presentación', valor: this.fechaExamenIntegrador }*/], color: examen.colorCumple, orden: 7, aplica: examen.esRequisito
          };
        }
      } else {
        this.auxExamenConocimiento = EnumTipoEstatusIntegrador.noRequisito;
        return {
          Id: 6, icono: this.utilsService.getIcono(6), titulo: examen.tituloExamen, descripcion: examen.descripcionExamen, ultimaActualizacion: examen.fechaRegistro, isCumple: examen.isCumple, list: [/*{ nombre: 'No es requisito de tu carrera', valor: '' }*/], color: this.colorNoAplica, orden: 7, aplica: examen.esRequisito
        };
      }
    }
  }

  configuraProgramaBgbUF(programaBGB: ProgramaBGB): Card {
    if (programaBGB.cumpleRequisitosProgramaEspecial && programaBGB.cumpleRequisitoInglesCuarto && programaBGB.cumpleRequisitoInglesQuinto && programaBGB.cumpleRequisitoInglesOctavo) {
      programaBGB.matricula = this.usuario.matricula.toString();
      programaBGB.colorCumple = this.colorCumple;
      programaBGB.isCumple = true;
      let mensajeUf: string = '4to, 5to, 8vo';
      return { Id: 8, icono: this.utilsService.getIcono(8), titulo: 'UF’s en idioma distinto al español', descripcion: '', ultimaActualizacion: programaBGB.ultimaActualizacion, isCumple: programaBGB.isCumple, list: [{ nombre: 'UF\'s requisito', valor: '4to, 5to, 8vo' }, { nombre: 'UF\'s acreditadas', valor: mensajeUf }], color: programaBGB.colorCumple, orden: 10, aplica: true };
    } else {
      let mensajeUf: string = '';
      if (programaBGB.cumpleRequisitoInglesCuarto)
        mensajeUf += '4to ';
      if (programaBGB.cumpleRequisitoInglesQuinto)
        mensajeUf += '5to ';
      if (programaBGB.cumpleRequisitoInglesOctavo)
        mensajeUf += '8vo';
      mensajeUf = mensajeUf.replace(' ', ', ');
      programaBGB.colorCumple = this.colorNoCumple;
      programaBGB.isCumple = false;
      return { Id: 8, icono: this.utilsService.getIcono(8), titulo: 'UF’s en idioma distinto al español', descripcion: '', ultimaActualizacion: programaBGB.ultimaActualizacion, isCumple: programaBGB.isCumple, list: [{ nombre: 'UF\'s requisito', valor: '4to, 5to, 8vo' }, { nombre: 'UF\'s acreditados', valor: mensajeUf }], color: programaBGB.colorCumple, orden: 10, aplica: true };
    }
  }

  configuraProgramaBgbCreditos(programaBGB: ProgramaBGB): Card {
    if (programaBGB.creditosCursadosExtranjero >= 18) {
      programaBGB.matricula = this.usuario.matricula.toString();
      programaBGB.colorCumple = this.colorCumple;
      programaBGB.isCumple = true;
      return { Id: 9, icono: this.utilsService.getIcono(9), titulo: 'Créditos en el extranjero', descripcion: '', ultimaActualizacion: programaBGB.ultimaActualizacion, isCumple: programaBGB.isCumple, list: [{ nombre: 'Créditos requisito', valor: '18' }, { nombre: 'Créditos acréditados', valor: programaBGB.creditosCursadosExtranjero.toString() }], color: programaBGB.colorCumple, orden: 9, aplica: true };
    } else {
      programaBGB.colorCumple = this.colorNoCumple;
      programaBGB.isCumple = false;
      return { Id: 9, icono: this.utilsService.getIcono(9), titulo: 'Créditos en el extranjero', descripcion: '', ultimaActualizacion: programaBGB.ultimaActualizacion, isCumple: programaBGB.isCumple, list: [{ nombre: 'Créditos requisito', valor: '18' }, { nombre: 'Créditos acréditados', valor: programaBGB.creditosCursadosExtranjero.toString() }], color: programaBGB.colorCumple, orden: 9, aplica: true };
    }
  }

  openCardDialog(card: Card): void {
    //recibo el objeto de la tarjeta que seleccionó
    let dataCard;
    //dependiendo de la tarjeta se llama al servicio y obtiene la información desde la webapi
    switch (card.Id) {
      case 1: {
        //statements; 
        this.tService.getTarjeta(1, this.translate.currentLang).subscribe((s: any) => {
          s.nota = s.nota.replace('@fecha', `${card.ultimaActualizacion.getDate().toString().padStart(2, '0')}/${(card.ultimaActualizacion.getMonth() + 1).toString().padStart(2, '0')}/${card.ultimaActualizacion.getFullYear()}`);
          const dataCardSemanasTec = new DataModal(card.titulo, s.nota, s.link, [], card.list, card.isCumple, card.aplica);
          this.openDialog(dataCardSemanasTec);
        });
        break;
      }
      case 2: {
        //statements; 
        this.tService.getTarjeta(3, this.translate.currentLang).subscribe((r: any) => {
          let fecha = new Date(card.ultimaActualizacion.toString());
          r.nota = r.nota.replace('@fecha', `${fecha.getDate().toString().padStart(2, '0')}/${(fecha.getMonth() + 1).toString().padStart(2, '0')}/${fecha.getFullYear()}`);

          if (this.isNivelSea) {
            
            dataCard = new DataModal(card.titulo, r.nota, r.link, [], [{ nombre: card.list[0].nombre, valor: card.list[0].valor }, { nombre: card.list[1].nombre, valor: this.sinExamenAutorizado }], card.isCumple, card.aplica);
          } else {
            dataCard = new DataModal(card.titulo, r.nota, r.link, [], card.list, card.isCumple, card.aplica);
          }
          this.openDialog(dataCard);
        });
        break;
      }
      case 3: {
        const linkName = 'Aquí';
        if (this.isRequiredSS) {
          this.tService.getTarjeta(2, this.translate.currentLang).subscribe((resuSS: any) => {
            let fecha = this.fechaActualizacionSS.getDate() + '/' + (this.fechaActualizacionSS.getMonth() + 1) + '/' + this.fechaActualizacionSS.getFullYear()
            this.mensaje = resuSS.nota.replace('@fecha', fecha);
            dataCard = new DataModal(card.titulo, this.mensaje, resuSS.link, [], card.list, card.isCumple, card.aplica, linkName);
            this.openDialog(dataCard);
          });
        } else {
          this.tService.getTarjeta(10, this.translate.currentLang).subscribe((resuSS: any) => {
            this.arrayContacto = [resuSS.contacto, resuSS.correo];
            dataCard = new DataModal(card.titulo, resuSS.nota, '', this.arrayContacto, card.list, card.isCumple, card.aplica, linkName);
            this.openDialog(dataCard);
          });
        }
        break;
      }
      case 4: {
        //Expediente
        this.tService.getTarjeta(4, this.translate.currentLang).subscribe((r: any) => {
          const dataCardExpediente = new ExpedienteDataModal(card.titulo, r.documentos, r.nota, card.list[0].nombre, r.contacto, r.correo, card.isCumple, this.comentariosExpediente, r.link, 'none');
          const dialogRef = this.dialog.open(ModalDetalleExpedienteComponent, {
            width: '500px',
            maxHeight: '100vh',
            disableClose: true,
            panelClass: 'detalleExpedienteDialog',
            data: dataCardExpediente
          });

          dialogRef.afterClosed().subscribe(result => {
            this.mensaje = result;
          });
        });
        break;
      }
      case 5: {
        //planDeEstudiosService
        const linkName = 'Consulta tu plan de estudios aquí';
        this.tService.getTarjeta(5, this.translate.currentLang).subscribe((resuPlanEstu: any) => {
          dataCard = new DataModal(card.titulo, resuPlanEstu.nota, resuPlanEstu.link, [], card.list, card.isCumple, card.aplica, linkName);
          this.openDialog(dataCard);
        });
        break;
      }
      //CENEVAL
      case 6: {
        this.tService.getExamenConocimientoPorLenguaje(this.tipoExamen, this.translate.currentLang).subscribe((r: any) => {
          let fechaExamen;
          if (r.result) {
            this.carreraExenta = false;
            this.translate.get("modalDetalleCeneval.FechaAplicacionDeExamen").subscribe((result) => {
              fechaExamen = result;
            });
            if (card.aplica) {
              dataCard = new ExamenConocimientosDataModal(r.titulo, r.descripcion, r.nota, r.link, [r.contacto, r.correo], [{ nombre: fechaExamen, valor: this.fechaExamenConocimiento }], card.isCumple, card.aplica, 'none');
            } else {
              dataCard = new ExamenConocimientosDataModal(r.titulo, r.descripcion, r.nota, r.link, [], card.list, card.isCumple, card.aplica, 'none');
            }
          }
          else {
            this.carreraExenta = true;
            r.link = 'Con tu director de programa';
            dataCard = new ExamenConocimientosDataModal(r.titulo, r.descripcion, r.nota, r.link, [{ nombre: '', valor: '' }], card.list, card.isCumple, card.aplica, 'none');
          }
          const dialogRef = this.dialog.open(ModalExamenConocimientosComponent, {
            width: '500px',
            maxHeight: '100vh',
            disableClose: true,
            panelClass: 'detalleExamenConocimientosDialog',
            data: dataCard
          });

          dialogRef.afterClosed().subscribe(result => {
            this.mensaje = result;
          });
        });
        break;
      }
      case 8: {
        this.tService.getTarjeta(8, this.translate.currentLang).subscribe((r: any) => {
          const ultimaFecha = new Date(card.ultimaActualizacion);
          r.nota = r.nota.replace('@fecha', `${ultimaFecha.getDate().toString().padStart(2, '0')}/${(ultimaFecha.getMonth() + 1).toString().padStart(2, '0')}/${ultimaFecha.getFullYear()}`);
          dataCard = new DataModal(card.titulo, r.nota, r.link, [r.contacto, r.correo], card.list, card.isCumple, card.aplica);
          this.openDialog(dataCard);
        });
        break;
      }
      case 9: {
        //statements; 
        this.tService.getTarjeta(9, this.translate.currentLang).subscribe((r: any) => {
          dataCard = new DataModal(card.titulo, r.nota, r.link, [r.contacto, r.correo], card.list, card.isCumple, card.aplica);
          this.openDialog(dataCard);
        });
        break;
      }
    }
  }

  openDialog(dataCard: DataModal): void {
    const dialogRef = this.dialog.open(ModalDetalleComponent, {
      width: '500px',
      disableClose: true,
      data: dataCard
    });

    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
    });
  }

  abrirModal(origen: string): void {
    if (this.isAdminHelp) {
      return;
    }

    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    

    if (origen == 'MiInformacion') {
      dialogConfig.width = '700px';
      this.configuracionModal.titulo = "Validación de información personal y académica";
      this.configuracionModal.isSelectCampus = true;
      this.configuracionModal.isCambioFecha = false;
      dialogConfig.data = {
        configuracion: this.configuracionModal,
        objeto: this.usuario,
        distinciones: this.distincion,
        terminado: false,
        regreso: false
      };
      dialogConfig.disableClose = true;

      const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(result => {
        if(result.terminado){
          this.edicionDatosPersonales = false;
        }
        this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
          if (resuPA.descripcion != null) {
            this.periodoActual = resuPA.descripcion;
            this.periodoConfirmado = true;
          }
        });
        if (this.periodoGraduacion == this.periodoTranscurridoActual) {
          if (this.creditosFaltantes > this.creditosInscritos) {
            this.abrirModalCreditosNoAlcanzan();
          }
        }
      });
    }
    else if (origen == 'ModificarFecha') {
      dialogConfig.width = '570px';
      dialogConfig.height ='auto';
      dialogConfig.maxHeight = '100vh';
      let tituloModificarFecha = "";
      this.translate.get("modificarFecha.encabezado").subscribe((result) => {
        tituloModificarFecha = result;
      });
      this.configuracionModal.titulo = tituloModificarFecha;
      this.configuracionModal.isSelectCampus = false;
      this.configuracionModal.isCambioFecha = true;
      dialogConfig.data = {
        configuracion: this.configuracionModal,
        usuario: this.matricula,
        objeto: this.usuario,
        periodoActual : '',
        periodoGuardado: false,
        paramsEndpoints : this.paramsEndpoints
      };

      const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(result => {
        if(result != null && result != 'undefined' && result != '') {
          if(result.periodoGuardado){
            
            if(result.periodoActual != 'undefined' && result.periodoActual != ''){
              this.periodoActual = result.periodoActual;
              this.periodoConfirmado = true;
            } else{
              this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
              if (resuPA.descripcion != null) {
                this.periodoActual = resuPA.descripcion;
                this.periodoConfirmado = true;
              }
              });
            }
          }
          else{
            this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
              if (resuPA.descripcion != null) {
                this.periodoActual = resuPA.descripcion;
                this.periodoConfirmado = true;
              }
              });
          }
        
        }
        
      });
    }
    
  }

  cambiarLenguaje(lang: string): void {
    if (this.activeLang == 'es') {
      this.activeLang = 'en';
    }
    else {
      this.activeLang = 'es';
    }
    this.usuarioService.setLenguaje(this.activeLang);
  }

  OrdenarLista(): void {
    this.ListaCard = this.ListaCard.sort((a, b) => (a.orden > b.orden) ? 1 : -1);
  }

  verAvisos(matricula: string): void {
    if (this.isAdminHelp) {
      return;
    }
    this.router.navigate(['/veravisos/'], {
      queryParams: {
        matricula: matricula
      }
    })
  }

  openDialogBienvenidaGP(): void {
    if (this.isAdminHelp) {
      return
    }
    this.notificacionService.bienvenidoGraduacion(this.matricula, EnumTipoCorreo.BienvenidaProspectos).subscribe((r: any) => {
     if (r.result) {
        const dialogRef = this.dialog.open(BienvenidaGraduacionComponent, {
          width: '560px',
          height: '465px',
          disableClose: true,
          data: { objeto: this.usuario, paramsEndpoints : this.paramsEndpoints }
        });
        dialogRef.afterClosed().subscribe(result => {
          this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
            this.periodoActual = resuPA.descripcion;
          });
          this.notificacionesService.guardarNotificacionCorreo(EnumTipoCorreo.BienvenidaProspectos, this.usuario.matricula).subscribe();
          if (this.creditosFaltantes <= 21 && this.claveEstatusGraduacion === EnumRequisitos.GS) {
            this.notificacionService.bienvenidoGraduacion(this.matricula, EnumTipoCorreo.BienvenidaCandidatos).subscribe((r: any) => {
              if (r.result) {
                this.abrirModal('MiInformacion');
              }
            });
          }
        });
      }
    });
  }

  calculoBarraProgreso(): void {
    this.barra_ProgresoTotal = '100%';
    let menosRequisito = 1;
    this.barraTF = true;
    this.ListaCard.forEach(b => {
      if (b.aplica) {
        let aux = (20 / (this.ListaCard.length - menosRequisito));
        this.requisitosAcademicos = 0;
        this.ListaCard.forEach(a => {
          if (a.isCumple && (a.titulo != EnumRequisitos.PlanEstudios)) {
            this.requisitosAcademicos += aux;
          }
          this.barraProgreso = this.requisitosAcademicos + this.creditosAcademicos;
          this.barra_Progreso = Math.round(this.barraProgreso).toString() + '%';
          this.barra_ProgresoTotal = '100%';
        });
      } else {
        menosRequisito = menosRequisito + 1;
      }
    });
    if (this.barraProgreso >= 85) {
      this.barra_ProgresoTotal = '';
      this.barra_Progreso = Math.round(this.barraProgreso).toString() + '%';
      this.barra_ProgresoPorcentaje = '';
    }
    else if (this.barraProgreso >= 0 && this.barraProgreso <= 6) {
      this.barraTF = false;
      this.barra_ProgresoTotal = '100%';
      this.barra_ProgresoPorcentaje = Math.round(this.barraProgreso).toString() + '%';
      this.barra_Progreso = '';
    }
    else if (this.barraProgreso >= 7 && this.barraProgreso <= 84) {
      this.barraTF = true;
      this.barra_ProgresoTotal = '100%';
      this.barra_Progreso = Math.round(this.barraProgreso).toString() + '%';
    }
    else if (this.barraProgreso == 0) {
      this.barra_ProgresoTotal = '100%';
      this.barra_ProgresoPorcentaje = '0%';
      this.barra_Progreso = '';
      this.barraTF = false;
    }
  }

  redirectTesoreria(): void {
    window.open(this.urlTesoreria, "_blank");
  }

  redirectPrestamo(): void {
    window.open(this.urlPrestamo, "_blank");
  }

  taducirTarjetas(): void {
    this.translate.get('tarjetas.titulos').subscribe((cliente: any[]) => {
      this.ListaCard.forEach(element => {
        element.titulo = cliente[element.Id];
        if (element.Id == 1) {
          let traduccion = this.validacionTarjetasIdioma("semanas");
          element.list[0].nombre = traduccion[0];
          element.list[1].nombre = traduccion[1];
        }
        if (element.Id == 2) {
          let traduccion = this.validacionTarjetasIdioma("ingles");
          element.list[0].nombre = traduccion[0];
          element.list[1].nombre = traduccion[1];
        }
        if (element.Id == 3) {
          let traduccion = this.validacionTarjetasIdioma("servicio-social");
          element.list[0].nombre = traduccion[0];
          element.list[1].nombre = traduccion[1];
        }
        if (element.Id == 4) {
          let traduccion = this.validacionTarjetasIdioma("expediente");
          if (element.isCumple) {
            element.list[0].nombre = traduccion[1];
          } else {
            element.list[0].nombre = traduccion[0];
          }
        }
        if (element.Id == 5) {
          let traduccion = this.validacionTarjetasIdioma("plan-estudio");
          if (this.creditosRequisito <= 60) {
            element.list[0].nombre = traduccion[2];
          } else {
            element.list[0].nombre = traduccion[0];
            element.list[1].nombre = traduccion[1];
          }
        }
        if (element.Id == 6 && element.descripcion == 'CENEVAL') {
          let traduccion = this.validacionTarjetasIdioma("examen-conocimientos");
          if (this.auxExamenConocimiento.includes(EnumTipoEstatusIntegrador.siCumple)) {
            element.list[0].nombre = traduccion[0];
          } else if (this.auxExamenConocimiento == EnumTipoEstatusIntegrador.noRequisito) {
            element.list[0].nombre = traduccion[1];
          } else if (this.auxExamenConocimiento.includes(EnumTipoEstatusIntegrador.noCumple)) {
            element.list[0].nombre = traduccion[0];
          } else if (this.auxExamenConocimiento.includes(EnumTipoEstatusIntegrador.noPresento)) {
            element.list[0].nombre = traduccion[0];
          }
          if (this.auxExamenConocimiento.includes(EnumTipoEstatusIntegrador.sinFecha)) {
            element.list[0].valor = traduccion[2];
          }
        }
        if (element.Id == 7) {
          let traduccion = this.validacionTarjetasIdioma("integrador");
          if (this.auxCeneval == EnumTipoEstatusIntegrador.siCumple) {
            element.list[0].nombre = traduccion[1];
          } else {
            element.list[0].nombre = traduccion[0];
          }
        }
        if (element.Id == 8) {
          let traduccion = this.validacionTarjetasIdioma("uf");
          element.list[0].nombre = traduccion[0];
          element.list[1].nombre = traduccion[1];
        }
        if (element.Id == 9) {
          let traduccion = this.validacionTarjetasIdioma("creditos-extranjero");
          element.list[0].nombre = traduccion[0];
          element.list[1].nombre = traduccion[1];
        }
      });
    });
  }

  validacionTarjetasIdioma(tarjeta: string): any[] {
    let busqueda = "tarjetas.contenido." + tarjeta;
   
    let array: any[] = [];
    this.translate.get(busqueda).subscribe((element) => {
      
      if (tarjeta == "examen-conocimientos") {
        array.push(element["requisito"]);
        array.push(element["acreditadas"]);
        array.push(element["proximamente"]);
      } else if (tarjeta == "integrador") {
        array.push(element["presentado"]);
        array.push(element["requisito"]);
        array.push(element["acreditadas"]);
        array.push(element["proximamente"]);
      } else if (tarjeta == "plan-estudio") {
        array.push(element["requisito"]);
        array.push(element["acreditadas"]);        
        array.push(element["etapaExploracion"]);
      } else if (tarjeta == "ingles") {
        array.push(element["requisito"]);
        array.push(element["acreditadas"]);        
        array.push(element["subtitulo"]);
      } else {
        array.push(element["requisito"]);
        array.push(element["acreditadas"]);        
      }
    });
    return array;
  }

  cargaCalendario(): void {
    this.calendarioService.getCalendarioAlumno(this.matricula).subscribe((resp: any) => {
      if (this.creditosFaltantes >= 22 && this.creditosFaltantes <= 49) {
        this.linkCalendario = resp.linkProspecto;
      }
      else if (this.creditosFaltantes <= 21) {
        this.linkCalendario = resp.linkCandidato;
      }

      if (!this.linkCalendario.includes("http") && this.linkCalendario != "")
        this.linkCalendario = "//" + this.linkCalendario;

      if (this.linkCalendario == "") {
        this.linkVacio = true;
      }

    });
  }

  redirectCalendario(): void {
    if (this.linkCalendario == "")
      return;
    window.open(this.linkCalendario, "_blank");
  }

  abrirModalCreditosNoAlcanzan(): void {
    if (this.claveEstatusGraduacion === EnumRequisitos.GS) {
      this.notificacionService.enteradoCreditosNoAlcanzan(this.matricula).subscribe((r: any) => {
        if (r.result) {
          const dialogRef = this.dialog.open(ModalCreditosInsuficientesComponent, {
            width: '560px',
            height: '465px',
            disableClose: true,
            data: { objeto: this.usuario, periodo: this.periodoActual, periodoId: this.periodoActualId, paramsEndpoints : this.paramsEndpoints   }
          });
          dialogRef.afterClosed().subscribe(result => {
            this.periodosService.getPeriodoAlumno(this.matricula).subscribe((resuPA: any) => {
              this.periodoActual = resuPA.descripcion;
            });
          });
        }
      });
    }
  }
}

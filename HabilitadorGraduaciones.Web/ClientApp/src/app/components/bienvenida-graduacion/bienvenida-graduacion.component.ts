import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Correo } from '../../interfaces/Correo';
import { ModalConfirmConfiguration } from '../../interfaces/ModalConfirmConfiguration';
import { Usuario } from '../../interfaces/Usuario';
import { EnvioCorreoService } from '../../services/envio.correo.service';
import { PeriodosService } from '../../services/periodos.service';
import { RegistroExitosoGraduacionComponent } from '../registro-exitoso-graduacion/registro-exitoso-graduacion.component';
import { EnumTipoCorreo } from 'src/app/enums/EnumTipoCorreo';
import { NotificacionesService } from '../../services/notificaciones.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { TranslateService } from '@ngx-translate/core';
import { UtilsService } from 'src/app/services/utils.service';
import { Periodo } from 'src/app/interfaces/Periodos';
import { EnumOrigenCambioPeriodo } from 'src/app/enums/EnumOrigenCambioPeriodo';
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';
import { Endpoints } from 'src/app/interfaces/Endpoints';

@Component({
  selector: 'app-bienvenida-graduacion',
  templateUrl: './bienvenida-graduacion.component.html',
  styleUrls: ['./bienvenida-graduacion.component.css']
})

export class BienvenidaGraduacionComponent implements OnInit {

  public usuario: Usuario = <Usuario>{};
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};
  mensaje: any;
  usuarioLogin: string = this.usuario.matricula;
  periodo: string = '';
  periodoId: string = '';
  public correo: Correo = <Correo>{};
  movilBienvenida: boolean = false;
  periodoGuardar: Periodo = <Periodo>{};
  
  paramsEndpoints: Endpoints = <Endpoints>{};

  constructor(public dialog: MatDialog, private periodosService: PeriodosService, private enviarCorreo: EnvioCorreoService,
              private notificacionService: NotificacionesService, @Inject(MAT_DIALOG_DATA) public data: any,
              private responsive: BreakpointObserver,private translate: TranslateService,private utilsService: UtilsService) {
  }


  ngOnInit() {
    this.usuario = this.data.objeto;
    this.usuarioLogin = this.usuario.matricula;
    this.paramsEndpoints = this.data.paramsEndpoints;
    this.periodosService.getPronosticoAlumno(this.paramsEndpoints).subscribe((resuPA: any) => {
      this.periodo = resuPA.descripcion;
      this.periodoId = resuPA.periodoId;

      this.notificacionService.bienvenidoGraduacion(this.usuario.matricula, EnumTipoCorreo.BienvenidaProspectos).subscribe((r: any) => {
        if (r.result) {
          this.correo.destinatario = this.usuario.correo;
          this.correo.asunto = '¡Bienvenido(a)! A la recta final de tu carrera';
          this.correo.cuerpo = '¡Bienvenido(a)! A la recta final de tu carrera<br/>'+'Hola ' + this.usuario.nombre + '<div>Hemos detectado que te acercas a tu meta de graduación.</div><br/>' +
            '<div>Según tu avance en tus unidades de formación, tenemos pronosticado que tu periodo de graduación será en:</div>' + this.periodo;
          this.enviarCorreo.enviarCorreo(this.correo); 
        }
      });

    
    });

    this.responsive.observe([Breakpoints.XSmall, Breakpoints.HandsetLandscape, Breakpoints.TabletPortrait, Breakpoints.Handset, Breakpoints.HandsetLandscape,
    Breakpoints.Small, Breakpoints.Tablet, Breakpoints.Medium, Breakpoints.Web])
      .subscribe(result => {
        const breakpoints = result.breakpoints;
        this.movilBienvenida = false;
        //movilBienvenida
        if (breakpoints[Breakpoints.XSmall] || breakpoints[Breakpoints.TabletPortrait] || breakpoints[Breakpoints.HandsetPortrait]
          || breakpoints[Breakpoints.Small] || breakpoints[Breakpoints.TabletPortrait]) {
          this.movilBienvenida = true;
        }
      });   
  }

  abrirModalSelecionPeriodoPCG(origen: string): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '570px';
    dialogConfig.minHeight = '489px';
    dialogConfig.height = 'auto';

    origen == 'ModificarFecha';
    let tituloModificarFecha = "";
    this.translate.get("modificarFecha.encabezado").subscribe((result)=> {
      tituloModificarFecha = result;
    });
    this.configuracionModal.titulo = tituloModificarFecha;
    this.configuracionModal.isSelectCampus = false;
    this.configuracionModal.isCambioFecha = true;
    dialogConfig.data = {
      configuracion: this.configuracionModal,
      usuario: this.usuarioLogin,
      objeto: this.usuario,
      periodoActual : '',
      periodoGuardado: false,
      paramsEndpoints : this.data.paramsEndpoints
    };

    const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
    });
  }

  abrirModalRegistroExitosoGP(): void {
    const dialogRef = this.dialog.open(RegistroExitosoGraduacionComponent, {
      width: '570px',
      height: '489px',
      disableClose: true,
      data: { periodo: this.periodo, usuario: this.usuario, intensivo : false }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
    });
  }

  guardarPeriodo(): void {
    this.periodoGuardar = <Periodo>{};
    this.periodoGuardar.periodoElegido = this.periodoId;
    this.periodoGuardar.matricula = this.usuarioLogin;
    this.periodoGuardar.periodoEstimado = this.periodoId;
    this.periodoGuardar.periodoCeremonia = this.periodoId;
    this.periodoGuardar.origenActualizacionPeriodoId = EnumOrigenCambioPeriodo.BienvenidaCandidato;
    this.periodosService.guardarPeriodoAlumno(this.periodoGuardar).subscribe((resuRE: any) => {
      this.abrirModalRegistroExitosoGP();
    });
  }
}

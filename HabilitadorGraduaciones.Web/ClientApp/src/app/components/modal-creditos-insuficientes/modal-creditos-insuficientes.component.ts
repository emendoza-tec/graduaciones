import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Correo, EnumTipoCorreo, ModalConfirmConfiguration, Usuario } from 'src/app/classes';
import { EnvioCorreoService } from 'src/app/services/envio.correo.service';
import { LogService } from 'src/app/services/log.service';
import { NotificacionesService } from 'src/app/services/notificaciones.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';

@Component({
  selector: 'app-modal-creditos-insuficientes',
  templateUrl: './modal-creditos-insuficientes.component.html',
  styleUrls: ['./modal-creditos-insuficientes.component.css']
})
export class ModalCreditosInsuficientesComponent implements OnInit {
movil : boolean = false;
public usuario: Usuario = <Usuario>{};
public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};
periodo: string = "";
periodoId: string = "";
public correo: Correo = <Correo>{};

  constructor(private responsive: BreakpointObserver, private translate: TranslateService, private enviarCorreo: EnvioCorreoService,
    public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any, private notificacionService: NotificacionesService, private logService:LogService) { }

  ngOnInit(): void {
    this.usuario = this.data.objeto;
    this.periodo = this.data.periodo;
    this.periodoId = this.data.periodoId;
    this.responsive.observe([Breakpoints.XSmall, Breakpoints.HandsetLandscape, Breakpoints.TabletPortrait, Breakpoints.Handset, Breakpoints.HandsetLandscape,
      Breakpoints.Small, Breakpoints.Tablet, Breakpoints.Medium, Breakpoints.Web])
        .subscribe(result => {
          const breakpoints = result.breakpoints;
          this.movil = false;
          if (breakpoints[Breakpoints.XSmall] || breakpoints[Breakpoints.TabletPortrait] || breakpoints[Breakpoints.HandsetPortrait]
            || breakpoints[Breakpoints.Small] || breakpoints[Breakpoints.TabletPortrait]) {
            this.movil = true;
          }
        });
        
    this.notificacionService.guardarNotificacion("Requisito de Plan de Estudio", "Tus créditos inscritos no alcanzan para completar tu plan de estudios, favor de revisar tu situación o cambiar el periodo de graduación",  this.usuario.matricula).subscribe();
    this.notificacionService.isCorreoEnviado(EnumTipoCorreo.EnteradoCreditosInsuficientes, this.usuario.matricula).subscribe((res: any) => {
      if (!res.result) {
        this.correo.destinatario = this.usuario.correo;
        this.correo.asunto = 'Requisito de Plan de Estudio';
        this.correo.cuerpo = 'Hola <b>' + this.usuario.nombre + '</b><div>Tus créditos inscritos no alcanzan para completar tu plan de estudios, favor de revisar tu situación o cambiar el periodo de graduación</div><br/>';
        this.enviarCorreo.enviarCorreo(this.correo);
        this.notificacionService.guardarNotificacionCorreo(EnumTipoCorreo.EnteradoCreditosInsuficientes, this.usuario.matricula).subscribe((resuGNC: any) => { });
       
      }
    });
  }

  abrirModalSelecionPeriodo(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '570px';
    dialogConfig.height = '489px';

    let tituloModificarFecha = "";
    this.translate.get("modificarFecha.encabezado").subscribe((result)=> {
      tituloModificarFecha = result;
    });
    this.configuracionModal.titulo = tituloModificarFecha;
    this.configuracionModal.isSelectCampus = false;
    this.configuracionModal.isCambioFecha = true;
    dialogConfig.data = {
      configuracion: this.configuracionModal,
      usuario: this.usuario.matricula,
      objeto: this.usuario,
      periodoActual : '',
      periodoGuardado: false,
      paramsEndpoints : this.data.paramsEndpoints
    };

    const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
    });
  }

  guardarEnterado(): void{
   this.logService.guardarLog(this.usuario.matricula, this.periodo, this.periodoId).subscribe();
   this.dialog.closeAll();
  }
}

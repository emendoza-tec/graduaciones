import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { UtilsService } from 'src/app/services/utils.service';
import { ModalConfirmConfiguration } from '../../interfaces/ModalConfirmConfiguration';
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';
import { Correo, EnumTipoCorreo, Usuario } from 'src/app/classes';
import { NotificacionesService } from 'src/app/services/notificaciones.service';
import { PeriodosService } from 'src/app/services/periodos.service';

@Component({
  selector: 'app-registro-exitoso-graduacion',
  templateUrl: './registro-exitoso-graduacion.component.html',
  styleUrls: ['./registro-exitoso-graduacion.component.css']
})
export class RegistroExitosoGraduacionComponent implements OnInit {
  @Input() usuario: any;
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};
  mensaje: any;
  public correo: Usuario = <Usuario>{};
  
  constructor(public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any, 
  private translate: TranslateService, private utilsService: UtilsService, public dialogRef: MatDialogRef<ModificarFechaGraduacionComponent>,
  private notificacionService: NotificacionesService, private periodoService: PeriodosService) { }

  ngOnInit(): void {
    this.correo.nombre = this.data.usuario.nombre + ' ' + this.data.usuario.apeidoPaterno + ' ' + this.data.usuario.apeidoMaterno;
    this.correo.periodoGraduacion = this.data.periodo;
    this.correo.correo = this.data.usuario.correo;
    this.periodoService.enviarCorreo(this.correo);
    this.notificacionService.guardarNotificacionCorreo(EnumTipoCorreo.PeriodoGraduacion, this.data.usuario.matricula).subscribe();
  }


  abrirModalSelecionPeriodoPCG(origen: string): void {
    this.dialogRef.close();
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '570px';
    dialogConfig.height = 'auto';

    origen == 'ModificarFecha';let tituloModificarFecha = "";
    this.translate.get("modificarFecha.encabezado").subscribe((result)=> {
      tituloModificarFecha = result;
    });
    this.configuracionModal.titulo = tituloModificarFecha;
    this.configuracionModal.isSelectCampus = false;
    this.configuracionModal.isCambioFecha = true;
    dialogConfig.data = {
      configuracion: this.configuracionModal,
      usuario: this.data.usuario.matricula,
      objeto: this.data.usuario,
      periodoActual : '',
      periodoGuardado: false
    };
    const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
    });

  }

  aceptarPeriodo(): void {
    this.dialog.closeAll();
  }
}

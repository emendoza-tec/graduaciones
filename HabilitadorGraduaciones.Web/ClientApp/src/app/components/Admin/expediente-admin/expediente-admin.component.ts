import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ExpedienteService } from 'src/app/services/requisitos/expediente.service';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { EnumTipoProceso } from '../../../enums/EnumTipoProceso';
import { EnumTipoAnalisisExpediente } from '../../../enums/EnumTipoAnalisisExpediente';
import { tipoProceso } from '../../../constants/TipoProcesos';
import { UtilsService } from 'src/app/services/utils.service';
import { ExcelService } from 'src/app/services/exportarExcel.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';

@Component({
  selector: 'app-expediente-admin',
  templateUrl: './expediente-admin.component.html',
  styleUrls: ['./expediente-admin.component.css']
})

export class ExpedienteAdminComponent {
  archivo?: File;
  archivoCargado = false;
  isAnalizado = false;
  hasError = true;
  disableAction = false;
  isProcesoActivado = false;
  procesos: any[] = [];
  listaProcesos: any[] = [];
  AnalisisProcesos= tipoProceso;
  archivoPlantilla = 'api/archivo/download?fileName=TemplateExpediente.xlsx';
  duplicados = false;
  comentarioVacio = false;
  isAnalizandoData = false;
  isSavingData = false;
  permisoExpediente: PermisosMenu;

  //Resultado Procesado del Excel
  alumnosDuplicados: any[] = [];
  expedientesaModificar: any[] = [];
  expedientesNuevos: any[] = [];
  expedientesaCompleto: any[] = [];
  expedientesdeCompleto: any[] = [];
  expedientesIncompletoSinDetalle: any[] = [];
  expedientesErrorFiltro: any[]= [];

  private static tipoAnalisis = [
    { Descripcion: 'Alumnos encontrados' },
    { Descripcion: 'Alumnos duplicados en la plantilla' },
    { Descripcion: 'No. de registros actualizados' },
    { Descripcion: 'No. de registros nuevos' },    
    { Descripcion: 'No. de registros que cambiaron estatus de incompleto y en revisión a completo' },
    { Descripcion: 'No. de registros que cambiaron estatus de completo a incompleto o en revisión' },
    { Descripcion: 'No. de registros con estatus incompleto que no contienen mensaje' },
    { Descripcion: 'No. de registros con rechazo por campus, sede y nivel' },
  ];

  constructor(public dialog: MatDialog, private expedienteService: ExpedienteService, private pnService: PermisosNominaService,private spinner: NgxSpinnerService,
    private snackBarService: SnackBarService,private utilsService: UtilsService, private excelService: ExcelService) { 
      this.permisoExpediente = this.pnService.revisarSubmenuPermisoPorId(5,6);
  }

  procesaExcel() {
    this.archivoCargado = false;
    this.isAnalizado = true;
    this.isAnalizandoData = true;
    this.spinner.show();
    this.AnalisisProcesos[EnumTipoProceso.Analizando].Estado = true;
    if (this.archivo) {
      this.excelService.expExcelToJson(this.archivo).subscribe(r => {
        if (r) {
          this.AnalisisProcesos[EnumTipoProceso.Preparando].Estado = true;
          this.procesos.push({
            id: EnumTipoAnalisisExpediente.Encontrados,
            key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Encontrados].Descripcion,
            value: r.totalRegistros
          });
          let totalAlumnosDuplicados = this.getDuplicates(r.procesaExpediente);
          this.alumnosDuplicados = this.removeDuplicates(totalAlumnosDuplicados);
          if (this.alumnosDuplicados.length > 0) {
            this.duplicados = true;
            this.hasError = true;
            this.procesos.push({
              id: EnumTipoAnalisisExpediente.Duplicados,
              key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Duplicados].Descripcion,
              value: this.alumnosDuplicados.length, verMas: '<i class="bi bi-eye"></i> ver'
            });
            this.snackBarService.openSnackBar('Por favor revise los alumnos duplicados en su plantilla', 'default', 5000);
          } else {
            this.AnalisisProcesos[EnumTipoProceso.Enviado].Estado = true;
            if (this.archivo) {
              const idUsuario = this.pnService.obtenIdUsuario();
              this.expedienteService.procesaExcel(this.archivo, idUsuario).subscribe((procesadoExcel: any) => {
                this.expedientesaModificar = procesadoExcel.ExpedienteAtualizados;
                this.expedientesNuevos = procesadoExcel.ExpedienteNuevos;
                this.expedientesaCompleto = procesadoExcel.ExpedienteCambioaCompleto;
                this.expedientesdeCompleto = procesadoExcel.ExpedienteCambiodeCompleto;
                this.expedientesIncompletoSinDetalle = procesadoExcel.ExpedienteIncompletoSinDetalle;
                this.expedientesErrorFiltro = procesadoExcel.ExpedienteErrorFiltro;

                this.procesos.push({ id: EnumTipoAnalisisExpediente.Actualizados, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Actualizados].Descripcion, value: !this.expedientesaModificar ? 0 : this.expedientesaModificar.length });
                this.procesos.push({ id: EnumTipoAnalisisExpediente.Nuevos, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Nuevos].Descripcion, value: !this.expedientesNuevos ? 0 : this.expedientesNuevos.length });
                this.procesos.push({ id: EnumTipoAnalisisExpediente.DiferentesNc, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.DiferentesNc].Descripcion, value: !this.expedientesaCompleto ? 0 : this.expedientesaCompleto.length, verMas: '<i class="bi bi-eye"></i> ver' });
                this.procesos.push({ id: EnumTipoAnalisisExpediente.DiferentesSc, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.DiferentesSc].Descripcion, value: !this.expedientesdeCompleto ? 0 : this.expedientesdeCompleto.length, verMas: '<i class="bi bi-eye"></i> ver' });
                this.procesos.push({ id: EnumTipoAnalisisExpediente.SinMensajesIncompleto, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.SinMensajesIncompleto].Descripcion, value: !this.expedientesIncompletoSinDetalle ? 0 : this.expedientesIncompletoSinDetalle.length, verMas: '<i class="bi bi-eye"></i> ver' });
                this.procesos.push({ id: EnumTipoAnalisisExpediente.ErrorFiltro, key: ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.ErrorFiltro].Descripcion, value: !this.expedientesErrorFiltro ? 0 : this.expedientesErrorFiltro.length, verMas: '<i class="bi bi-eye"></i> ver' });

                this.AnalisisProcesos[EnumTipoProceso.Procesado].Estado = true;   
                
                if (this.expedientesNuevos.length > 0 || this.expedientesaModificar.length > 0 || this.expedientesaCompleto.length > 0 || this.expedientesdeCompleto.length > 0) {
                  this.hasError = false;
                }

                if(this.expedientesIncompletoSinDetalle.length > 0){
                  this.hasError = true;
                  this.snackBarService.openSnackBar('Los alumnos con estatus incompleto no pueden ir sin comentario', 'default', 5000);
                }

                if(this.expedientesErrorFiltro.length > 0){
                  this.hasError = true;
                  this.snackBarService.openSnackBar('Los alumnos con rechazo por campus, sede y nivel', 'default', 5000);
                }

                this.isAnalizandoData = false;
                this.spinner.hide()
              },
                (error: HttpErrorResponse) => {
                  this.hasError = false;
                  this.isAnalizandoData = false;
                  this.spinner.hide();
                  this.snackBarService.openSnackBar(error.message, 'default');
                });
            }
          }
        }
      },
        (err: Error) => {
          this.isAnalizado = false;
          this.isAnalizandoData = false;
          this.spinner.hide()
          this.snackBarService.openSnackBar(err.message, 'default', 5000);
        });
    } else {
      this.procesos.push({ key: "Error al leer los datos", value: "" });
    }
  }

  guardaExpedientes() : void {
    this.disableAction = true;
    this.isSavingData = true;
    this.spinner.show();
    if (this.pnService.obtenNomina() != null) {
      this.hasError = true;
      if (this.archivo) {
        this.expedienteService.guardaExpedientes(this.archivo, this.pnService.obtenNomina(), this.pnService.obtenIdUsuario()).subscribe((r: any) => {
          if (r.result) {
            this.snackBarService.openSnackBar('Expedientes guardados éxitosamente', 'default');
            this.disableAction = false;
            this.isSavingData = false;
            this.spinner.hide();
          }
        }, (err: HttpErrorResponse) => {
          this.snackBarService.openSnackBar('Hubo un problema al guardar expedientes', 'default');
          this.disableAction = false;
          this.isSavingData = false;
          this.spinner.hide();
        });
      }
    }
    else {
      this.snackBarService.openSnackBar('La sesión a espirado .', 'default');
      window.location.replace(`${window.location.origin}/Auth/Login`);
      
    }     
  }

  onFileDropped($event: any) : void {
    if ($event.lenght > 0) {
      this.snackBarService.openSnackBar('solo un archivo por carga es permitido', 'default');
    } else {
      this.archivoCargado = true;
    }
  }

  fileBrowseHandler(files: any): void {
    if (files.target.lenght > 0) {
      this.snackBarService.openSnackBar('solo un archivo por carga es permitido', 'default');
    } else {
      if (files.target.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
        this.archivo = files.target.files[0];
        this.archivoCargado = true;
      } else {
        this.snackBarService.openSnackBar('Template incorrecto', 'default');
      }
    }
  }

  resetEvent($event: any): void {
    this.procesos.length = 0;
    $event.target.value = '';
    this.isAnalizado = false;
    this.AnalisisProcesos[EnumTipoProceso.Analizando].Estado = false;
    this.AnalisisProcesos[EnumTipoProceso.Preparando].Estado = false;
    this.AnalisisProcesos[EnumTipoProceso.Enviado].Estado = false;
    this.AnalisisProcesos[EnumTipoProceso.Procesado].Estado = false;
  }

  formatBytes(bytes: any) {
    return this.utilsService.formatBytes(bytes);
  }

  getDuplicates(data: any[]) {

    let uniqueChars: any[] = [];
    data.forEach((c) => {
      let uC = data.filter(f => f.Matricula === c.Matricula)
      if (uC.length > 1) {
        let duplicate = data.find(a => a.Matricula === c.Matricula);
        uniqueChars.push(duplicate);
      }
    });
    return uniqueChars;
  }

  removeDuplicates(data: any[]) {

    let uniqueChars: any[] = [];
    data.forEach((c) => {
      let uC = uniqueChars.filter(f => f.Matricula === c.Matricula)
      if (uC.length === 0) {
        uniqueChars.push(c);
      }
    });

    return uniqueChars;
  }

  verMas(id: number) {
    let titulo = "";
    let alumnosDialog: any[] = [];
    switch (id) {
      case EnumTipoAnalisisExpediente.Duplicados: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Duplicados].Descripcion;
        alumnosDialog = this.alumnosDuplicados;
        break;
      }
      case EnumTipoAnalisisExpediente.Actualizados: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Actualizados].Descripcion;
        alumnosDialog = this.expedientesaModificar;
        break;
      }
      case EnumTipoAnalisisExpediente.Nuevos: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.Nuevos].Descripcion;
        alumnosDialog = this.expedientesNuevos;
        break;
      }
      case EnumTipoAnalisisExpediente.DiferentesNc: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.DiferentesNc].Descripcion;
        alumnosDialog = this.expedientesaCompleto;
        break;
      }
      case EnumTipoAnalisisExpediente.DiferentesSc: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.DiferentesSc].Descripcion;
        alumnosDialog = this.expedientesdeCompleto;
        break;
      }
      case EnumTipoAnalisisExpediente.SinMensajesIncompleto: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.SinMensajesIncompleto].Descripcion;
        alumnosDialog = this.expedientesIncompletoSinDetalle;
        break;
      }
      case EnumTipoAnalisisExpediente.ErrorFiltro: {
        titulo = ExpedienteAdminComponent.tipoAnalisis[EnumTipoAnalisisExpediente.ErrorFiltro].Descripcion;
        alumnosDialog = this.expedientesErrorFiltro;
        break;
      }
    }
    let dialogRef = this.dialog.open(alumnosDuplicadosDialog, {
      width: '500px',
      data: { alumnos: alumnosDialog, titulo: titulo }
    });

    dialogRef.afterClosed().subscribe();
  }
}

@Component({
  selector: 'alumnosDuplicadosModal',
  templateUrl: 'alumnosDuplicadosModal.html',
})

export class alumnosDuplicadosDialog {
  constructor(public dialogRef: MatDialogRef<alumnosDuplicadosDialog>, @Inject(MAT_DIALOG_DATA) public data: any
  ) {
  }

  cancelar() {
    this.dialogRef.close();
  }
}

import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject} from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { filter } from 'rxjs';
import { ExamenIntegradorService } from 'src/app/services/requisitos/examenIntegrador.service';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { EnumTipoProceso } from '../../../enums/EnumTipoProceso';
import { EnumTipoAnalisisExIntegrador } from '../../../enums/EnumTipoAnalisisExIntegrador';
import { tipoProceso } from '../../../constants/TipoProcesos';
import { UtilsService } from 'src/app/services/utils.service';
import { ExcelService } from 'src/app/services/exportarExcel.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';

@Component({
  selector: 'app-examenIntegrador-admin',
  templateUrl: './examenIntegrador-admin.component.html',
  styleUrls: ['./examenIntegrador-admin.component.css']
})
export class ExamenIntegradorAdminComponent {
  archivo?: File;
  disableAction = false;
  archivoCargado = false;
  isAnalizado = false;
  isAnalizandoData = false;
  isSavingData = false;
  hasError = true;
  isProcesoActivado = false;
  AnalisisProcesos = tipoProceso;
  procesos: any[] = [];
  listaProcesos: any[] = [];
  alumnosDuplicados: any[] = [];
  permisoExIntegrador: PermisosMenu;

  alumnosNuevos: any[] = [];
  alumnosAModificar: any[] = [];
  alumnosDiferenteASc: any[] = []; //diferentes a si cumple
  alumnosDiferenteANc: any[] = []; //diferentes a no cumple

  alumnosFormatoInvalido: any[] = []; //formato invalido
  alumnosNumeroInvalido: any[] = []; //numero invalidado
  alumnosAnioInvalido: any[] = []; //año invalidado
  alumnosPeriodoInvalido: any[] = []; //periodo invalidado
  alumnosFechaInvalida: any[] = []; //fecha invalidado
  alumnosErrorFiltro: any[] = [];
  archivoPlantilla = 'api/archivo/download?fileName=TemplateExamenIntegrador.xlsx';

  private static tipoAnalisis = [
    { Descripcion: 'Alumnos Encontrados' },
    { Descripcion: 'Alumnos duplicados en la plantilla' },
    { Descripcion: 'Alumnos con formato periodo no válido' },
    { Descripcion: 'Alumnos con periodo antiguo no válido' },
    { Descripcion: 'Alumnos con periodo no válido' },
    { Descripcion: 'Alumnos Nuevos' },
    { Descripcion: 'Alumnos a Modificar' },
    { Descripcion: 'Alumnos que cambiaron estatus NC a SC o NP' },
    { Descripcion: 'Alumnos que cambiaron estatus SC a NC o NP' },
    { Descripcion: 'Alumnos con fecha no válida' },
    { Descripcion: 'Alumnos con rechazo por campus, sede y nivel' },
  ];

  constructor(public dialog: MatDialog, private snackBarService: SnackBarService, private spinner: NgxSpinnerService,
    private examenIntegradorSercice: ExamenIntegradorService, private utilsService: UtilsService, private excelService: ExcelService, private pnService: PermisosNominaService) {
      this.permisoExIntegrador = this.pnService.revisarSubmenuPermisoPorId(5,8);
  }

  procesaExcel() {
    this.archivoCargado = false;
    this.isAnalizado = true;
    this.isAnalizandoData = true;
    this.spinner.show();
    this.AnalisisProcesos[EnumTipoProceso.Analizando].Estado = true;
    if (this.archivo) {
      this.excelService.exIntExcelToJson(this.archivo).pipe(filter((r) => !!r)).subscribe({
        next: (r: any) => {
          this.AnalisisProcesos[EnumTipoProceso.Preparando].Estado = true;
          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.Encontrados, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.Encontrados].Descripcion, value: r.totalRegistros });
          let totalAlumnosDuplicados = this.getDuplicates(r.procesaExamenIntegrador);
          this.alumnosDuplicados = this.removeDuplicates(totalAlumnosDuplicados);
          if (this.alumnosDuplicados.length > 0) {
            this.hasError = true;
            this.procesos.push({ id: EnumTipoAnalisisExIntegrador.Duplicados, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.Duplicados].Descripcion, value: this.alumnosDuplicados.length, verMas: '<i class="bi bi-eye"></i> ver' });
            this.snackBarService.openSnackBar('Por favor revise los alumnos duplicados en su plantilla', 'default', 5000);
          } else {
            this.AnalisisProcesos[EnumTipoProceso.Enviado].Estado = true;
            //sigue enviarlo a procesar en el api
            this.enviaExcelAProcesar(this.archivo!);
          }
        },
        error: (err: Error) => {
          this.isAnalizado = false;
          this.isAnalizandoData = false;
          this.spinner.hide();
          this.snackBarService.openSnackBar(err.message, 'default', 5000);
        }
      });
    } else {
      this.snackBarService.openSnackBar("Error al leer los datos", 'default', 5000);
    }
  }

  enviaExcelAProcesar(archivoEnvio: File) {
    this.examenIntegradorSercice.procesaExcel(archivoEnvio, this.pnService.obtenIdUsuario()).pipe(filter((res) => !!res)).subscribe({
      next: (res: any) => {
        if (res.ExamenesIntegradorNuevos || res.ExamenesIntegradorAModificar || res.ExamenesIntegradorNcAScYNP || res.ExamenesIntegradorScANcYNP) {
          this.alumnosNuevos = res.ExamenesIntegradorNuevos;
          this.alumnosAModificar = res.ExamenesIntegradorAModificar;
          this.alumnosDiferenteANc = res.ExamenesIntegradorNcAScYNP;
          this.alumnosDiferenteASc = res.ExamenesIntegradorScANcYNP;
          this.alumnosErrorFiltro = res.ExamenesIntegradorErrorFiltro;

          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.Nuevos, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.Nuevos].Descripcion, value: !this.alumnosNuevos ? 0 : this.alumnosNuevos.length });
          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.Modificados, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.Modificados].Descripcion, value: !this.alumnosAModificar ? 0 : this.alumnosAModificar.length });
          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.DiferentesNc, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.DiferentesNc].Descripcion, value: !this.alumnosDiferenteANc ? 0 : this.alumnosDiferenteANc.length, verMas: '<i class="bi bi-eye"></i> ver' });
          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.DiferentesSc, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.DiferentesSc].Descripcion, value: !this.alumnosDiferenteASc ? 0 : this.alumnosDiferenteASc.length, verMas: '<i class="bi bi-eye"></i> ver' });
          this.procesos.push({ id: EnumTipoAnalisisExIntegrador.ErrorFiltro, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.ErrorFiltro].Descripcion, value: !this.alumnosErrorFiltro ? 0 : this.alumnosErrorFiltro.length, verMas: '<i class="bi bi-eye"></i> ver' });

          if (this.alumnosNuevos.length > 0 || this.alumnosAModificar.length > 0 || this.alumnosDiferenteANc.length > 0 || this.alumnosDiferenteASc.length > 0) {
            this.hasError = false;
          }
        }

        this.validaPeriodoGraduacion(res.ExamenesIntegradorFormatoInvalido, res.ExamenesIntegradorNumeroInvalido, res.ExamenesIntegradorAnioInvalido, res.ExamenesIntegradorPeriodoInvalido);

        this.alumnosFechaInvalida = res.ExamenesIntegradorFechaInvalida;
        this.procesos.push({ id: EnumTipoAnalisisExIntegrador.FechaInvalida, key: ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.FechaInvalida].Descripcion, value: !this.alumnosFechaInvalida ? 0 : this.alumnosFechaInvalida.length, verMas: '<i class="bi bi-eye"></i> ver' });

        if (this.alumnosFechaInvalida.length > 0) {
          this.hasError = true;
        }

        if(this.alumnosErrorFiltro.length > 0){
          this.hasError = true;
          this.snackBarService.openSnackBar('Alumnos con rechazo por campus, sede y nivel', 'default', 5000);
        }
        this.AnalisisProcesos[EnumTipoProceso.Procesado].Estado = true;
        this.isAnalizandoData = false;
        this.spinner.hide()
      },
      error: (error: HttpErrorResponse) => {
        this.hasError = false;
        this.snackBarService.openSnackBar(error.message, 'default');
        this.isAnalizandoData = false;
        this.spinner.hide();
      }
    });
  }

  validaPeriodoGraduacion(examenesIntegradorFormatoInvalido: any[], examenesIntegradorNumeroInvalido: any[], examenesIntegradorAnioInvalido: any[], examenesIntegradorPeriodoInvalido: any[]) {
    if (examenesIntegradorFormatoInvalido.length > 0 && examenesIntegradorNumeroInvalido.length > 0 && examenesIntegradorAnioInvalido.length > 0 && examenesIntegradorPeriodoInvalido.length > 0) {
      this.alumnosFormatoInvalido = examenesIntegradorFormatoInvalido.concat(examenesIntegradorNumeroInvalido);
      this.alumnosAnioInvalido = examenesIntegradorAnioInvalido;
      this.alumnosPeriodoInvalido = examenesIntegradorPeriodoInvalido;

      this.procesos.push({ id: EnumTipoAnalisisExIntegrador.FormatoInvalido, key: ExamenIntegradorAdminComponent.tipoAnalisis[2].Descripcion, value: !this.alumnosFormatoInvalido ? 0 : this.alumnosFormatoInvalido.length, verMas: '<i class="bi bi-eye"></i> ver' });
      this.procesos.push({ id: EnumTipoAnalisisExIntegrador.PeriodoAnioInvalido, key: ExamenIntegradorAdminComponent.tipoAnalisis[3].Descripcion, value: !this.alumnosAnioInvalido ? 0 : this.alumnosAnioInvalido.length, verMas: '<i class="bi bi-eye"></i> ver' });
      this.procesos.push({ id: EnumTipoAnalisisExIntegrador.PeriodoInvalido, key: ExamenIntegradorAdminComponent.tipoAnalisis[4].Descripcion, value: !this.alumnosPeriodoInvalido ? 0 : this.alumnosPeriodoInvalido.length, verMas: '<i class="bi bi-eye"></i> ver' });
      this.hasError = true;
    }
  }

  guardaExamenesIntegrador() {
    this.disableAction = true;
    this.isSavingData = true;
    this.spinner.show();
    if (this.pnService.obtenNomina() != null) {
      this.hasError = true;
      if (this.archivo) {
        this.examenIntegradorSercice.guardaExamenesIntegrador(this.archivo, this.pnService.obtenNomina(), this.pnService.obtenIdUsuario()).subscribe({
          next: (r: any) => {
            if (r.result) {
              this.snackBarService.openSnackBar('Registros de Examen Integrador guardados éxitosamente', 'default');
              this.disableAction = false;
              this.isSavingData = false;
              this.spinner.hide();
            }
          },
            error: (error: HttpErrorResponse) => {
              console.error(error);
              this.snackBarService.openSnackBar('Carga de archivo de estatus de requisito Examen integrador. Estatus inexistentes.', 'default');
              this.disableAction = false;
              this.isSavingData = false;
              this.spinner.hide();
            }
        });
      }
    }
    else {
      this.snackBarService.openSnackBar('La sesión a espirado .', 'default');
      window.location.replace(`${window.location.origin}/Auth/Login`);
    }
  }

  onFileDropped($event: any) {
    if ($event.lenght > 0) {
      this.snackBarService.openSnackBar('solo un archivo por carga es permitido', 'default');
    } else {
      this.archivoCargado = true;
    }
  }

  fileBrowseHandler(files: any) {
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

  resetEvent($event: any) {
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
      case EnumTipoAnalisisExIntegrador.Duplicados: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.Duplicados].Descripcion;
        alumnosDialog = this.alumnosDuplicados;
        break;
      }
      case EnumTipoAnalisisExIntegrador.FormatoInvalido: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.FormatoInvalido].Descripcion;
        alumnosDialog = this.alumnosFormatoInvalido;
        break;
      }
      case EnumTipoAnalisisExIntegrador.PeriodoAnioInvalido: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.PeriodoAnioInvalido].Descripcion;
        alumnosDialog = this.alumnosAnioInvalido;
        break;
      }
      case EnumTipoAnalisisExIntegrador.PeriodoInvalido: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.PeriodoAnioInvalido].Descripcion;
        alumnosDialog = this.alumnosPeriodoInvalido;
        break;
      }
      case EnumTipoAnalisisExIntegrador.DiferentesNc: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.DiferentesNc].Descripcion;
        alumnosDialog = this.alumnosDiferenteANc;
        break;
      }
      case EnumTipoAnalisisExIntegrador.DiferentesSc: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.DiferentesSc].Descripcion;
        alumnosDialog = this.alumnosDiferenteASc;
        break;
      }
      case EnumTipoAnalisisExIntegrador.FechaInvalida: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.FechaInvalida].Descripcion;
        alumnosDialog = this.alumnosFechaInvalida;
        break;
      }
      case EnumTipoAnalisisExIntegrador.ErrorFiltro: {
        titulo = ExamenIntegradorAdminComponent.tipoAnalisis[EnumTipoAnalisisExIntegrador.ErrorFiltro].Descripcion;
        alumnosDialog = this.alumnosErrorFiltro;
        break;
      }
    }
    let dialogRef = this.dialog.open(AlumnosDuplicadosExamenIntegradorDialog, {
      width: '500px',
      data: { alumnos: alumnosDialog, titulo: titulo }
    });

    dialogRef.afterClosed().subscribe();
  }

}

@Component({
  selector: 'alumnosDuplicadosExamenIntegradorModal',
  templateUrl: 'alumnosDuplicadosModal.html',
})

export class AlumnosDuplicadosExamenIntegradorDialog {
  constructor(public dialogRef: MatDialogRef<AlumnosDuplicadosExamenIntegradorDialog>, @Inject(MAT_DIALOG_DATA) public data: any
  ) {

  }

  cancelar() {
    this.dialogRef.close();
  }
}


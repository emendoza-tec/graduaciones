import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Periodo } from 'src/app/interfaces/Periodos';
import { PeriodosService } from 'src/app/services/periodos.service';
import { Correo } from '../../interfaces/Correo';
import { ModalCeremoniaGraduacionComponent } from '../modal-ceremonia-graduacion/modal-ceremonia-graduacion.component';
import { RegistroExitosoGraduacionComponent } from '../registro-exitoso-graduacion/registro-exitoso-graduacion.component';
import { UtilsService } from 'src/app/services/utils.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EnumOrigenCambioPeriodo } from 'src/app/enums/EnumOrigenCambioPeriodo';
import { Endpoints } from 'src/app/interfaces/Endpoints';

@Component({
  selector: 'app-modificar-fecha-graduacion',
  templateUrl: './modificar-fecha-graduacion.component.html',
  styleUrls: ['./modificar-fecha-graduacion.component.css']
})

export class ModificarFechaGraduacionComponent implements OnInit {

  loading: boolean = false;
  public listPeriodos: Periodo[] = [];
  usuario: any;
  @Input() periodo: any;
  descripcion: string = '';
  id: string = '';
  isRegualar: boolean = false;
  mostrarMotivo: boolean = false;
  public correo: Correo = <Correo>{};
  periodoGuardar: Periodo = <Periodo>{};
  periodoEstimado: string = '';
  motivo: string = '';
  validacionMotivo: boolean = true;
  caracteres: number = 0;

  paramsEndpoints: Endpoints = <Endpoints>{};

  constructor(private periodoService: PeriodosService, public dialogRef: MatDialogRef<ModificarFechaGraduacionComponent>, public dialog: MatDialog,
    private spinner: NgxSpinnerService, @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit(): void {
    this.data.periodoGuardado = false;
    this.paramsEndpoints = this.data.paramsEndpoints;
    this.usuario = this.data.objeto;
    this.loading = true;
    this.spinner.show();
    this.periodoService.getPeriodos(this.paramsEndpoints).subscribe((r) => {
      this.listPeriodos = r as Periodo[];
      this.loading = false;
      this.spinner.hide();
    });

    this.periodoService.getPronosticoAlumno(this.paramsEndpoints).subscribe((res: any) => {
      this.periodoEstimado = res.periodoId;
    });
  }

  form = new FormGroup({
    periodo: new FormControl('', Validators.required),
    descripcion: new FormControl(),
    motivo: new FormControl()
  });

  get f() {
    return this.form.controls;
  }

  submit(): void {
    const periodo = this.form.value.periodo;
    const motivo = this.form.value.motivo;

    if (motivo == null || motivo == '') {
      this.validacionMotivo = false;
    } else {
      this.validacionMotivo = true;
      const found = this.listPeriodos.find(x => x.periodoId == periodo);
      this.descripcion = found?.descripcion as string;
      this.id = found?.periodoId as string;
      this.isRegualar = found?.isRegular as boolean;

      this.periodoGuardar = <Periodo>{};
      this.periodoGuardar.periodoElegido = periodo as string;
      this.periodoGuardar.matricula = this.usuario.matricula;
      this.periodoGuardar.periodoEstimado = this.periodoEstimado;
      this.periodoGuardar.periodoCeremonia = periodo as string;
      this.periodoGuardar.motivoCambioPeriodo = motivo as string;
      this.periodoGuardar.origenActualizacionPeriodoId = EnumOrigenCambioPeriodo.ModificacionAlumno;

      this.data.periodoGuardado = true;
      if (!this.isRegualar) {
        this.abrirValidacionNoCeremonia(this.id, this.descripcion, motivo as string, this.periodoEstimado as string);
      }
      else {
        this.periodoService.guardarPeriodoAlumno(this.periodoGuardar).subscribe(() => {
          this.registroExitoso(this.descripcion, motivo as string);
        });

        this.data.periodoGuardado = true;
      }
    }

  }

  registroExitoso(periodo: string, motivo: string): void {
    this.dialogRef.close(this.data);
    const dialogRef = this.dialog.open(RegistroExitosoGraduacionComponent, {
      width: '570px',
      height: 'auto',
      maxHeight: '100vh',
      disableClose: true,
      data: { periodo: periodo, usuario: this.usuario, motivo: motivo, intensivo: false }
    });
  }

  abrirValidacionNoCeremonia(_periodoElegidoId: string, _periodoElegido: string, motivo: string, periodoEstimado: string): void {
    this.dialogRef.close(this.data);
    const dialogRef = this.dialog.open(ModalCeremoniaGraduacionComponent, {
      width: '570px',
      height: 'auto',
      disableClose: true,
      data: {
        usuario: this.usuario, periodoElegido: _periodoElegido, periodoElegidoId: _periodoElegidoId, periodos: this.listPeriodos, motivoCambioPeriodo: motivo,
        periodoEstimado: periodoEstimado, paramsEndpoints: this.paramsEndpoints
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.periodoService.getPeriodos(this.paramsEndpoints).subscribe((r) => {
        this.listPeriodos = r as Periodo[];
      });
    });
  }

  isVeranoInvierno(periodo: string): boolean {
    if (periodo.toLocaleUpperCase().includes('INVIERNO') || periodo.toLocaleUpperCase().includes('INV') || periodo.toLocaleUpperCase().includes('VERANO')) {
      return true;
    }
    else {
      return false;
    }
  }

  onItemChange(item: any): void {
    const found = this.listPeriodos.find(x => x.periodoId == item.id);
    this.data.periodoActual = found?.descripcion as string;
    this.data.periodoGuardado = false;
    this.mostrarMotivo = true;
  }

  onKeyPress(event: any): void {
    this.validacionMotivo = true;
    if ((event.keyCode >= 47 && event.keyCode <= 90) || (event.keyCode >= 96 && event.keyCode <= 109) || (event.keyCode >= 187) || event.keyCode == 32) {
      this.caracteres++;
    }
    else {
      this.caracteres = this.motivo.length
    }
  }
  onKeyUp(event: any): void {
    this.caracteres = this.motivo.length;
  }

  onFocusOut(): void {
    this.caracteres = this.motivo.length;
  }

}

import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DatosPersonalesCorreo, Usuario } from 'src/app/interfaces/Usuario';
import { UtilsService } from 'src/app/services/utils.service';
import { CampusCeremoniaGraduacion } from '../../classes/CampusCeremoniaGraduacion';
import { Endpoints } from '../../interfaces/Endpoints';
import { CampusCeremoniaGraduacionService } from '../../services/campusCeremoniaGraduacionService';
import { DistincionesService } from '../../services/distinciones.service';
import { FiltroCampusService } from '../../services/FiltroCampus';
import { PlanDeEstudiosService, RequisitosService } from '../../services/requisitos';
import { FormDialogComponent } from '../form-dialog/form-dialog.component';
import { EnumTipoCorreo } from 'src/app/classes';
import { NotificacionesService } from 'src/app/services/notificaciones.service';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})

export class ConfirmDialogComponent implements OnInit {

  step = 0;
  public usuario: Usuario = <Usuario>{};
  form!: FormGroup;
  mensaje: any;
  periodo: string = "";
  hasMasCampus = false;
  filtroCampus: any[];
  concentraciones: any[];
  diplomasAcademicos: any[] = [];
  isGetCampusComplete = false;
  selectedCampus: string;
  paramsEndpoints: Endpoints = <Endpoints>{};
  miTec: string;
  datosCorreo: DatosPersonalesCorreo;

  constructor(public dialogRef: MatDialogRef<ConfirmDialogComponent>, private tService: RequisitosService, private utilsService: UtilsService, private planEstudiosService: PlanDeEstudiosService, private filtroCampusService: FiltroCampusService, public ccgService: CampusCeremoniaGraduacionService,
    @Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog, public fb: FormBuilder, private distService: DistincionesService, private notificacionService: NotificacionesService) { }

  ngOnInit(): void {
    this.tService.getLinks().subscribe({
      next: (links: any) => {
        this.miTec = links.datosPersonales;
      },
      error: (error: HttpErrorResponse) => {
        console.error(error);
      }
    });

    this.usuario = this.data.objeto;
    this.notificacionService.bienvenidoGraduacion(this.usuario.matricula, EnumTipoCorreo.BienvenidaCandidatos).subscribe((r: any) => {
      if (r.result) {
        this.step = 1;
      } else {
        this.step = 2;
      }
    });

    this.periodo = this.data.periodo;
    this.form = this.fb.group({
      usuarios: this.fb.array([])
    });

    const usuarioForm = this.fb.group({
      nombre: ['', Validators.required],
      curp: ['', Validators.required],
      progrmaAcademico: ['', Validators.required],
      correo: ['', Validators.required],
      diplomaAdicional: ['', Validators.required],
      telefonoCelular: ['', Validators.required],
      telefonoCasa: ['', Validators.required],
      direccion: ['', Validators.required]
    });

    this.usuarios.push(usuarioForm);

    this.paramsEndpoints =
    {
      NumeroMatricula: this.data.objeto.matricula,
      ClaveProgramaAcademico: this.data.objeto.claveProgramaAcademico,
      ClaveCarrera: this.data.objeto.carreraId,
      ClaveNivelAcademico: this.data.objeto.nivelAcademico,
      ClaveEjercicioAcademico: this.data.objeto.periodoActual,
      ClaveCampus: this.data.objeto.claveCampus,
      Correo: this.data.objeto.correo
    };

    this.concentraciones = this.data.distinciones.lstConcentracion;
    if (this.data.distinciones.hasDiploma) {
      this.diplomasAcademicos.push(this.data.distinciones.diploma);
    }
    if (this.data.distinciones.hasUllead) {
      this.diplomasAcademicos.push(this.data.distinciones.ulead);
    }
  }

  cancelar(): void {
    this.data.terminado = false;
    this.dialogRef.close();
  }

  openForm(formulario: string): void {
    this.data.terminado = false;
    this.dialogRef.addPanelClass('custom-modalbox');
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '600px';
    dialogConfig.maxHeight = '100vh';
    dialogConfig.disableClose = true;

    if (formulario == 'Usuario') {
      dialogConfig.data = {
        usuario: this.usuario,
        paramsEndpoints: this.paramsEndpoints,
        distinciones: this.data.distinciones,
        formType: 'Usuario',
        titulo: 'Editar datos personales',
        mensaje: '',
        terminado: false
      };
    }

    const dialogRef = this.dialog.open(FormDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      this.data.terminado = result.terminado;
      if (this.data.terminado) {
        this.dialogRef.close(this.data);
      }else{
        this.dialogRef.removePanelClass('custom-modalbox');
      }
    });
  }

  get usuarios() {
    return this.form.controls["usuarios"] as FormArray;
  }

  confirmarDatos(): void {    
    this.step = 3
  }

  finalizar(): void {
    this.data.terminado = true;
    this.dialogRef.close(this.data);    
  }

  continuarConfirmacion(): void {    
    this.datosCorreo = {
      nombre : this.usuario.nombre,
      apellidoPaterno : this.usuario.apeidoPaterno,
      apellidoMaterno : this.usuario.apeidoMaterno,
      curp : this.usuario.curp,
      programaAcademico : this.usuario.programaAcademico,
      correo : this.usuario.correo,
      concentracion : this.concentraciones,
      diplomasAdicionales : this.diplomasAcademicos
    }; 
    this.notificacionService.envioCorreoConfirmacion(this.datosCorreo).subscribe({
      error: (error: HttpErrorResponse) => {
        console.error(error);
      }
    });
    this.notificacionService.guardarNotificacionCorreo(EnumTipoCorreo.BienvenidaCandidatos, this.usuario.matricula).subscribe();
    this.step = 4
  }
  regresar(): void {
    this.step -= 1;
  }
  cerraModal(): void {
    this.dialog.closeAll();
  }
  bienvenidaContinuar(): void {
    this.step = 2
  }
}

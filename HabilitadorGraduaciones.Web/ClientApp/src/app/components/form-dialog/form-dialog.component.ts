import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDetalleComponent } from '../modal-detalle/modal-detalle.component';
import { TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { GuardaDetalleSolicitud, GuardaSolicitud } from '../../classes/SolicitudCambioDatos';
import { SolicitudCopia } from '../../classes/SolicitudCopia';
import { EnumTipoDatosPersonales } from '../../enums/EnumTipoDatosPersonales';
import { SolicitudCambioDatosService } from '../../services/solicitud-cambio-datos.service';
import { NgxSpinnerService } from "ngx-spinner";
import { Observable } from 'rxjs';
import { RequisitosService } from '../../services/requisitos';
import { FiltroCampusService } from '../../services/FiltroCampus';
import { CampusCeremoniaGraduacionService } from '../../services/campusCeremoniaGraduacionService';
import { CampusCeremoniaGraduacion } from '../../classes/CampusCeremoniaGraduacion';
import { UtilsService } from '../../services/utils.service';
import { EnumArreglos } from '../../enums/EnumArreglos';
import { EnumTipoCorreo, ModalConfirmConfiguration } from 'src/app/classes';
import { noWhitespaceValidator } from 'src/app/helpers/customValidations';
import { NotificacionesService } from 'src/app/services/notificaciones.service';
import { EnumMovimientosEditarDatos } from 'src/app/enums/EnumMovimientosEditarDatos';
import { distinciones } from 'src/app/interfaces/EdicionDeDatos';


@Component({
  selector: 'app-form-dialog',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.css']
})

export class FormDialogComponent implements OnInit {
  form!: FormGroup;
  step = 1;

  nLsSolicitud: GuardaSolicitud[] = [];
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};

  //Archivos
  aNombre?: File;
  aCurp?: File;
  aProgramaAcademico?: File;
  aConcentraciones: distinciones[] = [];
  aDiplomasAcademicos: distinciones[] = [];

  //Booleanos
  isGuardandoDatos = false;
  isCargandoArrays = false;
  changeName = false;
  changeCurp = false;
  changeProgramaAcademico = false;
  loadANombre = false;
  loadaCurpm = false;
  loadaProgramaAcademico = false;
  isDiplomasCargados = false;

  //Observables
  datosCopia: SolicitudCopia;
  hasMasCampus = false;
  filtroCampus: any[];
  selectedCampus: string;
  miTec: string;

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<ModalDetalleComponent>, private tService: RequisitosService, private spinner: NgxSpinnerService, public ccgService: CampusCeremoniaGraduacionService,
    @Inject(MAT_DIALOG_DATA) public data: any, private solService: SolicitudCambioDatosService, private filtroCampusService: FiltroCampusService, private notificacionService: NotificacionesService,
    private utilsService: UtilsService, public dialog: MatDialog) {
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.required],
      aNombre: [''],
      curp: ['', Validators.required],
      aCurp: [''],
      programaAcademico: ['', Validators.required],
      aProgramaAcademico: [''],
      concentraciones: this.fb.array([]),
      diplomasAcademicos: this.fb.array([])
    }
    );

    this.isCargandoArrays = true
    this.spinner.show();

    const nombreCompleto = this.data.usuario.nombre + ' ' + this.data.usuario.apeidoPaterno + ' ' + this.data.usuario.apeidoMaterno;
    this.form.setValue({
      nombre: nombreCompleto,
      aNombre: '',
      curp: this.data.usuario.curp,
      aCurp: '',
      programaAcademico: this.data.usuario.programaAcademico,
      aProgramaAcademico: '',
      diplomasAcademicos: [],
      concentraciones: []
    });

    this.datosCopia = new SolicitudCopia(nombreCompleto, this.data.usuario.curp, this.data.usuario.programaAcademico, [], []);

    this.data.distinciones.lstConcentracion.forEach((c: any) => {
      let creaControl = this.creaControlForm(c, true);
      this.datosCopia.concentraciones.push(c)
      this.concentraciones.push(creaControl);
      this.aConcentraciones.push({ isLoadFile: false, file: new File([], '') })
    });

    if (this.data.distinciones.hasDiploma) {
      const creaControlDiploma = this.creaControlForm(this.data.distinciones.diploma, false);
      this.datosCopia.diplomasAcademicos.push(this.data.distinciones.diploma);
      this.diplomasAcademicos.push(creaControlDiploma);
    }
    if (this.data.distinciones.hasUllead) {
      const creaControlUDiploma = this.creaControlForm(this.data.distinciones.ulead, false);
      this.datosCopia.diplomasAcademicos.push(this.data.distinciones.diploma);
      this.diplomasAcademicos.push(creaControlUDiploma);
    }

    this.concentraciones.controls.forEach(d => {
      d.get('nombreArray')?.valueChanges.subscribe({
        next: () => {
          d.get('changeNombreArray')?.setValue(true);
        }
      });
    });

    this.diplomasAcademicos.controls.forEach(d => {
      d.get('nombreArray')?.valueChanges.subscribe({
        next: () => {
          d.get('changeNombreArray')?.setValue(true);
        }
      });
    });

    this.nombre?.valueChanges.subscribe({
      next: (d: any) => {
        this.changeName = true;
      }
    });

    this.curp?.valueChanges.subscribe({
      next: (d: any) => {
        this.changeCurp = true;
      }
    });

    this.programaAcademico?.valueChanges.subscribe({
      next: (d: any) => {
        this.changeProgramaAcademico = true;
      }
    });

    this.filtroCampusService.getFiltroCampus().subscribe(r => {
      this.filtroCampus = r
    })

    this.tService.getLinks().subscribe({
      next: (links: any) => {
        this.miTec = links.datosPersonales;
      },
      error: (error: HttpErrorResponse) => {
        console.error(error);
      }
    });
    this.validaDiplomas();
    this.isCargandoArrays = false;
    this.spinner.hide();
  }

  get nombre() { return this.form.get('nombre'); }
  get aNombreFile() { return this.form.get('aNombre'); }
  get curp() { return this.form.get('curp'); }
  get aCurpFile() { return this.form.get('aCurp'); }
  get programaAcademico() { return this.form.get('programaAcademico'); }
  get aProgramaAcademicoFile() { return this.form.get('aProgramaAcademico'); }
  get diplomasAcademicos() { return this.form.controls["diplomasAcademicos"] as FormArray; }
  get concentraciones() { return this.form.controls["concentraciones"] as FormArray; }

  creaControlForm(valor: any, isRequerido: boolean): FormGroup {
    if (isRequerido) {
      return this.fb.group({
        nombreArray: [valor],
        aNombreArray: [''],
        nombreFile: [''],
        size: [''],
        changeNombreArray: [false],
        file: []
      });
    }
    return this.fb.group({
      nombreArray: [valor, Validators.required],
      aNombreArray: [''],
      nombreFile: [''],
      size: [''],
      changeNombreArray: [false],
      file: []
    });
  }

  agregarDiploma(): void {
    let creaControlDiploma = this.creaControlForm('', false);
    this.diplomasAcademicos.push(creaControlDiploma);
    this.aDiplomasAcademicos.push({ isLoadFile: false, file: new File([], '') })
    this.diplomasAcademicos.controls.forEach(d => {
      d.get('nombreArray')?.valueChanges.subscribe({
        next: () => {
          d.get('changeNombreArray')?.setValue(true);
        }
      });
    })
    this.validaDiplomas();
  }

  cancelar(): void {
    this.data.ternminado = false;
    this.dialogRef.close(this.data);
  }

  onSubmitDatosPersonales(): void {
    this.nLsSolicitud = [];
    this.solicitudAgregaNombre();
    this.solicitudAgregaCurp();
    this.solicitudAgregaProgramaAcademico();
    this.solicitudAgregaConcentraciones();
    this.solicitudAgregaDiplomas();
    this.step = 2
  }

  deshabilitaDatosPersonales(): boolean {
    if (this.validaDatosPersonales()) {
      return true;
    } else {
      return false;
    }
  }

  validaDatosPersonales(): boolean {
    let lsMovimientos = [];
    if (this.changeName && this.nombre?.value !== this.datosCopia.nombre) {
      lsMovimientos.push({ type: EnumMovimientosEditarDatos.Name, fileLoad: false });
    }

    if (this.changeCurp && this.curp?.value !== this.datosCopia.curp) {
      lsMovimientos.push({ type: EnumMovimientosEditarDatos.Curp, fileLoad: false });
    }

    if (this.changeProgramaAcademico && this.programaAcademico?.value !== this.datosCopia.programaAcademico) {
      lsMovimientos.push({ type: EnumMovimientosEditarDatos.ProgramaAcademico, fileLoad: false });
    }

    lsMovimientos.forEach(e => {
      switch (e.type) {
        case EnumMovimientosEditarDatos.Name: {
          if (this.aNombre) {
            e.fileLoad = true;
          }
          break;
        }
        case EnumMovimientosEditarDatos.Curp: {
          if (this.aCurp) {
            e.fileLoad = true;
          }
          break;
        }
        case EnumMovimientosEditarDatos.ProgramaAcademico: {
          if (this.aProgramaAcademico) {
            e.fileLoad = true;
          }
          break;
        }
      }
    });

    this.aConcentraciones.forEach((d, i) => {
      if (this.concentraciones.at(i).dirty && this.datosCopia.concentraciones[i] !== this.concentraciones.at(i).value.nombreArray) {
        lsMovimientos.push({ type: EnumMovimientosEditarDatos.Concentracion + i, fileLoad: d.isLoadFile });
      }
    });

    this.aDiplomasAcademicos.forEach((d, i) => {
      if (this.diplomasAcademicos.at(i).dirty && this.datosCopia.diplomasAcademicos[i] !== this.diplomasAcademicos.at(i).value.nombreArray) {
        lsMovimientos.push({ type: EnumMovimientosEditarDatos.DiplomaAcademico + i, fileLoad: d.isLoadFile });
      }
    });

    if (lsMovimientos.length != 0) {
      return false;
    }
    return true
  }

  validaDiplomas(): void {
    let lsMovimientosDiplomas: any[] = [];

    this.aDiplomasAcademicos.forEach((d, i) => {
      if (this.diplomasAcademicos.at(i).dirty && this.datosCopia.diplomasAcademicos[i] !== this.diplomasAcademicos.at(i).value.nombreArray) {
        lsMovimientosDiplomas.push({ type: EnumMovimientosEditarDatos.DiplomaAcademico + i, fileLoad: d.isLoadFile, valid: true });
      } else {
        lsMovimientosDiplomas.push({ type: EnumMovimientosEditarDatos.DiplomaAcademico + i, fileLoad: d.isLoadFile, valid: false });
      }
    });

    if (this.aDiplomasAcademicos.length == 0) {
      this.isDiplomasCargados = true;
    } else {
      this.isDiplomasCargados = lsMovimientosDiplomas.every(v => v.valid);
    }
  }

  guardarSolicitudes(): void {
    this.isGuardandoDatos = true;
    this.spinner.show();

    this.solService.guardaSolicitudes(this.nLsSolicitud).subscribe({
      next: r => {
        if (r) {
          this.isGuardandoDatos = false;
        }
        this.isGuardandoDatos = false;
      },
      error: (e: HttpErrorResponse) => {
        this.isGuardandoDatos = false;
      }
    });
    this.isGuardandoDatos = false;
    this.spinner.hide();

    this.step = this.step + 1;
  }

  finalizar(): void {
    this.data.terminado = true;
    this.dialogRef.close(this.data);
  }

  regresoEditarDatosPersonales(): void {
    this.dialogRef.close(this.data);
  }

  aceptarSolicitudesGuardadas(): void {
    this.notificacionService.guardarNotificacionCorreo(EnumTipoCorreo.BienvenidaCandidatos, this.data.usuario.matricula).subscribe();
    this.data.terminado = true;
    this.dialogRef.close(this.data);
  }

  guardarCampusSeleccionado(): void {
    let campusSeleccionado = new CampusCeremoniaGraduacion(this.selectedCampus, this.data.usuario.matricula, this.data.usuario.periodoGraduacion);

    this.ccgService.guardaCampusSeleccionado(campusSeleccionado).subscribe({
      next: () => {
        this.step = 5;
      }, error: (err: HttpErrorResponse) => {
        console.error(err);
        this.step = 4;
      }
    });
  }

  formatBytes(bytes: any) {
    return this.utilsService.formatBytes(bytes);
  }

  fileBrowseHandler(files: any, i: number, arrayName?: string,): void {
    let id = files.srcElement.id;

    switch (id) {
      case "aNombre": {
        this.aNombre = files.target.files[0];
        break;
      }
      case "aCurp": {
        this.aCurp = files.target.files[0];
        break;
      }
      case "aProgramaAcademico": {
        this.aProgramaAcademico = files.target.files[0];
        break
      }
      case "aNombreArray": {
        if (arrayName === EnumArreglos.Concentraciones) {
          if (!this.aConcentraciones[i]) {
            this.aConcentraciones?.push({ isLoadFile: true, file: files.target.files[0] });
          } else {
            this.aConcentraciones[i].isLoadFile = true;
            this.aConcentraciones[i].file = files.target.files[0];
          }
          this.concentraciones.at(i).get('file')?.setValue(files.target.files[0]);
          this.concentraciones.at(i).get('size')?.setValue(this.formatBytes(files.target.files[0].size));
          this.concentraciones.at(i).get('nombreFile')?.setValue(files.target.files[0].name);
          this.form.updateValueAndValidity();
        } else {
          if (!this.aDiplomasAcademicos[i]) {
            this.aDiplomasAcademicos?.push({ isLoadFile: true, file: files.target.files[0] });
          } else {
            this.aDiplomasAcademicos[i].isLoadFile = true;
            this.aDiplomasAcademicos[i].file = files.target.files[0];
          }
          this.diplomasAcademicos.at(i).get('file')?.setValue(files.target.files[0]);
          this.diplomasAcademicos.at(i).get('size')?.setValue(this.formatBytes(files.target.files[0].size));
          this.diplomasAcademicos.at(i).get('nombreFile')?.setValue(files.target.files[0].name);
          this.form.updateValueAndValidity();
        }
        this.validaDiplomas();
        break;
      }
    }
  }

  deleteFile($event: any, i: number, arrayName?: string): void {
    const id = $event.srcElement.id;
    switch (id) {
      case "aNombre": {
        delete this.aNombre;
        this.form.get('aNombre')?.reset();
        break;
      }
      case "aCurp": {
        delete this.aCurp;
        this.form.get('aCurp')?.reset();
        break;
      }
      case "aProgramaAcademico": {
        delete this.aProgramaAcademico;
        this.form.get('aProgramaAcademico')?.reset();
        break
      }
      case "aNombreArray": {
        if (arrayName === EnumArreglos.Concentraciones) {
          this.concentraciones.at(i).get('aNombreArray')?.reset();
          this.concentraciones.at(i).get('changeNombreArray')?.setValue(false);
          this.concentraciones.at(i).get('size')?.reset();
          this.concentraciones.at(i).get('nombreFile')?.reset();
          this.concentraciones.at(i).get('file')?.reset();
          this.aConcentraciones[i].isLoadFile = false;

        } else {
          this.diplomasAcademicos.at(i).get('aNombreArray')?.reset();
          this.diplomasAcademicos.at(i).get('changeNombreArray')?.setValue(false);
          this.diplomasAcademicos.at(i).get('size')?.reset();
          this.diplomasAcademicos.at(i).get('nombreFile')?.reset();
          this.diplomasAcademicos.at(i).get('file')?.reset();
          this.aDiplomasAcademicos[i].isLoadFile = false;
        }
        break;
      }
    }
  }

  solicitudAgregaNombre(): void {
    if (this.changeName && this.nombre?.value !== this.datosCopia.nombre) {
      let nuevaSolicitud = new GuardaSolicitud();

      nuevaSolicitud.Matricula = this.data.usuario.matricula;
      nuevaSolicitud.PeriodoGraduacion = this.data.usuario.periodoGraduacion;
      nuevaSolicitud.IdDatosPersonales = EnumTipoDatosPersonales.Nombre;
      nuevaSolicitud.DatoIncorrecto = this.datosCopia.nombre;
      nuevaSolicitud.DatoCorrecto = this.nombre?.value;

      if (this.aNombre) {
        nuevaSolicitud.Detalle = [];
        let detalleSolicitud = new GuardaDetalleSolicitud();

        this.imagetoBase64(this.aNombre).subscribe({
          next: (r) => { detalleSolicitud.Archivo = r; }
        });
        detalleSolicitud.Documento = this.aNombre.name;
        detalleSolicitud.Extension = this.aNombre.name.slice(this.aNombre.name.indexOf('.') + 1);
        nuevaSolicitud.Detalle.push(detalleSolicitud);
      }
      this.nLsSolicitud.push(nuevaSolicitud);
    };
  }

  solicitudAgregaCurp(): void {
    if (this.changeCurp && this.curp?.value !== this.datosCopia.curp) {
      let nuevaSolicitud = new GuardaSolicitud();
      nuevaSolicitud.Matricula = this.data.usuario.matricula;
      nuevaSolicitud.PeriodoGraduacion = this.data.usuario.periodoGraduacion;
      nuevaSolicitud.IdDatosPersonales = EnumTipoDatosPersonales.Curp;
      nuevaSolicitud.DatoIncorrecto = this.datosCopia.curp;
      nuevaSolicitud.DatoCorrecto = this.curp?.value;

      if (this.aCurp) {
        nuevaSolicitud.Detalle = [];
        let detalleSolicitud = new GuardaDetalleSolicitud();

        this.imagetoBase64(this.aCurp).subscribe({
          next: (r) => { detalleSolicitud.Archivo = r; }
        });
        detalleSolicitud.Documento = this.aCurp.name;
        detalleSolicitud.Extension = this.aCurp.name.slice(this.aCurp.name.indexOf('.') + 1);
        nuevaSolicitud.Detalle.push(detalleSolicitud);
      }
      this.nLsSolicitud.push(nuevaSolicitud);
    }
  }

  solicitudAgregaProgramaAcademico(): void {
    if (this.changeProgramaAcademico && this.programaAcademico?.value !== this.datosCopia.programaAcademico) {
      let nuevaSolicitud = new GuardaSolicitud();
      nuevaSolicitud.Matricula = this.data.usuario.matricula;
      nuevaSolicitud.PeriodoGraduacion = this.data.usuario.periodoGraduacion;
      nuevaSolicitud.IdDatosPersonales = EnumTipoDatosPersonales.ProgramaAcademico;
      nuevaSolicitud.DatoIncorrecto = this.datosCopia.programaAcademico;
      nuevaSolicitud.DatoCorrecto = this.programaAcademico?.value;

      if (this.aProgramaAcademico) {
        nuevaSolicitud.Detalle = [];

        let detalleSolicitud = new GuardaDetalleSolicitud();

        this.imagetoBase64(this.aProgramaAcademico).subscribe({
          next: (r) => { detalleSolicitud.Archivo = r; }
        });
        detalleSolicitud.Documento = this.aProgramaAcademico.name;
        detalleSolicitud.Extension = this.aProgramaAcademico.name.slice(this.aProgramaAcademico.name.indexOf('.') + 1);
        nuevaSolicitud.Detalle.push(detalleSolicitud);
      }
      this.nLsSolicitud.push(nuevaSolicitud);
    }
  }

  solicitudAgregaDiplomas(): void {
    this.diplomasAcademicos.controls.forEach((d: any, i: number) => {
      if (d.dirty && d.value.nombreArray !== this.datosCopia.diplomasAcademicos[i]) {
        let nuevaSolicitud = new GuardaSolicitud();
        nuevaSolicitud.Matricula = this.data.usuario.matricula;
        nuevaSolicitud.PeriodoGraduacion = this.data.usuario.periodoGraduacion;
        nuevaSolicitud.IdDatosPersonales = EnumTipoDatosPersonales.DiplomaAcademico;
        nuevaSolicitud.DatoIncorrecto = this.datosCopia.diplomasAcademicos.length > 0 ? this.datosCopia.diplomasAcademicos[i] : 'No existe';
        nuevaSolicitud.DatoCorrecto = d.get('nombreArray')?.value;

        if (this.aDiplomasAcademicos[i] && this.aDiplomasAcademicos[i].isLoadFile) {
          nuevaSolicitud.Detalle = [];
          let detalleSolicitud = new GuardaDetalleSolicitud();

          this.imagetoBase64(this.aDiplomasAcademicos[i].file).subscribe({
            next: (r) => { detalleSolicitud.Archivo = r; }
          });

          detalleSolicitud.Documento = this.aDiplomasAcademicos[i].file.name;
          detalleSolicitud.Extension = this.aDiplomasAcademicos[i].file.name.slice(this.aDiplomasAcademicos[i].file.name.indexOf('.') + 1);
          nuevaSolicitud.Detalle.push(detalleSolicitud);
        }
        this.nLsSolicitud.push(nuevaSolicitud);
      }
    });
  }

  solicitudAgregaConcentraciones(): void {
    this.concentraciones.controls.forEach((d: any, i: number) => {
      if (d.dirty && d.value.nombreArray !== this.datosCopia.concentraciones[i]) {
        let nuevaSolicitud = new GuardaSolicitud();
        nuevaSolicitud.Matricula = this.data.usuario.matricula;
        nuevaSolicitud.PeriodoGraduacion = this.data.usuario.periodoGraduacion;
        nuevaSolicitud.IdDatosPersonales = EnumTipoDatosPersonales.Concentracion;
        nuevaSolicitud.DatoIncorrecto = this.datosCopia.concentraciones.length > 0 ? this.datosCopia.concentraciones[i] : 'No existe';
        nuevaSolicitud.DatoCorrecto = d.get('nombreArray')?.value;

        if (this.aConcentraciones[i] && this.aConcentraciones[i].isLoadFile) {
          nuevaSolicitud.Detalle = [];
          let detalleSolicitud = new GuardaDetalleSolicitud();

          this.imagetoBase64(this.aConcentraciones[i].file).subscribe({
            next: (r) => { detalleSolicitud.Archivo = r; }
          });

          detalleSolicitud.Documento = this.aConcentraciones[i].file.name;
          detalleSolicitud.Extension = this.aConcentraciones[i].file.name.slice(this.aConcentraciones[i].file.name.indexOf('.') + 1);
          nuevaSolicitud.Detalle.push(detalleSolicitud);
        }
        this.nLsSolicitud.push(nuevaSolicitud);
      }
    });
  }

  imagetoBase64(file: File): Observable<string> {
    return new Observable(obs => {
      let reader = new FileReader();

      reader.onload = (e: any) => {
        let subString = e.target.result.substring(e.target.result.indexOf(',') + 1);
        obs.next(subString);
        obs.complete();
      };
      reader.readAsDataURL(file);
    });

  }
  regresar(): void {
    this.step -= 1;
    this.aNombreFile?.reset();
    this.aCurpFile?.reset();
    this.aProgramaAcademicoFile?.reset();
    this.concentraciones.controls.forEach(control => {
      control.get('aNombreArray')?.reset();
    });
    this.diplomasAcademicos.controls.forEach(control => {
      control.get('aNombreArray')?.reset();
    });
    this.form.updateValueAndValidity()
  }

  cambiaARequerido(tipoDistincion: string, index: number): void {
    if (tipoDistincion === 'diplomasArray') {
      const controlDiplomas = this.diplomasAcademicos.controls[index] as FormGroup;
      controlDiplomas.controls['nombreFile'].setValidators(noWhitespaceValidator());
      controlDiplomas.controls['nombreFile'].updateValueAndValidity();
    }

    if (tipoDistincion === 'concentracionesArray') {
      const controlDiplomas = this.concentraciones.controls[index] as FormGroup;
      controlDiplomas.controls['nombreFile'].setValidators(noWhitespaceValidator());
      controlDiplomas.controls['nombreFile'].updateValueAndValidity();
    }
  }
}

import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { Periodo } from 'src/app/interfaces/Periodos';
import { Usuario } from 'src/app/interfaces/Usuario';
import { PeriodosService } from 'src/app/services/periodos.service';
import { RegistroExitosoGraduacionComponent } from '../registro-exitoso-graduacion/registro-exitoso-graduacion.component';
import { EnumOrigenCambioPeriodo } from 'src/app/enums/EnumOrigenCambioPeriodo';
import { ModalConfirmConfiguration } from 'src/app/classes';
import { TranslateService } from '@ngx-translate/core';
import { UtilsService } from 'src/app/services/utils.service';
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';

@Component({
  selector: 'app-modal-ceremonia-graduacion',
  templateUrl: './modal-ceremonia-graduacion.component.html',
  styleUrls: ['./modal-ceremonia-graduacion.component.css']
})
export class ModalCeremoniaGraduacionComponent implements OnInit {

  public usuario: Usuario = <Usuario>{};
  public listPeriodos: Periodo[] = [];
  periodoGuardar: Periodo = <Periodo>{};
  motivoCambioPeriodo: string = '';
  mostrarMotivo: boolean = false;
  motivo: string = '';
  periodoEstimado: string = '';
  validacionMotivo: boolean = true;
  caracteres: number = 0;
  periodoCeremonia: Periodo;
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};

  constructor(private periodoService: PeriodosService, public dialog: MatDialog, private translate: TranslateService, public dialogRef: MatDialogRef<ModificarFechaGraduacionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private utilsService: UtilsService) { }

  ngOnInit(): void {
    this.usuario = this.data.usuario;
    this.listPeriodos = this.data.periodos;
    this.motivoCambioPeriodo = this.data.motivoCambioPeriodo;
    this.periodoEstimado = this.data.periodoEstimado;
    this.periodoCeremonia =this.obtenerSiguientePeriodo(this.data.periodoElegidoId, this.listPeriodos);
  }
  form = new FormGroup({
    aceptar: new FormControl('', Validators.required),
    motivo: new FormControl()
  });
   
  get f(){
    return this.form.controls;
  }
   
  submit(): void{
    let respuesta = this.form.value.aceptar;
    let motivoNoCeremonia =  this.form.value.motivo as string;

    this.periodoGuardar = <Periodo>{};
    this.periodoGuardar.periodoElegido = this.data.periodoElegidoId;
    this.periodoGuardar.periodoEstimado = this.periodoEstimado;
    this.periodoGuardar.matricula = this.usuario.matricula;
    this.periodoGuardar.motivoCambioPeriodo = this.motivoCambioPeriodo;
    this.periodoGuardar.origenActualizacionPeriodoId = EnumOrigenCambioPeriodo.ModificacionAlumno;
    if(respuesta == '1'){
      let nuevaPerido = this.obtenerSiguientePeriodo(this.data.periodoElegidoId, this.listPeriodos);
      this.periodoGuardar.periodoCeremonia = nuevaPerido?.periodoId!;
      this.periodoGuardar.eleccionAsistenciaCeremonia = 'Si'; 
      this.periodoService.guardarPeriodoAlumno(this.periodoGuardar).subscribe((result: any ) =>{
        this.registroExitoso(nuevaPerido?.descripcion as string, 'Si', '');
      });
    }
    else if(respuesta == '0'){
      if(motivoNoCeremonia == null || motivoNoCeremonia == ''){
        this.validacionMotivo = false;
        return;
      }else{
        this.validacionMotivo = true;
        let nuevaPerido = this.obtenerSiguientePeriodo(this.data.periodoElegidoId, this.listPeriodos);
        this.periodoGuardar.periodoCeremonia = this.data.periodoElegidoId;;
        this.periodoGuardar.eleccionAsistenciaCeremonia = 'No'; 
        this.periodoGuardar.motivoNoAsistirCeremonia = motivoNoCeremonia;
        this.periodoService.guardarPeriodoAlumno(this.periodoGuardar).subscribe((result: any ) =>{
          this.registroExitoso(nuevaPerido?.descripcion as string, 'No', motivoNoCeremonia);
        });
      }
      
    }
    else{
      let nuevaPerido = this.obtenerSiguientePeriodo(this.data.periodoElegidoId, this.listPeriodos);
      this.periodoService.guardarPeriodoAlumno(this.periodoGuardar).subscribe((result: any ) =>{
        this.registroExitoso('', 'No se', '');
      });
    }
  }

  registroExitoso(periodo: string, resp: string, _motivoNoCeremonia: string){
    this.dialogRef.close();
    const dialogRef = this.dialog.open(RegistroExitosoGraduacionComponent, {
      width: '570px',
      height : 'auto',
      maxHeight : '100vh',
      disableClose: true,
      data: {periodo: this.data.periodoElegido, periodoCeremonia: periodo, respuesta: resp, usuario: this.usuario, motivoNoCeremonia: _motivoNoCeremonia, motivo: this.motivoCambioPeriodo, intensivo: true }
    });

  }

  obtenerSiguientePeriodo(periodo: string, lista: Periodo[]): Periodo{
    let periodoInt = Number(periodo);
    let periodoSiguienteInt = periodoInt + 1;
    let periodosig = lista.find(x => x.periodoId == String(periodoSiguienteInt)) as Periodo;
    return periodosig;
  }

  onItemChange(item: any): void{
    this.mostrarMotivo = item.value == 1 ? false : true
  }

  cerrarModal(): void{
  const dialogConfig = new MatDialogConfig();
  dialogConfig.autoFocus = true;
  dialogConfig.width = '570px';
  dialogConfig.height = 'auto';

  let tituloModificarFecha = "";
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
    periodoGuardado: false,
    paramsEndpoints : this.data.paramsEndpoints 
  };
  this.dialog.closeAll();
  const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent, dialogConfig);
  dialogRef.afterClosed().subscribe(result => {
    
  });


 }
 onKeyPress(event:any): void{
  this.validacionMotivo = true;
  if((event.keyCode >= 47 && event.keyCode <= 90) || (event.keyCode >= 96 && event.keyCode <= 109) || (event.keyCode >= 187 )|| event.keyCode == 32){
    this.caracteres ++;
  }
  else{
    this.caracteres = this.motivo.length
  }
 }
 onKeyUp(event:any): void{
    this.caracteres = this.motivo.length;
 }

 onFocusOut(): void{
  this.caracteres = this.motivo.length;
 }
}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CardAcordeon } from 'src/app/interfaces/CardAcordeon'
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormDialogComponent } from '../form-dialog/form-dialog.component';
import { ModalConfirmConfiguration } from 'src/app/interfaces/ModalConfirmConfiguration';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { Card } from 'src/app/interfaces/Card';
import { Router } from '@angular/router';
import { PeriodosService } from 'src/app/services/periodos.service';
import { environment } from 'src/environments/environment';
import {DatePipe} from '@angular/common';
import { LangChangeEvent, TranslateModule, TranslateService } from '@ngx-translate/core';
import { EnumTipoCardAcordeon } from 'src/app/enums/EnumTipoCardAcordeon';
import { Distinciones, Usuario } from 'src/app/classes';
import { MatTooltip } from '@angular/material/tooltip'
import { ModificarFechaGraduacionComponent } from '../modificar-fecha-graduacion/modificar-fecha-graduacion.component';

@Component({
  selector: 'app-home-movil',
  templateUrl: './home-movil.component.html',
  styleUrls: ['./home-movil.component.css']
})
export class HomeMovilComponent implements OnInit {

  @Input() usuario: any;
  @Input() ListaCard: any []=[];
  @Input() laptop: boolean = false;
  @Input() avisos: any;
  @Input() periodoActual: any;
  @Input() barra_Progreso: any;
  @Input() barra_ProgresoTotal: any;
  @Input() distincion: any;
  @Input() linkCalendario: any;
  @Input() linkVacio: boolean = false;
  @Input() normalSemanas: boolean = true;
  @Input() periodoConfirmado = false;
  @Input() bandProspecto= false;
  @Input() bandCandidato = false;
  @Input() bandAlumnoNormal = true;
  @Input() tienePrestamo:boolean;
  @Input() urlPrestamo:string;
  @Input() urlTesoreria:string;
  @Input() urlDistinciones:string;


  @Output("openCardDialog") openCardDialog: EventEmitter<any> = new EventEmitter();

  
  public configuracionModal: ModalConfirmConfiguration = <ModalConfirmConfiguration>{};
  mensaje: any; 
  form!: FormGroup;
  dateTramitesAdmon: Date = new Date()
  
  constructor(private fb: FormBuilder, public dialog: MatDialog, private router: Router, private periodosService: PeriodosService, private datePipe: DatePipe, private translate: TranslateService){}

  ngOnInit(): void {
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
  }

  openForm(formulario: string): void {
  
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width='600px';
    dialogConfig.maxWidth='92vw'

    if(formulario == 'Usuario'){
      dialogConfig.data = {
        form: this.form,
        formType: 'Usuario',
        titulo: 'Servicio de actualización de datos',
        mensaje: ''
    };
    }
    
    const dialogRef = this.dialog.open(FormDialogComponent,dialogConfig );
    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
    });
  }

  abrirModal(origen: string, _usuario: Usuario): void {
  
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width='600px';

    if(origen == 'MiInformacion'){
      this.configuracionModal.titulo = "Datos Personales";
      this.configuracionModal.isSelectCampus = true;
      this.configuracionModal.isCambioFecha = false;
      dialogConfig.data = {
        configuracion: this.configuracionModal,
        objeto: this.usuario,
      };
      
    const dialogRef = this.dialog.open(ConfirmDialogComponent,dialogConfig );
    dialogRef.afterClosed().subscribe(result => {
      this.mensaje = result;
      this.periodosService.getPeriodoAlumno(_usuario.matricula).subscribe((resuPA: any) => {
        this.periodoActual = resuPA.descripcion;
      });
    });
    }
    else if(origen == 'ModificarFecha'){
      this.configuracionModal.titulo = "Selecciona el Periodo en que deseas graduarte";
      this.configuracionModal.isSelectCampus = false;
      this.configuracionModal.isCambioFecha = true;
      dialogConfig.data = {
        configuracion: this.configuracionModal,
        objeto: _usuario,
        periodoActual : ''
      };
      const dialogRef = this.dialog.open(ModificarFechaGraduacionComponent,dialogConfig );
      dialogRef.afterClosed().subscribe(result => {
        if(result != null && result != undefined){
          this.periodoActual = result;
        } else{
          this.periodosService.getPeriodoAlumno(_usuario.matricula).subscribe((resuPA: any) => {
            this.periodoActual = resuPA.descripcion;
          });
        }
      });
      
    }
    
  }

  openCardDialogTrigger(card: Card): void {
    this.openCardDialog.emit(card);
  }
  get usuarios() {
    return this.form.controls["usuarios"] as FormArray;
  }

  verAvisos(matricula:string): void{
    this.router.navigate(['/veravisos/'],{
      queryParams:{
        matricula:matricula
      }
    });
  }

  redirectTesoreria(): void{
    window.open(this.urlTesoreria, "_blank");
  }

  redirectPrestamo(): void{
    window.open(this.urlPrestamo, "_blank");
  }

  redirectCalendario(): void {
    if(this.linkCalendario == "")
      return;
    window.open(this.linkCalendario, "_blank");
  }

  hideTooltipIn(tooltip : MatTooltip, ms : number): void{
    tooltip.show();
    setTimeout(() => tooltip.hide(), ms);
  }
  
}

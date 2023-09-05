import { Component, HostListener, Inject, OnInit, AfterViewInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AvisosService } from 'src/app/services/avisos.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { DomSanitizer } from '@angular/platform-browser';

import { SnackBarService } from 'src/app/services/snackBar.service';
import { FitrosMatricula } from 'src/app/interfaces/Aviso';
import { MatDialog } from '@angular/material/dialog';
import { AvisosAdminConfirmComponent } from '../avisos-admin-confirm/avisos-admin-confirm.component';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject, take, takeUntil } from 'rxjs';
import { MatriculasClass } from '../../../classes/MatriculasClass';
import { CampusNomina, NivelesNomina, PermisosMenu, Sedes } from 'src/app/interfaces/PermisosNomina';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';

@Component({
  selector: 'app-avisos-admin',
  templateUrl: './avisos-admin.component.html',
  styleUrls: ['./avisos-admin.component.css']
})

export class AvisosAdminComponent implements OnInit, AfterViewInit, OnDestroy{
  lstFiltroEscrito: MatriculasClass[];
  mensajeDrag:string = 'Arrastra y suelta una imagen';
  subTxt:string = 'Acepta imágenes .jpg y .png de máximo 2MB.';
  msgError:boolean = false;
  docCargado: boolean = false;
  confirmed: boolean = false;
  primerEnvio: boolean = true; 
  filtros:any;
  form!: FormGroup;
  requisitoBandera:boolean = false;
  filtroMatricula:[];
  
  
  lstFiltroMatricula: MatriculasClass[]=[];
  banks= this.lstFiltroMatricula;
  bankMultiCtrl: FormControl<string[] | null> = new FormControl<string[]>([]);
  bankMultiFilterCtrl: FormControl<string | null> = new FormControl<string>('');
  filteredBanksMulti: ReplaySubject<MatriculasClass[]> = new ReplaySubject<MatriculasClass[]>(1);
  @ViewChild('multiSelect', { static: true }) multiSelect: MatSelect;
  _onDestroy = new Subject<void>();

  permisosMenu: PermisosMenu[] = [];
  permisos: PermisosMenu;
  permisoVer :boolean;
  permisoEditar: boolean = false;
  permisosCampus: CampusNomina[] = [];
  campus: string[] = [];
  permisosSedes: Sedes[] = [];
  permisosNiveles: NivelesNomina[] = [];
  IdUsuario: number = 0;

  editorConfig:AngularEditorConfig= {
    editable: true,
    spellcheck: false,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Escribe un mensaje',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    toolbarHiddenButtons: [
      [
        'undo',
        'redo',
        'strikeThrough',
        'subscript',
        'superscript',
        'justifyLeft',
        'justifyCenter',
        'justifyRight',
        'justifyFull',
        'indent',
        'outdent',
        'heading',
        'fontName',
      ],
      [
        'fontSize',
        'textColor',
        'backgroundColor',
        'customClasses',
        'link',
        'unlink',
        'insertImage',
        'insertVideo',
        'insertHorizontalRule',
        'removeFormat',
        'toggleEditorMode',
      ],
    ],
  };

  constructor(private dialog: MatDialog,private fb:FormBuilder, private avisosService:AvisosService, @Inject(DomSanitizer) private readonly sanitizer:DomSanitizer, 
    private snackBarService: SnackBarService, private pnService: PermisosNominaService, private cdr: ChangeDetectorRef,) { }

  onFileChange(event: any) :void {
    if(!this.permisoEditar){
      return;
    }
    const files: FileList = event.target.files;
    this.saveFiles(files);
  }

  ngOnInit() {
    this.permisosMenu = this.pnService.obtenMenu();
    this.permisos = this.permisosMenu.find(permiso => permiso.nombreMenu == 'Envío de avisos')!;
    this.permisosCampus = this.pnService.obtenCampus();
    this.permisosNiveles = this.pnService.obtenNiveles();  
    this.permisoVer = this.permisos.ver;
    this.permisoEditar = this.permisos.editar;  
    this.IdUsuario = this.pnService.obtenIdUsuario();

    this.filteredBanksMulti.next(this.banks.slice());
    this.bankMultiFilterCtrl?.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterBanksMulti();
      });


    this.avisosService.getFiltros().subscribe(res =>{
      this.filtros = res;
      this.filtros.nivel = this.permisosNiveles;
      this.filtros.campus = this.permisosCampus;
      this.filtros.sedes = [];
      
    });

    this.form = this.fb.group({
      titulo: new FormControl('',[Validators.required]),
      texto: new FormControl('',[Validators.required]),
      documento: new FormControl(null,[Validators.nullValidator]),
      nivel: new FormControl('',Validators.nullValidator),
      campusId: new FormControl('',Validators.nullValidator),
      sedeId: new FormControl('',Validators.nullValidator),
      escuelasId: new FormControl('',Validators.nullValidator),
      programaId: new FormControl('',Validators.nullValidator),
      requisitoId: new FormControl('',Validators.nullValidator),
      requisitoEstatus: new FormControl('', Validators.nullValidator),
      matricula: new FormControl('',Validators.nullValidator),
      cc_rolesId: new FormControl('',Validators.nullValidator),
      cc_camposId: new FormControl('',Validators.nullValidator),
      habilitador: new FormControl(false,Validators.required),
      correo: new FormControl(false,Validators.required),
      urlImage: new FormControl(null),
      idUsuario: new FormControl(0)
    });
    if(!this.permisoEditar){
      this.form.controls.titulo.disable();
      this.form.controls.documento.disable();
      this.form.controls.nivel.disable();
      this.form.controls.campusId.disable();
      this.form.controls.sedeId.disable();
      this.form.controls.escuelasId.disable();
      this.form.controls.programaId.disable();
      this.form.controls.matricula.disable();
      this.form.controls.cc_camposId.disable();
      this.form.controls.habilitador.disable();
      this.form.controls.correo.disable();
      this.form.controls.urlImage.disable();
      this.editorConfig.editable = this.permisoEditar;
      this.cdr.detectChanges();
    }
    this.form.controls.requisitoId.disable();
    this.form.controls.cc_rolesId.disable();
    this.onChangeFilter();
  }  

  ngAfterViewInit() {
    this.setInitialValue();
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  setInitialValue(): void {
    this.filteredBanksMulti
      .pipe(take(1), takeUntil(this._onDestroy))
      .subscribe(() => {
        this.multiSelect.compareWith = (a: MatriculasClass, b: MatriculasClass) => a && b && a.clave === b.clave;
      });
  }

  filterBanksMulti() : void {
    if (!this.banks) {
      return;
    }
    let search: string = this.bankMultiFilterCtrl.value as string;
    if (!search) {
      this.filteredBanksMulti.next(this.banks.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    this.lstFiltroEscrito = this.lstFiltroMatricula;
    this.filteredBanksMulti.next(
      this.lstFiltroEscrito = this.lstFiltroMatricula.filter(bank => bank.descripcion.toLowerCase().indexOf(search) > -1)
    );
  }



  @HostListener("drop", ["$event"]) onDrop(event: any) {
    if(!this.permisoEditar){
      return;
    }
    event.preventDefault();
    event.stopPropagation();
    if (event.dataTransfer.files) {
      let files: FileList = event.dataTransfer.files;
      this.saveFiles(files);
    }
  }

  saveFiles(file: FileList) :void {

    if (file.length > 1) this.snackBarService.openSnackBar("No se pueden cargar más de dos imágenes", "default");
    else {
      if(file[0].type != "image/png" && file[0].type != "image/jpeg" && file[0].type != "image/gif"){  
        this.subTxt = "El archivo ingresado no es un archivo válido";
        this.mensajeDrag = 'Arrastra y suelta una imagen';
        this.docCargado = false;
        this.msgError = true;
        return;
      }
      if (file[0].size > 2000000) {
        const tamanioEnMb = 2000000 / 1000000;
        this.mensajeDrag = 'Arrastra y suelta una imagen';
        this.subTxt = `El tamaño máximo es de ${tamanioEnMb} MB`;
        this.docCargado = false;
        this.msgError = true;
        return;
    }
      this.mensajeDrag = file[0].name;
      this.form.get('documento')?.setValue(file[0]);
      this.docCargado = false;
      this.msgError = false;
    }
  }

  toggleCheck(input:string) :void {
    switch (input) {
      case 'correo':
        this.form.get('correo')?.setValue(!this.form.get('correo')?.value);
        this.form.get('correo')?.updateValueAndValidity();
        break;
      case 'habilitador':
        this.form.get('habilitador')?.setValue(!this.form.get('habilitador')?.value);
          this.form.updateValueAndValidity();
          break;
      default:
        break;
    }
  }

  enviarAviso() :void {
    if (this.primerEnvio) {
      this.confirmed = true;
      this.primerEnvio = false;
    } else {
      this.primerEnvio = true;
      this.confirmed = false;

      this.form.get('idUsuario')?.setValue(this.IdUsuario);
      this.sanitizer.bypassSecurityTrustHtml(this.form.value.mensaje);
      if (this.form.value.documento != null) {
        this.avisosService.subirImagen(this.form.value.documento).then((res: any) => {
          if (res.result) {
            this.form.get('urlImage')?.setValue(res.res);
            this.avisosService.guardarAviso(JSON.stringify(this.form.getRawValue())).subscribe(res => {
              this.snackBarService.openSnackBar("Aviso guardado correctamente.", "default");
              this.form.reset();
              this.mensajeDrag = 'Arrastra y suelta una imagen';
              this.subTxt = 'Acepta una imagen';
              this.docCargado = false;
              this.msgError = false;
              this.resetFilters();
            });
          }
        }).catch(err => {
          console.error(err);
          this.snackBarService.openSnackBar("Ocurrió un error al guardar el Aviso.", "default");
        });
      } else {
        this.avisosService.guardarAviso(JSON.stringify(this.form.getRawValue())).subscribe((res: any) => {
          if (res.result) {
            this.snackBarService.openSnackBar("Aviso guardado correctamente.", "default");
            this.form.reset();
            this.resetFilters();
          } else {
            this.snackBarService.openSnackBar("Ocurrió un error al guardar el Aviso.", "default");
          }
        },
        error => {
          this.snackBarService.openSnackBar("Ocurrió un error al guardar el Aviso.", "default");
        });
      }
    }

  }
  confirmar() :void {    
    this.showPopup();
  }

  showPopup() :void {
    const dialogRef = this.dialog.open(AvisosAdminConfirmComponent, {
      data: {
        json: JSON.stringify(this.form.getRawValue()),
        filtros: this.filtros,
        numMatriculas:this.lstFiltroMatricula.length
      },
    });
    

    dialogRef.afterClosed().subscribe(result => {
      if (result === "Accepted") {
        this.confirmed = true;
        this.primerEnvio = false;
        this.enviarAviso();
      } else {
        this.confirmed = false;
        this.primerEnvio = true;
      }
    });

  }

  onRequisitoChange(value:string) :void {
    if(value != null && value != "")
      this.requisitoBandera = true;
    else
      this.requisitoBandera = false;
  }

  onChangeFilter() :void {
    this.form.get('matricula')?.setValue('');
    let filtrosMatricula = {} as FitrosMatricula;
    filtrosMatricula.nivelId = this.form.value.nivel;
    filtrosMatricula.campusId = this.form.value.campusId;
    filtrosMatricula.sedeId = this.form.value.sedeId;
    filtrosMatricula.escuelasId = this.form.value.escuelasId;
    filtrosMatricula.programaId = this.form.value.programaId;
    filtrosMatricula.idUsuario = this.IdUsuario;
    let filtros = JSON.stringify(filtrosMatricula);
    this.avisosService.getFiltroMatricula(filtros).subscribe((res:any) =>{
      if (res) {
        this.filtroMatricula = res;
        this.lstFiltroMatricula = res;

        this.lstFiltroMatricula = this.filtroMatricula.map((banco: any) => {
          return {
            clave: banco.clave,
            descripcion: banco.descripcion,
            errorMessage: banco.errorMessage,
            result: banco.result
          };
        });
        this.lstFiltroEscrito = this.lstFiltroMatricula;
      }
    });
  }

  onChangeCampus() :void{
    this.permisosSedes = this.permisosCampus.find(campus => campus.claveCampus == this.form.value.campusId)?.sedes!;
    this.filtros.sedes = this.permisosSedes;
  }

  resetFilters() :void{
    this.mensajeDrag = 'Arrastra y suelta una imagen';
    this.subTxt = 'Acepta imágenes .jpg y .png de máximo 2MB.';
    this.docCargado = false;
    this.msgError = false;
    this.form.get('titulo')?.setValue('');
    this.form.get('texto')?.setValue('');
    this.form.get('documento')?.setValue(null);
    this.form.get('nivel')?.setValue('');
    this.form.get('campusId')?.setValue('');
    this.form.get('sedeId')?.setValue('');
    this.form.get('escuelasId')?.setValue('');
    this.form.get('programaId')?.setValue('');
    this.form.get('requisitoId')?.setValue('');
    this.form.get('requisitoEstatus')?.setValue('');
    this.form.get('matricula')?.setValue('');
    this.form.get('cc_rolesId')?.setValue('');
    this.form.get('cc_camposId')?.setValue('');
    this.form.get('habilitador')?.setValue(false);
    this.form.get('correo')?.setValue(false);
    this.form.get('urlImage')?.setValue(null);
    this.form.get('idUsuario')?.setValue(0);
  }

}


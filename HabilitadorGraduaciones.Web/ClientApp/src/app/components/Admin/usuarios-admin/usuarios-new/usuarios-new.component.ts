import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSelect } from '@angular/material/select';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Roles } from 'src/app/classes/Roles';
import { Campus, Nivel, Sede, UsuarioAdmin } from 'src/app/interfaces/UsuarioAdmin';
import { RolesService } from 'src/app/services/roles.service';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { UsuarioAdminService } from 'src/app/services/usuario-admin.service';
import { PaginatorService } from '../../../../services/paginator.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';

@Component({
  selector: 'app-usuarios-new',
  templateUrl: './usuarios-new.component.html',
  styleUrls: ['./usuarios-new.component.css']
})
export class UsuariosNewComponent implements OnInit {

  campusList: Campus[] = [];
  campusFilter: Campus[] = [];
  sedesList: Sede[] = [];
  sedesFiltradas: Sede[] = [];
  @ViewChild('multiSelect') multiSelect: MatSelect;
  @ViewChild('multiSelectRoles') multiSelectRoles: MatSelect;
  @ViewChild('multiSelectSedes') multiSelectSedes: MatSelect;
  @ViewChild('multiSelectNiveles') multiSelectNiveles: MatSelect;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  todosCampus = false;
  todosSedes = false;
  todosRoles = false;
  todosNiveles = false;
  form!: FormGroup;
  dataRoles: Roles[];
  selectedValues: Campus[];
  filtros:any;
  listaNominas: UsuarioAdmin[] = [];
  mostrarNominas: boolean = false;
  numeroNomina: string;
  usuario: UsuarioAdmin;
  usuarioForm: UsuarioAdmin;
  isEdit : boolean = false;
  idUsuario: number = 0;
  dataSourceUsuarios = new MatTableDataSource<UsuarioAdmin>();
  dataUsuarios: UsuarioAdmin[] = [];
  displayedColumns: string[] = ['rol', 'nivel', 'campus', 'sede', 'fechaModificacion', 'usuarioModificacion'];
  hasData = false;
  validacionesCampos: boolean = true;
  disabledGuardar: boolean = true;
  isLoading: boolean = false;

  constructor(private usuarioAdminService: UsuarioAdminService, private fb: FormBuilder, private rolService: RolesService, private pService: PaginatorService, private pnService: PermisosNominaService,
    private router: Router, private snackBarService: SnackBarService, private dialog: MatDialog, private aRoute: ActivatedRoute, private spinner: NgxSpinnerService, private cdr: ChangeDetectorRef) { 
      
    this.isEdit = this.aRoute.snapshot.paramMap.get('id') != null ? true : false;
    }

  ngOnInit(): void {
    this.isEdit = this.aRoute.snapshot.paramMap.get('id') != null ? true : false;
    this.idUsuario =  Number(this.aRoute.snapshot.paramMap.get('id'));
    
    if (this.isEdit) {
      this.isLoading = true;
      this.spinner.show();
      this.cargarFormulario(this.idUsuario); 
      this.usuarioAdminService.obtenerHistorialUsuario(this.idUsuario).subscribe(usuarios => {
        this.dataUsuarios = usuarios;
        this.dataSourceUsuarios.data = this.dataUsuarios;
        this.cdr.detectChanges();

        this.isLoading = false;
        this.spinner.hide();
        const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataSourceUsuarios.data.length, true);
        this.paginator.pageSizeOptions = pageSizeOptions;
        this.paginator.pageSize = pageSizeOptions[0];

        this.dataSourceUsuarios.paginator = this.paginator;

      });

    }
   
    this.form = this.fb.group({
      nomina: new FormControl('',[Validators.required]),
      nombre: new FormControl('',[Validators.required]),
      correo: new FormControl('',[Validators.required]),
      campus: new FormControl('',Validators.required),
      sedes: new FormControl('',Validators.required),
      niveles: new FormControl('',Validators.required),
      roles: new FormControl('',Validators.required),
      updateOn: 'blur'
    });

    this.usuarioAdminService.obtenerCampus().subscribe(campus => {
      this.campusList = campus;
      this.campusFilter = this.campusList;
    });
    this.usuarioAdminService.obtenerSedes().subscribe(sedes => {
      this.sedesList = sedes;
    });
    this.usuarioAdminService.obtenerNiveles().subscribe(res =>{
      this.filtros = res;
    });

    this.rolService.obtenerDescripcionRoles().subscribe({
      next: (r: Roles[]) => {
        this.dataRoles = r;

      },
      error: (err: Error) => {
      }
    });

  }

  cargarFormulario(idUsuario: number):void{
    this.usuarioAdminService.obtenerUsuario(idUsuario).subscribe(usuario => {
      this.usuarioForm = usuario;
      this.form.controls["nombre"].setValue(usuario.nombre);
      this.form.controls["correo"].setValue(usuario.correo);
      this.form.controls["nomina"].setValue(usuario.nomina);
      let campus: string[] = [];
      usuario.listCampus?.forEach(function (value) {
        campus.push(value.claveCampus);
      }); 
      this.usuarioAdminService.obtenerSedes().subscribe(sedes => {
        this.sedesList = sedes;
        let array:  Sede[] = [];
        for(var i= 0; i <= campus.length -1; i++){
          let a = this.sedesList.filter(x => x.claveCampus == campus[i] as unknown);
          Array.prototype.push.apply(array,a);
        }
        this.sedesFiltradas = this.sort(array, 'descripcion');
      });
      let sedes: string[] = [];
      usuario.sedes?.forEach(function (value) {
        sedes.push(value.claveSede);
      }); 
      let niveles: string[] = [];
      usuario.niveles?.forEach(function (value) {
        niveles.push(value.claveNivel);
      }); 
      
      let roles: number[] = [];
      usuario.roles?.forEach(function (value) {
        roles.push(value.idRol);
      }); 
      
      this.form.controls["campus"].setValue(campus);
      this.form.controls["sedes"].setValue(sedes);
      this.form.controls["niveles"].setValue(niveles);
      this.form.controls["roles"].setValue(roles);

      this.form.valueChanges.subscribe({
        next: (f: any) => {
          this.disabledGuardar = false;
        }
      });

    });
  }

  guardarUsuario(): void {
    if (!this.form.valid) {
      this.validacionesCampos = false;
      return;
    }else{
      this.validacionesCampos = true;
    }
    let campus: Campus[] = [];
    for (let i = 0; i < this.form.value.campus.length; i++) {
      if(this.form.value.campus[i] != '0'){
        campus.push({ claveCampus: this.form.value.campus[i], descripcion: '' });
      }
    }
    let sedes: Sede[] = [];
    for (let i = 0; i < this.form.value.sedes.length; i++) {
      if(this.form.value.sedes[i] != '0'){
        let claveCampus = this.sedesList.find(x => x.claveSede == this.form.value.sedes[i] && x.claveSede !== '0')?.claveCampus;
        sedes.push({ claveSede: this.form.value.sedes[i], claveCampus: claveCampus, descripcion: '' });
      }
      
    }
    let niveles: Nivel[] = [];
    for (let i = 0; i < this.form.value.niveles.length; i++) {
      if(this.form.value.niveles[i] != '0'){
       niveles.push({ claveNivel: this.form.value.niveles[i], descripcion: '' });
      }
    }
    let roles: any[] = [];
    for (let i = 0; i < this.form.value.roles.length; i++) {
      if(this.form.value.roles[i] != '0'){
        roles.push({ idRol: this.form.value.roles[i], descripcion: '' });
      }
    }

    if (!this.isEdit) {
      this.usuario = {
        nomina: this.form.value.nomina, nombre: this.form.value.nombre, correo: this.form.value.correo,
        listCampus: campus, usuarioModificacion: this.pnService.obtenNomina(), sedes: sedes,
        niveles: niveles, roles: roles
      };

    } else {
      this.usuario = {
        idUsuario: this.idUsuario, nomina: this.form.value.nomina, nombre: this.form.value.nombre, correo: this.form.value.correo,
        listCampus: campus, usuarioModificacion: this.pnService.obtenNomina(), sedes: sedes,
        niveles: niveles, roles: roles
      };
    } 

    this.usuarioAdminService.guardarUsuario(this.usuario).subscribe({
      next: (r: any) => {
        if (!this.isEdit) {
          if(r.result){
            this.snackBarService.openSnackBar('Alta de nuevo usuario exitosa', 'success', 5000);
            sessionStorage.setItem("idUsuarioRegistro", String(r.idUsuario));
          }
          else{
            this.snackBarService.openSnackBar('Error al guardar usuario', 'warning', 5000);
          }
        } else {
          if(r.result){
            this.snackBarService.openSnackBar('Actualización de usuario exitosa', 'success', 5000);
            sessionStorage.setItem("idUsuarioRegistro", String(this.usuario.idUsuario));
          }
          else{
            this.snackBarService.openSnackBar('Error al actualizar usuario', 'warning', 5000);
          }
        }
        this.router.navigate(['/admin/usuarios']);
      },
      error: () => {
        if (!this.isEdit) {
          this.snackBarService.openSnackBar('Error al guardar usuario', 'default', 5000);
        } else {
          this.snackBarService.openSnackBar('Error al actualizar usuario', 'default', 5000);
        }
      }
    });
  }

  onChangeCampus(): void{
    let array:  Sede[] = [];
    for(var i= 0; i <= this.selectedValues.length -1; i++){
       let a = this.sedesList.filter(x => x.claveCampus == this.selectedValues[i] as unknown);
       Array.prototype.push.apply(array,a);
    }
    this.sedesFiltradas = this.sort(array, 'descripcion');
  }
  sort<T>(array: Array<T>, args: string): Array<T> {
    return array.sort((a: any, b: any) => {
      if (a[args].toLowerCase() < b[args].toLowerCase()) {
        return -1;
      } else if (a[args] > b[args]) {
        return 1;
      } else {
        return 0;
      }
    });
  }

  obtenerNombrePorNomina(event: any): void{
    let busqueda: string =  this.form.controls['nomina'].value;
    this.usuarioAdminService.obtenerUsuarioPorNomina(busqueda).subscribe(result => {
      this.listaNominas = result;
      this.mostrarNominas = true;
    });
  }

  alumnoSeleccionado(nomina: string): void{
    this.usuarioAdminService.obtenerUsuarioPorNomina(nomina).subscribe(result => {
      this.listaNominas = result;
      this.form.controls['nomina'].setValue(result[0].nomina);
      this.form.controls['nombre'].setValue(result[0].nombre);
      this.form.controls['correo'].setValue(result[0].correo);
      this.mostrarNominas = false;
    });
  }

  marcarTodos(combo:string): void {
    switch (combo) {
      case 'multiSelect': {
        this.todosCampus = !this.todosCampus;
        this.multiSelect.options.forEach( (item : MatOption) => this.todosCampus ? item.select() : item.deselect());
        break;
      }
      case 'multiSelectRoles':{
        this.todosRoles = !this.todosRoles;
        this.multiSelectRoles.options.forEach( (item : MatOption) => this.todosRoles ? item.select() : item.deselect());
        break;
      }
      case 'multiSelectSedes':{
        this.todosSedes = !this.todosSedes;
        this.multiSelectSedes.options.forEach( (item : MatOption) =>this.todosSedes ? item.select() : item.deselect());
        break;
      }
      case 'multiSelectNiveles':{
        this.todosNiveles = !this.todosNiveles;
        this.multiSelectNiveles.options.forEach( (item : MatOption) => this.todosNiveles ? item.select() : item.deselect());
        break;
      }
    }
    
  }
  onInputChangeCampus(event: any): void {
    const searchInput = event.target.value.toLowerCase();
    this.campusFilter = this.campusList.filter(({ descripcion }) => {
      const prov = descripcion.toLowerCase();
      return prov.includes(searchInput);
    });
  }
  onInputChangeSedes(event: any): void {
    const searchInput = event.target.value.toLowerCase();
    this.sedesFiltradas = this.sedesList.filter(({ descripcion }) => {
      const prov = descripcion?.toLowerCase();
      return prov?.includes(searchInput);
    });
  }
  cancelarCambios(): void{
    this.dialog.open(UsuarioCancelarDialog, {
      width: '500px'
    });
  }
}

@Component({
  selector: 'usuario-cancelar-modal',
  templateUrl: 'usuario-cancelar-modal.html',
})

export class UsuarioCancelarDialog {
  constructor(public dialogRef: MatDialogRef<UsuarioCancelarDialog>, private router: Router) {
  }

  cancelar(): void {
    this.dialogRef.close();
  }

  aceptar(): void {
    this.router.navigate(['/admin/usuarios']);
    this.dialogRef.close();
  }
}


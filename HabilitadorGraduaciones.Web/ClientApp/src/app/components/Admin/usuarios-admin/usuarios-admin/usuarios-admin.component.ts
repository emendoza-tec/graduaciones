import { Component, ViewChild, Inject, AfterViewInit, ChangeDetectorRef, ViewContainerRef } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Campus, Sede, UsuarioAdmin } from '../../../../interfaces/UsuarioAdmin';
import { Roles } from '../../../../classes/Roles';
import { RolesService } from '../../../../services/roles.service';
import { UsuarioAdminService } from '../../../../services/usuario-admin.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ExcelService } from 'src/app/services/exportarExcel.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PaginatorService } from '../../../../services/paginator.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { UtilsService } from '../../../../services/utils.service';
import { UsuarioService } from 'src/app/services/usuario.service';

@Component({
  selector: 'app-usuarios-admin',
  templateUrl: './usuarios-admin.component.html',
  styleUrls: ['./usuarios-admin.component.css'],
})

export class UsuariosAdminComponent implements AfterViewInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  rolesForm = new FormControl();
  campusForm = new FormControl();
  searchForm = new FormControl('');
  rolesList: Roles[] = [];
  campusList: Campus[] = [];
  dataUsuarios: UsuarioAdmin[] = [];
  hasData = false;
  rolesFiltrados: any[] = this.rolesList;
  campusFiltrados: any[] = this.campusList;
  displayedColumns: string[] = ['nomina', 'nombre', 'correo', 'campus', 'rol', 'acciones'];
  dataSourceUsuarios = new MatTableDataSource<UsuarioAdmin>();
  permisos: PermisosMenu;
  permisoMenuId: PermisosMenu;
  lengthPaginator = 5;
  permisoEditar: boolean = true;
  isLoading: boolean = false;

  constructor(private spinner: NgxSpinnerService, private vr: ViewContainerRef, private cdr: ChangeDetectorRef, private _live: LiveAnnouncer, private excelExportService: ExcelService, private dialog: MatDialog,
    private pnService: PermisosNominaService, private rolesService: RolesService, private usuarioAdminService: UsuarioAdminService, private pService: PaginatorService, 
    private permisoService: PermisosNominaService, private snackBarService: SnackBarService) {
      this.isLoading = true;
      this.spinner.show();
      this.permisoEditar = true;
  }

  ngAfterViewInit(): void {
    this.permisoMenuId = this.permisoService.obtenSubmenuPermisoPorNombre('usuarios');
    this.permisos = this.permisoService.revisarSubmenuPermisoPorId(this.permisoMenuId.idMenu , this.permisoMenuId.idSubMenu);
    this.permisoEditar = this.permisos.editar;
    this.cdr.detectChanges();
    this.cargaClases();
    this.obtenerUsuariosAdmin();

    this.rolesService.obtenerRoles().subscribe({
      next: (r: Roles[]) => {
        this.rolesList = r.filter(x => x.activo);
      },
      error: (err: Error) => {
      }
    });

    this.usuarioAdminService.obtenerCampus().subscribe(campus => {
      this.campusList = campus;
    });

    this.dataSourceUsuarios.filterPredicate = (data: string | any, filter: string) => {
      let busqueda = JSON.parse(filter);
      let bandRol = false;
      let bandCampus = false;
      let rolesFiltro: any = [];
      let campusFiltro: any = [];
      if (busqueda.rol == undefined) {
        bandRol = true;
      } else {
        rolesFiltro = busqueda.rol.split(',');
      }
      if (busqueda.campus == undefined) {
        bandCampus = true;
      } else {
        campusFiltro = busqueda.campus.split(',');
      }
      let roles = data.rol.split(',');
      let campus = data.campus.split(',');
      roles = roles.map((r: string) => r.trim());
      campus = campus.map((c: string) => c.trim());
      for (let i = 0; roles.length > i; i++) {
        if (rolesFiltro.includes(roles[i])) {
          bandRol = true;
        }
      }
      for (let i = 0; campus.length > i; i++) {
        if (campusFiltro.includes(campus[i])) {
          bandCampus = true;
        }
      }
      if (campusFiltro.length == 1 && campusFiltro[0] == '') {
        bandCampus = true;
      }
      if (rolesFiltro.length == 1 && rolesFiltro[0] == '') {
        bandRol = true;
      }
      return bandRol && bandCampus && (data.nombre.toLowerCase().includes(busqueda.usuario) || data.nomina.toLowerCase().includes(busqueda.usuario) || data.correo.toLowerCase().includes(busqueda.usuario));
    }
  }

  obtenerUsuariosAdmin(): void {
    let idUsuario: string = JSON.parse(sessionStorage.getItem("idUsuarioRegistro") || '{}');

    this.usuarioAdminService.obtenerUsuarios().subscribe({
      next: (r: UsuarioAdmin[]) => {
        this.dataUsuarios = r;
        this.dataUsuarios = this.sortArray(this.dataUsuarios, 'nombre');
        if (idUsuario != null) {
          const us = this.dataUsuarios.find(usuario => usuario.idUsuario == Number(idUsuario));
          if (us != null) {
            us.nuevo = true;
            this.dataUsuarios = this.dataUsuarios.filter(usuario => usuario.idUsuario !== Number(idUsuario));
            this.dataUsuarios = this.sortArray(this.dataUsuarios, 'nombre');
            this.dataUsuarios.unshift(us);
            sessionStorage.setItem("idUsuarioRegistro", "");
          }
        }
        this.dataSourceUsuarios.data = this.dataUsuarios;
        this.cdr.detectChanges();

        const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataSourceUsuarios.data.length, false);
        this.paginator.pageSizeOptions = pageSizeOptions;
        this.paginator.pageSize = pageSizeOptions[0]; 
        this.dataSourceUsuarios.paginator = this.paginator;
        this.dataSourceUsuarios.sort = this.sort;
        this.isLoading = false;
        this.spinner.hide();
      },
      error: (err: Error) => {
        this.isLoading = false;
        this.spinner.hide();
      }
    });
  }

  sortArray<T>(array: Array<T>, args: string): Array<T> {
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

  sortChange(sortState: Sort): void {
    if (sortState.direction) {
      this._live.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._live.announce('Sorting cleared');
    }
  }

  filtroUsuarios(event: Event): void {
    this.filtrar();
  }

  exportarExcelUsuarios(): void {
    const excelFiltros = this.dataSourceUsuarios.filteredData;
    excelFiltros.forEach((d: UsuarioAdmin) => {
      delete d.idUsuario;
      delete d.estatus;
      delete d.fechaRegistro;
      delete d.fechaModificacion;
      delete d.usuarioModificacion;
      delete d.listCampus;
      delete d.sedes;
      delete d.niveles;
      delete d.sede;
      delete d.nivel;
      delete d.roles;
      delete d.fechaCreacion;
      delete d.result;
      delete d.errorMessage;
      delete d.cantidadRoles;
      delete d.nuevo;
    });
    this.excelExportService.exportarExcel(excelFiltros, 'Usuarios' + '.xlsx');
  }


  cargarExcelUsuarios(): void {
    const dialogRef = this.dialog.open(DescargarTemplateUsuariosDialog, {
      width: '570px',
      height: 'auto',
      disableClose: true,
      data: {
        campus: this.campusList, roles: this.rolesList
      }
    });
    dialogRef.afterClosed().subscribe(usuarios => {
      if (usuarios) {
        this.obtenerUsuariosAdmin();
        this.snackBarService.openSnackBar("Carga de usuarios éxitosa", 'success', 5000);
      }else{
        this.snackBarService.openSnackBar("Error al cargar usuarios, favor de revisar el template", 'warn', 5000);
      }
    });
  }

  filtroMultiRol(): void {
    this.filtrar();
  }

  filtroMultiCampus(): void {
    this.filtrar();
  }

  filtrar(): void {
    this.dataSourceUsuarios.filter = JSON.stringify({
      usuario: this.searchForm.getRawValue()?.trim().toLowerCase(),
      rol: this.rolesForm.getRawValue()?.toString(),
      campus: this.campusForm.getRawValue()?.toString(),
    });
  }

  onInputChangeRoles(event: any): void {
    const searchInput = event.target.value.toLowerCase();
    this.rolesFiltrados = this.rolesList.filter(({ descripcion }) => {
      const prov = descripcion.toLowerCase();
      return prov.includes(searchInput);
    });
  }

  onOpenChangeRoles(searchInput: any): void {
    searchInput.value = "";
    this.rolesFiltrados = this.rolesList;
  }

  onInputChangeCampus(event: any): void {
    const searchInput = event.target.value.toLowerCase();
    this.campusFiltrados = this.campusList.filter(({ descripcion }) => {
      const prov = descripcion.toLowerCase();
      return prov.includes(searchInput);
    });
  }

  onOpenChangeCampus(searchInput: any): void {
    searchInput.value = "";
    this.campusFiltrados = this.campusList;
  }

  eliminarUsuario(usuario: UsuarioAdmin): void {
    const dialogRef = this.dialog.open(EliminarUsuarioDialog, {
      width: '500px',
      data: { usuario: usuario, usuarioElimino: this.pnService.obtenNomina() }
    });

    dialogRef.afterClosed().subscribe(r => {
      if (r == true) {
        this.obtenerUsuariosAdmin();
      }
    });
  }

  

  cargaClases(): void {
    this.vr.element.nativeElement.querySelector(
      'mat-select[name="rolesForm"] > div > div.mat-select-arrow-wrapper'
    ).classList.add('noDisplay');

    this.vr.element.nativeElement.querySelector(
      'mat-select[name="campusForm"] > div > div.mat-select-arrow-wrapper'
    ).classList.add('noDisplay');

    this.vr.element.nativeElement.querySelector(
      'mat-form-field[name="labelRol"] > div > div.mat-form-field-flex > div > span > label'
    ).classList.add('usuariosLabelSelect');

    this.vr.element.nativeElement.querySelector(
      'mat-form-field[name="labelCampus"] > div > div.mat-form-field-flex > div > span > label'
    ).classList.add('usuariosLabelSelect');
  }

}


@Component({
  selector: 'suario-eliminar-dialog',
  templateUrl: 'usuario-eliminar-dialog.html',
  styleUrls: ['./usuarios-admin.component.css']
})
export class EliminarUsuarioDialog {
  constructor(public dialogRef: MatDialogRef<EliminarUsuarioDialog>, @Inject(MAT_DIALOG_DATA) public data: any, private usuarioService: UsuarioAdminService, private snackBarService: SnackBarService) {
  }

  cancelar(): void {
    this.dialogRef.close();
  }

  aceptar(): void {
    this.usuarioService.eliminarUsuario(this.data.usuario.idUsuario, this.data.usuarioElimino).subscribe({
      next: (r: any) => {
        if (r.result) {
          this.snackBarService.openSnackBar("El usuario seleccionado se ha eliminado exitosamente", 'success', 5000);
          this.dialogRef.close(r.result);
        } else {
          this.snackBarService.openSnackBar("Error al eliminar el usuario", 'default', 5000);
        }
      },
      error: (err: HttpErrorResponse) => {
        this.snackBarService.openSnackBar("Error al eliminar el usuario seleccionado", 'default', 5000);
      }
    });
  }
}


@Component({
  selector: 'usuarios-descargar-template-dialog',
  templateUrl: 'usuarios-descargar-template-dialog.html',
  styleUrls: ['./usuarios-admin.component.css']
})
export class DescargarTemplateUsuariosDialog {
  archivoUsuario?: File;
  disableAction = false;
  isAnalizandoData = false;
  archivoCargado = false;
  usuariosExcel: UsuarioAdmin [] = [];
  sedesList: Sede[] = [];
  campusList: Campus[] = [];
  rolesList: Roles[] = [];
  nivelList: any;
  errorCampus:boolean = false;
  errorSede:boolean = false;
  errorNivel:boolean = false
  errorRol:boolean = false;
  archivoPlantillaUsu = 'api/archivo/download?fileName=TemplateUsuarios.xlsx';

  constructor(public dialogRef: MatDialogRef<DescargarTemplateUsuariosDialog>, @Inject(MAT_DIALOG_DATA) public data: any, private excelService: ExcelService, private rolService: RolesService,
    private snackBarService: SnackBarService, private utilsService: UtilsService, private usuarioService: UsuarioService, private usuarioAdminService: UsuarioAdminService,  private pnService: PermisosNominaService) {
      this.usuarioAdminService.obtenerSedes().subscribe(sedes => {
        this.sedesList = sedes;
      });

      this.usuarioAdminService.obtenerNiveles().subscribe(res =>{
        this.nivelList = res;
      });
      this.campusList = data.campus;
      ;this.rolService.obtenerDescripcionRoles().subscribe({
        next: (r: Roles[]) => {
          this.rolesList = r;
        },
        error: (err: Error) => {
        }
      });
  }

  cancelar(): void {
    this.dialogRef.close();
  }
  cargar(): void {

    this.errorCampus = false;
      this.errorSede = false;
      this.errorNivel = false
      this.errorRol = false;
    this.disableAction = true;
    if (this.archivoUsuario) {
      //Procesar excel para saber si no tiene datos incorrectos
      this.excelService.usuarioExcelToJson(this.archivoUsuario).subscribe(r => {
        if (r[0].estatus) {
          this.usuariosExcel = r;
          this.usuariosExcel.forEach((usuario) => {
            if(!this.campusList.some(e => e.claveCampus == usuario.campus)){
              this.errorCampus = true;
            }
  
            if(!this.nivelList.some((e: any) => e.descripcion.toUpperCase() == usuario.nivel?.toUpperCase())){
              this.errorSede = true;
            }
  
            if(!this.sedesList.some(e => e.claveSede == usuario.sede)){
              this.errorNivel = true;
            }
  
            if(!this.rolesList.some(e  => e.descripcion.toUpperCase() == usuario.rol?.toUpperCase())){
              this.errorRol = true;
            }
          });
          if (this.archivoUsuario) {
            if(!this.errorSede && !this.errorRol && !this.errorCampus && !this.errorNivel){
              this.usuarioService.guardarCargaArchivo(this.archivoUsuario,  this.pnService.obtenNomina()).subscribe((r:any )=> {
                if(r.result == 'success') {
                  this.dialogRef.close(true);
                }else{
                  this.dialogRef.close(true);
                }
                
              });
            }
          }
        }
        else{
          this.dialogRef.close(false);
        }
      });
      
    }
  }

  onFileDropped($event: any): void {
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
        this.archivoUsuario = files.target.files[0];
        this.archivoCargado = true;
      } else {
        this.snackBarService.openSnackBar('Template incorrecto', 'default');
      }
    }
  }
  resetEvent($event: any): void {
    $event.target.value = '';
  }
  formatBytes(bytes: any): string {
    return this.utilsService.formatBytes(bytes);
  }
}

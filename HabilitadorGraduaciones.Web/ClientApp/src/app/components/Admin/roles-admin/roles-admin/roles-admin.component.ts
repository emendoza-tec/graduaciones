import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild, Inject, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { Roles, usuarioRol } from 'src/app/classes/Roles';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { PaginatorService } from 'src/app/services/paginator.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { RolesService } from 'src/app/services/roles.service';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'app-roles-admin',
  templateUrl: './roles-admin.component.html',
  styleUrls: ['./roles-admin.component.css']
})
export class RolesAdminComponent implements AfterViewInit {

  //Tabla Roles
  dataRoles: MatTableDataSource<Roles>;
  displayedColumns: string[] = ['descripcion', 'totalUsuarios', 'fechaRegistro', 'usuarioRegistro', 'fechaModificacion', 'usuarioModifico', 'estatus', 'acciones'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  estatusSeleccionado = null;
  isLoading: boolean = false;
  hasData: boolean = false;
  permisos: PermisosMenu;
  permisoEditar: boolean = true;
  permisoMenuId: PermisosMenu;
  valoresAFiltrar = {
    busquedaGeneral: '',
    estatus: ''
  };

  constructor(private pService: PaginatorService, private spinner: NgxSpinnerService, private cdr: ChangeDetectorRef, 
    private snackBarService: SnackBarService, private dialog: MatDialog, private rolService: RolesService, private pnService: PermisosNominaService,
    private titleService : TitleService) {
    this.titleService.changeTitle('');
  }

  ngAfterViewInit(): void {
    this.permisoMenuId = this.pnService.obtenSubmenuPermisoPorNombre('roles');
    this.permisos = this.pnService.revisarSubmenuPermisoPorId(this.permisoMenuId.idMenu , this.permisoMenuId.idSubMenu);
    this.permisoEditar = this.permisos.editar;
    this.cdr.detectChanges();
    this.consultaRoles();
  }

  consultaRoles():void {
    this.isLoading = true;
    this.spinner.show();
    this.cdr.detectChanges(); 
    this.rolService.obtenerRoles().subscribe({
      next: (r: Roles[]) => {
        if (history.state.rol) {
          const rolNuevo = r.find(f => f.descripcion == history.state.rol.descripcion);
          if (rolNuevo) {
            rolNuevo.nuevo = true;
            r = r.filter(rol => rol.descripcion !== rolNuevo?.descripcion);
            r.unshift(rolNuevo);
          }
        }
        this.dataRoles = new MatTableDataSource<Roles>(r);
        this.cdr.detectChanges();

        const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataRoles.data.length, false);
        this.paginator.pageSizeOptions = pageSizeOptions;
        this.paginator.pageSize = pageSizeOptions[0];
        this.cdr.detectChanges(); 
      },
      error: () => {
        this.snackBarService.openSnackBar("Error al obtener roles", 'default', 5000);
      },
      complete: () => {       
        this.dataRoles.filterPredicate = this.crearFiltro();
        this.dataRoles.sort = this.sort;
        this.dataRoles.paginator = this.paginator;        
        this.isLoading = false;
        this.spinner.hide();
        this.cdr.detectChanges();
      }
    });
  }

  crearFiltro(): (data: string | any, filter: string) => boolean {
    let filterFunction = function (data: string | any, filter: string): boolean {
      let searchTerms = JSON.parse(filter);
      return (data.idRol.toString().toLowerCase().includes(searchTerms.busquedaGeneral) || data.descripcion.toLowerCase().includes(searchTerms.busquedaGeneral)
        || data.fechaRegistro.toLowerCase().includes(searchTerms.busquedaGeneral) || data.usuarioRegistro.toLowerCase().includes(searchTerms.busquedaGeneral)
        || data.fechaModificacion.toLowerCase().includes(searchTerms.busquedaGeneral) || data.usuarioModifico.includes(searchTerms.busquedaGeneral))
        && data.estatus.toString().toLowerCase().includes(searchTerms.estatus)
    }
    return filterFunction;
  }

  filtroRoles(event?: Event): void {
    if (event) {
      const busquedaGeneral = (event.target as HTMLInputElement).value;
      this.valoresAFiltrar.busquedaGeneral = (busquedaGeneral.toLocaleLowerCase());
    }
    this.dataRoles.filter = JSON.stringify(this.valoresAFiltrar);

  }

  selecccionarFiltro(): void {
    this.valoresAFiltrar.estatus = String(this.estatusSeleccionado);
    this.filtroRoles();
  }

  eliminarRol(rol: Roles): void {
    rol.usuarioModifico = this.pnService.obtenNomina();
    const dialogRef = this.dialog.open(RolEliminarDialog, {
      width: '500px',
      data: { rol, isEliminar: true }
    });

    dialogRef.afterClosed().subscribe(r => {
      if (!r.isCancelar) {
        this.consultaRoles();
      }
    });
  }

  modificaEstatusRol(rol: Roles, i: number, event: any): void {
    rol.usuarioModifico = this.pnService.obtenNomina();
    rol.fechaModificacion = new Date();

    const dialogRef = this.dialog.open(RolEliminarDialog, {
      width: '500px',
      data: { rol, isEliminar: false, valueCheck: event.target.checked }
    });

    dialogRef.afterClosed().subscribe(r => {
      if (!r.isCancelar) {
        this.consultaRoles();
      } else {
        !event.target.checked ? this.dataRoles.data[i].estatus = true : this.dataRoles.data[i].estatus = false;
      }
    });
  }

  verUsuariosAsignados(rol: Roles): void {
    this.dialog.open(verUsuariosAsignadosDialog, {
      width: '500px',
      data: { rol }
    });
  }
}

@Component({
  selector: 'rolEliminarDialog',
  templateUrl: 'rolEliminarDialog.html',
  styleUrls: ['./roles-admin.component.css']
})

export class RolEliminarDialog {
  acciones = {
    isCancelar: false,
    result: false
  };

  usuariosUnRol: usuarioRol[];
  hasUsuariosUnRol = false;
  constructor(public dialogRef: MatDialogRef<RolEliminarDialog>, @Inject(MAT_DIALOG_DATA) public data: any, private rolService: RolesService, private snackBarService: SnackBarService) {
    this.usuariosUnRol = data.rol.usuarios.filter((f: usuarioRol) => f.roles == 1);
    this.hasUsuariosUnRol = this.usuariosUnRol.length > 1;
  }

  cancelar(): void {
    this.acciones.isCancelar = true;
    this.dialogRef.close(this.acciones);
  }

  aceptar(): void {
    if (this.data.isEliminar) {
      this.rolService.eliminarRol(this.data.rol).subscribe({
        next: (r: any) => {
          if (r.result) {
            this.snackBarService.openSnackBar("Rol eliminado", 'default', 5000);
            this.acciones.result = r.result
            this.dialogRef.close(this.acciones);
          } else {
            this.snackBarService.openSnackBar("Error al eliminar el rol", 'default', 5000);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.snackBarService.openSnackBar("Error al eliminar el rol seleccionado", 'default', 5000);
        }
      });
    } else {
      this.rolService.cambiaEstatusRol(this.data.rol).subscribe({
        next: (r: any) => {
          if (r.result) {
            if (this.data.rol.estatus) {
              this.snackBarService.openSnackBar("Rol habilitado", 'default', 5000);
              this.acciones.result = r.result;
              this.dialogRef.close(r.result);
            } else {
              this.snackBarService.openSnackBar("Rol deshabilitado", 'default', 5000);
              this.acciones.result = r.result;
              this.dialogRef.close(this.acciones);
            }
          } else {
            this.snackBarService.openSnackBar("Error al deshabilitar rol", 'default', 5000);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.snackBarService.openSnackBar("Error al deshabilitar el rol seleccionado", 'default', 5000);
        }
      });
    }
  }
}

@Component({
  selector: 'verUsuariosAsignadosDialog',
  templateUrl: 'verUsuariosAsignadosDialog.html',
  styleUrls: ['./roles-admin.component.css']
})

export class verUsuariosAsignadosDialog {

  constructor(public dialogRef: MatDialogRef<RolEliminarDialog>, @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  aceptar(): void {
    this.dialogRef.close();
  }
}

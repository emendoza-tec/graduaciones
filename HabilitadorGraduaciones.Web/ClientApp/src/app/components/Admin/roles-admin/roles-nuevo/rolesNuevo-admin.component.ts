import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Permisos, Roles, Seccion, SeccionesPermisos } from 'src/app/classes/Roles';
import { rolValidator } from 'src/app/helpers/customValidations';
import { RolesService } from 'src/app/services/roles.service';
import { groupBy, keys, sortBy } from "lodash-es";
import { ActivatedRoute, Router } from '@angular/router';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { Title } from '@angular/platform-browser';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'app-rolesNuevo-admin',
  templateUrl: './rolesNuevo-admin.component.html',
  styleUrls: ['./rolesNuevo-admin.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class RolesNuevoAdminComponent implements OnInit {

  form!: FormGroup;
  isEditar: boolean = false;
  isVer: boolean = false;
  allRowsExpanded: boolean = false;

  //Tabla Secciones
  dataSeccionesPermisos: MatTableDataSource<Seccion>;
  displayedColumns: string[] = ['columna', 'ver', 'editar'];
  columnsToDisplayWithExpand = [...this.displayedColumns, 'expand'];
  expandedElement: SeccionesPermisos | null;
  permisosObsoletos: Permisos[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  totalPages = 0;

  //Tabla de cada seccion
  dataMenuSecciones: MatTableDataSource<Permisos>;
  displayedColumnsMenuSecciones: string[] = ['menu', 'ver', 'editar'];

  isLoading = false;
  hasData = false;
  hasChangePermisoRol = false;
  disableGuardar = true;

  @Output() titleEmmiter = new EventEmitter<{ title: string }>();
  constructor(private spinner: NgxSpinnerService, private cdr: ChangeDetectorRef, private dialog: MatDialog, private fb: FormBuilder, 
    private rolService: RolesService, private router: Router, private snackBarService: SnackBarService, private aRoute: ActivatedRoute, 
    private pnService: PermisosNominaService, private titleService : TitleService, private title: Title) {
    const editar = this.aRoute.snapshot.paramMap.get('isEditar');
    if (editar != null) {
      const boolEditar = +editar != 0;
      if (boolEditar) {
        this.isEditar = true; 
        this.titleService.changeTitle('Editar'); 
      } else {
        this.isVer = true; 
        this.titleService.changeTitle('Ver');
      }
    }else{
      this.titleService.changeTitle('Nuevo');
    }
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      rol: ['', Validators.required],
      updateOn: 'blur'
    });
    if (this.isVer) {
      this.rol?.disable();
    }

    let roles: Roles[];
    this.rolService.obtenerRoles().subscribe({
      next: (r: Roles[]) => {
        roles = r;
      },
      error: () => {
        this.snackBarService.openSnackBar('Error al obtener roles', 'default', 5000);
      },
      complete: () => {
        if (this.isEditar || this.isVer) {
          const findRol = roles.find(f => f.idRol == Number(this.aRoute.snapshot.paramMap.get('id')));
          this.form.controls["rol"].setValue(findRol?.descripcion);
        }
        this.form.controls["rol"].addValidators(rolValidator(roles));
      }
    });

    this.rolService.obtenerSecciones().subscribe({
      next: (r: Permisos[]) => {
        const lsKeys = keys(groupBy(r, "nombreMenu"));
        const sortKeys = sortBy(lsKeys);
        let columns: Seccion[] = []
        sortKeys.forEach((e: string) => {
          let filtroSeccionHijo = r.filter(f => f.nombreMenu === e);
          let columna = new Seccion(e, filtroSeccionHijo);
          columns.push(columna);
        });

        this.dataSeccionesPermisos = new MatTableDataSource<Seccion>(columns);

        this.dataMenuSecciones = new MatTableDataSource<Permisos>(r)
      },
      error: () => {
        this.snackBarService.openSnackBar('Error al obtener secciones', 'default', 5000);
      },
      complete: () => {
        if (this.isEditar || this.isVer) {
          const idRol = Number(this.aRoute.snapshot.paramMap.get('id'));

          this.rolService.obtenerRolesPorId(idRol).subscribe({
            next: (r: Roles) => {
              r.permisos.forEach((e: Permisos) => {
                const isLargeNumber = (f: Permisos) => f.idMenu === e.idMenu && f.idSubMenu === e.idSubMenu;
                const indexPermiso = this.dataMenuSecciones.data.findIndex(isLargeNumber);
                if(indexPermiso  != -1){
                  this.dataMenuSecciones.data[indexPermiso].ver = e.ver;
                  this.dataMenuSecciones.data[indexPermiso].editar = e.editar;
                }else{
                  e.activa = false;
                  this.permisosObsoletos.push(e);
                }
              });
            },
            error: () => {
              this.snackBarService.openSnackBar('Error al obtener rol por id', 'default', 5000);
            },
          });
        }
      }
    });
  }

  guardaRol(): void {
    let permisos: Permisos[] = [];
    if(this.permisosObsoletos.length >= 1){
      this.permisosObsoletos.forEach(element => {
        permisos.push(element);
      }); 
    }
    this.dataMenuSecciones.data.forEach((e, index) => {
      let permiso = new Permisos(index, e.idMenu, e.nombreMenu, e.idSubMenu, e.nombreSubMenu, e.ver, e.editar, e.activa, false, '');
      permisos.push(permiso);
    });

    let nuevoRol = new Roles(1, this.rol?.value, true, 0, [], '', new Date(), '', new Date(), true, permisos, true, '');

    if (!this.isEditar) {
      nuevoRol.usuarioRegistro = this.pnService.obtenNomina();
      this.isLoading = true;
      this.disableGuardar = true;
      this.spinner.show();
      this.rolService.guardaRol(nuevoRol).subscribe({
        next: (r: any) => {
          if(r.result){
            this.snackBarService.openSnackBar('El nuevo rol ha sido creado con éxito.', 'success', 5000);
            this.router.navigateByUrl('/admin/roles', { state: { rol: nuevoRol } });
            this.isLoading = false;
            this.disableGuardar = true;
            this.spinner.hide();
          }else{
            this.snackBarService.openSnackBar('No se ha podido guardar rol con éxito.', 'success', 5000);
            this.isLoading = false;
            this.disableGuardar = false;
            this.spinner.hide();
          }

        },
        error: () => {
          this.snackBarService.openSnackBar('Error al guarda rol', 'default', 5000);
          this.isLoading = false;
          this.disableGuardar = false;
          this.spinner.hide();
        }
      });
    } else {
      const idRol = Number(this.aRoute.snapshot.paramMap.get('id'));
      nuevoRol.idRol = idRol;
      nuevoRol.usuarioModifico = this.pnService.obtenNomina();
      this.isLoading = true;
      this.disableGuardar = true;
      this.spinner.show();
      this.rolService.modificaRol(nuevoRol).subscribe({
        next: (r: any) => {
          if (r.result) {
            this.snackBarService.openSnackBar('Tus cambios han sido registrados con éxito.', 'success', 5000);
            this.router.navigateByUrl('/admin/roles', { state: { rol: nuevoRol } });
            this.isLoading = false;
            this.disableGuardar = true;
            this.spinner.hide();
          }else{
            this.snackBarService.openSnackBar('No se han podido registradar tus cambios con éxito.', 'success', 5000);
            this.isLoading = false;
            this.disableGuardar = false;
            this.spinner.hide();
          }
        },
        error: () => {
          this.snackBarService.openSnackBar('Error al guarda rol', 'default', 5000);
          this.isLoading = false;
          this.disableGuardar = false;
          this.spinner.hide();
        }
      });
    }
  }

  toggle(): void {
    this.allRowsExpanded = !this.allRowsExpanded;
    this.expandedElement = null;
  }


  get rol() { return this.form.get('rol'); }

  cancelarRol(): void {
    if (this.isVer) {
      this.router.navigate(['/admin/roles']);
    } else {
      this.dialog.open(RolCancelarDialog, {
        width: '500px'
      });
    }
  }

  isChangePermisosRoles(): void {
    this.hasChangePermisoRol = true;
    this.checkDisable();
  }

  checkDisable(): void {
    if(!this.isEditar){
      this.hasChangePermisoRol && !this.rol?.invalid ? this.disableGuardar = false : this.disableGuardar = true;
    }else{
      this.hasChangePermisoRol || !this.rol?.invalid ? this.disableGuardar = false : this.disableGuardar = true;
    }
  }

  editarRol(): void {
    this.router.navigate(['/admin/roles/editar', 1, this.aRoute.snapshot.paramMap.get('id')]);
  }

  checkVer(indexSH: number, element: any, event: any): void {
    let indexPadre = this.dataSeccionesPermisos.data.findIndex(f => f == element);
    if(event.target.checked){
      this.dataSeccionesPermisos.data[indexPadre].seccionesHijos[indexSH].ver = event.target.checked;
      this.cdr.detectChanges();
    }
  }
}

@Component({
  selector: 'rolCancelarModal',
  templateUrl: 'rolCancelarModal.html',
})

export class RolCancelarDialog {
  constructor(public dialogRef: MatDialogRef<RolCancelarDialog>, private router: Router) {
  }

  cancelar(): void {
    this.dialogRef.close();
  }

  aceptar(): void {
    this.router.navigate(['/admin/roles']);
    this.dialogRef.close();
  }
}

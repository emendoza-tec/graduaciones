import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { ModeloInformacionUsuario } from 'src/app/classes';
import { Calendario } from 'src/app/interfaces/Calendario';
import { CampusNomina, PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { CalendariosService } from 'src/app/services/calendarios.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { SnackBarService } from 'src/app/services/snackBar.service'

@Component({
  selector: 'app-calendario-admin',
  templateUrl: './calendario-admin.component.html',
  styleUrls: ['./calendario-admin.component.css']
})
export class CalendarioAdminComponent implements OnInit {

  displayedColumns = ['campus', 'linkProspecto', 'linkCandidato'];
  dataSource = new MatTableDataSource<any>();
  calendarios: Calendario[] = [];
  calendariosUpdate: Calendario[] = [];
  filtro: string = "";
  permisosMenu: PermisosMenu[] = [];
  permisos: PermisosMenu;
  permisoVer :boolean;
  permisoEditar: boolean;
  permisosCampus: CampusNomina[] = [];
  campus: string[] = [];

  constructor(private calendariosService : CalendariosService, private snackBarService: SnackBarService, private spinner: NgxSpinnerService, 
    private permisoNominaService: PermisosNominaService, private pnService: PermisosNominaService ) {
   }

  ngOnInit(): void {    
    this.permisosMenu = this.pnService.obtenMenu();
    this.permisos = this.permisosMenu.find(permiso => permiso.nombreMenu == 'Calendario')!;
    this.permisoVer = this.permisos.ver;
    this.permisoEditar = this.permisos.editar;
    this.permisosCampus = this.pnService.obtenCampus();
    this.permisosCampus.forEach(campus => this.campus.push(campus.claveCampus));
    this.llenadoTabla();
  }

  llenadoTabla(): void {
    this.calendariosService.getCalendarios().subscribe((r: any) => {
      this.calendarios = r.calendarios;
      if(this.permisoEditar){
        this.calendarios.forEach(calendario => calendario.permiso = (this.campus.includes(calendario.claveCampus)));
      }
      else{
        this.calendarios.forEach(calendario => calendario.permiso = false);
      }
      this.dataSource = new MatTableDataSource(this.calendarios);
      this.dataSource.filterPredicate = (data: { campus: string }, filterValue: string) =>
        data.campus.trim().toLowerCase().indexOf(filterValue) !== -1;
      this.calendariosUpdate = [];
    });
  }

  actualizarCalendarios() {
    this.spinner.show();
    this.calendariosUpdate.forEach(calendario => {
      calendario.idUsuario = this.permisoNominaService.obtenNomina();
    });
    this.calendariosService.guardarConfiguracionCalendarios(this.calendariosUpdate).subscribe((res: any) => {
      this.spinner.hide();
      if (res.result) {
        this.snackBarService.openSnackBar("Calendarios actualizados correctamente.", "default");
        this.llenadoTabla();
        const e: Event = <Event><any>{
          target: {
              value: ''      
          }
        };
        this.filtro = "";
        this.busqueda(e);
      } else {
        this.snackBarService.openSnackBar("Ocurrió un error al actualizar los calendarios.", "default");
      }
    },(error => {
      this.spinner.hide();
      this.snackBarService.openSnackBar("Ocurrió un error al actualizar los calendarios.", "default");
    }));
  }

  busqueda(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  actualizar(calendarioId:string){
    let calendario = this.dataSource.data.find(x => x.calendarioId == calendarioId);
    var existe = this.calendariosUpdate.find(x => x.calendarioId == calendarioId);
    if(existe == null || existe == undefined){
      this.calendariosUpdate.push(calendario);
    }      
    else{
      var index = this.calendariosUpdate.indexOf(calendario);
      this.calendariosUpdate[index] = calendario;
    }
  }

}

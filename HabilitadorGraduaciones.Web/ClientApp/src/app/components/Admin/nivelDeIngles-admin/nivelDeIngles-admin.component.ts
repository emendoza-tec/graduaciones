import { Component, OnInit } from '@angular/core';
import { NivelInglesService } from 'src/app/services/requisitos/nivelIngles.service';
import { Programa, NivelIngles } from '../../../classes/NivelIngles';
import { MatTableDataSource } from '@angular/material/table';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { ConfiguracionNivelIngles } from '../../../interfaces/NivelIngles';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';


@Component({
  selector: 'app-nivelDeIngles-admin',
  templateUrl: './nivelDeIngles-admin.component.html',
  styleUrls: ['./nivelDeIngles-admin.component.css']
})

export class NivelDeInglesAdminComponent implements OnInit {
  datActualizacionInglesNivel: Date = new Date();
  programa: Programa = new Programa('');
  displayedColumns = ['Programa', 'B2', 'C1'];
  dataSource = new MatTableDataSource<any>();
  titulo = "Configurar requisito Actualización: " + this.datActualizacionInglesNivel.toLocaleDateString();

  permisos: PermisosMenu;
  permisoMenuId: PermisosMenu;
  permisoEditar: boolean = true;


  constructor(private nivelInglesService: NivelInglesService, private snackBarService: SnackBarService, private spinner: NgxSpinnerService,
    private permisoService: PermisosNominaService) {
    this.permisoEditar = true;
   }

  ngOnInit() {
    this.llenadoTabla()
    this.permisoMenuId = this.permisoService.obtenSubmenuPermisoPorNombre('Nivel de Inglés'); /**/
    this.permisos = this.permisoService.revisarSubmenuPermisoPorId(this.permisoMenuId.idMenu , this.permisoMenuId.idSubMenu); /**/
    this.permisoEditar = this.permisos.editar;
  }

  llenadoTabla() {
    this.nivelInglesService.getProgramas().subscribe((r: any) => {
      this.programa = r;
      this.dataSource.data = r.programa;
    });
  }

  ngAfterViewChecked(): void {
    this.dataSource.data.forEach(nivelChecado => {
      let radio = "radio" + nivelChecado.nombrePrograma.trim();
      let x = document.querySelector('input[name=' + radio + '][value="' + nivelChecado.nivelIngles + '"]') as HTMLInputElement | null | undefined;
      if (x != null && nivelChecado.nivelIngles != null) {
        x!.value = nivelChecado.nivelIngles;
        x!.checked = true;
      }

    })
  }

  configInglesPrograma() {
    this.spinner.show();
    let arreglo: ConfiguracionNivelIngles[] = [];
    this.dataSource.data.forEach(programa => {
      let radio = "radio" + programa.nombrePrograma.trim();

      let x = document.querySelector('input[name=' + radio + ']:checked') as HTMLInputElement | null;
      if (x?.value != undefined) {
        let obj: ConfiguracionNivelIngles = {
          claveProgramaAcademico: programa.nombrePrograma.trim(),
          idNivelIngles: x?.value.toString()
        }
        arreglo.push(obj);
      }
    });
    this.nivelInglesService.guardarConfiguracionNivelIngles(arreglo).subscribe((res: any) => {
      this.spinner.hide();
      if (res.result) {
        this.snackBarService.openSnackBar("programa actualizado correctamente.", "default");
        this.llenadoTabla();
        let x = document.querySelector('input[name=filtrado]') as HTMLInputElement | null;
        x!.value = '';
        const e: Event = <Event><any>{
          target: {
              value: ''
          }
        };
        this.applyFilter(e);
      } else {
        this.snackBarService.openSnackBar("Ocurrió un error al actualizar el programa.", "default");
      }
    });
  }

  applyFilter(event: Event) {

    const filterValue = (event.target as HTMLInputElement).value;

    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}

import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { Campus, Nivel, Sede, UsuarioAdmin } from 'src/app/interfaces/UsuarioAdmin';
import { RoportesService } from 'src/app/services/reportes.service';
import { UsuarioAdminService } from 'src/app/services/usuario-admin.service';
import { environment } from 'src/environments/environment';
import { saveAs } from 'file-saver-es';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { CampusNomina, NivelesNomina } from 'src/app/interfaces/PermisosNomina';
@Component({
  selector: 'app-reporteCompleto-admin',
  templateUrl: './reporteCompleto-admin.component.html',
  styleUrls: ['./reporteCompleto-admin.component.css']
})
export class ReporteCompletoAdminComponent implements OnInit {
  disableSabana = false;
  disableEstimadoGraducion = false;
  form!: FormGroup;
  selectedValuesCampus: Campus[];
  selectedValuesSedes: Sede[];
  selectedValuesNiveles: any[];
  campusList: Campus[] = [];
  filtros: UsuarioAdmin;
  campusPermiso: CampusNomina[];
  nivelPermiso: NivelesNomina[];
  campusFilter: any[] = [];
  sedesList: Sede[] = [];
  sedesFiltradas: Sede[] = [];
  nivelList:any; 
  @ViewChild('multiSelect') multiSelect: MatSelect;
  @ViewChild('multiSelectSedes') multiSelectSedes: MatSelect;
  @ViewChild('multiSelectNiveles') multiSelectNiveles: MatSelect;
  todosCampus = false;
  todosSedes = false;
  todosNiveles = false;
  campus: Campus[] = [];
  sedes: Sede[] = [];
  niveles: Nivel[] = [];
  nombreExcel: string;
 
  constructor( private fb: FormBuilder,private usuarioAdminService: UsuarioAdminService, private permisosService: PermisosNominaService, private reporteService: RoportesService) { }

  private urlPM = environment.baseUrl;

  ngOnInit(): void {
    this.form = this.fb.group({
      campus: new FormControl(),
      sedes: new FormControl(),
      niveles: new FormControl(),
      updateOn: 'blur'
    });
    this.campusPermiso = this.permisosService.obtenCampus();
    this.nivelList = this.permisosService.obtenNiveles();

    this.campusList = this.campusPermiso;
    this.campusFilter = this.campusPermiso;
    
      
      const array:  Sede[] = [];
      for(let i= 0; i <= this.campusPermiso.length -1; i++){
        for(let j= 0; j <= this.campusPermiso[i].sedes.length -1; j++){
          let sede: Sede = { claveCampus: this.campusPermiso[i].claveCampus, claveSede: this.campusPermiso[i].sedes[j].claveSede, descripcion : this.campusPermiso[i].sedes[j].descripcion};
          array.push(sede);
        }
      } 
      
      this.sedesFiltradas = this.sort(array , 'descripcion');
      this.sedesList = this.sedesFiltradas;
  }

  descargarSabana(): void {
    this.disableSabana = true;
    this.setCampus();
    this.setSedes();
    this.setNiveles();
    this.filtros = { listCampus: this.campus, sedes: this.sedes, niveles: this.niveles, nombre: '', nomina: '', correo: '' };
    this.reporteService.descargarReporteCompletoSabana(this.filtros).subscribe(archivo => {
      this.nombreExcel = "Sabana.xlsx";
      saveAs(archivo, this.nombreExcel);
      this.disableSabana = false;
    });
}

  descargarReporteEstimadoDeGraduacion(): void {
    this.disableEstimadoGraducion = true;
    this.setCampus();
    this.setSedes();
    this.setNiveles();
    this.filtros = { listCampus: this.campus,sedes: this.sedes, niveles:  this.niveles, nombre: '', nomina: '',correo: ''};
    this.reporteService.descargarEstimadoGraduacion(this.filtros).subscribe(archivo =>{
      this.nombreExcel = "Reporte Estimado de Graduacion.xlsx";
      saveAs(archivo, this.nombreExcel);
       this.disableEstimadoGraducion = false;
    });
  }

  setCampus(): void {
    if (this.form.value.campus) {
      for (let i = 0; i < this.form.value.campus.length; i++) {
        if (this.form.value.campus[i] != '0') {
          this.campus.push({ claveCampus: this.form.value.campus[i], descripcion: '' });
        }
      }
    } else {
      for (let i = 0; i <= this.campusList.length - 1; i++) {
        this.campus.push({ claveCampus: this.campusList[i].claveCampus, descripcion: '' });
      }
    }
  }

  setSedes(): void {
    if (this.form.value.sedes) {
      for (let i = 0; i < this.form.value.sedes.length; i++) {
        if (this.form.value.sedes[i] != '0') {
          let claveCampus = this.sedesList.find(x => x.claveSede == this.form.value.sedes[i] && x.claveSede !== '0')?.claveCampus;
          this.sedes.push({ claveSede: this.form.value.sedes[i], claveCampus: claveCampus, descripcion: '' });
        }
      }
    } else {
      for (let i = 0; i <= this.sedesFiltradas.length - 1; i++) {
        this.sedes.push({ claveCampus: this.sedesFiltradas[i].claveCampus, claveSede: this.sedesFiltradas[i].claveSede, descripcion: '' });
      }
    }
  }

  setNiveles(): void {
    if (this.form.value.niveles) {
      for (let i = 0; i < this.form.value.niveles.length; i++) {
        if (this.form.value.niveles[i] != '0') {
          this.niveles.push({ claveNivel: this.form.value.niveles[i], descripcion: '' });
        }
      }
    } else {
      for (let i = 0; i <= this.nivelList.length - 1; i++) {
        this.niveles.push({ claveNivel: this.nivelList[i].claveNivel, descripcion: '' });
      }
    }

  }

  onChangeCampus(): void{
    let array:  Sede[] = [];
    for(let i= 0; i <= this.selectedValuesCampus.length -1; i++){
       let sede = this.sedesList.filter(x => x.claveCampus == this.selectedValuesCampus[i] as unknown);
       Array.prototype.push.apply(array,sede);
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

  marcarTodos(combo:string): void {
    switch (combo) {
      case 'multiSelect': {
        this.todosCampus = !this.todosCampus;
        this.multiSelect.options.forEach( (item : MatOption) => this.todosCampus ? item.select() : item.deselect());
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
}

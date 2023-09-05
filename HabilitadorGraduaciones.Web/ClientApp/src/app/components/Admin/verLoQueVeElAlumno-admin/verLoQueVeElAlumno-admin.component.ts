import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { UsuarioService } from 'src/app/services/usuario.service';

@Component({
  selector: 'app-verLoQueVeElAlumno-admin',
  templateUrl: './verLoQueVeElAlumno-admin.component.html',
  styleUrls: ['./verLoQueVeElAlumno-admin.component.css']
})
export class VerLoQueVeElAlumnoAdminComponent {
  busqueda = '';
  isMatriculaString = '1';
  isSearching = false;
  matricula = '';
  BusquedaEncontrada: any[] = [];
  isLoading = false;
  constructor(private pnService: PermisosNominaService, private sUsuario: UsuarioService, private spinner: NgxSpinnerService) { }

  
  _buscar(busqueda: string, isMatriculaString: string): void{
    this.BusquedaEncontrada = [];
    let isMatricula
    if (isMatriculaString === '1') {
      isMatricula = true;
    } else {
      isMatricula = false;
    }
    this.isLoading = false;
    this.spinner.show();
    let data = { Busqueda: busqueda, IsMatricula: isMatricula , IdUsuario : this.pnService.obtenIdUsuario()};
    
    this.sUsuario.obtenerUsuarioPorMatriculaONombre(data).subscribe((r: any) => {
      if (r) {
        this.BusquedaEncontrada = r
      }
      this.spinner.hide();
      this.isLoading = true;
      this.isSearching = false;
    },(err: HttpErrorResponse)=>{
      this.spinner.hide();
      this.isLoading = true;
      console.error(err);
    });
  }
  alumnoSeleccionado( _matricula: string): void {
      this.matricula = _matricula;
      this.isSearching = true;
      this.isLoading = false;
  }
  limpiarBusqueda() : void{
    this.busqueda = '';
    this.isLoading = false;
  }
}

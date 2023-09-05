import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Acceso, UserClaims, Usuario } from '../interfaces/Usuario';
import { TranslateService } from '@ngx-translate/core';
import { Campus, Sede, UsuarioAdmin } from '../interfaces/UsuarioAdmin';
@Injectable({
  providedIn: 'root'
})
export class UsuarioAdminService {

  usuario!: Usuario;
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  constructor(private http: HttpClient, public translate: TranslateService) { }
  baseURL: string = environment.baseUrl;
  controlador: string = "usuario";


  obtenerUsuario(idUsuario:number): Observable<UsuarioAdmin>{
    return this.http.get<UsuarioAdmin>(this.baseURL + '/api/usuario/ObtenerUsuarioAdminsitrador/' + idUsuario, this.config);
  }

  obtenerUsuarios(): Observable<UsuarioAdmin[]>{
    return this.http.get<UsuarioAdmin[]>(this.baseURL + '/api/usuario/ObtenerUsuarios/' , this.config);
  }

  guardarUsuario(data: UsuarioAdmin) {
    return this.http.post(this.baseURL + '/api/Usuario/GuardarUsuario' , data, this.config);
  }

  eliminarUsuario(idUsuario: number, usuarioElimino: string) {
    let data = {IdUsuario: idUsuario, UsuarioModificacion: usuarioElimino};
    return this.http.post(this.baseURL + '/api/Usuario/EliminarUsuario' , data, this.config);
  }

  obtenerUsuarioPorNomina(nomina:any): Observable<UsuarioAdmin[]>{
    return this.http.get<UsuarioAdmin[]>(this.baseURL + '/api/usuario/ObtenerUsuarioNombrePorNomina/' + nomina, this.config);    
  }

  obtenerCampus(): Observable<Campus[]>{
    return this.http.get<Campus[]>(this.baseURL + '/api/usuario/ObtenerCampus', this.config);    
  }

  obtenerSedes(): Observable<Sede[]>{
    return this.http.get<Sede[]>(this.baseURL + '/api/usuario/ObtenerSedes/', this.config);    
  }

  obtenerNiveles(){
    return this.http.get(this.baseURL + '/api/filtros/1' , this.config);
  }

  obtenerHistorialUsuario(idUsuario:number): Observable<UsuarioAdmin[]>{
    return this.http.get<UsuarioAdmin[]>(this.baseURL + '/api/usuario/ObtenerHistorialUsuario/' + idUsuario, this.config);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Acceso, UserClaims, Usuario } from '../interfaces/Usuario';
import { TranslateService } from '@ngx-translate/core';
@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  usuario!: Usuario;
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };
  configArchivo = {
    headers: {
      'Access-Control-Allow-Origin': '*',
    }
  };
  constructor(private http: HttpClient, public translate: TranslateService) { }
  baseURL: string = environment.baseUrl;
  controlador: string = "usuario";


  obtenerUsuario(matricula:string): Observable<Usuario>{
    var datos = {id:matricula};
    return this.http.get<Usuario>(this.baseURL + '/api/usuario/' + matricula, this.config);
    
  }

  obtenerUsuarioPorMatriculaONombre(busqueda:any){
    return this.http.post(this.baseURL + '/api/usuario/busqueda', busqueda, this.config);    
  }

  setLenguaje(lang:string){
    this.translate.use(lang);
  }

  getUserClaims(): any {
    return this.http.get<UserClaims>(this.baseURL + '/User/GetUserClaims/', this.config);
  }

  getAcceso(matricula: string) {
    return this.http.get<Acceso>(this.baseURL + '/api/Acceso/' + matricula, this.config);
  }

  guardarCargaArchivo(file: File, usuarioAplicacion: string){
    const formData = new FormData();
    formData.append("ArchivoRecibido", file);
    formData.append("UsuarioApplicacion", usuarioAplicacion);

    return this.http.post(this.baseURL + '/api/usuario/GuardarCargaArchivo', formData, this.configArchivo);
  }
}

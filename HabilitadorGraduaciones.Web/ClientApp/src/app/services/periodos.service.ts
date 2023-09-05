import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Periodo } from '../interfaces/Periodos';
import { Endpoints } from '../interfaces/Endpoints';
import { Usuario } from '../classes';

@Injectable({
  providedIn: 'root'
})
export class PeriodosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };
  configPost = {
    headers: {
      'Content-Type': 'application/json',
    }
  };
  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getPeriodos(data: Endpoints){
    return this.http.post<any>(this.urlPM + '/api/Periodos/GetPeriodos/', data, this.config);
  }

  getPeriodoAlumno(matricula:string){
    return this.http.get(this.urlPM + '/api/Periodos/GetPeriodoAlumno/' + matricula ,this.config);
  }

  getPronosticoAlumno(data: Endpoints){
    return this.http.post(this.urlPM + '/api/Periodos/GetPeriodoPronostisco/' , data, this.config);
  }

  guardarPeriodoAlumno(periodo: Periodo){
    return this.http.post(this.urlPM + '/api/Periodos/GuardarPeriodo', periodo, this.config);
  }
  enviarCorreo(correo : Usuario){
    return this.http.post(this.urlPM + '/api/Periodos/EnviarCorreo',correo, this.config).subscribe((sesult: any ) =>{
    });
  }
}

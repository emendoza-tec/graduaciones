import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LogService {
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

  guardarLog(matricula:string, periodo: string, periodoId: string){
    const data = {PeriodoId: periodoId, Periodo: periodo, Matricula : matricula};
    return this.http.post(this.urlPM + '/api/Log/GuardarLog', data, this.config);
  }
}

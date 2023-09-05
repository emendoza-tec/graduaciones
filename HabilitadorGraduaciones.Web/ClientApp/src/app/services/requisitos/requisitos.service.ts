import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Endpoints } from '../../interfaces/Endpoints';
import { Links } from '../../interfaces/Links';

@Injectable({
  providedIn: 'root'
})
export class RequisitosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getTarjeta(id: number, idioma: string) {
    return this.http.get(this.urlPM + '/api/Tarjeta/' + id + '/' + idioma, this.config);
  }

  getExamenIntegradorByMatricula(id: string) {
    return this.http.get(this.urlPM + '/api/ExamenIntegrador/' + id, this.config);
  }

  getCeneval(matricula: string) {
    return this.http.get(this.urlPM + '/api/Ceneval/' + matricula, this.config);
  }

  getProgramaBGB(data: Endpoints){
    return this.http.post(this.urlPM + '/api/ProgramaBGB/', data, this.config);
  }

  getLinks() {
    return this.http.get<Links>(this.urlPM + '/api/Links/', this.config);
  }

  getExamenConocimientos(data: Endpoints) {
    return this.http.post(this.urlPM + '/api/ExamenConocimientos/GetExamenConocimiento/', data, this.config);
  }

  getExamenConocimientoPorLenguaje(id: number, idioma: string) {
    return this.http.get(this.urlPM + '/api/ExamenConocimientos/' + id + '/' + idioma, this.config);    
  }
}

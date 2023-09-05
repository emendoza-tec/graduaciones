import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Calendario } from '../interfaces/Calendario';

@Injectable({
  providedIn: 'root'
})

export class CalendariosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getCalendarioAlumno(id:string) {
    return this.http.get(this.urlPM+ '/api/calendarios/'+ id, this.config);
  }

  getCalendarios() {
    return this.http.get(this.urlPM + '/api/calendarios/GetCalendarios', this.config);
  }

  guardarConfiguracionCalendarios(configuracionCalendario: Calendario[]) {
    return this.http.post(this.urlPM + '/api/calendarios/ModificarCalendarios', configuracionCalendario, this.config);
  }

}

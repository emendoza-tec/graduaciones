import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CampusCeremoniaGraduacion } from '../classes/CampusCeremoniaGraduacion';

@Injectable({
  providedIn: 'root'
})
export class CampusCeremoniaGraduacionService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
  }
  guardaCampusSeleccionado(data: CampusCeremoniaGraduacion) {
    return this.http.post(this.urlPM + '/api/CampusCeremoniaGraduacion', data,
      this.config);
  }

}

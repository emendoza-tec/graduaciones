import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Endpoints } from '../../interfaces/Endpoints';

@Injectable({
  providedIn: 'root'
})

export class PlanDeEstudiosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getPlanDeEstudios(data: Endpoints) {
    return this.http.post(this.urlPM + '/api/PlanDeEstudios/ConsultaPlanDeEstudios/', data, this.config);
  }

  getDetallePlanDeEstudios(idTarjetaPE: number) {
    return this.http.get(this.urlPM + '/api/PlanDeEstudios/DetallePlanDeEstudios/' + idTarjetaPE, this.config);
  }
  
}

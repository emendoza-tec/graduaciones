import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Endpoints } from '../interfaces/Endpoints';

@Injectable({
  providedIn: 'root'
})
export class DistincionesService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
    
   }

  getDistinciones(data: Endpoints) {
    return this.http.post(this.urlPM + '/api/Distinciones/', data, this.config);
  }
}

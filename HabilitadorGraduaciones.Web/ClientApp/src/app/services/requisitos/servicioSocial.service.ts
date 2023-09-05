import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Endpoints } from 'src/app/interfaces/Endpoints';

@Injectable({
  providedIn: 'root'
})
export class ServicioSocialService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getServicioSocial(data: Endpoints) {
    return this.http.post(this.urlPM + '/api/ServicioSocial/', data, this.config);
  }
}

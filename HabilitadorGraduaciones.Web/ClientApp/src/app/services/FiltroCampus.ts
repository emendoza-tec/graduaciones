import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FiltroCampusService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };
  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getFiltroCampus() {
    return this.http.get<any>(this.urlPM + '/api/Filtros', this.config);
  }

}

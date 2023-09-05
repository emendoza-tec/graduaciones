import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class SemanasTecService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
    
   }

  getSemanasTec(id: string) {
    return this.http.get(this.urlPM + '/api/semanastec/' + id, this.config);
  }

  getSemasTecExt(id: string){
    return this.http.get(this.urlPM+ '/api/semanastec/ext/' + id, this.config);
  }
}

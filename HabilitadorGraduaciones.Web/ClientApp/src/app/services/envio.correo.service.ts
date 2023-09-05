import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Correo } from '../interfaces/Correo';

@Injectable({
  providedIn: 'root'
})
export class EnvioCorreoService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  enviarCorreo(correo : Correo){
    return this.http.post(this.urlPM + '/api/Notificaciones/EnviarCorreo',correo, this.config).subscribe((sesult: any ) =>{
    });
  }
}

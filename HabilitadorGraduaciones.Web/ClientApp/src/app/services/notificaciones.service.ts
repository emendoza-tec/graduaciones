import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { Notificaciones } from 'src/app/interfaces/Notificaciones';
import { DatosPersonalesCorreo } from '../classes';
@Injectable({
  providedIn: 'root'
})

export class NotificacionesService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json; charset=UTF-8',
    }
  };
  configPost = {
    headers: {
      'Content-Type': 'application/json; charset=UTF-8',
    }
  };
  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  getNotificaciones(isConsultaNoLeidas: boolean, matricula: string){
    return this.http.get(this.urlPM + '/api/Notificaciones/GetNotificaciones/'+ isConsultaNoLeidas + '/' + matricula,this.config);
  }

  marcarComoLeida(id: number, isModificarNotificacion: boolean,  isNotificacion: boolean, matricula: string){
    const data = { Id: id, IsModificarNotificacion: isModificarNotificacion , IsNotificacion: isNotificacion, Matricula: matricula };
    return this.http.post(this.urlPM + '/api/Notificaciones/ActualizarEstatus', data, this.configPost);
  }

  marcarTodasComoLeidas(notificaciones: Notificaciones[]){
    return this.http.post(this.urlPM + '/api/Notificaciones/MarcarTodasLeidas', notificaciones, this.config);
  }

  guardarNotificacion(titulo: string, descripcion: string, matricula: string){
    const data = { Titulo: titulo, Descripcion: descripcion, Matricula: matricula };
    return this.http.post(this.urlPM + '/api/Notificaciones/InsertarNotificacion', data, this.config);
  }

  guardarNotificacionCorreo(tipoCorreo: number, matricula: string){
    const data = { TipoCorreo: tipoCorreo, Matricula: matricula };
    return this.http.post(this.urlPM + '/api/Notificaciones/InsertarNotificacionCorreo', data, this.config);
  }

  bienvenidoGraduacion(matricula: string, tipoCorreo: number) {
    return this.http.get(this.urlPM + '/api/Notificaciones/BienvenidoGraduacion/' + matricula + '/' + tipoCorreo, this.config);
  }

  isCorreoEnviado(tipo:number, matricula: string) {
    return this.http.get<any>(this.urlPM + '/api/Notificaciones/IsCorreoEnviado/' + tipo + '/' + matricula, this.config);
  }

  enteradoCreditosNoAlcanzan(matricula: string) {
    return this.http.get(this.urlPM + '/api/Notificaciones/EnteradoCreditosInsuficientes/' + matricula, this.config);
  }

  envioCorreoConfirmacion(data: DatosPersonalesCorreo) {
    return this.http.post(this.urlPM + '/api/Notificaciones/EnviaCorreoConfirmacion', data, this.config);
  }
}

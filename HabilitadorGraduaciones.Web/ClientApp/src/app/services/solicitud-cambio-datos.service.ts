import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  DetalleSolicitud, EstatusSolicitud, GuardaSolicitud, ModificaSolicitud, Solicitud,
  TotalSolicitudesDto
} from '../classes/SolicitudCambioDatos';

@Injectable({
  providedIn: 'root'
})
export class SolicitudCambioDatosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
  }

  getEstatusSolicitudes() {
    return this.http.get(this.urlPM + '/api/SolicitudDeCambioDeDatos/GetEstatusSolicitudes/',
        this.config);
  }

  getPendientes(id: number) {
    return this.http.get(this.urlPM + '/api/SolicitudDeCambioDeDatos/GetPendientes/' + id,
      this.config);
  }

  getSolicitudes(idEstatusSolicitud: number, idUsuario: number) {
    return this.http.get(this.urlPM + '/api/SolicitudDeCambioDeDatos/' + idEstatusSolicitud + '/' + idUsuario,
      this.config);
  }

  getDetalle(id: number) {
    return this.http.get(this.urlPM + '/api/SolicitudDeCambioDeDatos/GetDetalle/' + id,
      this.config);
  }

  guardaSolicitudes(data: GuardaSolicitud[]) {
    return this.http.post(this.urlPM + '/api/SolicitudDeCambioDeDatos/', data,
      this.config);
  }

  modificaSolicitud(data: any) {
    return this.http.post(this.urlPM + '/api/SolicitudDeCambioDeDatos/ModificaSolicitud/', data,
      this.config);
  }

  getConteoPendientes(id: number) {
    return this.http.get<TotalSolicitudesDto>(this.urlPM + '/api/SolicitudDeCambioDeDatos/GetConteoPendientes/'+ id,
      this.config);
  }

  downloadFile(file: string): Observable<Blob> {
    return this.http.get(this.urlPM + '/api/SolicitudDeCambioDeDatos/DownloadFile?fileName=' + file,
      { responseType: 'blob' });
  }
}

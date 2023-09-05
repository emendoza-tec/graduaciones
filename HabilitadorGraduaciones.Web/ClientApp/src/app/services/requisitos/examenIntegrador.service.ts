import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExamenIntegradorService {

  configArchivo = {
    headers: {
      'Access-Control-Allow-Origin': '*',
    }
  };

  descargaArchivo = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Accept': 'application/octet-stream',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {

  }

  procesaExcel(file: File, id: number) {
    const formData = new FormData();
    formData.append("archivo", file);
    formData.append("idUsuario", id.toString());

    return this.http.post(this.urlPM + '/api/ExamenIntegrador/procesaExamenesIntegrador/' + id, formData, this.configArchivo);
  }

  guardaExamenesIntegrador(file: File, usuarioAplicacion: string, idUsuario: number) {
    const formData = new FormData();
    formData.append("ArchivoRecibido", file);
    formData.append("UsuarioApplicacion", usuarioAplicacion);

    return this.http.post(this.urlPM + '/api/ExamenIntegrador/guardaExamenesIntegrador/' + idUsuario, formData, this.configArchivo);
  }
}

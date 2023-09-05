import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExpedienteService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

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

  getByAlumno(id: string){
    return this.http.get(this.urlPM + '/api/expediente/' + id, this.config);
  }

  getDetalle(id:number){
    return this.http.get(this.urlPM+ '/api/expediente/detalle/'+ id, this.config);
  }

  getComentarios(id:string){
    return this.http.get(this.urlPM+ '/api/expediente/ConsultarComentarios/'+ id, this.config);
  }

  procesaExcel(file: File, id:number){
    const formData = new FormData();
    formData.append("archivo", file);

    return this.http.post(this.urlPM + '/api/expediente/procesaExpedientes/'+ id, formData, this.configArchivo);
  }

  guardaExpedientes(file: File, usuarioAplicacion: string, idUsuario: number){
    const formData = new FormData();
    formData.append("ArchivoRecibido", file);
    formData.append("UsuarioApplicacion", usuarioAplicacion);

    return this.http.post(this.urlPM + '/api/expediente/guardaExpedientes/'+ idUsuario, formData, this.configArchivo);
  }
}

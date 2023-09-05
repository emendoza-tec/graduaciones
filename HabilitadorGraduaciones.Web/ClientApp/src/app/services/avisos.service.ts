import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AvisosService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json; charset=UTF-8',
    }
  };

  configImg = {
    headers: {
      'Access-Control-Allow-Origin': '*',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
    
   }

  getAvisos(id: string){
    return this.http.get(this.urlPM + '/api/avisos/' + id, this.config);
  }

  getAvisosHistorial(id:string){
    return this.http.get(this.urlPM+ '/api/avisos/historial/'+ id, this.config);
  }

  getFiltros(){
    return this.http.get(this.urlPM + '/api/avisos/filtros', this.config);
  }

  guardarAviso(form:string){
    let formData = JSON.parse(form);
    formData['matricula'] = formData['matricula'].toString();
    return this.http.post(this.urlPM + '/api/avisos/', formData, this.config);
  }

  subirImagen(image:File){
    const formData = new FormData();
    formData.append('Image', image);
    return new Promise(
      (
        resolve: any,
        reject: any
      ) => {
        this.http.post(this.urlPM + '/api/avisos/GuardarArchivo/', formData, this.configImg).subscribe(data =>{
          resolve(data);
        },
        err=>{
          reject(err);
        })
      }
    );
    
  }

  getFiltroMatricula(filtros:string){
    let filtrosMatricula = JSON.parse(filtros);
    return this.http.post(this.urlPM + '/api/avisos/filtroMatriculas', filtrosMatricula, this.config);
  }
}


import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ConfiguracionNivelIngles } from '../../interfaces/NivelIngles';


@Injectable({
  providedIn: 'root'
})

export class NivelInglesService {
  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  private urlPM = environment.baseUrl;

  constructor(private http: HttpClient) {
    
   }

 getAlumnoNivelIngles(id: string){
    return this.http.get(this.urlPM+ '/api/nivelingles/'+ id, this.config);
  }

  getDetalleNivelIngles(id:number){
    return this.http.get(this.urlPM+ '/api/nivelIngles/' + id, this.config);
  }

  getProgramas(){
    return this.http.get(this.urlPM + '/api/nivelIngles/GetProgramas', this.config);
  }

  guardarConfiguracionNivelIngles(configuracionIngles:ConfiguracionNivelIngles[]){
    return this.http.post(this.urlPM + '/api/nivelIngles/ModificarNivelIngles',configuracionIngles, this.config);
  }

}

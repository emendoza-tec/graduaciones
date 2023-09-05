import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { UsuarioAdmin } from '../interfaces/UsuarioAdmin';
@Injectable({
    providedIn: 'root'
})
export class RoportesService {

    config = {
        headers: {
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'application/json',
        }
    };

    constructor(private http: HttpClient, public translate: TranslateService) { }
    baseURL: string = environment.baseUrl;

    descargarEstimadoGraduacion(data: UsuarioAdmin) {
        return this.http.post(this.baseURL + '/api/ReporteEstimadoDeGraduacion/Descargar', data, { responseType: 'blob' });
  }

  descargarReporteCompletoSabana(data: UsuarioAdmin) {
    return this.http.post(this.baseURL + '/api/Sabana/DescargarReporteSabana',data, { responseType: 'blob' });
  }

}

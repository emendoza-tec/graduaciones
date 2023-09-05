import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { Permisos, Roles, SeccionesPermisos } from '../classes/Roles';
import { Observable } from 'rxjs';
@Injectable({
    providedIn: 'root'
})
export class RolesService {

    config = {
        headers: {
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'application/json',
        }
    };

    constructor(private http: HttpClient, public translate: TranslateService) { }
    baseURL: string = environment.baseUrl;

    obtenerRoles(): Observable<Roles[]> {
        return this.http.get<Roles[]>(this.baseURL + '/api/roles', this.config);
    }

    obtenerSecciones(): Observable<Permisos[]> {
        return this.http.get<Permisos[]>(this.baseURL + '/api/roles/ObtenerSecciones', this.config);
    }

    obtenerRolesPorId(id: number): Observable<Roles> {
        return this.http.get<Roles>(this.baseURL + '/api/roles/' + id, this.config);
    }

    guardaRol(rol: Roles) {
        return this.http.post(this.baseURL + '/api/roles/GuardaRol', rol, this.config);
    }

    eliminarRol(rol: Roles){
        return this.http.post(this.baseURL + '/api/roles/EliminaRol', rol, this.config);
    }

    modificaRol(rol:Roles){
        return this.http.post(this.baseURL + '/api/roles/ModificaRol', rol, this.config);
    }

    cambiaEstatusRol(rol:Roles){
        return this.http.post(this.baseURL + '/api/roles/CambiaEstatusRol', rol, this.config);
    }

    obtenerDescripcionRoles(): Observable<Roles[]> {
        return this.http.get<Roles[]>(this.baseURL + '/api/roles/ObtenerDescripcionRoles', this.config);
    }
}

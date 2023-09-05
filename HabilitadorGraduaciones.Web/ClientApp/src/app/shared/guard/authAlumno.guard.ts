import { Injectable } from "@angular/core";
import { CanActivate, Router, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { ModeloInformacionUsuario } from "../models/usuario.module";

@Injectable({ providedIn: 'root' })
export class AuthAlumnoGuard implements CanActivate {

    constructor(
        private router: Router
    ) {
    }

    canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        const matricula = this.obtenMatricula();
        if (matricula.startsWith('A')) {
            return true;
        } else {
            this.router.navigate(['/']);
            return false;
        }
    }

    canActivateChild(
    ) {
        const matricula = this.obtenMatricula();
        if (matricula.startsWith('A')) {
            return true;
        } else {
            this.router.navigate(['/']);
            return false;
        }
    }

    obtenMatricula(): string {
        const sesion = sessionStorage.getItem("userInformation");
        if (sesion) {
            let modeloInformacion: ModeloInformacionUsuario = JSON.parse(sesion);
            return modeloInformacion.matricula;
        } else {
            return '';
        }
    }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';
import { CampusNomina, NivelesNomina, PermisosMenu, PermisosNomina } from '../interfaces/PermisosNomina';
@Injectable({
  providedIn: 'root'
})
export class PermisosNominaService {

  config = {
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
    }
  };

  constructor(private http: HttpClient) { }
  baseURL: string = environment.baseUrl;

  obtenerPermisosPorNomina(nomina: string): Observable<PermisosNomina> {
    return this.http.get<PermisosNomina>(this.baseURL + '/api/PermisosNomina/' + nomina, this.config).pipe(
      map((data) => {
        return {
          idUsuario: data.idUsuario, nomina: data.nomina, ambiente: data.ambiente, acceso: data.acceso, niveles: data.niveles,
          campus: data.campus, menu: data.menu, errorMessage: data.errorMessage, result: data.result
        }
      })
    );
  }

  obtenNomina(): string {
    const informacionColaborador: PermisosNomina = this.desencriptaSesion();
    if (informacionColaborador) {
      return informacionColaborador.nomina;
    } else {
      return '';
    }
  }

  obtenIdUsuario(): number {
    const informacionColaborador: PermisosNomina = this.desencriptaSesion();
    if (informacionColaborador) {
      return informacionColaborador.idUsuario;
    } else {
      return 0;
    }
  }

  obtenCampus(): CampusNomina[] {
    const informacionColaborador: PermisosNomina = this.desencriptaSesion();
    if (informacionColaborador) {
      return informacionColaborador.campus;
    } else {
      return [];
    }
  }

  obtenMenu(): PermisosMenu[] {
    const informacionColaborador: PermisosNomina = this.desencriptaSesion();
    if (informacionColaborador) {
      return informacionColaborador.menu;
    } else {
      return [];
    }
  }

  obtenNiveles(): NivelesNomina[] {
    const informacionColaborador: PermisosNomina = this.desencriptaSesion();
    if (informacionColaborador) {
      return informacionColaborador.niveles;
    } else {
      return [];
    }
  }

  revisaSeccionesPermisoPorId(idMenu: number, idSubMenu: number) {
    const permisosMenu: PermisosMenu[] = this.obtenMenu();
    if (permisosMenu) {
      return permisosMenu.find(f => f.idMenu == idMenu && f.idSubMenu == idSubMenu && f.seccion);
    } else {
      return;
    }
  }


  revisarSubmenuPermisoPorId(idMenu: number, idSubMenu: number): PermisosMenu {
    const permisosMenu: PermisosMenu[] = this.obtenMenu();
    const permiso: PermisosMenu = {} as PermisosMenu;

    if (permisosMenu) {
      return permisosMenu.find(permiso => { return permiso.idMenu == idMenu && permiso.idSubMenu == idSubMenu && !permiso.seccion; }) as PermisosMenu;
    } else {
      return permiso;
    }
  }

  obtenSubmenuPermisoPorNombre(nombre: string): PermisosMenu {
    const obtenMenu = this.obtenMenu();
    const permiso: PermisosMenu = {} as PermisosMenu;

    if (obtenMenu) {
      return obtenMenu.find(f => f.nombreSubMenu.toUpperCase() == nombre.toUpperCase()) as PermisosMenu;
    } else {
      return permiso;
    }
  }
  revisaPermisoPorPath(pathInArray: string[]): PermisosMenu[] {
    const permisosMenu: PermisosMenu[] = this.obtenMenu();
    const permisosPath: PermisosMenu[] = [];

    if (permisosMenu) {
      permisosMenu.forEach(permiso => {
        pathInArray.forEach(path => {
          if (permiso.pathMenu == path || permiso.pathSubMenu == path) {
            permisosPath.push(permiso);
          }
        });
      });
      return permisosPath;
    } else {
      return [];
    }
  }

  desencriptaSesion(): PermisosNomina {
    const sessionInformacionUsuario = sessionStorage.getItem("colaboradorInformation");
    if (sessionInformacionUsuario) {
      return JSON.parse(window.atob(sessionInformacionUsuario));
    } else {
      return { idUsuario: 0, nomina: '', ambiente: '', acceso: false, niveles: [], campus: [], menu: [], errorMessage: '', result: false }
    }
  }
}

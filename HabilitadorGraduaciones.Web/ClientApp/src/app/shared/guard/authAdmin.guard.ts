import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { PermisosNominaService } from "src/app/services/permisosNomina.service";

@Injectable({ providedIn: 'root' })

export class AuthAdminGuard implements CanActivate {

  constructor(
    private router: Router,
    private pnService: PermisosNominaService
  ) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const nomina = this.pnService.obtenNomina();
    const url = state.url.split('/');
    const arrayPath = url.filter(Boolean);
    const permisos = this.pnService.revisaPermisoPorPath(arrayPath);

    if (nomina == '') {
      this.router.navigate(['/']);
      return false;
    }
    if(arrayPath.length == 1){
      return true;
    }
    if (nomina.startsWith('L')) {
      if (permisos.length > 0) {
        //revisar permisos ver o editar
        if (permisos.filter(f => f.editar || f.ver).length > 0) {
          return true;
        } else {
          //acceso denegado no tiene permisos
          this.router.navigate(['/unauthorized']);
          return false;
        }
      }
    }
    this.router.navigate(['/']);
    return false;
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    const nomina = this.pnService.obtenNomina();
    const url = state.url.split('/');
    const arrayPath = url.filter(Boolean);
    const permisos = this.pnService.revisaPermisoPorPath(arrayPath);

    if(arrayPath.length == 1){
      return true;
    }

    if(nomina == ''){
      this.router.navigate(['/']);
      return false;
    }
    if(arrayPath.length == 1){
      return true;
    }

    if (nomina.startsWith('L')) {
      if (permisos.length > 0) {
        //revisar permisos ver o editar
        if (permisos.filter(f => f.editar || f.ver).length > 0) {
          return true;
        } else {
          //acceso denegado no tiene permisos
          this.router.navigate(['/unauthorized']);
          return false;
        }
      }
    }
    this.router.navigate(['/']);
    return false;
  }
}




import { Injectable, isDevMode } from "@angular/core";
import { UsuarioService } from "./usuario.service";
import { ModeloInformacionUsuario } from "../shared/models/usuario.module";
import { AuthService } from "./auth-service.service";
import { SnackBarService } from "./snackBar.service";
import { PermisosNominaService } from "./permisosNomina.service";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class ConfiguracionService {

  constructor(private usuarioService: UsuarioService, private authService: AuthService,
    private snackBarService: SnackBarService, private pnService: PermisosNominaService, private router : Router) { }
  informacionUsuarioDummy: ModeloInformacionUsuario = {
    matricula: "A01734365",
    nombreCompleto: "Alejandro Tamez Galindo",
    correo: "altaga39332045@tec.mx",
    pidm: "410738",
    nomina: "A01653412",
  };
  //funcion que se usa al cargar la app
  load(): Promise<any> {
    return this.getUserInformation();
  }

  //Funcion
  getUserInformation = () => {
    return new Promise((resolve) => {
      if (isDevMode()) {
        sessionStorage.clear();
        if (this.informacionUsuarioDummy.matricula.startsWith('L')){
          this.pnService.obtenerPermisosPorNomina(this.informacionUsuarioDummy.matricula).subscribe((data: any) => {
            if (data.acceso && data.idUsuario > 0) {
              sessionStorage.setItem("userInformation", JSON.stringify(this.informacionUsuarioDummy));
              sessionStorage.setItem("colaboradorInformation", window.btoa(JSON.stringify(data)));
              resolve(data);
            }
            else {
              this.router.navigate(['/unauthorized']);
              resolve(data);
            }
          });
        } else {
          sessionStorage.setItem("userInformation", JSON.stringify(this.informacionUsuarioDummy));
          resolve(this.informacionUsuarioDummy);
        }


      } else {
        this.usuarioService.getUserClaims().subscribe((userClaims: any) => {
          if (userClaims.matricula != null) {
            if (userClaims.matricula.startsWith('L')) {
              this.pnService.obtenerPermisosPorNomina(userClaims.nomina).subscribe((data: any) => {
                if (data.acceso && data.idUsuario > 0 && data.menu.length > 0) {
                  sessionStorage.setItem("userInformation", JSON.stringify(userClaims));
                  sessionStorage.setItem("colaboradorInformation", window.btoa(JSON.stringify(data)));
                  resolve(data);
                }
                else {
                  this.router.navigate(['/unauthorized']);
                  resolve(data);
                }
              });
            } else {
              this.usuarioService.getAcceso(userClaims.nomina).subscribe((data: any) => {
                if (data.acceso) {
                  sessionStorage.setItem("userInformation", JSON.stringify(userClaims));
                  resolve(data);
                }
                else {
                  this.snackBarService.openSnackBar('No tiene permiso para acceder al portal Graduaciones', 'default');
                  this.authService.cerrarSesion();
                }
              });
            }
          }
          else {
            window.location.replace(`${window.location.origin}/Auth/Login`);
          }
        });
      }
    });
  };
}



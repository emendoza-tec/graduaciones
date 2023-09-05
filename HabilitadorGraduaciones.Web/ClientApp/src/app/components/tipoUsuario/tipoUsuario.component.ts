import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';

@Component({
    selector: 'app-tipoUsuario',
    templateUrl: './tipoUsuario.component.html'
})

export class TipoUsuarioComponent {

    constructor(public translate: TranslateService, private router: Router, private pnService: PermisosNominaService) {
      const informacionUsuario = JSON.parse(sessionStorage.getItem("userInformation") || '{}');
      if(Object.entries(informacionUsuario).length === 0){
        this.router.navigate(['/unauthorized']);
      }
      if (informacionUsuario && !informacionUsuario.matricula.startsWith('L')) {
            this.router.navigate(['/index']);
        } else if(this.pnService.obtenNomina()){
            this.router.navigate(['/admin']);
        }
    }
}

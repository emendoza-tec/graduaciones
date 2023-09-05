import { ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivationEnd, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-roles-index',
  templateUrl: './usuarios-index.component.html',
  styleUrls: ['./usuarios-index.component.css']
})
export class UsuariosIndexComponent {
  titulo: string = '';

  constructor(private router: Router, private title: Title, private cdr: ChangeDetectorRef,) {
    this.router.events.subscribe((event: any) => {
      if (event instanceof ActivationEnd) {
        const nuevoUsuario = this.router.url.indexOf('usuarios');
        const str = this.router.url.substring(nuevoUsuario + 9);
        if (str) {
          const slash = str.indexOf('/');
          if (slash !== -1) {
              this.titulo = 'Editar';
          } else {
              this.titulo = 'Agregar Usuario';
          }
        }else{
          this.titulo = '';
        }
      }
    });
  }
}

import { Component} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UsuarioService } from './services/usuario.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent {
  title = 'Habilitador Graduaciones';
  public activeLang = 'es';

  constructor(public translate: TranslateService, public usuarioService: UsuarioService) { }

  cambiarLenguaje(): void {
    if(this.activeLang == 'es'){
      this.activeLang = 'en';
    }
    else{
      this.activeLang = 'es';
    }
    this.usuarioService.setLenguaje(this.activeLang);
  }
}

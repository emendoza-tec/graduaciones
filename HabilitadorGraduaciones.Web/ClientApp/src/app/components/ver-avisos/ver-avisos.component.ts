import { Component, Inject, OnInit } from '@angular/core';
import { Usuario } from 'src/app/interfaces/Usuario';   
import { UsuarioService } from 'src/app/services/usuario.service';
import { TranslateService } from '@ngx-translate/core';
import { AvisosService } from 'src/app/services/avisos.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-ver-avisos',
  templateUrl: './ver-avisos.component.html',
  styleUrls: ['./ver-avisos.component.css'],
})
export class VerAvisosComponent implements OnInit {
 
  public r: any[] = [];
  public usuario: Usuario = <Usuario>{};

  public activeLang = 'es';
  matricula: string = '';

  parser = new DOMParser();

  avisos:any;

  constructor(public serviceUsuario: UsuarioService, private translate: TranslateService,
    public usuarioService: UsuarioService, private avisosService:AvisosService, private route:ActivatedRoute, 
    @Inject(DomSanitizer) private readonly sanitizer:DomSanitizer) {
    this.translate = serviceUsuario.translate;
  }

  ngOnInit() {

    this.translate.setDefaultLang(this.activeLang);
    this.route.queryParams.subscribe(params =>{
      this.matricula = params['matricula'];
    });
    this.serviceUsuario.obtenerUsuario(this.matricula).subscribe(data => {
      this.usuario = data
    });
  
    this.avisosService.getAvisosHistorial(this.matricula).subscribe((resp:any) => {
      resp.lstAvisos.forEach((aviso:any) => {
        aviso.texto = this.sanitizer.bypassSecurityTrustHtml(aviso.texto)
      });
      this.avisos = resp.lstAvisos;
    });
  }

  cambiarLenguaje(lang: string) {
    if(this.activeLang == 'es'){
      this.activeLang = 'en';
    }
    else{
      this.activeLang = 'es';
    }
    this.usuarioService.setLenguaje(this.activeLang);
  }
}

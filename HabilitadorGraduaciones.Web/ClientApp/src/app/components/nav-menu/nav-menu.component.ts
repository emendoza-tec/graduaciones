import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, HostListener, Inject, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Notificaciones } from 'src/app/interfaces/Notificaciones';
import { AvisosService } from 'src/app/services/avisos.service';
import { NotificacionesService } from 'src/app/services/notificaciones.service';
import { ModeloInformacionUsuario } from 'src/app/shared/models/usuario.module';
import { UsuarioService } from '../../services/usuario.service';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})


export class NavMenuComponent implements OnInit {

  isExpanded = false;
  @Output() dataupdateevent = new EventEmitter<string>();
  @Input() usuario: any;
  public activeLang = navigator.language.split('-')[0];
  movil: boolean = false;
  notificacionesCount: string = '';
  mostarModalAvisos: boolean = false;
  mostrarAvisos: boolean = false;
  isFocusInsideComponent: boolean = false;
  isComponentClicked: boolean = false;
  linkLogin = environment.urllogin;
  usuarioLogin: string = "";
  url: string = "";
  public Notificaciones: Notificaciones[] = [];
  public NotificacionesNoLeidas: Notificaciones[] = [];
  informacionUsuario: ModeloInformacionUsuario;

  @HostListener('click')
  clickInside(): void {
    this.isFocusInsideComponent = true;
    this.isComponentClicked = true;
  }

  @HostListener('document:click')
  clickOutside(): void {
    if (!this.isFocusInsideComponent && this.isComponentClicked && !this.isExpanded) {
      this.mostrarAvisos = false;
      this.mostarModalAvisos = false;
      this.isComponentClicked = false;
      this.isExpanded = false;
    }
    this.isFocusInsideComponent = false;
  }

  constructor(private responsive: BreakpointObserver, private avisosService: AvisosService, private translate: TranslateService, private router: Router, private notificacioones: NotificacionesService,
    public userService: UsuarioService) {

    this.informacionUsuario = JSON.parse(sessionStorage.getItem("userInformation") || '{}');
  }

  ngOnInit() {
    this.usuarioLogin = this.informacionUsuario.matricula;
    this.getNotificaciones();
    this.getNotificacionesSinLeer();
    this.isExpanded = false;
    this.responsive.observe([Breakpoints.XSmall, Breakpoints.HandsetLandscape, Breakpoints.TabletPortrait, Breakpoints.Handset, Breakpoints.HandsetLandscape,
    Breakpoints.Small, Breakpoints.Tablet, Breakpoints.Medium, Breakpoints.Web])
      .subscribe(result => {
        const breakpoints = result.breakpoints;
        this.movil = false;
        if (breakpoints[Breakpoints.XSmall] || breakpoints[Breakpoints.TabletPortrait] || breakpoints[Breakpoints.HandsetPortrait]
          || breakpoints[Breakpoints.Small] || breakpoints[Breakpoints.TabletPortrait]) {
          this.movil = true;
        }
      });
  }

  abrirModalAvisos(): void {
    if (!this.mostarModalAvisos) {
      this.mostarModalAvisos = true;
    } else {
      this.mostarModalAvisos = false;
    }
  }

  toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  cambiarLenguaje(): void {
    if (this.activeLang == 'es') {
      this.activeLang = 'en';
    }
    else {
      this.activeLang = 'es';
    }
  }

  verNotificaciones(): void {
    this.mostrarAvisos = !this.mostrarAvisos
  }

  getNotificaciones(): void {
    this.avisosService.getAvisos(this.usuarioLogin).subscribe((r: any) => {
      if (r.result) {
        r.lstAvisos.forEach((a: { id: any; titulo: any; texto: any; fechaCreacion: any; activo: any; }) => {
          const _notificacion: Notificaciones = {
            id: a.id,
            titulo: a.titulo,
            texto: a.texto,
            fechaCreacion: a.fechaCreacion,
            urlImage: '',
            tiempo: new Date(),
            activo: a.activo,
            isNotificacion: false,
            matricula: this.usuarioLogin,
            aviso: true
          };
          this.Notificaciones.push(_notificacion);
        });
        this.translate.onLangChange.subscribe((event: LangChangeEvent) => {
          this.cambiarLenguaje();
          this.getDiffTime();
        });
        this.getDiffTime();
      }
      this.notificacioones.getNotificaciones(false, this.usuarioLogin).subscribe((r: any) => {
        if (r.result) {
          r.listaNotificaciones.forEach((a: { id: any; titulo: any; descripcion: any; fechaRegistro: any; activo: any; }) => {
            const _notificacion: Notificaciones = {
              id: a.id,
              titulo: a.titulo,
              texto: a.descripcion,
              fechaCreacion: a.fechaRegistro,
              urlImage: '',
              tiempo: new Date(),
              activo: a.activo,
              isNotificacion: true,
              matricula: this.usuarioLogin,
              aviso: false
            };
            this.Notificaciones.push(_notificacion);
          })
          let date: Date = new Date();
          for (let element of this.Notificaciones) {
            const fecha: Date = new Date(element.fechaCreacion);
            element.tiempo = fecha;
            element.texto = this.replaceCaracteresespeciales(element.texto);
          }
        }
        this.activarNotificacionesNoLeidas();
        this.Notificaciones = this.Notificaciones.sort((a, b) => (a.fechaCreacion < b.fechaCreacion) ? 1 : -1);
      });
    });
  }

  getDiffTime(): void {
    for (let element of this.Notificaciones) {
      const fecha: Date = new Date(element.fechaCreacion);
      element.tiempo = fecha;
      element.texto = element.texto.replace('<b>', '').replace('</b>', '');
      element.texto = element.texto.replace('<i>', '').replace('</i>', '');
      element.texto = element.texto.replace('<u>', '').replace('</u>', '');
    }
  }

  diferenciaEntreDiasEnDias(a: Date, b: Date) {
    const MILISENGUNDOS_POR_DIA = 1000 * 60 * 60 * 24;
    let dias = Math.floor((b.getTime() - a.getTime()) / MILISENGUNDOS_POR_DIA);
    let horas = 0;
    let semanas = 0;
    let hacee = '';
    let diass = '';
    let semanass = '';
    let horass = '';
    if (this.activeLang == 'es') {
      hacee = 'Hace ';
      diass = ' días';
      semanass = ' semanas';
      horass = ' horas';
    } else {
      hacee = '';
      diass = ' days ago';
      semanass = ' weeks ago';
      horass = ' hours ago';
    }
    if (dias == 0) {
      const MILISENGUNDOS_POR_HORA = 1000 * 60 * 60;
      horas = Math.floor((b.getTime() - a.getTime()) / MILISENGUNDOS_POR_HORA);
      return hacee + horas + horass;
    }
    else if (dias >= 7) {
      semanas = Math.floor(dias / 7);
      return hacee + semanas + semanass;
    }
    else {
      return hacee + dias + diass;
    }
  }

  verAvisos(): void {
    this.notificacioones.marcarTodasComoLeidas(this.Notificaciones).subscribe((r) => {
      this.getNotificacionesSinLeer();
      this.router.navigate(['/veravisos/'], {
        queryParams: {
          matricula: this.usuarioLogin
        }
      })
    });
    this.mostarModalAvisos = false;
  }

  verAvisosNotificaciones(isAviso: boolean): void {
    if (isAviso) {
      this.router.navigate(['/veravisos/'], {
        queryParams: {
          matricula: this.usuarioLogin
        }
      });
    }

  }

  getNotificacionesSinLeer(): void {
    this.notificacioones.getNotificaciones(true, this.usuarioLogin).subscribe((r: any) => {
      if (r.result) {
        this.notificacionesCount = String(r.listaNotificaciones.length) == '0' ? '' : String(r.listaNotificaciones.length);
        this.NotificacionesNoLeidas = r.listaNotificaciones;
        this.activarNotificacionesNoLeidas();
      }
    });
  }

  activarNotificacionesNoLeidas(): void {
    this.Notificaciones.forEach((a) => {
      a.activo = false;
      let find = this.NotificacionesNoLeidas.find(x => x.id == a.id);
      if (find) {
        a.activo = true;
      }
    });
  }

  marcarComoLeida(id: number, isNotificacion: boolean, activo: boolean): void {
    if (activo) {
      this.notificacioones.marcarComoLeida(id, true, isNotificacion, this.usuarioLogin).subscribe((r) => {
        this.getNotificacionesSinLeer();
      });
    }
    this.mostarModalAvisos = false;
  }

  marcarTodasComoLeidas(): void {
    this.notificacioones.marcarTodasComoLeidas(this.Notificaciones).subscribe((r) => {
      this.getNotificacionesSinLeer();
    });
    this.mostarModalAvisos = false;
  }

  replaceCaracteresespeciales(texto: String) {
    return texto.replace(/(<([^>]+)>)/ig, '');
  }

  logOut(): void {
    sessionStorage.clear();
    const url = `Auth/Logout/`;
    window.location.href = url;
  }

  login(): void {
    this.userService.getUserClaims().subscribe((data: any) => {
    });
  }

  parsedText(text: any): string|null {
    const dom = new DOMParser().parseFromString(
      '<!doctype html><body>' + text,
      'text/html');
    const decodedString = dom.body.textContent;
    return decodedString;
  }

}

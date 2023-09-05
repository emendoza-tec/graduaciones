import { Component, OnInit } from '@angular/core';
import { Menu, MenuHijo } from 'src/app/classes/Menu';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { AuthService } from 'src/app/services/auth-service.service';
import { MenuService } from 'src/app/services/menu.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { SideNavService } from 'src/app/services/sideNav.service';
import { groupBy, keys } from "lodash-es"
@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent implements OnInit {
  ListaMenu: Menu[] = [];
  showSubMenu = -1;

  constructor(public sideNavService: SideNavService, private menuService: MenuService, public aService: AuthService, private pnService: PermisosNominaService) { }

  ngOnInit() {
    let menuPermisos: PermisosMenu[] = this.pnService.obtenMenu();
   const lsKeys = keys(groupBy(menuPermisos, "idMenu"));
    lsKeys.forEach(e => {
      const index = menuPermisos.findIndex(f => f.idMenu === +e);
      let menuHijos: MenuHijo[] = []
      menuPermisos.forEach((e: PermisosMenu, iSubmenu: number) => {
        if (e.nombreSubMenu !== '' && menuPermisos[index].idMenu == e.idMenu && !menuPermisos[index].seccion) {
          let menuHijo = new MenuHijo(e.idSubMenu, e.nombreSubMenu, menuPermisos[iSubmenu].pathSubMenu, menuPermisos[iSubmenu].iconoSubMenu, e.idSubMenu, e.activa, e.editar, e.ver);
          menuHijos.push(menuHijo);
        }
      });
      const menu = new Menu(menuPermisos[index].idMenu, menuPermisos[index].nombreMenu, menuPermisos[index].pathMenu, menuPermisos[index].iconoMenu, menuPermisos[index].idMenu, menuPermisos[index].activa, menuPermisos[index].editar, menuPermisos[index].ver , menuHijos);
      this.ListaMenu.push(menu)
    });

    this.ListaMenu.push(new Menu(this.ListaMenu.length + 1, "Cerrar Sesión", 'Auth/Logout', 'bi-box-arrow-left', this.ListaMenu.length + 1, true, true, true, []))
  }

  toggleSideNav() {
    this.sideNavService.toggle();
  }
}

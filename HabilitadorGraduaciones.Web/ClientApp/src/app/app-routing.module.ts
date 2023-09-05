import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VerAvisosComponent } from './components/ver-avisos/ver-avisos.component';
import { UnauthorizedComponent } from './components/Admin/unauthorized/unauthorized.component';
import * as authGuard  from './shared/guard/index.guard';
import { TipoUsuarioComponent } from './components/tipoUsuario/tipoUsuario.component';

const routes: Routes = [
  {
    path: 'index',  
    loadChildren: () =>
    import("./modules/alumno.module").then(
      (m) => m.AlumnoModule
    ), 
    canActivate: [authGuard.AuthAlumnoGuard],
    canActivateChild: [authGuard.AuthAlumnoGuard],
  },
  { path: 'veravisos', component: VerAvisosComponent, canActivate: [authGuard.AuthAlumnoGuard] },
  {
    path: 'admin',
    canActivate: [authGuard.AuthAdminGuard],
    canActivateChild: [authGuard.AuthAdminGuard],
    loadChildren: () =>
    import("./modules/admin.module").then(
      (m) => m.AdminModule
    ),
  },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '', component: TipoUsuarioComponent },
  { path: '**',  component: TipoUsuarioComponent }
]
@NgModule({
  imports: [ RouterModule.forRoot(routes, { relativeLinkResolution: "legacy" })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

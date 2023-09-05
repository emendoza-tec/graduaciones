import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalDetalleComponent } from './components/modal-detalle/modal-detalle.component';
import { SnackbarComponent } from './shared/components/snackBar/snackBar.component';
import { ModalDetalleExpedienteComponent } from './components/modal-DetalleExpediente/modal-DetalleExpediente.component';
import { FormDialogComponent } from './components/form-dialog/form-dialog.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HomeMovilComponent } from './components/home-movil/home-movil.component';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ModificarFechaGraduacionComponent } from './components/modificar-fecha-graduacion/modificar-fecha-graduacion.component';
import { MaterialModules } from './material.module';
import { HomeAdminComponent } from './components/Admin/home-admin/home-admin.component';
import { SidenavComponent } from './components/Admin/sidenav/sidenav.component';
import { alumnosDuplicadosDialog, ExpedienteAdminComponent } from './components/Admin/expediente-admin/expediente-admin.component';
import { IndexAdminComponent } from './components/Admin/index-admin/index-admin.component';
import { HeaderAdminComponent } from './components/Admin/header-admin/header-admin.component';
import { PlanDeEstudiosAdminComponent } from './components/Admin/planDeEstudios-admin/planDeEstudios-admin.component';
import { SemanasTecAdminComponent } from './components/Admin/semanasTec-admin/semanasTec-admin.component';
import { ServicioSocialAdminComponent } from './components/Admin/servicioSocial-admin/servicioSocial-admin.component';
import { NivelDeInglesAdminComponent } from './components/Admin/nivelDeIngles-admin/nivelDeIngles-admin.component';
import { VerLoQueVeElAlumnoAdminComponent } from './components/Admin/verLoQueVeElAlumno-admin/verLoQueVeElAlumno-admin.component';
import { AvisosAdminComponent } from './components/Admin/avisos-admin/avisos-admin.component';
import { ConfiguracionAdminComponent } from './components/Admin/configuracion-admin/configuracion-admin.component';
import { ReporteCompletoAdminComponent } from './components/Admin/reporteCompleto-admin/reporteCompleto-admin.component';
import { DatePipe, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { VerAvisosComponent } from './components/ver-avisos/ver-avisos.component';
import { AlumnosDuplicadosExamenIntegradorDialog, ExamenIntegradorAdminComponent } from './components/Admin/examenIntegrador-admin/examenIntegrador-admin.component';
import { BienvenidaGraduacionComponent } from './components/bienvenida-graduacion/bienvenida-graduacion.component';
import { RegistroExitosoGraduacionComponent } from './components/registro-exitoso-graduacion/registro-exitoso-graduacion.component';
import { ModalCeremoniaGraduacionComponent } from './components/modal-ceremonia-graduacion/modal-ceremonia-graduacion.component';

import { RouterModule } from '@angular/router';
import { ConfiguracionService } from './services/configuracion.service';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CalendarioAdminComponent } from './components/Admin/calendario-admin/calendario-admin.component';
import { ModalCreditosInsuficientesComponent } from './components/modal-creditos-insuficientes/modal-creditos-insuficientes.component';
import { AvisosAdminConfirmComponent } from './components/Admin/avisos-admin-confirm/avisos-admin-confirm.component';
import { registerLocaleData } from '@angular/common';
import localeEs from '@angular/common/locales/es-MX';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { MatPaginatorIntlCro } from './helpers/CustomPaginator';
import { ModalExamenConocimientosComponent } from './components/modal-examen-conocimientos/modal-examen-conocimientos.component';
import { RolesIndexComponent } from './components/Admin/roles-admin/roles-index/roles-index.component';
import { DescargarTemplateUsuariosDialog, UsuariosAdminComponent } from './components/Admin/usuarios-admin/usuarios-admin/usuarios-admin.component';
import { PaginatorDirective } from './directives/paginator.directive';
import { UsuariosNewComponent } from './components/Admin/usuarios-admin/usuarios-new/usuarios-new.component';
import { UnauthorizedComponent } from './components/Admin/unauthorized/unauthorized.component';
import { RolCancelarDialog, RolesNuevoAdminComponent } from './components/Admin/roles-admin/roles-nuevo/rolesNuevo-admin.component';
import { RolEliminarDialog, RolesAdminComponent, verUsuariosAsignadosDialog } from './components/Admin/roles-admin/roles-admin/roles-admin.component';
import { TipoUsuarioComponent } from './components/tipoUsuario/tipoUsuario.component';
import { UsuariosIndexComponent } from './components/Admin/usuarios-admin/usuarios-index/usuarios-index.component';
import { PanelSolicitudComponent, SolicitudDeCambios } from './components/Admin/situacionesPorResolver-admin/panel-solicitud-admin/panel-solicitud.component';
import { SituacionesPorResolverAdminComponent } from './components/Admin/situacionesPorResolver-admin/situacionesPorResolver-admin/situacionesPorResolver-admin.component';
import { SituacionesPorResolverIndexComponent } from './components/Admin/situacionesPorResolver-admin/situacionesPorResolver-index/situacionesPorResolver-index.component';
import { AuthorizationInterceptor } from './shared/interceptor/interceptor';

// Llama el método load el cual es el que carga la información del estudiante
export function configurationProviderFactory(provider: ConfiguracionService) {
  return () => provider.load();
}

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}

registerLocaleData(localeEs, 'es-MX');

@NgModule({
  declarations: [
    TipoUsuarioComponent,
    AppComponent,
    IndexAdminComponent,
    HeaderAdminComponent,
    ReporteCompletoAdminComponent,
    ExpedienteAdminComponent,
    SituacionesPorResolverAdminComponent,
    PlanDeEstudiosAdminComponent,
    SemanasTecAdminComponent,
    ServicioSocialAdminComponent,
    NivelDeInglesAdminComponent,
    ExamenIntegradorAdminComponent,
    AlumnosDuplicadosExamenIntegradorDialog,
    PanelSolicitudComponent,
    VerLoQueVeElAlumnoAdminComponent,
    AvisosAdminComponent,
    ConfiguracionAdminComponent,
    NavMenuComponent,
    HomeComponent,
    ModalDetalleComponent,
    ModalDetalleExpedienteComponent,
    SnackbarComponent,
    FormDialogComponent,
    ConfirmDialogComponent,
    DynamicFormComponent,
    HomeMovilComponent,
    ModificarFechaGraduacionComponent,
    HomeAdminComponent,
    SidenavComponent,
    ExpedienteAdminComponent,
    alumnosDuplicadosDialog,
    VerAvisosComponent,
    BienvenidaGraduacionComponent,
    RegistroExitosoGraduacionComponent,
    ModalCeremoniaGraduacionComponent,
    CalendarioAdminComponent,
    ModalCreditosInsuficientesComponent,
    AvisosAdminConfirmComponent,
    SolicitudDeCambios,
    ModalExamenConocimientosComponent,
    RolesAdminComponent,
    UsuariosAdminComponent,
    UsuariosIndexComponent,
    PaginatorDirective,
    UsuariosNewComponent,
    UnauthorizedComponent,
    RolesNuevoAdminComponent,
    RolesIndexComponent,
    RolCancelarDialog,
    RolEliminarDialog,
    verUsuariosAsignadosDialog,
    SituacionesPorResolverIndexComponent,
    DescargarTemplateUsuariosDialog
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MaterialModules,
    AngularEditorModule,
    RouterModule,
    NgxSpinnerModule,
    TranslateModule.forRoot({
      defaultLanguage: 'es',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  exports: [
    NgxSpinnerModule],
  providers: [
    ConfiguracionService,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationProviderFactory,
      deps: [ConfiguracionService],
      multi: true,
    },
    DatePipe,
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: LOCALE_ID, useValue: 'es-MX' },
    [{ provide: MatPaginatorIntl, useClass: MatPaginatorIntlCro }],
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }

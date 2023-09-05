

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AvisosAdminComponent } from '../components/Admin/avisos-admin/avisos-admin.component';
import { CalendarioAdminComponent } from '../components/Admin/calendario-admin/calendario-admin.component';
import { ConfiguracionAdminComponent } from '../components/Admin/configuracion-admin/configuracion-admin.component';
import { ExamenIntegradorAdminComponent } from '../components/Admin/examenIntegrador-admin/examenIntegrador-admin.component';
import { ExpedienteAdminComponent } from '../components/Admin/expediente-admin/expediente-admin.component';
import { IndexAdminComponent } from '../components/Admin/index-admin/index-admin.component';
import { NivelDeInglesAdminComponent } from '../components/Admin/nivelDeIngles-admin/nivelDeIngles-admin.component';
import { PlanDeEstudiosAdminComponent } from '../components/Admin/planDeEstudios-admin/planDeEstudios-admin.component';
import { ReporteCompletoAdminComponent } from '../components/Admin/reporteCompleto-admin/reporteCompleto-admin.component';
import { RolesAdminComponent } from '../components/Admin/roles-admin/roles-admin/roles-admin.component';
import { RolesIndexComponent } from '../components/Admin/roles-admin/roles-index/roles-index.component';
import { RolesNuevoAdminComponent } from '../components/Admin/roles-admin/roles-nuevo/rolesNuevo-admin.component';
import { SemanasTecAdminComponent } from '../components/Admin/semanasTec-admin/semanasTec-admin.component';
import { ServicioSocialAdminComponent } from '../components/Admin/servicioSocial-admin/servicioSocial-admin.component';
import { UsuariosAdminComponent } from '../components/Admin/usuarios-admin/usuarios-admin/usuarios-admin.component';
import { UsuariosNewComponent } from '../components/Admin/usuarios-admin/usuarios-new/usuarios-new.component';
import { VerLoQueVeElAlumnoAdminComponent } from '../components/Admin/verLoQueVeElAlumno-admin/verLoQueVeElAlumno-admin.component';
import { HomeAdminComponent } from '../components/Admin/home-admin/home-admin.component';
import { UsuariosIndexComponent } from '../components/Admin/usuarios-admin/usuarios-index/usuarios-index.component';
import { SituacionesPorResolverIndexComponent } from '../components/Admin/situacionesPorResolver-admin/situacionesPorResolver-index/situacionesPorResolver-index.component';
import { SituacionesPorResolverAdminComponent } from '../components/Admin/situacionesPorResolver-admin/situacionesPorResolver-admin/situacionesPorResolver-admin.component';
import { PanelSolicitudComponent } from '../components/Admin/situacionesPorResolver-admin/panel-solicitud-admin/panel-solicitud.component';

const routes: Routes = [
    {
        path: '',
        component: HomeAdminComponent,
        children: [
            { path: 'expediente', component: ExpedienteAdminComponent },
            {
                path: 'porresolver', component: SituacionesPorResolverIndexComponent,
                children: [
                    {path: '', component: SituacionesPorResolverAdminComponent},
                    { path: 'paneldesolicitudes', component: PanelSolicitudComponent }
                ]
            },
            { path: 'reportes', component: ReporteCompletoAdminComponent },
            { path: 'avisos', component: AvisosAdminComponent },
            { path: 'configuracion', component: ConfiguracionAdminComponent },
            { path: 'nivelingles', component: NivelDeInglesAdminComponent },
            { path: 'planestudios', component: PlanDeEstudiosAdminComponent },
            { path: 'semanastec', component: SemanasTecAdminComponent },
            { path: 'serviciosocial', component: ServicioSocialAdminComponent },
            { path: 'veralumnos', component: VerLoQueVeElAlumnoAdminComponent },
            { path: 'examenintegrador', component: ExamenIntegradorAdminComponent },
            { path: 'calendario', component: CalendarioAdminComponent },
            {
                path: 'roles', component: RolesIndexComponent,
                children: [
                    { path: '', component: RolesAdminComponent },
                    { path: 'nuevo', component: RolesNuevoAdminComponent },
                    { path: 'editar/:isEditar/:id', component: RolesNuevoAdminComponent },
                    { path: 'ver/:isEditar/:id', component: RolesNuevoAdminComponent },
                ]
            }, {
                path: 'usuarios', component: UsuariosIndexComponent,
                children: [
                    { path: '', component: UsuariosAdminComponent, title: '' },
                    { path: 'nuevo', component: UsuariosNewComponent, title: 'Agregar Usuario' },
                    { path: 'editar/:id', component: UsuariosNewComponent, title: 'Editar' }
                ]
            },
            { path: 'usuarios-new', component: UsuariosNewComponent },
            { path: 'usuarios-edit/:id', component: UsuariosNewComponent },
            { path: '**', component: IndexAdminComponent }
        ]
    },

]
@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AdminRoutingModule { }
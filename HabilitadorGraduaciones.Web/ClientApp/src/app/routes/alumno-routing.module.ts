import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UnauthorizedComponent } from "../components/Admin/unauthorized/unauthorized.component";
import { HomeComponent } from "../components/home/home.component";
import { VerAvisosComponent } from "../components/ver-avisos/ver-avisos.component";

const routes: Routes = [
    {
        path: '', component: HomeComponent,
        children: [
            { path: 'veravisos', component: VerAvisosComponent },
            { path: 'unauthorized', component: UnauthorizedComponent },
            { path: '**', component: HomeComponent }
        ]
    },

]
@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AlumnoRoutingModule { }
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { SolicitudCambioDatosService } from 'src/app/services/solicitud-cambio-datos.service';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'app-situacionesPorResolver-admin',
  templateUrl: './situacionesPorResolver-admin.component.html',
  styleUrls: ['./situacionesPorResolver-admin.component.css']
})
export class SituacionesPorResolverAdminComponent implements OnInit {
  titulo = 'Situaciones por resolver';
  public total: number;
  cardPanel = 0;
  isCargandoSolPtes = false;
  permisoPanelSolicitud: PermisosMenu;

  constructor(
    private solicitudService: SolicitudCambioDatosService,
    private snackBar: SnackBarService,
    private spinner: NgxSpinnerService,
    private titleService: TitleService,
    private pnService: PermisosNominaService
  ) {
    this.titleService.changeTitle('');
    this.permisoPanelSolicitud = this.pnService.obtenSubmenuPermisoPorNombre('Panel de solicitud de modificación de datos');
  }

  ngOnInit() {
    this.isCargandoSolPtes = true;
    this.spinner.show();
    const idUsuario = +this.pnService.obtenIdUsuario();
    this.solicitudService.getConteoPendientes(idUsuario).subscribe((data: any) => {
      this.total = data.total;
      this.isCargandoSolPtes = false;
      this.spinner.hide();
    }, (err: HttpErrorResponse) => {
      this.snackBar.openSnackBar('Error al procesar la solicitud', 'error');
      this.isCargandoSolPtes = false;
      this.spinner.hide();
    });
  }
}

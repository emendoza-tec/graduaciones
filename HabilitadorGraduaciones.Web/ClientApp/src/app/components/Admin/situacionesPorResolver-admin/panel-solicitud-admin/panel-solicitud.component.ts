import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, Inject, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort} from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { saveAs } from 'file-saver-es';
import { NgxSpinnerService } from 'ngx-spinner';
import { DetalleSolicitud, Solicitud } from '../../../../classes/SolicitudCambioDatos';
import { SnackBarService } from '../../../../services/snackBar.service';
import { SolicitudCambioDatosService } from '../../../../services/solicitud-cambio-datos.service';
import { PermisosNominaService } from 'src/app/services/permisosNomina.service';
import { TitleService } from 'src/app/services/title.service';
import { PermisosMenu } from 'src/app/interfaces/PermisosNomina';
import { PaginatorService } from 'src/app/services/paginator.service';

@Component({
  selector: 'app-panel-solicitud',
  templateUrl: './panel-solicitud.component.html',
  styleUrls: ['./panel-solicitud.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class PanelSolicitudComponent implements AfterViewInit {

  selectedTab = 0;
  public solicitud: Solicitud;
  displayedColumns: string[] = ['matricula', 'periodoGraduacion', 'descripcion',
    'fechaSolicitud', 'ultimaActualizacion', 'estatus', 'actions'];
  displayedColumnsAprobadas: string[] = ['matricula', 'periodoGraduacion', 'descripcion',
    'fechaSolicitud', 'ultimaActualizacion', 'estatus'];
  dataSourcePendientes: MatTableDataSource<Solicitud>;
  dataSourceAprobadas: MatTableDataSource<Solicitud>;
  dataSourceNoAprobadas: MatTableDataSource<Solicitud>;
  totalPendienes = 0; totalAprobados = 0; totalNoAprobados = 0;
  isLoadingResultsSolNoAprobadas = false;
  isLoadingResultsSolPendientes = false;
  isLoadingResultsSolAprobadas = false;
  isDetailsResults = false;
  public hasData: any;



  @ViewChild('paginatorSolPendientes') paginatorSolPendientes: MatPaginator;
  @ViewChild('paginatorSolAprobadas') paginatorSolAprobadas: MatPaginator;
  @ViewChild('paginatorSolNoAprobadas') paginatorSolNoAprobadas: MatPaginator;

  @ViewChild('sortSolPendientes') sortSolPendientes: MatSort;
  @ViewChild('sortSolAprobadas') sortSolAprobadas: MatSort;
  @ViewChild('sortSolNoAprobadas') sortNoAprobadas: MatSort;

  constructor(
    private _snackBar: SnackBarService,
    private solicitudService: SolicitudCambioDatosService,
    public dialog: MatDialog, private pnService: PermisosNominaService,
    private spinner: NgxSpinnerService, private pService: PaginatorService,
    private titleService: TitleService, private cdr: ChangeDetectorRef
  ) {
    this.titleService.changeTitle('Panel de solicitudes de modificación de datos');
  }

  ngAfterViewInit() {
    const idUsuario = this.pnService.obtenIdUsuario();
    this.getSolicitudesPendientes(idUsuario);
    this.getSolicitudesAprobadas(idUsuario);
    this.getSolicitudesNoAprobadas(idUsuario);
  }

  getSolicitudesPendientes(idUsuario: number): void {
    this.isLoadingResultsSolPendientes = true;
    this.spinner.show('isLoadingResultsSolPendientes');
    this.cdr.detectChanges();
    this.solicitudService.getPendientes(idUsuario).subscribe((data: any) => {
      if (data.length > 0) {
        this.totalPendienes = data.length;
        this.dataSourcePendientes = new MatTableDataSource<Solicitud>(data);
      } else {
        this.totalPendienes = 0;
        this.dataSourcePendientes = new MatTableDataSource<Solicitud>([]);
      }
      this.cdr.detectChanges();
      const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataSourcePendientes.data.length, false);
      this.paginatorSolPendientes.pageSizeOptions = pageSizeOptions;
      this.paginatorSolPendientes.pageSize = pageSizeOptions[0];
      this.dataSourcePendientes.paginator = this.paginatorSolPendientes;
      this.dataSourcePendientes.sort = this.sortSolPendientes;
      this.hasData = (this.dataSourcePendientes.data.length > 0);
      this.spinner.hide('isLoadingResultsSolPendientes');
      this.isLoadingResultsSolPendientes = false;
    }, (err: HttpErrorResponse) => {
      this._snackBar.openSnackBar('Error al procesar la solicitud', 'error');
      this.spinner.hide('isLoadingResultsSolPendientes');
      this.isLoadingResultsSolPendientes = false;
    });
  }

  getSolicitudesAprobadas(idUsuario: number): void {
    this.isLoadingResultsSolAprobadas = true;
    this.spinner.show('isLoadingResultsSolAprobadas');
    this.cdr.detectChanges();
    this.solicitudService.getSolicitudes(3, idUsuario).subscribe((data: any) => {
      if (data.length > 0) {
        this.totalAprobados = data.length;
        this.dataSourceAprobadas = new MatTableDataSource(data);
      } else {
        this.totalAprobados = 0;
        this.dataSourceAprobadas = new MatTableDataSource<Solicitud>([]);
      }
      this.cdr.detectChanges();
      const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataSourceAprobadas.data.length, false);
      this.paginatorSolAprobadas.pageSizeOptions = pageSizeOptions;
      this.paginatorSolAprobadas.pageSize = pageSizeOptions[0];
      this.dataSourceAprobadas.paginator = this.paginatorSolAprobadas;
      this.dataSourceAprobadas.sort = this.sortSolAprobadas;
      this.hasData = (this.dataSourceAprobadas.data.length > 0);
      this.isLoadingResultsSolAprobadas = false;
      this.spinner.hide('isLoadingResultsSolAprobadas');
    }, (err: HttpErrorResponse) => {
      this._snackBar.openSnackBar('Error al procesar la solicitud', 'error');
      this.isLoadingResultsSolAprobadas = false;
      this.spinner.hide('isLoadingResultsSolAprobadas');
    });
  }

  getSolicitudesNoAprobadas(idUsuario: number): void {
    this.isLoadingResultsSolNoAprobadas = true;
    this.spinner.show('isLoadingResultsSolNoAprobadas');
    this.cdr.detectChanges();
    this.solicitudService.getSolicitudes(4, idUsuario).subscribe((data: any) => {
      if (data.length > 0) {
        this.totalNoAprobados = data.length;
        this.dataSourceNoAprobadas = new MatTableDataSource(data);
      }else {
        this.totalNoAprobados = 0;
        this.dataSourceNoAprobadas = new MatTableDataSource<Solicitud>([]);
      }
      this.cdr.detectChanges();
      const pageSizeOptions = this.pService.obtenPageSizeOptions(this.dataSourceNoAprobadas.data.length, false);
      this.paginatorSolNoAprobadas.pageSizeOptions = pageSizeOptions;
      this.paginatorSolNoAprobadas.pageSize = pageSizeOptions[0];
      this.dataSourceNoAprobadas.paginator = this.paginatorSolNoAprobadas;
      this.dataSourceNoAprobadas.sort = this.sortNoAprobadas;
      this.hasData = (this.dataSourceNoAprobadas.data.length > 0);
      this.isLoadingResultsSolNoAprobadas = false;
      this.spinner.hide('isLoadingResultsSolNoAprobadas');
    }, (err: HttpErrorResponse) => {
      this._snackBar.openSnackBar('Error al procesar la solicitud', 'error');
      this.isLoadingResultsSolNoAprobadas = false;
      this.spinner.hide('isLoadingResultsSolNoAprobadas');
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourcePendientes.filter = filterValue.trim().toLowerCase();

    if (this.dataSourcePendientes.paginator) {
      this.dataSourcePendientes.paginator.firstPage();
    }
  }

  applyFilterAprobadas(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceAprobadas.filter = filterValue.trim().toLowerCase();

    if (this.dataSourceAprobadas.paginator) {
      this.dataSourceAprobadas.paginator.firstPage();
    }
  }

  applyFilterNoAprobadas(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceNoAprobadas.filter = filterValue.trim().toLowerCase();

    if (this.dataSourceNoAprobadas.paginator) {
      this.dataSourceNoAprobadas.paginator.firstPage();
    }
  }

  verDetalle(solicitud: Solicitud): void {
    let detalleSolicitud: any;
    this.isDetailsResults = true;
    this.spinner.show('isDetailsResults');
    this.solicitudService.getDetalle(solicitud.numeroSolicitud).subscribe({
      next: (detalle: any) => {
        if (detalle.length > 0) {
          detalleSolicitud = detalle;
        } else {
          this._snackBar.openSnackBar('La solicitud no contiene detalles', 'success');
        }
      },
      error: (err: HttpErrorResponse) => {
        this._snackBar.openSnackBar('Error al procesar la solicitud', 'error');
        this.isDetailsResults = false;
        this.spinner.hide('isDetailsResults');
      },
      complete: () => {
        this.isDetailsResults = false;
        this.spinner.hide('isDetailsResults');
        const dialogRef = this.dialog.open(SolicitudDeCambios, {
          width: '800px',
          data: { detalleSolicitud: detalleSolicitud, solicitud: solicitud }
        });

        dialogRef.afterClosed().subscribe(id => {
          if (!id) {
            return;
          }
          if (id != -1) {
            const idUsuario = this.pnService.obtenIdUsuario();
            this.getSolicitudesPendientes(idUsuario);
            this.getSolicitudesAprobadas(idUsuario);
            this.getSolicitudesNoAprobadas(idUsuario);
          }
        });
      }
    });
  }
}

@Component({
  selector: 'solicitudDeCambiosDialog',
  templateUrl: 'solicitudDeCambiosDialog.html',
  styleUrls: ['./panel-solicitud.component.css']
})

export class SolicitudDeCambios {

  isLoadingResults = false;
  displayedColumns: string[] = ['Descripcion', 'DatoIncorrecto', 'DatoCorrecto', 'Documento'];
  dataSourceDetalle: MatTableDataSource<DetalleSolicitud>;
  fechaSolicitud: Date;
  listEstatus: any[] = [];
  form: FormGroup;
  usuario: string = '';
  permisoPanelSolicitud: PermisosMenu;
  isSavingData = false;
  isDownloadFile = false;

  constructor(
    private solicitudService: SolicitudCambioDatosService,
    private _snackBar: SnackBarService,
    public dialogRef: MatDialogRef<SolicitudDeCambios>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private pnService: PermisosNominaService,
    private spinner: NgxSpinnerService,
  ) {
    this.permisoPanelSolicitud = this.pnService.obtenSubmenuPermisoPorNombre('Panel de solicitud de modificación de datos');
    this.usuario = this.pnService.obtenNomina();
    this.form = this.fb.group({
      idSolicitud: ['', Validators.required],
      matricula: ['', Validators.required],
      idEstatusSolicitud: ['', Validators.required],
      usarioRegistro: ['', Validators.required],
      numeroSolicitud: [''],
      comentarios: ['', Validators.required],
      ok: [''],
      error: ['',],
      idCorreo: ['']
    });
    if (!this.permisoPanelSolicitud.editar) {
      this.form.get('idEstatusSolicitud')?.disable();
      this.form.get('comentarios')?.disable();
      this.form.disable();
    }
  }

  ngOnInit() {
    this.getEstatus();
    this.fechaSolicitud = this.data.solicitud.fechaSolicitud;
    this.dataSourceDetalle = new MatTableDataSource(this.data.detalleSolicitud);
    this.form.setValue({
      idSolicitud: this.data.solicitud.idSolicitud,
      matricula: this.data.solicitud.matricula,
      idEstatusSolicitud: null,
      usarioRegistro: this.usuario,
      numeroSolicitud: this.data.solicitud.numeroSolicitud,
      comentarios: null,
      ok: true,
      error: '',
      idCorreo: 0
    });
  }

  getEstatus(): void {
    this.solicitudService.getEstatusSolicitudes().subscribe((estatus: any) => {
      this.listEstatus = estatus;
    }, (err: HttpErrorResponse) => {
      this._snackBar.openSnackBar('Error al procesar la solicitud', 'error');
    });
  }

  cancelar(): void {
    this.dialogRef.close();
  }

  downloadFile(item: any): void {
    this.isDownloadFile = true;
    this.solicitudService.downloadFile(item.azureStorage).subscribe(res => {
      const nombre = item.documento;
      saveAs(res, nombre);
      this.isDownloadFile = false;
    });
  }

  guardaModificacion(): void {
    this.isSavingData = true;
    this.spinner.show();
    this.solicitudService.modificaSolicitud(this.form.value).subscribe((data: any) => {
      this.isSavingData = false;
      this.spinner.hide();
      if (data.result) {
        this._snackBar.openSnackBar('La solicitud ha cambiado de estatus ', 'success');
        this.dialogRef.close(1);
      } else {        
        this._snackBar.openSnackBar('Error al cambiar el estatus de la solicitud', 'error');
        this.dialogRef.close(-1);
      }
    }, (err: HttpErrorResponse) => {
      this.isSavingData = false;
      this.spinner.hide();
      this._snackBar.openSnackBar('Error al cambiar el estatus de la solicitud' + err.message, 'error');
      this.dialogRef.close(-1);
    });
  }
} 

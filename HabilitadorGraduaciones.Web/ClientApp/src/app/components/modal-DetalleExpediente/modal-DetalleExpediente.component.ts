import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { DetalleExpediente } from 'src/app/classes/Expediente';
import { RequisitosService } from '../../services/requisitos';
import { SnackBarService } from '../../services/snackBar.service';

@Component({
  selector: 'app-modal-DetalleExpediente',
  templateUrl: './modal-DetalleExpediente.component.html',
  styleUrls: ['./modal-DetalleExpediente.component.css']
})

export class ModalDetalleExpedienteComponent implements OnInit {
  listaDocumentos: any = [];
  listaDoc: any = [];
  descExtranjero: string = '';
  link: string='';

  constructor(public dialogRef: MatDialogRef<ModalDetalleExpedienteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DetalleExpediente, private snackBarService: SnackBarService,
    private tService: RequisitosService, private translate: TranslateService  ) {
  }

  ngOnInit() {
    this.listaDocumentos = this.data.documentos;
    this.listaDoc = this.data.documentos;
    this.descExtranjero = this.data.detalle;
    this.validarTab(true);
  }

  cancelar():void {
    this.dialogRef.close();
  }

  copyMessage(val: string) {
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = val;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
    this.snackBarService.openSnackBar('El Link se copió correctamente', 'default', 3000);
  }

  validarTab(Mexicano: boolean): void {
    if (Mexicano) {
      this.listaDoc = this.listaDocumentos.filter((x: any) => x.mexicano);
      this.listaDoc = this.listaDoc.sort((a: any, b: any) => (a.orden > b.orden) ? 1 : -1);
    } else {
      this.listaDoc = this.listaDocumentos.filter((x: any) => x.extranjero);
      this.listaDoc = this.listaDoc.sort((a: any, b: any) => (a.orden > b.orden) ? 1 : -1);
      this.tService.getTarjeta(11, this.translate.currentLang).subscribe((r: any) => {
        this.descExtranjero = r.nota;
      });
    }

  }
}

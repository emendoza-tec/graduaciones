import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackBarService } from 'src/app/services/snackBar.service';
import { DataModal } from '../../classes/DataModal';
@Component({
  selector: 'app-modal-detalle',
  templateUrl: './modal-detalle.component.html',
  styleUrls: ['./modal-detalle.component.css']
})

export class ModalDetalleComponent implements OnInit {
  isContacto: boolean = false;

  constructor(public dialogRef: MatDialogRef<ModalDetalleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DataModal, private snackBarService: SnackBarService) { }

  ngOnInit(): void {
    if (this.data.contacto!.length > 0 && (this.data.contacto![1])) {
      this.isContacto = true;
    }
  }

  cancelar() {
    this.dialogRef.close();
  }

  copyMessage(val: string){
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
    this.snackBarService.openSnackBar('El Link se copió correctamente','default', 3000);
  }
}

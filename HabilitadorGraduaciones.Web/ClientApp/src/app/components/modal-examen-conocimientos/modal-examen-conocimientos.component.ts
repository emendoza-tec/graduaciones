import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ExamenConocimientosDataModal } from '../../classes/ExamenConocimientos';
import { SnackBarService } from '../../services/snackBar.service';

@Component({
  selector: 'app-modal-examen-conocimientos',
  templateUrl: './modal-examen-conocimientos.component.html',
  styleUrls: ['./modal-examen-conocimientos.component.css']
})
export class ModalExamenConocimientosComponent implements OnInit {


  isContacto: boolean = false;

  constructor(public dialogRef: MatDialogRef<ModalExamenConocimientosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExamenConocimientosDataModal, private snackBarService: SnackBarService) { }

  ngOnInit(): void {
    if (this.data.contacto!.length > 0 && (this.data.contacto![1])) {
      this.isContacto = true;
    }
  }

  cancelar(): void {
    this.dialogRef.close();
  }

  copyMessage(val: string):void {
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

}

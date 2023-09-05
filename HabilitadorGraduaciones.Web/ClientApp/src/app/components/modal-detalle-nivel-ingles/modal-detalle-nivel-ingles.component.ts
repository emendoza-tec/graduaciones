import { Component, Inject, OnInit } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { DetalleNivelIngles } from '../../classes/NivelIngles';

@Component({
  selector: 'app-modal-detalle-nivel-ingles',
  templateUrl: './modal-detalle-nivel-ingles.component.html',
  styleUrls: ['./modal-detalle-nivel-ingles.component.css']
})

export class ModalDetalleNivelInglesComponent implements OnInit {
  constructor( public dialogRef: MatDialogRef<ModalDetalleNivelInglesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DetalleNivelIngles) { }

    ngOnInit() {
    }
  
    cancelar() {
      this.dialogRef.close();
    }
  
  }

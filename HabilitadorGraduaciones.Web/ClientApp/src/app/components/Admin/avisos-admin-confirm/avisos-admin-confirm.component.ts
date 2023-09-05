import { CommonModule } from '@angular/common';
import { Component, Inject, NgModule, NO_ERRORS_SCHEMA, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-avisos-admin-confirm',
  templateUrl: './avisos-admin-confirm.component.html',
  styleUrls: ['./avisos-admin-confirm.component.css']
})
export class AvisosAdminConfirmComponent implements OnInit {
  hasMatricula: boolean | undefined;
  formattedData: string | undefined;
  dictionary: any;
  numMatriculas: number = 0;
  constructor(public dialogRef: MatDialogRef<AvisosAdminConfirmComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.numMatriculas = JSON.parse(this.data.numMatriculas);
    this.dictionary = JSON.parse(this.data.json);
    this.dictionary.nivel = this.data.filtros.nivel.find((obj: { claveNivel: string; descripcion: string; }) => obj.claveNivel === this.dictionary.nivel)?.descripcion || 'Todos';
    this.dictionary.sedeId = this.data.filtros.sedes.filter((obj: { claveSede: string; descripcion: string; }) => obj.claveSede === this.dictionary.sedeId)[0]?.descripcion || 'Todos';
    this.dictionary.campusId = this.data.filtros.campus.filter((obj: { claveCampus: string; descripcion: string; }) => obj.claveCampus === this.dictionary.campusId)[0]?.descripcion || 'Todos';
    this.dictionary.escuelasId = this.data.filtros.escuelas.filter((obj: { clave: string; descripcion: string; }) => obj.clave === this.dictionary.escuelasId)[0]?.descripcion || 'Todos';
    this.dictionary.programaId = this.data.filtros.programas.filter((obj: { clave: string; descripcion: string; }) => obj.clave === this.dictionary.programaId)[0]?.descripcion || 'Todos';
    this.dictionary.requisitoId = this.data.filtros.requisitos.filter((obj: { clave: string; descripcion: string; }) => obj.clave === this.dictionary.requisitoId)[0]?.descripcion || 'Todos';
    if (typeof (this.dictionary.matricula) === 'string')
    {
      this.dictionary.matricula = 'Todos';
      this.hasMatricula = false;
    } else {
      if (this.dictionary.matricula.length > 0) {
        this.hasMatricula = true;
      }
      else {
        this.dictionary.matricula = 'Todos';
        this.hasMatricula = false;
      }
    }

    if(this.numMatriculas == 0){
      this.dictionary.matricula = 'No se encontró ninguna matrícula con esa información.';
    }

    this.dictionary.cc_camposId = this.data.filtros.campus.filter((obj: { claveCampus: string; descripcion: string; }) => obj.claveCampus === this.dictionary.cc_camposId)[0]?.descripcion || 'Ninguno';
    this.dictionary.cc_rolesId = this.data.filtros.roll.filter((obj: { clave: string; descripcion: string; }) => obj.clave === this.dictionary.cc_rolesId)[0]?.descripcion || 'Ninguno';

    
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  submit(): void  {
    this.dialogRef.close("Accepted");
  }

  exit(): void  {
    this.dialogRef.close("Cancelled");
  }

}


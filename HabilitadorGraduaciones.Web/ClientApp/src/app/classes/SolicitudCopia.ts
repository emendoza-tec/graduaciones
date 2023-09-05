export class SolicitudCopia {
  nombre: string;
  curp: string;
  programaAcademico: string;
  concentraciones: any[];
  diplomasAcademicos: any[];

  constructor(nombre: string, curp: string, programaAcademico: string, concentraciones: any[], diplomasAcademicos: any[]) {
    this.nombre = nombre;
    this.curp = curp;
    this.programaAcademico = programaAcademico;
    this.concentraciones = concentraciones;
    this.diplomasAcademicos = diplomasAcademicos;
  }
}

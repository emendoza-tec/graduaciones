
export class Ceneval {
    matricula: string;
    cumpleRequisitoCeneval: boolean;
  fechaExamen: Date;
    fechaUltimaModificacion: Date;
    colorCumple: string;
    isCumple: boolean;
    esRequisito: boolean;


  constructor(matricula: string, cumpleRequisitoCeneval: boolean, fechaExamen: Date, fechaUltimaModificacion: Date, colorCumple: string, isCumple: boolean, esRequisito: boolean) {

        this.matricula = matricula;
        this.cumpleRequisitoCeneval = cumpleRequisitoCeneval;
        this.fechaExamen = fechaExamen;
        this.fechaUltimaModificacion = fechaUltimaModificacion;
        this.colorCumple = colorCumple;
        this.isCumple = isCumple;
        this.esRequisito = esRequisito;
    }
}

export class DetalleCeneval{
    titulo: string;
    detalle: string;
    isCUMPLE_NIVEL: boolean;

    constructor(titulo: string, detalle: string, isCUMPLE_NIVEL: boolean) {
        this.titulo = titulo;
        this.detalle = detalle;
        this.isCUMPLE_NIVEL = isCUMPLE_NIVEL; 
      }
}

export class CenevalDataModal {
    constructor(public titulo?: string, public estadoDetalle?:string, public isCUMPLE_NIVEL?: boolean) {
    }
}

export class ExamenIntegrador{
  matricula: string;
  estatus: string;
  ultimaActualizacion: Date;
  isCumple: boolean;
  colorCumple: string;
  colorOff: string;
  aplica: boolean;
  fechaExamen: string;


  constructor(matricula: string, estatus: string, ultimaActualizacion: Date, isCumple: boolean, colorCumple: string, colorOff: string, aplica: boolean, fechaExamen: string) {

    this.matricula = matricula;
    this.estatus = estatus;
    this.ultimaActualizacion = ultimaActualizacion;
    this.isCumple=isCumple;
    this.colorCumple = colorCumple;
    this.colorOff = colorOff;
    this.aplica = aplica;
    this.fechaExamen = fechaExamen;
  }
}

export class DetalleExamenIntegrador {
  titulo: string;
  detalle: string;
  isCUMPLE: boolean;

  constructor(titulo: string, detalle: string, isCUMPLE: boolean) {
    this.titulo = titulo;
    this.detalle = detalle;
    this.isCUMPLE = isCUMPLE; 
  
  }
}

export class ExamenIntegradorDataModal {
  constructor(public titulo?: string, public estadoDetalle?: string, public isCUMPLE?: boolean) {
  }
}

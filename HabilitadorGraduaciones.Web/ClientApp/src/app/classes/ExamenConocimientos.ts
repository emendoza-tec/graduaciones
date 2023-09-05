export class ExamenConocimientos {
  idTipoExamen: number;
  matricula: string;
  descripcionExamen: string;
  tituloExamen: string;
  cumpleRequisito: boolean;
  estatus: string;
  fechaExamen: Date;
  fechaRegistro: Date;
  colorCumple: string;
  isCumple: boolean;
  esRequisito: boolean;


  constructor(idTipoExamen: number, matricula: string, descripcionExamen: string, tituloExamen: string,
    cumpleRequisito: boolean, fechaExamen: Date, fechaRegistro: Date, colorCumple: string, isCumple: boolean,
    esRequisito: boolean) {
    this.idTipoExamen = idTipoExamen;
    this.matricula = matricula;
    this.descripcionExamen = descripcionExamen
    this.tituloExamen = tituloExamen
    this.cumpleRequisito = cumpleRequisito;
    this.fechaExamen = fechaExamen;
    this.fechaRegistro = fechaRegistro;
    this.colorCumple = colorCumple;
    this.isCumple = isCumple;
    this.esRequisito = esRequisito;
  }
}

export class DetalleExamenConocimientos {
  titulo: string;
  descripcionExamen: string;
  detalle: string;
  isCUMPLE_NIVEL: boolean;

  constructor(titulo: string, descripcionExamen: string, detalle: string, isCUMPLE_NIVEL: boolean) {
    this.titulo = titulo;
    this.descripcionExamen =descripcionExamen;
    this.detalle = detalle;
    this.isCUMPLE_NIVEL = isCUMPLE_NIVEL;
  }
}

export class ExamenConocimientosDataModal {
  constructor(public titulo: string, public descripcion: string, public mensaje: string, public link: string,
    public contacto?: any[], public lista?: any[], public isCumple?: boolean, public aplica?: boolean,
    public linkName: string = 'none') {
  }
}

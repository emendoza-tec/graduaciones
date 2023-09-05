
export class PlanDeEstudios
{
  IdAlumno: number;
  CreditosRequisito: number;
  CreditosAcreditados: number;
  UltimaActualizacion: Date;
  IsCumple: boolean;
  ColorCumple: string;

  constructor(IdAlumno: number, CreditosRequisito: number, CreditosAcreditados: number, UltimaActualizacion: Date, IsCumple: boolean, ColorCumple: string) {
    this.IdAlumno = IdAlumno;
    this.CreditosRequisito = CreditosRequisito;
    this.CreditosAcreditados = CreditosAcreditados;
    this.UltimaActualizacion = UltimaActualizacion;
    this.IsCumple = IsCumple;
    this.ColorCumple = ColorCumple;
  }
}

export class DetallePlanDeEstudios {
  Titulo: string;
  Detalle: string;
  Dudas: Duda;
  EstadoDetalle: string;
  IsCumple: boolean;

  constructor(Titulo: string, Detalle: string, Dudas: Duda, EstadoDetalle: string, IsCumple: boolean) {
    this.Titulo = Titulo;
    this.Detalle = Detalle;
    this.Dudas = Dudas;
    this.IsCumple = IsCumple;
    this.EstadoDetalle = EstadoDetalle;
  }
}

export class Duda {
  contactos: Contacto[];
  constructor(contactos: Contacto[]) {
    this.contactos = contactos;
  }
}

export class Contacto {
  nombre: string;
  correo: string;

  constructor(nombre: string, correo: string) {
    this.nombre = nombre;
    this.correo = correo;
  }
}


export class ServicioSocial {
  IdAlumno: number;
  Carrera: string;
  HorasRequeridas: number;
  HorasAcreditadas: number;
  UltimaActualizacion: Date;
  IsCumple: boolean;
  ColorCumple: string;

  constructor(IdAlumno: number, Carrera: string, HorasRequeridas: number, HorasAcreditadas: number, UltimaActualizacion: Date, IsCumple: boolean, ColorCumple: string) {
    this.IdAlumno = IdAlumno;
    this.Carrera = Carrera;
    this.HorasRequeridas = HorasRequeridas;
    this.HorasAcreditadas = HorasAcreditadas;
    this.UltimaActualizacion = UltimaActualizacion;
    this.IsCumple = IsCumple;
    this.ColorCumple = ColorCumple;
  }
}// FIN DE LA CLASE ServicioSocial

  export class DetalleServicioSocial {
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
} // FIN DE LA CLASE DetalleServicioSocial


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


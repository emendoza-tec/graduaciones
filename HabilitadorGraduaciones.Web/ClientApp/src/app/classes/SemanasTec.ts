export class SemanasTec {
  constructor(
    isCumple: boolean,
    color: string,
    semanasObtenidas: number,
    semanasMaximas: number,
    ultimaActualizacion: string,
    result: boolean,
    errorMessage: string
  ) {
    this.isCumple = isCumple;
    this.color = color;
    this.semanasMaximas = semanasMaximas;
    this.semanasObtenidas = semanasObtenidas;
    this.ultimaActualizacion = ultimaActualizacion;
    this.errorMessage = errorMessage;
    this.result = result;
  }
  isCumple: boolean;
  color: string;
  semanasObtenidas: number;
  semanasMaximas: number;
  ultimaActualizacion: string;
  result: boolean;
  errorMessage: string;
}



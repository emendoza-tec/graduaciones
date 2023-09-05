export class DatoPersonal {
  nombreDato: string;
  valor: string;
  isActivo: boolean

  constructor(nombreDato: string, valor: string, isActivo: boolean) {
    this.nombreDato = nombreDato;
    this.valor = valor;
    this.isActivo = isActivo;
  }
}

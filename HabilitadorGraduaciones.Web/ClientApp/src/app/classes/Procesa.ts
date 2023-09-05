export class ProcesaExpediente{
  Matricula: string;
  Estatus: string;
  Mensaje: string;
  
  constructor(Matricula: string, Estatus: string, Mensaje: string) {
    this.Matricula = Matricula;
    this.Estatus = Estatus;
    this.Mensaje = Mensaje;
  }
}

export class AnalisisExpediente{
  totalRegistros: number;
  procesaExpediente: ProcesaExpediente[]
  constructor(totalRegistros: number, procesaExpediente: ProcesaExpediente[]){
    this.totalRegistros= totalRegistros;
    this.procesaExpediente = procesaExpediente;
  }
}

export class ProcesaExamenIntegrador{
  Matricula: string;
  PeriodoGraduacion: number;
  Nivel: number;
  NombreRequisito: string;
  Estatus: string;
  FechaExamen: Date;

  constructor(Matricula: string, PeriodoGraduacion: number, Nivel: number, NombreRequisito: string, Estatus: string, FechaExamen: Date){
    this.Matricula = Matricula,
    this.PeriodoGraduacion = PeriodoGraduacion,
    this.Nivel = Nivel,
    this.NombreRequisito = NombreRequisito,
    this.Estatus = Estatus,
    this.FechaExamen = FechaExamen
  }
}

export class AnalisisExamenIntegrador{
  totalRegistros: number;
  procesaExamenIntegrador: ProcesaExamenIntegrador[];

  constructor(totalRegistros: number, procesaExamenIntegrador: ProcesaExamenIntegrador[]){
    this.totalRegistros = totalRegistros,
    this.procesaExamenIntegrador = procesaExamenIntegrador
  }
}
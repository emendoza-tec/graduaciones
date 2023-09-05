export interface Periodo {
    periodoId: string;
    matricula:string;
    descripcion: string;
    fechaInicio: Date;
    fechaFin: Date;
    isRegular: boolean;
    periodoElegido: string;
    periodoEstimado: string; 
    periodoCeremonia: string;
    motivoCambioPeriodo: string;
    eleccionAsistenciaCeremonia: string; 
    motivoNoAsistirCeremonia: string;
    origenActualizacionPeriodoId: number;
}
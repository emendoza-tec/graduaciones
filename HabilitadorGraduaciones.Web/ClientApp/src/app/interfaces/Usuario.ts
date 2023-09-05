export interface Usuario {
  nivelAcademico: string;
  nombre: string;
  apeidoPaterno: string;
  apeidoMaterno: string;
  matricula: string;
  curp: string;
  correo: string;
  programaAcademico: string;
  diplomasAdicionales: string;
  concentracion:string;
  telefono: string;
  mentor: string;
  directorPrograma: string;
  correoDirector: string;
  correoMentor: string;
  periodoActual: string;
  claveProgramaAcademico: string;
  carreraId: string;
  periodoGraduacion: string;
  concentraciones: any[]; 
  claveEstatusGraduacion: string;
  periodoTranscurridoActual: string;
  claveCampus: string; 
}

export interface UserClaims {
  nombreCompleto: string;
  matricula: string;
  pidm: string;
  email: string;
  nomina: string
}

export interface Acceso {
  matricula: string;
  ambiente: string;
  acceso: boolean
}

export interface DatosPersonalesCorreo {
  nombre: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  programaAcademico: string;  
  curp: string;
  correo: string;
  diplomasAdicionales: string[];
  concentracion:string[];
}
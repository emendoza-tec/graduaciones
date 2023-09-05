export interface Aviso {
  titulo: String;
  texto: String;
  urlImage: String;
  matricula: String;
  nivelId: String;
  campusId: String;
  sedeId: String;
  requisitoId: String;
  programaId: String;
  escuelasId: string;
  cc_rolesId: String;
  cc_camposId: String;
  fechaCreacion: Date;
  correo: boolean;
  habilitador: boolean;
}

export interface FitrosMatricula {
  nivelId: String;
  campusId: String;
  sedeId: String;
  programaId: String;
  escuelasId: string;
  idUsuario: number;
}
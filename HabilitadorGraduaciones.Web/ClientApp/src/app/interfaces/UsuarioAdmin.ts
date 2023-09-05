
export interface UsuarioAdmin {
  idUsuario?: number;
  nomina: string;
  nombre: string;
  correo: string;
  fechaCreacion?: Date;
  estatus?: boolean;
  rol?: string;
  cantidadRoles?:number;
  campus?: string;
  nivel?:string;
  sede?:string;
  fechaRegistro?: string;
  fechaModificacion?: string;
  usuarioModificacion?: string;
  listCampus?: Campus[];
  sedes?: Sede[];
  niveles?: Nivel[];
  roles?: any[];
  result?: boolean;
  errorMessage?: string;
  nuevo?: boolean;
}

export interface Campus {
  claveCampus: string;
  descripcion: string;
}

export interface Sede {
  claveCampus?: string;
  claveSede: string;
  descripcion?: string;
}
export interface Nivel {
  claveNivel: string;
  descripcion: string;
}

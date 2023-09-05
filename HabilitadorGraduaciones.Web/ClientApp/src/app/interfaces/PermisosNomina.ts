export interface PermisosNomina {
    idUsuario: number;
    nomina: string;
    ambiente: string;
    acceso: boolean;
    niveles: NivelesNomina[];
    campus: CampusNomina[];
    menu: PermisosMenu[];
    errorMessage: string;
    result: boolean;
  }

export interface NivelesNomina {
  claveNivel: string;
  descripcion: string;
  result: boolean;
}

export interface CampusNomina {
  claveCampus: string;
  descripcion: string;
  sedes: Sedes[];
  result: boolean;
}

export interface Sedes {
  claveSede: string;
  descripcion: string;
}

export interface PermisosMenu {
  idPermiso: number;
  idMenu: number;
  nombreMenu: string;
  pathMenu: string;
  iconoMenu: string;
  idSubMenu: number;
  nombreSubMenu: string;
  pathSubMenu: string;
  iconoSubMenu: string;
  seccion: string
  ver: boolean;
  editar: boolean;
  activa: boolean;
}
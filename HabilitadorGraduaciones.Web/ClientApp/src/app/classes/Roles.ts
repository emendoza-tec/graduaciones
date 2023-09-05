import { UsuarioAdmin } from "../interfaces/UsuarioAdmin";

export class Roles {
    idRol: number;
    descripcion: string;
    estatus: boolean;
    totalUsuarios: number;
    usuarios: UsuarioAdmin[];
    usuarioRegistro: string;
    fechaRegistro: Date;
    usuarioModifico: string;
    fechaModificacion: Date;
    activo: boolean;
    permisos: Permisos[];
    result: boolean;
    error: string;
    nuevo?: boolean;

    constructor(IdRol: number, Descripcion: string, Estatus: boolean, TotalUsuarios: number, Usuarios: UsuarioAdmin[], UsuarioRegistro: string, FechaRegistro: Date,
        UsuarioModifico: string, FechaModificacion: Date, Activo: boolean, Permisos: Permisos[], Result: boolean, Error: string, Nuevo?: boolean) {
        this.idRol = IdRol
        this.descripcion = Descripcion;
        this.estatus = Estatus;
        this.totalUsuarios = TotalUsuarios;
        this.usuarios = Usuarios;
        this.usuarioRegistro = UsuarioRegistro;
        this.fechaRegistro = FechaRegistro;
        this.usuarioModifico = UsuarioModifico;
        this.fechaModificacion = FechaModificacion;
        this.activo = Activo;
        this.permisos = Permisos;
        this.result = Result;
        this.error = Error;
        this.nuevo = Nuevo;
    }
}

export class Permisos {
    idPermiso: number;
    idMenu: number;
    nombreMenu: string;
    idSubMenu: number;
    nombreSubMenu: string;
    ver: boolean;
    editar: boolean;
    activa: boolean
    ok: boolean;
    error: string;

    constructor(IdPermiso: number, IdMenu: number, NombreMenu: string, IdSubMenu: number,
        NombreSubMenu: string, Ver: boolean, Editar: boolean, Activa: boolean, OK: boolean, Error: string) {
        this.idPermiso = IdPermiso;
        this.idMenu = IdMenu;
        this.nombreMenu = NombreMenu;
        this.idSubMenu = IdSubMenu;
        this.nombreSubMenu = NombreSubMenu;
        this.ver = Ver;
        this.editar = Editar;
        this.activa = Activa
        this.ok = OK;
        this.error = Error;
    }
}

export class SeccionesPermisos {
    idMenu: number;
    nombreMenu: string;
    idSubMenu: number;
    nombreSubMenu: string;
    ver: boolean;
    editar: boolean;
    result: boolean;

    constructor( IdMenu: number, NombreMenu: string, IdSubMenu: number, NombreSubMenu: string, Ver: boolean, Editar: boolean, Result: boolean){
        this.idMenu= IdMenu;
        this.nombreMenu= NombreMenu;
        this.idSubMenu= IdSubMenu;
        this.nombreSubMenu= NombreSubMenu;
        this.ver= Ver;
        this.editar= Editar;
        this.result= Result;
    }
}

export class Seccion{
    columna: string;
    seccionesHijos: Permisos[];
    constructor(Columna: string, SeccionesHijos: Permisos[]){
        this.columna = Columna;
        this.seccionesHijos =SeccionesHijos;
    }
}

export class usuarioRol{
    id: number;
    idRol: number;
    idUsuario: number;
    nombre: string;
    numeroNomina: string;
    roles: number

    constructor(Id: number, IdRol: number, IdUsuario: number, Nombre: string, NumeroNomina: string, Roles:number) {
        this.id= Id;
        this.idRol = IdRol;
        this.idUsuario = IdUsuario;
        this.nombre = Nombre;
        this.numeroNomina = NumeroNomina;
        this.roles = Roles;
    }
}
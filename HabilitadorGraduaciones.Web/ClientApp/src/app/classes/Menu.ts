export class Menu {
    id: number;
    nombre: string;
    path: string;
    icono: string;
    idMenu: number;
    activa: boolean;
    editar: boolean;
    ver: boolean;
    menuHijo?: MenuHijo[];

    constructor(id: number, nombre: string, path: string, icono: string, idMenu: number, activa: boolean, editar: boolean, ver: boolean, menuHijo: Menu[]) {
        this.id = id,
            this.nombre = nombre,
            this.path = path,
            this.icono = icono,
            this.idMenu = idMenu,
            this.activa = activa,
            this.editar = editar,
            this.ver = ver,
            this.menuHijo = menuHijo
    }
}

export class MenuHijo {
    id: number;
    nombre: string;
    path: string;
    icono: string;
    idMenu: number;
    activa: boolean;
    editar: boolean;
    ver: boolean;

    constructor(id: number, nombre: string, path: string, icono: string, idMenu: number, activa: boolean, editar: boolean, ver: boolean) {
        this.id = id,
            this.nombre = nombre,
            this.path = path,
            this.icono = icono,
            this.idMenu = idMenu,
            this.activa = activa,
            this.editar = editar,
            this.ver = ver
    }
}

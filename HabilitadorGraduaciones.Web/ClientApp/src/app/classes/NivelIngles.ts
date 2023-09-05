

export class NivelIngles {
    matricula: string;
    nivelIdiomaAlumno: string;
    requisitoNvl: string;
    cumpleNivel: boolean;
    fechaUltimaModificacion: Date;
    colorCumple: string;
    isCumple: boolean;



    constructor(matricula: string, nivelIdiomaAlumno: string, requisitoNvl: string, cumpleNivel: boolean, fechaUltimaModificacion: Date, colorCumple: string, isCumple: boolean) {

        this.matricula = matricula;
        this.nivelIdiomaAlumno = nivelIdiomaAlumno;
        this.requisitoNvl = requisitoNvl;
        this.cumpleNivel = cumpleNivel;
        this.fechaUltimaModificacion = fechaUltimaModificacion;
        this.colorCumple = colorCumple;
        this.isCumple = isCumple;

    }
}

export class DetalleNivelIngles{
    titulo: string;
    detalle: string;
    dudas: Duda;
    estadoDetalle: string;
    NivelIdiomaAlumno: string;
    requisitoNvl: string;


    isCUMPLE_NIVEL: boolean;

    constructor(titulo: string, detalle: string, dudas: Duda, isCUMPLE_NIVEL: boolean, estadoDetalle: string, NivelIdiomaAlumno: string, requisitoNvl: string
        ) {
        this.titulo = titulo;
        this.detalle = detalle;
        this.dudas = dudas;
        this.isCUMPLE_NIVEL = isCUMPLE_NIVEL;
        this.estadoDetalle = estadoDetalle;
        this.NivelIdiomaAlumno = NivelIdiomaAlumno;
        this.requisitoNvl = requisitoNvl;

      }
}

export class TablaProgramaIngles {
    programa: Programa[];
    constructor(programa: Programa[]) {
        this.programa = programa;
    }
}

export class Programa {
    programa: string;

    constructor(programa: string) {
        this.programa = programa;
    }
}

export class Duda {
    contactos: Contacto[];
    constructor(contactos: Contacto[]) {
        this.contactos = contactos;
    }
}

export class Contacto {
    nombre: string;
    correo: string;

    constructor(nombre: string, correo: string) {
        this.nombre = nombre;
        this.correo = correo;
    }
}

export class NivelInglesDataModal {
    constructor(public titulo?: string, public documentos?: any[], public detalle?: string, public estadoDetalle?:string, public nivelIdiomaAlumno?:string, public requisitoNvl?:string, public nivelIdiomaRequisito?:string, public dudas?: Duda, public isCUMPLE_NIVEL?: boolean) {
    }
}

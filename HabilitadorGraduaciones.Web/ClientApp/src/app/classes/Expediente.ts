
export class Expediente {
    estatus: string;
    detalle: string;
    ultimaActualizacion: Date;
    isCumple: boolean;
    colorCumple: string;

    constructor(estatus: string, detalle: string, ultimaActualizacion: Date, isCumple: boolean, colorCumple: string) {
        this.estatus = estatus;
        this.detalle = detalle;
        this.ultimaActualizacion = ultimaActualizacion;
        this.isCumple = isCumple;
        this.colorCumple = colorCumple;
    }
}

export class DetalleExpediente {
    titulo: string;
    documentos: Documento[];
    detalle: string;
    contacto: string;
    correo: string;
    estadoDetalle: string;
    isCumple: boolean;
    comentarios: Comentario[];
    link:string;
    linkName:string;

    constructor(titulo: string, documentos: Documento[], detalle: string, contacto: string, correo: string, isCumple: boolean, estadoDetalle: string, comentarios: Comentario[],link:string='',linkName:string='none') {
        this.titulo = titulo;
        this.documentos = documentos;
        this.detalle = detalle;
        this.contacto = contacto;
        this.correo = correo;
        this.isCumple = isCumple;
        this.estadoDetalle = estadoDetalle;
        this.comentarios = comentarios;
        this.link = link;
        this.linkName = linkName;
    }
}

export class Documento {
    descripcion: string;
    constructor(descripcion: string,) {
        this.descripcion = descripcion;
    }
}

export class Comentario {
    detalle: string;
    ultimaActualizacion: Date;
    constructor(detalle: string, ultimaActualizacion: Date) {
        this.detalle = detalle;
        this.ultimaActualizacion = ultimaActualizacion;
    }
}


export class ExpedienteDataModal {
    constructor(public titulo?: string, public documentos?: any[], public detalle?: string, public estadoDetalle?: string, public contacto?: string, public correo?: string, public isCumple?: boolean, public comentarios?:Comentario[], public link:string='', public linkName:string='none') {
    }
}

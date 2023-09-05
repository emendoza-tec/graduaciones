export class ProgramaBGB {
    matricula: string;
    cumpleRequisitoInglesCuarto: boolean;
    cumpleRequisitoInglesQuinto: boolean;
    cumpleRequisitoInglesOctavo: boolean;
    cumpleProgramaInternacional: boolean;
    ultimaActualizacion: Date;
    cumpleRequisitosProgramaEspecial: boolean;
    creditosCursadosExtranjero: number;
    colorCumple: string;
    isCumple: boolean;


    constructor(matricula: string, cumpleRequisitoInglesCuarto: boolean, cumpleRequisitoInglesQuinto: boolean, cumpleRequisitoInglesOctavo:boolean,
        cumpleProgramaInternacional: boolean, cumpleRequisitosProgramaEspecial: boolean, colorCumple:string, isCumple: boolean, ultimaActualizacion: Date, 
        creditosCursadosExtranjero: number) {
        this.matricula = matricula;
        this.cumpleRequisitoInglesCuarto = cumpleRequisitoInglesCuarto;
        this.cumpleRequisitoInglesQuinto = cumpleRequisitoInglesQuinto;
        this.cumpleRequisitoInglesOctavo = cumpleRequisitoInglesOctavo;
        this.cumpleProgramaInternacional = cumpleProgramaInternacional;
        this.ultimaActualizacion = ultimaActualizacion;
        this.cumpleRequisitosProgramaEspecial = cumpleRequisitosProgramaEspecial;
        this.colorCumple = colorCumple;
        this.isCumple = isCumple;
        this.creditosCursadosExtranjero = creditosCursadosExtranjero;
    }
}

export class DetalleProgramaBGB{
    titulo: string;
    detalle: string;
    dudas: Duda;
    estadoDetalle: string;
    NivelIdiomaAlumno: string;

    isCUMPLE_NIVEL: boolean;

    constructor(titulo: string, detalle: string, dudas: Duda, isCUMPLE_NIVEL: boolean, estadoDetalle: string, NivelIdiomaAlumno: string) {
        this.titulo = titulo;
        this.detalle = detalle;
        this.dudas = dudas;
        this.isCUMPLE_NIVEL = isCUMPLE_NIVEL;
        this.estadoDetalle = estadoDetalle;
        this.NivelIdiomaAlumno = NivelIdiomaAlumno;
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

export class ProgramaBGBDataModal {
    constructor(public titulo?: string, public documentos?: any[], public detalle?: string, public estadoDetalle?:string, public nivelIdiomaAlumno?:string, public nivelIdiomaRequisito?:string, public dudas?: Duda, public isCUMPLE_NIVEL?: boolean) {
    }
}
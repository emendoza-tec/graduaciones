import { MatTableDataSource } from "@angular/material/table";

export class Solicitud {
  IdSolicitud: number;
  Matricula: string;
  PeriodoGraduacion: string;
  IdDatosPersonales: number;
  Descripcion: string;
  FechaSolicitud: Date;
  UltimaActualizacion: Date;
  IdEstatusSolicitud: number;
  Estatus: string;
  numeroSolicitud: number;
}

export class DetalleSolicitud {
  IdSolicitud: number;
  IdDetalleSolicitud: number;
  IdDatosPersonales: number;
  Descripcion: string;
  FechaSolicitud: Date;
  UltimaActualizacion: Date;
  IdEstatusSolicitud: number;
  Estatus: string;
  DatoIncorrecto: string;
  DatoCorrecto: string;
  Documento: string;
  Extension: string;
  AzureStorage: string; 
}

export class EstatusSolicitud {
  IdDatosPersonales: number;
  Descripcion: string;
}

export class GuardaSolicitud {
  Matricula: string;
  PeriodoGraduacion: string;
  IdSolicitud: number;
  IdDetalleSolicitud: number;
  IdDatosPersonales: number;
  Descripcion: string;
  FechaSolicitud: Date;
  UltimaActualizacion: Date;
  IdEstatusSolicitud: number;
  Estatus: string;
  DatoIncorrecto: string;
  DatoCorrecto: string;
  Ok: boolean;
  Error: string;
  Detalle?: GuardaDetalleSolicitud[] | MatTableDataSource<GuardaDetalleSolicitud>;
}


export class GuardaDetalleSolicitud {
  IdDetalleSolicitud: number;
  IdSolicitud: number;
  Archivo: string;
  Documento: string;
  Extension: string;
  AzureStorage: string; 
  Ok: boolean;
  Error: string;
}


export class ModificaSolicitud {
  IdSolicitud: number;
  Matricula: string;
  IdEstatusSolicitud: number;
  UsarioRegistro: string;
  Comentarios: string;
  IdCorreo: number;
  Ok: boolean;
  Error: string;
}

export class TotalSolicitudesDto {
  Total: number;
}
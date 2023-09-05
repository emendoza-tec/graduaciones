export interface Card {
  Id: number;
  icono: string;
  titulo: string;
  descripcion: string;
  list: Lista[];
  ultimaActualizacion: Date;
  isCumple: boolean;
  color: string;
  orden: number;
  aplica: boolean;
}

export interface Lista {
  nombre: string;
  valor: string;
}

export interface icon {
  Id: number;
  Nombre: string;
}



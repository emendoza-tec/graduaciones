export class Distinciones {
  constructor(
    lstConcentracion: string[]
    ,diploma: string
    , ulead: string
    , diplomaOk: boolean
    , uleadOk: boolean
    , hasUlead: boolean
    , hasDiploma: boolean
    , hasConcentracion: boolean
  ) {
    this.lstConcentracion = lstConcentracion;
    this.diploma = diploma;
    this.ulead = ulead;
    this.diplomaOk = diplomaOk;
    this.uleadOk = uleadOk;
    this.hasUlead = hasUlead;
    this.hasDiploma = hasDiploma;
    this.hasConcentracion = hasConcentracion;
  }
  lstConcentracion: string[];
  diploma: string;
  diplomaOk: boolean;
  diplomaIcon: string | undefined;
  ulead: string;
  uleadOk: boolean;
  uleadIcon: string | undefined;
  hasUlead: boolean;
  hasDiploma: boolean;
  hasConcentracion: boolean;
}



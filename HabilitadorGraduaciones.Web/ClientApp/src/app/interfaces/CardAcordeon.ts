export interface CardAcordeon {
    id: number;
    titulo: String;
    lista: AcordeonList[];
}
interface AcordeonList{
    id: number,
    iconos:  String[];
    elemento: String;
    valor: String;
    isCumple: Boolean;
}
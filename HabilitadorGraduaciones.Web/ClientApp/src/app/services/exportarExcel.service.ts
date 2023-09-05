import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AnalisisExamenIntegrador, AnalisisExpediente, ProcesaExamenIntegrador, ProcesaExpediente } from '../classes/Procesa';
import { EnumTipoTemplate } from '../enums/EnumTipoTemplate';
import { UsuarioAdmin } from '../interfaces/UsuarioAdmin';

const EXCEL_EXTENSION = '.xlsx';

@Injectable({ providedIn: 'root' })

export class ExcelService {

  constructor() { /* TODO document why this constructor is empty */  }

  public exportarExcel(datosJson: any, nombreArchivo: string): void {
    import('xlsx').then(xlsx => {
      const hojadeCalculo: import('xlsx').WorkSheet = xlsx.utils.json_to_sheet(datosJson);
      const libro: import('xlsx').WorkBook = xlsx.utils.book_new();
      const headers = [['Nómina', 'Nombre', 'Correo', 'Roles', 'Campus']];
      xlsx.utils.sheet_add_aoa(hojadeCalculo, headers);
      xlsx.utils.book_append_sheet(libro, hojadeCalculo, nombreArchivo);
      xlsx.writeFile(libro, nombreArchivo, datosJson)
    });
  }

  public exIntExcelToJson(file: File): Observable<AnalisisExamenIntegrador> {
    return new Observable(obs => {
      const reader: FileReader = new FileReader();

      reader.onload = (e: any) => {
        import('xlsx').then(xlsx => {
          const binarystr: string = e.target.result;
          const wb: import('xlsx').WorkSheet = xlsx.read(binarystr, { type: 'binary' });
  
          const wsname: string = wb.SheetNames[0];
          const ws: import('xlsx').WorkSheet = wb.Sheets[wsname];
          const dataSheet = xlsx.utils.sheet_to_json(ws, { raw: true, defval: null });
          const range = xlsx.utils.decode_range("A1:XFD1");
          const headersNameJSON: any[] = xlsx.utils.sheet_to_json(ws, { defval: '', range, header: 1 });
          const headerList: any[] = [];
          headersNameJSON[0].forEach((element: any) => {
            if (element) {
              headerList.push(element);
            }
          });
  
          if (headerList[0] !== EnumTipoTemplate.Matricula || headerList[1] !== EnumTipoTemplate.PeriodoGraduacion || headerList[2] !== EnumTipoTemplate.Nivel || headerList[3] !== EnumTipoTemplate.NombreRequisito || headerList[4] !== EnumTipoTemplate.Estatus || headerList[5] !== EnumTipoTemplate.FechaExamen || headerList.length > 6) {
            obs.error(new Error("Carga de archivo de estatus de requisito Examen integrador. No coinciden las columnas"));
          }
  
          let data = new AnalisisExamenIntegrador(0, []);
          data.totalRegistros = dataSheet.length;
  
          dataSheet.forEach((element: any) => {
            let procesaExamenIntegrador: ProcesaExamenIntegrador = new ProcesaExamenIntegrador(element.Matricula, element.PeriodoGraduacion, element.Nivel, element.NombreRequisito, element.Estatus, element.FechaExamen);
            data.procesaExamenIntegrador.push(procesaExamenIntegrador);
          });
          obs.next(data);
          obs.complete();
        });
      };
      reader.readAsBinaryString(file);
    });
  }

  public expExcelToJson(file: File): Observable<AnalisisExpediente> {
    return new Observable(obs => {

      const reader: FileReader = new FileReader();

      reader.onload = (e: any) => {
        import('xlsx').then(xlsx => {
          const binarystr: string = e.target.result;
          const wb: import('xlsx').WorkSheet = xlsx.read(binarystr, { type: 'binary' });
  
          const wsname: string = wb.SheetNames[0];
          const ws: import('xlsx').WorkSheet = wb.Sheets[wsname];
  
          const dataSheet = xlsx.utils.sheet_to_json(ws);
          const range = xlsx.utils.decode_range("A1:C1");
          const headersNameJSON = xlsx.utils.sheet_to_json(ws, { defval: '', range, header: 1 });
          const headerList = JSON.stringify(headersNameJSON[0]);
          const isTemplate = headerList.includes('Matricula' && 'Estatus' && 'Mensaje');
          if (!isTemplate) {
            obs.error(new Error("Por favor descarga el template correcto"));
          }
  
          let data = new AnalisisExpediente(0, []);
          data.totalRegistros = dataSheet.length;
          dataSheet.forEach((element: any) => {
            let procesaExpediente = new ProcesaExpediente(element.Matricula, element.Estatus, element.Mensaje);
            data.procesaExpediente.push(procesaExpediente);
          });
          obs.next(data as AnalisisExpediente);
          obs.complete();
        });
      };
      reader.readAsBinaryString(file);
    });
  }
  public usuarioExcelToJson(file: File): Observable<UsuarioAdmin[]> {
    return new Observable(obs => {

      const reader: FileReader = new FileReader();

      reader.onload = (e: any) => {
        import('xlsx').then(xlsx => {
          const binarystr: string = e.target.result;
          const wb: import('xlsx').WorkSheet = xlsx.read(binarystr, { type: 'binary' });
  
          const wsname: string = wb.SheetNames[0];
          const ws: import('xlsx').WorkSheet = wb.Sheets[wsname];
  
          const dataSheet = xlsx.utils.sheet_to_json(ws);
          const range = xlsx.utils.decode_range("A1:F1");
          const headersNameJSON = xlsx.utils.sheet_to_json(ws, { defval: '', range, header: 1 });
          const headerList = JSON.stringify(headersNameJSON[0]);
          const isTemplate = headerList.includes('Nomina' && 'Correo' && 'Campus' && 'Sede' && 'Nivel' && 'Rol');
          if (!isTemplate) {
            let data: UsuarioAdmin[] = [];
            let usuario: UsuarioAdmin = {nomina: 'Template Incorreco ', nombre: '', correo: '', estatus : false};
            data.push(usuario);
            obs.next(data as UsuarioAdmin[] );
          }else{
            let data: UsuarioAdmin[] = [];
            dataSheet.forEach((element: any) => {
              let usuario: UsuarioAdmin = {nomina: element.Nomina, nombre: '', correo: element.Correo, campus: element.Campus, sede: element.Sede, nivel : element.Nivel, rol: element.Rol, estatus : true};
              data.push(usuario);
            });
            obs.next(data as UsuarioAdmin[] );
            obs.complete();
          }
  
          
        });
      };
      reader.readAsBinaryString(file);
    });
  }
}

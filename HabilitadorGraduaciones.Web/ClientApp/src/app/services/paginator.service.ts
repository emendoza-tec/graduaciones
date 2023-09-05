import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })

export class PaginatorService {

  constructor() { /* TODO document why this constructor is empty */  }

  public obtenPageSizeOptions(lenghtTable: number, historial:boolean ): number[] {
    //length, pagesize, pageSizeOptions
    let pageSizeArray: number[] = [];
    if (historial) {
      pageSizeArray = [3, 5, 10, 15];
    } else {
      pageSizeArray = [15, 30, 50, 100];
    }
    let returnPageSizeOptions: number[] = [];
    const indexPageOptions = pageSizeArray.findIndex(f => f >= lenghtTable);

    switch (indexPageOptions) {
      case -1:
        returnPageSizeOptions = pageSizeArray;
        break;
      case 0:
        returnPageSizeOptions.push(lenghtTable);
        break;
      default:
        returnPageSizeOptions = pageSizeArray.filter((f, index) => index <= indexPageOptions)
        break;
    }

    return returnPageSizeOptions;
  }
}

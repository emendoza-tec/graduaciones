import { Injectable } from '@angular/core';
import { MatPaginatorIntl, _MatPaginatorBase } from '@angular/material/paginator';

@Injectable()
export class MatPaginatorIntlCro extends MatPaginatorIntl {
  itemsPerPageLabel = 'Filas por Página';
  nextPageLabel = 'siguiente';
  lastPageLabel = 'ultimo';
  previousPageLabel = 'atrás';
  firstPageLabel = 'primero';

  getRangeLabel = (page: number, pageSize: number, length: number) => {
    //if(length  < pageSizeOptions[0])
    if (length === 0 || pageSize === 0) {
      return '0 de ' + length;
    }

    length = Math.max(length, 0);

    const startIndex = page * pageSize;
    const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;

    return startIndex + 1 + ' - ' + endIndex + ' filas de ' + length;

  };
}


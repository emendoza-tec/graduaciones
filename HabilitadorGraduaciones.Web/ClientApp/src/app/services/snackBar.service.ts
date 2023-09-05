import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { SnackbarComponent } from "../shared/components/snackBar/snackBar.component";

@Injectable({
    providedIn: 'root'
})

export class SnackBarService {
    messageText: any[] = [];
    constructor(
        public snackBar: MatSnackBar,
    ) {
    }
    public openSnackBar(message: string, type: string, duration?: any, verticalPosition?: any, horizontalPosition?: any) {
        const _snackType = type !== undefined ? type : 'success';
        let classType = '';
        switch (type) {
            case 'success':
                classType = 'snackBarSuccess';
                break;
            case 'error':
                classType = 'snackBarError';
                break;
            case 'warn':
                classType = 'snackBarWarning';
                break;
            case 'info':
                classType = 'snackBarWarning';
                break;
            case 'default': {
                //statements; 
                classType = 'snackBarDefault';
                break;
            }
        }

        this.snackBar.openFromComponent(SnackbarComponent, {
            duration: duration || 5000,
            panelClass: [classType],
            horizontalPosition: horizontalPosition || 'end',
            verticalPosition: verticalPosition || 'top',
            data: { message: message, snackType: _snackType, snackBar: this.snackBar }
        });
    }

}

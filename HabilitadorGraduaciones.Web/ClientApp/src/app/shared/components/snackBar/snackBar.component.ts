import { Component, Inject, OnInit } from "@angular/core";
import { MAT_SNACK_BAR_DATA } from "@angular/material/snack-bar";

@Component({
    selector: 'my-snackbar',
    templateUrl: './snackbar.component.html'
})

export class SnackbarComponent implements OnInit {

    constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) {
    }

    ngOnInit() { }

    get getIcon() {
        switch (this.data.snackType) {
            case 'success':
                return { type: this.data.snackType, icon: 'check' };
            case 'error':
                return { type: this.data.snackType, icon: 'error' };
            case 'warn':
                return { type: this.data.snackType, icon: 'warning_outline' };
            case 'info':
                return { type: this.data.snackType, icon: 'info' };
                
        }
        return { type: this.data.snackType, icon: '' };
    }

    closeSnackbar() {
        this.data.snackBar.dismiss();
    }

}
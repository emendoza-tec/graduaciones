import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Roles } from '../classes/Roles';

export function rolValidator(roles: Roles[]): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

        const value = control.value;
        let existeRol: boolean = false;

        if (!value) {
            return null;
        }
        const filtraRoles = roles.filter(rol=>rol.descripcion.toLowerCase() == value.toLowerCase());
        existeRol = filtraRoles.length > 0;
        return existeRol ? { rolExistente: true } : null;
    }
}


export function noWhitespaceValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        return (control.value === '' || control.value === null) ? { 'whitespace': true } : null;       
    }
    
}

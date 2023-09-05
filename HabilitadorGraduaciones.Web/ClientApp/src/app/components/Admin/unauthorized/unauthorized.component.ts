import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent  {
  constructor(private authService: AuthService) {  }
  sinAcceso() : void{
    this.authService.cerrarSesion();
  }
}

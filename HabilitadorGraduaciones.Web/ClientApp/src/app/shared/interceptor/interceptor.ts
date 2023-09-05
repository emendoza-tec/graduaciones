import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { AuthService } from "src/app/services/auth-service.service";

  
  @Injectable()
  export class AuthorizationInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) {}
    contador: number = 1;
    intercept(
      req: HttpRequest<any>,
      next: HttpHandler
    ): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status == 401 && this.contador == 1) {
            this.contador += 1;
            this.authService.actualizarSesion();
          }
          return throwError(error);
        })
      );
    }
  }

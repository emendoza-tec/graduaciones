import { HttpClient, HttpParams } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  url: string = "";
  urlHome: string = "";
  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.url = baseUrl + "Auth/Login";
    this.urlHome = baseUrl + "Home";
  }

  cerrarSesion() {
    sessionStorage.clear();
    window.location.href = "Auth/Logout";
  }

  cerrarSesionPRD() {
    return this.http.post(`${this.url}/SingleLogout`, {});
  }

  actualizarSesion(): void {
    sessionStorage.clear();
    window.location.reload();
  }
}

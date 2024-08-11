import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { loginForm } from '../../Models/loginForm';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5263/api';
  constructor(private http: HttpClient) { }
  
  user: any = null;

  loadUser() {
    const request = this.http.get<any>("/api/user");

    request.subscribe(user => this.user =user)
    
   }
  
  login(model:loginForm) :Observable<any>{ 
    return this.http.post<any>(`${this.apiUrl}/login`, model, { withCredentials: true });

  }
  
  register(){}

  logout() {
    
    return this.http.get("/api/logout").subscribe(_=>this.user=null)

  }


}

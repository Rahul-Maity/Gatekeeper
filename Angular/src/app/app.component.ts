import { Component } from '@angular/core';
import { AuthService } from './shared/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers:[AuthService]
})
export class AppComponent {
  title = 'Angular';
  constructor(private auth: AuthService) { }
  
  logout() {
    return this.auth.logout();
  }
}

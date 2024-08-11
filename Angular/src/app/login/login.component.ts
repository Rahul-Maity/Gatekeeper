import { Component } from '@angular/core';
import { AuthService } from '../shared/auth.service';
import { loginForm } from '../../Models/loginForm';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [AuthService]
})
export class LoginComponent {



  loginform: loginForm = { username: '', password: '' };
  constructor(private auth: AuthService) { }


  onSubmit() {
    this.auth.login(this.loginform).subscribe(
      (res) => {
        console.log('login suc', res);
      },
      (err) => {
        console.log('failed', err);

      }
    )
  }
}

// auth.component.ts
import { Component } from '@angular/core';
import { LoginUser } from '../../models/login-user';
import { ApiResponse } from '../../models/api-reponse';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { RegisterUser } from '../../models/register-user';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule
  ],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isToggleLogin = true;
  isDarkMode = false;
  showPassword = false;
  showConfirmPassword = false;

  loginUser: LoginUser = {
    userEmail: "",
    password: "",
  };

  registerUser: RegisterUser = {
    email: "",
    password: "",
    retype_password: "",
    firstName: "",
    lastName: "",
  }

  constructor(private authService: AuthService, private router: Router) {}

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  login(): void {
    this.authService.login(this.loginUser).subscribe({
      next: (response: ApiResponse<any>) => {
        const token = response.data;
        alert("Logged in successfully");
        localStorage.setItem("auth_token", token);
        this.router.navigate(['/dashboard']);
      },
      error: err => {
        alert(err.error?.message || 'An error occurred during login.');
      }
    });
  }

  signUp(): void {
    if (this.registerUser.password === this.registerUser.retype_password) {
      this.authService.signUp(this.registerUser).subscribe({
        next: (response: ApiResponse<any>) => {
          alert("SignUp Successfully");
          this.isToggleLogin = true;
        },
        error: err => {
          alert(err.error?.message || 'An error occurred during sign up.');
        }
      });
    } else {
      alert("Passwords don't match");
    }
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}
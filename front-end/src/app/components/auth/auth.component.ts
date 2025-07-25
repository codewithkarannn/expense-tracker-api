// auth.component.ts
import { Component, inject } from '@angular/core';
import { LoginUser } from '../../models/login-user';
import { ApiResponse } from '../../models/api-reponse';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { RegisterUser } from '../../models/register-user';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button'; 
import { ToastServiceService } from '../../services/toast-service.service';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NotificationService } from '../../services/notification-service';
@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    NzButtonModule,
    NzFormModule,
    NzInputModule, 
    NzPageHeaderModule,
    NzIconModule,
    NzTabsModule
  ],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isToggleLogin = true;
  isDarkMode = false;
  showPassword = false;
  showConfirmPassword = false;
  isLoading = false;
  


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

private notificationService =  inject(NotificationService);

  constructor(private authService: AuthService, 
    private router: Router,
  private toast : ToastServiceService) {}

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  login(): void {
    this.isLoading = true;
    this.authService.login(this.loginUser).subscribe({
      next: (response: ApiResponse<any>) => {
        const token = response.data;
      
        localStorage.setItem("auth_token", token);
        this.router.navigate(['/dashboard']);
        this.isLoading = false;
        this.notificationService.success('Success', 'Login successful!');
      },
      error: err => {
        this.notificationService.error('Error', err.message || 'An error occurred during login.');
        this.isLoading = false;
      }
    });
  }


  
  signUp(): void {
      this.isLoading = true;
    if (this.registerUser.password === this.registerUser.retype_password) {
      this.authService.signUp(this.registerUser).subscribe({
        next: (response: ApiResponse<any>) => {
        
          this.isToggleLogin = true;
          this.isLoading = false; 
          this.notificationService.success('Success', 'Account created successfully! You can now log in.');
          this.router.navigate(['/login']);
        },
        error: err => {

          this.notificationService.error('Error', err.error?.message || 'An error occurred during sign up.');
          this.isLoading = false;
        }
      });
    } else {
        this.notificationService.error('Error', 'Passwords do not match');
        this.isLoading = false;
    }
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}
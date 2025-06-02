import { Component } from '@angular/core';
import { Toast, ToastServiceService, ToastType } from '../../services/toast-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent {
  toast: Toast | null = null;

  constructor(private toastService: ToastServiceService) {
    this.toastService.toast$.subscribe(toast => {
      this.toast = toast;
    });
  }

  get toastClasses(): string {
    return 'flex items-center w-full max-w-xs p-4 mb-4 text-gray-500 bg-white rounded-lg shadow-sm animate-fadeInUp';
  }

  get iconClasses(): string {
    if (!this.toast) return '';

    const baseClasses = 'inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg';

    const typeClasses = {
      success: 'text-green-500 bg-green-100',
      error: 'text-red-500 bg-red-100',
      warning: 'text-yellow-500 bg-yellow-100',
      info: 'text-blue-500 bg-blue-100'
    };

    return `${baseClasses} ${typeClasses[this.toast.type]}`;
  }

  // Add this to your close method
  close() {
    const toastElement = document.querySelector('.animate-fadeInUp');
    if (toastElement) {
      toastElement.classList.remove('animate-fadeInUp');
      toastElement.classList.add('animate-fadeOutUp');

      setTimeout(() => {
        this.toast = null;
      }, 300);
    }
  }
}
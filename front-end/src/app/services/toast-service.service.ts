import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ToastType = 'success' | 'error' | 'warning' | 'info';
export interface Toast {
  message: string;
  type: ToastType;
  duration?: number;
}
@Injectable({
  providedIn: 'root'
})



export class ToastServiceService {
  private toastSubject = new BehaviorSubject<Toast | null>(null);
  toast$ = this.toastSubject.asObservable();

  show(toast: Toast) {
    this.toastSubject.next(toast);
    console.log('Toast shown:', toast);
    // Auto hide after duration or default 5 seconds
    const duration = toast.duration || 5000;
    setTimeout(() => this.hide(), duration);
  }

  hide() {
    this.toastSubject.next(null);
  }
}

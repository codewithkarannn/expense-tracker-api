import { Injectable } from '@angular/core';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private placement: NzNotificationPlacement = 'top';

  constructor(private notification: NzNotificationService) {}

  show(
    type: string,
    title: string,
    message: string,
    placement: NzNotificationPlacement = this.placement
  ): void {
    this.notification.create(type, title, message, { nzPlacement: placement });
  }

  success(title: string, message: string, placement?: NzNotificationPlacement): void {
    this.show('success', title, message, placement);
  }

  error(title: string, message: string, placement?: NzNotificationPlacement): void {
    this.show('error', title, message, placement);
  }

  info(title: string, message: string, placement?: NzNotificationPlacement): void {
    this.show('info', title, message, placement);
  }

  warning(title: string, message: string, placement?: NzNotificationPlacement): void {
    this.show('warning', title, message, placement);
  }

  blank(title: string, message: string, placement?: NzNotificationPlacement): void {
    this.notification.blank(title, message, { nzPlacement: placement ?? this.placement });
  }
}
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private toastr: ToastrService) {}

  error(message: string, title?: string): void {
    this.toastr.error(message, title);
  }

  warning(message: string, title?: string): void {
    this.toastr.warning(message, title);
  }
} 
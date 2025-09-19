import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class HttpNotificationInterceptorService implements HttpInterceptor {
  constructor(private toastr: ToastrService, private injector: Injector) { }

  private get translate() {
    return this.injector.get(TranslateService)
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap({
        next: (event) => {
          if (event instanceof HttpResponse) {
            //skip get requests
            if (req.method !== 'GET') {
              this.toastr.success(this.translate.instant('NOTIFICATIONS.REQUEST_COMPLETED_SUCCESSFULLY'),
                this.translate.instant('NOTIFICATIONS.SUCCESS'));
            }
          }
        },
        error: (error) => {
          if (error instanceof HttpErrorResponse) {
            // Delegate error toast to ErrorHandlingInterceptor; do not show here
            // no-op
          }
        }
      })
    );
  }
}

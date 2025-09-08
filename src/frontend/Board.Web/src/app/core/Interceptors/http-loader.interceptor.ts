import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { LoaderService } from 'src/app/services/loader.service';

@Injectable()
export class HttpLoaderInterceptor implements HttpInterceptor {
  private requestCount = 0;

  constructor(private loaderService: LoaderService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.showLoader();
    this.requestCount++;
    
    return next.handle(req).pipe(
      catchError((err) => {
        this.removeRequest();
        return throwError(() => err);
      }),
      tap(() => {
        this.removeRequest()
      })
    );
  }

  private removeRequest() {
    this.requestCount--;

    if (this.requestCount < 0) {
      this.requestCount = 0;
    }

    if (this.requestCount == 0) {
      this.hideLoader();
    }
  }
  private showLoader(): void {
    this.loaderService?.show();
  }

  private hideLoader(): void {
    this.loaderService?.hide();
  }
}

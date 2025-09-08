import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
    constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let userLanguage: string = "en";

    // Only access localStorage if we're in the browser (not server-side)
    if (isPlatformBrowser(this.platformId)) {
      userLanguage = localStorage.getItem('user-language') ?? "en";
    }
    
    // If language is found, clone the request and set the header
    if (userLanguage) {
      const clonedRequest = req.clone({
        setHeaders: {
          'User-Language': userLanguage
        }
      });

      return next.handle(clonedRequest);
    }

    // If no language is found in local storage, just proceed with the original request
    return next.handle(req);
  }
}

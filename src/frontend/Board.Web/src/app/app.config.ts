import {
  ApplicationConfig,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { provideNativeDateAdapter } from '@angular/material/core';
import { provideAnimationsAsync as provideAnimationsAsyncMaterial } from '@angular/platform-browser/animations/async';
import { environment } from './environments/environment';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';
import { UnauthorizedInterceptor } from './core/Interceptors/unathorized.interceptor';
import { HttpLoaderInterceptor } from './core/Interceptors/http-loader.interceptor';
import { LanguageInterceptor } from './core/Interceptors/language.interceptor';
import { HttpNotificationInterceptorService } from './core/Interceptors/http-notification-interceptor-service';

export function HttpLoaderFactory(http: HttpClient) {
  // return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authHttpInterceptorFn]),
      withInterceptorsFromDi()
    ),
    provideClientHydration(withEventReplay()),
    provideNativeDateAdapter(),
    provideAnimationsAsyncMaterial(),
    provideAnimations(), // required animations providers
    provideToastr({
      preventDuplicates: true,
    }), // Toastr providers
    // provideTranslations(),
    provideAnimationsAsync(),
    //Auth
    provideAuth0({
      domain: environment.auth.domain,
      clientId: environment.auth.clientId,

      authorizationParams: {
        redirect_uri: window.location.origin,

        // Request this audience at user authentication time
        audience: environment.auth.audience,
        scope: 'openid profile email',
      },

    
      // Specify configuration for the interceptor
      httpInterceptor: {
        allowedList: [
          {
            // Match any request that starts 'https://localhost:7069' (note the asterisk)
            uri: environment.auth.access_token_uri,
            tokenOptions: {
              authorizationParams: {
                // The attached token should target this audience
                audience: environment.auth.audience,
                scope: 'openid profile email',
              },
            },
          },
        ],
      },
    }),

    //Interceptors
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true, 
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpLoaderInterceptor,
      multi: true, 
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpNotificationInterceptorService,
      multi: true, // this ensures you can have multiple interceptors if needed
    },
  ],
};
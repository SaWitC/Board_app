import {
  ApplicationConfig,
  provideZoneChangeDetection,
  importProvidersFrom,
  APP_INITIALIZER,
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
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UnauthorizedInterceptor } from './core/Interceptors/unathorized.interceptor';
import { HttpLoaderInterceptor } from './core/Interceptors/http-loader.interceptor';
import { LanguageInterceptor } from './core/Interceptors/language.interceptor';
import { HttpNotificationInterceptorService } from './core/Interceptors/http-notification-interceptor-service';

// translations are stored in assets/i18n/*.json

export function AppTranslateHttpLoaderFactory(http: HttpClient): TranslateLoader {
  return {
    getTranslation: (lang: string) => http.get<any>(`assets/i18n/${lang}.json`)
  } as TranslateLoader;
}

export function initializeTranslations(translate: TranslateService) {
  return () => {
    const isBrowser = typeof window !== 'undefined';
    const defaultLang = 'en';
    let lang = defaultLang;
    try {
      if (isBrowser) {
        lang = localStorage.getItem('user-language') ?? defaultLang;
        if (!localStorage.getItem('user-language')) {
          localStorage.setItem('user-language', lang);
        }
      }
    } catch {}

    translate.setDefaultLang(defaultLang);
    try {
      return firstValueFrom(translate.use(lang));
    } catch {
      return Promise.resolve();
    }
  };
}


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      ...(environment.auth.isBypassAuthorization ? [] : [withInterceptors([authHttpInterceptorFn])]),
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
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: AppTranslateHttpLoaderFactory,
          deps: [HttpClient]
        }
      })
    ),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeTranslations,
      deps: [TranslateService],
      multi: true,
    },
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
      ...(environment.auth.isBypassAuthorization
        ? { cacheLocation: 'localstorage', useRefreshTokens: true, authorizationParams: { ...{ redirect_uri: window.location.origin }, audience: environment.auth.audience, scope: 'openid profile email' } }
        : {}),
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

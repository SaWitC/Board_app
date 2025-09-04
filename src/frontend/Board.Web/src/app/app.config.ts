import {
  ApplicationConfig,
  EnvironmentProviders,
  importProvidersFrom,
  makeEnvironmentProviders,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
// import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
// import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app-routing.module';
// import { LanguageInterceptor } from './core/interceptors/language.interceptor';

export function HttpLoaderFactory(http: HttpClient) {
  // return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

// export const translationConfig = {
//   defaultLanguage: 'en',
//   loader: {
//     provide: TranslateLoader,
//     useFactory: HttpLoaderFactory,
//     deps: [HttpClient],
//   },
// };

// export function provideTranslations(): EnvironmentProviders {
//   return makeEnvironmentProviders([
//     importProvidersFrom(TranslateModule.forRoot(translationConfig)),
//   ]);
// }

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideClientHydration(withEventReplay()),
    provideAnimations(), // required animations providers
    provideToastr({
      preventDuplicates: true,
    }), // Toastr providers
    // provideTranslations(),
    provideAnimationsAsync(),
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: LanguageInterceptor,
    //   multi: true, // this ensures you can have multiple interceptors if needed
    // },
  ],
};

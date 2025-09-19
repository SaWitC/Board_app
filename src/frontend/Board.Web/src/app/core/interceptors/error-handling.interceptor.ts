import { Injectable, Injector } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { NotificationService } from 'src/app/core/services/other/notification.service';
import { TranslateService } from '@ngx-translate/core';

interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
}

interface ValidationError {
  name: string;
  reason: string;
}

interface ValidationProblemDetails extends ProblemDetails {
  errors?: ValidationError[];
}

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {
  constructor(private notification: NotificationService, private injector: Injector) {}

  private get translate() {
    return this.injector.get(TranslateService);
  }

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          const body: unknown = error.error;
          
          // Case 1: ValidationProblemDetails (400 + non-empty errors array)
          if (error.status === 400 && this.isValidationProblemDetails(body) && this.hasAnyValidationErrors(body.errors)) {
            const validationErrors = this.formatValidationErrors(body.errors!);
            this.notification.error(
              validationErrors,
              this.translate.instant('ERRORS.VALIDATION_TITLE')
            );
            return throwError(() => error);
          }
          
          // Case 2: Any other ProblemDetails
          const title = this.extractProblemTitle(body) ?? this.translate.instant('ERRORS.UNEXPECTED_ERROR');
          const errorTitle = this.getErrorTitle(error.status);
          this.notification.error(title, errorTitle);
          return throwError(() => error);
        }
        return throwError(() => error);
      })
    );
  }

  private isValidationProblemDetails(body: unknown): body is ValidationProblemDetails {
    return !!body && typeof body === 'object' && 'errors' in (body as ValidationProblemDetails);
  }

  private hasAnyValidationErrors(errors: ValidationProblemDetails['errors']): boolean {
    if (!errors) {
      return false;
    }
    return Array.isArray(errors) && errors.length > 0;
  }

  private extractProblemTitle(body: unknown): string | undefined {
    if (!body || typeof body === 'object') {
      return undefined;
    }
    const pd = body as ProblemDetails;
    return pd.title || pd.detail;
  }

  private getErrorTitle(status: number): string {
    switch (status) {
      case 401:
        return this.translate.instant('ERRORS.UNAUTHORIZED_TITLE');
      case 403:
        return this.translate.instant('ERRORS.FORBIDDEN_TITLE');
      case 404:
        return this.translate.instant('ERRORS.NOT_FOUND_TITLE');
      case 500:
        return this.translate.instant('ERRORS.SERVER_ERROR_TITLE');
      default:
        return this.translate.instant('ERRORS.ERROR_TITLE');
    }
  }

  private formatValidationErrors(errors: ValidationError[]): string {
    if (!errors || errors.length === 0) {
      return this.translate.instant('ERRORS.VALIDATION_FAILED');
    }

    const errorMessages = errors.map((error, index) => {
      return `${index + 1}. ${error.reason}`;
    });

    return errorMessages.join('\n');
  }
} 
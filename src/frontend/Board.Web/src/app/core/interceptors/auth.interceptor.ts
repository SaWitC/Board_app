import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	private getToken(): string | null {
		// TODO: Replace with real token retrieval from auth service/storage
		return localStorage.getItem('access_token');
	}

	intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		const token = this.getToken();
		const authReq = token
			? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
			: req;

		return next.handle(authReq).pipe(
			catchError((error: HttpErrorResponse) => {
				if (error.status === 401) {
					// handle unauthorized: optionally redirect to login
					console.warn('Unauthorized (401)');
				}
				if (error.status === 403) {
					console.warn('Forbidden (403)');
				}
				return throwError(() => error);
			})
		);
	}
} 
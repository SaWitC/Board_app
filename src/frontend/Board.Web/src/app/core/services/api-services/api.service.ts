import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
	private readonly _baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	public get<T>(url: string, params?: Record<string, string | number | boolean | undefined>): Observable<T> {
		const httpParams = this.createParams(params);
		return this.http.get<T>(`${this._baseUrl}${url}`, { params: httpParams });
	}

	public post<T>(url: string, body: unknown): Observable<T> {
		return this.http.post<T>(`${this._baseUrl}${url}`, body);
	}

	public put<T>(url: string, body: unknown): Observable<T> {
		return this.http.put<T>(`${this._baseUrl}${url}`, body);
	}

	public delete<T>(url: string): Observable<T> {
		return this.http.delete<T>(`${this._baseUrl}${url}`);
	}

	private createParams(params?: Record<string, string | number | boolean | undefined>): HttpParams | undefined {
		if (!params) {
			return undefined;
		}
		let httpParams = new HttpParams();
		for (const [key, value] of Object.entries(params)) {
			if (value !== undefined && value !== null) {
				httpParams = httpParams.set(key, String(value));
			}
		}
		return httpParams;
	}
}

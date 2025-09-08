import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { UserLookupDTO } from "src/app/models/user/user-lookup-DTO.model";

@Injectable({
  providedIn: 'root'
})
export class UsersApiService {

	constructor(private http: HttpClient) {}

	public searchUsers(search: string): Observable<UserLookupDTO[]> {
		const params = new HttpParams().set('q', search ?? '');
		return this.http.get<UserLookupDTO[]>(`${environment.apiUrl}/users`, { params });
	}
}

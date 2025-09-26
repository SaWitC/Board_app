import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { UserLookupDTO } from "../../models/user/user-lookup-DTO.model";
import { environment } from "src/environments/environment";

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

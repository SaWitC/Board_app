import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { environment } from "../../environments/environment";
import { UserLookupDTO } from "src/app/models/user/user-lookup-DTO.model";

@Injectable({
  providedIn: 'root'
})
export class UsersApiService {

  constructor(private http: HttpClient) {
  }

  // public searchUsers(search: string): Observable<UserLookupDTO[]> {
  //   return this.http.get<UserLookupDTO[]>(`${environment.apiUrl}/users/search?search=${search}`);
  // }

  public searchUsers(search: string): Observable<UserLookupDTO[]> {
    return of(
      [
        {
          id: '1',
          email: 'test1@test.com'
        },
        {
          id: '2',
          email: 'test2@test.com'
        }
      ]);
  }
}

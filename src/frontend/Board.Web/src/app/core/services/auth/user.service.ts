import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    //User auth0 permissions
    private permissionsSubject$$: BehaviorSubject<string[] | null> = new BehaviorSubject<string[] | null>(null);
    private currentUserEmail: string = "";

    constructor() { }

    public get getPermissions() {
        return this.permissionsSubject$$.value;
    }

    public getCurrentUserEmail(): string {
      return this.currentUserEmail;
    }

    public setCurrentUserEmail(email: string) {
      this.currentUserEmail = email;
    }

    public emitPermissionsChanged(permissions: string[]) {
        this.permissionsSubject$$.next(permissions)
    }

    public getPermissionsObservable$(): Observable<string[] | null> {
        return this.permissionsSubject$$.asObservable();
    }

    public hasAdminPermission(): boolean {
        return this.permissionsSubject$$.value?.includes('admin') ?? false;
    }
}


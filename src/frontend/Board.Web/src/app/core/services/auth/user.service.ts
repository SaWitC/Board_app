import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    //User board permissions
    private permissionsSubject$$: BehaviorSubject<string[] | null> = new BehaviorSubject<string[] | null>(null);

    constructor() { }

    public get getPermissions() {
        return this.permissionsSubject$$.value;
    }

    public emitPermissionsChanged(permissions: string[]) {
        this.permissionsSubject$$.next(permissions)
    }

    public getPermissionsObservable$(): Observable<string[] | null> {
        return this.permissionsSubject$$.asObservable();
    }

}


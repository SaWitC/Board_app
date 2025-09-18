import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { UserAccess } from "../../models/enums/user-access.enum";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    //User auth0 permissions
    private permissionsSubject$$: BehaviorSubject<string[] | null> = new BehaviorSubject<string[] | null>(null);
    private boardPermissionsSubject$$: BehaviorSubject<UserAccess | null> = new BehaviorSubject<UserAccess | null>(null);
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

    public hasGlobalAdminPermission(): boolean {
        return this.permissionsSubject$$.value?.includes('GlobalAdmin') ?? false;
    }

    public setUserBoardAccess(access: UserAccess){
      this.boardPermissionsSubject$$.next(access);
    }

     public isUserBoardUser(): boolean {
       return this.hasGlobalAdminPermission() || +(this.boardPermissionsSubject$$.value??0)>=+UserAccess.USER;
     }

     public isUserBoardAdmin(): boolean {
       return this.hasGlobalAdminPermission() || +(this.boardPermissionsSubject$$.value??0)>=+UserAccess.ADMIN;
     }

     public isUserBoardOwner(): boolean {
       return this.hasGlobalAdminPermission() || +(this.boardPermissionsSubject$$.value??0)>=+UserAccess.OWNER;
     }
}


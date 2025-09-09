import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

export interface LoaderState {
  show: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  private loaderSubject = new BehaviorSubject<LoaderState>({ show: false });
  private _isHiddenRequest = false;
  public loaderState = this.loaderSubject.asObservable();
  constructor() { }

  public show() {
    if (!this._isHiddenRequest)
      this.loaderSubject.next(<LoaderState>{ show: true });
    else
      this.loaderSubject.next(<LoaderState>{ show: false });
  }
  public hide() {
    this.loaderSubject.next(<LoaderState>{ show: false });
  }

  public hideRequest() {
    this._isHiddenRequest = true;

    setTimeout(() => {
      this._isHiddenRequest = false;
    }, 150);
  }
}

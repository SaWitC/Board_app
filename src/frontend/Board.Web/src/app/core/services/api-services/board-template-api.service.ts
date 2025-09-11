import { Injectable } from "@angular/core";
import { Observable, switchMap } from "rxjs";
import { ApiService } from "./api.service";
import {
    AddBoardTemplateDTO
} from "../../models/board-template/add-board-template-DTO.interface";

@Injectable({
  providedIn: 'root'
})
export class BoardTemplateServiceApi {
    constructor(private api: ApiService) {}
    public addBoardtTmplate(item: AddBoardTemplateDTO): Observable<AddBoardTemplateDTO> {
      console.log('add bt api invoked ', item)
		return this.api.post<AddBoardTemplateDTO>(`/boardtemplates`, item);
	}
}
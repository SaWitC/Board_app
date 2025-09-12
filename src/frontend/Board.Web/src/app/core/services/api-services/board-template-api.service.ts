import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "./api.service";
import {
    AddBoardTemplateDTO
} from "../../models/board-template/add-board-template-DTO.interface";
import { BoardTemplateDTO } from "../../models/board-template/board-template-DTO.interface";

@Injectable({
  providedIn: 'root'
})
export class BoardTemplateServiceApi {
    constructor(private api: ApiService) {}

    public addBoardtTmplate(item: AddBoardTemplateDTO): Observable<AddBoardTemplateDTO> {
      console.log('add bt api invoked ', item)
		return this.api.post<AddBoardTemplateDTO>(`/boardtemplates`, item);
	}

  public updateBoardTemplate(boardId: string, isActive: boolean): Observable<boolean> {
    return this.api.put<boolean>(`/boardtemplates/${boardId}`, { isActive });
  }

  public searchActiveBoardsTemplates(searchTerm: string): Observable<BoardTemplateDTO[]> {
    return this.api.get<BoardTemplateDTO[]>(`/boardtemplates/search?searchTerm=${searchTerm}`);
  }
}

import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {
  BoardColumnLookupDTO,
  BoardColumnDetailsDTO,
  AddBoardColumnDTO,
  UpdateBoardColumnDTO
} from "../../models";
import { ApiService } from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class BoardColumnApiService {

  constructor(private api: ApiService) {
  }

  public getBoardColumns(boardId: string): Observable<BoardColumnLookupDTO[]> {
    return this.api.get<BoardColumnLookupDTO[]>(`/boards/${boardId}/columns`);
  }

  public getBoardColumnById(boardId: string, boardColumnId: string): Observable<BoardColumnDetailsDTO> {
    return this.api.get<BoardColumnDetailsDTO>(`/boards/${boardId}/columns/${boardColumnId}`);
  }

  public addBoardColumn(boardId: string, column: AddBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    return this.api.post<BoardColumnDetailsDTO>(`/boards/${boardId}/columns`, column);
  }

  public updateBoardColumn(boardId: string, column: UpdateBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    return this.api.put<BoardColumnDetailsDTO>(`/boards/${boardId}/columns`, column);
  }

  public deleteBoardColumn(boardId: string, boardColumnId: string): Observable<boolean> {
    return this.api.delete<boolean>(`/boards/${boardId}/columns/${boardColumnId}`);
  }
}

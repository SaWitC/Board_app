import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import {
  BoardColumnLookupDTO,
  BoardColumnDetailsDTO,
  AddBoardColumnDTO,
  UpdateBoardColumnDTO
} from "../../models";

@Injectable({
  providedIn: 'root'
})
export class BoardColumnApiService {

  constructor(private http: HttpClient) {
  }

  public getBoardColumns(boardId: string): Observable<BoardColumnLookupDTO[]> {
    return this.http.get<BoardColumnLookupDTO[]>(`${environment.apiUrl}/boards/${boardId}/columns`);
  }

  public getBoardColumnById(boardId: string, columnId: string): Observable<BoardColumnDetailsDTO> {
    return this.http.get<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}`);
  }

  public addBoardColumn(boardId: string, column: AddBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    return this.http.post<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns`, column);
  }

  public updateBoardColumn(boardId: string, column: UpdateBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    return this.http.put<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns`, column);
  }

  public deleteBoardColumn(boardId: string, columnId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}`);
  }
}

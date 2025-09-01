import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import {
  BoardItemLookupDTO,
  BoardItemDetailsDTO,
  AddBoardItemDTO,
  UpdateBoardItemDTO
} from "../../models";

@Injectable({
  providedIn: 'root'
})
export class BoardItemApiService {

  constructor(private http: HttpClient) {
  }

  public getBoardItems(boardId: string, columnId: string): Observable<BoardItemLookupDTO[]> {
    return this.http.get<BoardItemLookupDTO[]>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}/items`);
  }

  public getBoardItemById(boardId: string, columnId: string, itemId: string): Observable<BoardItemDetailsDTO> {
    return this.http.get<BoardItemDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}/items/${itemId}`);
  }

  public addBoardItem(boardId: string, columnId: string, item: AddBoardItemDTO): Observable<BoardItemDetailsDTO> {
    return this.http.post<BoardItemDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}/items`, item);
  }

  public updateBoardItem(boardId: string, columnId: string, item: UpdateBoardItemDTO): Observable<BoardItemDetailsDTO> {
    return this.http.put<BoardItemDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}/items`, item);
  }

  public deleteBoardItem(boardId: string, columnId: string, itemId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}/items/${itemId}`);
  }

  public moveBoardItem(boardId: string, fromColumnId: string, toColumnId: string, itemId: string): Observable<boolean> {
    return this.http.put<boolean>(`${environment.apiUrl}/boards/${boardId}/columns/${fromColumnId}/items/${itemId}/move`, {
      targetColumnId: toColumnId
    });
  }
}

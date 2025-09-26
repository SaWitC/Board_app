import { Injectable } from "@angular/core";
import { Observable, switchMap } from "rxjs";
import { ApiService } from "./api.service";
import {
  BoardItemLookupDTO,
  BoardItemDetailsDTO,
  AddBoardItemDTO,
  UpdateBoardItemDTO
} from "../../models";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class BoardItemApiService {

	constructor(private api: ApiService) {}

	public getBoardItems(boardId: string): Observable<BoardItemLookupDTO[]> {
		return this.api.get<BoardItemLookupDTO[]>(`/boards/${boardId}/items`);
	}

  public deleteBoardItem(boardId: string, itemId: string): Observable<boolean> {
		return this.api.delete<boolean>(`/boards/${boardId}/items/${itemId}`);
	}

	public getBoardItemById(boardId: string, itemId: string): Observable<BoardItemDetailsDTO> {
		return this.api.get<BoardItemDetailsDTO>(`/boards/${boardId}/items/${itemId}`);
	}

	public addBoardItem(boardId: string, boardColumnId: string, item: AddBoardItemDTO): Observable<BoardItemDetailsDTO> {
		return this.api.post<BoardItemDetailsDTO>(`/boards/${boardId}/columns/${boardColumnId}/items`, item);
	}

	public updateBoardItem(boardId: string, boardColumnId: string, itemId: string, item: UpdateBoardItemDTO): Observable<BoardItemDetailsDTO> {
		return this.api.put<BoardItemDetailsDTO>(`/boards/${boardId}/columns/${boardColumnId}/items/${itemId}`, item);
	}

	public moveBoardItem(boardId: string, boardColumnId: string, itemId: string): Observable<BoardItemDetailsDTO> {
		return this.getBoardItemById(boardId, itemId).pipe(
			switchMap((item) => {
				const updatePayload: UpdateBoardItemDTO = {
					id: item.id,
					title: item.title,
					description: item.description,
				    boardColumnId: boardColumnId,
					priority: item.priority,
					assigneeEmail: item.assigneeEmail,
					dueDate: item.dueDate,
					taskType: item.taskType,
					tags: item.tags
				};
				return this.updateBoardItem(boardId, boardColumnId, itemId, updatePayload);
			})
		);
	}
}

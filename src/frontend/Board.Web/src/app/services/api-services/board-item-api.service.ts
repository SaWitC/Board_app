import { Injectable } from "@angular/core";
import { Observable, switchMap } from "rxjs";
import { ApiService } from "./api.service";
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

	constructor(private api: ApiService) {}

	public getBoardItems(): Observable<BoardItemLookupDTO[]> {
		return this.api.get<BoardItemLookupDTO[]>(`/boarditems`);
	}

	public getBoardItemById(itemId: string): Observable<BoardItemDetailsDTO> {
		return this.api.get<BoardItemDetailsDTO>(`/boarditems/${itemId}`);
	}

	public addBoardItem(item: AddBoardItemDTO): Observable<BoardItemDetailsDTO> {
		return this.api.post<BoardItemDetailsDTO>(`/boarditems`, item);
	}

	public updateBoardItem(itemId: string, item: UpdateBoardItemDTO): Observable<BoardItemDetailsDTO> {
		return this.api.put<BoardItemDetailsDTO>(`/boarditems/${itemId}`, item);
	}

	public deleteBoardItem(itemId: string): Observable<boolean> {
		return this.api.delete<boolean>(`/boarditems/${itemId}`);
	}

	public moveBoardItem(itemId: string, targetColumnId: string): Observable<BoardItemDetailsDTO> {
		return this.getBoardItemById(itemId).pipe(
			switchMap((item) => {
				const updatePayload: UpdateBoardItemDTO = {
					id: item.id,
					title: item.title,
					description: item.description,
					boardColumnId: targetColumnId,
					priority: item.priority,
					assigneeId: item.assigneeId,
					dueDate: item.dueDate,
				};
				return this.updateBoardItem(itemId, updatePayload);
			})
		);
	}
}

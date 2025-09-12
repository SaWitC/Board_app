import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { ApiService } from "./api.service";
import {
  BoardLookupDTO,
  BoardDetailsDTO,
  AddBoardDTO,
  UpdateBoardDTO
} from "../../models";

@Injectable({
  providedIn: 'root'
})
export class BoardApiService {

	constructor(private api: ApiService) {}

	public getBoards(): Observable<BoardLookupDTO[]> {
		return this.api.get<any[]>(`/boards`).pipe(
			map(items => items.map(i => this.mapBoardLookup(i)))
		);
	}

	public addBoard(board: AddBoardDTO): Observable<BoardDetailsDTO> {
		const payload = {
			title: board.title,
			description: board.description,
			boardUsers: board.boardUsers,
			boardColumns: board.boardColumns
		};
		return this.api.post<any>(`/boards`, payload).pipe(map(x => this.mapBoardDetails(x)));
	}

	public updateBoard(board: UpdateBoardDTO): Observable<BoardDetailsDTO> {
		const payload = { title: board.title, description: board.description, boardUsers: board.boardUsers, boardColumns: board.boardColumns };
		return this.api.put<any>(`/boards/${board.id}`, payload).pipe(map(x => this.mapBoardDetails(x)));
	}

	public getById(id: string): Observable<BoardDetailsDTO> {
		return this.api.get<any>(`/boards/${id}`).pipe(map(x => this.mapBoardDetails(x)));
	}

	public getBoardById(id: string): Observable<BoardDetailsDTO> {
		return this.getById(id);
	}

	public deleteBoard(id: string): Observable<boolean> {
		return this.api.delete<any>(`/boards/${id}`).pipe(map(() => true));
	}

	private mapBoardLookup(source: any): BoardLookupDTO {
		return {
			id: String(source.id),
			title: source.title,
			description: source.description,
			boardUsers: source.boardUsers ?? [],
			modificationDate: source.modificationDate ? new Date(source.modificationDate) : undefined,
			IsTemplate: source.IsTemplate,
			IsActiveTemplate: source.IsActiveTemplate
		};
	}

	private mapBoardDetails(source: any): BoardDetailsDTO {
		return {
			id: String(source.id),
			title: source.title,
			description: source.description,
			boardUsers: source.boardUsers ?? [],
			boardColumns: (source.boardColumns ?? []).map((c: any) => ({ id: String(c.id), title: c.title, description: c.description })),
			modificationDate: new Date(source.modificationDate),
			IsTemplate: source.IsTemplate,
			IsActiveTemplate: source.IsActiveTemplate
		};
	}
}

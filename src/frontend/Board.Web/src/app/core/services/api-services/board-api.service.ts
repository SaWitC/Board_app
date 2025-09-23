import { Injectable } from "@angular/core";
import { Observable, map, switchMap, of } from "rxjs";
import { ApiService } from "./api.service";
import {
  BoardLookupDTO,
  BoardDetailsDTO,
  AddBoardDTO,
  UpdateBoardDTO
} from "../../models";
import { UserAccess } from "../../models/enums/user-access.enum";
import { GetBoardsRequest } from "../../models/board/get-boards-request.interface";
import { PagedResult } from "../../models/common/paged-result.interface";


@Injectable({
  providedIn: 'root'
})
export class BoardApiService {

	constructor(private api: ApiService) {}

	public getBoards(request: GetBoardsRequest): Observable<PagedResult<BoardLookupDTO>> {
		const params: Record<string, string> = {};
		if (request.page) {
		  params['page'] = request.page.toString();
		}
		if (request.pageSize) {
		  params['pageSize'] = request.pageSize.toString();
		}
		if (request.titleSearchTerm) {
		  params['titleSearchTerm'] = request.titleSearchTerm;
		}
		if (request.ownerSearchTerm) {
		  params['ownerSearchTerm'] = request.ownerSearchTerm;
		}
	
		return this.api.get<PagedResult<BoardLookupDTO>>('/boards', params).pipe(
			map(result => ({
				...result,
				items: result.items.map(item => this.mapBoardLookup(item))
			}))
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
		const basePayload = { title: board.title, description: board.description, boardUsers: board.boardUsers, boardColumns: board.boardColumns };

		// If boardUsers is empty or undefined, preserve the current owner from server state
		if (!basePayload.boardUsers || basePayload.boardUsers.length === 0) {
			return this.getById(board.id).pipe(
				switchMap(current => {
					const owner = (current.boardUsers || []).find(u => u.role === UserAccess.OWNER);
					const payload = owner ? { ...basePayload, boardUsers: [owner] } : basePayload;
					return this.api.put<any>(`/boards/${board.id}`, payload).pipe(map(x => this.mapBoardDetails(x)));
				})
			);
		}

		return this.api.put<any>(`/boards/${board.id}`, basePayload).pipe(map(x => this.mapBoardDetails(x)));
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

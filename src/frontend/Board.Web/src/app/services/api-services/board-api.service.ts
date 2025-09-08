import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
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

  constructor(private http: HttpClient) {
  }


  public getBoards(): Observable<BoardLookupDTO[]> {
    return this.http.get<BoardLookupDTO[]>(`${environment.apiUrl}/boards`);
  }


  public addBoard(board: AddBoardDTO): Observable<BoardDetailsDTO> {
    return this.http.post<BoardDetailsDTO>(`${environment.apiUrl}/boards`, board);
  }

  public updateBoard(board: UpdateBoardDTO): Observable<BoardDetailsDTO> {
    return this.http.put<BoardDetailsDTO>(`${environment.apiUrl}/boards`, board);
  }

  public getById(id: string): Observable<BoardDetailsDTO> {
    return this.http.get<BoardDetailsDTO>(`${environment.apiUrl}/boards/${id}`);
  }

  public deleteBoard(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiUrl}/boards/${id}`);
  }


  // public getBoards(): Observable<BoardLookupDTO[]> {
  //   // Заглушка для тестирования
  //   const mockBoards: BoardLookupDTO[] = [
  //     {
  //       id: '1',
  //       title: 'Project Alpha',
  //       description: 'Main development project for the company',
  //       users: ['user1', 'user2', 'user3'],
  //       admins: ['admin1'],
  //       owners: ['owner1']
  //     },
  //     {
  //       id: '2',
  //       title: 'Marketing Campaign',
  //       description: 'Q4 marketing strategy and campaigns',
  //       users: ['user4', 'user5'],
  //       admins: ['admin2'],
  //       owners: ['owner2']
  //     },
  //     {
  //       id: '3',
  //       title: 'Product Roadmap',
  //       description: 'Product development timeline and milestones',
  //       users: ['user1', 'user6', 'user7'],
  //       admins: ['admin1', 'admin3'],
  //       owners: ['owner1']
  //     },
  //     {
  //       id: '4',
  //       title: 'Team Building',
  //       description: 'Team activities and events planning',
  //       users: ['user8', 'user9', 'user10'],
  //       admins: ['admin4'],
  //       owners: ['owner3']
  //     }
  //   ];

  //   return new Observable(observer => {
  //     setTimeout(() => {
  //       observer.next(mockBoards);
  //       observer.complete();
  //     }, 500);
  //   });
  // }

  // public addBoard(board: AddBoardDTO): Observable<BoardDetailsDTO> {
  //   // Заглушка для тестирования
  //   const mockBoard: BoardDetailsDTO = {
  //     id: Math.random().toString(36).substr(2, 9), // Генерируем случайный ID
  //     title: board.title,
  //     description: board.description,
  //     users: board.users || [],
  //     admins: board.admins || [],
  //     owners: board.owners || [],
  //     modificationDate: new Date()
  //   };

  //   return new Observable(observer => {
  //     setTimeout(() => {
  //       observer.next(mockBoard);
  //       observer.complete();
  //     }, 300);
  //   });
  // }

  // public updateBoard(board: UpdateBoardDTO): Observable<BoardDetailsDTO> {
  //   // Заглушка для тестирования
  //   const mockBoard: BoardDetailsDTO = {
  //     id: board.id,
  //     title: board.title,
  //     description: board.description,
  //     users: board.users || [],
  //     admins: board.admins || [],
  //     owners: board.owners || [],
  //     modificationDate: new Date()
  //   };

  //   return new Observable(observer => {
  //     setTimeout(() => {
  //       observer.next(mockBoard);
  //       observer.complete();
  //     }, 300);
  //   });
  // }

  // public getById(id: string): Observable<BoardDetailsDTO> {
  //   // Заглушка для тестирования
  //   const mockBoard: BoardDetailsDTO = {
  //     id: id,
  //     title: `Board ${id}`,
  //     description: `This is a detailed description for board ${id}`,
  //     users: ['user1', 'user2', 'user3'],
  //     admins: ['admin1'],
  //     owners: ['owner1'],
  //     modificationDate: new Date()
  //   };

  //   return new Observable(observer => {
  //     setTimeout(() => {
  //       observer.next(mockBoard);
  //       observer.complete();
  //     }, 400);
  //   });
  // }

  // public deleteBoard(id: string): Observable<boolean> {
  //   // Заглушка для тестирования
  //   return new Observable(observer => {
  //     setTimeout(() => {
  //       observer.next(true);
  //       observer.complete();
  //     }, 200);
  //   });
  // }
}

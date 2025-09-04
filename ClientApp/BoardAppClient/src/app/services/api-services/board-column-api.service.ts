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

  // public getBoardColumns(boardId: string): Observable<BoardColumnLookupDTO[]> {
  //   return this.http.get<BoardColumnLookupDTO[]>(`${environment.apiUrl}/boards/${boardId}/columns`);
  // }

  // public getBoardColumnById(boardId: string, columnId: string): Observable<BoardColumnDetailsDTO> {
  //   return this.http.get<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}`);
  // }

  // public addBoardColumn(boardId: string, column: AddBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
  //   return this.http.post<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns`, column);
  // }

  // public updateBoardColumn(boardId: string, column: UpdateBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
  //   return this.http.put<BoardColumnDetailsDTO>(`${environment.apiUrl}/boards/${boardId}/columns`, column);
  // }

  // public deleteBoardColumn(boardId: string, columnId: string): Observable<boolean> {
  //   return this.http.delete<boolean>(`${environment.apiUrl}/boards/${boardId}/columns/${columnId}`);
  // }


  public getBoardColumns(boardId: string): Observable<BoardColumnLookupDTO[]> {
    // Заглушка для тестирования
    const mockColumns: BoardColumnLookupDTO[] = [
      {
        id: '1',
        title: 'К выполнению',
        description: 'Задачи, которые нужно выполнить'
      },
      {
        id: '2',
        title: 'В работе',
        description: 'Задачи в процессе выполнения'
      },
      {
        id: '3',
        title: 'На проверке',
        description: 'Задачи на проверке и тестировании'
      },
      {
        id: '4',
        title: 'Завершено',
        description: 'Выполненные задачи'
      },
      {
        id: '5',
        title: 'К выполнению',
        description: 'Задачи, которые нужно выполнить'
      },
      {
        id: '6',
        title: 'К выполнению',
        description: 'Задачи, которые нужно выполнить'
      },
    ];

    return new Observable(observer => {
      setTimeout(() => {
        observer.next(mockColumns);
        observer.complete();
      }, 300);
    });
  }

  public getBoardColumnById(boardId: string, columnId: string): Observable<BoardColumnDetailsDTO> {
    // Заглушка для тестирования
    const mockColumn: BoardColumnDetailsDTO = {
      id: columnId,
      title: `Column ${columnId}`,
      description: `This is a detailed description for column ${columnId}`,
      color: '#000000'
    };

    return new Observable(observer => {
      setTimeout(() => {
        observer.next(mockColumn);
        observer.complete();
      }, 200);
    });
  }

  public addBoardColumn(boardId: string, column: AddBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    // Заглушка для тестирования
    const mockColumn: BoardColumnDetailsDTO = {
      id: Math.random().toString(36).substr(2, 9), // Генерируем случайный ID
      title: column.title,
      description: column.description,
      color: '#000000'
    };

    return new Observable(observer => {
      setTimeout(() => {
        observer.next(mockColumn);
        observer.complete();
      }, 300);
    });
  }

  public updateBoardColumn(boardId: string, column: UpdateBoardColumnDTO): Observable<BoardColumnDetailsDTO> {
    // Заглушка для тестирования
    const mockColumn: BoardColumnDetailsDTO = {
      id: column.id,
      title: column.title,
      description: column.description,
      color: '#000000'
    };

    return new Observable(observer => {
      setTimeout(() => {
        observer.next(mockColumn);
        observer.complete();
      }, 300);
    });
  }

  public deleteBoardColumn(boardId: string, columnId: string): Observable<boolean> {
    // Заглушка для тестирования
    return new Observable(observer => {
      setTimeout(() => {
        observer.next(true);
        observer.complete();
      }, 200);
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';
import { BoardLookupDTO, AddBoardDTO, BoardDetailsDTO, UpdateBoardDTO } from 'src/app/core/models';
import { BoardApiService } from 'src/app/core/services/api-services';
import { DialogService } from 'src/app/core/services/other/dialog.service';
import { Observable, switchMap, tap } from 'rxjs';
import { UserService } from 'src/app/core/services/auth/user.service';
import { UserAccess } from 'src/app/core/models/enums/user-access.enum';

@Component({
  selector: 'app-boards-list',
  standalone: true,
  imports: [CommonModule, MatMenuModule, TranslateModule],
  templateUrl: './boards-list.component.html',
  styleUrl: './boards-list.component.scss'
})
export class BoardsListComponent implements OnInit {
  boards: BoardLookupDTO[] = [];
  error: string | null = null;
  selectedBoard: BoardLookupDTO | null = null;

  getUsersCount(board: BoardLookupDTO): number {
    return (board.boardUsers ?? []).filter(u => u.role === 7).length;
  }

  getAdminsCount(board: BoardLookupDTO): number {
    return (board.boardUsers ?? []).filter(u => u.role === 127).length;
  }

  constructor(
    private boardApiService: BoardApiService,
    private router: Router,
    private dialogService: DialogService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.loadBoards().subscribe();
  }

  loadBoards(): Observable<BoardLookupDTO[]> {
    return this.boardApiService.getBoards().pipe(
      tap(boards => this.boards = boards)
    );
  }


  onBoardClick(board: BoardLookupDTO): void {
    this.router.navigate(['/board', board.id]);
  }

  onCreateBoard(): void {
    this.dialogService.openCreateBoardModal().subscribe((boardData) => {
      if (boardData) {
        this.boardApiService.addBoard(boardData as AddBoardDTO).pipe(switchMap(()=>this.loadBoards())).subscribe();
      }
    });
  }

  onDeleteBoard(boardId: string): void {
    this.boardApiService.deleteBoard(boardId).subscribe({
      next: () => {
        this.boards = this.boards.filter(b => b.id !== boardId);
      },
      error: (err) => {
        console.error('Error deleting board:', err);
        this.error = 'Failed to delete board';
      }
    });
  }

  onOpenMenu(event: MouseEvent, board: BoardLookupDTO): void {
    event.stopPropagation();
    this.selectedBoard = board;
  }

  onEditSelected(): void {
    if (!this.selectedBoard) { return; }

    this.boardApiService.getBoardById(this.selectedBoard.id).subscribe({
      next: (boardDetails: BoardDetailsDTO) => {
        this.dialogService.openEditBoardModal(boardDetails).subscribe((updateDto?: UpdateBoardDTO) => {
          if (updateDto) {
            this.boardApiService.updateBoard(updateDto).pipe(switchMap(()=>this.loadBoards())).subscribe();
          }
        });
      }
    });
  }

  private getCurrentUserRole(board: BoardLookupDTO): UserAccess | null {
    const currentEmail = (this.userService.getCurrentUserEmail() || '').toLowerCase();
    const membership = (board.boardUsers ?? []).find(u => (u.email || '').toLowerCase() === currentEmail);
    if (!membership) { return null; }
    switch (membership.role) {
      case UserAccess.USER: return UserAccess.USER;
      case UserAccess.ADMIN: return UserAccess.ADMIN;
      case UserAccess.OWNER: return UserAccess.OWNER;
      default: return null;
    }
  }

  canSeeActions(board: BoardLookupDTO): boolean {
    const role = this.getCurrentUserRole(board);
    return role === UserAccess.ADMIN || role === UserAccess.OWNER;
  }

  canEdit(board: BoardLookupDTO): boolean {
    const role = this.getCurrentUserRole(board);
    return role === UserAccess.ADMIN || role === UserAccess.OWNER;
  }

  canDelete(board: BoardLookupDTO): boolean {
    const role = this.getCurrentUserRole(board);
    return role === UserAccess.OWNER;
  }
}

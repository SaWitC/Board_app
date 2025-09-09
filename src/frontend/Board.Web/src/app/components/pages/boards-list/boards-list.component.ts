import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BoardApiService } from '../../../services/api-services';
import { DialogService } from '../../../services/dialog.service';
import { BoardLookupDTO, AddBoardDTO, BoardDetailsDTO, UpdateBoardDTO } from '../../../models';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-boards-list',
  standalone: true,
  imports: [CommonModule, MatMenuModule, TranslateModule],
  templateUrl: './boards-list.component.html',
  styleUrl: './boards-list.component.scss'
})
export class BoardsListComponent implements OnInit {
  boards: BoardLookupDTO[] = [];
  loading = false;
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
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.loadBoards();
  }

  loadBoards(): void {
    this.loading = true;
    this.error = null;

    this.boardApiService.getBoards().subscribe({
      next: (boards) => {
        this.boards = boards;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load boards';
        this.loading = false;
        console.error('Error loading boards:', error);
      }
    });
  }

  onBoardClick(board: BoardLookupDTO): void {
    this.router.navigate(['/board', board.id]);
  }

  onCreateBoard(): void {
    this.dialogService.openCreateBoardModal().subscribe((boardData) => {
      if (boardData) {
        this.boardApiService.addBoard(boardData as AddBoardDTO).subscribe({
          next: (newBoard) => {
            const newBoardLookup: BoardLookupDTO = {
              id: newBoard.id,
              title: newBoard.title,
              description: newBoard.description,
              modificationDate: newBoard.modificationDate,
              boardUsers: newBoard.boardUsers ?? []
            };
            this.boards = [...this.boards, newBoardLookup];
            console.log('Board created successfully:', newBoard);
          },
          error: (error) => {
            console.error('Error creating board:', error);
            this.error = 'Failed to create board';
          }
        });
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
            this.boardApiService.updateBoard(updateDto).subscribe({
              next: (updated) => {
                this.boards = this.boards.map(b => b.id === updated.id ? ({
                  id: updated.id,
                  title: updated.title,
                  description: updated.description,
                  modificationDate: updated.modificationDate,
                  boardUsers: updated.boardUsers ?? []
                }) : b);
              },
              error: (err) => {
                console.error('Error updating board:', err);
                this.error = 'Failed to update board';
              }
            });
          }
        });
      },
      error: (err) => {
        console.error('Error loading board details:', err);
        this.error = 'Failed to load board details';
      }
    });
  }
}

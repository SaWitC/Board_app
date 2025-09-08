import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';
import { BoardLookupDTO, AddBoardDTO, BoardDetailsDTO, UpdateBoardDTO } from 'src/app/core/models';
import { BoardApiService } from 'src/app/core/services/api-services';
import { DialogService } from 'src/app/core/services/dialog.service';

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
              users: newBoard.users,
              admins: newBoard.admins,
              owners: newBoard.owners
            };
            this.boards.push(newBoardLookup);
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

    // Load full board details if needed; here we have enough for title/description
    const boardDetails: BoardDetailsDTO = {
      id: this.selectedBoard.id,
      title: this.selectedBoard.title,
      description: this.selectedBoard.description,
      users: this.selectedBoard.users ?? [],
      admins: this.selectedBoard.admins ?? [],
      owners: this.selectedBoard.owners ?? [],
      modificationDate: new Date()
    };

    this.dialogService.openEditBoardModal(boardDetails).subscribe((updateDto?: UpdateBoardDTO) => {
      if (updateDto) {
        this.boardApiService.updateBoard(updateDto).subscribe({
          next: (updated) => {
            this.boards = this.boards.map(b => b.id === updated.id ? ({
              id: updated.id,
              title: updated.title,
              description: updated.description,
              users: updated.users,
              admins: updated.admins,
              owners: updated.owners,
              modificationDate: updated.modificationDate
            }) : b);
          },
          error: (err) => {
            console.error('Error updating board:', err);
            this.error = 'Failed to update board';
          }
        });
      }
    });
  }
}

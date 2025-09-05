import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { BoardApiService } from '../../../services/api-services';
import { DialogService } from '../../../services/dialog.service';
import { BoardLookupDTO, AddBoardDTO } from '../../../models';

@Component({
  selector: 'app-boards-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './boards-list.component.html',
  styleUrl: './boards-list.component.scss'
})
export class BoardsListComponent implements OnInit {
  boards: BoardLookupDTO[] = [];
  loading = false;
  error: string | null = null;

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
        // this.boardApiService.createBoard(boardData).subscribe({
        //   next: (newBoard) => {
        //     this.boards.push(newBoard);
        //     // Показать уведомление об успехе
        //   },
        //   error: (error) => {
        //     console.error('Error creating board:', error);
        //     // Показать уведомление об ошибке
        //   }
        // });
      }
    });
  }
}

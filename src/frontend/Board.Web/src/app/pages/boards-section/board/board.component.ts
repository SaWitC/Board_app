import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { BoardItem } from 'src/app/core/models/board-item.interface';
import { TranslateModule } from '@ngx-translate/core';
import { BoardColumnLookupDTO, BoardDetailsDTO, DragDropEvent, UpdateBoardDTO } from 'src/app/core/models';
import { BoardColumnApiService, BoardItemApiService, BoardApiService } from 'src/app/core/services/api-services';
import { BoardTemplateServiceApi } from 'src/app/core/services/api-services/board-template-api.service';
import { DialogService } from 'src/app/core/services/other/dialog.service';
import { BoardColumnComponent } from './components/board-column/board-column.component';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { boardItemToTask, taskToCreateDto, taskToUpdateDto } from 'src/app/core/mappers/board-item.mapper';
import { UserService } from 'src/app/core/services/auth/user.service';
import { BoardModalResult } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';


@Component({
    selector: 'app-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss'],
    standalone: true,
    imports: [CommonModule,
      BoardColumnComponent,
      TranslateModule,
      MatButtonModule,
      MatFormFieldModule
    ],
    providers: [BoardTemplateServiceApi]
})
export class BoardComponent implements OnInit {
  tasks: BoardItem[] = [];
  columns: BoardColumnLookupDTO[] = [];
  currentBoardId: string | null = null;
  currentBoard: BoardDetailsDTO | null = null;
  isActiveTemplate: boolean = false;

  constructor(
    private boardColumnService: BoardColumnApiService,
    private boardItemService: BoardItemApiService,
    private dialogService: DialogService,
    private boardApiService: BoardApiService,
    private boardTemplateServiceApi: BoardTemplateServiceApi,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  public isBoardManager: boolean = false;

  ngOnInit(): void {
    // Get board ID from route parameters
    this.route.params.subscribe(params => {
      const boardId = params['id'];
      if (boardId) {
        this.currentBoardId = boardId;
        this.loadBoardDetails(boardId);
        this.loadColumns(boardId);
        this.loadTasks();
      } else {
        // If no board ID, load the first available board
        this.loadFirstBoard();
      }
    });
  }

  private loadFirstBoard(): void {
    this.boardApiService.getBoards().subscribe({
      next: (boards) => {
        const boardId = boards?.[0]?.id;
        if (!boardId) {
          this.columns = [];
          return;
        }
        this.currentBoardId = boardId;
        this.loadBoardDetails(boardId);
        this.loadColumns(boardId);
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error loading boards:', error);
      }
    });
  }

  private loadBoardDetails(boardId: string): void {
    this.boardApiService.getBoardById(boardId).subscribe({
      next: (board: BoardDetailsDTO) => {
        this.currentBoard = board;
        var currentUser = board.boardUsers?.find(u=>u.email===this.userService.getCurrentUserEmail())
        if(currentUser){
          this.userService.setUserBoardAccess(currentUser?.role??null);
          this.isBoardManager = this.userService.isUserBoardAdmin();
        }
        else{
          this.router.navigate(['/boards']);
        }
      },
      error: (error: unknown) => {
        console.error('Error loading board details:', error);
      }
    });
  }

  private loadColumns(boardId: string): void {
    this.boardColumnService.getBoardColumns(boardId).subscribe({
      next: (columns) => this.columns = columns,
      error: (error) => {
        console.error('Error loading columns:', error);
      }
    });
  }

  private loadTasks(): void {
    this.boardItemService.getBoardItems(this.currentBoardId as string).subscribe({
      next: (items) => {
        this.tasks = items.map(boardItemToTask);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  getTasksByStatus(boardColumnId: string): BoardItem[] {
    return this.tasks.filter(task => task.boardColumnId === boardColumnId);
  }

  onAddTask(data: {boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'create',
    }).subscribe((task) => {
      if (task) {
        const createDto = taskToCreateDto(task);
        createDto.boardColumnId = data.boardColumnId;
        this.boardItemService.addBoardItem(this.currentBoardId as string, data.boardColumnId, createDto).subscribe({
          next: (created) => {
            this.tasks = [...this.tasks, boardItemToTask(created)];
          },
          error: (err) => console.error('Error creating task ', err)
        });
      }
    });
  }

  onEditTask(data: {task: BoardItem, boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'edit',
      task: data.task
    }).subscribe((updatedTask) => {
      if (updatedTask) {
        this.boardItemService.getBoardItemById(this.currentBoardId as string, data.task.id).subscribe({
          next: (existing) => {
            const updateDto = taskToUpdateDto(existing, updatedTask);
            updateDto.boardColumnId = data.boardColumnId;
            this.boardItemService.updateBoardItem(this.currentBoardId as string, data.boardColumnId, data.task.id, updateDto).subscribe({
              next: (res) => {
                const mapped = boardItemToTask(res);
                this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
              },
              error: (err) => console.error('Failed to update board', err)
            });
          },
          error: (err) => console.error('Failed to get board for updating', err)
        });
      }
    });
  }

  onDeleteTask(taskId: string): void {
    this.boardItemService.deleteBoardItem(this.currentBoardId as string, taskId).subscribe({
      next: () => {
        this.tasks = this.tasks.filter(t => t.id !== taskId);
      },
      error: (err) => console.error('Failed to delete task', err)
    });
  }

  onMoveTask(data: {taskId: string, toColumnId: string}): void {
    this.boardItemService.moveBoardItem(this.currentBoardId as string, data.toColumnId, data.taskId).subscribe({
      next: (res) => {
        const mapped = boardItemToTask(res);
        this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
      },
      error: (err) => console.error('Ошибка перемещения задачи', err)
    });
  }

  onDropTask(event: DragDropEvent): void {
    this.boardItemService.moveBoardItem(this.currentBoardId as string,event.toColumnId, event.taskId).subscribe({
      next: (res) => {
        const mapped = boardItemToTask(res);
        this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
      },
      error: (err) => console.error('Ошибка перемещения задачи (D&D)', err)
    });
  }

  onColumnDragStart(event: DragEvent, column: BoardColumnLookupDTO, index: number): void {
    if (event.dataTransfer) {
      event.dataTransfer.setData('text/plain', JSON.stringify({
        boardColumnId: column.id,
        fromIndex: index
      }));
      event.dataTransfer.effectAllowed = 'move';
    }
  }

  onColumnDragOver(event: DragEvent): void {
    event.preventDefault();
    event.dataTransfer!.dropEffect = 'move';
  }

  onColumnDrop(event: DragEvent, toIndex: number): void {
    event.preventDefault();

    try {
      const dragData = JSON.parse(event.dataTransfer?.getData('text/plain') || '{}');
      const fromIndex = dragData.fromIndex;

      if (fromIndex !== undefined && fromIndex !== toIndex) {
        this.reorderColumns(fromIndex, toIndex);
      }
    } catch (error) {
      console.error('Error parsing drag data:', error);
    }
  }

  private reorderColumns(fromIndex: number, toIndex: number): void {
    const columns = [...this.columns];
    const [movedColumn] = columns.splice(fromIndex, 1);
    columns.splice(toIndex, 0, movedColumn);
    this.columns = columns;
  }


  getTaskCount(): number {
    return this.tasks.length;
  }

  openSettings(): void {
    this.dialogService.openEditBoardModal(this.currentBoard as BoardDetailsDTO).subscribe((result?: BoardModalResult) => {
      if (result?.success && result?.boards) {
        // Board was updated successfully, refresh current board data
        this.boardApiService.getBoardById(this.currentBoardId as string).subscribe({
          next: (board) => {
            this.currentBoard = board;
          }
        });
      }
    });
  }
}

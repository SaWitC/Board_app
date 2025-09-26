import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';
import { BoardColumnLookupDTO, BoardDetailsDTO, BoardItemLookupDTO, DragDropEvent, UpdateBoardDTO } from 'src/app/core/models';
import { BoardColumnApiService, BoardItemApiService, BoardApiService } from 'src/app/core/services/api-services';
import { BoardTemplateServiceApi } from 'src/app/core/services/api-services/board-template-api.service';
import { DialogService } from 'src/app/core/services/other/dialog.service';
import { BoardColumnComponent } from './components/board-column/board-column.component';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { taskToCreateDto, taskToUpdateDto } from 'src/app/core/mappers/board-item.mapper';
import { UserService } from 'src/app/core/services/auth/user.service';
import { BoardModalResult } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';
import { UserAccess } from 'src/app/core/models/enums/user-access.enum';
import { OrderedBoardColumnDTO } from 'src/app/core/models/board-column/ordered-board-column-DTO.interface';
import { BoardLookupDTO } from 'src/app/core/models/board/board-lookup-DTO.interface';
import { GetBoardsRequest } from 'src/app/core/models/board/get-boards-request.interface';
import { PagedResult } from 'src/app/core/models/common/paged-result.interface';
import { forkJoin } from 'rxjs';


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
  tasks: BoardItemLookupDTO[] = [];
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
  public isGlobalAdmin: boolean = false;

  // Drag and drop properties
  public draggedColumnIndex: number | null = null;
  public hoveredColumnIndex: number | null = null;

  ngOnInit(): void {
    this.isGlobalAdmin = this.userService.hasGlobalAdminPermission();
    // Get board ID from route parameters
    this.route.params.subscribe(params => {
      const boardId = params['id'];
     this.loadData(boardId);
    });
  }

  public loadData(boardId: string){
    if (boardId) {
      this.currentBoardId = boardId;
      this.loadBoardDetails(boardId);
      this.loadColumns(boardId);
      this.loadTasks();
    } else {
      // If no board ID, load the first available board
      this.loadFirstBoard();
    }

  }

  private loadFirstBoard(): void {
    const request: GetBoardsRequest = { page: 1, pageSize: 12 };
    this.boardApiService.getBoards(request).subscribe({
      next: (result: PagedResult<BoardLookupDTO>) => {
        const boardId = result.items?.[0]?.id;
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

public updateBoardColumnOrder(columns: OrderedBoardColumnDTO[]): void {
  this.boardColumnService.updateBoardColumnOrder(this.currentBoardId as string, { OrderedBoardColumns: columns }).subscribe({
    next: () => {
      // Успешно обновлен порядок колонок
      console.log('Column order updated successfully');
    },
    error: (error) => {
      console.error('Error updating column order:', error);
      // В случае ошибки перезагружаем колонки для восстановления правильного порядка
      this.loadColumns(this.currentBoardId as string);
    }
  });
}

  private loadBoardDetails(boardId: string): void {
    this.boardApiService.getBoardById(boardId).subscribe({
      next: (board: BoardDetailsDTO) => {
        this.currentBoard = board;
        var currentUser = board.boardUsers?.find(u=>u.email===this.userService.getCurrentUserEmail())

        if(currentUser){
          this.userService.setUserBoardAccess(currentUser?.role ?? UserAccess.USER);
          this.isBoardManager = this.userService.isUserBoardAdmin();
        }
        else if(this.isGlobalAdmin){
          // GlobalAdmin can access any board even if not a participant
          this.userService.setUserBoardAccess(UserAccess.OWNER);
          this.isBoardManager = true; // GlobalAdmin has admin rights on any board
        }
        else{
          this.router.navigate(['/boards']);
        }
      },
      error: (error: unknown) => {
        console.error('Error loading board details:', error);
        this.router.navigate(['/boards']);
      }
    });
  }

  private loadColumns(boardId: string): void {
    this.boardColumnService.getBoardColumns(boardId).subscribe({
      next: (columns) => this.columns = columns.sort((a, b) => a.order - b.order),
      error: (error) => {
        console.error('Error loading columns:', error);
      }
    });
  }

  private loadTasks(): void {
    this.boardItemService.getBoardItems(this.currentBoardId as string).subscribe({
      next: (items) => {
        this.tasks = items;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  getTasksByStatus(boardColumnId: string): BoardItemLookupDTO[] {
    return this.tasks.filter(task => task.boardColumnId === boardColumnId);
  }

  onAddTask(data: {boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'create',
      boardId: this.currentBoardId!
    }).subscribe((task) => {
      if (task) {
        const createDto = taskToCreateDto(task);
        createDto.boardColumnId = data.boardColumnId;
        this.boardItemService.addBoardItem(this.currentBoardId as string, data.boardColumnId, createDto).subscribe({
          next: (created) => {
            this.tasks = [...this.tasks, created];
          },
          error: (err) => console.error('Error creating task ', err)
        });
      }
    });
  }

  onEditTask(data: {task: BoardItemLookupDTO, boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'edit',
      task: data.task,
      boardId: this.currentBoardId!
    }).subscribe((updatedTask) => {
      if (updatedTask) {
        this.boardItemService.getBoardItemById(this.currentBoardId as string, data.task.id).subscribe({
          next: (existing) => {
            const updateDto = taskToUpdateDto(existing, updatedTask);
            updateDto.boardColumnId = data.boardColumnId;
            this.boardItemService.updateBoardItem(this.currentBoardId as string, data.boardColumnId, data.task.id, updateDto).subscribe({
              next: (res) => {
                const mapped = res;
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
        const mapped = res;
        this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
      },
      error: (err) => console.error('Ошибка перемещения задачи', err)
    });
  }

  onDropTask(event: DragDropEvent): void {
    this.boardItemService.moveBoardItem(this.currentBoardId as string,event.toColumnId, event.taskId).subscribe({
      next: (res) => {
        const mapped = res;
        this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
      },
      error: (err) => console.error('Ошибка перемещения задачи (D&D)', err)
    });
  }

  //Columns order management start
  onColumnDragStart(event: DragEvent, index: number): void {
    if (event.dataTransfer) {
      event.dataTransfer.effectAllowed = 'move';
      event.dataTransfer.setData('text/plain', index.toString());
      this.draggedColumnIndex = index;
    }
  }

  onColumnDragEnd(event: DragEvent): void {
    this.draggedColumnIndex = null;
    this.hoveredColumnIndex = null;
  }

  onColumnDragOver(event: DragEvent, index: number): void {
    event.preventDefault();

    this.hoveredColumnIndex = index;

    if (event.dataTransfer) {
      event.dataTransfer.dropEffect = 'move';
    }

    if (this.draggedColumnIndex === null || this.draggedColumnIndex === index) {
      return;
    }

    const draggedColumn = this.columns[this.draggedColumnIndex];
    this.columns.splice(this.draggedColumnIndex, 1);
    this.columns.splice(index, 0, draggedColumn);

    this.draggedColumnIndex = index;
  }

  onColumnDragEnter(event: DragEvent): void {
    event.preventDefault();
  }

  onColumnDrop(event: DragEvent, dropIndex: number): void {
    event.preventDefault();

    if (this.draggedColumnIndex === null) {
      return;
    }

    const draggedColumn = this.columns[this.draggedColumnIndex];
    this.columns.splice(this.draggedColumnIndex, 1);
    this.columns.splice(dropIndex, 0, draggedColumn);

    const orderedColumns: OrderedBoardColumnDTO[] = this.columns.map((column, index) => ({
      id: column.id,
      order: index
    }));

    this.updateBoardColumnOrder(orderedColumns);

    this.draggedColumnIndex = null;
    this.hoveredColumnIndex = null;
  }

  trackByColumnId(index: number, column: BoardColumnLookupDTO): string {
    return column.id;
  }
  //Columns order management end


  getTaskCount(): number {
    return this.tasks.length;
  }

  openSettings(): void {
    this.dialogService.openEditBoardModal(this.currentBoard as BoardDetailsDTO, this.columns).subscribe((result?: BoardModalResult) => {
      if (result?.success && result?.boards) {
        this.boardApiService.getBoardById(this.currentBoardId as string).subscribe({
          next: (board) => {
            this.currentBoard = board;
            this.loadData(this.currentBoardId as string);
          }
        });
      }
    });
  }
}

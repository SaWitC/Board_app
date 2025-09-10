import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Task } from 'src/app/core/models/task.interface';
import { TranslateModule } from '@ngx-translate/core';
import { BoardColumnLookupDTO, BoardDetailsDTO, DragDropEvent } from 'src/app/core/models';
import { BoardColumnApiService, BoardItemApiService, BoardApiService } from 'src/app/core/services/api-services';
import { DialogService } from 'src/app/core/services/dialog.service';
import { boardItemToTask, taskToCreateDto, taskToUpdateDto } from 'src/app/core/services/mappers/board-item.mapper';
import { BoardColumnComponent } from './components/board-column/board-column.component';


@Component({
    selector: 'app-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss'],
    standalone: true,
    imports: [CommonModule, BoardColumnComponent, TranslateModule]
})
export class BoardComponent implements OnInit {
  tasks: Task[] = [];
  columns: BoardColumnLookupDTO[] = [];
  loading = false;
  error: string | null = null;
  currentBoardId: string | null = null;
  currentBoard: BoardDetailsDTO | null = null;

  constructor(
    private boardColumnService: BoardColumnApiService,
    private boardItemService: BoardItemApiService,
    private dialogService: DialogService,
    private boardApiService: BoardApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

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
        this.error = 'Failed to load boards';
      }
    });
  }

  private loadBoardDetails(boardId: string): void {
    this.boardApiService.getBoardById(boardId).subscribe({
      next: (board: BoardDetailsDTO) => {
        this.currentBoard = board;
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
        this.error = 'Failed to load columns';
      }
    });
  }

  private loadTasks(): void {
    this.loading = true;
    this.error = null;
    this.boardItemService.getBoardItems().subscribe({
      next: (items) => {
        this.tasks = items.map(boardItemToTask);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load task';
        this.loading = false;
        console.error(err);
      }
    });
  }

  getTasksByStatus(columnId: string): Task[] {
    return this.tasks.filter(task => task.columnId === columnId);
  }

  onAddTask(data: {boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'create',
    }).subscribe((task) => {
      if (task) {
        const createDto = taskToCreateDto(task);
        createDto.boardColumnId = data.boardColumnId;
        this.boardItemService.addBoardItem(createDto).subscribe({
          next: (created) => {
            this.tasks = [...this.tasks, boardItemToTask(created)];
          },
          error: (err) => console.error('Error creating task ', err)
        });
      }
    });
  }

  onEditTask(data: {task: Task, boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'edit',
      task: data.task
    }).subscribe((updatedTask) => {
      if (updatedTask) {
        this.boardItemService.getBoardItemById(data.task.id).subscribe({
          next: (existing) => {
            const updateDto = taskToUpdateDto(existing, updatedTask);
            updateDto.boardColumnId = data.boardColumnId;
            this.boardItemService.updateBoardItem(data.task.id, updateDto).subscribe({
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
    this.boardItemService.deleteBoardItem(taskId).subscribe({
      next: () => {
        this.tasks = this.tasks.filter(t => t.id !== taskId);
      },
      error: (err) => console.error('Failed to delete task', err)
    });
  }

  onMoveTask(data: {taskId: string, newStatus: string}): void {
    this.boardItemService.moveBoardItem(data.taskId, data.newStatus).subscribe({
      next: (res) => {
        const mapped = boardItemToTask(res);
        this.tasks = this.tasks.map(t => t.id === mapped.id ? mapped : t);
      },
      error: (err) => console.error('Ошибка перемещения задачи', err)
    });
  }

  onDropTask(event: DragDropEvent): void {
    this.boardItemService.moveBoardItem(event.taskId, event.toColumnId).subscribe({
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
        columnId: column.id,
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
}

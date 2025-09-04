import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, DragDropEvent } from '../../../models/task.model';
import { TaskService } from '../../../services/task.service';
import { BoardColumnApiService } from '../../../services/api-services/board-column-api.service';
import { BoardColumnLookupDTO } from '../../../models/board-column/board-column-lookup-DTO.interface';
import { BoardColumnComponent } from '../board-column/board-column.component';
import { DialogService } from '../../../services/dialog.service';

@Component({
    selector: 'app-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss'],
    standalone: true,
    imports: [CommonModule, BoardColumnComponent]
})
export class BoardComponent implements OnInit {
  tasks: Task[] = [];
  columns: BoardColumnLookupDTO[] = [];

  constructor(
    private taskService: TaskService,
    private boardColumnService: BoardColumnApiService,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.loadColumns();
    this.taskService.tasks$.subscribe(tasks => {
      this.tasks = tasks;
    });
  }

  private loadColumns(): void {
    const boardId = 'default-board';
    this.boardColumnService.getBoardColumns(boardId).subscribe(columns => {
      this.columns = columns;
    });
  }

  getTasksByStatus(columnId: string): Task[] {
    return this.tasks.filter(task => task.columnId === columnId);
  }

  onAddTask(columnId: string): void {
    this.dialogService.openTaskModal({
      mode: 'create',
      boardColumns: this.columns,
      selectedBoardColumnId:columnId
    }).subscribe((task) => {
      if (task) {
        const taskData = {
          ...task,
          boardColumns: this.columns
        };
        this.taskService.createTask(taskData);
      }
    });
  }

  onEditTask(data: {task: Task, boardColumnId: string}): void {
    this.dialogService.openTaskModal({
      mode: 'edit',
      boardColumns: this.columns,
      selectedBoardColumnId: data.boardColumnId
    }).subscribe((updatedTask) => {
      if (updatedTask) {
        this.taskService.updateTask(data.task.id, updatedTask);
      }
    });
  }

  onDeleteTask(taskId: string): void {
    this.taskService.deleteTask(taskId);
  }

  onMoveTask(data: {taskId: string, newStatus: string}): void {
    this.taskService.moveTask(data.taskId, data.newStatus);
  }

  onDropTask(event: DragDropEvent): void {
    this.taskService.moveTaskByDragDrop(event);
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

  getCompletedTaskCount(): number {
    return this.tasks.filter(task => task.columnId === '4').length;
  }

  getProgressPercentage(): number {
    if (this.tasks.length === 0) return 0;
    return Math.round((this.getCompletedTaskCount() / this.tasks.length) * 100);
  }
}

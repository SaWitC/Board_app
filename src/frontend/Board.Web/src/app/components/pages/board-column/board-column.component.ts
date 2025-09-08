import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, DragDropEvent } from '../../../models/task.model';
import { BoardColumnApiService } from '../../../services/api-services';
import { BoardColumnDetailsDTO } from '../../../models';
import { Subscription } from 'rxjs';
import { TaskCardComponent } from '../task-card/task-card.component';
import {MatDialog} from '@angular/material/dialog';

@Component({
    selector: 'app-board-column',
    templateUrl: './board-column.component.html',
    styleUrls: ['./board-column.component.scss'],
    standalone: true,
    imports: [CommonModule, TaskCardComponent]
})
export class BoardColumnComponent implements OnInit, OnDestroy {
  @Input() columnId: string = '1';
  @Input() boardId: string = '1';

  @Input() allTasks: Task[] = [];
  @Input() columnTasks: Task[] = [];
  @Output() addTask = new EventEmitter<{boardColumnId: string}>();
  @Output() editTask = new EventEmitter<{task: Task, boardColumnId: string}>();
  @Output() deleteTask = new EventEmitter<string>();
  @Output() moveTask = new EventEmitter<{taskId: string, newStatus: string}>();
  @Output() dropTask = new EventEmitter<DragDropEvent>();

  isDragOver = false;
  columnData?: BoardColumnDetailsDTO;
  loading = false;
  error: string | null = null;
  private subscription = new Subscription();

  constructor(private boardColumnApiService: BoardColumnApiService,matDialog: MatDialog) {}

  ngOnInit(): void {
    this.loadData();
  }

  public loadData(): void {
    this.boardColumnApiService.getBoardColumnById(this.boardId, this.columnId).subscribe((data) => {
      this.columnData = data;
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onAddTask(): void {
    const targetColumnId = this.columnData?.id ?? this.columnId;
    this.addTask.emit({boardColumnId: targetColumnId});
  }

  onEditTask(task: Task): void {
    const targetColumnId = this.columnData?.id ?? this.columnId;
    this.editTask.emit({task: task, boardColumnId: targetColumnId});
  }

  onDeleteTask(taskId: string): void {
    this.deleteTask.emit(taskId);
  }

  onMoveTask(data: {taskId: string, newStatus: string}): void {
    this.moveTask.emit(data);
  }

  // Drag and Drop methods
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.dataTransfer!.dropEffect = 'move';
  }

      onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;

    const taskId = event.dataTransfer?.getData('text/plain');

    if (taskId) {
      const dragDropEvent: DragDropEvent = {
        taskId: taskId,
        fromColumnId: this.getTaskStatusFromId(taskId),
        toColumnId: this.columnData?.id ?? this.columnId
      };

      if (dragDropEvent.fromColumnId !== dragDropEvent.toColumnId) {
        this.dropTask.emit(dragDropEvent);
      }
    }
  }

    private getTaskStatusFromId(taskId: string): string {
    const task = this.allTasks.find(t => t.id === taskId);
    if (task) {
      return task.id;
    }

    return this.columnData?.id ?? this.columnId;
  }

  onDragEnter(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
  }
}

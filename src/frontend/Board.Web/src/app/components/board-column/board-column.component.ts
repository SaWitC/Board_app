import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, TaskStatus, TASK_STATUS_CONFIG, DragDropEvent } from '../../../models/task.model';
import { BoardColumnApiService } from '../../../services/api-services';
import { BoardColumnDetailsDTO } from '../../../models';
import { Subscription } from 'rxjs';
import { TaskCardComponent } from '../task-card/task-card.component';

@Component({
    selector: 'app-board-column',
    templateUrl: './board-column.component.html',
    styleUrls: ['./board-column.component.scss'],
    standalone: true,
    imports: [CommonModule, TaskCardComponent]
})
export class BoardColumnComponent implements OnInit, OnDestroy {
  @Input() status!: TaskStatus;
  @Input() customTitle?: string;
  @Input() customDescription?: string;
  @Input() tasks: Task[] = [];
  @Input() allTasks: Task[] = []; // Добавляем все задачи для поиска исходного статуса
  @Input() boardId: string = '1'; // ID доски для API запросов
  @Output() addTask = new EventEmitter<TaskStatus>();
  @Output() editTask = new EventEmitter<Task>();
  @Output() deleteTask = new EventEmitter<string>();
  @Output() moveTask = new EventEmitter<{taskId: string, newStatus: TaskStatus}>();
  @Output() dropTask = new EventEmitter<DragDropEvent>();

  isDragOver = false;
  columnData: BoardColumnDetailsDTO | null = null;
  loading = false;
  error: string | null = null;
  private subscription = new Subscription();

  constructor(private boardColumnApiService: BoardColumnApiService) {}

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  get statusConfig() {
    return TASK_STATUS_CONFIG.find(config => config.status === this.status);
  }

  get columnTitle(): string {
    // Используем входной параметр, если доступен, иначе данные из API, иначе fallback на конфиг
    return this.customTitle || this.columnData?.title || this.statusConfig?.label || this.status;
  }

  get columnColor(): string {
    return this.statusConfig?.color || '#f5f5f5';
  }

  get columnIcon(): string {
    return this.statusConfig?.icon || 'list';
  }

  get columnDescription(): string {
    // Используем входной параметр, если доступен, иначе данные из API
    return this.customDescription || this.columnData?.description || '';
  }

  onAddTask(): void {
    this.addTask.emit(this.status);
  }

  onEditTask(task: Task): void {
    this.editTask.emit(task);
  }

  onDeleteTask(taskId: string): void {
    this.deleteTask.emit(taskId);
  }

  onMoveTask(data: {taskId: string, newStatus: TaskStatus}): void {
    this.moveTask.emit(data);
  }

  // Drag and Drop methods
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.dataTransfer!.dropEffect = 'move';
  }

      onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false; // Сбрасываем состояние drag-over

    const taskId = event.dataTransfer?.getData('text/plain');

    if (taskId) {
      // Создаем событие drag and drop
      const dragDropEvent: DragDropEvent = {
        taskId: taskId,
        fromStatus: this.getTaskStatusFromId(taskId), // Получаем статус из сервиса
        toStatus: this.status
      };

      // Проверяем, что статус действительно изменился
      if (dragDropEvent.fromStatus !== dragDropEvent.toStatus) {
        this.dropTask.emit(dragDropEvent);
      }
    }
  }

    private getTaskStatusFromId(taskId: string): TaskStatus {
    // Найдем задачу во всех задачах
    const task = this.allTasks.find(t => t.id === taskId);
    if (task) {
      return task.status;
    }

    // Fallback - если задача не найдена, используем текущий статус
    return this.status;
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

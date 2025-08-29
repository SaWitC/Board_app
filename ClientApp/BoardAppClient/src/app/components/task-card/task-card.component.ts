import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, TaskPriority, TASK_STATUS_CONFIG } from '../../models/task.model';
import { TaskStatus } from '../../enums/task-status.enum';

@Component({
    selector: 'app-task-card',
    templateUrl: './task-card.component.html',
    styleUrls: ['./task-card.component.scss'],
    standalone: true,
    imports: [CommonModule]
})
export class TaskCardComponent {
  @Input() task!: Task;
  @Output() editTask = new EventEmitter<Task>();
  @Output() deleteTask = new EventEmitter<string>();
  @Output() moveTask = new EventEmitter<{taskId: string, newStatus: TaskStatus}>();

  taskStatuses = TASK_STATUS_CONFIG;
  priorities = Object.values(TaskPriority);
  isDropdownOpen = false;
  isDragging = false;

  // Drag and Drop methods
    onDragStart(event: DragEvent): void {
    if (event.dataTransfer) {
      event.dataTransfer.setData('text/plain', this.task.id);
      event.dataTransfer.effectAllowed = 'move';
      this.isDragging = true;
    }
  }

  onDragEnd(event: DragEvent): void {
    this.isDragging = false;
  }

  getPriorityColor(priority: TaskPriority): string {
    const colors = {
      [TaskPriority.LOW]: '#4caf50',
      [TaskPriority.MEDIUM]: '#ff9800',
      [TaskPriority.HIGH]: '#f44336',
      [TaskPriority.URGENT]: '#9c27b0'
    };
    return colors[priority] || '#757575';
  }

  getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: 'Низкий',
      [TaskPriority.MEDIUM]: 'Средний',
      [TaskPriority.HIGH]: 'Высокий',
      [TaskPriority.URGENT]: 'Срочный'
    };
    return labels[priority] || 'Неизвестно';
  }

  getStatusLabel(status: TaskStatus): string {
    const statusConfig = this.taskStatuses.find(s => s.status === status);
    return statusConfig?.label || status;
  }

  onEdit(): void {
    this.editTask.emit(this.task);
  }

  onDelete(): void {
    if (confirm('Вы уверены, что хотите удалить эту задачу?')) {
      this.deleteTask.emit(this.task.id);
    }
  }

  onMoveTask(newStatus: TaskStatus): void {
    this.moveTask.emit({ taskId: this.task.id, newStatus });
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  closeDropdown(): void {
    this.isDropdownOpen = false;
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('ru-RU');
  }

  isOverdue(): boolean {
    if (!this.task.dueDate) return false;
    return new Date(this.task.dueDate) < new Date();
  }
}

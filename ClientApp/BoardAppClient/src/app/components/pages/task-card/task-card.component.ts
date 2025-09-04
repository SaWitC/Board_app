import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, TaskPriority } from 'src/app/models';

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
  @Output() moveTask = new EventEmitter<{taskId: string, newStatus: string}>();

  priorities = Object.values(TaskPriority);
  isDragging = false;

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

  onEdit(): void {
    this.editTask.emit(this.task);
  }

  onDelete(): void {
    if (confirm('Вы уверены, что хотите удалить эту задачу?')) {
      this.deleteTask.emit(this.task.id);
    }
  }

  onMoveTask(newStatus: string): void {
    this.moveTask.emit({ taskId: this.task.id, newStatus });
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('ru-RU');
  }

  isOverdue(): boolean {
    if (!this.task.dueDate) return false;
    return new Date(this.task.dueDate) < new Date();
  }
}

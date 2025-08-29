import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, TaskStatus, DragDropEvent } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { BoardColumnComponent } from '../board-column/board-column.component';
import { TaskModalComponent } from '../task-modal/task-modal.component';

@Component({
    selector: 'app-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss'],
    standalone: true,
    imports: [CommonModule, BoardColumnComponent, TaskModalComponent]
})
export class BoardComponent implements OnInit {
  tasks: Task[] = [];
  isModalOpen = false;
  editingTask?: Task;
  creatingForStatus?: TaskStatus;
  taskStatuses = Object.values(TaskStatus);

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.taskService.tasks$.subscribe(tasks => {
      this.tasks = tasks;
    });
  }

  getTasksByStatus(status: TaskStatus): Task[] {
    return this.tasks.filter(task => task.status === status);
  }

  onAddTask(status: TaskStatus): void {
    this.creatingForStatus = status;
    this.editingTask = undefined;
    this.isModalOpen = true;
  }

  onEditTask(task: Task): void {
    this.editingTask = task;
    this.creatingForStatus = undefined;
    this.isModalOpen = true;
  }

  onDeleteTask(taskId: string): void {
    this.taskService.deleteTask(taskId);
  }

  onMoveTask(data: {taskId: string, newStatus: TaskStatus}): void {
    this.taskService.moveTask(data.taskId, data.newStatus);
  }

  onDropTask(event: DragDropEvent): void {
    this.taskService.moveTaskByDragDrop(event);
  }

  onSaveTask(task: Task): void {
    if (this.editingTask) {
      // Редактирование существующей задачи
      this.taskService.updateTask(task.id, task);
    } else {
      // Создание новой задачи
      const taskData = {
        ...task,
        status: this.creatingForStatus || TaskStatus.TODO
      };
      this.taskService.createTask(taskData);
    }
    this.closeModal();
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.editingTask = undefined;
    this.creatingForStatus = undefined;
  }

  getTaskCount(): number {
    return this.tasks.length;
  }

  getCompletedTaskCount(): number {
    return this.tasks.filter(task => task.status === TaskStatus.DONE).length;
  }

  getProgressPercentage(): number {
    if (this.tasks.length === 0) return 0;
    return Math.round((this.getCompletedTaskCount() / this.tasks.length) * 100);
  }
}

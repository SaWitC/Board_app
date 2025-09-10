import { Component, Input, Output, EventEmitter, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Task } from 'src/app/core/models/task.interface';
import { TaskStatus } from 'src/app/core/models/enums/task-status.enum';
import { TaskPriority } from 'src/app/core/models/enums/task-priority.enum';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TaskModalData } from 'src/app/pages/boards-section/board/modals/task-modal/task-modal.component';

@Component({
    selector: 'app-task-modal',
    templateUrl: './task-modal.component.html',
    styleUrls: ['./task-modal.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class TaskModalComponent implements OnInit {
  taskForm!: FormGroup;
  taskStatuses = Object.values(TaskStatus);
  taskPriorities = Object.values(TaskPriority);

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TaskModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TaskModalData

  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  public task?: Task;

  ngOnChanges(): void {
    this.initForm();
  }

  private initForm(): void {
    this.task = this.data.task;

    this.taskForm = this.fb.group({
      title: [this.task?.title || '', [Validators.required, Validators.minLength(3)]],
      description: [this.task?.description || ''],
      status: [this.task?.status || TaskStatus.TODO, Validators.required],
      priority: [this.task?.priority || TaskPriority.MEDIUM, Validators.required],
      assignee: [this.task?.assignee || ''],
      dueDate: [this.task?.dueDate ? new Date(this.task.dueDate).toISOString().split('T')[0] : ''],
      tags: [this.task?.tags?.join(', ') || '']
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskData: Partial<Task> = {
        title: formValue.title,
        description: formValue.description,
        status: formValue.status,
        priority: formValue.priority,
        assignee: formValue.assignee || undefined,
        dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined,
        tags: formValue.tags ? formValue.tags.split(',').map((tag: string) => tag.trim()).filter((tag: string) => tag) : undefined
      };

      if (this.task) {
        // Редактирование существующей задачи
        const updatedTask: Task = {
          ...this.task,
          ...taskData,
          updatedAt: new Date()
        };
        this.dialogRef.close(updatedTask);
      } else {
        // Создание новой задачи
        const newTask = {
          ...taskData,
          createdAt: new Date(),
          updatedAt: new Date()
        };
        this.dialogRef.close(newTask as Task);
      }
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }

  getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: 'Низкий',
      [TaskPriority.MEDIUM]: 'Средний',
      [TaskPriority.HIGH]: 'Высокий',
      [TaskPriority.URGENT]: 'Срочный'
    };
    return labels[priority] || priority;
  }

  getStatusLabel(status: TaskStatus): string {
    const labels = {
      [TaskStatus.TODO]: 'К выполнению',
      [TaskStatus.IN_PROGRESS]: 'В работе',
      [TaskStatus.REVIEW]: 'На проверке',
      [TaskStatus.DONE]: 'Завершено'
    };
    return labels[status] || status;
  }
}

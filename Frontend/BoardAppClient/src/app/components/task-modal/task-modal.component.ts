import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Task, TaskStatus, TaskPriority } from '../../models/task.model';

@Component({
  selector: 'app-task-modal',
  templateUrl: './task-modal.component.html',
  styleUrls: ['./task-modal.component.scss']
})
export class TaskModalComponent implements OnInit {
  @Input() task?: Task;
  @Input() isOpen = false;
  @Output() saveTask = new EventEmitter<Task>();
  @Output() closeModal = new EventEmitter<void>();

  taskForm!: FormGroup;
  taskStatuses = Object.values(TaskStatus);
  taskPriorities = Object.values(TaskPriority);

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(): void {
    if (this.isOpen) {
      this.initForm();
    }
  }

  private initForm(): void {
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
        this.saveTask.emit(updatedTask);
      } else {
        // Создание новой задачи
        const newTask = {
          ...taskData,
          createdAt: new Date(),
          updatedAt: new Date()
        };
        this.saveTask.emit(newTask as Task);
      }
    }
  }

  onClose(): void {
    this.closeModal.emit();
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

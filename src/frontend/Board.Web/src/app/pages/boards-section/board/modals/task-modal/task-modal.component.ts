import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { Task } from 'src/app/core/models/task.interface';
import { BoardColumnLookupDTO } from 'src/app/core/models';
import { TaskPriority } from 'src/app/core/models/enums/task-priority.enum';

export interface TaskModalData {
  task?: Task;
  mode: 'create' | 'edit';
}

@Component({
    selector: 'app-task-modal',
    templateUrl: './task-modal.component.html',
    styleUrls: ['./task-modal.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatIconModule,
        MatChipsModule
    ]
})
export class TaskModalComponent implements OnInit {
  task?: Task;
  taskForm!: FormGroup;
  taskPriorities = Object.values(TaskPriority);
  dialogTitle: string;
  submitButtonText: string;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TaskModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TaskModalData
  ) {
    this.task = data.task;
    this.dialogTitle = data.mode === 'create' ? 'Создать новую задачу' : 'Редактировать задачу';
    this.submitButtonText = data.mode === 'create' ? 'Создать' : 'Сохранить';
  }

  ngOnInit(): void {
    console.log(this.task);
    this.initForm();
  }

  private initForm(): void {

    this.taskForm = this.fb.group({
      title: [this.task?.title || '', [Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
      description: [this.task?.description || '', [Validators.required]],
      priority: [this.task?.priority || TaskPriority.MEDIUM, Validators.required],
      assignee: [this.task?.assignee || ''],
      dueDate: [this.task?.dueDate ? new Date(this.task.dueDate) : undefined],
      tags: [this.task?.tags?.join(', ') || '']
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskData: Partial<Task> = {
        title: formValue.title,
        description: formValue.description,
        columnId: formValue.columnId,
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

  getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: 'Низкий',
      [TaskPriority.MEDIUM]: 'Средний',
      [TaskPriority.HIGH]: 'Высокий',
      [TaskPriority.URGENT]: 'Срочный'
    };
    return labels[priority] || priority;
  }
}

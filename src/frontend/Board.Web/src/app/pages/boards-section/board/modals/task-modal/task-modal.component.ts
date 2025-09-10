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
import { BoardItem } from 'src/app/core/models/board-item.interface';
import { TaskPriority } from 'src/app/core/models/enums/task-priority.enum';
import { TaskType } from 'src/app/core/models/enums/task-type.enum';
import { TaskTypeIconComponent } from 'src/app/components/shared/story-icon/task-type-icon.component';

export interface TaskModalData {
  task?: BoardItem;
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
        MatChipsModule,
        TaskTypeIconComponent
    ]
})
export class TaskModalComponent implements OnInit {
  task?: BoardItem;
  taskForm!: FormGroup;
  taskPriorities = Object.values(TaskPriority) as TaskPriority[];
  taskTypes = Object.values(TaskType).filter(k => !isNaN(Number(k))) as TaskType[];
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
      tags: [this.task?.tags?.join(', ') || ''],
      taskType: [this.task?.taskType || TaskType.USER_STORY, Validators.required]
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskData: Partial<BoardItem> = {
        title: formValue.title,
        description: formValue.description,
        columnId: formValue.columnId,
        priority: formValue.priority,
        taskType: formValue.taskType,
        assignee: formValue.assignee || undefined,
        dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined,
        tags: formValue.tags ? formValue.tags.split(',').map((tag: string) => tag.trim()).filter((tag: string) => tag) : undefined
      };

      if (this.task) {
        // Редактирование существующей задачи
        const updatedTask: BoardItem = {
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
        this.dialogRef.close(newTask as BoardItem);
      }
    }
  }

  public onClose(): void {
    this.dialogRef.close();
  }

  public getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: 'Низкий',
      [TaskPriority.MEDIUM]: 'Средний',
      [TaskPriority.HIGH]: 'Высокий',
      [TaskPriority.URGENT]: 'Срочный'
    };
    return labels[priority] || priority.toString();
  }

  public getTaskTypeLabel(taskType: TaskType): string {
    const labels = {
      [TaskType.BUG]: 'Bug',
      [TaskType.HOT_FIX]: 'Hot Fix',
      [TaskType.USER_STORY]: 'User Story'
    };
    return labels[taskType] || taskType.toString();
  }
}

import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { TaskPriority } from 'src/app/core/models/enums/task-priority.enum';
import { TaskType } from 'src/app/core/models/enums/task-type.enum';
import { TaskTypeIconComponent } from 'src/app/components/shared/story-icon/task-type-icon.component';
import { DescriptionEditorComponent } from '../../components/description-editor/description-editor.component';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BoardItemApiService } from 'src/app/core/services/api-services';
import { Observable, of, switchMap, tap } from 'rxjs';
import { BoardItemDetailsDTO, BoardItemLookupDTO } from 'src/app/core/models';
import { MatMenuContent } from "@angular/material/menu";

export interface TaskModalData {
  task?: BoardItemLookupDTO;
  mode: 'create' | 'edit' | 'preview';
  boardId: string;
}

@Component({
  selector: 'app-task-modal',
  templateUrl: './task-modal.component.html',
  styleUrls: ['./task-modal.component.scss'],
  standalone: true,
  imports: [
    TranslateModule,
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
    TaskTypeIconComponent,
    DescriptionEditorComponent,
]
})
export class TaskModalComponent implements OnInit {
  taskDetails?: BoardItemDetailsDTO;
  public boardId: string | null = null;
  public dialogData: TaskModalData | null;

  taskForm!: FormGroup;
  taskPriorities = Object.values(TaskPriority).filter(k => !isNaN(Number(k))) as TaskPriority[];
  taskTypes = Object.values(TaskType).filter(k => !isNaN(Number(k))) as TaskType[];
  dialogTitle: string;
  submitButtonText: string;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TaskModalComponent>,
    private toastr: ToastrService,
    private boardItemService: BoardItemApiService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: TaskModalData
  ) {
    this.dialogData = data;
    this.boardId = data.boardId;

    this.dialogTitle = data.mode === 'create' ? 'Создать новую задачу' :
      data.mode === 'edit' ? 'Редактировать задачу' : 'Просмотр задачи';
    this.submitButtonText = data.mode === 'create' ? 'Создать' : 'Сохранить';
  }

  ngOnInit(): void {
    if (this.dialogData == null) {
      this.toastr.error(this.translate.instant('ERRORS.UNEXPECTED_ERROR'));
      return
    }

    //Create form, init data
    this.createForm()
    .pipe(
      switchMap((form: FormGroup) => {
        if (this.dialogData?.mode === 'edit' || this.dialogData?.mode === 'preview') {
          if (this.boardId != null && this.data.task?.id) {
            return this.boardItemService.getBoardItemById(this.boardId, this.data.task.id).pipe(
              tap(x => this.taskDetails = x)
            );
          }
        }
        return of(null);
      }),
      switchMap(task => this.initFormData(task)) // bind `this`
    )
    .subscribe();
  }

  private createForm(): Observable<FormGroup> {
    this.taskForm = this.fb.group({
      title: [undefined,[Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
      description: [undefined,[Validators.required]],
      priority: [TaskPriority.MEDIUM, Validators.required],
      assignee: [undefined],
      dueDate: [undefined],
      tags: [undefined],
      taskType: [TaskType.USER_STORY, Validators.required]
    });

    if(this.data.mode=='preview'){
      this.taskForm.disable();
    }

    return of(this.taskForm);
  }

  public initFormData(taskData: BoardItemDetailsDTO | null): Observable<FormGroup> {
    if (taskData) {
      this.taskForm.controls['title'].patchValue(taskData.title)
      this.taskForm.controls['priority'].patchValue(taskData.priority)
      this.taskForm.controls['assignee'].patchValue(taskData.assignee)//TODO should be boardUsers user select box
      this.taskForm.controls['dueDate'].patchValue(taskData.dueDate)
      this.taskForm.controls['tags'].patchValue(taskData.tags?.join(', ') || '')
      this.taskForm.controls['taskType'].patchValue(taskData?.taskType || TaskType.USER_STORY)
    }

    return of(this.taskForm);
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskData: Partial<BoardItemDetailsDTO> = {
        title: formValue.title,
        description: formValue.description,
        boardColumnId: formValue.boardColumnId,
        priority: formValue.priority,
        taskType: formValue.taskType,
        assignee: formValue.assignee || undefined,
        dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined,
        tags: formValue.tags ? formValue.tags.split(',').map((tag: string) => tag.trim()).filter((tag: string) => tag) : undefined
      };

      if (this.taskDetails) {
        // Редактирование существующей задачи
        const updatedTask: BoardItemDetailsDTO = {
          ...this.taskDetails,
          ...taskData,
        };
        this.dialogRef.close(updatedTask);
      } else {
        // Создание новой задачи
        const newTask = {
          ...taskData,
          createdAt: new Date(),
        };
        this.dialogRef.close(newTask as BoardItemDetailsDTO);
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

  public getControl(controlName: string): FormControl {
    return this.taskForm.get(controlName) as FormControl;
  }

  public get isPreviewMode(): boolean {
    return this.data.mode === 'preview';
  }
}

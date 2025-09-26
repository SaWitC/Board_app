import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { TaskPriority } from 'src/app/core/models/enums/task-priority.enum';
import { TaskType } from 'src/app/core/models/enums/task-type.enum';
import { TaskTypeIconComponent } from 'src/app/components/shared/story-icon/task-type-icon.component';
import { DescriptionEditorComponent } from '../../components/description-editor/description-editor.component';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BoardItemApiService } from 'src/app/core/services/api-services';
import { Observable, of, switchMap, tap, map, startWith } from 'rxjs';
import { BoardItemDetailsDTO, BoardItemLookupDTO } from 'src/app/core/models';

import { BoardApiService } from 'src/app/core/services/api-services/board-api.service';
import { UserLookupDTO } from 'src/app/core/models/user/user-lookup-DTO.model';
import { TagDTO } from 'src/app/core/models/tag/tag-DTO.interface';

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
    MatAutocompleteModule,
    TaskTypeIconComponent,
    DescriptionEditorComponent,
  ]
})
export class TaskModalComponent implements OnInit, AfterViewInit {
  taskDetails?: BoardItemDetailsDTO;
  public boardId: string | null = null;
  public dialogData: TaskModalData | null;

  taskForm!: FormGroup;
  taskPriorities = Object.values(TaskPriority).filter(k => !isNaN(Number(k))) as TaskPriority[];
  taskTypes = Object.values(TaskType).filter(k => !isNaN(Number(k))) as TaskType[];
  dialogTitle: string;
  submitButtonText: string;

  boardUsers: UserLookupDTO[] = [];
  filteredUsers$!: Observable<UserLookupDTO[]>;
  assigneeKey = 0;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<TaskModalComponent>,
    private toastr: ToastrService,
    private boardItemService: BoardItemApiService,
    private boardService: BoardApiService,
    private translate: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: TaskModalData
  ) {
    this.dialogData = data;
    this.boardId = data.boardId;

    this.dialogTitle = data.mode === 'create' ? this.translate.instant('TASK_MODAL.CREATE_TITLE') :
      data.mode === 'edit' ? this.translate.instant('TASK_MODAL.EDIT_TITLE') : this.translate.instant('TASK_MODAL.PREVIEW_TITLE');
    this.submitButtonText = data.mode === 'create' ? this.translate.instant('TASK_MODAL.CREATE_BUTTON') : this.translate.instant('TASK_MODAL.SAVE_BUTTON');
  }

  ngOnInit(): void {
    if (this.dialogData == null) {
      this.toastr.error(this.translate.instant('ERRORS.UNEXPECTED_ERROR'));
      return;
    }

    this.loadBoardData().pipe(
      switchMap(() => {
        this.createForm();
        this.setupUserFiltering();

        if ((this.dialogData?.mode === 'edit' || this.dialogData?.mode === 'preview') && this.boardId && this.data.task?.id) {
          return this.boardItemService.getBoardItemById(this.boardId, this.data.task.id);
        }

        return of(null);
      }),

      tap(task => {
        if (task) {
          this.taskDetails = task;
          this.initFormData(task);
        }
      })
    ).subscribe({
      error: (err) => console.error('Error on component initialization', err)
    });
  }

  ngAfterViewInit(): void {
    // This ensures the trigger is available
  }

  private setupUserFiltering(): void {
    this.filteredUsers$ = this.taskForm.get('assigneeEmail')!.valueChanges.pipe(
      startWith(''),
      map(value => typeof value === 'string' ? value : value?.email || ''),
      map(email => this.filterUsers(email))
    );
  }

  private filterUsers(email: string): UserLookupDTO[] {
    if (!email) {
      return this.boardUsers;
    }
    const filterValue = email.toLowerCase();
    return this.boardUsers.filter(user =>
      user.email.toLowerCase().includes(filterValue)
    );
  }

  private loadBoardData(): Observable<any> {
    if (this.boardId) {
      return this.boardService.getBoardById(this.boardId).pipe(
        tap(boardDetails => {
          this.boardUsers = boardDetails.boardUsers || [];
        })
      );
    }
    return of(null);
  }

  public get f(){
    return this.taskForm.controls;
  }

  private createForm(): Observable<FormGroup> {
    this.taskForm = this.fb.group({
      title: [undefined, [Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
      description: [undefined, [Validators.required]],
      priority: [TaskPriority.MEDIUM, Validators.required],
      assigneeEmail: [undefined, [this.assigneeValidator]],
      dueDate: [null],
      tags: [undefined],
      taskType: [TaskType.USER_STORY, Validators.required],
      boardColumnId: [null],
    });

    if (this.data.mode == 'preview') {
      this.taskForm.disable();
    }

    return of(this.taskForm);
  }

  public initFormData(taskData: BoardItemDetailsDTO | null): void {
    if (taskData) {
      const assigneeEmail = this.boardUsers.find(u => u.email === taskData.assigneeEmail);

      this.taskForm.patchValue({
        title: taskData.title,
        description: taskData.description,
        priority: taskData.priority,
        dueDate: taskData.dueDate,
        tags: taskData.tags?.map(t => t.title).join(', ') || '',
        taskType: taskData.taskType,
        assigneeEmail: assigneeEmail || null,
        boardColumnId: taskData.boardColumnId
      });
    }
  }

  displayUserEmail(user: UserLookupDTO): string {
    return user && user.email ? user.email : '';
  }

  clearAssignee(): void {
    this.taskForm.controls['assigneeEmail'].setValue(null);
    this.taskForm.controls['assigneeEmail'].markAsTouched();
    this.taskForm.controls['assigneeEmail'].updateValueAndValidity();
    this.taskForm.markAsDirty();
  }

  get hasAssigneeValue(): boolean {
    const value = this.taskForm.controls['assigneeEmail'].value;
    return value && (typeof value === 'string' ? value.trim() !== '' : value.email);
  }

  onAssigneeSelectionChange(event: any): void {
    const selectedValue = event.option?.value;
    if (selectedValue && typeof selectedValue === 'object' && selectedValue.email) {
      this.taskForm.controls['assigneeEmail'].setValue(selectedValue);
    }
  }


  private assigneeValidator = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value || (typeof value === 'string' && value.trim() === '')) {
      return null;
    }

    if (typeof value === 'object' && value?.email) {
      const userExists = this.boardUsers.some(u => u.email === value.email);
      return userExists ? null : { invalidUser: true };
    }

    if (typeof value === 'string') {
      const userExists = this.boardUsers.some(u => u.email.toLowerCase() === value.toLowerCase());
      return userExists ? null : { invalidUser: true };
    }

    return { invalidUser: true };
  };

  onSubmit(): void {

    this.markAllFieldsAsTouched();

    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;

      const assigneeValue = formValue.assigneeEmail;

      const assigneeEmailString = (assigneeValue?.email || assigneeValue) ?? null;
      const tagTitles = formValue.tags
        ? formValue.tags.split(',')
            .map((tag: string) => tag.trim())
            .filter((tag: string) => tag)
        : [];

        const tagsForDto: TagDTO[] = tagTitles.map((title: string) => ({
          title: title
        }));

      const taskData: Partial<BoardItemDetailsDTO> = {
        title: formValue.title,
        description: formValue.description,
        boardColumnId: formValue.boardColumnId,
        priority: formValue.priority,
        taskType: formValue.taskType,
        assigneeEmail: assigneeEmailString,
        dueDate: formValue.dueDate ?? null,
        tags: tagsForDto
      };

      if (this.taskDetails) {

        const updatedTask: BoardItemDetailsDTO = {
          ...this.taskDetails,
          ...taskData,
        };
        this.dialogRef.close(updatedTask);
      } else {

        const newTask = {
          ...taskData,
          createdAt: new Date(),
        };
        this.dialogRef.close(newTask as BoardItemDetailsDTO);
      }
    } else {

      this.showValidationErrors();
    }
  }


  private markAllFieldsAsTouched(): void {
    Object.keys(this.taskForm.controls).forEach(key => {
      const control = this.taskForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }


  private showValidationErrors(): void {
    const errors: string[] = [];

    Object.keys(this.taskForm.controls).forEach(key => {
      const control = this.taskForm.get(key);
      if (control && control.errors && control.touched) {
        const fieldName = this.getFieldDisplayName(key);
        const fieldErrors = this.getFieldErrors(control.errors);
        errors.push(`${fieldName}: ${fieldErrors.join(', ')}`);
      }
    });

    if (errors.length > 0) {
      this.toastr.error(errors.join('\n'), this.translate.instant('ERRORS.VALIDATION_TITLE'));
    }
  }


  private getFieldDisplayName(fieldName: string): string {
    const fieldNames: { [key: string]: string } = {
      'title': this.translate.instant('TASK_MODAL.TITLE_PLACEHOLDER'),
      'description': this.translate.instant('TASK_MODAL.DESCRIPTION'),
      'assignee': this.translate.instant('TASK_MODAL.ASSIGNEE'),
      'priority': this.translate.instant('TASK_MODAL.PRIORITY'),
      'dueDate': this.translate.instant('TASK_MODAL.DUE_DATE'),
      'tags': this.translate.instant('TASK_MODAL.TAGS'),
      'taskType': this.translate.instant('TASK_MODAL.TYPE')
    };

    return fieldNames[fieldName] || fieldName;
  }


  private getFieldErrors(errors: any): string[] {
    const errorMessages: string[] = [];

    if (errors['required']) {
      errorMessages.push(this.translate.instant('ERRORS.FIELD_REQUIRED'));
    }

    if (errors['minlength']) {
      errorMessages.push(this.translate.instant('ERRORS.FIELD_MIN_LENGTH', { min: errors['minlength'].requiredLength }));
    }

    if (errors['maxlength']) {
      errorMessages.push(this.translate.instant('ERRORS.FIELD_MAX_LENGTH', { max: errors['maxlength'].requiredLength }));
    }

    if (errors['invalidUser']) {
      errorMessages.push(this.translate.instant('TASK_MODAL.ASSIGNEE_INVALID'));
    }

    return errorMessages;
  }

  public onClose(): void {
    this.dialogRef.close();
  }

  public getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: this.translate.instant('TASK_MODAL.PRIORITY_LOW'),
      [TaskPriority.MEDIUM]: this.translate.instant('TASK_MODAL.PRIORITY_MEDIUM'),
      [TaskPriority.HIGH]: this.translate.instant('TASK_MODAL.PRIORITY_HIGH'),
      [TaskPriority.URGENT]: this.translate.instant('TASK_MODAL.PRIORITY_URGENT')
    };
    return labels[priority] || priority.toString();
  }

  public getTaskTypeLabel(taskType: TaskType): string {
    const labels = {
      [TaskType.BUG]: this.translate.instant('TASK_MODAL.TYPE_BUG'),
      [TaskType.HOT_FIX]: this.translate.instant('TASK_MODAL.TYPE_HOT_FIX'),
      [TaskType.USER_STORY]: this.translate.instant('TASK_MODAL.TYPE_USER_STORY')
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

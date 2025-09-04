import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { AddBoardDTO, Task, TaskPriority } from 'src/app/models';

export interface CreateBoardModalData {
  mode: 'create';
}

@Component({
    selector: 'app-create-board-modal',
    templateUrl: './create-board-modal.component.html',
    styleUrls: ['./create-board-modal.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatChipsModule
    ]
})
export class CreateBoardModalComponent implements OnInit {
  boardForm!: FormGroup;
  // taskStatuses = Object.values(TaskStatus);
  taskPriorities = Object.values(TaskPriority);
  dialogTitle = 'Создать новую доску';
  submitButtonText = 'Создать';

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateBoardModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateBoardModalData
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.boardForm = this.fb.group({
      boardTitle: ['', Validators.required],
      boardDescription: ['', Validators.required],
      boardUsers: [[] as string[], Validators.required],
      boardmanagers: [[] as string[], Validators.required],
      boardColumns: new FormArray([])
    });
  }

  public addColumn(): void {
    const columnForm = this.fb.group({
      columnTitle: ['', Validators.required],
      columnDescription: ['', Validators.required],
    });

    (this.boardForm.controls['boardColumns'] as FormArray).push(columnForm);
  }

  public removeColumn(index: number): void {
    (this.boardForm.controls['boardColumns'] as FormArray).removeAt(index);
  }

  onSubmit(): void {
    if (this.boardForm.valid) {
      const formValue = this.boardForm.value;
      const users = formValue.boardUsers ?
        formValue.boardUsers.split(',').map((user: string) => user.trim()).filter((user: string) => user) : [];

      const admins = formValue.boardmanagers ?
        formValue.boardmanagers.split(',').map((admin: string) => admin.trim()).filter((admin: string) => admin) : [];

      const boardData: AddBoardDTO = {
        title: formValue.boardTitle,
        description: formValue.boardDescription,
        users: users,
        admins: admins,
        owners: []
      };

      this.dialogRef.close(boardData);
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }

  get boardColumnsArray() {
    return this.boardForm.get('boardColumns') as FormArray;
  }
}

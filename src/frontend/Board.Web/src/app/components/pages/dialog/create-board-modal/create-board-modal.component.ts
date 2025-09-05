import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { AddBoardDTO } from 'src/app/models';
import { UserLookupDTO } from 'src/app/models/user/user-lookup-DTO.model';
import { UserSelectorComponent } from 'src/app/components/shared/user-selector/user-selector.component';

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
        MatChipsModule,
        UserSelectorComponent
    ]
})
export class CreateBoardModalComponent implements OnInit {
  boardForm!: FormGroup;
  dialogTitle = 'Создать новую доску';
  submitButtonText = 'Создать';

  selectedUsers: UserLookupDTO[] = [];
  selectedAdmins: UserLookupDTO[] = [];

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

  onUsersChange(users: UserLookupDTO[]): void {
    this.selectedUsers = users;
  }

  onAdminsChange(admins: UserLookupDTO[]): void {
    this.selectedAdmins = admins;
  }

  onSubmit(): void {
    if (this.boardForm.valid && this.selectedUsers.length > 0 && this.selectedAdmins.length > 0) {
      const formValue = this.boardForm.value;

      const boardData: AddBoardDTO = {
        title: formValue.boardTitle,
        description: formValue.boardDescription,
        users: this.selectedUsers.map(user => user.email),
        admins: this.selectedAdmins.map(admin => admin.email),
        owners: [] // Пока пустой массив
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

  get isFormValid(): boolean {
    return this.boardForm.valid &&
           this.selectedUsers.length > 0 &&
           this.selectedAdmins.length > 0;
  }
}

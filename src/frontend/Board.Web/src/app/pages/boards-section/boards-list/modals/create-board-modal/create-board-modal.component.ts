import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { UserSelectorComponent } from 'src/app/components/shared/user-selector/user-selector.component';
import { BoardDetailsDTO, UpdateBoardDTO, BoardColumnDTO, AddBoardDTO } from 'src/app/core/models';
import { UserLookupDTO } from 'src/app/core/models/user/user-lookup-DTO.model';

export interface CreateBoardModalData {
  mode: 'create' | 'edit';
  board?: BoardDetailsDTO;
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
  dialogTitle = 'Create New Board';
  submitButtonText = 'Create';

  selectedUsers: UserLookupDTO[] = [];
  selectedAdmins: UserLookupDTO[] = [];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateBoardModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateBoardModalData
  ) { }

  ngOnInit(): void {
    this.initForm();

    if (this.data?.mode === 'edit' && this.data.board) {
      this.dialogTitle = 'Edit Board';
      this.submitButtonText = 'Save';

      this.boardForm.patchValue({
        boardTitle: this.data.board.title,
        boardDescription: this.data.board.description
      });

      // Preselect users/admins by role based on boardUsers
      const boardUsers = this.data.board.boardUsers ?? [];
      this.selectedUsers = boardUsers.filter(u => u.role === 7).map(u => ({ id: u.email, email: u.email }));
      this.selectedAdmins = boardUsers.filter(u => u.role === 127).map(u => ({ id: u.email, email: u.email }));

      // Prefill columns with existing values
      const arr = this.boardForm.get('boardColumns') as FormArray;
      arr.clear();
      (this.data.board.boardColumns ?? []).forEach(c => {
        arr.push(this.fb.group({
          id: [c.id || ''],
          columnTitle: [c.title, Validators.required],
          columnDescription: [c.description, Validators.required]
        }));
      });
    } else {
      ["TODO", "In Progress", "Ready for Review", "Done"].forEach(column => {
        const columnForm = this.fb.group({
          columnTitle: [column, Validators.required],
          columnDescription: ['', Validators.required],
        });

        (this.boardForm.controls['boardColumns'] as FormArray).push(columnForm);
      })
    }
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
    if (this.boardForm.valid) {
      const formValue = this.boardForm.value;

      if (this.data?.mode === 'edit' && this.data.board) {
        const boardColumns = formValue.boardColumns.map((column: any) => ({
          id: column.id,
          title: column.columnTitle,
          description: column.columnDescription
        }));

        const updateData: UpdateBoardDTO = {
          id: this.data.board.id,
          title: formValue.boardTitle,
          description: formValue.boardDescription,
          boardUsers: [
            ...this.selectedUsers.map(u => ({ email: u.email, role: 7 })),
            ...this.selectedAdmins.map(a => ({ email: a.email, role: 127 }))
          ],
          boardColumns: boardColumns
        };
        this.dialogRef.close(updateData);
        return;
      }

      const boardColumns: any[] = formValue.boardColumns.map((column: any) => ({
        id: column.id,
        title: column.columnTitle,
        description: column.columnDescription
      }));

      const boardData: AddBoardDTO = {
        title: formValue.boardTitle,
        description: formValue.boardDescription,
        boardUsers: [
          ...this.selectedUsers.map(u => ({ email: u.email, role: 7 })),
          ...this.selectedAdmins.map(a => ({ email: a.email, role: 127 }))
        ],
        boardColumns: boardColumns
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

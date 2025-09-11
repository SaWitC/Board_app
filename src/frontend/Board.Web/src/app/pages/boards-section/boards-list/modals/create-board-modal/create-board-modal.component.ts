import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { BoardDetailsDTO, UpdateBoardDTO, AddBoardDTO } from 'src/app/core/models';
import { UserAccess } from 'src/app/core/models/enums/user-access.enum';
import { UserService } from 'src/app/core/services/auth/user.service';
import { ToastrService } from 'ngx-toastr';

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
  ]
})
export class CreateBoardModalComponent implements OnInit {
  boardForm!: FormGroup;
  dialogTitle = 'Create New Board';
  submitButtonText = 'Create';

  readonly separatorKeysCodes = [13] as const;

  selectedUsers: string[] = [];
  selectedAdmins: string[] = [];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateBoardModalComponent>,
    private userService: UserService,
    private toastr: ToastrService,
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
      boardColumns: new FormArray([]),
      boardUser: ['',[Validators.email]],
      boardAdmin: ['',[Validators.email]]
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
    if(!this.isAllUserEmailsValid()){
      return;
    }
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
          boardUsers:[
            ...this.selectedUsers.map(u => ({ email: u, role: UserAccess.USER })),
            ...this.selectedAdmins.map(a => ({ email: a, role: UserAccess.ADMIN }))
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
        boardUsers:[
          ...this.selectedUsers.map(u => ({ email: u, role: UserAccess.USER })),
          ...this.selectedAdmins.map(a => ({ email: a, role: UserAccess.ADMIN }))
        ,{ email: this.userService.getCurrentUserEmail(), role: UserAccess.OWNER }
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


  private normalize(email: string): string {
    return (email || '').trim().toLowerCase();
  }

  private isAllUserEmailsValid(): boolean {
    const current = this.normalize(this.userService.getCurrentUserEmail());

    const admins = this.selectedAdmins.map(this.normalize).filter(Boolean);
    const users  = this.selectedUsers.map(this.normalize).filter(Boolean);

    if (admins.includes(current) || users.includes(current)) {
      this.toastr.error('You cannot add yourself as an admin or user');
      return false;
    }

    const hasDuplicates = (arr: string[]) => new Set(arr).size !== arr.length;
    if (hasDuplicates(admins) || hasDuplicates(users)) {
      this.toastr.error('You cannot add duplicate emails as an admin or user');
      return false;
    }

    const adminSet = new Set(admins);
    for (const u of users) {
      this.toastr.error('You cannot add the same email as an admin and user');
      if (adminSet.has(u)) return false;
    }

    return true;
  }

  addUser(): void {
    if(this.boardForm.get('boardUser')?.valid) {
      let value = this.boardForm.get('boardUser')?.value;
      if (value) {
        this.selectedUsers.push(value);
      }

      this.boardForm.get('boardUser')?.reset();
    }
  }

  removeUser(user: string): void {
    this.selectedUsers = this.selectedUsers.filter(u => u !== user);
  }


  addAdmin(): void {
    if(this.boardForm.get('boardAdmin')?.valid) {
      let value = this.boardForm.get('boardAdmin')?.value;
      if (value) {
        this.selectedAdmins.push(value);
      }

      this.boardForm.get('boardAdmin')?.reset();
    }
  }

  removeAdmin(admin: string): void {
    this.selectedAdmins = this.selectedAdmins.filter(a => a !== admin);
  }
}

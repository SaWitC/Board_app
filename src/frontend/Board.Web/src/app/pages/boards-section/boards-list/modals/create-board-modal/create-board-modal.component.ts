import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { BoardDetailsDTO, UpdateBoardDTO, AddBoardDTO } from 'src/app/core/models';
import { UserAccess } from 'src/app/core/models/enums/user-access.enum';
import { UserService } from 'src/app/core/services/auth/user.service';
import { ToastrService } from 'ngx-toastr';
import { BoardTemplateServiceApi } from 'src/app/core/services/api-services/board-template-api.service';
import { AddBoardTemplateDTO } from 'src/app/core/models/board-template/add-board-template-DTO.interface';
import { BoardCreationOptions } from 'src/app/core/models/enums/board-creation-options.enum';
import { debounceTime, distinctUntilChanged, filter, Observable, of, Subject, switchMap } from 'rxjs';
import { BoardTemplateDTO } from 'src/app/core/models/board-template/board-template-DTO.interface';
import { MatSelectModule } from '@angular/material/select';
import { NgSelectComponent } from '@ng-select/ng-select';

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
    MatRadioModule,
    MatCheckboxModule,
    MatSelectModule,
    NgSelectComponent
  ]
})
export class CreateBoardModalComponent implements OnInit {
  boardForm!: FormGroup;
  dialogTitle = 'Create New Board';
  submitButtonText = 'Create';

  readonly separatorKeysCodes = [13] as const;
  public boardCreationOptions = BoardCreationOptions;
  public boardTemplates$: Observable<BoardTemplateDTO[]> = of([]);
  searchTerm$ = new Subject<string>();

  selectedUsers: string[] = [];
  selectedAdmins: string[] = [];

  public draggedIndex: number | null = null;
  public hoveredIndex: number | null = null;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateBoardModalComponent>,
    private userService: UserService,
    private boardTemplateServiceApi: BoardTemplateServiceApi,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: CreateBoardModalData
  ) { }

  public isNew(): boolean {
    return this.data.mode === 'create';
  }

  ngOnInit(): void {
    this.initForm();
    this.initSearchBoardsSubscription();

    if (this.data?.mode === 'edit' && this.data.board) {
      this.dialogTitle = 'Edit Board';
      this.submitButtonText = 'Save';

      this.boardForm.patchValue({
        boardTitle: this.data.board.title,
        boardDescription: this.data.board.description
      });


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
      boardAdmin: ['',[Validators.email]],

      boardTemplateId: [null],
      boardType: [BoardCreationOptions.EMPTY, Validators.required]
    });
  }


  onSubmit(): void {
    if(!this.isAllUserEmailsValid()){
      return;
    }

    //Template board
    if(this.boardForm.get('boardType')?.value == BoardCreationOptions.TEMPLATE){
      if(!this.boardForm.get('boardTemplateId')?.value){
        this.toastr.error('Template is required');

        //TODO add logic to create board based on template
      }
    }
    else { //Empty board
      if(!this.boardForm.get('boardColumns')?.value) {
        this.toastr.error('Columns are required');
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
  }

  onClose(): void {
    this.dialogRef.close();
  }

  get boardColumnsArray() {
    return this.boardForm.get('boardColumns') as FormArray;
  }

  //Form validation
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

  //Template configuration
  createTemplatebasedOnThisBoard(): void {
    if(!this.data.board?.IsTemplate){
      const dto: AddBoardTemplateDTO = { title: this.data.board?.title + ' Template', description: this.data.board?.description ?? '', isActive: true, boardId: this.data.board?.id ?? ''};
      this.boardTemplateServiceApi.addBoardtTmplate(dto).subscribe();
    }
  }

  onChangeTemplateActive(event: any): void {
    this.boardTemplateServiceApi.updateBoardTemplate(this.data.board?.id ?? '', event.checked).subscribe();
  }

  //Users and admins configuration
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

  //Empty board columns configuration
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

  //Board templates configuration
  public initSearchBoardsSubscription(){
    this.boardTemplates$ = this.searchTerm$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(term =>
        term.length > 2
          ? this.boardTemplateServiceApi.searchActiveBoardsTemplates(term)
          : of([])
      )
    );
  }

  onSearch(event: { term: string; items: any[] }) {
    this.searchTerm$.next(event.term);
  }

  onDragStart(event: DragEvent, index: number): void {
    if (event.dataTransfer) {
      event.dataTransfer.effectAllowed = 'move';
      event.dataTransfer.setData('text/plain', index.toString());
      this.draggedIndex = index;
    }
  }

  onDragEnd(event: DragEvent): void {
    this.draggedIndex = null;
    this.hoveredIndex = null;
  }

  onDragOver(event: DragEvent, index: number): void {
    event.preventDefault();

    this.hoveredIndex = index;

    if (event.dataTransfer) {
      event.dataTransfer.dropEffect = 'move';
    }

    if (this.draggedIndex === null || this.draggedIndex === index) {
      return;
    }

    const draggedItem = this.boardColumnsArray.at(this.draggedIndex);
    
    this.boardColumnsArray.removeAt(this.draggedIndex);
    this.boardColumnsArray.insert(index, draggedItem);

    this.onDragStart(event, index);
  }

  onDragEnter(event: DragEvent): void {
    event.preventDefault();
  }

  onDrop(event: DragEvent, dropIndex: number): void {
    event.preventDefault();

    if (this.draggedIndex === null || this.draggedIndex === dropIndex) {
      return;
    }

    const draggedItem = this.boardColumnsArray.at(this.draggedIndex);
    
    this.boardColumnsArray.removeAt(this.draggedIndex);
    this.boardColumnsArray.insert(dropIndex, draggedItem);

    this.draggedIndex = null;
    this.hoveredIndex = null;
  }
}

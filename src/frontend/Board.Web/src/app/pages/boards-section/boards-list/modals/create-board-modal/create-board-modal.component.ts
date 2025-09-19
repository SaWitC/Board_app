import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
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
import { debounceTime, distinctUntilChanged, Observable, of, Subject, switchMap, catchError } from 'rxjs';
import { BoardTemplateDTO } from 'src/app/core/models/board-template/board-template-DTO.interface';
import { MatSelectModule } from '@angular/material/select';
import { NgSelectComponent } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BoardApiService } from 'src/app/core/services/api-services';
import { BoardLookupDTO } from 'src/app/core/models/board/board-lookup-DTO.interface';

export interface CreateBoardModalData {
  mode: 'create' | 'edit';
  board?: BoardDetailsDTO;
}

export interface BoardModalResult {
  success: boolean;
  boards?: BoardLookupDTO[];
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
    NgSelectComponent,
    TranslateModule
  ]
})
export class CreateBoardModalComponent implements OnInit {
  private static saveButtonTextLocalizationKey: string = 'BOARD_EDIT.CREATE';
  private static editButtonTextLocalizationKey: string = 'BOARD_EDIT.EDIT';
  
  boardForm!: FormGroup;
  dialogTitle = 'Create New Board';
  submitButtonTextLocalizationKey = CreateBoardModalComponent.saveButtonTextLocalizationKey;

  readonly separatorKeysCodes = [13] as const;
  public boardCreationOptions = BoardCreationOptions;
  public boardTemplates$: Observable<BoardTemplateDTO[]> = of([]);
  searchTerm$ = new Subject<string>();

  public isBoardManager: boolean = false;
  public isBoardOwner: boolean = false;

  public draggedIndex: number | null = null;
  public hoveredIndex: number | null = null;

  public isSubmitting = false; // Add loading state

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateBoardModalComponent>,
    private userService: UserService,
    private boardTemplateServiceApi: BoardTemplateServiceApi,
    private toastr: ToastrService,
    private boardApiService: BoardApiService,
    @Inject(MAT_DIALOG_DATA) public data: CreateBoardModalData
  ) { }

  public isNew(): boolean {
    return this.data.mode === 'create';
  }

  ngOnInit(): void {
    this.initForm();
    this.initSearchBoardsSubscription();

    //Edit Mode
    if (this.data?.mode === 'edit' && this.data.board) {

      this.isBoardManager = this.userService.isUserBoardAdmin();
      this.isBoardOwner = this.userService.isUserBoardOwner();

      this.updateBoardFieldsConfiguration();

      this.dialogTitle = 'Edit Board';
      this.submitButtonTextLocalizationKey = CreateBoardModalComponent.editButtonTextLocalizationKey;

      this.boardForm.patchValue({
        boardTitle: this.data.board.title,
        boardDescription: this.data.board.description
      });

      this.setUsers();

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

  private updateBoardFieldsConfiguration(){
    if(this.isBoardOwner){
      this.boardForm.get('boardTitle')?.enable();
      this.boardForm.get('boardDescription')?.enable();
    }
    else{
      this.boardForm.get('boardTitle')?.disable();
      this.boardForm.get('boardDescription')?.disable();
    }
  }

  private initForm(): void {
    this.boardForm = this.fb.group({
      boardTitle: ['', Validators.required],
      boardDescription: ['', Validators.required],
      boardColumns: new FormArray([]),
      boardUser: ['',[]],
      boardAdmin: ['',[]],

      boardAdmins: new FormArray([]),
      boardUsers: new FormArray([]),

      boardTemplateId: [null],
      boardType: [BoardCreationOptions.EMPTY, Validators.required]
    });
  }

  private setUsers(): void {
    if (this.data.board === undefined) {
      return;
    }

    const adminEmails = this.data.board?.boardUsers.filter(x => x.role === UserAccess.ADMIN).map(x => x.email);
    const userEmails = this.data.board?.boardUsers.filter(x => x.role === UserAccess.USER).map(x => x.email);

    for (let adminEmail of adminEmails) {
      this.addAdmin(adminEmail);
    }

    for (let userEmail of userEmails) {
      this.addUser(userEmail);
    }
  }


  onSubmit(): void {
    if (this.isSubmitting) return;
    
    if (!this.isAllUserEmailsValid()) {
      return;
    }

    //Template board
    if (this.boardForm.get('boardType')?.value == BoardCreationOptions.TEMPLATE) {
      if (!this.boardForm.get('boardTemplateId')?.value) {
        this.toastr.error('Template is required');
        //TODO add logic to create board based on template
        return;
      }
    }
    else if (!this.boardForm.get('boardColumns')?.value) {
      this.toastr.error('Columns are required');
      return;
    }
    else if (this.boardForm.valid) {
      this.isSubmitting = true;

      if (this.data?.mode === 'edit' && this.data.board) {
        const updateData = this.getUpdateBoardDTO(this.boardForm);
        this.boardApiService.updateBoard(updateData).pipe(
          switchMap(() => this.boardApiService.getBoards())
        ).subscribe({
          next: (boards) => {
            this.dialogRef.close({ success: true, boards: boards });
          },
          error: (error) => {
            this.isSubmitting = false;
          }
        });
        return;
      }

      const boardData = this.getAddBoardDTO(this.boardForm);
      this.boardApiService.addBoard(boardData).pipe(
        switchMap(() => this.boardApiService.getBoards())
      ).subscribe({
        next: (boards) => {
          this.dialogRef.close({ success: true, boards: boards });
        },
        error: (error) => {
          this.isSubmitting = false;
        }
      });
    }
  }

  private getUpdateBoardDTO(boardForm: FormGroup<any>): UpdateBoardDTO {
    const formValue = boardForm.value;

    const boardColumns = formValue.boardColumns.map((column: any) => ({
      id: column.id,
      title: column.columnTitle,
      description: column.columnDescription
    }));

    const adminEmails = this.adminEmails;
    const userEmails = this.userEmails;

    const updateData: UpdateBoardDTO = {
      id: this.data.board!.id,
      title: formValue.boardTitle,
      description: formValue.boardDescription,
      boardUsers:[
        ...userEmails.map(u => ({ email: u, role: UserAccess.USER })),
        ...adminEmails.map(a => ({ email: a, role: UserAccess.ADMIN }))
      ],
      boardColumns: boardColumns
    };

    return updateData;
  }

  private getAddBoardDTO(boardForm: FormGroup<any>): AddBoardDTO {
    const formValue = boardForm.value;

    const boardColumns: any[] = formValue.boardColumns.map((column: any) => ({
      id: column.id,
      title: column.columnTitle,
      description: column.columnDescription
    }));

    const adminEmails = this.adminEmails;
    const userEmails = this.userEmails;

    const boardData: AddBoardDTO = {
      title: formValue.boardTitle,
      description: formValue.boardDescription,
      boardUsers:[
        ...userEmails.map(u => ({ email: u, role: UserAccess.USER })),
        ...adminEmails.map(a => ({ email: a, role: UserAccess.ADMIN }))
      ],
      boardColumns: boardColumns
    };

    return boardData;
  }

  onClose(): void {
    this.dialogRef.close();
  }

  get boardColumnsArray() {
    return this.boardForm.get('boardColumns') as FormArray;
  }

  get boardAdmins() {
    return this.boardForm.get('boardAdmins') as FormArray;
  }

  get adminEmails() {
    return this.boardAdmins.controls.map(x => x.get('adminEmail')?.value as string).map(this.normalize).filter(Boolean);
  }

  get userEmails() {
    return this.boardUsers.controls.map(x => x.get('userEmail')?.value as string).map(this.normalize).filter(Boolean);
  }

  get boardUsers() {
    return this.boardForm.get('boardUsers') as FormArray;
  }

  get isFormValid(): boolean {
    return this.boardForm.valid
  }

  private normalize(email: string): string {
    return (email || '').trim().toLowerCase();
  }

  private isAllUserEmailsValid(): boolean {
    const current = this.normalize(this.userService.getCurrentUserEmail());

    const adminEmails = this.adminEmails;
    const userEmails = this.userEmails;

    if (adminEmails.includes(current) || userEmails.includes(current)) {
      this.toastr.error('You cannot add yourself as an admin or user');
      return false;
    }

    const hasDuplicates = (arr: string[]) => new Set(arr).size !== arr.length;
    if (hasDuplicates(adminEmails) || hasDuplicates(userEmails)) {
      this.toastr.error('You cannot add duplicate emails as an admin or user');
      return false;
    }

    const adminSet = new Set(adminEmails);
    for (const u of userEmails) {
      if (adminSet.has(u)) {
        this.toastr.error('You cannot add the same email as an admin and user');
        return false;
      }
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

  addUser(email: string = ''): void {
    const userForm = this.fb.group({
      userEmail: [email, [Validators.email, Validators.required]]
    });

    this.boardUsers.push(userForm);
  }

  removeUser(index: number): void {
    this.boardUsers.removeAt(index);
  }


  addAdmin(email: string = ''): void {
    const adminForm = this.fb.group({
      adminEmail: [email, [Validators.email, Validators.required]]
    });

    this.boardAdmins.push(adminForm);
  }

  removeAdmin(index: number): void {
    this.boardAdmins.removeAt(index);
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

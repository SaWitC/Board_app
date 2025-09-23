import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule, FormControl, FormGroup } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';
import { BoardLookupDTO, BoardDetailsDTO } from 'src/app/core/models';
import { BoardApiService } from 'src/app/core/services/api-services';
import { DialogService } from 'src/app/core/services/other/dialog.service';
import { Observable, tap, debounceTime, distinctUntilChanged, switchMap, catchError, of } from 'rxjs';
import { UserService } from 'src/app/core/services/auth/user.service';
import { UserAccess } from 'src/app/core/models/enums/user-access.enum';
import { BoardModalResult } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';
import { GetBoardsRequest } from "../../../core/models/board/get-boards-request.interface";
import { PagedResult } from "../../../core/models/common/paged-result.interface";
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-boards-list',
  standalone: true,
  imports: [
    CommonModule,
    MatMenuModule,
    TranslateModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './boards-list.component.html',
  styleUrl: './boards-list.component.scss'
})
export class BoardsListComponent implements OnInit {
  boards: BoardLookupDTO[] = [];
  selectedBoard: BoardLookupDTO | null = null;
  public isGlobalAdmin: boolean = false;
  totalCount: number = 0;
  pageSize: number = 12;
  pageIndex: number = 0;
  isSearchExpanded: boolean = false;

  @ViewChild('searchSection', { static: false }) searchSection!: ElementRef;

  searchForm = new FormGroup({
    titleSearch: new FormControl(''),
    ownerSearch: new FormControl('')
  });

  get titleSearchControl() {
    return this.searchForm.get('titleSearch') as FormControl;
  }

  get ownerSearchControl() {
    return this.searchForm.get('ownerSearch') as FormControl;
  }

  getUsersCount(board: BoardLookupDTO): number {
    return (board.boardUsers ?? []).filter(u => u.role === 7).length;
  }

  getAdminsCount(board: BoardLookupDTO): number {
    return (board.boardUsers ?? []).filter(u => u.role === 127).length;
  }

  constructor(
    private boardApiService: BoardApiService,
    private router: Router,
    private dialogService: DialogService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.isGlobalAdmin = this.userService.hasGlobalAdminPermission();
    this.loadBoards().subscribe();

    this.searchForm.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(() => {
        this.pageIndex = 0;
        this.loadBoards().subscribe();
      })
    ).subscribe();
  }

  toggleSearch(): void {
    this.isSearchExpanded = !this.isSearchExpanded;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (this.isSearchExpanded) {
      const searchButton = (event.target as Element).closest('.search-toggle-btn');
      if (searchButton) {
        return;
      }

      if (this.searchSection) {
        const clickedInside = this.searchSection.nativeElement.contains(event.target as Node);
        if (!clickedInside) {
          this.isSearchExpanded = false;
        }
      }
    }
  }

  loadBoards(): Observable<PagedResult<BoardLookupDTO>> {

    const formValue = this.searchForm.value;
    const request: GetBoardsRequest = {
      page: this.pageIndex + 1,
      pageSize: this.pageSize,
      titleSearchTerm: formValue.titleSearch || null,
      ownerSearchTerm: formValue.ownerSearch || null
    };

    return this.boardApiService.getBoards(request).pipe(
      tap(result => {
        this.boards = result.items;
        this.totalCount = result.totalCount;
        this.pageIndex = result.pageNumber - 1;
        this.pageSize = result.pageSize;
      }),
      catchError(error => {
        return of({
          items: [],
          totalCount: 0,
          pageNumber: 1,
          pageSize: 12,
          totalPages: 0,
          hasPreviousPage: false,
          hasNextPage: false
        });
      })
    );
  }

  handlePageEvent(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadBoards().subscribe();
  }

  onBoardClick(board: BoardLookupDTO): void {
    this.router.navigate(['/board', board.id]);
  }

  onCreateBoard(): void {
    this.dialogService.openCreateBoardModal().subscribe((result?: BoardModalResult) => {
      if (result?.success && result?.boards) {
        this.loadBoards().subscribe();
      }
    });
  }

  onDeleteBoard(boardId: string): void {
    this.boardApiService.deleteBoard(boardId).subscribe({
      next: () => {
        this.loadBoards().subscribe();
      },
      error: (err) => {
      }
    });
  }

  onOpenMenu(event: MouseEvent, board: BoardLookupDTO): void {
    event.stopPropagation();
    this.selectedBoard = board;
  }


  private getCurrentUserRole(board: BoardLookupDTO): UserAccess | null {
    const currentEmail = (this.userService.getCurrentUserEmail() || '').toLowerCase();
    const membership = (board.boardUsers ?? []).find(u => (u.email || '').toLowerCase() === currentEmail);
    if (!membership) { return null; }
    switch (membership.role) {
      case UserAccess.USER: return UserAccess.USER;
      case UserAccess.ADMIN: return UserAccess.ADMIN;
      case UserAccess.OWNER: return UserAccess.OWNER;
      default: return null;
    }
  }

  canSeeActions(board: BoardLookupDTO): boolean {
    if (this.isGlobalAdmin) {
      return true;
    }

    const role = this.getCurrentUserRole(board);
    return role === UserAccess.ADMIN || role === UserAccess.OWNER;
  }

  canEdit(board: BoardLookupDTO): boolean {
    if (this.isGlobalAdmin) {
      return true;
    }

    const role = this.getCurrentUserRole(board);
    return role === UserAccess.ADMIN || role === UserAccess.OWNER;
  }

  canDelete(board: BoardLookupDTO): boolean {
    if (this.isGlobalAdmin) {
      return true;
    }

    const role = this.getCurrentUserRole(board);
    return role === UserAccess.OWNER;
  }
}

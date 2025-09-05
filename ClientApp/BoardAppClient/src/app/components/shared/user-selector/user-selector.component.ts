import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { UserLookupDTO } from '../../../models/user/user-lookup-DTO.model';
import { debounceTime, distinctUntilChanged, switchMap, filter, map } from 'rxjs/operators';
import { Observable, Subject, of } from 'rxjs';

@Component({
  selector: 'app-user-selector',
  templateUrl: './user-selector.component.html',
  styleUrls: ['./user-selector.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class UserSelectorComponent implements OnInit, OnDestroy {
  @Input() selectedUsers: UserLookupDTO[] = [];
  @Input() placeholder: string = 'Поиск пользователей...';
  @Input() label: string = 'Пользователи';
  @Input() required: boolean = false;
  @Output() usersChange = new EventEmitter<UserLookupDTO[]>();

  searchControl = new FormControl('');
  filteredUsers: Observable<UserLookupDTO[]> = of([]);
  private searchSubject = new Subject<string>();

  constructor() {}

  ngOnInit(): void {
    this.setupSearch();
  }

  ngOnDestroy(): void {
    this.searchSubject.complete();
  }

  private setupSearch(): void {
    this.filteredUsers = this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      filter(searchTerm => searchTerm.length >= 2),
      switchMap(searchTerm => this.searchUsers(searchTerm))
    );
  }

  private searchUsers(searchTerm: string): Observable<UserLookupDTO[]> {
    // Моковые данные для демонстрации
    const mockUsers: UserLookupDTO[] = [
      { id: '1', email: 'user1@example.com' },
      { id: '2', email: 'user2@example.com' },
      { id: '3', email: 'admin1@example.com' },
      { id: '4', email: 'admin2@example.com' },
      { id: '5', email: 'developer1@example.com' },
      { id: '6', email: 'tester1@example.com' }
    ];

    const filtered = mockUsers.filter(user =>
      user.email.toLowerCase().includes(searchTerm.toLowerCase()) &&
      !this.selectedUsers.some(selected => selected.id === user.id)
    );

    return of(filtered);
  }

  onSearchInput(event: any): void {
    const searchTerm = event.target.value;
    this.searchSubject.next(searchTerm);
  }

  onUserSelect(user: UserLookupDTO): void {
    if (!this.selectedUsers.some(selected => selected.id === user.id)) {
      this.selectedUsers = [...this.selectedUsers, user];
      this.usersChange.emit(this.selectedUsers);
      this.searchControl.setValue('');
    }
  }

  onUserRemove(userId: string): void {
    this.selectedUsers = this.selectedUsers.filter(user => user.id !== userId);
    this.usersChange.emit(this.selectedUsers);
  }

  displayFn = (user: UserLookupDTO): string => {
    return user ? user.email : '';
  }
}

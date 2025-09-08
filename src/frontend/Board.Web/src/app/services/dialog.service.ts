import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TaskModalComponent, TaskModalData } from '../components/pages/dialog/task-modal/task-modal.component';
import { CreateBoardModalComponent, CreateBoardModalData } from '../components/pages/dialog/create-board-modal/create-board-modal.component';
import { Task, AddBoardDTO, BoardDetailsDTO, UpdateBoardDTO } from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openTaskModal(taskModalData: TaskModalData): Observable<Task | undefined> {
    const dialogRef: MatDialogRef<TaskModalComponent> = this.dialog.open(TaskModalComponent, {
      width: '1200px',
      maxWidth: '90vw',
      height: '800px',
      maxHeight: '90vh',
      data: taskModalData,
      disableClose: false,
      autoFocus: true,
      panelClass: 'task-modal-dialog'
    });

    return dialogRef.afterClosed();
  }

  openCreateBoardModal(): Observable<AddBoardDTO | undefined> {
    const dialogRef: MatDialogRef<CreateBoardModalComponent> = this.dialog.open(CreateBoardModalComponent, {
      width: '700px',
      maxWidth: '90vw',
      maxHeight: '90vh',
      data: {
        mode: 'create'
      } as CreateBoardModalData,
      disableClose: false,
      autoFocus: true,
      panelClass: 'create-board-modal-dialog'
    });

    return dialogRef.afterClosed();
  }

  openEditBoardModal(board: BoardDetailsDTO): Observable<UpdateBoardDTO | undefined> {
    const dialogRef: MatDialogRef<CreateBoardModalComponent> = this.dialog.open(CreateBoardModalComponent, {
      width: '700px',
      maxWidth: '90vw',
      maxHeight: '90vh',
      data: {
        mode: 'edit',
        board
      } as CreateBoardModalData,
      disableClose: false,
      autoFocus: true,
      panelClass: 'create-board-modal-dialog'
    });

    return dialogRef.afterClosed();
  }
}

import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BoardItem, AddBoardDTO, BoardDetailsDTO, UpdateBoardDTO } from '../models';
import { Observable } from 'rxjs';
import { TaskModalComponent, TaskModalData } from 'src/app/pages/boards-section/board/modals/task-modal/task-modal.component';
import { CreateBoardModalComponent, CreateBoardModalData } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openTaskModal(taskModalData: TaskModalData): Observable<BoardItem | undefined> {
    const dialogRef: MatDialogRef<TaskModalComponent> = this.dialog.open(TaskModalComponent, {
      width: '1200px',
      maxWidth: '90vw',
      height: '800px',
      maxHeight: '90vh',
      data: taskModalData,
      disableClose: true,
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
      disableClose: true,
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
      disableClose: true,
      autoFocus: true,
      panelClass: 'create-board-modal-dialog'
    });

    return dialogRef.afterClosed();
  }
}

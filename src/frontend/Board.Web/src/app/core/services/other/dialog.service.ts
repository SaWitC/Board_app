import { Injectable, model } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BoardDetailsDTO, BoardItemDetailsDTO, BoardItemLookupDTO } from '../../models';
import { Observable } from 'rxjs';
import { TaskModalComponent, TaskModalData } from 'src/app/pages/boards-section/board/modals/task-modal/task-modal.component';
import { CreateBoardModalComponent, CreateBoardModalData, BoardModalResult } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openTaskModal(taskModalData: TaskModalData): Observable<BoardItemDetailsDTO | undefined> {
    const dialogRef: MatDialogRef<TaskModalComponent> = this.dialog.open(TaskModalComponent, {
      width: '1800px',
      maxWidth: '95vw',
      height: '800px',
      data: taskModalData,
      disableClose: true,
      autoFocus: true,
      panelClass: 'task-modal-dialog'
    });

    return dialogRef.afterClosed();
  }

  openTaskPreviewModal(task:BoardItemLookupDTO, boardId:string): Observable<undefined> {
    const dialogRef: MatDialogRef<TaskModalComponent> = this.dialog.open(TaskModalComponent, {
      width: '1800px',
      maxWidth: '95vw',
      height: '800px',
      data: {
        task:task,
        boardId:boardId,
        mode:'preview'
      },
      disableClose: false,
      autoFocus: true,
      panelClass: 'task-modal-dialog'
    });

    return dialogRef.afterClosed();
  }

  openCreateBoardModal(): Observable<BoardModalResult | undefined> {
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

  openEditBoardModal(board: BoardDetailsDTO): Observable<BoardModalResult | undefined> {
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

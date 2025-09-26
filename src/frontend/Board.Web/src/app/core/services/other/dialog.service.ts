import { Injectable, model } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BoardColumnDetailsDTO, BoardDetailsDTO, BoardItemDetailsDTO, BoardItemLookupDTO } from '../../models';
import { Observable } from 'rxjs';
import { TaskModalComponent, TaskModalData } from 'src/app/pages/boards-section/board/modals/task-modal/task-modal.component';
import { CreateBoardModalComponent, CreateBoardModalData, BoardModalResult } from 'src/app/pages/boards-section/boards-list/modals/create-board-modal/create-board-modal.component';
import { ConfirmationModalComponent, ConfirmationModalData } from 'src/app/components/shared/modal/confirmation-dialog/configmation-modal.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openTaskModal(taskModalData: TaskModalData): Observable<BoardItemDetailsDTO | undefined> {
    const dialogRef: MatDialogRef<TaskModalComponent> = this.dialog.open(TaskModalComponent, {
      width: '1800px',
      maxWidth: '95vw',
      height: '1000px',
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
      height: '1000px',
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

  openEditBoardModal(board: BoardDetailsDTO, boardColumns: BoardColumnDetailsDTO[]): Observable<BoardModalResult | undefined> {
    const dialogRef: MatDialogRef<CreateBoardModalComponent> = this.dialog.open(CreateBoardModalComponent, {
      width: '700px',
      maxWidth: '90vw',
      maxHeight: '90vh',
      data: {
        mode: 'edit',
        board,
        boardColumns
      } as CreateBoardModalData,
      disableClose: true,
      autoFocus: true,
      panelClass: 'create-board-modal-dialog'
    });

    return dialogRef.afterClosed();
  }

  openConfirmationModal(confirmationModalData: ConfirmationModalData): Observable<boolean | undefined> {
    const dialogRef: MatDialogRef<ConfirmationModalComponent> = this.dialog.open(ConfirmationModalComponent, {
      data: confirmationModalData,
      disableClose: true,
      autoFocus: true,
    });

    return dialogRef.afterClosed();
  }
}

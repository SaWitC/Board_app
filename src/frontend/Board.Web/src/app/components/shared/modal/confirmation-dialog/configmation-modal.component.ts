import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';

export interface ConfirmationModalData {
  dialogTitle: string;
  description: string;
}

@Component({
  selector: 'app-configmation-modal',
  templateUrl: './configmation-modal.component.html',
  styleUrls: ['./configmation-modal.component.scss'],
  standalone: true,
  imports: [
    TranslateModule,
    MatDialogModule,
    MatButtonModule,
]
})
export class ConfirmationModalComponent {
  dialogTitle: string;
  description: string;

  constructor(
    public dialogRef: MatDialogRef<ConfirmationModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationModalData
  ) {
    this.dialogTitle = data.dialogTitle;
    this.description = data.description;
  }

  onClose(): void {
    this.dialogRef.close(false);
  }

  onSubmit(): void {
    this.dialogRef.close(true);
  }
}

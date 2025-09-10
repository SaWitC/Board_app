import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TaskType } from 'src/app/core/models/enums/task-type.enum';

@Component({
  selector: 'app-task-type-icon',
  templateUrl: './task-type-icon.component.html',
  standalone: true,
  imports: [
    CommonModule
  ]
})
export class TaskTypeIconComponent {
  @Input() public taskType!: TaskType;
  public taskTypes = TaskType;
}

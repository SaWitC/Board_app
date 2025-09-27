import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { TaskTypeIconComponent } from "src/app/components/shared/story-icon/task-type-icon.component";
import { BoardItemLookupDTO } from "src/app/core/models";
import { TaskPriority } from "src/app/core/models/enums/task-priority.enum";
import { TaskType } from "src/app/core/models/enums/task-type.enum";
import { DialogService } from "src/app/core/services/other/dialog.service";


@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.scss'],
  standalone: true,
  imports: [CommonModule, TaskTypeIconComponent]
})
export class TaskCardComponent {
  @Input() task!: BoardItemLookupDTO;
  @Output() editTask = new EventEmitter<BoardItemLookupDTO>();
  @Output() deleteTask = new EventEmitter<string>();
  @Output() moveTask = new EventEmitter<{ taskId: string, toColumnId: string }>();


  constructor(
    private dialogService: DialogService,
    private translate: TranslateService
  ) {}

  public taskTypes = TaskType;
  public priorities = Object.values(TaskPriority);
  public isDragging = false;

  public taskTypeClassMap: Record<TaskType, string> = {
    [TaskType.BUG]: 'bug',
    [TaskType.HOT_FIX]: 'hot-fix',
    [TaskType.USER_STORY]: 'user-story'
  };

  public taskTypeClassMapPriority: Record<TaskPriority, string> = {
    [TaskPriority.LOW]: 'priority-color-low',
    [TaskPriority.MEDIUM]: 'priority-color-medium',
    [TaskPriority.HIGH]: 'priority-color-high',
    [TaskPriority.URGENT]: 'priority-color-urgent'
  };

  onDragStart(event: DragEvent): void {
    if (event.dataTransfer) {
      event.dataTransfer.setData('text/plain', this.task.id);
      event.dataTransfer.effectAllowed = 'move';
      this.isDragging = true;
    }
  }

  onDragEnd(event: DragEvent): void {
    this.isDragging = false;
  }

  getPriorityLabel(priority: TaskPriority): string {
    const labels = {
      [TaskPriority.LOW]: 'Низкий',
      [TaskPriority.MEDIUM]: 'Средний',
      [TaskPriority.HIGH]: 'Высокий',
      [TaskPriority.URGENT]: 'Срочный'
    };

    return labels[priority] || 'Неизвестно';
  }

  onEdit(event:any): void {
    event.stopPropagation();
    this.editTask.emit(this.task);
  }

  onDelete(event:any): void {
    event.stopPropagation();
    this.dialogService.openConfirmationModal({
      dialogTitle: this.translate.instant('BOARD_ITEM.DELETE_TASK_CONFIRMATION_TITLE'),
      description: this.translate.instant('BOARD_ITEM.DELETE_TASK_CONFIRMATION')
    }).subscribe((result) => {
      if (result) {
        this.deleteTask.emit(this.task.id);
      }
    }); 
  }

  onMoveTask(toColumnId: string): void {
    this.moveTask.emit({ taskId: this.task.id, toColumnId: toColumnId });
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('ru-RU');
  }

  isOverdue(): boolean {
    if (this.task?.dueDate == null) return false;
    return new Date(this.task.dueDate) < new Date();
  }
}

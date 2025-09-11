import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";

export interface BoardItemDetailsDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: TaskPriority;
  assigneeId: string;
  dueDate: string;
  modificationDate?: string;
  createdTime?: string;
  taskType: TaskType;
}
